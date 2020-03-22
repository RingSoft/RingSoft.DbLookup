namespace RingSoft.DbLookupCore.QueryBuilder
{
    /// <summary>
    /// A query that generates a count of records.
    /// </summary>
    /// <seealso cref="QueryBase" />
    public class CountQuery : QueryBase
    {
        public override QueryTypes QueryType => QueryTypes.CountQuery;

        /// <summary>
        /// Gets the select query.
        /// </summary>
        /// <value>
        /// The select query.
        /// </value>
        public SelectQuery SelectQuery { get; }

        /// <summary>
        /// Gets the name of the count column.
        /// </summary>
        /// <value>
        /// The name of the count column.
        /// </value>
        public string CountColumnName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CountQuery"/> class.
        /// </summary>
        /// <param name="selectQuery">The select query.</param>
        /// <param name="countColumnName">Name of the count column.</param>
        public CountQuery(SelectQuery selectQuery, string countColumnName)
        {
            SelectQuery = selectQuery;
            CountColumnName = countColumnName;
        }
    }
}
