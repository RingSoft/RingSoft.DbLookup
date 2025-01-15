// ***********************************************************************
// Assembly         : RingSoft.DbMaintenance
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 09-03-2024
// ***********************************************************************
// <copyright file="DbMaintenanceViewModel.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
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
using System.Diagnostics.Metrics;

namespace RingSoft.DbMaintenance
{
    /// <summary>
    /// Enum ViewModelOperations
    /// </summary>
    public enum ViewModelOperations
    {
        /// <summary>
        /// The save
        /// </summary>
        Save = 0,
        /// <summary>
        /// The delete
        /// </summary>
        Delete = 1
    }

    /// <summary>
    /// Class ViewModelOperationPreviewEventArgs.
    /// </summary>
    /// <typeparam name="TEntity">The type of the t entity.</typeparam>
    public class ViewModelOperationPreviewEventArgs<TEntity> where TEntity : new()
    {
        /// <summary>
        /// Gets the entity.
        /// </summary>
        /// <value>The entity.</value>
        public TEntity Entity { get; internal set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ViewModelOperationPreviewEventArgs{TEntity}" /> is handled.
        /// </summary>
        /// <value><c>true</c> if handled; otherwise, <c>false</c>.</value>
        public bool Handled { get; set; }

        /// <summary>
        /// Gets the operation.
        /// </summary>
        /// <value>The operation.</value>
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
        /// <summary>
        /// Gets the entity.
        /// </summary>
        /// <value>The entity.</value>
        public TEntity Entity { get; private set; }

        /// <summary>
        /// Gets the number of rows on a page.
        /// </summary>
        /// <value>The number of rows on the page.</value>
        public int PageSize { get; } = 1;
        /// <summary>
        /// Gets the type of the search.
        /// </summary>
        /// <value>The type of the search.</value>
        public LookupSearchTypes SearchType { get; } = LookupSearchTypes.Equals;
        /// <summary>
        /// Gets or sets the search text.
        /// </summary>
        /// <value>The search text.</value>
        public string SearchText { get; set; } = string.Empty;
        /// <summary>
        /// Gets the index of the selected.
        /// </summary>
        /// <value>The index of the selected.</value>
        public int SelectedIndex => 0;
        /// <summary>
        /// Sets the index of the lookup.
        /// </summary>
        /// <param name="index">The index.</param>
        public void SetLookupIndex(int index)
        {
        }

        /// <summary>
        /// Gets the table definition.
        /// </summary>
        /// <value>The table definition.</value>
        public virtual TableDefinition<TEntity> TableDefinition { get; }

        /// <summary>
        /// Gets the table definition.
        /// </summary>
        /// <value>The table definition.</value>
        public override sealed TableDefinitionBase TableDefinitionBase => TableDefinition;

        /// <summary>
        /// Gets the initial search for text when the Find button is clicked.  By default it is the key auto fill text.
        /// </summary>
        /// <value>The find button initial search for.</value>
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
        /// <value><c>true</c> if records changed; otherwise, <c>false</c>.</value>
        public bool RecordsChanged { get; private set; }


        /// <summary>
        /// Gets a value indicating whether the base entity is loading from the database or is being cleared.
        /// </summary>
        /// <value><c>true</c> if loading from the database or clearing; otherwise, <c>false</c>.</value>
        protected bool ChangingEntity { get; private set; }

        /// <summary>
        /// Gets the rename key auto fill value message caption.  Override this for localization.
        /// </summary>
        /// <value>The rename key auto fill value caption.  Override this for localization.</value>
        protected virtual string RenameKeyAutoFillValueCaption => "Change Unique Field Value";

        /// <summary>
        /// Gets the save changes message.  Override this for localization.
        /// </summary>
        /// <value>The save changes message.</value>
        protected virtual string SaveChangesMessage => "Do you wish to save your changes to this data?";

        /// <summary>
        /// Gets the confirm delete caption.  Override this for localization.
        /// </summary>
        /// <value>The confirm delete caption.</value>
        protected virtual string ConfirmDeleteCaption => "Confirm Delete";

        /// <summary>
        /// Gets the tables to delete.
        /// </summary>
        /// <value>The tables to delete.</value>
        protected virtual List<TableDefinitionBase> TablesToDelete { get; } = new List<TableDefinitionBase>();

        /// <summary>
        /// Occurs when [view model operation preview].
        /// </summary>
        public event EventHandler<ViewModelOperationPreviewEventArgs<TEntity>> ViewModelOperationPreview;

        /// <summary>
        /// Gets a value indicating whether [validate all at once].
        /// </summary>
        /// <value><c>true</c> if [validate all at once]; otherwise, <c>false</c>.</value>
        public bool ValidateAllAtOnce { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [show confirm delete q].
        /// </summary>
        /// <value><c>true</c> if [show confirm delete q]; otherwise, <c>false</c>.</value>
        protected bool ShowConfirmDeleteQ { get; set; } = true;

        /// <summary>
        /// The lookup data
        /// </summary>
        private LookupDataMauiBase _lookupData;
        /// <summary>
        /// From lookup form add view
        /// </summary>
        private bool _fromLookupFormAddView;
        /// <summary>
        /// The saved key automatic fill value
        /// </summary>
        private AutoFillValue _savedKeyAutoFillValue;
        /// <summary>
        /// The selecting record
        /// </summary>
        private bool _selectingRecord;
        /// <summary>
        /// The saving record
        /// </summary>
        private bool _savingRecord;
        /// <summary>
        /// The lookup read only mode
        /// </summary>
        private bool _lookupReadOnlyMode;
        /// <summary>
        /// The read only automatic fill value
        /// </summary>
        private AutoFillValue _readOnlyAutoFillValue;
        /// <summary>
        /// The start date
        /// </summary>
        private DateTime? _startDate;
        /// <summary>
        /// The timer
        /// </summary>
        private System.Timers.Timer _timer = new Timer(1000);
        /// <summary>
        /// The is active
        /// </summary>
        private bool _isActive = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbMaintenanceViewModel{TEntity}" /> class.
        /// </summary>
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

                //Peter Ringering - 01/14/2025 07:39:16 PM - E-118
                if (minutes < 10 && RecordDirty)
                {
                    Processor.SetPendingSaveStatus("Pending Save");
                }
                else if (minutes > 10 && minutes < 20 && RecordDirty)
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
        /// <summary>
        /// Setups the view lookup definition.
        /// </summary>
        /// <param name="lookupDefinition">The lookup definition.</param>
        protected virtual void SetupViewLookupDefinition( LookupDefinitionBase lookupDefinition)
        {

        }
        /// <summary>
        /// Internals the initialize.
        /// </summary>
        /// <exception cref="System.ArgumentException">Table definition '{TableDefinition}' does not have a lookup definition setup.</exception>
        protected internal override void InternalInitialize()
        {
            SystemGlobals.LookupContext.Initialize();
            if (TableDefinition.LookupDefinition == null)
                throw new ArgumentException(
                    $"Table definition '{TableDefinition}' does not have a lookup definition setup.");

            var tableLookupDefinition = TableDefinition.LookupDefinition.Clone();
            Setup(tableLookupDefinition);
            SetupViewLookupDefinition(ViewLookupDefinition);
            _lookupData = TableDefinition.LookupDefinition.GetLookupDataMaui(ViewLookupDefinition, true);
            _lookupData.DbMaintenanceMode = true;
            _lookupData.SetParentControls(this);

            //_lookupData.LookupDataChanged += _lookupData_LookupDataChanged;
            _lookupData.LookupDataChanged += _lookupData_LookupDataChanged1;
            FindButtonLookupDefinition = ViewLookupDefinition;

            base.InternalInitialize();

            if (LookupAddViewArgs != null)
            {
                if (LookupAddViewArgs.LookupReadOnlyMode)
                {
                    AllowNew = false;
                    NewButtonEnabled = false;
                }
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
            if (!TableDefinition.HasRight(RightTypes.AllowEdit))
            {
                AllowSave = false;
                ReadOnlyMode = true;
            }

            if (!TableDefinition.HasRight(RightTypes.AllowAdd))
            {
                AllowNew = false;
                if (MaintenanceMode == DbMaintenanceModes.AddMode)
                {
                    OnGotoNextButton();
                }
            }

            if (!TableDefinition.HasRight(RightTypes.AllowDelete))
            {
                AllowDelete = false;
            }

            FireInitializeEvent();
        }

        /// <summary>
        /// Lookups the data lookup data changed1.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
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

                PopulatePrimaryKeyControls(newEntity, _lookupData.SelectedPrimaryKeyValue);
                Entity = GetEntityFromDb(newEntity, _lookupData.SelectedPrimaryKeyValue);
                if (Entity == null)
                {
                    DbDataProcessor.UserInterface.PlaySystemSound(RsMessageBoxIcons.Exclamation);
                    ControlsGlobals.UserInterface.SetWindowCursor(WindowCursorTypes.Default);
                    ChangingEntity = false;
                    return;
                }
                foreach (var lookupMap in Lookups)
                {
                    lookupMap.LookupDefinition.FilterLookup(Entity, lookupMap.AddViewParameter);
                }
                if (!_savingRecord)
                {
                    Entity.UtFillOutEntity();

                    if (KeyAutoFillSetup != null)
                    {
                        KeyAutoFillValue = Entity.GetAutoFillValue();
                    }
                    LoadFromEntity(Entity);
                    foreach (var dataEntryGridManagerBase in Grids)
                    {
                        dataEntryGridManagerBase.Grid.LoadGridFromHeaderEntity(Entity);
                    }

                    Processor?.OnRecordSelected();
                    FireRecordSelectedEvent();
                }
                ControlsGlobals.UserInterface.SetWindowCursor(WindowCursorTypes.Default);
                ChangingEntity = false;

                OnLookupDataChanged();
            }
        }

        /// <summary>
        /// Gets the add view filter.
        /// </summary>
        /// <returns>TableFilterDefinitionBase.</returns>
        protected virtual TableFilterDefinitionBase GetAddViewFilter()
        {
            if (LookupAddViewArgs != null && LookupAddViewArgs.LookupData.LookupDefinition.TableDefinition == TableDefinition)
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
            var origCursor = ControlsGlobals.UserInterface.GetWindowCursor();
            ControlsGlobals.UserInterface.SetWindowCursor(WindowCursorTypes.Wait);

            if (LookupAddViewArgs != null)
            {
                PrimaryKeyValue primaryKeyValue = null;
                if (LookupAddViewArgs.LookupData != null)
                {
                    
                    primaryKeyValue = LookupAddViewArgs.LookupData.SelectedPrimaryKeyValue;
                    if (LookupAddViewArgs
                            .LookupData
                            .SelectedPrimaryKeyValue != null)
                    {
                        primaryKeyValue =
                            GetAddViewPrimaryKeyValue(LookupAddViewArgs.LookupData.SelectedPrimaryKeyValue);
                    }
                }

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

                NewButtonEnabled = SaveButtonEnabled = LookupAddViewArgs.AllowEdit && !ReadOnlyMode;

                if (primaryKeyValue != null && primaryKeyValue.IsValid())
                    _lookupData.SelectPrimaryKey(primaryKeyValue);

                if (LookupAddViewArgs.LookupData != null)
                {
                    if (LookupAddViewArgs.LookupData.SelectedPrimaryKeyValue != null)
                    {
                        var gridTable = LookupAddViewArgs.LookupData.SelectedPrimaryKeyValue.TableDefinition;
                        if (Grids != null)
                        {
                            var grid = Grids.FirstOrDefault(
                                p => p.Grid.TableDefinition == gridTable);
                            if (grid != null)
                            {
                                grid.Grid.SelectGridRow(LookupAddViewArgs.LookupData.SelectedPrimaryKeyValue);
                            }
                        }
                    }
                }

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

            ControlsGlobals.UserInterface.SetWindowCursor(origCursor);
        }

        /// <summary>
        /// Gets the add view primary key value.
        /// </summary>
        /// <param name="addViewPrimaryKeyValue">The add view primary key value.</param>
        /// <returns>PrimaryKeyValue.</returns>
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
                        var val = selectQuery.GetPropertyValue(0, column.FieldDefinition.PropertyName);
                        var newPrimaryKey = new PrimaryKeyValue(TableDefinition);
                        newPrimaryKey.LoadFromIdValue(val);
                        selectedPrimaryKeyValue = newPrimaryKey;
                    }
                }

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
        /// <exception cref="System.ArgumentException">Column {column.Caption} is not found in this view model's table lookup definition.</exception>
        protected void ChangeSortColumn(LookupColumnDefinitionBase column)
        {
            var columnIndex = _lookupData.LookupDefinition.GetIndexOfVisibleColumn(column);
            if (columnIndex < 0)
                throw new ArgumentException($"Column {column.Caption} is not found in this view model's table lookup definition.");

            //_lookupData.OnColumnClick(columnIndex, true);
            _lookupData.OnColumnClick((LookupFieldColumnDefinition)column, true);
        }

