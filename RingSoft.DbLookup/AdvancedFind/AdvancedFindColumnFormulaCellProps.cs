// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-16-2023
// ***********************************************************************
// <copyright file="AdvancedFindColumnFormulaCellProps.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup.Lookup;

namespace RingSoft.DbLookup.AdvancedFind
{
    /// <summary>
    /// Class AdvancedFindColumnFormulaCellProps.
    /// Implements the <see cref="DataEntryGridEditingCellProps" />
    /// </summary>
    /// <seealso cref="DataEntryGridEditingCellProps" />
    public class AdvancedFindColumnFormulaCellProps : DataEntryGridEditingCellProps
    {
        /// <summary>
        /// Gets or sets the lookup formula column.
        /// </summary>
        /// <value>The lookup formula column.</value>
        public LookupFormulaColumnDefinition LookupFormulaColumn { get; set; }

        /// <summary>
        /// The column formula cell identifier
        /// </summary>
        public const int ColumnFormulaCellId = 53;
        /// <summary>
        /// Initializes a new instance of the <see cref="AdvancedFindColumnFormulaCellProps" /> class.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="columnId">The column identifier.</param>
        /// <param name="formulaColumn">The formula column.</param>
        public AdvancedFindColumnFormulaCellProps(DataEntryGridRow row, int columnId,
            LookupFormulaColumnDefinition formulaColumn) : base(row, columnId)
        {
            LookupFormulaColumn = formulaColumn;
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
            return "<Formula>";
        }

        /// <summary>
        /// Gets the editing control identifier.
        /// </summary>
        /// <value>The editing control identifier.</value>
        public override int EditingControlId => ColumnFormulaCellId;
    }
}
