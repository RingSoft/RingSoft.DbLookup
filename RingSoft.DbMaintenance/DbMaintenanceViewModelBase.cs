using System.ComponentModel;
using System.Runtime.CompilerServices;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.Lookup;

namespace RingSoft.DbMaintenance
{
    public enum DbMaintenanceResults
    {
        Success,
        ValidationError,
        DatabaseError
    }

    public enum DbMaintenanceModes
    {
        AddMode = 0,
        EditMode = 1
    }

    /// <summary>
    /// The base class for database maintenance operations.
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    public abstract class DbMaintenanceViewModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets the view lookup definition used to get the next and previous record.
        /// </summary>
        /// <value>
        /// The view lookup definition.
        /// </value>
        protected LookupDefinitionBase ViewLookupDefinition { get; private set; }

        /// <summary>
        /// Gets the view.
        /// </summary>
        /// <value>
        /// The view.
        /// </value>
        protected IDbMaintenanceView View { get; private set; }

        /// <summary>
        /// Gets the initial search for text when the Find button is clicked.  By default it is the key auto fill text.
        /// </summary>
        /// <value>
        /// The find button initial search for.
        /// </value>
        protected virtual string FindButtonInitialSearchFor { get; } = string.Empty;

        private AutoFillSetup _keyAutoFillSetup;

        /// <summary>
        /// Gets the unique key control auto fill setup.
        /// </summary>
        /// <value>
        /// The unique key control auto fill setup.
        /// </value>
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

        private AutoFillValue _keyAutoFillValue;
        /// <summary>
        /// Gets or sets the unique key control auto fill value.
        /// </summary>
        /// <value>
        /// The key auto fill value.
        /// </value>
        public AutoFillValue KeyAutoFillValue
        {
            get => _keyAutoFillValue;
            set
            {
                if (_keyAutoFillValue == value)
                    return;

                _keyAutoFillValue = value;
                OnPropertyChanged();
            }
        }

        private bool _primaryKeyControlsEnabled;

        /// <summary>
        /// Gets or sets a value indicating whether the primary key controls are enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if primary key controls enabled; otherwise, <c>false</c>.
        /// </value>
        public bool PrimaryKeyControlsEnabled
        {
            get => _primaryKeyControlsEnabled;
            set
            {
                if (_primaryKeyControlsEnabled == value)
                    return;

                _primaryKeyControlsEnabled = value;
                OnPropertyChanged(nameof(PrimaryKeyControlsEnabled));
            }
        }

        private bool _selectButtonEnabled;
        /// <summary>
        /// Gets or sets a value indicating whether the Select button is enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the select button is enabled; otherwise, <c>false</c>.
        /// </value>
        public bool SelectButtonEnabled
        {
            get => _selectButtonEnabled;
            set
            {
                if (_selectButtonEnabled == value)
                    return;
                _selectButtonEnabled = value;
                OnPropertyChanged(nameof(SelectButtonEnabled));
            }
        }

        private bool _deleteButtonEnabled;
        /// <summary>
        /// Gets or sets a value indicating whether the Delete button is enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the delete button is enabled; otherwise, <c>false</c>.
        /// </value>
        public bool DeleteButtonEnabled
        {
            get => _deleteButtonEnabled;
            set
            {
                if (_deleteButtonEnabled == value)
                    return;

                _deleteButtonEnabled = value;
                OnPropertyChanged(nameof(DeleteButtonEnabled));
            }
        }

        private bool _keyValueDirty;

        /// <summary>
        /// Gets or sets a value indicating whether the data in the user-editable unique key value control has been changed.  Used when focus leaves the unique user-editable control.  When that happens and this value is true, then the record matching that unique value is automatically loaded.
        /// </summary>
        /// <value>
        ///   <c>true</c> if data in the key value control has changed; otherwise, <c>false</c>.
        /// </value>
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
        /// <value>
        /// The maintenance mode.
        /// </value>
        protected DbMaintenanceModes MaintenanceMode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user has changed the data on this record and has yet to save.
        /// </summary>
        /// <value>
        ///   <c>true</c> if there are unsaved changes in this record; otherwise, <c>false</c>.
        /// </value>
        public bool RecordDirty { get; set; }


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
            View = view;
            InternalInitialize();
        }

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
        public abstract DbMaintenanceResults OnSaveButton();

        /// <summary>
        /// Executed when the Delete button is clicked.
        /// </summary>
        /// <returns>The result.</returns>
        public abstract DbMaintenanceResults OnDeleteButton();

        /// <summary>
        /// Executed when the key control looses focus.  Used to ensure no duplicate value in the key control is entered.
        /// </summary>
        public abstract void OnKeyControlLeave();

        /// <summary>
        /// Executed when the user is trying to close the view.
        /// </summary>
        /// <param name="e">The <see cref="CancelEventArgs"/> instance containing the event data.</param>
        public abstract void OnWindowClosing(CancelEventArgs e);

        /// <summary>
        /// Initializes from lookup data that is sent when the user wishes to add/view lookup data on the fly.
        /// </summary>
        /// <param name="e">The e.</param>
        public abstract void InitializeFromLookupData(LookupAddViewArgs e);

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

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
