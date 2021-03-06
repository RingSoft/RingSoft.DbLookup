﻿using System.Data;

namespace RingSoft.DbLookup.AutoFill
{
    /// <summary>
    /// Argument sent in the AutoFillChanged event.
    /// </summary>
    public class AutoFillDataChangedArgs
    {
        /// <summary>
        /// Gets a value indicating whether to refresh the contains list.
        /// </summary>
        /// <value>
        ///   <c>true</c> if refresh contains list; otherwise, <c>false</c>.
        /// </value>
        public bool RefreshContainsList { get; internal set; }

        /// <summary>
        /// Gets the contains box data table.
        /// </summary>
        /// <value>
        /// The contains box data table.
        /// </value>
        public DataTable ContainsBoxDataTable { get; internal set; }
    }
}
