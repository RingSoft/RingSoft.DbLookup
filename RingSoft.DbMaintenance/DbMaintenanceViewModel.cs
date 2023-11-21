using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.DataProcessor.SelectSqlGenerator;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DbLookup.RecordLocking;
using RingSoft.DbLookup.TableProcessing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Timers;
using Microsoft.VisualBasic;

namespace RingSoft.DbMaintenance
{
    public enum ViewModelOperations
    {
        Save = 0,
        Delete = 1
    }

    public class ViewModelOperationPreviewEventArgs<TEntity> where TEntity : new()
    {
        public TEntity Entity { get; internal set; }

        public bool Handled { get; set; }

        public ViewModelOperations  Operation { get; internal set; }
    }

    /// <summary>
    /// The base class of all database maintenance view model classes.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <seealso cref="DbMaintenanceViewModelBase" />
    /// <seealso cref="ILookupControl" />
    public abstract class DbMaintenanceViewModel<TEntity> : DbMaintenanceViewModelBase, ILookupControl, IValidationSource
        where TEntity : class, new()
    {
        public TEntity Entity { get; private set; }

        public int PageSize { get; } = 1;
        public LookupSearchTypes SearchType { get; } = LookupSearchTypes.Equals;
        public string SearchText { get; set; } = string.Empty;
        public int SelectedIndex => 0;
        public void SetLookupIndex(int index)
        {
        }

        /// <summary>
        /// Gets the table definition.
        /// </summary>
        /// <value>
        /// The table definition.
        /// </value>
        public virtual TableDefinition<TEntity> TableDefinition { get; }

        public override sealed TableDefinitionBase TableDefinitionBase => TableDefinition;

        /// <summary>
        /// Gets the initial search for text when the Find button is clicked.  By default it is the key auto fill text.
        /// </summary>
        /// <value>
        /// The find button initial search for.
        /// </value>
        protected override string FindButtonInitialSearchFor
        {
            get
            {
                if (KeyAutoFillValue != null)
                {
                    return KeyAutoFillValue.Text;
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this process has changed the data source.
        /// </summary>
        /// <value>
        ///   <c>true</c> if records changed; otherwise, <c>false</c>.
        /// </value>
        public bool RecordsChanged { get; private set; }


        /// <summary>
        /// Gets a value indicating whether the base entity is loading from the database or is being cleared.
        /// </summary>
        /// <value>
        ///   <c>true</c> if loading from the database or clearing; otherwise, <c>false</c>.
        /// </value>
        protected bool ChangingEntity { get; private set; }

        /// <summary>
        /// Gets the rename key auto fill value message caption.  Override this for localization.
        /// </summary>
        /// <value>
        /// The rename key auto fill value caption.  Override this for localization.
        /// </value>
        protected virtual string RenameKeyAutoFillValueCaption => "Change Unique Field Value";

        /// <summary>
        /// Gets the save changes message.  Override this for localization.
        /// </summary>
        /// <value>
        /// The save changes message.
        /// </value>
        protected virtual string SaveChangesMessage => "Do you wish to save your changes to this data?";

        /// <summary>
        /// Gets the confirm delete caption.  Override this for localization.
        /// </summary>
        /// <value>
        /// The confirm delete caption.
        /// </value>
        protected virtual string ConfirmDeleteCaption => "Confirm Delete";

        protected virtual List<TableDefinitionBase> TablesToDelete { get; } = new List<TableDefinitionBase>();

        public event EventHandler<ViewModelOperationPreviewEventArgs<TEntity>> ViewModelOperationPreview;

        public bool ValidateAllAtOnce { get; set; }

        private LookupDataMauiBase _lookupData;
        private bool _fromLookupFormAddView;
        private AutoFillValue _savedKeyAutoFillValue;
        private bool _selectingRecord;
        private bool _savingRecord;
        private bool _lookupReadOnlyMode;
        private AutoFillValue _readOnlyAutoFillValue;
        private DateTime? _startDate;
        private System.Timers.Timer _timer = new Timer(1000);
        private bool _isActive = true;

        public DbMaintenanceViewModel()
        {
            if (TableDefinition == null)
            {
                TableDefinition = GblMethods.GetTableDefinition<TEntity>();

            }

            _startDate = DateTime.Now;
            _timer.Elapsed += (sender, args) =>
            {
                if (Processor == null)
                {
                    return;
                }
                _timer.Stop();
                if (_isActive == false)
                {
                    return;
                }
                var duration = DateTime.Now.Subtract(_startDate.Value.ToLocalTime());
                var minutes = duration.TotalMinutes;

                if (minutes > 10 && minutes < 20 && RecordDirty)
                {
                    Processor.SetSaveStatus("Don't forget to save this record.", AlertLevels.Yellow);
                }
                else if (minutes > 20 && RecordDirty)
                {
                    Processor.SetSaveStatus("Save this record ASAP!", AlertLevels.Red);
                }
                else
                {
                    Processor.SetSaveStatus("", AlertLevels.Green);
                }

                _timer.Start();
            };
            _timer.Enabled = true;
            _timer.Start();
        }
        protected virtual void SetupViewLookupDefinition( LookupDefinitionBase lookupDefinition)
        {

        }
        protected internal override void InternalInitialize()
        {
            if (TableDefinition.LookupDefinition == null)
                throw new ArgumentException(
                    $"Table definition '{TableDefinition}' does not have a lookup definition setup.");

            var tableLookupDefinition = TableDefinition.LookupDefinition.Clone();
            Setup(tableLookupDefinition);
            SetupViewLookupDefinition(ViewLookupDefinition);
            _lookupData = TableDefinition.LookupDefinition.GetLookupDataMaui(ViewLookupDefinition, true);
            _lookupData.SetParentControls(this);

            //_lookupData.LookupDataChanged += _lookupData_LookupDataChanged;
            _lookupData.LookupDataChanged += _lookupData_LookupDataChanged1;
            FindButtonLookupDefinition = ViewLookupDefinition;

            base.InternalInitialize();

            if (LookupAddViewArgs != null)
            {
                if (InputParameter == null)
                {
                    InputParameter = LookupAddViewArgs.InputParameter;
                }

                var filter = GetAddViewFilter();
                if (filter != null)
                {
                    _lookupData.LookupDefinition.FilterDefinition.CopyFrom(filter);
                    _lookupData.LookupDefinition.FilterDefinition.ClearUserFilters();
                    if (LookupAddViewArgs.LookupData.LookupDefinition.TableDefinition == TableDefinition)
                    {
                        foreach (var joinDefinition in LookupAddViewArgs.LookupData.LookupDefinition.Joins)
                        {
                            _lookupData.LookupDefinition.AddCopyJoin(joinDefinition);
                        }
                    }
                }
            }

            Initialize();
        }

        private void _lookupData_LookupDataChanged1(object sender, LookupDataMauiOutput e)
        {
            if (_lookupData.SelectedPrimaryKeyValue == null)
            {
                return;
            }
            {
                MaintenanceMode = DbMaintenanceModes.EditMode;
                TEntity newEntity =
                    TableDefinition.GetEntityFromPrimaryKeyValue(_lookupData.SelectedPrimaryKeyValue);

                ChangingEntity = true;
                ControlsGlobals.UserInterface.SetWindowCursor(WindowCursorTypes.Wait);
                LockDate = DateTime.Now;
                GetLastSavedDate(_lookupData.SelectedPrimaryKeyValue);

                Entity = PopulatePrimaryKeyControls(newEntity, _lookupData.SelectedPrimaryKeyValue);
                if (Entity == null)
                {
                    DbDataProcessor.UserInterface.PlaySystemSound(RsMessageBoxIcons.Exclamation);
                    ControlsGlobals.UserInterface.SetWindowCursor(WindowCursorTypes.Default);
                    ChangingEntity = false;
                    return;
                }
                if (!_savingRecord)
                {
                    if (SystemGlobals.UnitTestMode)
                    {
                        TableDefinition.FillOutEntity(Entity);
                    }

                    if (KeyAutoFillSetup != null)
                    {
                        KeyAutoFillValue = Entity.GetAutoFillValue();
                    }
                    LoadFromEntity(Entity);
                    Processor?.OnRecordSelected();
                }
                ControlsGlobals.UserInterface.SetWindowCursor(WindowCursorTypes.Default);
                ChangingEntity = false;

                OnLookupDataChanged();
            }
        }

        protected virtual TableFilterDefinitionBase GetAddViewFilter()
        {
            if (LookupAddViewArgs.LookupData.LookupDefinition.TableDefinition == TableDefinition)
            {
                var result = LookupAddViewArgs.LookupData.LookupDefinition.FilterDefinition;
                return result;
            }

            return null;
        }

        /// <summary>
        /// Initializes this instance.  Executed after the view is loaded.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        protected virtual void Initialize()
        {
            if (LookupAddViewArgs != null)
            {
                var primaryKeyValue = GetAddViewPrimaryKeyValue(LookupAddViewArgs.LookupData.SelectedPrimaryKeyValue);
                switch (LookupAddViewArgs.LookupFormMode)
                {
                    case LookupFormModes.Add:
                        RecordDirty = false;
                        OnNewButton();
                        //if (primaryKeyValue != null)
                            KeyAutoFillValue = new AutoFillValue(primaryKeyValue, LookupAddViewArgs.InitialAddModeText);
                        break;
                    default:
                        DeleteButtonEnabled = LookupAddViewArgs.AllowEdit && MaintenanceMode == DbMaintenanceModes.EditMode;
                        break;
                }

                NewButtonEnabled = SaveButtonEnabled = LookupAddViewArgs.AllowEdit;

                if (primaryKeyValue != null && primaryKeyValue.IsValid())
                    _lookupData.SelectPrimaryKey(primaryKeyValue);

                if (LookupAddViewArgs.LookupReadOnlyMode)
                {
                    SelectButtonEnabled = false;
                    _lookupReadOnlyMode = true;
                }
            }
            else
            {
                RecordDirty = false;
                OnNewButton();
            }

            RecordDirty = false;

            FireInitializeEvent();
        }

        protected virtual PrimaryKeyValue GetAddViewPrimaryKeyValue(PrimaryKeyValue addViewPrimaryKeyValue)
        {
            var selectedPrimaryKeyValue = addViewPrimaryKeyValue;
            if (selectedPrimaryKeyValue == null)
            {
                return null;
            }
            if (addViewPrimaryKeyValue.TableDefinition != TableDefinition && TableDefinition.PrimaryKeyFields.Count == 1)
            {
                var pkField = TableDefinition.PrimaryKeyFields[0];
                var value = addViewPrimaryKeyValue.KeyValueFields[0].Value;
                var primaryField = addViewPrimaryKeyValue.TableDefinition.FieldDefinitions
                    .FirstOrDefault(p => p.ParentJoinForeignKeyDefinition != null
                                         && p.ParentJoinForeignKeyDefinition.FieldJoins[0].PrimaryField == pkField);

                var selectQuery = addViewPrimaryKeyValue.TableDefinition.LookupDefinition.GetSelectQueryMaui();
                selectQuery.SetMaxRecords(1);
                var column = selectQuery.AddColumn(primaryField);
                var pkColumn = selectQuery.AddColumn(addViewPrimaryKeyValue.KeyValueFields[0].FieldDefinition);
                selectQuery.AddFilter(pkColumn, Conditions.Equals, value);
                if (selectQuery.GetData())
                {
                    var listPks = selectQuery.GetDataResult();
                    if (listPks.Any())
                    {
                        var newPrimaryKey = new PrimaryKeyValue(TableDefinition);
                        newPrimaryKey.LoadFromIdValue(listPks[0].KeyValueFields[0].Value);
                        selectedPrimaryKeyValue = newPrimaryKey;
                    }
                }

                //var joinTable = selectQuery.AddPrimaryJoinTable(JoinTypes.InnerJoin, TableDefinition.TableName);
                //joinTable.AddJoinField(pkField.FieldName, primaryField.FieldName);
                //selectQuery.AddSelectColumn(primaryField.FieldName);
                //selectQuery.AddWhereItem(addViewPrimaryKeyValue.KeyValueFields[0].FieldDefinition.FieldName
                //    , Conditions.Equals, value);

                //var getTableResult = TableDefinition.Context.DataProcessor.GetData(selectQuery);
                //if (getTableResult.ResultCode == GetDataResultCodes.Success)
                //{
                //    var table = getTableResult.DataSet.Tables[0];
                //    if (table.Rows.Count > 0)
                //    {
                //        var newPrimaryKey = new PrimaryKeyValue(TableDefinition);
                //        newPrimaryKey.LoadFromIdValue(table.Rows[0].GetRowValue(primaryField.FieldName));
                //        selectedPrimaryKeyValue = newPrimaryKey;
                //    }
                //}
            }
            var result = addViewPrimaryKeyValue;
            switch (LookupAddViewArgs.LookupFormMode)
            {
                case LookupFormModes.Add:
                    if (!LookupAddViewArgs.InitialAddModeText.IsNullOrEmpty())
                    {
                        result =
                            LookupAddViewArgs.LookupData.GetPrimaryKeyValueForSearchText(LookupAddViewArgs
                                .InitialAddModeText);
                    }
                    else
                    {
                        result = new PrimaryKeyValue(addViewPrimaryKeyValue.TableDefinition);
                    }
                    break;
                case LookupFormModes.View:
                    if (selectedPrimaryKeyValue != null)
                    {
                        result = selectedPrimaryKeyValue;
                    }
                    break;
            }

            //if (result.TableDefinition != TableDefinition)
            //    throw new Exception($"The Add/View's Primary Key Definition's Table Definition '{result.TableDefinition}' does not match this Table Definition ('{TableDefinition}').  You must override GetAddViewPrimaryKeyValue and return a PrimaryKeyValue whose Table Definition is '{TableDefinition}'.");

            return result;
        }

        /// <summary>
        /// Changes the sort column.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <exception cref="ArgumentException">Column {column.Caption} is not found in this view model's table lookup definition.</exception>
        protected void ChangeSortColumn(LookupColumnDefinitionBase column)
        {
            var columnIndex = _lookupData.LookupDefinition.GetIndexOfVisibleColumn(column);
            if (columnIndex < 0)
                throw new ArgumentException($"Column {column.Caption} is not found in this view model's table lookup definition.");

            //_lookupData.OnColumnClick(columnIndex, true);
            _lookupData.OnColumnClick((LookupFieldColumnDefinition)column, true);
        }

        private void _lookupData_LookupDataChanged(object sender, LookupDataChangedArgs e)
        {
            if (e.SelectedRowIndex >= 0)
            {
                MaintenanceMode = DbMaintenanceModes.EditMode;
                TEntity newEntity =
                    TableDefinition.GetEntityFromPrimaryKeyValue(_lookupData.SelectedPrimaryKeyValue);

                ChangingEntity = true;
                ControlsGlobals.UserInterface.SetWindowCursor(WindowCursorTypes.Wait);
                LockDate = DateTime.Now;
                GetLastSavedDate(_lookupData.SelectedPrimaryKeyValue);

                Entity = PopulatePrimaryKeyControls(newEntity, _lookupData.SelectedPrimaryKeyValue);
                if (Entity == null)
                {
                    DbDataProcessor.UserInterface.PlaySystemSound(RsMessageBoxIcons.Exclamation);
                    ControlsGlobals.UserInterface.SetWindowCursor(WindowCursorTypes.Default);
                    ChangingEntity = false;
                    return;
                }
                if (!_savingRecord)
                {
                    LoadFromEntity(Entity);
                    Processor?.OnRecordSelected();
                }
                ControlsGlobals.UserInterface.SetWindowCursor(WindowCursorTypes.Default);
                ChangingEntity = false;

                OnLookupDataChanged();
            }
        }

        private void OnLookupDataChanged()
        {
            if (_lookupData.SelectedPrimaryKeyValue.IsEqualTo(LookupAddViewArgs?.ReadOnlyPrimaryKeyValue))
            {
                DeleteButtonEnabled = false;
            }
            else
            {

                DeleteButtonEnabled = MaintenanceMode == DbMaintenanceModes.EditMode && !ReadOnlyMode;
            }
            SelectButtonEnabled = _fromLookupFormAddView && !_lookupReadOnlyMode;
            PrimaryKeyControlsEnabled = false;
            RecordDirty = false;
            _savedKeyAutoFillValue = KeyAutoFillValue;

        }

        private bool DataExists()
        {
            if (_lookupData.IsThereData())
            {
                return true;
            }

            var message = "There is no data in this table";
            var caption = "No Data";
            ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Information);

            return false;
        }

        /// <summary>
        /// Called when Previous button is clicked.
        /// </summary>
        public override void OnGotoPreviousButton()
        {
            if (!DataExists())
            {
                return;
            }
            FirePreviousEvent();
            if (!PreviousCommand.IsEnabled)
                return;

            if (!CheckDirty())
                return;

            if (MaintenanceMode == DbMaintenanceModes.AddMode)
            {
                _lookupData.GotoBottom();
            }
            else
            {
                if (_lookupData.ScrollPosition == LookupScrollPositions.Top
                                        )
                    _lookupData.GotoBottom();
                else
                {
                    _lookupData.GotoPreviousRecord();
                }
            }

            //if (!Processor.IsMaintenanceKeyDown(MaintenanceKey.Ctrl))
            //{
            //    View.ResetViewForNewRecord();
            //}

        }

        /// <summary>
        /// Called when Next button is clicked.
        /// </summary>
        public override void OnGotoNextButton()
        {
            if (!DataExists())
            {
                return;
            }

            FireNextEvent();
            if (!NextCommand.IsEnabled)
                return;

            if (!CheckDirty())
                return;

            if (MaintenanceMode == DbMaintenanceModes.AddMode)
            {
                _lookupData.GotoTop();
            }
            else
            {
                if (_lookupData.ScrollPosition == LookupScrollPositions.Bottom)
                _lookupData.GotoTop();
                else
                {
                    _lookupData.GotoNextRecord();
                }
            }

            //if (!Processor.IsMaintenanceKeyDown(MaintenanceKey.Ctrl))
            //{
            //    View.ResetViewForNewRecord();
            //}
        }
        /// <summary>
        /// Called when the Find button is clicked.
        /// </summary>
        public override void OnFindButton()
        {
            var keyDown = Processor.IsMaintenanceKeyDown(MaintenanceKey.Alt);

            FireFindEvent();
            if (!FindCommand.IsEnabled)
                return;

            var searchText = FindButtonInitialSearchFor;
            if (FindButtonLookupDefinition.InitialOrderByColumn != FindButtonLookupDefinition.InitialSortColumnDefinition &&
                !FindButtonInitialSearchFor.IsNullOrEmpty() && _lookupData.SelectedPrimaryKeyValue.IsValid())
            {
                searchText =
                    FindButtonLookupDefinition.InitialOrderByColumn.GetTextForColumn(
                        _lookupData.SelectedPrimaryKeyValue);
            }

            var primaryKey = _lookupData.SelectedPrimaryKeyValue;
            if (MaintenanceMode == DbMaintenanceModes.AddMode)
            {
                primaryKey = null;
            }
            Processor.ShowFindLookupWindow(FindButtonLookupDefinition, false, false, searchText,
                primaryKey);

            //if (!keyDown)
            //{
            //    View.ResetViewForNewRecord();
            //}
        }

        public override void OnRecordSelected(LookupSelectArgs e)
        {
            OnRecordSelected(e.LookupData.SelectedPrimaryKeyValue);
        }

        public override void OnRecordSelected(PrimaryKeyValue primaryKey)
        {
            if (!CheckDirty())
                return;

            _lookupData.SelectPrimaryKey(primaryKey);
        }

        /// <summary>
        /// Called when the Select button is clicked.
        /// </summary>
        public override void OnSelectButton()
        {
            var args = FireSelectEvent();
            
            if (!args.Cancel)
            {
                if (!SelectCommand.IsEnabled)
                    return;

                if (!CheckDirty())
                    return;

                _selectingRecord = true;
                if (_lookupData.SelectedPrimaryKeyValue != null && _lookupData.SelectedPrimaryKeyValue.IsValid())
                {
                    LookupAddViewArgs.CallBackToken.NewAutoFillValue =
                        TableDefinition
                            .LookupDefinition
                            .GetAutoFillValue(_lookupData.SelectedPrimaryKeyValue);
                    LookupAddViewArgs.CallBackToken.RefreshMode = AutoFillRefreshModes.DbSelect;
                }

                Processor.CloseWindow();
                //LookupAddViewArgs.LookupData.ViewSelectedRow(0, View);
                LookupAddViewArgs.CallBackToken.OnRefreshData();  //Necessary to select drill down to AutoFillControl.
                LookupAddViewArgs.LookupData.SelectPrimaryKey(_lookupData.SelectedPrimaryKeyValue);
            }
        }

        /// <summary>
        /// Called when the New button is clicked.
        /// </summary>
        public override void OnNewButton()
        {
            if (!CheckDirty())
                return;

            MaintenanceMode = DbMaintenanceModes.AddMode;

            ChangingEntity = true;
            ClearData();
            Processor.SetSaveStatus("", AlertLevels.Green);

            LastSavedDate = null;
            _startDate = DateTime.Now;
            KeyAutoFillValue = new AutoFillValue(new PrimaryKeyValue(TableDefinition), string.Empty);
            ChangingEntity = false;

            SelectButtonEnabled = false;
            DeleteButtonEnabled = false;
            PrimaryKeyControlsEnabled = true;
            ReadOnlyMode = false;

            KeyAutoFillUiCommand.IsEnabled = true;
            KeyAutoFillUiCommand.SetFocus();
            View.ResetViewForNewRecord();
            RecordDirty = false;
            FireNewEvent();
        }

        /// <summary>
        /// Called when the Save button is clicked.
        /// </summary>
        /// <param name="unitTestMode"></param>
        /// <returns>
        /// The result.
        /// </returns>
        public override DbMaintenanceResults DoSave(bool unitTestMode = false)
        {
            if (Processor == null)
            {
                throw new Exception("Processor is null");
            }
            var keyDown = Processor.IsMaintenanceKeyDown(MaintenanceKey.Alt);
            FireSaveEvent();
            if (!SaveCommand.IsEnabled)
                return DbMaintenanceResults.NotAllowed;

            if (MaintenanceMode == DbMaintenanceModes.AddMode && KeyAutoFillValue.IsValid()
                && _lookupData != null)
            {
                _lookupData.SelectPrimaryKey(KeyAutoFillValue.PrimaryKeyValue);
                return DbMaintenanceResults.Success;
            }

            if (!CheckKeyValueTextChanged())
                return DbMaintenanceResults.ValidationError;

            var entity = GetEntityData();

            if (Processor.KeyControlRegistered)
            {
                if (KeyAutoFillValue == null || KeyAutoFillValue.Text.IsNullOrEmpty())
                {
                    if (KeyAutoFillSetup.LookupDefinition.InitialSortColumnDefinition is LookupFieldColumnDefinition
                        lookupFieldColumn)
                    {
                        if (!lookupFieldColumn.FieldDefinition.GeneratedKey || MaintenanceMode == DbMaintenanceModes.EditMode)
                        {
                            var message = $"{lookupFieldColumn.FieldDefinition.Description} cannot be empty.";
                            var caption = "Invalid Key Value";
                            ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
                            KeyAutoFillUiCommand.SetFocus();
                            return DbMaintenanceResults.ValidationError;
                        }
                    }
                }
            }
            if (!ValidateEntity(entity))
                return DbMaintenanceResults.ValidationError;

            var previewArgs = new ViewModelOperationPreviewEventArgs<TEntity>
            {
                Entity = entity,
                Operation = ViewModelOperations.Save
            };

            OnViewModelOperationPreview(previewArgs);

            if (previewArgs.Handled)
                return DbMaintenanceResults.NotAllowed;

            if (!unitTestMode)
            {
                var recordLockPrimaryKey = TableDefinition.GetPrimaryKeyValueFromEntity(entity);
                var keyString = recordLockPrimaryKey.KeyString;
                //var tableField =
                //    SystemGlobals.AdvancedFindLookupContext.RecordLocks.GetFieldDefinition(p => p.Table);
                //var pkField =
                //    SystemGlobals.AdvancedFindLookupContext.RecordLocks.GetFieldDefinition(p => p.PrimaryKey);
                //var dateField =
                //    SystemGlobals.AdvancedFindLookupContext.RecordLocks.GetFieldDefinition(p => p.LockDateTime);
                //var userField = SystemGlobals.AdvancedFindLookupContext.RecordLocks.GetFieldDefinition(p => p.User);

                switch (MaintenanceMode)
                {
                    case DbMaintenanceModes.AddMode:
                        break;
                    case DbMaintenanceModes.EditMode:
                        var context = SystemGlobals.DataRepository.GetDataContext();
                        var query = context.GetTable<RecordLock>();
                        var recordLock = query.FirstOrDefault(
                            p => p.Table == TableDefinition.TableName
                                 && p.PrimaryKey == keyString
                                 && p.LockDateTime >= LockDate.ToUniversalTime());
                        //var selectQuery =
                        //    new SelectQuery(SystemGlobals.AdvancedFindLookupContext.RecordLocks.TableName);
                        //selectQuery.AddWhereItem(tableField.FieldName, Conditions.Equals, TableDefinition.TableName);
                        //selectQuery.AddWhereItem(pkField.FieldName, Conditions.Equals, keyString);
                        //var lockDateWhere = selectQuery.AddWhereItem(dateField.FieldName, Conditions.GreaterThanEquals,
                        //    LockDate.ToUniversalTime(),
                        //    DbDateTypes.Millisecond);

                        //var dataResult =
                        //    SystemGlobals.AdvancedFindLookupContext.RecordLocks.Context.DataProcessor.GetData(
                        //        selectQuery);

                        //if (dataResult.ResultCode == GetDataResultCodes.Success)
                        {
                            //var sqlGenerator = SystemGlobals.AdvancedFindLookupContext.RecordLocks.Context.DataProcessor
                            //    .SqlGenerator;
                            //var table = SystemGlobals.AdvancedFindLookupContext.RecordLocks.TableName;
                            //table = sqlGenerator.FormatSqlObject(table);

                            if (recordLock != null)
                            {
                                //var row = dataResult.DataSet.Tables[0].Rows[0];
                                var message =
                                        $"You started editing this record on {LockDate.ToString("dddd, MMM dd yyyy")} at {LockDate.ToString("h:mm:ss tt")}.";
                                message +=
                                    "  This record was saved by someone else while you were editing.  Do you wish to continue saving?";

                                //var lockKey =
                                //    new PrimaryKeyValue(SystemGlobals.AdvancedFindLookupContext.RecordLocks);
                                //lockKey.PopulateFromDataRow(row);
                                if (!Processor.ShowRecordLockWindow(SystemGlobals.AdvancedFindLookupContext.RecordLocks.GetPrimaryKeyValueFromEntity(recordLock), message, InputParameter))
                                {
                                    return DbMaintenanceResults.ValidationError;
                                }
                            }

                            //selectQuery.RemoveWhereItem(lockDateWhere);
                            //dataResult =
                            //    SystemGlobals.AdvancedFindLookupContext.RecordLocks.Context.DataProcessor.GetData(
                            //        selectQuery);

                            //if (dataResult.DataSet.Tables[0].Rows.Count > 0)
                            //{
                            //    lockSql = GetUpdateLockSql(table, sqlGenerator, tableField, pkField, keyString,
                            //        dateField, userField);
                            //}
                            //else
                            //{
                            //    var fields = sqlGenerator.FormatSqlObject(tableField.FieldName);
                            //    var values = sqlGenerator.ConvertValueToSqlText(TableDefinition.TableName,
                            //        ValueTypes.String, DbDateTypes.DateTime);

                            //    fields += $", {sqlGenerator.FormatSqlObject(pkField.FieldName)}";
                            //    values +=
                            //        $", {sqlGenerator.ConvertValueToSqlText(keyString, ValueTypes.String, DbDateTypes.DateTime)}";

                            //    fields += $", {sqlGenerator.FormatSqlObject(dateField.FieldName)}";
                            //    var dateText = GetNowDateText();
                            //    values +=
                            //        $", {sqlGenerator.ConvertValueToSqlText(dateText, ValueTypes.DateTime, DbDateTypes.Millisecond)}";

                            //    if (!SystemGlobals.UserName.IsNullOrEmpty())
                            //    {
                            //        fields += $", {sqlGenerator.FormatSqlObject(userField.FieldName)}";
                            //        values +=
                            //            $", {sqlGenerator.ConvertValueToSqlText(SystemGlobals.UserName, ValueTypes.String, DbDateTypes.DateTime)}";
                            //    }

                            //    lockSql = $"INSERT INTO {table} ({fields}) VALUES ({values})";
                            //}
                        }

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (!SaveEntity(entity))
                return DbMaintenanceResults.DatabaseError;
            var primaryKey = TableDefinition.GetPrimaryKeyValueFromEntity(entity);

            if (!unitTestMode)
            {
                switch (MaintenanceMode)
                {
                    case DbMaintenanceModes.AddMode:
                        break;
                    case DbMaintenanceModes.EditMode:
                        if (!GblMethods.DoRecordLock(primaryKey))
                        {
                            return DbMaintenanceResults.DatabaseError;
                        }
                        break;
                }

                LockDate = DateTime.Now;
                _savedKeyAutoFillValue = KeyAutoFillValue;
                GetLastSavedDate(primaryKey);

                //var recordLockResult = SystemGlobals.AdvancedFindLookupContext.RecordLocks.Context.DataProcessor
                //    .ExecuteSql(lockSql);

                //if (recordLockResult.ResultCode != GetDataResultCodes.Success)
                //{
                //    return DbMaintenanceResults.DatabaseError;
                //}
            }

            _savingRecord = true;

            if (unitTestMode)
            {
                PopulatePrimaryKeyControls(entity, primaryKey);
                OnLookupDataChanged();
            }
            else
            {
                if (MaintenanceMode == DbMaintenanceModes.AddMode)
                {
                    var context = SystemGlobals.DataRepository.GetDataContext();
                    _lookupData.SelectPrimaryKey(primaryKey);
                }

                if (LookupAddViewArgs != null)
                {
                    if (_lookupData.SelectedPrimaryKeyValue != null && _lookupData.SelectedPrimaryKeyValue.IsValid())
                    {
                        LookupAddViewArgs.CallBackToken.NewAutoFillValue =
                            TableDefinition.LookupDefinition.GetAutoFillValue(_lookupData.SelectedPrimaryKeyValue);
                    }

                    LookupAddViewArgs.CallBackToken.RefreshMode = AutoFillRefreshModes.PkRefresh;
                    LookupAddViewArgs.CallBackToken.OnRefreshData(); //Important so launched from lookup gets refreshed.
                    LookupAddViewArgs.LookupData.SelectPrimaryKey(_lookupData.SelectedPrimaryKeyValue);
                }

            }

            _savingRecord = false;

            Processor?.ShowRecordSavedMessage();
            RecordDirty = false;
            RecordsChanged = true;

            //if (!keyDown)
            //{
            //    View.ResetViewForNewRecord();
            //}

            return DbMaintenanceResults.Success;
        }

        private void GetLastSavedDate(PrimaryKeyValue primaryKey)
        {
            var context = SystemGlobals.DataRepository.GetDataContext();
            if (context != null)
            {
                var table = context.GetTable<RecordLock>();
                if (table != null)
                {
                    var recordLock = table.FirstOrDefault(
                        p => p.Table == TableDefinition.TableName
                             && p.PrimaryKey == primaryKey.KeyString);
                    if (recordLock == null)
                    {
                        LastSavedDate = _startDate = DateTime.Now;
                    }
                    else
                    {
                        LastSavedDate = recordLock.LockDateTime.ToLocalTime();
                    }
                    Processor.SetSaveStatus("", AlertLevels.Green);
                    _startDate = DateTime.Now;
                }
            }
        }

        private string GetUpdateLockSql(string table, DbSelectSqlGenerator sqlGenerator, StringFieldDefinition tableField,
            StringFieldDefinition pkField, string keyString, DateFieldDefinition dateField, StringFieldDefinition userField)
        {
            string lockSql;
            lockSql = $"UPDATE {table} SET ";//{sqlGenerator.FormatSqlObject(tableField.FieldName)}";
                                             //lockSql +=
                                             //    $" = {sqlGenerator.ConvertValueToSqlText(TableDefinition.TableName, ValueTypes.String, DbDateTypes.DateTime)}";

            //lockSql += $", {sqlGenerator.FormatSqlObject(pkField.FieldName)}";
            //lockSql +=
            //    $" = {sqlGenerator.ConvertValueToSqlText(keyString, ValueTypes.String, DbDateTypes.DateTime)}";

            var dateText = GetNowDateText();
            lockSql += $"{sqlGenerator.FormatSqlObject(dateField.FieldName)}";
            lockSql +=
                $" = {sqlGenerator.ConvertValueToSqlText(dateText, ValueTypes.DateTime, DbDateTypes.Millisecond)}";

            if (!SystemGlobals.UserName.IsNullOrEmpty())
            {
                lockSql += $", {sqlGenerator.FormatSqlObject(userField.FieldName)}";
                lockSql +=
                    $" = {sqlGenerator.ConvertValueToSqlText(SystemGlobals.UserName, ValueTypes.String, DbDateTypes.DateTime)}";
            }

            lockSql += $"WHERE {sqlGenerator.FormatSqlObject(tableField.FieldName)} = ";
            lockSql +=
                $"{sqlGenerator.ConvertValueToSqlText(TableDefinition.TableName, ValueTypes.String, DbDateTypes.DateTime)} ";

            lockSql += $"AND {sqlGenerator.FormatSqlObject(pkField.FieldName)} = ";
            lockSql +=
                $"{sqlGenerator.ConvertValueToSqlText(keyString, ValueTypes.String, DbDateTypes.DateTime)} ";

            return lockSql;
        }

        private static string GetNowDateText()
        {
            var newDate = DateTime.Now.ToUniversalTime();
            var dateText = newDate.ToString("yyyy-MM-dd HH:mm:ss.fff");
            return dateText;
        }

        protected virtual bool CheckKeyValueTextChanged()
        {
            if (MaintenanceMode == DbMaintenanceModes.EditMode && _savedKeyAutoFillValue != null &&
                KeyAutoFillValue != null && _savedKeyAutoFillValue.Text != KeyAutoFillValue.Text)
            {
                var recordDescription = TableDefinition.RecordDescription;
                if (recordDescription.IsNullOrEmpty())
                    recordDescription = TableDefinition.ToString();

                var fieldDescription = string.Empty;
                if (KeyAutoFillSetup.LookupDefinition.InitialSortColumnDefinition is LookupFieldColumnDefinition
                    fieldColumn)
                {
                    fieldDescription = fieldColumn.FieldDefinition.ToString();
                }

                var message = RenameKeyAutoFillValueMessage(recordDescription, fieldDescription);

                if (!Processor.ShowYesNoMessage(message, RenameKeyAutoFillValueCaption))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// This is the message that will show when the user tries to rename a record's unique key AutoFillValue.  Checks to see if the user really wants to rename an existing record or create a new record instead.  Override this for localization.
        /// </summary>
        /// <param name="recordDescription">The record description.</param>
        /// <param name="fieldDescription">The field description.</param>
        /// <returns></returns>
        protected virtual string RenameKeyAutoFillValueMessage(string recordDescription, string fieldDescription)
        {
            var message =
                $"Are you sure you wish to rename this {recordDescription}'s unique {fieldDescription} value from {_savedKeyAutoFillValue.Text} to {KeyAutoFillValue.Text}?\r\n\r\nClick the 'New' button to create a new {recordDescription}.";
            return message;
        }

        /// <summary>
        /// Validates the entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        protected virtual bool ValidateEntity(TEntity entity)
        {
            if (!TableDefinition.ValidateEntity(entity, this))
            {
                return false;
            }
            var autoFills = Processor.GetAutoFills();
            if (autoFills != null)
            {
                foreach (var dbAutoFillMap in autoFills)
                {
                    if (dbAutoFillMap.AutoFillSetup == KeyAutoFillSetup)
                    {
                        if (dbAutoFillMap.AutoFillValue == null || KeyAutoFillValue.Text.IsNullOrEmpty())
                        {
                            var validate = true;
                            if (KeyAutoFillSetup
                                    .LookupDefinition
                                    .InitialSortColumnDefinition is LookupFieldColumnDefinition lookupFieldColumn)
                            {
                                validate = !lookupFieldColumn.FieldDefinition.GeneratedKey;
                            }

                            if (validate)
                            {
                                ProcessAutoFillValidationResponse(dbAutoFillMap);
                                return false;
                            }
                        }
                    }
                    else
                    {
                        if (!dbAutoFillMap.AutoFillValue.ValidateAutoFill(dbAutoFillMap.AutoFillSetup))
                        {
                            ProcessAutoFillValidationResponse(dbAutoFillMap);
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        private void ProcessAutoFillValidationResponse(DbAutoFillMap autoFillMap)
        {
            var caption = "Validation Fail";
            var message = string.Empty;
            if (autoFillMap.AutoFillSetup.ForeignField == null)
            {
                message = "Field has an invalid value.";
                ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
            }
            else
            {
                Processor.HandleAutoFillValFail(autoFillMap);
            }
        }

        AutoFillValue IValidationSource.GetAutoFillValueForNullableForeignKeyField(FieldDefinition fieldDefinition)
        {
            return GetAutoFillValueForNullableForeignKeyField(fieldDefinition);
        }

        /// <summary>
        /// Override this to get the auto fill value for a nullable foreign key field.  Used by validation.  If the returned value has text but not a valid PrimaryKeyValue then it will fail validation.
        /// </summary>
        /// <param name="fieldDefinition">The field definition.</param>
        /// <returns></returns>
        protected virtual AutoFillValue GetAutoFillValueForNullableForeignKeyField(FieldDefinition fieldDefinition)
        {
            return null;
        }

        public void OnValidationFail(FieldDefinition fieldDefinition, string text, string caption)
        {
            var uiMap = UiControls.FirstOrDefault(p => p.FieldDefinition == fieldDefinition);
            if (uiMap != null)
            {
                uiMap.UiCommand.SetFocus();
            }

            ControlsGlobals.UserInterface.ShowMessageBox(text, caption, RsMessageBoxIcons.Exclamation);
        }

        public virtual bool ValidateEntityProperty(FieldDefinition fieldDefinition, string valueToValidate)
        {
            return true;
        }

        private bool CheckDirty()
        {
            if (ReadOnlyMode)
                return true;

            if (!CheckDirtyFlag)
            {
                return true;
            }

            if (RecordDirty && SaveButtonEnabled)
            {
                if (MaintenanceMode == DbMaintenanceModes.AddMode && KeyAutoFillValue.IsValid())
                    return true;

                var message = SaveChangesMessage;
                var result = Processor.ShowYesNoCancelMessage(message, TableDefinition.ToString());
                OnCheckDirtyFlagMessageShown(new CheckDirtyResultArgs(result));
                switch (result)
                {
                    case MessageButtons.Yes:
                        if (DoSave() != DbMaintenanceResults.Success)
                        {
                            return false;
                        }
                        break;
                    case MessageButtons.No:
                        break;
                    case MessageButtons.Cancel:
                        return false;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            return true;
        }

        /// <summary>
        /// Called when the Delete button is clicked.
        /// </summary>
        /// <returns>
        /// The result.
        /// </returns>
        public override DbMaintenanceResults DoDelete(bool unitTestMode = false)
        {
            FireDeleteEvent();
            if (!DeleteCommand.IsEnabled)
                return DbMaintenanceResults.NotAllowed;

            if (unitTestMode)
            {
                if (!DeleteEntity())
                    return DbMaintenanceResults.DatabaseError;
                return DbMaintenanceResults.Success;
            }
            var description = TableDefinition.RecordDescription;
            if (description.IsNullOrEmpty())
                description = TableDefinition.ToString();

            var message = ConfirmDeleteMessage(description);
            if (Processor.ShowYesNoMessage(message, ConfirmDeleteCaption))
            {
                var fields = new List<FieldDefinition>();
                var deleteTables = new DeleteTables();
                foreach (var childField in TableDefinition.ChildFields)
                {
                    if (!ProcessDeleteChildField(fields, childField, deleteTables))
                    {
                        return DbMaintenanceResults.ValidationError;
                    }
                }

                if (deleteTables.Tables.Any())
                {
                    deleteTables.Context = SystemGlobals.DataRepository.GetDataContext();
                    if (!Processor.CheckDeleteTables(deleteTables))
                    {
                        ControlsGlobals.UserInterface.SetWindowCursor(WindowCursorTypes.Default);
                        return DbMaintenanceResults.ValidationError;
                    }

                    ControlsGlobals.UserInterface.SetWindowCursor(WindowCursorTypes.Wait);
                    if (!DeleteChildren(deleteTables))
                    {
                        ControlsGlobals.UserInterface.SetWindowCursor(WindowCursorTypes.Default);
                        return DbMaintenanceResults.DatabaseError;
                    }

                    if (!deleteTables.Context.Commit($"Deleting Children"))
                    {
                        return DbMaintenanceResults.DatabaseError;
                    }
                    OnDeletedRelatedTables(deleteTables);
                    ControlsGlobals.UserInterface.SetWindowCursor(WindowCursorTypes.Default);
                }
                var operationArgs = new ViewModelOperationPreviewEventArgs<TEntity>
                {
                    Operation = ViewModelOperations.Delete
                };
                OnViewModelOperationPreview(operationArgs);
                if (operationArgs.Handled)
                    return DbMaintenanceResults.NotAllowed;

                if (!DeleteEntity())
                {
                    var entity = PopulatePrimaryKeyControls(Entity, null);
                    return DbMaintenanceResults.DatabaseError;
                }

                if (LookupAddViewArgs != null && LookupAddViewArgs.CallBackToken != null)
                {
                    LookupAddViewArgs.CallBackToken.NewAutoFillValue = null;
                    LookupAddViewArgs.CallBackToken.RefreshMode = AutoFillRefreshModes.DbDelete;
                    LookupAddViewArgs.SelectedPrimaryKeyValue = null;
                    LookupAddViewArgs.CallBackToken.DeletedPrimaryKeyValue
                        = TableDefinition.GetPrimaryKeyValueFromEntity(Entity);
                    LookupAddViewArgs.CallBackToken.OnRefreshData(RefreshOperations.Delete);
                }

                RecordsChanged = true;
                RecordDirty = false;
                OnNewButton();
            }

            return DbMaintenanceResults.Success;
        }

        protected void OnDeletedRelatedTables(DeleteTables relatedTables)
        {

        }

        private bool ProcessDeleteChildField(List<FieldDefinition> fields
            , FieldDefinition childField
            , DeleteTables deleteTables
            , FieldDefinition parentField = null
            , FieldDefinition rootChild = null
            , DeleteTable parentDeleteTable = null
            , FieldDefinition prevField = null)
        {
            var origParentField = parentField;
            FieldDefinition topField = null;
            var deleteTable = new DeleteTable();
            if (!fields.Contains(childField))
            {
                //if (childField.TableDefinition.PrimaryKeyFields.Count > 1)
                //{
                //    return true;
                //}
                if (!TablesToDelete.Contains(childField.TableDefinition))
                {
                    if (childField.TableDefinition.LookupDefinition == null)
                    {
                        var message = $"{childField.TableDefinition.Description} has no Lookup Definition";
                        var caption = "Validation fail.";
                        ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Error);
                        return false;
                    }
                    fields.Add(childField);
                    var query = childField.TableDefinition.LookupDefinition.GetSelectQueryMaui();
                    query.SetMaxRecords(1);
                    //var query = new SelectQuery(childField.TableDefinition.TableName).SetMaxRecords(1);
                    if (parentField == null)
                    {
                        if (childField.ParentJoinForeignKeyDefinition != null)
                        {
                            foreach (var fieldJoin in childField.ParentJoinForeignKeyDefinition.FieldJoins)
                            {
                                var test = childField;
                                var test2 = parentField;
                                var test3 = parentDeleteTable;

                                var keyValueField =
                                    _lookupData.SelectedPrimaryKeyValue.KeyValueFields.FirstOrDefault(p =>
                                        p.FieldDefinition == fieldJoin.PrimaryField);
                                deleteTable.Column = query.AddColumn(childField);
                                if (deleteTable.Column.FieldDefinition.TableDefinition
                                        .Description == "Products")
                                {

                                }
                                query.AddFilter(deleteTable.Column, Conditions.Equals, keyValueField.Value);
                                //query.AddWhereItem(fieldJoin.ForeignField.FieldName, Conditions.Equals,
                                //    keyValueField.Value, false, fieldJoin.ForeignField.ValueType);
                            }
                        }
                    }
                    else
                    {
                        var parentTable = parentDeleteTable;
                        PrimaryJoinTable joinTable = null;
                        FieldDefinition whereField = null;
                        if (parentTable == null || parentTable.ParentDeleteTable == null)
                        {
                            //joinTable = MakeJoinTable(childField, parentField, query);
                        }
                        else
                        {
                            topField = parentField;
                            var firstField = childField;
                            while (parentTable?.ParentDeleteTable != null)
                            {
                                //joinTable = MakeJoinTable(firstField, parentField, query, joinTable);
                                firstField = parentTable.ChildField;
                                parentField = parentTable.ParentField;
                                parentTable = parentDeleteTable.ParentDeleteTable;
                                var existingTable = deleteTables.Tables.Contains(parentTable);
                                if (existingTable)
                                {
                                    break;
                                }
                            }
                            //joinTable = MakeJoinTable(topField, parentField, query, joinTable);
                        }

                        if (rootChild?.ParentJoinForeignKeyDefinition != null)
                        {
                            foreach (var fieldJoin in rootChild.ParentJoinForeignKeyDefinition.FieldJoins)
                            {
                                var keyValueField =
                                    _lookupData.SelectedPrimaryKeyValue.KeyValueFields.FirstOrDefault(p =>
                                        p.FieldDefinition == fieldJoin.PrimaryField);

                                if (whereField == null)
                                {
                                    whereField = fieldJoin.PrimaryField;
                                }

                                if (keyValueField != null)
                                {
                                    var test = childField;
                                    var test2 = parentField;
                                    var test3 = parentDeleteTable;

                                    deleteTable.Column = query.AddColumn(childField);
                                    var checkParent = parentField;
                                    if (checkParent == null)
                                    {
                                        checkParent = childField;
                                    }

                                    //if (deleteTable.Column.FieldDefinition.TableDefinition
                                    //    .Description == "Product Version Departments")
                                    if (deleteTable.Column.FieldDefinition.TableDefinition
                                            .Description == "Error Developers")
                                    {

                                    }

                                    FieldDefinition fieldFilter = null;
                                    if (parentDeleteTable != null
                                        && parentDeleteTable.Column.FieldDefinition != null)
                                    {
                                        fieldFilter = parentDeleteTable.Column.FieldDefinition;
                                    }
                                    var filter = query.AddFilter(deleteTable.Column, Conditions.Equals, keyValueField, checkParent, fieldFilter);
                                }
                                else
                                {
                                    
                                }
                                //query.AddWhereItem(joinTable, whereField.FieldName, Conditions.Equals,
                                //    keyValueField.Value, false, fieldJoin.ForeignField.ValueType);
                            }
                        }
                    }

                    //var dataResult = TableDefinition.Context.DataProcessor.GetData(query);
                    var recordCount = 0;
                    if (!query.Filter.FixedFilters.Any())
                    {
                        if (parentDeleteTable.Query != null && parentDeleteTable.Query.RecordCount() > 0)
                        {
                            var test = childField;
                            var test2 = parentField;
                            var test3 = parentDeleteTable;
                            foreach (var keyValueField in _lookupData.SelectedPrimaryKeyValue.KeyValueFields)
                            {
                                deleteTable.Column = query.AddColumn(keyValueField.FieldDefinition.TableDefinition
                                    , keyValueField.FieldDefinition);
                                if (deleteTable.Column.FieldDefinition.TableDefinition
                                        .Description == "Products")
                                {

                                }
                                
                                query.AddFilter(deleteTable.Column, Conditions.Equals
                                , keyValueField.Value);
                            }
                        }
                    }
                    if (query.Filter.FixedFilters.Any())
                    {
                        var dataResult = query.GetData();
                        if (dataResult)
                        {
                            recordCount = query.RecordCount();
                        }
                    }
                    //if (dataResult)
                    {
                        if (recordCount > 0)
                        {
                            if (!childField.TableDefinition.CanViewTable)
                            {
                                var deleteMessage =
                                    $"You are not allowed to view records in the {childField.TableDefinition.Description} table.";
                                var deleteCaption = "Delete Denied!";
                                ControlsGlobals.UserInterface.ShowMessageBox(deleteMessage, deleteCaption,
                                    RsMessageBoxIcons.Exclamation);
                                {
                                    return false;
                                }
                            }
                            if (rootChild == null)
                            {
                                rootChild = childField;
                            }
                            deleteTables.PrimaryKeyValue = _lookupData.SelectedPrimaryKeyValue;
                            deleteTable.ChildField = childField;
                            deleteTable.Parent = deleteTables;
                            deleteTable.ParentField = parentField;
                            deleteTable.RootField = rootChild;
                            deleteTable.Query = query;
                            deleteTable.ParentDeleteTable = parentDeleteTable;
                            deleteTables.Tables.Add(deleteTable);
                            if (childField.TableDefinition.Description == "Products")
                            {

                            }

                            if (!childField.AllowNulls || !childField.AllowUserNulls)
                            {
                                foreach (var tableDefinitionChildField in childField.TableDefinition.ChildFields)
                                {
                                    //if (tableDefinitionChildField.AllowRecursion && childField.AllowRecursion)
                                    {
                                        var existingField = deleteTables.Tables.FirstOrDefault(p =>
                                            p.ChildField == tableDefinitionChildField);
                                        if (existingField == null)
                                        {
                                            if (!ProcessDeleteChildField(fields, tableDefinitionChildField,
                                                    deleteTables, childField,
                                                    rootChild, deleteTable))
                                            {
                                                return false;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

            }

            if (!childField.AllowNulls || !childField.AllowUserNulls)
            {
                foreach (var tableDefinitionChildField in childField.TableDefinition.ChildFields)
                {
                    //if (tableDefinitionChildField.AllowRecursion && childField.AllowRecursion)
                    {
                        if (!ProcessDeleteChildField(fields, tableDefinitionChildField, deleteTables, childField,
                                rootChild, deleteTable, childField))
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        private static PrimaryJoinTable MakeJoinTable(FieldDefinition childField, FieldDefinition parentField,
            SelectQuery query, QueryTable parentTable = null)
        {
            if (parentTable == null)
            {
                parentTable = query.BaseTable;
            }
            
            var joinTable = query.AddPrimaryJoinTable(JoinTypes.InnerJoin, parentTable,
                parentField.TableDefinition.TableName);

            foreach (var fieldJoin in childField.ParentJoinForeignKeyDefinition.FieldJoins)
            {
                joinTable.AddJoinField(fieldJoin.PrimaryField.FieldName,
                    fieldJoin.ForeignField.FieldName);
            }

            return joinTable;
        }

        protected bool DeleteChildren(DeleteTables deleteTables)
        {
            var childFieldsProcessed = new List<FieldDefinition>();
            var sqls = new List<string>();
            var allowNullsTables = deleteTables.Tables.Where(p => p.ChildField.AllowNulls);
            foreach (var table in allowNullsTables)
            {
                if (!ProcessDeleteChildren(table, sqls, childFieldsProcessed))
                {
                    return false;
                }
            }
            var tables = deleteTables.Tables
                .OrderByDescending(p => p.ChildField.TableDefinition.PriorityLevel);

            foreach (var deleteTable in tables)
            {
                if (!ProcessDeleteChildren(deleteTable, sqls, childFieldsProcessed))
                {
                    return false;
                }
            }

            //var result = deleteTables.PrimaryKeyValue.TableDefinition.Context.DataProcessor.ExecuteSqls(sqls);
            //if (result.ResultCode != GetDataResultCodes.Success)
            //{
            //    return false;
            //}
            OnTablesDeleted(deleteTables);
            return true;
        }

        public virtual void OnTablesDeleted(DeleteTables deleteTables)
        {

        }

        private bool ProcessDeleteChildren(DeleteTable deleteTable, List<string> sqls
            , List<FieldDefinition> childFieldsProcessed)
        {
            //if (deleteTable.ChildField.TableDefinition.ChildFields.Any())
            //{
            //    foreach (var childField in deleteTable.ChildField.TableDefinition.ChildFields)
            //    {
            //        var childTable = deleteTable.Parent.Tables.FirstOrDefault(p => p.ChildField.TableDefinition == childField.TableDefinition);
            //        if (childTable != null && childTable.ChildField.AllowRecursion)
            //        {
            //            var processedChildField = childFieldsProcessed.FirstOrDefault(childField);
            //            if (processedChildField == null)
            //            {
            //                ProcessDeleteChildren(childTable, sqls, childFieldsProcessed);
            //                childTable.Processed = true;
            //                childFieldsProcessed.Add(childField);
            //            }
            //        }
            //    }
            //}
            var result = true;
            if (!deleteTable.Processed)
            {
                var sql = string.Empty;
                if (deleteTable.ChildField.AllowNulls && deleteTable.ChildField.AllowUserNulls)
                {
                    result = deleteTable.Query.SetNull(deleteTable.Column, deleteTable.Parent.Context);
                }
                else
                {
                    result = deleteTable.Query.DeleteAllData(deleteTable.Parent.Context);
                }

                sqls.Add(sql);
            }
            return result;
        }

        /// <summary>
        /// Gets the confirm delete message.  Override this for localization.
        /// </summary>
        /// <param name="recordDescription">The record description.</param>
        /// <returns></returns>
        protected virtual string ConfirmDeleteMessage(string recordDescription)
        {
            var message = $"Are you sure you wish to delete this {recordDescription}?";
            return message;
        }

        /// <summary>
        /// called when the user is trying to close the view.
        /// </summary>
        /// <param name="e">The <see cref="CancelEventArgs" /> instance containing the event data.</param>
        public override void OnWindowClosing(CancelEventArgs e)
        {
            _isActive = false;
            FireCloseEvent();
            if (!CheckDirty())
            {
                e.Cancel = true;
                return;
            }

            //if (LookupAddViewArgs != null && !_selectingRecord && RecordsChanged)
            //{
            //    if (_lookupData.SelectedPrimaryKeyValue != null && _lookupData.SelectedPrimaryKeyValue.IsValid)
            //    {
            //        LookupAddViewArgs.CallBackToken.NewAutoFillValue =
            //            TableDefinition.LookupDefinition.GetAutoFillValue(_lookupData.SelectedPrimaryKeyValue);
            //    }
            //    LookupAddViewArgs.CallBackToken.OnRefreshData();
            //}
        }

        /// <summary>
        /// Populates the primary key controls.  This is executed during record save and retrieval operations.
        /// </summary>
        /// <param name="newEntity">The entity containing just the primary key values.</param>
        /// <param name="primaryKeyValue">The primary key value.</param>
        /// <returns>An entity populated from the database.</returns>
        protected virtual TEntity PopulatePrimaryKeyControls(TEntity newEntity, PrimaryKeyValue primaryKeyValue)
        {
            var entity = newEntity.FillOutProperties();
            return entity;
        }

        public void UnitTestLoadFromEntity(TEntity entity)
        {
            MaintenanceMode = DbMaintenanceModes.EditMode;

            var primaryKeyValue = TableDefinition.GetPrimaryKeyValueFromEntity(entity);

            LoadFromEntity(PopulatePrimaryKeyControls(entity, primaryKeyValue));
            
            OnLookupDataChanged();
        }

        /// <summary>
        /// Loads this view model from the entity generated from PopulatePrimaryKeyControls.  This is executed only during record retrieval operations.
        /// </summary>
        /// <param name="entity">The entity that was loaded from the database by PopulatePrimaryKeyControls.</param>
        protected abstract void LoadFromEntity(TEntity entity);

        /// <summary>
        /// Creates and returns a LookupCommand. The primaryKeyValue argument must be passed in if this view model does not set the KeyAutoFillValue property.
        /// </summary>
        /// <param name="command">The command type.</param>
        /// <param name="primaryKeyValue">The primary key value.</param>
        /// <param name="addViewParameter">The add-on-the-fly input parameter.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">You must pass in a primary key value if KeyAutoFillValue is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">command - null</exception>
        protected LookupCommand GetLookupCommand(LookupCommands command, PrimaryKeyValue primaryKeyValue = null, object addViewParameter = null)
        {
            if (primaryKeyValue == null && KeyAutoFillValue != null)
                primaryKeyValue = KeyAutoFillValue.PrimaryKeyValue;

            switch (command)
            {
                case LookupCommands.Clear:
                case LookupCommands.AddModify:
                case LookupCommands.Reset:
                    break;
                case LookupCommands.Refresh:
                    //if (primaryKeyValue == null)
                    //    throw new ArgumentException("You must pass in a primary key value if KeyAutoFillValue is null.");
                    return new LookupCommand(command, primaryKeyValue, RecordsChanged){AddViewParameter = addViewParameter};
                default:
                    throw new ArgumentOutOfRangeException(nameof(command), command, null);
            }

            return new LookupCommand(command){AddViewParameter = addViewParameter};
        }

        /// <summary>
        /// Gets the entity data.
        /// </summary>
        /// <returns></returns>
        protected abstract TEntity GetEntityData();

        public TEntity UnitTestGetEntityData() => GetEntityData();

        /// <summary>
        /// Clears the data.
        /// </summary>
        protected abstract void ClearData();

        /// <summary>
        /// Saves the entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        protected abstract bool SaveEntity(TEntity entity);

        /// <summary>
        /// Deletes the entity.
        /// </summary>
        /// <returns></returns>
        protected abstract bool DeleteEntity();

        /// <summary>
        /// Called when the key control looses focus.  Used to ensure no duplicate value in the key control is entered.
        /// </summary>
        public override void OnKeyControlLeave()
        {
            if (KeyValueDirty && KeyAutoFillValue != null)
                SelectPrimaryKey(KeyAutoFillValue.PrimaryKeyValue);
            KeyValueDirty = false;
        }

        /// <summary>
        /// Loads a record into the view model for the PrimaryKeyValue argument.
        /// </summary>
        /// <param name="primaryKeyValue">The primary key value.</param>
        protected void SelectPrimaryKey(PrimaryKeyValue primaryKeyValue)
        {
            if (primaryKeyValue.IsValid())
                _lookupData.SelectPrimaryKey(primaryKeyValue);
        }

        /// <summary>
        /// Initializes from lookup data that is sent when the user wishes to add/view lookup data on the fly.
        /// </summary>
        /// <param name="e">The e.</param>
        public override void InitializeFromLookupData(LookupAddViewArgs e)
        {
            LookupAddViewArgs = e;

            _fromLookupFormAddView = !e.FromLookupControl;
        }

        /// <summary>
        /// Attempts top save the record if in add mode and returns that result.  Execute this method prior to sending the AddModify LookupCommand.
        /// </summary>
        /// <returns></returns>
        protected virtual DbMaintenanceResults ExecuteAddModifyCommand()
        {
            var result = DbMaintenanceResults.Success;
            if (MaintenanceMode == DbMaintenanceModes.AddMode)
            {
                var recordsChanged = RecordsChanged;
                result = DoSave();
                RecordsChanged = recordsChanged;
            }

            return result;
        }

        protected virtual void OnViewModelOperationPreview(ViewModelOperationPreviewEventArgs<TEntity> e)
        {
            ViewModelOperationPreview?.Invoke(this, e);
        }

        protected override void OnRecordDirtyChanged(bool newValue)
        {
            if (newValue)
            {
                _startDate = DateTime.Now;
            }
            base.OnRecordDirtyChanged(newValue);
        }

        /// <summary>
        /// Called when a property value has changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="raiseDirtyFlag">if set to <c>true</c> set RecordDirty property to true indicating the user has changed data.</param>
        protected override void OnPropertyChanged(string propertyName = null, bool raiseDirtyFlag = true)
        {
            if (raiseDirtyFlag)
                RecordDirty = true;
            base.OnPropertyChanged(propertyName, raiseDirtyFlag);
        }
    }
}
