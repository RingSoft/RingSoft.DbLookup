// ***********************************************************************
// Assembly         : RingSoft.DbMaintenance
// Author           : petem
// Created          : 02-23-2023
//
// Last Modified By : petem
// Last Modified On : 12-16-2023
// ***********************************************************************
// <copyright file="AdvancedFindNewColumnRow.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;

namespace RingSoft.DbMaintenance
{
    /// <summary>
    /// Class AdvancedFindNewColumnRow.
    /// Implements the <see cref="RingSoft.DbMaintenance.AdvancedFindColumnRow" />
    /// </summary>
    /// <seealso cref="RingSoft.DbMaintenance.AdvancedFindColumnRow" />
    public class AdvancedFindNewColumnRow : AdvancedFindColumnRow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdvancedFindNewColumnRow" /> class.
        /// </summary>
        /// <param name="manager">The manager.</param>
        public AdvancedFindNewColumnRow(AdvancedFindColumnsManager manager) : base(manager)
        {
            IsNew = true;
        }

        /// <summary>
        /// Gets the cell props.
        /// </summary>
        /// <param name="columnId">The column identifier.</param>
        /// <returns>DataEntryGridCellProps.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (AdvancedFindColumnColumns)columnId;

            switch (column)
            {
                case AdvancedFindColumnColumns.Table:
                    break;
                case AdvancedFindColumnColumns.Field:
                    break;
                case AdvancedFindColumnColumns.Name:
                    return new DataEntryGridTextCellProps(this, columnId, "Click 'Add Column' to add a new column here.");
                case AdvancedFindColumnColumns.PercentWidth:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return new DataEntryGridTextCellProps(this, columnId);
        }

        /// <summary>
        /// Gets the cell style.
        /// </summary>
        /// <param name="columnId">The column identifier.</param>
        /// <returns>DataEntryGridCellStyle.</returns>
        public override DataEntryGridCellStyle GetCellStyle(int columnId)
        {
            return new DataEntryGridCellStyle { State = DataEntryGridCellStates.Disabled };
        }
    }
}
