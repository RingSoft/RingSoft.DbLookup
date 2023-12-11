// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 09-23-2023
// ***********************************************************************
// <copyright file="IAutoFillControl.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DbLookup.Lookup;

namespace RingSoft.DbLookup.AutoFill
{
    /// <summary>
    /// Enum AutoFillRefreshModes
    /// </summary>
    public enum AutoFillRefreshModes
    {
        /// <summary>
        /// The database select
        /// </summary>
        DbSelect = 1,
        /// <summary>
        /// The pk refresh
        /// </summary>
        PkRefresh = 2,
        /// <summary>
        /// The database delete
        /// </summary>
        DbDelete = 3,
    }

    /// <summary>
    /// Interface IAutoFillControl
    /// </summary>
    public interface IAutoFillControl
    {
        /// <summary>
        /// Gets or sets the edit text.
        /// </summary>
        /// <value>The edit text.</value>
        string EditText { get; set; }

        /// <summary>
        /// Gets or sets the selection start.
        /// </summary>
        /// <value>The selection start.</value>
        int SelectionStart { get; set; }

        /// <summary>
        /// Gets or sets the length of the selection.
        /// </summary>
        /// <value>The length of the selection.</value>
        int SelectionLength { get; set; }

        /// <summary>
        /// Refreshes the value.
        /// </summary>
        /// <param name="token">The token.</param>
        void RefreshValue(LookupCallBackToken token);

        /// <summary>
        /// Called when [select].
        /// </summary>
        void OnSelect();
    }
}
