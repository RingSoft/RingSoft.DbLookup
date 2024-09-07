// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-16-2023
// ***********************************************************************
// <copyright file="QuerySet.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections.Generic;

namespace RingSoft.DbLookup.QueryBuilder
{
    /// <summary>
    /// A list of Query objects.
    /// </summary>
    public class QuerySet
    {
        /// <summary>
        /// The queries
        /// </summary>
        private List<QueryBase> _queries = new List<QueryBase>();

        /// <summary>
        /// Gets the queries.
        /// </summary>
        /// <value>The queries.</value>
        public IReadOnlyList<QueryBase> Queries => _queries;

        /// <summary>
        /// Gets or sets the debug message.
        /// </summary>
        /// <value>The debug message.</value>
        public string DebugMessage { get; set; }

        /// <summary>
        /// Adds the query object to the collection.
        /// </summary>
        /// <param name="query">The Query object to add.</param>
        /// <param name="dataTableName">Name of the resulting DataTable.</param>
        /// <returns>QuerySet.</returns>
        public QuerySet AddQuery(QueryBase query, string dataTableName)
        {
            query.DataTableName = dataTableName;
            _queries.Add(query);
            return this;
        }
    }
}
