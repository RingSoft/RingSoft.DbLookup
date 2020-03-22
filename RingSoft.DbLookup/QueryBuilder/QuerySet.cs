using System.Collections.Generic;

namespace RingSoft.DbLookup.QueryBuilder
{
    /// <summary>
    /// A list of Query objects.
    /// </summary>
    public class QuerySet
    {
        private List<QueryBase> _queries = new List<QueryBase>();

        /// <summary>
        /// Gets the queries.
        /// </summary>
        /// <value>
        /// The queries.
        /// </value>
        public IReadOnlyList<QueryBase> Queries => _queries;

        /// <summary>
        /// Gets or sets the debug message.
        /// </summary>
        /// <value>
        /// The debug message.
        /// </value>
        public string DebugMessage { get; set; }

        /// <summary>
        /// Adds the query object to the collection.
        /// </summary>
        /// <param name="query">The Query object to add.</param>
        /// <param name="dataTableName">Name of the resulting DataTable.</param>
        /// <returns></returns>
        public QuerySet AddQuery(QueryBase query, string dataTableName)
        {
            query.DataTableName = dataTableName;
            _queries.Add(query);
            return this;
        }
    }
}
