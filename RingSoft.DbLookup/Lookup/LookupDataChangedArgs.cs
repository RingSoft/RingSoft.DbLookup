// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 01-28-2023
// ***********************************************************************
// <copyright file="LookupDataChangedArgs.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Data;

namespace RingSoft.DbLookup.Lookup
{
    /// <summary>
    /// Class LookupDataChangedArgs.
    /// </summary>
    public class LookupDataChangedArgs
    {
        /// <summary>
        /// Gets the lookup results data table.
        /// </summary>
        /// <value>The lookup results data table.</value>
        public DataTable OutputTable { get; }

        /// <summary>
        /// Gets the index of the selected row.
        /// </summary>
        /// <value>The index of the selected row.</value>
        public int SelectedRowIndex { get; }

        /// <summary>
        /// Gets the scroll position.
        /// </summary>
        /// <value>The scroll position.</value>
        public LookupScrollPositions ScrollPosition { get; }

        /// <summary>
        /// Gets a value indicating whether records are being counted.
        /// </summary>
        /// <value><c>true</c> if counting records; otherwise, <c>false</c>.</value>
        public bool CountingRecords { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether search for is changing.
        /// </summary>
        /// <value><c>true</c> if search for is changing; otherwise, <c>false</c>.</value>
        public bool SearchForChanging { get; internal set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="LookupDataChangedArgs"/> is abort.
        /// </summary>
        /// <value><c>true</c> if abort; otherwise, <c>false</c>.</value>
        public bool Abort { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupDataChangedArgs"/> class.
        /// </summary>
        /// <param name="outputTable">The output table.</param>
        /// <param name="selectedRowIndex">Index of the selected row.</param>
        /// <param name="scrollPosition">The scroll position.</param>
        internal LookupDataChangedArgs(DataTable outputTable, int selectedRowIndex, LookupScrollPositions scrollPosition)
        {
            OutputTable = outputTable;
            SelectedRowIndex = selectedRowIndex;
            ScrollPosition = scrollPosition;
        }
    }
}
