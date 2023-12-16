// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-19-2022
// ***********************************************************************
// <copyright file="QueryBase.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace RingSoft.DbLookup.QueryBuilder
{
    /// <summary>
    /// Enum QueryTypes
    /// </summary>
    public enum QueryTypes
    {
        /// <summary>
        /// The select query
        /// </summary>
        SelectQuery = 0,
        /// <summary>
        /// The count query
        /// </summary>
        CountQuery = 1
    }

    /// <summary>
    /// Class QueryBase.
    /// </summary>
    public abstract class QueryBase
    {
        /// <summary>
        /// Gets the type of the query.
        /// </summary>
        /// <value>The type of the query.</value>
        public abstract QueryTypes QueryType { get; }

        /// <summary>
        /// Gets the name of the DataTable in the resulting DataSet.
        /// </summary>
        /// <value>The name of the DataTable in the resulting DataSet.</value>
        public string DataTableName { get; internal set; }

        /// <summary>
        /// Gets or sets the raw SQL.  If this has text, then it will be executed and the query info will be ignored.
        /// </summary>
        /// <value>The raw SQL.</value>
        public string RawSql { get; set; }

        /// <summary>
        /// Gets or sets the debug message which is displayed in the DataResultViewer.
        /// </summary>
        /// <value>The debug message.</value>
        public string DebugMessage { get; set; }
    }
}
