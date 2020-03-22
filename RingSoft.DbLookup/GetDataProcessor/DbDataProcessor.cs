using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using RingSoft.DbLookup.GetDataProcessor.SelectSqlGenerator;
using RingSoft.DbLookup.QueryBuilder;

namespace RingSoft.DbLookup.GetDataProcessor
{
    /// <summary>
    /// Base class for retrieving data from a database.
    /// </summary>
    public abstract class DbDataProcessor
    {
        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <value>
        /// The connection string.
        /// </value>
        public abstract string ConnectionString { get; }

        /// <summary>
        /// Gets or sets a value indicating whether to keep the database connection open after retrieving data.  If set to true,
        /// then the calling code will need to close the connection manually by running the CloseConnection method.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [keep database connection open]; otherwise, <c>false</c>.
        /// </value>
        public bool KeepConnectionOpen { get; set; }

        /// <summary>
        /// Implement this to create and return the SQL generator which will be used in GetData.
        /// </summary>
        /// <value>
        /// The SQL generator.
        /// </value>
        protected abstract DbSelectSqlGenerator SqlGenerator { get; }

        /// <summary>
        /// Gets the error viewer.
        /// </summary>
        /// <value>
        /// The error viewer.
        /// </value>
        public static IGetDataResultErrorViewer SqlErrorViewer { get; set; }

        /// <summary>
        /// Gets or sets the window cursor.
        /// </summary>
        /// <value>
        /// The window cursor.
        /// </value>
        public static IWindowCursor WindowCursor { get; set; }

        internal static bool ShowSqlWindow => _showSqlWindow;


        private static bool _showSqlWindow;
        public DbDataProcessor()
        {
            if (SqlErrorViewer == null)
                SqlErrorViewer = new DefaultGetDataResultErrorViewer();

            if (WindowCursor == null)
                WindowCursor = new DefaultWindowCursor();
        }

        /// <summary>
        /// Shows the SQL window.
        /// </summary>
        /// <param name="value">if set to <c>true</c> then the Sql viewwer will show every time a Sql statement is executed.</param>
        public static void ShowSQLWindow(bool value = true)
        {
            _showSqlWindow = value;
        }
        
        /// <summary>
        /// Implement this to create and open the database connection.
        /// </summary>
        protected abstract IDbConnection CreateConnection();

        /// <summary>
        /// Closes the database connection.
        /// </summary>
        public void CloseConnection(IDbConnection connection)
        {
            KeepConnectionOpen = false;
            if (connection != null)
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                connection.Dispose();
                connection = null;
                GC.Collect();
            }

        }

        /// <summary>
        /// Implement this to create and return the data adapter used to fill a DataSet with the results of the SQL statement.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="sqlStatement">The SQL statement.</param>
        /// <returns></returns>
        protected abstract IDataAdapter GetDataAdapter(IDbConnection connection, string sqlStatement);

        /// <summary>
        /// Implement this to create and return the command object used to execute the SQL statement.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="sqlStatement">The SQL statement.</param>
        /// <returns></returns>
        protected abstract IDbCommand GetDbCommand(IDbConnection connection, string sqlStatement);

        /// <summary>
        /// Gets the list of databases.
        /// </summary>
        /// <returns>A GetDataResult object containing a list of databases or an error.</returns>
        public virtual GetDataResult GetListOfDatabases()
        {
            return  new GetDataResult("");
        }

        /// <summary>
        /// Gets the resulting data or error after executing the query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public GetDataResult GetData(QueryBase query)
        {
            var querySet = new QuerySet();
            querySet.AddQuery(query, "TABLE");
            querySet.DebugMessage = query.DebugMessage;
            return GetData(querySet);
        }

        /// <summary>
        /// Gets the resulting DataSet or error after executing the query set.
        /// </summary>
        /// <param name="querySet">The query set.</param>
        /// <returns></returns>
        public GetDataResult GetData(QuerySet querySet)
        {
            WindowCursor.SetWindowCursor(WindowCursorTypes.Wait);
            
            var result = new GetDataResult(querySet.DebugMessage);

            if (!querySet.Queries.Any())
            {
                result.ResultCode = GetDataResultCodes.NothingToDo;
                return result;
            }

            IDbConnection connection = TryOpenConnection(result, false);
            if (result.ResultCode == GetDataResultCodes.DbConnectError)
                return result;

            result.ConnectionString = ConnectionString;
            foreach (var query in querySet.Queries)
            {
                ProcessQuery(connection, query, result);
                if (result.ResultCode == GetDataResultCodes.SqlError)
                {
                    WindowCursor.SetWindowCursor(WindowCursorTypes.Default);
                    SqlErrorViewer.ShowGetDataError(result);
                    return result;
                }
                else if (ShowSqlWindow)
                {
                    result.ResultCode = GetDataResultCodes.Success;
                    result.DebugMessage += $"{Environment.NewLine}{Environment.NewLine}Result Row Count = ";
                    result.DebugMessage += $"{result.DataSet.Tables[0].Rows.Count}";
                    result.FailedSqlStatement = result.Sqls[result.Sqls.Count - 1].SqlText;
                    SqlErrorViewer.ShowGetDataError(result);
                }
            }

            if (!KeepConnectionOpen)
                CloseConnection(connection);

            result.ResultCode = GetDataResultCodes.Success;
            WindowCursor.SetWindowCursor(WindowCursorTypes.Default);
            return result;
        }

