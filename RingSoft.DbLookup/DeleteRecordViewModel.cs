using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Google.Protobuf.WellKnownTypes;
using Org.BouncyCastle.Utilities;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.Lookup;

namespace RingSoft.DbLookup
{
    public interface IDeleteRecordView
    {
        void CloseWindow(bool result);
    }

    public class DeleteTableWindowData
    {
        public DeleteRecordItemViewModel DeleteRecordItem { get; set; }

        public bool AllowSetNull { get; set; }

        public ListControlDataSourceRow ItemRow { get; set; }

        public bool Processed { get; set; }

        public DeleteTable DeleteTable { get; set; }
    }

    public class DeleteRecordViewModel : INotifyPropertyChanged
    {
        private bool _deleteAllData;

        public bool DeleteAllData
        {
            get => _deleteAllData;
            set
            {
                if (_deleteAllData == value)
                {
                    return;
                }
                _deleteAllData = value;
                //SetAllTabsDelete(value);
                OnPropertyChanged();
            }
        }

        private ListControlSetup _tableSetup;

        public ListControlSetup TableSetup
        {
            get => _tableSetup;
            set
            {
                if (_tableSetup == value)
                    return;

                _tableSetup = value;
                OnPropertyChanged();
            }
        }

        private ListControlDataSource _tableDataSource;

        public ListControlDataSource TableDataSource
        {
            get => _tableDataSource;
            set
            {
                if (_tableDataSource == value)
                    return;

                _tableDataSource = value;
                OnPropertyChanged();
            }
        }

        private ListControlDataSourceRow _selectedDeleteTable;

        public ListControlDataSourceRow SelectedDeleteTable
        {
            get => _selectedDeleteTable;
            set
            {
                if (_selectedDeleteTable == value)
                    return;

                PreSetActiveTable(SelectedDeleteTable);
                _selectedDeleteTable = value;
                SetNewActiveTable(SelectedDeleteTable);
                OnPropertyChanged();
            }
        }

        private LookupDefinitionBase _lookupDefinition;

        public LookupDefinitionBase LookupDefinition
        {
            get => _lookupDefinition;
            set
            {
                if (_lookupDefinition == value)
                    return;

                _lookupDefinition = value;
                OnPropertyChanged();
            }
        }

        private LookupCommand _lookupCommand;

        public LookupCommand LookupCommand
        {
            get => _lookupCommand;
            set
            {
                if (value == _lookupCommand)
                    return;

                _lookupCommand = value;
                OnPropertyChanged();
            }
        }

        private bool _itemProcessed;

        public bool ItemProceessed
        {
            get => _itemProcessed;
            set
            {
                if (_itemProcessed == value)
                    return;

                _itemProcessed = value;
                PreSetActiveTable(SelectedDeleteTable);
                OnPropertyChanged();
            }
        }

        private string _processCaption;

        public string ProcessCaption
        {
            get => _processCaption;
            set
            {
                if (_processCaption == value)
                    return;

                _processCaption = value;
                OnPropertyChanged();
            }
        }


        public IDeleteRecordView View { get; private set; }

        public List<DeleteTableWindowData> Items { get; private set; } = new List<DeleteTableWindowData>();

        public RelayCommand OkCommand { get; set; }

        public RelayCommand CancelCommand { get; set; }
        
        public DeleteTables DeleteTables { get; private set; }
        
        public DeleteRecordViewModel()
        {
            OkCommand = new RelayCommand(() =>
            {
                OnOk();
            });

            CancelCommand = new RelayCommand(() =>
            {
                View.CloseWindow(false);
            });
        }

        public void Initialize(IDeleteRecordView view, DeleteTables deleteTables)
        {
            View = view;
            DeleteTables = deleteTables;
            TableSetup = new ListControlSetup();
            TableDataSource = new ListControlDataSource();
            var dataColumn = TableSetup.AddColumn(1, "Table", FieldDataTypes.String, 55);
            var column2 = TableSetup.AddColumn(2, "Related Item", FieldDataTypes.String, 30);
            var column3 = TableSetup.AddColumn(3, "Processed?", FieldDataTypes.String, 15);

            foreach (var deleteTable in deleteTables.Tables)
            {
                var tableRow = new ListControlDataSourceRow();
                tableRow.AddColumn(dataColumn, deleteTable.ChildField.TableDefinition.Description);
                tableRow.AddColumn(column2, deleteTable.ChildField.Description);
                tableRow.AddColumn(column3, false.ToString());
                TableDataSource.AddRow(tableRow);
                //tableRow.DataCells[2].TextValue = "True";

                var deleteLookupData = new DeleteTableWindowData();
                deleteLookupData.AllowSetNull =
                    deleteTable.ChildField.AllowNulls && deleteTable.ChildField.AllowUserNulls;
                deleteLookupData.DeleteRecordItem = new DeleteRecordItemViewModel();
                deleteLookupData.DeleteRecordItem.Initialize(deleteTable);
                deleteLookupData.ItemRow = tableRow;
                deleteLookupData.DeleteTable = deleteTable;
                deleteTable.Description = deleteTable.ChildField.TableDefinition.Description 
                    + "\r\n" + deleteTable.ChildField.Description;
                Items.Add(deleteLookupData);

            }
            SelectedDeleteTable = TableDataSource.Items[0];
        }

