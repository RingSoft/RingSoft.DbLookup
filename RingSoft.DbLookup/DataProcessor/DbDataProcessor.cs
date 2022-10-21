using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.DataProcessor.SelectSqlGenerator;
using RingSoft.DbLookup.QueryBuilder;

namespace RingSoft.DbLookup.DataProcessor
{
    public enum DbFieldTypes
    {
        Integer,
        String,
        Decimal,
        DateTime,
        Byte,
        Bool,
        Memo
    }
    /// <summary>
    /// Base class for processing database data.
    /// </summary>
    public abstract class DbDataProcessor
    {
        /// <summary>
        /// Gets the database connection string.
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
        public abstract DbSelectSqlGenerator SqlGenerator { get; }

        /// <summary>
        /// Gets the user interface for this class to interact with.
        /// </summary>
        /// <value>
        /// Gets the user interface.
        /// </value>
        public static IDbLookupUserInterface UserInterface { get; set; }

        internal static bool ShowSqlWindow => _showSqlWindow;


        private static bool _showSqlWindow;
        public DbDataProcessor()
        {
            if (UserInterface == null)
                UserInterface = new DefaultUserInterface();
        }

        /// <summary>
        /// Shows the data process result viewer window right after a SQL statement is processed.  Useful in debugging.
        /// </summary>
        /// <param name="value">If set to true then the data process result viewer will right after a SQL statement is processed.</param>
        public static void ShowSqlStatementWindow(bool value = true)
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
        public virtual void CloseConnection(IDbConnection connection)
        {
            KeepConnectionOpen = false;
            if (connection != null)
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                connection.Dispose();
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
        public virtual DataProcessResult GetListOfDatabases()
        {
            return  new DataProcessResult("");
        }

        public abstract DataProcessResult DropDatabase();

        /// <summary>
        /// Gets the resulting data or error after executing the query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="setWaitCursor">if set to <c>true</c> set mouse cursor to wait.</param>
        /// <returns></returns>
        public DataProcessResult GetData(QueryBase query, bool setWaitCursor = true, bool showError = true)
        {
            var querySet = new QuerySet();
            querySet.AddQuery(query, "TABLE");
            querySet.DebugMessage = query.DebugMessage;
            return GetData(querySet, setWaitCursor, showError);
        }

        /// <summary>
        /// Gets the resulting DataSet or error after executing the query set.
        /// </summary>
        /// <param name="querySet">The query set.</param>
        /// <param name="setWaitCursor">if set to <c>true</c> set mouse cursor to wait.</param>
        /// <returns></returns>
        public DataProcessResult GetData(QuerySet querySet, bool setWaitCursor = true, bool showError = true)
        {
            if (setWaitCursor)
                ControlsGlobals.UserInterface.SetWindowCursor(WindowCursorTypes.Wait);
            
            var result = new DataProcessResult(querySet.DebugMessage);

            if (!querySet.Queries.Any())
            {
                result.ResultCode = GetDataResultCodes.NothingToDo;
                return result;
            }

            IDbConnection connection = TryOpenConnection(result, false, setWaitCursor, showError);
            if (result.ResultCode == GetDataResultCodes.DbConnectError)
                return result;

            result.ConnectionString = ConnectionString;
            foreach (var query in querySet.Queries)
            {
                ProcessQuery(connection, query, result);
                if (result.ResultCode == GetDataResultCodes.SqlError)
                {
                    if (setWaitCursor)
                        ControlsGlobals.UserInterface.SetWindowCursor(WindowCursorTypes.Default);
                    if (showError)
                    {
                        UserInterface.ShowDataProcessResult(result);
                    }

                    return result;
                }
                else if (ShowSqlWindow)
                {
                    if (setWaitCursor)
                        ControlsGlobals.UserInterface.SetWindowCursor(WindowCursorTypes.Default);
                    result.ResultCode = GetDataResultCodes.Success;
                    result.DebugMessage += $"{Environment.NewLine}{Environment.NewLine}Result Row Count = ";
                    result.DebugMessage += $"{result.DataSet.Tables[0].Rows.Count}";
                    result.ProcessedSqlStatement = result.Sqls[result.Sqls.Count - 1].SqlText;
                    UserInterface.ShowDataProcessResult(result);
                }
            }

            if (!KeepConnectionOpen)
                CloseConnection(connection);

            result.ResultCode = GetDataResultCodes.Success;
            if (setWaitCursor)
                ControlsGlobals.UserInterface.SetWindowCursor(WindowCursorTypes.Default);
            return result;
        }

        private IDbConnection TryOpenConnection(DataProcessResult result, bool clearConnectionPools, bool setCursor, bool showError = true)
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
                if (setCursor)
                    ControlsGlobals.UserInterface.SetWindowCursor(WindowCursorTypes.Default);

                result.Message = $"Database Connection Error!\r\n\r\n{e.Message}";
                result.ResultCode = GetDataResultCodes.DbConnectError;
                result.ProcessedSqlStatement = ConnectionString;
                if (showError)
                {
                    ControlsGlobals.UserInterface.ShowMessageBox(result.Message, "Database Connection Error",
                        RsMessageBoxIcons.Error);
                }

                CloseConnection(connection);
            }

