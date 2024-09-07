// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-16-2023
// ***********************************************************************
// <copyright file="LookupUserInterface.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.DataProcessor;

namespace RingSoft.DbLookup.Lookup
{
    /// <summary>
    /// Class LookupUserInterface.
    /// Implements the <see cref="RingSoft.DbLookup.Lookup.ILookupControl" />
    /// </summary>
    /// <seealso cref="RingSoft.DbLookup.Lookup.ILookupControl" />
    public class LookupUserInterface : ILookupControl
    {
        /// <summary>
        /// Gets the number of rows on a page.
        /// </summary>
        /// <value>The number of rows on the page.</value>
        public int PageSize { get; set; } = 20;
        /// <summary>
        /// Gets the type of the search.
        /// </summary>
        /// <value>The type of the search.</value>
        public LookupSearchTypes SearchType { get; set; } = LookupSearchTypes.Equals;
        /// <summary>
        /// Gets or sets the search text.
        /// </summary>
        /// <value>The search text.</value>
        public string SearchText { get; set; }
        /// <summary>
        /// Gets the index of the selected.
        /// </summary>
        /// <value>The index of the selected.</value>
        public int SelectedIndex => 0;
        /// <summary>
        /// Sets the index of the lookup.
        /// </summary>
        /// <param name="index">The index.</param>
        public void SetLookupIndex(int index)
        {
            
        }
    }
}
