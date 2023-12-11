// ***********************************************************************
// Assembly         : RingSoft.DbMaintenance
// Author           : petem
// Created          : 02-04-2023
//
// Last Modified By : petem
// Last Modified On : 07-23-2023
// ***********************************************************************
// <copyright file="AdvancedFindFormulaColumnRow.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.Lookup;
using System;

namespace RingSoft.DbMaintenance
{
    /// <summary>
    /// Class AdvancedFindFormulaColumnRow.
    /// Implements the <see cref="RingSoft.DbMaintenance.AdvancedFindColumnRow" />
    /// </summary>
    /// <seealso cref="RingSoft.DbMaintenance.AdvancedFindColumnRow" />
    public class AdvancedFindFormulaColumnRow : AdvancedFindColumnRow
    {
        /// <summary>
        /// Gets the formula column.
        /// </summary>
        /// <value>The formula column.</value>
        public LookupFormulaColumnDefinition FormulaColumn { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdvancedFindFormulaColumnRow"/> class.
        /// </summary>
        /// <param name="manager">The manager.</param>
        public AdvancedFindFormulaColumnRow(AdvancedFindColumnsManager manager) : base(manager)
        {
        }

        /// <summary>
        /// Gets the cell props.
        /// </summary>
        /// <param name="columnId">The column identifier.</param>
        /// <returns>DataEntryGridCellProps.</returns>
        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (AdvancedFindColumnColumns)columnId;
            switch (column)
            {
                case AdvancedFindColumnColumns.Field:
                    return new AdvancedFindColumnFormulaCellProps(this, columnId, FormulaColumn);
            }

            return base.GetCellProps(columnId);
        }

        /// <summary>
        /// Sets the cell value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public override void SetCellValue(DataEntryGridEditingCellProps value)
        {
            var column = (AdvancedFindColumnColumns)value.ColumnId;
            switch (column)
            {
                case AdvancedFindColumnColumns.Table:
                    break;
                case AdvancedFindColumnColumns.Field:
                    if (value is AdvancedFindColumnFormulaCellProps formulaCellProps)
                    {
                        Manager.ViewModel.ResetLookup();
                        return;
                    }
                    break;
                case AdvancedFindColumnColumns.Name:
                    break;
                case AdvancedFindColumnColumns.PercentWidth:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            base.SetCellValue(value);
        }

        /// <summary>
        /// Loads from column definition.
        /// </summary>
        /// <param name="column">The column.</param>
        public override void LoadFromColumnDefinition(LookupColumnDefinitionBase column)
        {
            if (column is LookupFormulaColumnDefinition lookupFormulaColumn)
            {
                FormulaColumn = lookupFormulaColumn;
            }
            base.LoadFromColumnDefinition(column);
        }
    }
}
