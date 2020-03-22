namespace RingSoft.DbLookupCore.QueryBuilder
{
    public enum QueryTypes
    {
        SelectQuery = 0,
        CountQuery = 1
    }

    public abstract class QueryBase
    {
        /// <summary>
        /// Gets the type of the query.
        /// </summary>
        /// <value>
        /// The type of the query.
        /// </value>
        public abstract QueryTypes QueryType { get; }

        /// <summary>
        /// Gets the name of the DataTable in the resulting DataSet.
        /// </summary>
        /// <value>
        /// The name of the DataTable in the resulting DataSet.
        /// </value>
        public string DataTableName { get; internal set; }

        /// <summary>
        /// Gets or sets the raw SQL.  If this has text, then it will be executed and the query info will be ignored.
        /// </summary>
        /// <value>
        /// The raw SQL.
        /// </value>
        public string RawSql { get; set; }

        /// <summary>
        /// Gets or sets the debug message.
        /// </summary>
        /// <value>
        /// The debug message.
        /// </value>
        public string DebugMessage { get; set; }
    }
}