        private void SetTableLookup(ListControlDataSourceRow row)
        {
            var item = Items.FirstOrDefault(p => p.ItemRow == row);
            if (item != null)
            {
                if (item.AllowSetNull)
                {
                    ProcessCaption = $"Set All the {item.DeleteTable.Description}s Related Data to NULL";
                }
                else
                {
                    var description = item.DeleteTable.ChildField.TableDefinition.Description;
                    if (!description.EndsWith('s'))
                    {
                        description += 's';
                    }
                    ProcessCaption = $"Delete All the {description} Related Data";
                }
                LookupDefinition = item.DeleteRecordItem.LookupDefinition;
                LookupCommand = new LookupCommand(LookupCommands.Reset);
            }
        }


        private void SetAllTabsDelete(bool value)
        {
            foreach (var deleteTable in DeleteTables.Tables)
            {
                deleteTable.DeleteAllData = value;
                deleteTable.NullAllData = value;
            }
            //View.SetAllDataDelete(value);
        }

        private void OnOk()
        {
            foreach (var deleteTable in DeleteTables.Tables)
            {
                if (!ValidateDeleteTable(deleteTable))
                {
                    return;
                }
            }
            View.CloseWindow(true);
        }

        public bool ValidateDeleteTable(DeleteTable deleteTable)
        {
            var item = Items.FirstOrDefault(p => p.DeleteTable == deleteTable);
            if (DeleteAllData == true)
            {
                item.Processed = true;
            }
            var getDataResult = deleteTable.Query.GetData();
            if (!getDataResult)
            {
                return false;
            }
            var hasData = deleteTable.Query.RecordCount() > 0;
            var caption = "Validation Failure";
            var tableDescription = deleteTable.Description.Replace("\r\n", " ");
            if (deleteTable.ChildField.AllowNulls && deleteTable.ChildField.AllowUserNulls)
            {
                if (hasData && !deleteTable.ChildField.TableDefinition.CanEditTabe)
                {
                    var message = $"You are not allowed to edit data in the {tableDescription} table. Delete Denied!";
                    SetFocusToTable(deleteTable);
                    ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
                    return false;
                }

                if (hasData && !item.Processed)
                {
                    var message =
                        $"There is data left in the {tableDescription} table. You must edit the table or check Set All Values to NULL before continuing.";
                    SetFocusToTable(deleteTable);
                    ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
                    return false;
                }
            }
            else
            {
                if (hasData && !deleteTable.ChildField.TableDefinition.CanDeleteTable(item.DeleteRecordItem.LookupDefinition))
                {
                    var message = $"You are not allowed to delete data in the {tableDescription} table. Delete Denied!";
                    SetFocusToTable(deleteTable);
                    ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
                    return false;
                }

                if (hasData && !item.Processed)
                {
                    var message =
                        $"There is data left in the {tableDescription} table. You must edit the table or check Delete All Records before continuing.";
                    SetFocusToTable(deleteTable);
                    ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
                    return false;
                }
            }
            return true;
        }

        private void SetFocusToTable(DeleteTable deleteTable)
        {
            var item = Items.FirstOrDefault(p => p.DeleteTable == deleteTable);
            if (item != null)
            {
                SelectedDeleteTable = item.ItemRow;
            }
        }

        private void PreSetActiveTable(ListControlDataSourceRow oldRow)
        {
            var item = Items.FirstOrDefault(p => p.ItemRow == oldRow);
            if (item != null)
            {
                item.Processed = ItemProceessed;
                oldRow.DataCells[2].TextValue = ItemProceessed.ToString();
            }

        }

        private void SetNewActiveTable(ListControlDataSourceRow newRow)
        {
            SetTableLookup(newRow);
            var item = Items.FirstOrDefault(p => p.ItemRow == newRow);
            if (item != null)
            {
                ItemProceessed = item.Processed;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
