using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using System;
using System.ComponentModel;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.TableProcessing;

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
        where TEntity : new()
    {
        public int PageSize { get; } = 1;
        public LookupSearchTypes SearchType { get; } = LookupSearchTypes.Equals;
        public string SearchText { get; set; } = string.Empty;

        /// <summary>
        /// Gets the table definition.
        /// </summary>
        /// <value>
        /// The table definition.
        /// </value>
        public abstract TableDefinition<TEntity> TableDefinition { get; }

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

        public event EventHandler<ViewModelOperationPreviewEventArgs<TEntity>> ViewModelOperationPreview;

        public bool ValidateAllAtOnce { get; set; }

        private LookupDataBase _lookupData;
        private bool _fromLookupFormAddView;
        private AutoFillValue _savedKeyAutoFillValue;
        private bool _selectingRecord;
        private bool _savingRecord;
        private bool _lookupReadOnlyMode;
        private AutoFillValue _readOnlyAutoFillValue;

        protected internal override void InternalInitialize()
        {
            if (TableDefinition.LookupDefinition == null)
                throw new ArgumentException(
                    $"Table definition '{TableDefinition}' does not have a lookup definition setup.");

            var tableLookupDefinition = TableDefinition.LookupDefinition.Clone();
            Setup(tableLookupDefinition);
            _lookupData = new LookupDataBase(ViewLookupDefinition, this);

            _lookupData.LookupDataChanged += _lookupData_LookupDataChanged;
            FindButtonLookupDefinition = ViewLookupDefinition;

            base.InternalInitialize();

            if (LookupAddViewArgs != null)
            {
                var filter = GetAddViewFilter();
                if (filter != null)
                    _lookupData.LookupDefinition.FilterDefinition.CopyFrom(filter);
            }

            Initialize();
        }

        protected virtual TableFilterDefinitionBase GetAddViewFilter()
        {
            if (LookupAddViewArgs.LookupData.LookupDefinition.TableDefinition == TableDefinition)
                return LookupAddViewArgs.LookupData.LookupDefinition.FilterDefinition;

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
                        KeyAutoFillValue = new AutoFillValue(primaryKeyValue, LookupAddViewArgs.InitialAddModeText);
                        break;
                    default:
                        DeleteButtonEnabled = LookupAddViewArgs.AllowEdit && MaintenanceMode == DbMaintenanceModes.EditMode;
                        break;
                }

                NewButtonEnabled = SaveButtonEnabled = LookupAddViewArgs.AllowEdit;

                if (primaryKeyValue.IsValid)
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
            }

            if (result.TableDefinition != TableDefinition)
                throw new Exception($"The Add/View's Primary Key Definition's Table Definition '{result.TableDefinition}' does not match this Table Definition ('{TableDefinition}').  You must override GetAddViewPrimaryKeyValue and return a PrimaryKeyValue whose Table Definition is '{TableDefinition}'.");

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

            _lookupData.OnColumnClick(columnIndex, true);
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
                var entity = PopulatePrimaryKeyControls(newEntity, _lookupData.SelectedPrimaryKeyValue);
                if (entity == null)
                {
                    DbDataProcessor.UserInterface.PlaySystemSound(RsMessageBoxIcons.Exclamation);
                    ControlsGlobals.UserInterface.SetWindowCursor(WindowCursorTypes.Default);
                    ChangingEntity = false;
                    return;
                }
                if (!_savingRecord)
                {
                    LoadFromEntity(entity);
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

        /// <summary>
        /// Called when Previous button is clicked.
        /// </summary>
        public override void OnGotoPreviousButton()
        {
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
                if (_lookupData.ScrollPosition == LookupScrollPositions.Top)
                    _lookupData.GotoBottom();
                else
                {
                    _lookupData.GotoPreviousRecord();
                }
            }

            if (!Processor.IsMaintenanceKeyDown(MaintenanceKey.Ctrl))
            {
                View.ResetViewForNewRecord();
            }

        }

        /// <summary>
        /// Called when Next button is clicked.
        /// </summary>
        public override void OnGotoNextButton()
        {
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

            if (!Processor.IsMaintenanceKeyDown(MaintenanceKey.Ctrl))
            {
                View.ResetViewForNewRecord();
            }
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

            Processor.ShowFindLookupWindow(FindButtonLookupDefinition, false, false, FindButtonInitialSearchFor,
                _lookupData.SelectedPrimaryKeyValue);

            if (!keyDown)
            {
                View.ResetViewForNewRecord();
            }
        }

        public override void OnRecordSelected(LookupSelectArgs e)
        {
            if (!CheckDirty())
                return;

            _lookupData.SelectPrimaryKey(e.LookupData.SelectedPrimaryKeyValue);
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
                LookupAddViewArgs.LookupData.SelectPrimaryKey(_lookupData.SelectedPrimaryKeyValue);
                Processor.CloseWindow();
                LookupAddViewArgs.LookupData.ViewSelectedRow(0, View);
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
            KeyAutoFillValue = new AutoFillValue(new PrimaryKeyValue(TableDefinition), string.Empty);
            ChangingEntity = false;

            SelectButtonEnabled = false;
            DeleteButtonEnabled = false;
            PrimaryKeyControlsEnabled = true;
            ReadOnlyMode = false;

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
                        var message = $"{lookupFieldColumn.FieldDefinition.Description} cannot be empty.";
                        var caption = "Invalid Key Value";
                        //ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
                        View.OnValidationFail(lookupFieldColumn.FieldDefinition, message, caption);
                        return DbMaintenanceResults.ValidationError;
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

            if (!SaveEntity(entity))
                return DbMaintenanceResults.DatabaseError;

            var primaryKey = TableDefinition.GetPrimaryKeyValueFromEntity(entity);

            _savingRecord = true;

            if (unitTestMode)
            {
                PopulatePrimaryKeyControls(entity, primaryKey);
                OnLookupDataChanged();
            }
            else
            {
                _lookupData.SelectPrimaryKey(primaryKey);
            }

            _savingRecord = false;

            Processor?.ShowRecordSavedMessage();
            RecordsChanged = true;

            if (!keyDown)
            {
                View.ResetViewForNewRecord();
            }

            return DbMaintenanceResults.Success;
        }

        private bool CheckKeyValueTextChanged()
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
            return TableDefinition.ValidateEntity(entity, this);
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
            View.OnValidationFail(fieldDefinition, text, caption);
        }

        public virtual bool ValidateEntityProperty(FieldDefinition fieldDefinition, string valueToValidate)
        {
            return true;
        }

        private bool CheckDirty()
        {
            if (ReadOnlyMode)
                return true;

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
        public override DbMaintenanceResults DoDelete()
        {
            FireDeleteEvent();
            if (!DeleteCommand.IsEnabled)
                return DbMaintenanceResults.NotAllowed;

            var description = TableDefinition.RecordDescription;
            if (description.IsNullOrEmpty())
                description = TableDefinition.ToString();

            var message = ConfirmDeleteMessage(description);
            if (Processor.ShowYesNoMessage(message, ConfirmDeleteCaption))
            {
                var operationArgs = new ViewModelOperationPreviewEventArgs<TEntity>
                {
                    Operation = ViewModelOperations.Delete
                };
                OnViewModelOperationPreview(operationArgs);
                if (operationArgs.Handled)
                    return DbMaintenanceResults.NotAllowed;

                if (!DeleteEntity())
                    return DbMaintenanceResults.DatabaseError;

                RecordsChanged = true;
                RecordDirty = false;
                OnNewButton();
            }

            return DbMaintenanceResults.Success;
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
            FireCloseEvent();
            if (!CheckDirty())
            {
                e.Cancel = true;
                return;
            }

            if (!_selectingRecord && LookupAddViewArgs != null && RecordsChanged)
                LookupAddViewArgs.CallBackToken.OnRefreshData();
        }

        /// <summary>
        /// Populates the primary key controls.  This is executed during record save and retrieval operations.
        /// </summary>
        /// <param name="newEntity">The entity containing just the primary key values.</param>
        /// <param name="primaryKeyValue">The primary key value.</param>
        /// <returns>An entity populated from the database.</returns>
        protected abstract TEntity PopulatePrimaryKeyControls(TEntity newEntity, PrimaryKeyValue primaryKeyValue);

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
                    if (primaryKeyValue == null)
                        throw new ArgumentException("You must pass in a primary key value if KeyAutoFillValue is null.");
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
            if (primaryKeyValue.IsValid)
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