            return connection;
        }

        private void ProcessQuery(IDbConnection connection, QueryBase query, DataProcessResult queryResult)
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
                queryResult.Message = $"SQL Execution Error!\r\n\r\n{e.Message}";
                queryResult.ProcessedQuery = query;
                queryResult.ProcessedSqlStatement = sql;
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

        /// <summary>
        /// Executes the SQL.
        /// </summary>
        /// <param name="sqlStatement">The SQL statement.</param>
        /// <param name="clearConnectionPools">if set to <c>true</c> clear connection pools.</param>
        /// <param name="setWaitCursor">if set to <c>true</c> set mouse cursor to wait.</param>
        /// <returns></returns>
        public DataProcessResult ExecuteSql(string sqlStatement, bool clearConnectionPools = false, bool setWaitCursor = true, bool showError = true)
        {
            var sqlList = new List<string>();
            sqlList.Add(sqlStatement);
            return ExecuteSqls(sqlList, clearConnectionPools, setWaitCursor, showError);
        }

        /// <summary>
        /// Executes the SQLS.
        /// </summary>
        /// <param name="sqlsList">The SQLS list.</param>
        /// <param name="clearConnectionPools">if set to <c>true</c> [clear connection pools].</param>
        /// <param name="setWaitCursor">if set to <c>true</c> set mouse cursor to wait.</param>
        /// <returns></returns>
        public DataProcessResult ExecuteSqls(List<string> sqlsList, bool clearConnectionPools = false, bool setWaitCursor = true, bool showError = true)
        {
            if (setWaitCursor)
                ControlsGlobals.UserInterface.SetWindowCursor(WindowCursorTypes.Wait);

            var result = new DataProcessResult(string.Empty);

            if (!sqlsList.Any())
            {
                result.ResultCode = GetDataResultCodes.NothingToDo;
                return result;
            }

            var connection = TryOpenConnection(result, clearConnectionPools, setWaitCursor);
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
                    if (setWaitCursor)
                        ControlsGlobals.UserInterface.SetWindowCursor(WindowCursorTypes.Default);

                    if (showError)
                    {
                        ControlsGlobals.UserInterface.ShowMessageBox(result.Message, "SQL Process Failed",
                            RsMessageBoxIcons.Error);
                    }

                    return result;
                }
            }

            var newRow = returnTable.NewRow();
            newRow[returnTable.Columns[0]] = totalRecordsAffected;
            returnTable.Rows.Add(newRow);
            var outputMessage = $"{totalRecordsAffected.ToString(GblMethods.GetNumFormat(0, false))} Records Affected";
            result.DebugMessage += outputMessage;

            if (!KeepConnectionOpen)
                CloseConnection(connection);

            result.ResultCode = GetDataResultCodes.Success;
            if (setWaitCursor)
                ControlsGlobals.UserInterface.SetWindowCursor(WindowCursorTypes.Default);

            if (ShowSqlWindow)
            {
                ControlsGlobals.UserInterface.ShowMessageBox(result.DebugMessage, "SQL Process Success",
                    RsMessageBoxIcons.Information);
            }

            return result;
        }

        protected virtual void ClearConnectionPools()
        {

        }

        private int ExecuteSql(IDbConnection connection, string sql, DataProcessResult queryResult)
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
                queryResult.Message = $"SQL Execution Error!\r\n\r\n{e.Message}";
                queryResult.ProcessedSqlStatement = sql;
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
            ControlsGlobals.UserInterface.SetWindowCursor(WindowCursorTypes.Default);
            ControlsGlobals.UserInterface.ShowMessageBox(exception.Message, debugMessage, RsMessageBoxIcons.Error);
        }

        public virtual string GetIdentityInsertSql(string tableName)
        {
            return $"SET IDENTITY_INSERT {SqlGenerator.FormatSqlObject(tableName)} ON";
        }
    }
}
