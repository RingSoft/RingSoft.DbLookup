using System;
using System.ComponentModel;
using System.Linq;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.GetDataProcessor;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;

namespace RingSoft.DbMaintenance
{
    /// <summary>
    /// The base class of all database maintenance view model classes.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <seealso cref="DbMaintenanceViewModelBase" />
    /// <seealso cref="ILookupControl" />
    public abstract class DbMaintenanceViewModel<TEntity> : DbMaintenanceViewModelBase, ILookupControl
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
        /// Gets the lookup add on the fly arguments.
        /// </summary>
        /// <value>
        /// The lookup add on the fly arguments.
        /// </value>
        public LookupAddViewArgs LookupAddViewArgs { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this process has changed the data source.
        /// </summary>
        /// <value>
        ///   <c>true</c> if records changed; otherwise, <c>false</c>.
        /// </value>
        public bool RecordsChanged { get; private set; }

        /// <summary>
        /// Gets or sets the find button lookup definition.  By default it is the Lookup definition attached to the Table definition.
        /// </summary>
        /// <value>
        /// The find button lookup definition.
        /// </value>
        public LookupDefinitionBase FindButtonLookupDefinition { get; protected set; }

        /// <summary>
        /// Gets a value indicating whether the base entity is loading from the database or is being cleared.
        /// </summary>
        /// <value>
        ///   <c>true</c> if loading from the database or clearing; otherwise, <c>false</c>.
        /// </value>
        public bool ChangingEntity { get; private set; }

        private LookupDataBase _lookupData;
        private bool _fromLookupFormAddView;
        private AutoFillValue _savedKeyAutoFillValue;
        private bool _selectingRecord;

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

            View.LookupFormReturn += View_LookupFormReturn;

            base.InternalInitialize();

            if (LookupAddViewArgs != null)
            {
                _lookupData.LookupDefinition.FilterDefinition.CopyFrom(LookupAddViewArgs.LookupData.LookupDefinition
                    .FilterDefinition);
            }

            Initialize();
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        protected virtual void Initialize()
        {
            if (LookupAddViewArgs != null)
            {
                PrimaryKeyValue primaryKeyValue;
                switch (LookupAddViewArgs.LookupFormMode)
                {
                    case LookupFormModes.Add:
                        RecordDirty = false;
                        OnNewButton();
                        primaryKeyValue =
                            LookupAddViewArgs.LookupData.GetPrimaryKeyValueForSearchText(LookupAddViewArgs
                                .InitialAddModeText);
                        KeyAutoFillValue = new AutoFillValue(primaryKeyValue, LookupAddViewArgs.InitialAddModeText);
                        break;
                    case LookupFormModes.View:
                        primaryKeyValue = LookupAddViewArgs.LookupData.PrimaryKeyValue;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                if (primaryKeyValue.ContainsValidData())
                    _lookupData.SelectPrimaryKey(primaryKeyValue);
            }
            else
            {
                RecordDirty = false;
                OnNewButton();
            }

            RecordDirty = false;
        }

        /// <summary>
        /// Changes the sort column.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <exception cref="ArgumentException">Column {column.Caption} is not found in this view model's table lookup definition.</exception>
        protected void ChangeSortColumn(LookupColumnBase column)
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
                    TableDefinition.GetEntityFromPrimaryKeyValue(_lookupData.PrimaryKeyValue);

                ChangingEntity = true;
                DbDataProcessor.WindowCursor.SetWindowCursor(WindowCursorTypes.Wait);
                LoadFromEntity(newEntity);
                DbDataProcessor.WindowCursor.SetWindowCursor(WindowCursorTypes.Default);
                ChangingEntity = false;

                DeleteButtonEnabled = true;
                SelectButtonEnabled = _fromLookupFormAddView;
                PrimaryKeyControlsEnabled = false;
                RecordDirty = false;
                _savedKeyAutoFillValue = KeyAutoFillValue;
            }
        }

        /// <summary>
        /// Called when Previous button is clicked.
        /// </summary>
        public override void OnGotoPreviousButton()
        {
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
        }

        /// <summary>
        /// Called when Next button is clicked.
        /// </summary>
        public override void OnGotoNextButton()
        {
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
        }

        /// <summary>
        /// Called when the Find button is clicked.
        /// </summary>
        public override void OnFindButton()
        {
            View.ShowFindLookupWindow(FindButtonLookupDefinition, false, false, FindButtonInitialSearchFor);
        }

        private void View_LookupFormReturn(object sender, LookupSelectArgs e)
        {
            if (!CheckDirty())
                return;

            _lookupData.SelectPrimaryKey(e.LookupData.PrimaryKeyValue);
        }

