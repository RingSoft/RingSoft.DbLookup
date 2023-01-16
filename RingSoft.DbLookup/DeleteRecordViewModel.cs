using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.DataProcessor;

namespace RingSoft.DbLookup
{
    public interface IDeleteRecordView
    {
        void CloseWindow(bool result);

        void SetAllDataDelete(bool value);

        void SetAllDataNull(bool value);

        void SetFocusToTable(DeleteTable deleteTable);
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
                SetAllTabsDelete(value);
                OnPropertyChanged();
            }
        }

        public IDeleteRecordView View { get; private set; }

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
        }

        private void SetAllTabsDelete(bool value)
        {
            foreach (var deleteTable in DeleteTables.Tables)
            {
                deleteTable.DeleteAllData = value;
                deleteTable.NullAllData = value;
            }
            View.SetAllDataDelete(value);
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
            var getDataResult = deleteTable.ChildField.TableDefinition.Context.DataProcessor.GetData(deleteTable.Query);
            if (getDataResult.ResultCode != GetDataResultCodes.Success)
            {
                return false;
            }
            var hasData = getDataResult.DataSet.Tables[0].Rows.Count > 0;
            var caption = "Validation Failure";
            var tableDescription = deleteTable.Description.Replace("\r\n", " ");
            if (deleteTable.ChildField.AllowNulls && deleteTable.ChildField.AllowUserNulls)
            {
                if (hasData && !deleteTable.ChildField.TableDefinition.CanEditTabe)
                {
                    var message = $"You are not allowed to edit data in the {tableDescription} table. Delete Denied!";
                    View.SetFocusToTable(deleteTable);
                    ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
                    return false;
                }

                if (hasData && deleteTable.NullAllData == false)
                {
                    var message =
                        $"There is data left in the {tableDescription} table. You must edit the table or check Set All Values to NULL before continuing.";
                    View.SetFocusToTable(deleteTable);
                    ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
                    return false;
                }
            }
            else
            {
                if (hasData && !deleteTable.ChildField.TableDefinition.CanDeleteTable)
                {
                    var message = $"You are not allowed to delete data in the {tableDescription} table. Delete Denied!";
                    View.SetFocusToTable(deleteTable);
                    ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
                    return false;
                }

                if (hasData && deleteTable.DeleteAllData == false)
                {
                    var message =
                        $"There is data left in the {tableDescription} table. You must edit the table or check Delete All Records before continuing.";
                    View.SetFocusToTable(deleteTable);
                    ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
                    return false;
                }
            }
            return true;
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