        private IDbConnection TryOpenConnection(GetDataResult result, bool clearConnectionPools)
        {
            IDbConnection connection = null;
            try
            {
                if (clearConnectionPools)
                    ClearConnectionPools();

                connection = CreateConnection();
                connection.Open();
            }
            catch (Exception e)
            {
                result.ErrorMessage = $"Database Connection Error!\r\n\r\n{e.Message}";
                result.ResultCode = GetDataResultCodes.DbConnectError;
                result.FailedSqlStatement = ConnectionString;
                SqlErrorViewer.ShowGetDataError(result);
                CloseConnection(connection);
            }

            return connection;
        }

        private void ProcessQuery(IDbConnection connection, QueryBase query, GetDataResult queryResult)
        {
            var sql = query.RawSql;
            if (sql.IsNullOrEmpty())
                sql = SqlGenerator.GenerateSelectStatement(query);

            queryResult.AddGeneratedSql(query, sql);
            var adapter = GetDataAdapter(connection, sql);
            var dataSet = new DataSet();
            try
            {
                adapter.Fill(dataSet);
            }
            catch (Exception e)
            {
                queryResult.ResultCode = GetDataResultCodes.SqlError;
                queryResult.ErrorMessage = $"SQL Execution Error!\r\n\r\n{e.Message}";
                queryResult.FailedQuery = query;
                queryResult.FailedSqlStatement = sql;
                CloseConnection(connection);
                return;
            }

            if (queryResult.DataSet == null)
                queryResult.DataSet = new DataSet();

            if (dataSet.Tables.Count > 0)
            {
                var table = dataSet.Tables[0];
                table.TableName = query.DataTableName;
                queryResult.DataSet.Tables.Add(table.Copy());
            }
        }

        public GetDataResult ExecuteSql(string sqlStatement, bool clearConnectionPools = false)
        {
            var sqlList = new List<string>();
            sqlList.Add(sqlStatement);
            return ExecuteSqls(sqlList);
        }

        public GetDataResult ExecuteSqls(List<string> sqlsList, bool clearConnectionPools = false)
        {
            WindowCursor.SetWindowCursor(WindowCursorTypes.Wait);

            var result = new GetDataResult(string.Empty);

            if (!sqlsList.Any())
            {
                result.ResultCode = GetDataResultCodes.NothingToDo;
                return result;
            }

            var connection = TryOpenConnection(result, clearConnectionPools);
            if (result.ResultCode == GetDataResultCodes.DbConnectError)
                return result;

            DataTable returnTable = new DataTable("RETURN");
            returnTable.Columns.Add(new DataColumn("RecordsAffected"));
            result.DataSet = new DataSet();
            result.DataSet.Tables.Add(returnTable);

            result.ConnectionString = ConnectionString;
            var totalRecordsAffected = 0;
            foreach (var sql in sqlsList)
            {
                var recordsAffected = ExecuteSql(connection, sql, result);
                if (recordsAffected > 0)
                    totalRecordsAffected += recordsAffected;

                if (result.ResultCode == GetDataResultCodes.SqlError)
                {
                    CloseConnection(connection);
                    WindowCursor.SetWindowCursor(WindowCursorTypes.Default);
                    SqlErrorViewer.ShowGetDataError(result);
                    return result;
                }
            }

            var newRow = returnTable.NewRow();
            newRow[returnTable.Columns[0]] = totalRecordsAffected;
            returnTable.Rows.Add(newRow);
            result.DebugMessage +=
                $"{totalRecordsAffected.ToString(GblMethods.GetNumFormat(0, false))} Records Affected";

            if (!KeepConnectionOpen)
                CloseConnection(connection);

            result.ResultCode = GetDataResultCodes.Success;
            WindowCursor.SetWindowCursor(WindowCursorTypes.Default);
            if (ShowSqlWindow)
            {
                SqlErrorViewer.ShowGetDataError(result);
            }

            return result;
        }

        protected virtual void ClearConnectionPools()
        {

        }

        private int ExecuteSql(IDbConnection connection, string sql, GetDataResult queryResult)
        {
            var command = GetDbCommand(connection, sql);
            var result = 0;
            try
            {
                result = command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                queryResult.ResultCode = GetDataResultCodes.SqlError;
                queryResult.ErrorMessage = $"SQL Execution Error!\r\n\r\n{e.Message}";
                queryResult.FailedSqlStatement = sql;
                CloseConnection(connection);
            }

            return result;
        }

        /// <summary>
        /// Displays a data exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="debugMessage">The debug message.</param>
        public static void DisplayDataException(Exception exception, string debugMessage)
        {
            var getDataResult = new GetDataResult(debugMessage)
            {
                ErrorMessage = exception.Message,
                ResultCode = GetDataResultCodes.SqlError
            };
            SqlErrorViewer.ShowGetDataError(getDataResult);
        }
    }
}
