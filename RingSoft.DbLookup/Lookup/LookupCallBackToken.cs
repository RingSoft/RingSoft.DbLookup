// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-16-2023
// ***********************************************************************
// <copyright file="LookupCallBackToken.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using RingSoft.DbLookup.AutoFill;

namespace RingSoft.DbLookup.Lookup
{
    /// <summary>
    /// Enum RefreshOperations
    /// </summary>
    public enum RefreshOperations
    {
        /// <summary>
        /// The save
        /// </summary>
        Save = 0,
        /// <summary>
        /// The delete
        /// </summary>
        Delete = 1,
    }
    /// <summary>
    /// Used by the forms launched by the Add and View lookup buttons to refresh the lookup data when the form changes the lookup database.
    /// </summary>
    public class LookupCallBackToken
    {
        /// <summary>
        /// Gets or sets the lookup data.
        /// </summary>
        /// <value>The lookup data.</value>
        public LookupDataMauiBase LookupData { get; set; }

        /// <summary>
        /// Creates new autofillvalue.
        /// </summary>
        /// <value>The new automatic fill value.</value>
        public AutoFillValue NewAutoFillValue { get; set; }

        /// <summary>
        /// Gets or sets the deleted primary key value.
        /// </summary>
        /// <value>The deleted primary key value.</value>
        public PrimaryKeyValue DeletedPrimaryKeyValue { get; set; }

        /// <summary>
        /// Gets or sets the refresh mode.
        /// </summary>
        /// <value>The refresh mode.</value>
        public AutoFillRefreshModes RefreshMode { get; set; }

        /// <summary>
        /// Gets the refresh operation.
        /// </summary>
        /// <value>The refresh operation.</value>
        public RefreshOperations RefreshOperation { get; private set; }

        /// <summary>
        /// Occurs when the child window changes the underlying data source.
        /// </summary>
        public event EventHandler RefreshData;

        public event EventHandler CloseLookupWindow;

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupCallBackToken" /> class.
        /// </summary>
        public LookupCallBackToken()
        {
        }
        /// <summary>
        /// Invokes the RefreshData event.
        /// </summary>
        /// <param name="operation">The operation.</param>
        public void OnRefreshData(RefreshOperations operation = RefreshOperations.Save)
        {
            RefreshOperation = operation;
            RefreshData?.Invoke(this, EventArgs.Empty);
        }

        public void OnCloseLookupWindow()
        {
            CloseLookupWindow?.Invoke(this, EventArgs.Empty);
        }
    }
}
