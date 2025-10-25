// ***********************************************************************
// Assembly         : RingSoft.DbMaintenance
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 09-02-2024
// ***********************************************************************
// <copyright file="DbMaintenanceViewModelBase.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.TableProcessing;
using RingSoft.Printing.Interop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using RingSoft.DbLookup.QueryBuilder;

namespace RingSoft.DbMaintenance
{
    /// <summary>
    /// Class GridMap.
    /// </summary>
    public class GridMap
    {
        /// <summary>
        /// Gets the grid.
        /// </summary>
        /// <value>The grid.</value>
        public DbMaintenanceDataEntryGridManagerBase Grid { get; }

        /// <summary>
        /// Gets a value indicating whether [read only].
        /// </summary>
        /// <value><c>true</c> if [read only]; otherwise, <c>false</c>.</value>
        public bool ReadOnly { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GridMap"/> class.
        /// </summary>
        /// <param name="grid">The grid.</param>
        /// <param name="readOnly">if set to <c>true</c> [read only].</param>
        public GridMap(DbMaintenanceDataEntryGridManagerBase grid, bool readOnly)
        {
            Grid = grid;
            ReadOnly = readOnly;
        }
    }
    /// <summary>
    /// Class LookupMap.
    /// </summary>
    public class LookupMap
    {
        /// <summary>
        /// Gets the lookup definition.
        /// </summary>
        /// <value>The lookup definition.</value>
        public LookupDefinitionBase LookupDefinition { get; internal set; }

        /// <summary>
        /// Gets the add view parameter.
        /// </summary>
        /// <value>The add view parameter.</value>
        public object AddViewParameter { get; internal set; }
    }
    /// <summary>
    /// Class UiControlMap.
    /// </summary>
    public class UiControlMap
    {
        /// <summary>
        /// Gets the UI command.
        /// </summary>
        /// <value>The UI command.</value>
        public UiCommand UiCommand { get; }

        /// <summary>
        /// Gets the field definition.
        /// </summary>
        /// <value>The field definition.</value>
        public FieldDefinition FieldDefinition { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UiControlMap" /> class.
        /// </summary>
        /// <param name="uiCommand">The UI command.</param>
        /// <param name="fieldDefinition">The field definition.</param>
        public UiControlMap(UiCommand uiCommand, FieldDefinition fieldDefinition)
        {
            UiCommand = uiCommand;
            FieldDefinition = fieldDefinition;
        }
    }
    /// <summary>
    /// Class SelectArgs.
    /// </summary>
    public class SelectArgs
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="SelectArgs" /> is cancel.
        /// </summary>
        /// <value><c>true</c> if cancel; otherwise, <c>false</c>.</value>
        public bool Cancel { get; set; }
    }

    /// <summary>
    /// Enum DbMaintenanceResults
    /// </summary>
    public enum DbMaintenanceResults
    {
        /// <summary>
        /// The success
        /// </summary>
        Success,
        /// <summary>
        /// The validation error
        /// </summary>
        ValidationError,
        /// <summary>
        /// The database error
        /// </summary>
        DatabaseError,
        /// <summary>
        /// The not allowed
        /// </summary>
        NotAllowed
    }

    /// <summary>
    /// Enum DbMaintenanceModes
    /// </summary>
    public enum DbMaintenanceModes
    {
        /// <summary>
        /// The add mode
        /// </summary>
        AddMode = 0,
        /// <summary>
        /// The edit mode
        /// </summary>
        EditMode = 1
    }

    /// <summary>
    /// Class CheckDirtyResultArgs.
    /// </summary>
    public class CheckDirtyResultArgs
    {
        /// <summary>
        /// Gets the result.
        /// </summary>
        /// <value>The result.</value>
        public MessageButtons Result { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckDirtyResultArgs" /> class.
        /// </summary>
        /// <param name="result">The result.</param>
        internal CheckDirtyResultArgs(MessageButtons result)
        {
            Result = result;
        }
    }

    public class KeyFilterData
    {
        public List<KeyFilterItem> KeyFilters { get; } = new List<KeyFilterItem>();

        public bool KeyFilterChanged { get; set; }

        public void AddFilterItem(FieldDefinition fieldDefinition, Conditions condition, string value)
        {
            var filterItem = new KeyFilterItem(
                fieldDefinition, condition, value);
            KeyFilters.Add(filterItem);
        }
    }

    public class KeyFilterItem
    {
        public FieldDefinition FieldDefinition { get; }

        public Conditions Condition { get; }

        public string Value { get; }

        public KeyFilterItem(FieldDefinition fieldDefinition, Conditions condition, string value)
        {
            FieldDefinition = fieldDefinition;
            Condition = condition;
            Value = value;
        }
    }

