// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-19-2022
// ***********************************************************************
// <copyright file="DataEntryGridAutoFillCellProps.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup.AutoFill;

namespace RingSoft.DbLookup
{
    /// <summary>
    /// Class DataEntryGridAutoFillCellProps.
    /// Implements the <see cref="DataEntryGridEditingCellProps" />
    /// </summary>
    /// <seealso cref="DataEntryGridEditingCellProps" />
    public class DataEntryGridAutoFillCellProps : DataEntryGridEditingCellProps
    {
        /// <summary>
        /// Gets the data value.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="columnId">The column identifier.</param>
        /// <param name="controlMode">if set to <c>true</c> [control mode].</param>
        /// <returns>System.String.</returns>
        protected override string GetDataValue(DataEntryGridRow row, int columnId, bool controlMode)
        {
            if (AutoFillValue != null)
                return AutoFillValue.Text;

            return string.Empty;
        }

        /// <summary>
        /// Gets the automatic fill setup.
        /// </summary>
        /// <value>The automatic fill setup.</value>
        public AutoFillSetup AutoFillSetup { get; }

        /// <summary>
        /// Gets or sets the automatic fill value.
        /// </summary>
        /// <value>The automatic fill value.</value>
        public AutoFillValue AutoFillValue { get; set; }

        /// <summary>
        /// The automatic fill control host identifier
        /// </summary>
        public const int AutoFillControlHostId = 51;

        /// <summary>
        /// Gets or sets a value indicating whether [always update on select].
        /// </summary>
        /// <value><c>true</c> if [always update on select]; otherwise, <c>false</c>.</value>
        public bool AlwaysUpdateOnSelect { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [tab on select].
        /// </summary>
        /// <value><c>true</c> if [tab on select]; otherwise, <c>false</c>.</value>
        public bool TabOnSelect { get; set; } = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataEntryGridAutoFillCellProps"/> class.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="columnId">The column identifier.</param>
        /// <param name="setup">The setup.</param>
        /// <param name="value">The value.</param>
        public DataEntryGridAutoFillCellProps(DataEntryGridRow row, int columnId, AutoFillSetup setup, AutoFillValue value) : base(row, columnId)
        {
            AutoFillSetup = setup;
            AutoFillValue = value;
        }

        /// <summary>
        /// Gets the editing control identifier.
        /// </summary>
        /// <value>The editing control identifier.</value>
        public override int EditingControlId => AutoFillControlHostId;
    }
}
