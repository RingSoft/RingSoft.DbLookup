namespace RingSoft.DbLookup.Lookup
{
    public enum LookupCommands
    {
        Clear = 0,
        Refresh = 1,
        AddModify = 2
    }

    /// <summary>
    /// This object is used for data binding a LookupControl to view models so that view models can control the LookupControl's behavior.
    /// </summary>
    public class LookupCommand
    {
        /// <summary>
        /// Gets the command to send to the LookupControl.
        /// </summary>
        /// <value>
        /// The command.
        /// </value>
        public LookupCommands Command { get; }

        /// <summary>
        /// Gets the parent window's primary key value.  Used when the user wishes to add or view a record on-the-fly.
        /// </summary>
        /// <value>
        /// The parent window's primary key value.
        /// </value>
        public PrimaryKeyValue ParentWindowPrimaryKeyValue { get; }

        /// <summary>
        /// Gets a value indicating whether to reset the Lookup Control's Search For text box when refreshing data.
        /// </summary>
        /// <value>
        ///   <c>true</c> if reset search for; otherwise, <c>false</c>.
        /// </value>
        public bool ResetSearchFor { get; }

        public object AddViewParameter { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupCommand"/> class.
        /// </summary>
        /// <param name="command">The command.</param>
        public LookupCommand(LookupCommands command)
        {
            Command = command;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupCommand"/> class.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="parentWindowPrimaryKeyValue">The parent window's primary key value.</param>
        /// <param name="resetSearchFor">if set to <c>true</c> reset search for.</param>
        public LookupCommand(LookupCommands command, PrimaryKeyValue parentWindowPrimaryKeyValue, bool resetSearchFor)
        {
            Command = command;
            ParentWindowPrimaryKeyValue = parentWindowPrimaryKeyValue;
            ResetSearchFor = resetSearchFor;
        }
    }
}
