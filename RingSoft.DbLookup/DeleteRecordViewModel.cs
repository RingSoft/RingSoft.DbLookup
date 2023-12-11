// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 07-01-2023
// ***********************************************************************
// <copyright file="DeleteRecordViewModel.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.Lookup;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace RingSoft.DbLookup
{
    /// <summary>
    /// Interface IDeleteRecordView
    /// </summary>
    public interface IDeleteRecordView
    {
        /// <summary>
        /// Closes the window.
        /// </summary>
        /// <param name="result">if set to <c>true</c> [result].</param>
        void CloseWindow(bool result);
    }

    /// <summary>
    /// Class DeleteTableWindowData.
    /// </summary>
    public class DeleteTableWindowData
    {
        /// <summary>
        /// Gets or sets the delete record item.
        /// </summary>
        /// <value>The delete record item.</value>
        public DeleteRecordItemViewModel DeleteRecordItem { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [allow set null].
        /// </summary>
        /// <value><c>true</c> if [allow set null]; otherwise, <c>false</c>.</value>
        public bool AllowSetNull { get; set; }

        /// <summary>
        /// Gets or sets the item row.
        /// </summary>
        /// <value>The item row.</value>
        public ListControlDataSourceRow ItemRow { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="DeleteTableWindowData"/> is processed.
        /// </summary>
        /// <value><c>true</c> if processed; otherwise, <c>false</c>.</value>
        public bool Processed { get; set; }

        /// <summary>
        /// Gets or sets the delete table.
        /// </summary>
        /// <value>The delete table.</value>
        public DeleteTable DeleteTable { get; set; }
    }

    /// <summary>
    /// Class DeleteRecordViewModel.
    /// Implements the <see cref="INotifyPropertyChanged" />
    /// </summary>
    /// <seealso cref="INotifyPropertyChanged" />
    public class DeleteRecordViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// The delete all data
        /// </summary>
        private bool _deleteAllData;

        /// <summary>
        /// Gets or sets a value indicating whether [delete all data].
        /// </summary>
        /// <value><c>true</c> if [delete all data]; otherwise, <c>false</c>.</value>
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

        /// <summary>
        /// The table setup
        /// </summary>
        private ListControlSetup _tableSetup;

        /// <summary>
        /// Gets or sets the table setup.
        /// </summary>
        /// <value>The table setup.</value>
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

        /// <summary>
        /// The table data source
        /// </summary>
        private ListControlDataSource _tableDataSource;

        /// <summary>
        /// Gets or sets the table data source.
        /// </summary>
        /// <value>The table data source.</value>
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

        /// <summary>
        /// The selected delete table
        /// </summary>
        private ListControlDataSourceRow _selectedDeleteTable;

        /// <summary>
        /// Gets or sets the selected delete table.
        /// </summary>
        /// <value>The selected delete table.</value>
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

        /// <summary>
        /// The lookup definition
        /// </summary>
        private LookupDefinitionBase _lookupDefinition;

        /// <summary>
        /// Gets or sets the lookup definition.
        /// </summary>
        /// <value>The lookup definition.</value>
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

        /// <summary>
        /// The lookup command
        /// </summary>
        private LookupCommand _lookupCommand;

        /// <summary>
        /// Gets or sets the lookup command.
        /// </summary>
        /// <value>The lookup command.</value>
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

        /// <summary>
        /// The item processed
        /// </summary>
        private bool _itemProcessed;

        /// <summary>
        /// Gets or sets a value indicating whether [item proceessed].
        /// </summary>
        /// <value><c>true</c> if [item proceessed]; otherwise, <c>false</c>.</value>
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

        /// <summary>
        /// The process caption
        /// </summary>
        private string _processCaption;

        /// <summary>
        /// Gets or sets the process caption.
        /// </summary>
        /// <value>The process caption.</value>
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


        /// <summary>
        /// Gets the view.
        /// </summary>
        /// <value>The view.</value>
        public IDeleteRecordView View { get; private set; }

        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <value>The items.</value>
        public List<DeleteTableWindowData> Items { get; private set; } = new List<DeleteTableWindowData>();

        /// <summary>
        /// Gets or sets the ok command.
        /// </summary>
        /// <value>The ok command.</value>
        public RelayCommand OkCommand { get; set; }

        /// <summary>
        /// Gets or sets the cancel command.
        /// </summary>
        /// <value>The cancel command.</value>
        public RelayCommand CancelCommand { get; set; }

        /// <summary>
        /// Gets the delete tables.
        /// </summary>
        /// <value>The delete tables.</value>
        public DeleteTables DeleteTables { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteRecordViewModel"/> class.
        /// </summary>
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

        /// <summary>
        /// Initializes the specified view.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="deleteTables">The delete tables.</param>
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

        /// <summary>
        /// Sets the table lookup.
        /// </summary>
        /// <param name="row">The row.</param>
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


        /// <summary>
        /// Sets all tabs delete.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        private void SetAllTabsDelete(bool value)
        {
            foreach (var deleteTable in DeleteTables.Tables)
            {
                deleteTable.DeleteAllData = value;
                deleteTable.NullAllData = value;
            }
            //View.SetAllDataDelete(value);
        }

        /// <summary>
        /// Called when [ok].
        /// </summary>
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

        /// <summary>
        /// Validates the delete table.
        /// </summary>
        /// <param name="deleteTable">The delete table.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
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

        /// <summary>
        /// Sets the focus to table.
        /// </summary>
        /// <param name="deleteTable">The delete table.</param>
        private void SetFocusToTable(DeleteTable deleteTable)
        {
            var item = Items.FirstOrDefault(p => p.DeleteTable == deleteTable);
            if (item != null)
            {
                SelectedDeleteTable = item.ItemRow;
            }
        }

        /// <summary>
        /// Pres the set active table.
        /// </summary>
        /// <param name="oldRow">The old row.</param>
        private void PreSetActiveTable(ListControlDataSourceRow oldRow)
        {
            var item = Items.FirstOrDefault(p => p.ItemRow == oldRow);
            if (item != null)
            {
                item.Processed = ItemProceessed;
                oldRow.DataCells[2].TextValue = ItemProceessed.ToString();
            }

        }

        /// <summary>
        /// Sets the new active table.
        /// </summary>
        /// <param name="newRow">The new row.</param>
        private void SetNewActiveTable(ListControlDataSourceRow newRow)
        {
            SetTableLookup(newRow);
            var item = Items.FirstOrDefault(p => p.ItemRow == newRow);
            if (item != null)
            {
                ItemProceessed = item.Processed;
            }
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Sets the field.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
