﻿using System;

namespace RingSoft.DbLookup.Lookup
{
    /// <summary>
    /// Used by the forms launched by the Add and View lookup buttons to refresh the lookup data when the form changes the lookup database.
    /// </summary>
    public class LookupCallBackToken
    {
        /// <summary>
        /// Occurs when the child window changes the underlying data source.
        /// </summary>
        public event EventHandler RefreshData;

        /// <summary>
        /// Invokes the RefreshData event.
        /// </summary>
        public void OnRefreshData()
        {
            RefreshData?.Invoke(this, EventArgs.Empty);
        }
    }
}
