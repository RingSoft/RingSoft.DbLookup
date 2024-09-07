// ***********************************************************************
// Assembly         : RingSoft.DbMaintenance
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-16-2023
// ***********************************************************************
// <copyright file="AdvancedFindFilterCellProps.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.TableProcessing;

namespace RingSoft.DbMaintenance
{
    /// <summary>
    /// Class AdvancedFindFilterCellProps.
    /// Implements the <see cref="AdvancedFindMemoCellProps" />
    /// </summary>
    /// <seealso cref="AdvancedFindMemoCellProps" />
    public class AdvancedFindFilterCellProps : AdvancedFindMemoCellProps
    {
        /// <summary>
        /// The filter control identifier
        /// </summary>
        public const int FilterControlId = 55;

        /// <summary>
        /// Gets or sets the filter return.
        /// </summary>
        /// <value>The filter return.</value>
        public AdvancedFilterReturn FilterReturn { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdvancedFindFilterCellProps" /> class.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="columnId">The column identifier.</param>
        /// <param name="text">The text.</param>
        /// <param name="filter">The filter.</param>
        public AdvancedFindFilterCellProps(DataEntryGridRow row, int columnId, string text, 
            AdvancedFilterReturn filter) : base(row, columnId, text)
        {
            Text = text;
            FilterReturn = filter;
        }

        /// <summary>
        /// Gets the editing control identifier.
        /// </summary>
        /// <value>The editing control identifier.</value>
        public override int EditingControlId => FilterControlId;

        /// <summary>
        /// Gets the data value.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="columnId">The column identifier.</param>
        /// <param name="controlMode">if set to <c>true</c> [control mode].</param>
        /// <returns>System.String.</returns>
        protected override string GetDataValue(DataEntryGridRow row, int columnId, bool controlMode)
        {
            return Text;
            //return base.GetDataValue(row, columnId, controlMode);
        }
    }
}
