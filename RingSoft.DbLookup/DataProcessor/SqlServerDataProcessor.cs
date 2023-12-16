// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 07-01-2023
// ***********************************************************************
// <copyright file="SqlServerDataProcessor.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Data;
using System.Data.SqlClient;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.DataProcessor.SelectSqlGenerator;
using RingSoft.DbLookup.QueryBuilder;

namespace RingSoft.DbLookup.DataProcessor
{
    /// <summary>
    /// Enum SecurityTypes
    /// </summary>
    public enum SecurityTypes
    {
        /// <summary>
        /// The windows authentication
        /// </summary>
        WindowsAuthentication = 0,
        /// <summary>
        /// The SQL login
        /// </summary>
        SqlLogin = 1
    }

    /// <summary>
    /// Processes data for the Microsoft SQL Server database platform.
    /// </summary>
    /// <seealso cref="DbDataProcessor" />
    public class SqlServerDataProcessor : DbDataProcessor
    {
        /// <summary>
        /// Gets the database connection string.
        /// </summary>
        /// <value>The connection string.</value>
        public override string ConnectionString => GenerateConnectionString();

        /// <summary>
        /// Implement this to create and return the SQL generator which will be used in GetData.
        /// </summary>
        /// <value>The SQL generator.</value>
        public override DbSelectSqlGenerator SqlGenerator => _sqlGenerator;

        /// <summary>
        /// Gets or sets the server name.
        /// </summary>
        /// <value>The server name.</value>
        public string Server { get; set; }

        /// <summary>
        /// Gets or sets the database.
        /// </summary>
        /// <value>The database.</value>
        public string Database { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the type of the SQL Server security.
        /// </summary>
        /// <value>The type of the security.</value>
        public SecurityTypes SecurityType { get; set; }

        /// <summary>
        /// The SQL generator
        /// </summary>
        private readonly SqlServerSelectSqlGenerator _sqlGenerator = new SqlServerSelectSqlGenerator();

        /// <summary>
        /// Implement this to create and open the database connection.
        /// </summary>
        /// <returns>System.String.</returns>
        public override string GetDatabaseListSql()
        {
            return "SELECT name FROM master.dbo.sysdatabases";
        }

        /// <summary>
        /// Gets the drop database SQL.
        /// </summary>
        /// <param name="databaseName">Name of the database.</param>
        /// <returns>System.String.</returns>
        public override string GetDropDatabaseSql(string databaseName)
        {
            return $"DROP DATABASE IF EXISTS {SqlGenerator.FormatSqlObject(databaseName)}";
        }

        /// <summary>
        /// Drops the database.
        /// </summary>
        /// <returns>DataProcessResult.</returns>
        public override DataProcessResult DropDatabase()
        {
            var originalDatabase = Database;
            Database = "master";

            var context = SystemGlobals.DataRepository.GetDataContext(this);
            var sql = GetDropDatabaseSql(originalDatabase);

            var successResult = new DataProcessResult("Success")
            {
                ConnectionString = ConnectionString,
                ProcessedSqlStatement = sql,
                ResultCode = GetDataResultCodes.Success,
            };
            var result = context.OpenConnection();
            if (result)
            {
                result = context.ExecuteSql(sql);
            }

            context.CloseConnection();
            Database = originalDatabase;

            if (result)
            {
                return successResult;
            }
            //var result = ExecuteSql(GetDropDatabaseSql(originalDatabase), 
            //    false, true, false);
            return new DataProcessResult("Fail")
            {
                ConnectionString = ConnectionString,
                ResultCode = GetDataResultCodes.DbConnectError,
            };
        }

        /// <summary>
        /// Tests the connection.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public override bool TestConnection()
        {
            ControlsGlobals.UserInterface.SetWindowCursor(WindowCursorTypes.Wait);
            var context = SystemGlobals.DataRepository.GetDataContext(this);
            context.SetConnectionString(GenerateConnectionString());
            var result = context.OpenConnection();
            if (result)
            {
                context.CloseConnection();
            }
            context.SetConnectionString(null);
            ControlsGlobals.UserInterface.SetWindowCursor(WindowCursorTypes.Default);
            return result;
        }

        /// <summary>
        /// Generates the connection string.
        /// </summary>
        /// <returns>System.String.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        private string GenerateConnectionString()
        {
            var connectionString = $"data source={Server};initial catalog={Database};Pooling=false;";

            switch (SecurityType)
            {
                case SecurityTypes.WindowsAuthentication:
                    connectionString += "Integrated Security=SSPI;";
                    break;
                case SecurityTypes.SqlLogin:
                    connectionString += $"user id={UserName};password={Password};";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            connectionString += "TrustServerCertificate=True;";
            return connectionString;
        }

    }
}
