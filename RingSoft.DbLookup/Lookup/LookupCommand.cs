using System;

namespace RingSoft.DbLookupCore.Lookup
{
    public enum LookupCommands
    {
        Clear = 0,
        Refresh = 1,
        AddModify = 2
    }

    /// <summary>
    /// This object is used for data binding a LookupControl to view models so that view models can control LookupControl behavior.
    /// </summary>
    public class LookupCommand
    {
        /// <summary>
        /// Gets the command.
        /// </summary>
        /// <value>
        /// The command.
        /// </value>
        public LookupCommands Command { get; }

        /// <summary>
        /// Gets the parent window's primary key value.
        /// </summary>
        /// <value>
        /// The parent window's primary key value.
        /// </value>
        public PrimaryKeyValue ParentWindowPrimaryKeyValue { get; }

        /// <summary>
        /// Gets a value indicating whether to reset search for when refreshing data.
        /// </summary>
        /// <value>
        ///   <c>true</c> if reset search for; otherwise, <c>false</c>.
        /// </value>
        public bool ResetSearchFor { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupCommand"/> class.
        /// </summary>
        /// <param name="command">The command.</param>
        public LookupCommand(LookupCommands command)
        {
            Command = command;
            if (command == LookupCommands.Refresh)
                throw new ArgumentException("The Refresh command must pass in the parent window's primary key value.");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupCommand"/> class.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="parentWindowPrimaryKeyValue">The parent window primary key value.</param>
        /// <param name="resetSearchFor">if set to <c>true</c> reset search for.</param>
        public LookupCommand(LookupCommands command, PrimaryKeyValue parentWindowPrimaryKeyValue, bool resetSearchFor)
        {
            Command = command;
            ParentWindowPrimaryKeyValue = parentWindowPrimaryKeyValue;
            ResetSearchFor = resetSearchFor;
        }
    }
}