    /// <summary>
    /// The base class for database maintenance operations.
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    public abstract class DbMaintenanceViewModelBase : INotifyPropertyChanged, IPrintProcessor
    {
        /// <summary>
        /// Gets the table definition.
        /// </summary>
        /// <value>The table definition.</value>
        public abstract TableDefinitionBase TableDefinitionBase { get; }

        /// <summary>
        /// Gets the view lookup definition used to get the next and previous record.
        /// </summary>
        /// <value>The view lookup definition.</value>
        protected LookupDefinitionBase ViewLookupDefinition { get; private set; }

        /// <summary>
        /// Gets the view.
        /// </summary>
        /// <value>The view.</value>
        protected IDbMaintenanceView View { get; private set; }

        /// <summary>
        /// Gets the add-on-the fly arguments sent by the LookupControl or the LookupWindow.
        /// </summary>
        /// <value>The lookup add on the fly arguments.</value>
        public LookupAddViewArgs LookupAddViewArgs { get; internal set; }

        /// <summary>
        /// Gets the initial search for text when the Find button is clicked.  By default it is the key auto fill text.
        /// </summary>
        /// <value>The find button initial search for.</value>
        protected virtual string FindButtonInitialSearchFor { get; } = string.Empty;

        /// <summary>
        /// The key automatic fill setup
        /// </summary>
        private AutoFillSetup _keyAutoFillSetup;

        /// <summary>
        /// Gets the unique key control auto fill setup.
        /// </summary>
        /// <value>The unique key control auto fill setup.</value>
        public AutoFillSetup KeyAutoFillSetup
        {
            get => _keyAutoFillSetup;
            set
            {
                if (_keyAutoFillSetup == value)
                    return;

                _keyAutoFillSetup = value;
                OnPropertyChanged(nameof(KeyAutoFillSetup));
            }
        }

        /// <summary>
        /// The last saved date
        /// </summary>
        private DateTime? _lastSavedDate;

        /// <summary>
        /// Gets or sets the last saved date.
        /// </summary>
        /// <value>The last saved date.</value>
        public DateTime? LastSavedDate
        {
            get => _lastSavedDate;
            set
            {
                if (_lastSavedDate == value)
                    return;

                _lastSavedDate = value;
                OnPropertyChanged(null, false);
            }
        }


        /// <summary>
        /// Gets or sets the find button lookup definition.  By default it is the Lookup definition attached to the Table definition.
        /// </summary>
        /// <value>The find button lookup definition.</value>
        public LookupDefinitionBase FindButtonLookupDefinition { get; set; }


        /// <summary>
        /// The key automatic fill value
        /// </summary>
        private AutoFillValue _keyAutoFillValue;
        /// <summary>
        /// Gets or sets the unique key control auto fill value.
        /// </summary>
        /// <value>The key auto fill value.</value>
        public AutoFillValue KeyAutoFillValue
        {
            get => _keyAutoFillValue;
            set
            {
                if (_keyAutoFillValue == value)
                    return;

                if (value == null)
                {
                    _keyAutoFillValue = new AutoFillValue(null, "");
                }
                else
                {
                    _keyAutoFillValue = value;
                }

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the key automatic fill UI command.
        /// </summary>
        /// <value>The key automatic fill UI command.</value>
        public UiCommand KeyAutoFillUiCommand { get; }

        /// <summary>
        /// The allow delete
        /// </summary>
        private bool _allowDelete = true;

        /// <summary>
        /// Gets or sets a value indicating whether [allow delete].
        /// </summary>
        /// <value><c>true</c> if [allow delete]; otherwise, <c>false</c>.</value>
        public bool AllowDelete
        {
            get { return _allowDelete; }
            set
            {
                _allowDelete = value;
                DeleteButtonEnabled = value;
            }
        }

        /// <summary>
        /// The allow new
        /// </summary>
        private bool _allowNew = true;

        /// <summary>
        /// Gets or sets a value indicating whether [allow new].
        /// </summary>
        /// <value><c>true</c> if [allow new]; otherwise, <c>false</c>.</value>
        public bool AllowNew
        {
            get { return _allowNew; }
            set
            {
                _allowNew = value;
                NewButtonEnabled = value;
            }
        }

        /// <summary>
        /// The allow save
        /// </summary>
        private bool _allowSave = true;

        /// <summary>
        /// Gets or sets a value indicating whether [allow save].
        /// </summary>
        /// <value><c>true</c> if [allow save]; otherwise, <c>false</c>.</value>
        public bool AllowSave
        {
            get { return _allowSave; }
            set
            {
                _allowSave = value;
                SaveButtonEnabled = value;
            }
        }



        /// <summary>
        /// The primary key controls enabled
        /// </summary>
        private bool _primaryKeyControlsEnabled;

        /// <summary>
        /// Gets or sets a value indicating whether the primary key controls are enabled.
        /// </summary>
        /// <value><c>true</c> if primary key controls enabled; otherwise, <c>false</c>.</value>
        public bool PrimaryKeyControlsEnabled
        {
            get => _primaryKeyControlsEnabled;
            set
            {
                _primaryKeyControlsEnabled = value;
                OnPropertyChanged(nameof(PrimaryKeyControlsEnabled), false);
            }
        }

        /// <summary>
        /// The select button enabled
        /// </summary>
        private bool _selectButtonEnabled;
        /// <summary>
        /// Gets or sets a value indicating whether the Select button is enabled.
        /// </summary>
        /// <value><c>true</c> if the select button is enabled; otherwise, <c>false</c>.</value>
        public bool SelectButtonEnabled
        {
            get => _selectButtonEnabled;
            set
            {
                SelectCommand.IsEnabled = _selectButtonEnabled = value;
                OnPropertyChanged(nameof(SelectButtonEnabled), false);
            }
        }

        /// <summary>
        /// The delete button enabled
        /// </summary>
        private bool _deleteButtonEnabled;
        /// <summary>
        /// Gets or sets a value indicating whether the Delete button is enabled.
        /// </summary>
        /// <value><c>true</c> if the delete button is enabled; otherwise, <c>false</c>.</value>
        public bool DeleteButtonEnabled
        {
            get => _deleteButtonEnabled;
            set
            {
                if (!AllowDelete)
                {
                    value = false;
                }
                DeleteCommand.IsEnabled = _deleteButtonEnabled = value;
                DeleteUiCommand.IsEnabled = value;
                OnPropertyChanged(nameof(DeleteButtonEnabled), false);
            }
        }

        /// <summary>
        /// The new button enabled
        /// </summary>
        private bool _newButtonEnabled;

        /// <summary>
        /// Gets or sets a value indicating whether the New button is enabled.
        /// </summary>
        /// <value><c>true</c> if the new button is enabled; otherwise, <c>false</c>.</value>
        public bool NewButtonEnabled
        {
            get => _newButtonEnabled;
            set
            {
                if (!AllowNew)
                {
                    value = false;
                }

                NewCommand.IsEnabled = _newButtonEnabled = value;
                NewUiCommand.IsEnabled = value;
                OnPropertyChanged(nameof(NewButtonEnabled), false);
            }
        }

        /// <summary>
        /// The save button enabled
        /// </summary>
        private bool _saveButtonEnabled;

        /// <summary>
        /// Gets or sets a value indicating whether the Save button is enabled.
        /// </summary>
        /// <value><c>true</c> if the save button is enabled; otherwise, <c>false</c>.</value>
        public bool SaveButtonEnabled
        {
            get => _saveButtonEnabled;
            set
            {
                if (!AllowSave)
                {
                    value = false;
                }

                SaveCommand.IsEnabled = _saveButtonEnabled = value;
                SaveUiCommand.IsEnabled = value;
                OnPropertyChanged(nameof(SaveButtonEnabled), false);
            }
        }

        /// <summary>
        /// The key value dirty
        /// </summary>
        private bool _keyValueDirty;

        /// <summary>
        /// Gets or sets a value indicating whether the data in the user-editable unique key value control has been changed.  Used when focus leaves the unique user-editable control.  When that happens and this value is true, then the record matching that unique value is automatically loaded.
        /// </summary>
        /// <value><c>true</c> if data in the key value control has changed; otherwise, <c>false</c>.</value>
        public bool KeyValueDirty
        {
            get => _keyValueDirty;
            set
            {
                if (_keyValueDirty == value)
                    return;

                _keyValueDirty = value;
                var recordDirty = RecordDirty;
                OnPropertyChanged(nameof(KeyValueDirty));
                RecordDirty = recordDirty;
            }
        }

        /// <summary>
        /// Gets or sets the maintenance mode.
        /// </summary>
        /// <value>The maintenance mode.</value>
        public DbMaintenanceModes MaintenanceMode { get; protected set; }

        /// <summary>
        /// The record dirty
        /// </summary>
        private bool _recordDirty;

        /// <summary>
        /// Gets or sets a value indicating whether the user has changed the data on this record and has yet to save.
        /// </summary>
        /// <value><c>true</c> if there are unsaved changes in this record; otherwise, <c>false</c>.</value>
        public bool RecordDirty
        {
            get => _recordDirty;
            set
            {
                if (_recordDirty == value)
                    return;

                _recordDirty = value;
                OnRecordDirtyChanged(_recordDirty);
            }
        }

        /// <summary>
        /// The read only mode
        /// </summary>
        private bool _readOnlyMode;

        /// <summary>
        /// Gets or sets a value indicating whether [read only mode].
        /// </summary>
        /// <value><c>true</c> if [read only mode]; otherwise, <c>false</c>.</value>
        public bool ReadOnlyMode
        {
            get => _readOnlyMode;
            set
            {
                if (_readOnlyMode == value)
                    return;

                _readOnlyMode = value;
                OnReadOnlyModeChanged(_readOnlyMode);
            }
        }

        /// <summary>
        /// Gets the UI controls.
        /// </summary>
        /// <value>The UI controls.</value>
        public List<UiControlMap> UiControls { get; } = new List<UiControlMap>();
        /// <summary>
        /// Gets or sets the input parameter.
        /// </summary>
        /// <value>The input parameter.</value>
        public object InputParameter { get; set; }

        /// <summary>
        /// Gets or sets the lock date.
        /// </summary>
        /// <value>The lock date.</value>
        public DateTime LockDate { get; set; }

        /// <summary>
        /// Gets or sets the processor.
        /// </summary>
        /// <value>The processor.</value>
        public IDbMaintenanceDataProcessor Processor { get; set; }

        /// <summary>
        /// Gets the previous command.
        /// </summary>
        /// <value>The previous command.</value>
        public RelayCommand PreviousCommand { get; private set; }

        /// <summary>
        /// Gets the previous UI command.
        /// </summary>
        /// <value>The previous UI command.</value>
        public UiCommand PreviousUiCommand { get; }
        /// <summary>
        /// Gets the next command.
        /// </summary>
        /// <value>The next command.</value>
        public RelayCommand NextCommand { get; }
        /// <summary>
        /// Gets the next UI command.
        /// </summary>
        /// <value>The next UI command.</value>
        public UiCommand NextUiCommand { get; }
        /// <summary>
        /// Gets the save command.
        /// </summary>
        /// <value>The save command.</value>
        public RelayCommand SaveCommand { get; }
        /// <summary>
        /// Gets the save UI command.
        /// </summary>
        /// <value>The save UI command.</value>
        public UiCommand SaveUiCommand { get; }
        /// <summary>
        /// Gets the delete command.
        /// </summary>
        /// <value>The delete command.</value>
        public RelayCommand DeleteCommand { get; }
        /// <summary>
        /// Gets the delete UI command.
        /// </summary>
        /// <value>The delete UI command.</value>
        public UiCommand DeleteUiCommand { get; }
        /// <summary>
        /// Creates new command.
        /// </summary>
        /// <value>The new command.</value>
        public RelayCommand NewCommand { get; }
        /// <summary>
        /// Creates new uicommand.
        /// </summary>
        /// <value>The new UI command.</value>
        public UiCommand NewUiCommand { get; }
        /// <summary>
        /// Gets the find command.
        /// </summary>
        /// <value>The find command.</value>
        public RelayCommand FindCommand { get; }
        /// <summary>
        /// Gets the find UI command.
        /// </summary>
        /// <value>The find UI command.</value>
        public UiCommand FindUiCommand { get; }
        /// <summary>
        /// Gets the select command.
        /// </summary>
        /// <value>The select command.</value>
        public RelayCommand SelectCommand { get; }
        /// <summary>
        /// Gets the select UI command.
        /// </summary>
        /// <value>The select UI command.</value>
        public UiCommand SelectUiCommand { get; }
        /// <summary>
        /// Gets the print command.
        /// </summary>
        /// <value>The print command.</value>
        public RelayCommand PrintCommand { get; }
        /// <summary>
        /// Gets the print UI command.
        /// </summary>
        /// <value>The print UI command.</value>
        public UiCommand PrintUiCommand { get; }
        /// <summary>
        /// Gets the maintenance buttons UI command.
        /// </summary>
        /// <value>The maintenance buttons UI command.</value>
        public UiCommand MaintenanceButtonsUiCommand { get; }
        /// <summary>
        /// Gets the status bar UI command.
        /// </summary>
        /// <value>The status bar UI command.</value>
        public UiCommand StatusBarUiCommand { get; }
        /// <summary>
        /// Gets or sets a value indicating whether [check dirty flag].
        /// </summary>
        /// <value><c>true</c> if [check dirty flag]; otherwise, <c>false</c>.</value>
        public bool CheckDirtyFlag { get; set; } = true;

        /// <summary>
        /// The grids
        /// </summary>
        private List<GridMap> _grids
            = new List<GridMap>();
        /// <summary>
        /// Gets the grids.
        /// </summary>
        /// <value>The grids.</value>
        public IReadOnlyList<GridMap> Grids { get; }

        /// <summary>
        /// The lookups
        /// </summary>
        private List<LookupMap> _lookups = new List<LookupMap>();

        /// <summary>
        /// Gets the lookups.
        /// </summary>
        /// <value>The lookups.</value>
        public IReadOnlyList<LookupMap> Lookups { get; }

        /// <summary>
        /// Occurs when [check dirty message shown].
        /// </summary>
        public event EventHandler<CheckDirtyResultArgs> CheckDirtyMessageShown;
        /// <summary>
        /// Occurs when [initialize event].
        /// </summary>
        public event EventHandler InitializeEvent;
        public event EventHandler PreInitializeEvent;
        public event EventHandler RecordSelectedEvent;
        /// <summary>
        /// Occurs when [save event].
        /// </summary>
        public event EventHandler SaveEvent;

        public event EventHandler SavedEvent;
        /// <summary>
        /// Occurs when [select event].
        /// </summary>
        public event EventHandler<SelectArgs> SelectEvent;
        /// <summary>
        /// Occurs when [delete event].
        /// </summary>
        public event EventHandler DeleteEvent;
        /// <summary>
        /// Occurs when [find event].
        /// </summary>
        public event EventHandler FindEvent;
        /// <summary>
        /// Creates new event.
        /// </summary>
        public event EventHandler NewEvent;
        /// <summary>
        /// Occurs when [close event].
        /// </summary>
        public event EventHandler CloseEvent;
        /// <summary>
        /// Occurs when [next event].
        /// </summary>
        public event EventHandler NextEvent;
        /// <summary>
        /// Occurs when [previous event].
        /// </summary>
        public event EventHandler PreviousEvent;


        /// <summary>
        /// Initializes a new instance of the <see cref="DbMaintenanceViewModelBase" /> class.
        /// </summary>
        public DbMaintenanceViewModelBase()
        {
            PreviousCommand = new RelayCommand(OnGotoPreviousButton);
            PreviousUiCommand = new UiCommand();
            NextCommand = new RelayCommand(OnGotoNextButton);
            NextUiCommand = new UiCommand();
            SaveCommand = new RelayCommand(OnSaveButton){IsEnabled = SaveButtonEnabled};
            SaveUiCommand = new UiCommand();
            DeleteCommand = new RelayCommand(OnDeleteButton){IsEnabled = DeleteButtonEnabled};
            DeleteUiCommand = new UiCommand();
            NewCommand = new RelayCommand(OnNewButton){IsEnabled = NewButtonEnabled};
            NewUiCommand = new UiCommand();
            FindCommand = new RelayCommand(OnFindButton);
            FindUiCommand = new UiCommand();
            SelectCommand = new RelayCommand(OnSelectButton){IsEnabled = SelectButtonEnabled};
            SelectUiCommand = new UiCommand();
            PrintCommand = new RelayCommand(PrintOutput);
            PrintUiCommand = new UiCommand();
            NewButtonEnabled = SaveButtonEnabled = true;
            KeyAutoFillUiCommand = new UiCommand();

            MaintenanceButtonsUiCommand = new UiCommand();
            StatusBarUiCommand = new UiCommand();

            Grids = _grids.AsReadOnly();
            Lookups = _lookups.AsReadOnly();
        }

        protected virtual KeyFilterData GetKeyFilterData()
        {
            return null;
        }

        /// <summary>
        /// Maps the field to UI command.
        /// </summary>
        /// <param name="uiCommand">The UI command.</param>
        /// <param name="fieldDefinition">The field definition.</param>
        public void MapFieldToUiCommand(UiCommand uiCommand, FieldDefinition fieldDefinition)
        {
            UiControls.Add(new UiControlMap(uiCommand, fieldDefinition));

            //Peter Ringering - 01/16/2025 02:12:31 PM - E-112
            if (fieldDefinition is StringFieldDefinition stringField)
            {
                uiCommand.MaxLength = stringField.MaxLength;
            }
        }

        /// <summary>
        /// Setups the specified lookup definition.
        /// </summary>
        /// <param name="lookupDefinition">The lookup definition.</param>
        protected internal void Setup(LookupDefinitionBase lookupDefinition)
        {
            ViewLookupDefinition = lookupDefinition;
            KeyAutoFillSetup = new AutoFillSetup(lookupDefinition)
            {
                AllowLookupView = false,
                AllowLookupAdd = false
            };
        }

        /// <summary>
        /// Executed when the view is loaded.  Sets the View interface and initializes this object.
        /// </summary>
        /// <param name="view">The View interface.</param>
        public void OnViewLoaded(IDbMaintenanceView view)
        {
            FirePreInitializeEvent();
            View = view;
            InternalInitialize();
        }

        /// <summary>
        /// Internals the initialize.
        /// </summary>
        protected internal virtual void InternalInitialize()
        {
        }

        /// <summary>
        /// Executed when Previous button is clicked.
        /// </summary>
        public abstract void OnGotoPreviousButton();

        /// <summary>
        /// Executed when Next button is clicked.
        /// </summary>
        public abstract void OnGotoNextButton();

        /// <summary>
        /// Executed when the Find button is clicked.
        /// </summary>
        public abstract void OnFindButton();

        /// <summary>
        /// Executed when the Select button is clicked.
        /// </summary>
        public abstract void OnSelectButton();

        /// <summary>
        /// Executed when the New button is clicked.
        /// </summary>
        public abstract void OnNewButton();

        /// <summary>
        /// Executed when the Save button is clicked.
        /// </summary>
        /// <returns>The result.</returns>
        public abstract DbMaintenanceResults DoSave();

        /// <summary>
        /// Executed when the Save button is clicked.
        /// </summary>
        public void OnSaveButton() => DoSave();

        /// <summary>
        /// Executed when the Delete button is clicked.
        /// </summary>
        /// <returns>The result.</returns>
        protected abstract DbMaintenanceResults DoDelete();

        /// <summary>
        /// Executed when the Delete button is clicked.
        /// </summary>
        public void OnDeleteButton() => DoDelete();

        /// <summary>
        /// Executed when the key control looses focus.  Used to ensure no duplicate value in the key control is entered.
        /// </summary>
        public abstract void OnKeyControlLeave();

        /// <summary>
        /// Executed when the user is trying to close the view.
        /// </summary>
        /// <param name="e">The <see cref="CancelEventArgs" /> instance containing the event data.</param>
        public abstract void OnWindowClosing(CancelEventArgs e);

        /// <summary>
        /// Initializes from lookup data that is sent when the user wishes to add/view lookup data on the fly.
        /// </summary>
        /// <param name="e">The e.</param>
        public abstract void InitializeFromLookupData(LookupAddViewArgs e);

        /// <summary>
        /// Called when [record selected].
        /// </summary>
        /// <param name="e">The e.</param>
        public abstract void OnRecordSelected(LookupSelectArgs e);

        /// <summary>
        /// Called when [record selected].
        /// </summary>
        /// <param name="primaryKey">The primary key.</param>
        public abstract void OnRecordSelected(PrimaryKeyValue primaryKey);

        /// <summary>
        /// Called when [record dirty changed].
        /// </summary>
        /// <param name="newValue">if set to <c>true</c> [new value].</param>
        protected virtual void OnRecordDirtyChanged(bool newValue)
        {
        }

        protected void FirePreInitializeEvent()
        {
            PreInitializeEvent?.Invoke(this, new EventArgs());
        }

        protected void FireRecordSelectedEvent()
        {
            RecordSelectedEvent?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Fires the initialize event.
        /// </summary>
        protected void FireInitializeEvent()
        {
            InitializeEvent?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Fires the previous event.
        /// </summary>
        protected void FirePreviousEvent()
        {
            PreviousEvent?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Fires the next event.
        /// </summary>
        protected void FireNextEvent()
        {
            NextEvent?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Fires the find event.
        /// </summary>
        protected void FireFindEvent()
        {
            FindEvent?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Fires the select event.
        /// </summary>
        /// <returns>SelectArgs.</returns>
        protected SelectArgs FireSelectEvent()
        {
            var selectArgs = new SelectArgs();
            SelectEvent?.Invoke(this, selectArgs);
            return selectArgs;
        }

        /// <summary>
        /// Fires the new event.
        /// </summary>
        protected void FireNewEvent()
        {
            NewEvent?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Fires the save event.
        /// </summary>
        protected void FireSaveEvent()
        {
            SaveEvent?.Invoke(this, new EventArgs());
        }

        //Peter Ringering - 11/23/2024 12:14:57 PM - E-76
        protected void FireSavedEvent()
        {
            SavedEvent?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Fires the delete event.
        /// </summary>
        protected void FireDeleteEvent()
        {
            DeleteEvent?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Fires the close event.
        /// </summary>
        protected void FireCloseEvent()
        {
            CloseEvent?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Called when [read only mode changed].
        /// </summary>
        /// <param name="newValue">if set to <c>true</c> [new value].</param>
        protected virtual void OnReadOnlyModeChanged(bool newValue)
        {
            if (newValue)
            {
                DeleteButtonEnabled = SaveButtonEnabled = false;
            }
            else
            {
                if (MaintenanceMode != DbMaintenanceModes.AddMode)
                    DeleteButtonEnabled = true;

                SaveButtonEnabled = true;
            }

            View?.SetReadOnlyMode(newValue);
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Called when [check dirty flag message shown].
        /// </summary>
        /// <param name="e">The e.</param>
        protected virtual void OnCheckDirtyFlagMessageShown(CheckDirtyResultArgs e)
        {
            CheckDirtyMessageShown?.Invoke(this, e);
        }

        /// <summary>
        /// Prints the output.
        /// </summary>
        protected virtual void PrintOutput()
        {
            Processor.PrintOutput(CreatePrinterSetupArgs());
        }

        /// <summary>
        /// Creates the printer setup arguments.
        /// </summary>
        /// <returns>PrinterSetupArgs.</returns>
        protected PrinterSetupArgs CreatePrinterSetupArgs()
        {
            var printLookup = FindButtonLookupDefinition.Clone();
            var printerSetupArgs = new PrinterSetupArgs
            {
                CodeAutoFillValue = KeyAutoFillValue,
                CodeAutoFillSetup = KeyAutoFillSetup,
                LookupDefinition = printLookup,
                DataProcessor = this,
                CodeDescription = TableDefinitionBase.RecordDescription,
            };
            printerSetupArgs.PrintingProperties.ReportTitle = $"{TableDefinitionBase.Description} Report";
            SetupPrinterArgs(printerSetupArgs);

            return printerSetupArgs;
        }

        /// <summary>
        /// Setups the printer arguments.
        /// </summary>
        /// <param name="printerSetupArgs">The printer setup arguments.</param>
        /// <param name="stringFieldIndex">Index of the string field.</param>
        /// <param name="numericFieldIndex">Index of the numeric field.</param>
        /// <param name="memoFieldIndex">Index of the memo field.</param>
        protected virtual void SetupPrinterArgs(PrinterSetupArgs printerSetupArgs, int stringFieldIndex = 1
            , int numericFieldIndex = 1, int memoFieldIndex = 1)
        {
            printerSetupArgs.LookupDefinition.AddAllFieldsAsHiddenColumns(false);

            foreach (var hiddenColumn in printerSetupArgs.LookupDefinition.HiddenColumns)
            {
                var columnMap = MapLookupColumns(ref stringFieldIndex, ref numericFieldIndex, ref memoFieldIndex, hiddenColumn);

                if (columnMap != null && columnMap.ColumnDefinition != null)
                {
                    printerSetupArgs.ColumnMaps.Add(columnMap);
                }
            }
        }

        /// <summary>
        /// Maps the lookup columns.
        /// </summary>
        /// <param name="stringFieldIndex">Index of the string field.</param>
        /// <param name="numericFieldIndex">Index of the numeric field.</param>
        /// <param name="memoFieldIndex">Index of the memo field.</param>
        /// <param name="hiddenColumn">The hidden column.</param>
        /// <returns>PrintingColumnMap.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public static PrintingColumnMap MapLookupColumns(ref int stringFieldIndex, ref int numericFieldIndex,
            ref int memoFieldIndex, LookupColumnDefinitionBase hiddenColumn)
        {
            var columnMap = new PrintingColumnMap();
            switch (hiddenColumn.DataType)
            {
                case FieldDataTypes.String:
                    if (hiddenColumn is LookupFieldColumnDefinition stringColumn)
                    {
                        if (stringColumn.FieldDefinition is StringFieldDefinition stringField)
                        {
                            if (stringField.MemoField)
                            {
                                MapMemoField(memoFieldIndex, columnMap, hiddenColumn);
                                memoFieldIndex++;
                            }
                            else
                            {
                                MapStringField(columnMap, hiddenColumn, stringFieldIndex);
                                stringFieldIndex++;
                            }
                        }
                        else
                        {
                            MapStringField(columnMap, hiddenColumn, stringFieldIndex);
                            stringFieldIndex++;
                        }
                    }
                    else
                    {
                        MapStringField(columnMap, hiddenColumn, stringFieldIndex);
                        stringFieldIndex++;
                    }

                    break;
                case FieldDataTypes.Integer:
                case FieldDataTypes.DateTime:
                case FieldDataTypes.Bool:
                    MapStringField(columnMap, hiddenColumn, stringFieldIndex);
                    stringFieldIndex++;
                    break;
                case FieldDataTypes.Decimal:
                    columnMap.MapNumber(hiddenColumn, numericFieldIndex,
                        hiddenColumn.SelectSqlAlias);
                    PrintingInteropGlobals.PropertiesProcessor.SetNumberCaption(numericFieldIndex,
                        hiddenColumn.Caption.Replace("\n", " "));
                    numericFieldIndex++;
                    break;
                case FieldDataTypes.Memo:
                    MapMemoField(memoFieldIndex, columnMap, hiddenColumn);
                    memoFieldIndex++;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return columnMap;
        }

        /// <summary>
        /// Maps the memo field.
        /// </summary>
        /// <param name="memoFieldIndex">Index of the memo field.</param>
        /// <param name="columnMap">The column map.</param>
        /// <param name="hiddenColumn">The hidden column.</param>
        private static void MapMemoField(int memoFieldIndex, PrintingColumnMap columnMap,
            LookupColumnDefinitionBase hiddenColumn)
        {
            var caption = hiddenColumn.Caption;
            if (!caption.IsNullOrEmpty())
            {
                caption = caption.Replace("\r\n", " ");
            }

            columnMap.MapMemo(hiddenColumn, memoFieldIndex,
                hiddenColumn.SelectSqlAlias);
            PrintingInteropGlobals.PropertiesProcessor.SetMemoCaption(memoFieldIndex,
                caption);
        }

        /// <summary>
        /// Maps the string field.
        /// </summary>
        /// <param name="columnMap">The column map.</param>
        /// <param name="lookupColumn">The lookup column.</param>
        /// <param name="stringFieldIndex">Index of the string field.</param>
        private static void MapStringField(PrintingColumnMap columnMap, LookupColumnDefinitionBase lookupColumn,
            int stringFieldIndex)
        {
            var caption = lookupColumn.Caption;
            if (!caption.IsNullOrEmpty())
            {
                caption = caption.Replace("\r\n", " ");
            }
            columnMap.MapString(lookupColumn, stringFieldIndex, lookupColumn.SelectSqlAlias);
            PrintingInteropGlobals.PropertiesProcessor.SetStringCaption(stringFieldIndex,
                caption);
        }

        /// <summary>
        /// Processes the print output data.
        /// </summary>
        /// <param name="printerSetupArgs">The printer setup arguments.</param>
        public virtual void ProcessPrintOutputData(PrinterSetupArgs printerSetupArgs)
        {
            ProcessLookupPrintOutput(printerSetupArgs, this);
        }

        //Peter Ringering - 01/06/2025 03:08:18 PM - E-90
        private static void ScrubPrinterSetup(PrinterSetupArgs printerSetupArgs)
        {
            switch (printerSetupArgs.PrintingProperties.ReportType)
            {
                case ReportTypes.Summary:
                case ReportTypes.Condensed:
                    printerSetupArgs
                        .ColumnMaps
                        .RemoveAll(p => p.ColumnType == PrintColumnTypes.Memo);
                    printerSetupArgs.PrintingProperties.MemoFieldCaption1 = string.Empty;
                    printerSetupArgs.PrintingProperties.MemoFieldCaption2 = string.Empty;
                    printerSetupArgs.PrintingProperties.MemoFieldCaption3 = string.Empty;
                    printerSetupArgs.PrintingProperties.MemoFieldCaption4 = string.Empty;
                    break;
            }
        }

        /// <summary>
        /// Processes the lookup print output.
        /// </summary>
        /// <param name="printerSetupArgs">The printer setup arguments.</param>
        /// <param name="printProcessor">The print processor.</param>
        public static void ProcessLookupPrintOutput(PrinterSetupArgs printerSetupArgs, IPrintProcessor printProcessor)
        { 
            ScrubPrinterSetup(printerSetupArgs);
            var lookupData = printerSetupArgs.LookupDefinition.TableDefinition.LookupDefinition
                .GetLookupDataMaui(printerSetupArgs.LookupDefinition, false);

            printerSetupArgs.PrintingProcessingViewModel.UpdateStatus(0, 0, ProcessTypes.CountingHeaderRecords);
            printerSetupArgs.TotalRecords = lookupData.GetRecordCount();
            var page = 0;
            lookupData.PrintOutput += (sender, output) =>
            {
                var headerRows = new List<PrintingInputHeaderRow>();
                foreach (var primaryKeyValue in output.Result)
                {
                    var headerRow = lookupData.GetPrinterHeaderRow(primaryKeyValue, printerSetupArgs);
                    headerRows.Add(headerRow);

                    var dataProcessedArgs = new PrinterDataProcessedEventArgs
                    {
                        HeaderRow = headerRow,
                        PrinterSetup = printerSetupArgs,
                        LookupData = lookupData,
                        PrimaryKey = primaryKeyValue,
                    };

                    printProcessor.NotifyProcessingHeader(dataProcessedArgs);
                }

                if (printerSetupArgs.PrintingProcessingViewModel.Abort)
                {
                    output.Abort = true;
                    return;
                }

                var result = PrintingInteropGlobals.HeaderProcessor.AddChunk(headerRows, printerSetupArgs.PrintingProperties);
                if (!result.IsNullOrEmpty())
                {
                    printerSetupArgs.PrintingProcessingViewModel.Abort = true;
                    ControlsGlobals.UserInterface.ShowMessageBox(result, "Print Error!", RsMessageBoxIcons.Error);
                    output.Abort = true;
                }

                page += headerRows.Count;
                printerSetupArgs.PrintingProcessingViewModel
                    .UpdateStatus(page, printerSetupArgs.TotalRecords, ProcessTypes.ImportHeader);

            };
            lookupData.DoPrintOutput(10);
            if (printerSetupArgs.PrintingProcessingViewModel.Abort)
            {
                return;
            }


            MakeAdditionalFilter(printerSetupArgs);
        }

        /// <summary>
        /// Makes the additional filter.
        /// </summary>
        /// <param name="printerSetupArgs">The printer setup arguments.</param>
        public static void MakeAdditionalFilter(PrinterSetupArgs printerSetupArgs)
        {
            var fixedText = GetFiltersText(printerSetupArgs.LookupDefinition.FilterDefinition
                .FixedFilters.ToList(), printerSetupArgs);

            var userText = GetFiltersText(printerSetupArgs.LookupDefinition.FilterDefinition.UserFilters.ToList(),
                printerSetupArgs);

            var result = string.Empty;
            if (!fixedText.IsNullOrEmpty() && !userText.IsNullOrEmpty())
            {
                result = $"({fixedText}) AND\r\n({userText})";
            }
            else
            {
                result = fixedText + userText;
            }

            printerSetupArgs.PrintingProperties.AdditionalFilter = result;
        }

        /// <summary>
        /// Gets the filters text.
        /// </summary>
        /// <param name="filterItemDefinitions">The filter item definitions.</param>
        /// <param name="printerSetupArgs">The printer setup arguments.</param>
        /// <returns>System.String.</returns>
        private static string GetFiltersText(List<FilterItemDefinition> filterItemDefinitions, PrinterSetupArgs printerSetupArgs)
        {
            var result = string.Empty;
            var count = filterItemDefinitions.Count;
            if (count == 0)
            {
                return result;
            }
            var index = 0;
            var endText = string.Empty;
            foreach (var filterItem in filterItemDefinitions)
            {
                if (!printerSetupArgs.ReportFilters.Contains(filterItem))
                {
                    endText = filterItem.PrintEndLogicText();
                    result += filterItem.GetPrintText(printerSetupArgs.LookupDefinition);
                    var end = index == filterItemDefinitions.Count - 1;

                    if (!end)
                    {
                        result += filterItem.PrintEndLogicText();
                    }
                }
                index++;
            }

            if (!endText.IsNullOrEmpty() && !result.IsNullOrEmpty())
            {
                var indexOfEnd = result.LastIndexOf(endText);
                if (indexOfEnd != -1)
                {
                    var rightText = result.GetRightText(indexOfEnd, 0);
                    if (indexOfEnd + rightText.Length == indexOfEnd + endText.Length)
                    {
                        result = result.LeftStr(indexOfEnd);
                    }
                }

            }

            return result;
        }

        /// <summary>
        /// Occurs when [print processing header].
        /// </summary>
        public event EventHandler<PrinterDataProcessedEventArgs> PrintProcessingHeader;
        /// <summary>
        /// Notifies the processing header.
        /// </summary>
        /// <param name="args">The <see cref="T:RingSoft.DbLookup.PrinterDataProcessedEventArgs" /> instance containing the event data.</param>
        public void NotifyProcessingHeader(PrinterDataProcessedEventArgs args)
        {
            PrintProcessingHeader?.Invoke(this, args);
        }

        /// <summary>
        /// Registers the lookup.  Used for lookups that only connect to header table with no additional filters.
        /// Automatically refreshes lookup when view model state changes.
        /// </summary>
        /// <param name="lookupDefinition">The lookup definition.</param>
        /// <param name="addViewParameter">The add view parameter.</param>
        public void RegisterLookup(LookupDefinitionBase lookupDefinition, object addViewParameter = null)
        {
            var lookupMap = new LookupMap
            {
                LookupDefinition = lookupDefinition,
                AddViewParameter = addViewParameter,
            };
            _lookups.Add(lookupMap);
        }

        /// <summary>
        /// Registers the grid.
        /// </summary>
        /// <param name="grid">The grid.</param>
        /// <param name="readOnly">if set to <c>true</c> [read only].</param>
        public virtual void RegisterGrid(DbMaintenanceDataEntryGridManagerBase grid, bool readOnly = false)
        {
            var gridMap = new GridMap(grid, readOnly);
            _grids.Add(gridMap);
        }

        /// <summary>
        /// Deletes the children.
        /// </summary>
        /// <param name="deleteTables">The delete tables.</param>
        /// <param name="procedure">The procedure.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public abstract bool DeleteChildren(DeleteTables deleteTables, ITwoTierProcessingProcedure procedure);

        /// <summary>
        /// Does the get delete tables.
        /// </summary>
        /// <param name="fields">The fields.</param>
        /// <param name="deleteTables">The delete tables.</param>
        /// <param name="procedure">The procedure.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public abstract bool DoGetDeleteTables(
            List<FieldDefinition> fields
            , DeleteTables deleteTables
            , ITwoTierProcessingProcedure procedure);

        /// <summary>
        /// Executed when a property value has changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="raiseDirtyFlag">if set to <c>true</c> set RecordDirty property to true indicating the user has changed data.</param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null, bool raiseDirtyFlag = true)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
