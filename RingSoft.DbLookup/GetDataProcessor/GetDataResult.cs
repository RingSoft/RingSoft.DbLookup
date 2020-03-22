using System;
using System.Collections.Generic;
using System.Data;
using RingSoft.DbLookupCore.QueryBuilder;

namespace RingSoft.DbLookupCore.GetDataProcessor
{
    public enum GetDataResultCodes
    {
        PendingProcess = 0,
        Success = 1,
        DbConnectError = 2,
        SqlError = 3,
        NothingToDo = 4
    }

    /// <summary>
    /// Result after executing the DbDataProcessor GetData method.
    /// </summary>
    [Serializable]
    public class GetDataResult
    {
        /// <summary>
        /// Gets the result code.
        /// </summary>
        /// <value>
        /// The result code.
        /// </value>
        public GetDataResultCodes ResultCode { get; internal set; }

        /// <summary>
        /// Gets the data set.
        /// </summary>
        /// <value>
        /// The data set.  This will be null if ResultCode is not Success.
        /// </value>
        public DataSet DataSet { get; internal set; }

        /// <summary>
        /// Gets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        public string ErrorMessage { get; internal set; }

        /// <summary>
        /// Gets the failed SQL statement.
        /// </summary>
        /// <value>
        /// The failed SQL statement.
        /// </value>
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public string FailedSqlStatement { get; internal set; }

        /// <summary>
        /// Gets the failed query.
        /// </summary>
        /// <value>
        /// The failed query.
        /// </value>
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public QueryBase FailedQuery { get; internal set; }

        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <value>
        /// The connection string.
        /// </value>
        public string ConnectionString { get; internal set; }

        /// <summary>
        /// Gets the generated SQL statements.
        /// </summary>
        /// <value>
        /// The generated SQL statements.
        /// </value>
        public IReadOnlyList<QueryResultSql> Sqls => _queryResultSqls;

        /// <summary>
        /// Gets the debug message.
        /// </summary>
        /// <value>
        /// The debug message.
        /// </value>
        public string DebugMessage { get; internal set; }

        private readonly List<QueryResultSql> _queryResultSqls = new List<QueryResultSql>();

        public GetDataResult(string debugMessage)
        {
            DebugMessage = debugMessage;
        }

        public override string ToString()
        {
            if (!ErrorMessage.IsNullOrEmpty())
                return ErrorMessage;

            if (ResultCode == GetDataResultCodes.PendingProcess)
                return "Pending Process.";

            if (ResultCode == GetDataResultCodes.NothingToDo)
                return "Nothing To Do.";

            if (DataSet != null && ResultCode == GetDataResultCodes.Success)
                return "Success!";

            return base.ToString();
        }

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
            /// <value>
            /// The query.
            /// </value>
            public QueryBase Query { get; internal set; }

            /// <summary>
            /// Gets the generated SQL text.
            /// </summary>
            /// <value>
            /// The generated SQL text.
            /// </value>
            public string SqlText { get; internal set; }

            public override string ToString()
            {
                return Query.DataTableName;
            }
        }
    }
}
