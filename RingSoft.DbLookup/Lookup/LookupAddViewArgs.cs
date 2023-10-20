namespace RingSoft.DbLookup.Lookup
{
    public enum LookupFormModes
    {
        Add = 0,
        View = 1
    }

    /// <summary>
    /// Arguments sent when the user wants to select or view a lookup row.
    /// </summary>
    public class LookupAddViewArgs
    {
        /// <summary>
        /// Gets the lookup data.
        /// </summary>
        /// <value>
        /// The lookup data.
        /// </value>
        public LookupDataMauiBase LookupData { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="LookupAddViewArgs"/> is handled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if handled; otherwise, <c>false</c>.
        /// </value>
        public bool Handled { get; set; }

        /// <summary>
        /// Gets a value indicating whether this event came from a lookup control.
        /// </summary>
        /// <value>
        ///   <c>true</c> if from a lookup control; otherwise, <c>false</c>.
        /// </value>
        public bool FromLookupControl { get; private set; }

        /// <summary>
        /// Gets the lookup form mode.
        /// </summary>
        /// <value>
        /// The lookup form mode.
        /// </value>
        public LookupFormModes LookupFormMode { get; private set; }

        /// <summary>
        /// Gets the initial add mode text.
        /// </summary>
        /// <value>
        /// The initial add mode text.
        /// </value>
        public string InitialAddModeText { get; private set; }

        /// <summary>
        /// Gets or sets the parent window's primary key value.
        /// </summary>
        /// <value>
        /// The parent window's primary key value.
        /// </value>
        public PrimaryKeyValue ParentWindowPrimaryKeyValue { get; set; }

        /// <summary>
        /// Gets the call back token.
        /// </summary>
        /// <value>
        /// The call back token.
        /// </value>
        public LookupCallBackToken CallBackToken { get; }

        /// <summary>
        /// Gets the owner window that is firing this event.  For use in setting owner property of the WPF window associated with the Table Definition.
        /// </summary>
        /// <value>
        /// The owner window.
        /// </value>
        public object OwnerWindow { get; set; }

        /// <summary>
        /// Gets the new record's primary key value object.  Used to add a new record in a Db maintenance window with multiple primary key fields.
        /// </summary>
        /// <value>
        /// The new record primary key value.
        /// </value>
        public PrimaryKeyValue NewRecordPrimaryKeyValue { get; internal set; }

        public object InputParameter { get; set; }

        public bool AllowEdit { get; set; } = true;

        public bool LookupReadOnlyMode { get; set; }

        public PrimaryKeyValue ReadOnlyPrimaryKeyValue { get; set; }

        public PrimaryKeyValue SelectedPrimaryKeyValue { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupAddViewArgs" /> class.
        /// </summary>
        /// <param name="lookupData">The lookup data.</param>
        /// <param name="fromLookupControl">if set to <c>true</c> then this is from a lookup control.</param>
        /// <param name="lookupFormMode">The lookup form mode.</param>
        /// <param name="initialAddModeText">The initial add mode text.</param>
        /// <param name="ownerWindow">The owner window that launched this.</param>
        public LookupAddViewArgs(LookupDataMauiBase lookupData, bool fromLookupControl, LookupFormModes lookupFormMode,
            string initialAddModeText, object ownerWindow)
        {
            LookupData = lookupData;
            FromLookupControl = fromLookupControl;
            LookupFormMode = lookupFormMode;
            InitialAddModeText = initialAddModeText;
            CallBackToken = new LookupCallBackToken();
            OwnerWindow = ownerWindow;
        }
    }
}
