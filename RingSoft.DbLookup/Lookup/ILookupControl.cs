// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 07-05-2023
// ***********************************************************************
// <copyright file="ILookupControl.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace RingSoft.DbLookup.Lookup
{
    /// <summary>
    /// Enum LookupSearchTypes
    /// </summary>
    public enum LookupSearchTypes
    {
        /// <summary>
        /// The equals
        /// </summary>
        Equals = 0,
        /// <summary>
        /// Determines whether this instance contains the object.
        /// </summary>
        Contains = 1
    }

    /// <summary>
    /// Interface ILookupWindow
    /// </summary>
    public interface ILookupWindow
    {
        /// <summary>
        /// Selects the primary key.
        /// </summary>
        /// <param name="primaryKey">The primary key.</param>
        void SelectPrimaryKey(PrimaryKeyValue primaryKey);

        /// <summary>
        /// Gets a value indicating whether [read only mode].
        /// </summary>
        /// <value><c>true</c> if [read only mode]; otherwise, <c>false</c>.</value>
        bool ReadOnlyMode { get; }

        /// <summary>
        /// Called when [select button click].
        /// </summary>
        void OnSelectButtonClick();
    }

    /// <summary>
    /// The lookup's user interface.
    /// </summary>
    public interface ILookupControl
    {
        /// <summary>
        /// Gets the number of rows on a page.
        /// </summary>
        /// <value>The number of rows on the page.</value>
        int PageSize { get; }

        /// <summary>
        /// Gets the type of the search.
        /// </summary>
        /// <value>The type of the search.</value>
        LookupSearchTypes SearchType { get; }

        /// <summary>
        /// Gets or sets the search text.
        /// </summary>
        /// <value>The search text.</value>
        string SearchText { get; }

        /// <summary>
        /// Gets the index of the selected.
        /// </summary>
        /// <value>The index of the selected.</value>
        int SelectedIndex { get; }

        /// <summary>
        /// Sets the index of the lookup.
        /// </summary>
        /// <param name="index">The index.</param>
        void SetLookupIndex(int index);
    }
}
