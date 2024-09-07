// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-16-2023
// ***********************************************************************
// <copyright file="LookupCommand.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace RingSoft.DbLookup.Lookup
{
    /// <summary>
    /// Enum LookupCommands
    /// </summary>
    public enum LookupCommands
    {
        /// <summary>
        /// The clear
        /// </summary>
        Clear = 0,
        /// <summary>
        /// The refresh
        /// </summary>
        Refresh = 1,
        /// <summary>
        /// The add modify
        /// </summary>
        AddModify = 2,
        /// <summary>
        /// The reset
        /// </summary>
        Reset = 3
    }

    /// <summary>
    /// This object is used for data binding a LookupControl to view models so that view models can control the LookupControl's behavior.
    /// </summary>
    public class LookupCommand
    {
        /// <summary>
        /// Gets the command to send to the LookupControl.
        /// </summary>
        /// <value>The command.</value>
        public LookupCommands Command { get; }

        /// <summary>
        /// Gets the parent window's primary key value.  Used when the user wishes to add or view a record on-the-fly.
        /// </summary>
        /// <value>The parent window's primary key value.</value>
        public PrimaryKeyValue ParentWindowPrimaryKeyValue { get; }

        /// <summary>
        /// Gets a value indicating whether to reset the Lookup Control's Search For text box when refreshing data.
        /// </summary>
        /// <value><c>true</c> if reset search for; otherwise, <c>false</c>.</value>
        public bool ResetSearchFor { get; set; }

        /// <summary>
        /// Gets or sets the add view parameter.
        /// </summary>
        /// <value>The add view parameter.</value>
        public object AddViewParameter { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupCommand" /> class.
        /// </summary>
        /// <param name="command">The command.</param>
        public LookupCommand(LookupCommands command)
        {
            Command = command;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [clear columns].
        /// </summary>
        /// <value><c>true</c> if [clear columns]; otherwise, <c>false</c>.</value>
        public bool ClearColumns { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupCommand" /> class.
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
