// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-19-2022
// ***********************************************************************
// <copyright file="CountQuery.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace RingSoft.DbLookup.QueryBuilder
{
    /// <summary>
    /// A query that generates a count of records.
    /// </summary>
    /// <seealso cref="QueryBase" />
    public class CountQuery : QueryBase
    {
        /// <summary>
        /// Gets the type of the query.
        /// </summary>
        /// <value>The type of the query.</value>
        public override QueryTypes QueryType => QueryTypes.CountQuery;

        /// <summary>
        /// Gets the select query.
        /// </summary>
        /// <value>The select query.</value>
        public SelectQuery SelectQuery { get; }

        /// <summary>
        /// Gets the name of the count column.
        /// </summary>
        /// <value>The name of the count column.</value>
        public string CountColumnName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CountQuery" /> class.
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
