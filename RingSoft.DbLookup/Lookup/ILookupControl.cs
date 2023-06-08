namespace RingSoft.DbLookup.Lookup
{
    public enum LookupSearchTypes
    {
        Equals = 0,
        Contains = 1
    }

    public interface ILookupWindow
    {
        void SelectPrimaryKey(PrimaryKeyValue primaryKey);
    }

    /// <summary>
    /// The lookup's user interface.
    /// </summary>
    public interface ILookupControl
    {
        /// <summary>
        /// Gets the number of rows on a page.
        /// </summary>
        /// <value>
        /// The number of rows on the page.
        /// </value>
        int PageSize { get; }

        /// <summary>
        /// Gets the type of the search.
        /// </summary>
        /// <value>
        /// The type of the search.
        /// </value>
        LookupSearchTypes SearchType { get; }

        /// <summary>
        /// Gets or sets the search text.
        /// </summary>
        /// <value>
        /// The search text.
        /// </value>
        string SearchText { get; }

        int SelectedIndex { get; }
    }
}