        /// <summary>
        /// Lookups the data lookup data changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
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

                PopulatePrimaryKeyControls(newEntity, _lookupData.SelectedPrimaryKeyValue);
                Entity = GetEntityFromDb(newEntity, _lookupData.SelectedPrimaryKeyValue);
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

        /// <summary>
        /// Called when [lookup data changed].
        /// </summary>
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

        /// <summary>
        /// Datas the exists.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
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

        /// <summary>
        /// Called when [record selected].
        /// </summary>
        /// <param name="e">The e.</param>
        public override void OnRecordSelected(LookupSelectArgs e)
        {
            OnRecordSelected(e.LookupData.SelectedPrimaryKeyValue);
        }

        /// <summary>
        /// Called when [record selected].
        /// </summary>
        /// <param name="primaryKey">The primary key.</param>
        public override void OnRecordSelected(PrimaryKeyValue primaryKey)
        {
            if (!CheckDirty())
                return;

            _lookupData.SelectPrimaryKey(primaryKey);
        }

        /// <summary>
        /// Called when [record selected].
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void OnRecordSelected(TEntity entity)
        {
            var primaryKey = TableDefinition.GetPrimaryKeyValueFromEntity(entity);
            OnRecordSelected(primaryKey);
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
            foreach (var lookupMap in Lookups)
            {
                lookupMap.LookupDefinition.SetCommand(new LookupCommand(LookupCommands.Clear));
            }

            foreach (var grid in Grids)
            {
                grid.Grid.SetupForNewRecord();
            }
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
        /// <returns>The result.</returns>
        /// <exception cref="System.Exception">Processor is null</exception>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public override DbMaintenanceResults DoSave()
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

            //Peter Ringering - 01/14/2025 01:23:57 PM - E-109
            ControlsGlobals.UserInterface.SetWindowCursor(WindowCursorTypes.Wait);
            var entity = GetEntityData();
            ControlsGlobals.UserInterface.SetWindowCursor(WindowCursorTypes.Default);

            var children = TableDefinition
                .FieldDefinitions
                .Where(p => p.AllowNulls
                    && p.ParentJoinForeignKeyDefinition != null
                            && p.ParentJoinForeignKeyDefinition.PrimaryTable.IsIdentity());

            foreach (var fieldDefinition in children)
            {
                var value = GblMethods.GetPropertyValue(entity, fieldDefinition.PropertyName).ToInt();
                if (value == 0)
                {
                    GblMethods.SetPropertyObject(entity, fieldDefinition.PropertyName, null);
                }
            }

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

            //if (!unitTestMode)
            {
                var recordLockPrimaryKey = TableDefinition.GetPrimaryKeyValueFromEntity(entity);
                var keyString = recordLockPrimaryKey.KeyString;

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
                        {

                            if (recordLock != null)
                            {
                                var message =
                                        $"You started editing this record on {LockDate.ToString("dddd, MMM dd yyyy")} at {LockDate.ToString("h:mm:ss tt")}.";
                                message +=
                                    "  This record was saved by someone else while you were editing.  Do you wish to continue saving?";
                                if (!Processor.ShowRecordLockWindow(SystemGlobals.AdvancedFindLookupContext.RecordLocks.GetPrimaryKeyValueFromEntity(recordLock), message, InputParameter))
                                {
                                    return DbMaintenanceResults.ValidationError;
                                }
                            }
                        }

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            CheckSaveDeleted(entity);

            //Peter Ringering - 01/14/2025 01:27:00 PM - E-109
            ControlsGlobals.UserInterface.SetWindowCursor(WindowCursorTypes.Wait);
            if (!SaveEntity(entity))
            {
                ControlsGlobals.UserInterface.SetWindowCursor(WindowCursorTypes.Default);
                return DbMaintenanceResults.DatabaseError;
            }
            ControlsGlobals.UserInterface.SetWindowCursor(WindowCursorTypes.Default);

            //Peter Ringering - 11/23/2024 12:17:07 PM - E-76
            if (KeyAutoFillSetup != null)
            {
                KeyAutoFillValue = entity.GetAutoFillValue();
            }
            FireSavedEvent();

            var primaryKey = TableDefinition.GetPrimaryKeyValueFromEntity(entity);

            //if (!unitTestMode)
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
            }

            _savingRecord = true;

            //Peter Ringering - 01/14/2025 01:42:10 PM - E-109
            ControlsGlobals.UserInterface.SetWindowCursor(WindowCursorTypes.Wait);
            _lookupData.SelectPrimaryKey(primaryKey);
            ControlsGlobals.UserInterface.SetWindowCursor(WindowCursorTypes.Default);

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

        /// <summary>
        /// Gets the last saved date.
        /// </summary>
        /// <param name="primaryKey">The primary key.</param>
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
                        LastSavedDate = null;
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

        /// <summary>
        /// Gets the update lock SQL.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="sqlGenerator">The SQL generator.</param>
        /// <param name="tableField">The table field.</param>
        /// <param name="pkField">The pk field.</param>
        /// <param name="keyString">The key string.</param>
        /// <param name="dateField">The date field.</param>
        /// <param name="userField">The user field.</param>
        /// <returns>System.String.</returns>
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

        /// <summary>
        /// Gets the now date text.
        /// </summary>
        /// <returns>System.String.</returns>
        private static string GetNowDateText()
        {
            var newDate = DateTime.Now.ToUniversalTime();
            var dateText = newDate.ToString("yyyy-MM-dd HH:mm:ss.fff");
            return dateText;
        }

        /// <summary>
        /// Checks the key value text changed.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
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
        /// <returns>System.String.</returns>
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
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
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

            //Peter Ringering - 01/06/2025 05:59:59 PM - E-96
            var descColumn = KeyAutoFillSetup
                .LookupDefinition
                .TableDefinition
                .LookupDefinition
                .InitialSortColumnDefinition;

            var keyFilterData = GetKeyFilterData();
            var keyChanged = false;
            if (keyFilterData != null)
            {
                keyChanged = keyFilterData.KeyFilterChanged;
            }

            if ((!KeyAutoFillValue.IsValid() || keyChanged) && KeyAutoFillSetup != null)
            {
                var filter = new TableFilterDefinition<TEntity>(TableDefinition);

                if (descColumn != null)
                {
                    if (descColumn is LookupFieldColumnDefinition descFieldColumn)
                    {
                        var context = SystemGlobals.DataRepository.GetDataContext();
                        var table = context.GetTable<TEntity>();

                        if (keyFilterData != null)
                        {
                            foreach (var keyFilterItem in keyFilterData.KeyFilters)
                            {
                                filter.AddFixedFieldFilter(keyFilterItem.FieldDefinition
                                    , keyFilterItem.Condition
                                    , keyFilterItem.Value);
                            }
                        }

                        filter.AddFixedFieldFilter(descFieldColumn.FieldDefinition
                            , Conditions.Equals
                            , KeyAutoFillValue.Text);

                        var param = GblMethods.GetParameterExpression<TEntity>();
                        var expr = filter.GetWhereExpresssion<TEntity>(param);

                        if (expr != null)
                        {
                            var query = FilterItemDefinition.FilterQuery(table, param, expr);

                            if (query.Any())
                            {
                                var message =
                                    $"The Key Value '{KeyAutoFillValue.Text}' already exists in the database.  Please enter a different value";
                                var caption = "Invalid Key Value";
                                KeyAutoFillUiCommand.SetFocus();
                                ControlsGlobals.UserInterface.ShowMessageBox(message, caption,
                                    RsMessageBoxIcons.Exclamation);
                                return false;
                            }
                        }
                    }
                }
            }

            foreach (var grid in Grids)
            {
                if (!grid.ReadOnly)
                {
                    if (!grid.Grid.ValidateGrid())
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Processes the automatic fill validation response.
        /// </summary>
        /// <param name="autoFillMap">The automatic fill map.</param>
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

        /// <summary>
        /// Gets the automatic fill value for nullable foreign key field.
        /// </summary>
        /// <param name="fieldDefinition">The field definition.</param>
        /// <returns>AutoFillValue.</returns>
        AutoFillValue IValidationSource.GetAutoFillValueForNullableForeignKeyField(FieldDefinition fieldDefinition)
        {
            return GetAutoFillValueForNullableForeignKeyField(fieldDefinition);
        }

        /// <summary>
        /// Override this to get the auto fill value for a nullable foreign key field.  Used by validation.  If the returned value has text but not a valid PrimaryKeyValue then it will fail validation.
        /// </summary>
        /// <param name="fieldDefinition">The field definition.</param>
        /// <returns>AutoFillValue.</returns>
        protected virtual AutoFillValue GetAutoFillValueForNullableForeignKeyField(FieldDefinition fieldDefinition)
        {
            return null;
        }

        /// <summary>
        /// Called when [validation fail].
        /// </summary>
        /// <param name="fieldDefinition">The field definition.</param>
        /// <param name="text">The text.</param>
        /// <param name="caption">The caption.</param>
        public void OnValidationFail(FieldDefinition fieldDefinition, string text, string caption)
        {
            var uiMap = UiControls.FirstOrDefault(p => p.FieldDefinition == fieldDefinition);
            if (uiMap != null)
            {
                uiMap.UiCommand.SetFocus();
            }

            ControlsGlobals.UserInterface.ShowMessageBox(text, caption, RsMessageBoxIcons.Exclamation);
        }

        /// <summary>
        /// Validates the entity property.
        /// </summary>
        /// <param name="fieldDefinition">The field definition.</param>
        /// <param name="valueToValidate">The value to validate.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public virtual bool ValidateEntityProperty(FieldDefinition fieldDefinition, string valueToValidate)
        {
            return true;
        }

        /// <summary>
        /// Checks the dirty.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        protected bool CheckDirty()
        {
            if (ReadOnlyMode)
                return true;

            if (!RecordDirty)
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
                        RecordDirty = false;
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
        /// Shows the confirm delete message.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool ShowConfirmDeleteMessage()
        {
            var description = TableDefinition.RecordDescription;
            if (description.IsNullOrEmpty())
                description = TableDefinition.ToString();

            var message = ConfirmDeleteMessage(description);
            if (Processor.ShowYesNoMessage(message, ConfirmDeleteCaption))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Called when [pre delete children].
        /// </summary>
        /// <returns>DbMaintenanceResults.</returns>
        protected virtual DbMaintenanceResults OnPreDeleteChildren()
        {
            return DbMaintenanceResults.Success;
        }

        /// <summary>
        /// Called when the Delete button is clicked.
        /// </summary>
        /// <returns>The result.</returns>
        protected override DbMaintenanceResults DoDelete()
        {
            FireDeleteEvent();
            if (!DeleteCommand.IsEnabled)
                return DbMaintenanceResults.NotAllowed;

            //if (unitTestMode)
            //{
            //    if (!DeleteEntity())
            //        return DbMaintenanceResults.DatabaseError;
            //    return DbMaintenanceResults.Success;
            //}
            //var description = TableDefinition.RecordDescription;
            //if (description.IsNullOrEmpty())
            //    description = TableDefinition.ToString();

            //var message = ConfirmDeleteMessage(description);
            var goOn = true;
            if (ShowConfirmDeleteQ)
            {
                goOn = ShowConfirmDeleteMessage();
                if (!goOn)
                {
                    ShowConfirmDeleteQ = true;
                    return DbMaintenanceResults.ValidationError;
                }
            }
            ShowConfirmDeleteQ = true;
            if (goOn)
            {
                var fields = new List<FieldDefinition>();
                var deleteTables = new DeleteTables();

                Processor.GetPreDeleteProcedure(fields, deleteTables);
                if (!Processor.PreDeleteResult)
                {
                    return DbMaintenanceResults.ValidationError;
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
                    var preResult = OnPreDeleteChildren();
                    if (preResult != DbMaintenanceResults.Success)
                    {
                        return preResult;
                    }

                    var procedure = Processor.GetDeleteProcedure(deleteTables);
                    if (!Processor.DeleteChildrenResult)
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
                    var entity = GetEntityFromDb(Entity, null);
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

        /// <summary>
        /// Does the get delete tables.
        /// </summary>
        /// <param name="fields">The fields.</param>
        /// <param name="deleteTables">The delete tables.</param>
        /// <param name="procedure">The procedure.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public override bool DoGetDeleteTables(
            List<FieldDefinition> fields
            , DeleteTables deleteTables
            , ITwoTierProcessingProcedure procedure)
        {
            var total = TableDefinition.ChildFields.Count;
            var index = 0;
            foreach (var childField in TableDefinition.ChildFields)
            {
                index++;
                var text = $"Processing Field {childField.Description} {index} / {total}";
                procedure.SetProgress(total
                , index
                , text
                , 100
                , 100);
                if (!ProcessDeleteChildField(fields, childField, deleteTables))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Called when [deleted related tables].
        /// </summary>
        /// <param name="relatedTables">The related tables.</param>
        protected void OnDeletedRelatedTables(DeleteTables relatedTables)
        {

        }

        /// <summary>
        /// Processes the delete child field.
        /// </summary>
        /// <param name="fields">The fields.</param>
        /// <param name="childField">The child field.</param>
        /// <param name="deleteTables">The delete tables.</param>
        /// <param name="parentField">The parent field.</param>
        /// <param name="rootChild">The root child.</param>
        /// <param name="parentDeleteTable">The parent delete table.</param>
        /// <param name="prevField">The previous field.</param>
        /// <param name="parentQueryHasData">if set to <c>true</c> [parent query has data].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool ProcessDeleteChildField(List<FieldDefinition> fields
            , FieldDefinition childField
            , DeleteTables deleteTables
            , FieldDefinition parentField = null
            , FieldDefinition rootChild = null
            , DeleteTable parentDeleteTable = null
            , FieldDefinition prevField = null
            , bool parentQueryHasData = false)
        {
            var origParentField = parentField;
            FieldDefinition topField = null;
            var deleteTable = new DeleteTable();
            if (childField.TableDefinition.Description == "Order Detail")
            {

            }

            if (!fields.Contains(childField) || parentQueryHasData)
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

                    fields.Add(childField); //09/02/2024
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
                            parentQueryHasData = true;
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
                                                    rootChild, deleteTable, null, parentQueryHasData))
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
                                rootChild, deleteTable, childField, parentQueryHasData))
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Makes the join table.
        /// </summary>
        /// <param name="childField">The child field.</param>
        /// <param name="parentField">The parent field.</param>
        /// <param name="query">The query.</param>
        /// <param name="parentTable">The parent table.</param>
        /// <returns>PrimaryJoinTable.</returns>
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

        /// <summary>
        /// Deletes the children.
        /// </summary>
        /// <param name="deleteTables">The delete tables.</param>
        /// <param name="procedure">The procedure.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public override bool DeleteChildren(DeleteTables deleteTables, ITwoTierProcessingProcedure procedure)
        {
            var total = deleteTables.Tables.Count;
            var topIndex = 0;
            var childFieldsProcessed = new List<FieldDefinition>();
            var sqls = new List<string>();
            var allowNullsTables = deleteTables.Tables.Where(p => p.ChildField.AllowNulls);
            foreach (var table in allowNullsTables)
            {
                topIndex++;
                var topText = $"Processing Table {table.ChildField.TableDefinition.Description} {topIndex} / {total}";
                procedure.SetProgress(total, topIndex, topText);
                if (!ProcessDeleteChildren(table, sqls, childFieldsProcessed, procedure))
                {
                    return false;
                }
            }
            var tables = deleteTables.Tables
                .OrderByDescending(p => p.ChildField.TableDefinition.PriorityLevel);

            foreach (var deleteTable in tables)
            {
                topIndex++;
                var topText = $"Processing Table {deleteTable.ChildField.TableDefinition.Description} {topIndex} / {total}";
                procedure.SetProgress(total, topIndex, topText);
                if (!ProcessDeleteChildren(deleteTable, sqls, childFieldsProcessed, procedure))
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

        /// <summary>
        /// Called when [tables deleted].
        /// </summary>
        /// <param name="deleteTables">The delete tables.</param>
        public virtual void OnTablesDeleted(DeleteTables deleteTables)
        {

        }

        /// <summary>
        /// Processes the delete children.
        /// </summary>
        /// <param name="deleteTable">The delete table.</param>
        /// <param name="sqls">The SQLS.</param>
        /// <param name="childFieldsProcessed">The child fields processed.</param>
        /// <param name="procedure">The procedure.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool ProcessDeleteChildren(DeleteTable deleteTable, List<string> sqls
            , List<FieldDefinition> childFieldsProcessed, ITwoTierProcessingProcedure procedure)
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
                    result = deleteTable.Query.SetNull(deleteTable.Column, deleteTable.Parent.Context
                    , procedure);
                }
                else
                {
                    result = deleteTable.Query.DeleteAllData(deleteTable.Parent.Context, procedure);
                }

                sqls.Add(sql);
            }
            return result;
        }

        /// <summary>
        /// Gets the confirm delete message.  Override this for localization.
        /// </summary>
        /// <param name="recordDescription">The record description.</param>
        /// <returns>System.String.</returns>
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
        protected abstract void PopulatePrimaryKeyControls(TEntity newEntity, PrimaryKeyValue primaryKeyValue);

        /// <summary>
        /// Gets the entity from database.
        /// </summary>
        /// <param name="newEntity">The new entity.</param>
        /// <param name="primaryKeyValue">The primary key value.</param>
        /// <returns>TEntity.</returns>
        protected virtual TEntity GetEntityFromDb(TEntity newEntity, PrimaryKeyValue primaryKeyValue)
        {
            var gridTables = new List<TableDefinitionBase>();
            foreach (var gridMap in Grids)
            {
                gridTables.Add(gridMap.Grid.TableDefinition);
            }
            var entity = newEntity.FillOutProperties(gridTables);

            return entity;
        }

        /// <summary>
        /// Units the test load from entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void UnitTestLoadFromEntity(TEntity entity)
        {
            MaintenanceMode = DbMaintenanceModes.EditMode;

            var primaryKeyValue = TableDefinition.GetPrimaryKeyValueFromEntity(entity);

            LoadFromEntity(GetEntityFromDb(entity, primaryKeyValue));

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
        /// <returns>LookupCommand.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">command - null</exception>
        /// <exception cref="ArgumentException">command - null</exception>
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
        /// <returns>TEntity.</returns>
        protected abstract TEntity GetEntityData();

        /// <summary>
        /// Units the test get entity data.
        /// </summary>
        /// <returns>TEntity.</returns>
        public TEntity UnitTestGetEntityData() => GetEntityData();

        /// <summary>
        /// Clears the data.
        /// </summary>
        protected abstract void ClearData();

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
        /// <returns>DbMaintenanceResults.</returns>
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

        /// <summary>
        /// Called when [view model operation preview].
        /// </summary>
        /// <param name="e">The e.</param>
        protected virtual void OnViewModelOperationPreview(ViewModelOperationPreviewEventArgs<TEntity> e)
        {
            ViewModelOperationPreview?.Invoke(this, e);
        }

        /// <summary>
        /// Called when [record dirty changed].
        /// </summary>
        /// <param name="newValue">if set to <c>true</c> [new value].</param>
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

        /// <summary>
        /// Checks the save deleted.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void CheckSaveDeleted(TEntity entity)
        {
            if (TableDefinition.IsIdentity())
            {
                var identityVal = TableDefinition.GetIdentityValue(entity);
                if (identityVal > 0 && MaintenanceMode == DbMaintenanceModes.EditMode)
                {
                    var existEntity = entity.FillOutProperties(false);
                    if (existEntity == null)
                    {
                        GblMethods.SetPropertyValue(entity
                        , TableDefinition
                            .GetIdentityField()
                            .PropertyName, "0");
                    }
                }
            }
        }

        /// <summary>
        /// Generates the key value.
        /// </summary>
        /// <param name="prefix">The prefix.</param>
        /// <param name="entity">The entity.</param>
        protected void GenerateKeyValue(string prefix, TEntity entity)
        {
            var cont = true;
            if (KeyAutoFillValue.IsValid() 
                || !TableDefinition.IsIdentity())
            {
                cont = false;
            }

            if (cont)
            {
                if (KeyAutoFillValue != null)
                {
                    if (!KeyAutoFillValue.Text.IsNullOrEmpty())
                    {
                        cont = false;
                    }
                }
            }

            if (!cont)
            {
                var context = SystemGlobals.DataRepository.GetDataContext();
                context.SaveEntity(entity, "Saving Key");
                return;
            }
            var sortColumn = TableDefinition.LookupDefinition.InitialSortColumnDefinition;
            if (sortColumn is LookupFieldColumnDefinition sortFieldColumn)
            {
                var context = SystemGlobals.DataRepository.GetDataContext();
                GblMethods.SetPropertyValue(
                    entity
                    , sortFieldColumn.FieldDefinition.PropertyName
                    , Guid.NewGuid().ToString());

                var result = context.SaveEntity(entity, "Saving Key");

                if (result)
                {
                    var identValue = GblMethods.GetPropertyValue(entity
                        , TableDefinition.GetIdentityField().PropertyName);

                    identValue = $"{prefix}-{identValue}";
                    GblMethods.SetPropertyValue(
                        entity
                        , sortFieldColumn.FieldDefinition.PropertyName
                        , identValue);

                    result = context.SaveEntity(entity, "Updating Key Value");
                    if (result)
                    {
                        KeyAutoFillValue = entity.GetAutoFillValue();
                    }
                }
            }
        }

        /// <summary>
        /// Saves the entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        protected virtual bool SaveEntity(TEntity entity)
        {
            var context = SystemGlobals.DataRepository.GetDataContext();
            var result = SaveHeader(entity, context);

            if (!result)
            {
                return result;
            }
            foreach (var grid in Grids)
            {
                if (!grid.ReadOnly)
                {
                    grid.Grid.SaveNoCommitData(entity, context);
                    result = context.Commit("Saving Details");
                }
            }

            return result;

        }

        /// <summary>
        /// Saves the header.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="context">The context.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        protected bool SaveHeader(TEntity entity, IDbContext context)
        {
            if (!TableDefinition.IsIdentity())
            {
                var table = context.GetTable<TEntity>();
                var filter = new TableFilterDefinition<TEntity>(TableDefinition);
                foreach (var primaryKeyField in TableDefinition.PrimaryKeyFields)
                {
                    var value = GblMethods.GetPropertyValue(entity, primaryKeyField.PropertyName);
                    filter.AddFixedFieldFilter(primaryKeyField, Conditions.Equals, value);
                }

                var param = GblMethods.GetParameterExpression<TEntity>();
                var expr = filter.GetWhereExpresssion<TEntity>(param);
                var query = FieldFilterDefinition.FilterQuery(table, param, expr);
                var tableEntity = query.FirstOrDefault();
                if (tableEntity == null)
                {
                    return context.AddSaveEntity(entity, $"Saving {TableDefinition.Description}");
                }
                else
                {
                    context = SystemGlobals.DataRepository.GetDataContext();
                    return context.SaveEntity(entity, $"Saving {TableDefinition.Description}");
                }

            }
            var result = context.SaveEntity(entity, $"Saving {TableDefinition.Description}");
            return result;
        }

        /// <summary>
        /// Deletes the entity.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        protected virtual bool DeleteEntity()
        {
            var context = SystemGlobals.DataRepository.GetDataContext();
            var table = context.GetTable<TEntity>();
            var primaryKey = GetVmPrimaryKeyValue(context);
            var filter = new TableFilterDefinition<TEntity>(TableDefinition);
            foreach (var primaryKeyKeyValueField in primaryKey.KeyValueFields)
            {
                filter.AddFixedFieldFilter(
                    primaryKeyKeyValueField.FieldDefinition
                    , Conditions.Equals
                    , primaryKeyKeyValueField.Value);
            }

            var param = GblMethods.GetParameterExpression<TEntity>();
            var expr = filter.GetWhereExpresssion<TEntity>(param);
            var query = FieldFilterDefinition.FilterQuery(table, param, expr);
            var entity = query.FirstOrDefault();
            if (entity != null)
            {
                foreach (var grid in Grids)
                {
                    grid.Grid.DeleteNoCommitData(entity, context);
                }
            }

            return context.DeleteEntity(entity, $"Deleting {TableDefinition.Description}");
        }

        /// <summary>
        /// Registers the grid.  Will automatically save and load grid when database saves and loads view model.
        /// Used for grids that have one header field.
        /// </summary>
        /// <param name="grid">The grid.</param>
        /// <param name="readOnly">if set to <c>true</c> [read only].</param>
        public override void RegisterGrid(DbMaintenanceDataEntryGridManagerBase grid, bool readOnly = false)
        {
            var gridTable = grid.TableDefinition;

            if (!TablesToDelete.Contains(gridTable))
            {
                TablesToDelete.Add(gridTable);
            }
            base.RegisterGrid(grid, readOnly);
        }

        /// <summary>
        /// Gets the vm primary key value.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>PrimaryKeyValue.</returns>
        public virtual PrimaryKeyValue GetVmPrimaryKeyValue(IDbContext context)
        {
            return TableDefinition.GetPrimaryKeyValueFromEntity(GetEntityData());
        }

        protected TParentEntity GetParentEntity<TParentEntity>() where TParentEntity : class, new()
        {
            if (LookupAddViewArgs != null && LookupAddViewArgs.ParentWindowPrimaryKeyValue != null)
            {
                var parentTableDefinition = GblMethods.GetTableDefinition<TParentEntity>();
                if (parentTableDefinition != null)
                {
                    if (LookupAddViewArgs.ParentWindowPrimaryKeyValue.TableDefinition ==
                        parentTableDefinition && LookupAddViewArgs.ParentWindowPrimaryKeyValue.IsValid())
                    {
                        var entity =
                            parentTableDefinition.GetEntityFromPrimaryKeyValue(LookupAddViewArgs
                                .ParentWindowPrimaryKeyValue);
                        entity = entity.FillOutProperties(false);
                        return entity;
                    }

                }
            }
            return null;
        }
    }
}
