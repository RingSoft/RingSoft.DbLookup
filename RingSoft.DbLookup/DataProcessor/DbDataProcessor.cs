// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 07-01-2023
// ***********************************************************************
// <copyright file="DbDataProcessor.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.DataProcessor.SelectSqlGenerator;
using RingSoft.DbLookup.QueryBuilder;

namespace RingSoft.DbLookup.DataProcessor
{
    /// <summary>
    /// Enum DbFieldTypes
    /// </summary>
    public enum DbFieldTypes
    {
        /// <summary>
        /// The integer
        /// </summary>
        Integer,
        /// <summary>
        /// The string
        /// </summary>
        String,
        /// <summary>
        /// The decimal
        /// </summary>
        Decimal,
        /// <summary>
        /// The date time
        /// </summary>
        DateTime,
        /// <summary>
        /// The byte
        /// </summary>
        Byte,
        /// <summary>
        /// The bool
        /// </summary>
        Bool,
        /// <summary>
        /// The memo
        /// </summary>
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
        /// <value>The connection string.</value>
        public abstract string ConnectionString { get; }

        /// <summary>
        /// Gets or sets a value indicating whether to keep the database connection open after retrieving data.  If set to true,
        /// then the calling code will need to close the connection manually by running the CloseConnection method.
        /// </summary>
        /// <value><c>true</c> if [keep database connection open]; otherwise, <c>false</c>.</value>
        public bool KeepConnectionOpen { get; set; }

        /// <summary>
        /// Implement this to create and return the SQL generator which will be used in GetData.
        /// </summary>
        /// <value>The SQL generator.</value>
        public abstract DbSelectSqlGenerator SqlGenerator { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is closed.
        /// </summary>
        /// <value><c>true</c> if this instance is closed; otherwise, <c>false</c>.</value>
        public bool IsClosed { get; internal set; }

        /// <summary>
        /// Gets the user interface for this class to interact with.
        /// </summary>
        /// <value>Gets the user interface.</value>
        public static IDbLookupUserInterface UserInterface { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [dont display exceptions].
        /// </summary>
        /// <value><c>true</c> if [dont display exceptions]; otherwise, <c>false</c>.</value>
        public static bool DontDisplayExceptions { get; set; }

        /// <summary>
        /// Gets the last exception.
        /// </summary>
        /// <value>The last exception.</value>
        public static string LastException { get; private set; }


        /// <summary>
        /// Gets a value indicating whether [show SQL window].
        /// </summary>
        /// <value><c>true</c> if [show SQL window]; otherwise, <c>false</c>.</value>
        internal static bool ShowSqlWindow => _showSqlWindow;

        /// <summary>
        /// The valid
        /// </summary>
        private bool _valid;

        /// <summary>
        /// Returns true if ... is valid.
        /// </summary>
        /// <value><c>true</c> if this instance is valid; otherwise, <c>false</c>.</value>
        public bool IsValid
        {
            get => _valid;
            set => _valid= value;
        }

        /// <summary>
        /// The show SQL window
        /// </summary>
        private static bool _showSqlWindow;
        /// <summary>
        /// Initializes a new instance of the <see cref="DbDataProcessor"/> class.
        /// </summary>
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
        /// Closes the database connection.
        /// </summary>
        /// <param name="connection">The connection.</param>
        public virtual void CloseConnection(IDbConnection connection)
        {
            KeepConnectionOpen = false;
            if (connection != null && !IsClosed)
            {
                IsClosed = true;
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                connection.Dispose();
                GC.Collect();
            }

        }

        /// <summary>
        /// Gets the database list SQL.
        /// </summary>
        /// <returns>System.String.</returns>
        public abstract string GetDatabaseListSql();

        /// <summary>
        /// Gets the drop database SQL.
        /// </summary>
        /// <param name="databaseName">Name of the database.</param>
        /// <returns>System.String.</returns>
        public abstract string GetDropDatabaseSql(string databaseName);

        /// <summary>
        /// Drops the database.
        /// </summary>
        /// <returns>DataProcessResult.</returns>
        public abstract DataProcessResult DropDatabase();

        /// <summary>
        /// Gets the resulting data or error after executing the query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="setWaitCursor">if set to <c>true</c> set mouse cursor to wait.</param>
        /// <param name="showError">if set to <c>true</c> [show error].</param>
        /// <returns>DataProcessResult.</returns>
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
        /// <param name="showError">if set to <c>true</c> [show error].</param>
        /// <returns>DataProcessResult.</returns>
        /// <exception cref="System.Exception">Not Valid Anymore</exception>
        public DataProcessResult GetData(QuerySet querySet, bool setWaitCursor = true, bool showError = true)
        {
            throw new Exception("Not Valid Anymore");
        }


        /// <summary>
        /// Executes the SQL.
        /// </summary>
        /// <param name="sqlStatement">The SQL statement.</param>
        /// <param name="clearConnectionPools">if set to <c>true</c> clear connection pools.</param>
        /// <param name="setWaitCursor">if set to <c>true</c> set mouse cursor to wait.</param>
        /// <param name="showError">if set to <c>true</c> [show error].</param>
        /// <returns>DataProcessResult.</returns>
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
        /// <param name="showError">if set to <c>true</c> [show error].</param>
        /// <returns>DataProcessResult.</returns>
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

            var failResult = new DataProcessResult("Exec SQL Fail")
            {
                ResultCode = GetDataResultCodes.DbConnectError,
            };
            var context = SystemGlobals.DataRepository.GetDataContext(this);
            context.SetProcessor(this);
            if (!context.OpenConnection())
            {
                if (setWaitCursor)
                    ControlsGlobals.UserInterface.SetWindowCursor(WindowCursorTypes.Default);

                return failResult;
            }

            foreach (var sql in sqlsList)
            { 
                if (!context.ExecuteSql(sql))
                {
                    if (setWaitCursor)
                        ControlsGlobals.UserInterface.SetWindowCursor(WindowCursorTypes.Default);

                    context.CloseConnection();
                    return failResult;
                }
            }

            context.CloseConnection();
            if (setWaitCursor)
                ControlsGlobals.UserInterface.SetWindowCursor(WindowCursorTypes.Default);

            return new DataProcessResult("Success")
            {
                ResultCode = GetDataResultCodes.Success,
            };
        }

        /// <summary>
        /// Clears the connection pools.
        /// </summary>
        protected virtual void ClearConnectionPools()
        {

        }

        /// <summary>
        /// Displays a data exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="debugMessage">The debug message.</param>
        public static void DisplayDataException(Exception exception, string debugMessage)
        {
            if (DontDisplayExceptions)
            {
                LastException = exception.Message;
                return;
            }
            ControlsGlobals.UserInterface.SetWindowCursor(WindowCursorTypes.Default);
            ControlsGlobals.UserInterface.ShowMessageBox(exception.Message, debugMessage, RsMessageBoxIcons.Error);
        }

        /// <summary>
        /// Gets the identity insert SQL.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="setOn">if set to <c>true</c> [set on].</param>
        /// <returns>System.String.</returns>
        public virtual string GetIdentityInsertSql(string tableName, bool setOn)
        {
            var strOn = "ON";
            if (!setOn)
                strOn = "OFF";

            return $"SET IDENTITY_INSERT {SqlGenerator.FormatSqlObject(tableName)} {strOn}";
        }

        /// <summary>
        /// Tests the connection.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public abstract bool TestConnection();
    }
}
