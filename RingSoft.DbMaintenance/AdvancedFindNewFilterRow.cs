// ***********************************************************************
// Assembly         : RingSoft.DbMaintenance
// Author           : petem
// Created          : 02-23-2023
//
// Last Modified By : petem
// Last Modified On : 12-16-2023
// ***********************************************************************
// <copyright file="AdvancedFindNewFilterRow.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Linq;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;

namespace RingSoft.DbMaintenance
{
    /// <summary>
    /// Class AdvancedFindNewFilterRow.
    /// Implements the <see cref="RingSoft.DbMaintenance.AdvancedFindFilterRow" />
    /// </summary>
    /// <seealso cref="RingSoft.DbMaintenance.AdvancedFindFilterRow" />
    public class AdvancedFindNewFilterRow : AdvancedFindFilterRow
    {
        /// <summary>
        /// The allow delete
        /// </summary>
        private bool _allowDelete;
        /// <summary>
        /// Initializes a new instance of the <see cref="AdvancedFindNewFilterRow" /> class.
        /// </summary>
        /// <param name="manager">The manager.</param>
        public AdvancedFindNewFilterRow(AdvancedFindFiltersManager manager) : base(manager)
        {
            IsNew = true;
        }

        /// <summary>
        /// Sets the allow delete.
        /// </summary>
        /// <param name="allowDelete">if set to <c>true</c> [allow delete].</param>
        public void SetAllowDelete(bool allowDelete)
        {
            _allowDelete = allowDelete;
        }

        /// <summary>
        /// Gets the cell props.
        /// </summary>
        /// <param name="columnId">The column identifier.</param>
        /// <returns>DataEntryGridCellProps.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (AdvancedFindFiltersManager.FilterColumns)columnId;

            switch (column)
            {
                case AdvancedFindFiltersManager.FilterColumns.LeftParentheses:
                    break;
                case AdvancedFindFiltersManager.FilterColumns.Table:
                    break;
                case AdvancedFindFiltersManager.FilterColumns.Field:
                    break;
                case AdvancedFindFiltersManager.FilterColumns.Search:
                    return new DataEntryGridTextCellProps(this, columnId, "Click 'Add Filter' to add a filter here.");
                case AdvancedFindFiltersManager.FilterColumns.RightParentheses:
                    break;
                case AdvancedFindFiltersManager.FilterColumns.EndLogic:
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

        /// <summary>
        /// Disposes this instance.
        /// </summary>
        public override void Dispose()
        {
            //base.Dispose();
        }
    }
}