        /// <summary>
        /// Called when the Select button is clicked.
        /// </summary>
        public override void OnSelectButton()
        {
            _selectingRecord = true;
            LookupAddViewArgs.LookupData.SelectPrimaryKey(_lookupData.PrimaryKeyValue);
            View.CloseWindow();
            LookupAddViewArgs.LookupData.ViewSelectedRow(0, View);
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

            View.ResetViewForNewRecord();
            RecordDirty = false;
        }

        /// <summary>
        /// Called when the Save button is clicked.
        /// </summary>
        /// <returns>
        /// The result.
        /// </returns>
        public override DbMaintenanceResults OnSaveButton()
        {
            if (MaintenanceMode == DbMaintenanceModes.AddMode && KeyAutoFillValue != null &&
                KeyAutoFillValue.PrimaryKeyValue.ContainsValidData())
            {
                _lookupData.SelectPrimaryKey(KeyAutoFillValue.PrimaryKeyValue);
                return DbMaintenanceResults.Success;
            }

            if (!CheckKeyValueTextChanged())
                return DbMaintenanceResults.ValidationError;

            var entity = GetEntityData();

            if (!ValidateEntity(entity))
                return DbMaintenanceResults.ValidationError;

            if (!SaveEntity(entity))
                return DbMaintenanceResults.DatabaseError;

            var primaryKey = TableDefinition.GetPrimaryKeyValueFromEntity(entity);

            _lookupData.SelectPrimaryKey(primaryKey);
            View.ShowRecordSavedMessage();
            RecordsChanged = true;

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

                if (!View.ShowYesNoMessage(message, RenameKeyAutoFillValueCaption))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Gets the rename key auto fill value caption.
        /// </summary>
        /// <value>
        /// The rename key auto fill value caption.  Override this for localization.
        /// </value>
        protected virtual string RenameKeyAutoFillValueCaption => "Change Unique Field Value";

        /// <summary>
        /// Gets the rename the key auto fill value message.  Override this for localization.
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
            var fieldsToValidate = TableDefinition.FieldDefinitions.Where(p => p.AllowNulls == false);
            foreach (var fieldDefinition in fieldsToValidate)
            {
                var valueToValidate = GblMethods.GetPropertyValue(entity, fieldDefinition.PropertyName);
                if (!ValidateEntityProperty(entity, fieldDefinition, valueToValidate))
                    return false;
            }

            fieldsToValidate =
                TableDefinition.FieldDefinitions.Where(p => p.AllowNulls && p.ParentJoinForeignKeyDefinition != null);

            foreach (var fieldDefinition in fieldsToValidate)
            {
                var autoFillValue = GetAutoFillValueForNullableForeignKeyField(fieldDefinition);
                if (autoFillValue != null && !autoFillValue.PrimaryKeyValue.ContainsValidData() &&
                    !autoFillValue.Text.IsNullOrEmpty())
                {
                    var message = ValidateFieldFailMessage(fieldDefinition);
                    var title = ValidateFieldFailCaption;
                    View.OnValidationFail(fieldDefinition, message, title);
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Gets the validate field fail caption.  Override this for localization.
        /// </summary>
        /// <value>
        /// The validate field fail caption.
        /// </value>
        protected virtual string ValidateFieldFailCaption => "Validation Failure!";

        /// <summary>
        /// Gets the validates the field fail message.  Override this for localization.
        /// </summary>
        /// <param name="fieldDefinition">The field definition.</param>
        /// <returns></returns>
        protected virtual string ValidateFieldFailMessage(FieldDefinition fieldDefinition)
        {
            var message = $"{fieldDefinition} has an invalid value.  Please correct the value.";
            return message;
        }

        /// <summary>
        /// Override this to get the auto fill value for nullable foreign key field.
        /// </summary>
        /// <param name="fieldDefinition">The field definition.</param>
        /// <returns></returns>
        protected virtual AutoFillValue GetAutoFillValueForNullableForeignKeyField(FieldDefinition fieldDefinition)
        {
            return null;
        }

        /// <summary>
        /// Validates the entity property.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="fieldDefinition">The field definition.</param>
        /// <param name="valueToValidate">The value to validate.</param>
        /// <returns></returns>
        protected virtual bool ValidateEntityProperty(TEntity entity, FieldDefinition fieldDefinition, string valueToValidate)
        {
            if (!fieldDefinition.ValidateValueForSavingToDb(valueToValidate))
            {
                var message = ValidateFieldFailMessage(fieldDefinition);
                var title = ValidateFieldFailCaption;
                View.OnValidationFail(fieldDefinition, message, title);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks to see if the record dirty flag has been raised and if so, asks the user if he/she wants to save the data.
        /// </summary>
        /// <returns>
        /// True if the user successfully saved the data or declined to save.  Otherwise false which means the user wants to abort the operation that called this function.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public bool CheckDirty()
        {
            if (RecordDirty)
            {
                if (MaintenanceMode == DbMaintenanceModes.AddMode && KeyAutoFillValue != null &&
                    KeyAutoFillValue.PrimaryKeyValue.ContainsValidData())
                    return true;

                var message = SaveChangesMessage;
                switch (View.ShowYesNoCancelMessage(message, TableDefinition.ToString()))
                {
                    case MessageButtons.Yes:
                        if (OnSaveButton() != DbMaintenanceResults.Success)
                            return false;
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
        /// Gets the save changes message.  Override this for localization.
        /// </summary>
        /// <value>
        /// The save changes message.
        /// </value>
        protected virtual string SaveChangesMessage => "Do you wish to save your changes to this data?";

        /// <summary>
        /// Called when the Delete button is clicked.
        /// </summary>
        /// <returns>
        /// The result.
        /// </returns>
        public override DbMaintenanceResults OnDeleteButton()
        {
            var description = TableDefinition.RecordDescription;
            if (description.IsNullOrEmpty())
                description = TableDefinition.ToString();

            var message = ConfirmDeleteMessage(description);
            if (View.ShowYesNoMessage(message, ConfirmDeleteCaption))
            {
                if (!DeleteEntity())
                    return DbMaintenanceResults.DatabaseError;

                RecordsChanged = true;
                RecordDirty = false;
                OnNewButton();
            }

            return DbMaintenanceResults.Success;
        }

        /// <summary>
        /// Gets the confirm delete caption.  Override this for localization.
        /// </summary>
        /// <value>
        /// The confirm delete caption.
        /// </value>
        protected virtual string ConfirmDeleteCaption => "Confirm Delete";

        /// <summary>
        /// Gets the confirm delete message.  Override this for localization.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <returns></returns>
        protected virtual string ConfirmDeleteMessage(string description)
        {
            var message = $"Are you sure you wish to delete this {description}?";
            return message;
        }

        /// <summary>
        /// called when the user is trying to close the view.
        /// </summary>
        /// <param name="e">The <see cref="CancelEventArgs" /> instance containing the event data.</param>
        public override void OnWindowClosing(CancelEventArgs e)
        {
            if (!CheckDirty())
            {
                e.Cancel = true;
                return;
            }

            if (!_selectingRecord && LookupAddViewArgs != null && RecordsChanged)
                LookupAddViewArgs.CallBackToken.OnRefreshData();
        }

        /// <summary>
        /// Loads from entity.
        /// </summary>
        /// <param name="newEntity">The new entity.</param>
        protected abstract void LoadFromEntity(TEntity newEntity);

        /// <summary>
        /// Creates and returns a lookup command.
        /// </summary>
        /// <param name="command">The command type.</param>
        /// <param name="primaryKeyValue">The primary key value.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">You must pass in a primary key value if KeyAutoFillValue is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">command - null</exception>
        protected LookupCommand GetLookupCommand(LookupCommands command, PrimaryKeyValue primaryKeyValue = null)
        {
            if (primaryKeyValue == null && KeyAutoFillValue != null)
                primaryKeyValue = KeyAutoFillValue.PrimaryKeyValue;

            switch (command)
            {
                case LookupCommands.Clear:
                case LookupCommands.AddModify:
                    break;
                case LookupCommands.Refresh:
                    if (primaryKeyValue == null)
                        throw new ArgumentException("You must pass in a primary key value if KeyAutoFillValue is null.");
                    return new LookupCommand(command, primaryKeyValue, RecordsChanged);
                default:
                    throw new ArgumentOutOfRangeException(nameof(command), command, null);
            }

            return new LookupCommand(command);
        }

        /// <summary>
        /// Gets the entity data.
        /// </summary>
        /// <returns></returns>
        protected abstract TEntity GetEntityData();

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
        /// Selects the primary key.
        /// </summary>
        /// <param name="primaryKeyValue">The primary key value.</param>
        protected void SelectPrimaryKey(PrimaryKeyValue primaryKeyValue)
        {
            if (primaryKeyValue.ContainsValidData())
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
        /// Executes the add modify command.
        /// </summary>
        /// <returns></returns>
        protected virtual DbMaintenanceResults ExecuteAddModifyCommand()
        {
            var result = DbMaintenanceResults.Success;
            if (MaintenanceMode == DbMaintenanceModes.AddMode)
            {
                var recordsChanged = RecordsChanged;
                result = OnSaveButton();
                RecordsChanged = recordsChanged;
            }

            return result;
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
