// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-16-2023
// ***********************************************************************
// <copyright file="AdvancedFilterParenthesesCellProps.cs" company="Peter Ringering">
//     Copyright (c)2023 . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DataEntryControls.Engine.DataEntryGrid;

namespace RingSoft.DbLookup.AdvancedFind
{
    /// <summary>
    /// Class AdvancedFilterParenthesesCellProps.
    /// Implements the <see cref="DataEntryGridTextCellProps" />
    /// </summary>
    /// <seealso cref="DataEntryGridTextCellProps" />
    public class AdvancedFilterParenthesesCellProps : DataEntryGridTextCellProps
    {
        /// <summary>
        /// The parentheses host identifier
        /// </summary>
        public const int ParenthesesHostId = 60;

        /// <summary>
        /// Gets or sets the limit character.
        /// </summary>
        /// <value>The limit character.</value>
        public char LimitChar { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdvancedFilterParenthesesCellProps" /> class.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="columnId">The column identifier.</param>
        /// <param name="limitChar">The limit character.</param>
        public AdvancedFilterParenthesesCellProps(DataEntryGridRow row, int columnId, char limitChar) : base(row,
            columnId)
        {
            LimitChar = limitChar;  
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdvancedFilterParenthesesCellProps" /> class.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="columnId">The column identifier.</param>
        /// <param name="text">The text.</param>
        /// <param name="limitChar">The limit character.</param>
        public AdvancedFilterParenthesesCellProps(DataEntryGridRow row, int columnId, string text, char limitChar) :
            base(row, columnId, text)
        {
            LimitChar = limitChar;
        }

        /// <summary>
        /// Gets the editing control identifier.
        /// </summary>
        /// <value>The editing control identifier.</value>
        public override int EditingControlId => ParenthesesHostId;
    }
}
