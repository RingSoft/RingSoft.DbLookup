// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-16-2023
// ***********************************************************************
// <copyright file="DataProcessResult.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DataEntryControls.Engine;
using System;
using System.Collections.Generic;
using System.Data;
using RingSoft.DbLookup.QueryBuilder;

namespace RingSoft.DbLookup.DataProcessor
{
    /// <summary>
    /// Enum GetDataResultCodes
    /// </summary>
    public enum GetDataResultCodes
    {
        /// <summary>
        /// The pending process
        /// </summary>
        PendingProcess = 0,
        /// <summary>
        /// The success
        /// </summary>
        Success = 1,
        /// <summary>
        /// The database connect error
        /// </summary>
        DbConnectError = 2,
        /// <summary>
        /// The SQL error
        /// </summary>
        SqlError = 3,
        /// <summary>
        /// The nothing to do
        /// </summary>
        NothingToDo = 4
    }

    /// <summary>
    /// Result after executing the DbDataProcessor GetData method.
    /// </summary>
    [Serializable]
    public class DataProcessResult
    {
        /// <summary>
        /// Gets the result code.
        /// </summary>
        /// <value>The result code.</value>
        public GetDataResultCodes ResultCode { get; internal set; }

        /// <summary>
        /// Gets the data set.
        /// </summary>
        /// <value>The data set.  This will be null if ResultCode is not Success.</value>
        public DataSet DataSet { get; internal set; }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message { get; internal set; }

        /// <summary>
        /// Gets the processed SQL statement.
        /// </summary>
        /// <value>The processed SQL statement.</value>
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public string ProcessedSqlStatement { get; internal set; }

        /// <summary>
        /// Gets the processed query.
        /// </summary>
        /// <value>The processed query.</value>
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public QueryBase ProcessedQuery { get; internal set; }

        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <value>The connection string.</value>
        public string ConnectionString { get; internal set; }

        /// <summary>
        /// Gets the generated SQL statements.
        /// </summary>
        /// <value>The generated SQL statements.</value>
        public IReadOnlyList<QueryResultSql> Sqls => _queryResultSqls;

        /// <summary>
        /// Gets the debug message.
        /// </summary>
        /// <value>The debug message.</value>
        public string DebugMessage { get; internal set; }

        /// <summary>
        /// The query result SQLS
        /// </summary>
        private readonly List<QueryResultSql> _queryResultSqls = new List<QueryResultSql>();

        /// <summary>
        /// Initializes a new instance of the <see cref="DataProcessResult" /> class.
        /// </summary>
        /// <param name="debugMessage">The debug message.</param>
        public DataProcessResult(string debugMessage)
        {
            DebugMessage = debugMessage;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            if (!Message.IsNullOrEmpty())
                return Message;

            if (ResultCode == GetDataResultCodes.PendingProcess)
                return "Pending Process.";

            if (ResultCode == GetDataResultCodes.NothingToDo)
                return "Nothing To Do.";

            if (DataSet != null && ResultCode == GetDataResultCodes.Success)
                return "Success!";

            return base.ToString();
        }

        /// <summary>
        /// Adds the generated SQL.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="sql">The SQL.</param>
        internal void AddGeneratedSql(QueryBase query, string sql)
        {
            var queryResultSql = new QueryResultSql
            {
                Query = query,
                SqlText = sql
            };

            _queryResultSqls.Add(queryResultSql);
        }

        /// <summary>
        /// A generated SQL text and its corresponding table name.
        /// </summary>
        public class QueryResultSql
        {
            /// <summary>
            /// Gets the query.
            /// </summary>
            /// <value>The query.</value>
            public QueryBase Query { get; internal set; }

            /// <summary>
            /// Gets the generated SQL text.
            /// </summary>
            /// <value>The generated SQL text.</value>
            public string SqlText { get; internal set; }

            /// <summary>
            /// Returns a <see cref="System.String" /> that represents this instance.
            /// </summary>
            /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
            public override string ToString()
            {
                return Query.DataTableName;
            }
        }
    }
}
