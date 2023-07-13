using System;
using RingSoft.DbLookup.AutoFill;

namespace RingSoft.DbLookup.Lookup
{
    public enum RefreshOperations
    {
        Save = 0,
        Delete = 1,
    }
    /// <summary>
    /// Used by the forms launched by the Add and View lookup buttons to refresh the lookup data when the form changes the lookup database.
    /// </summary>
    public class LookupCallBackToken
    {
        public LookupDataMauiBase LookupData { get; set; }

        public AutoFillValue NewAutoFillValue { get; set; }

        public bool DbSelect { get; set; }

        public RefreshOperations RefreshOperation { get; private set; }

        /// <summary>
        /// Occurs when the child window changes the underlying data source.
        /// </summary>
        public event EventHandler RefreshData;

        public LookupCallBackToken()
        {
        }
        /// <summary>
        /// Invokes the RefreshData event.
        /// </summary>
        public void OnRefreshData(RefreshOperations operation = RefreshOperations.Save)
        {
            RefreshOperation = operation;
            RefreshData?.Invoke(this, EventArgs.Empty);
        }
    }
}
