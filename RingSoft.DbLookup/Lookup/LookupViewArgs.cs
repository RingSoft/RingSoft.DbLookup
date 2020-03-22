namespace RingSoft.DbLookup.Lookup
{
    public enum LookupFormModes
    {
        Add = 0,
        View = 1
    }

    /// <summary>
    /// Arguments sent when the user wants to select or view a lookup record.
    /// </summary>
    public class LookupAddViewArgs
    {
        /// <summary>
        /// Gets the lookup data.
        /// </summary>
        /// <value>
        /// The lookup data.
        /// </value>
        public LookupDataBase LookupData { get; private set; }

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
        /// Initializes a new instance of the <see cref="LookupAddViewArgs" /> class.
        /// </summary>
        /// <param name="lookupData">The lookup data.</param>
        /// <param name="fromLookupControl">if set to <c>true</c> then this is from a lookup control.</param>
        /// <param name="lookupFormMode">The lookup form mode.</param>
        /// <param name="initialAddModeText">The initial add mode text.</param>
        public LookupAddViewArgs(LookupDataBase lookupData, bool fromLookupControl, LookupFormModes lookupFormMode, string initialAddModeText)
        {
            LookupData = lookupData;
            FromLookupControl = fromLookupControl;
            LookupFormMode = lookupFormMode;
            InitialAddModeText = initialAddModeText;
            CallBackToken = new LookupCallBackToken();
        }
    }
}
