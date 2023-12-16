// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-19-2022
// ***********************************************************************
// <copyright file="AdvancedFindMemoCellProps.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;

namespace RingSoft.DbLookup.AdvancedFind
{
    /// <summary>
    /// Class AdvancedFindMemoCellProps.
    /// Implements the <see cref="DataEntryGridEditingCellProps" />
    /// </summary>
    /// <seealso cref="DataEntryGridEditingCellProps" />
    public class AdvancedFindMemoCellProps : DataEntryGridEditingCellProps
    {
        /// <summary>
        /// Enum MemoFormMode
        /// </summary>
        public enum MemoFormMode
        {
            /// <summary>
            /// The formula
            /// </summary>
            Formula = 0,
            /// <summary>
            /// The caption
            /// </summary>
            Caption = 1
        }

        /// <summary>
        /// Gets or sets the form mode.
        /// </summary>
        /// <value>The form mode.</value>
        public MemoFormMode FormMode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [read only mode].
        /// </summary>
        /// <value><c>true</c> if [read only mode]; otherwise, <c>false</c>.</value>
        public bool ReadOnlyMode { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text { get; set; }

        /// <summary>
        /// The advanced find memo host identifier
        /// </summary>
        public const int AdvancedFindMemoHostId = 52;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdvancedFindMemoCellProps"/> class.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="columnId">The column identifier.</param>
        /// <param name="text">The text.</param>
        public AdvancedFindMemoCellProps(DataEntryGridRow row, int columnId, string text) : base(row, columnId)
        {
            Text = text;
        }

        /// <summary>
        /// Gets the data value.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="columnId">The column identifier.</param>
        /// <param name="controlMode">if set to <c>true</c> [control mode].</param>
        /// <returns>System.String.</returns>
        protected override string GetDataValue(DataEntryGridRow row, int columnId, bool controlMode)
        {
            switch (FormMode)
            {
                case MemoFormMode.Caption:
                    if (!Text.IsNullOrEmpty() && Text.Contains('\n'))
                        return "<Multi-Line Caption>";
                    else
                        return Text;
                    break;
                default:
                    return "<Formula>";
            }
        }

        /// <summary>
        /// Gets the editing control identifier.
        /// </summary>
        /// <value>The editing control identifier.</value>
        public override int EditingControlId => AdvancedFindMemoHostId;
    }
}
