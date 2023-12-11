// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 07-01-2023
// ***********************************************************************
// <copyright file="SqliteDataProcessor.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.DataProcessor.SelectSqlGenerator;
using System;
using System.Data;

namespace RingSoft.DbLookup.DataProcessor
{
    /// <summary>
    /// Processes data for a Sqlite file name.
    /// </summary>
    /// <seealso cref="DbDataProcessor" />
    public class SqliteDataProcessor : DbDataProcessor
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
        public override DbSelectSqlGenerator SqlGenerator => _generator;

        /// <summary>
        /// Gets or sets the name of the Sqlite .sqlite file.
        /// </summary>
        /// <value>The name of the file.</value>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        /// <value>The file path.</value>
        public string FilePath { get; set; }

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        public string Password { get; set; }

        /// <summary>
        /// The generator
        /// </summary>
        private readonly SqliteSelectSqlGenerator _generator = new SqliteSelectSqlGenerator();

        /// <summary>
        /// Implement this to create and open the database connection.
        /// </summary>
        /// <returns>System.String.</returns>

        public override string GetDatabaseListSql()
        {
            return string.Empty;
        }

        /// <summary>
        /// Gets the drop database SQL.
        /// </summary>
        /// <param name="databaseName">Name of the database.</param>
        /// <returns>System.String.</returns>
        public override string GetDropDatabaseSql(string databaseName)
        {
            return string.Empty;
        }

        /// <summary>
        /// Drops the database.
        /// </summary>
        /// <returns>DataProcessResult.</returns>
        public override DataProcessResult DropDatabase()
        {
            var path = FilePath;
            if (!path.EndsWith("\\"))
                path += "\\";
            var filePath = $"{path}{FileName}";
            var file = new System.IO.FileInfo(filePath);
            var result = new DataProcessResult("");
            result.ResultCode = GetDataResultCodes.Success;
            try
            {
                file.Delete();
            }
            catch (Exception e)
            {
                //ControlsGlobals.UserInterface.ShowMessageBox(e.Message, "Error!", RsMessageBoxIcons.Error);
                result.ResultCode = GetDataResultCodes.SqlError;
                result.Message = e.Message;
            }

            return result;
        }

        /// <summary>
        /// Generates the connection string.
        /// </summary>
        /// <returns>System.String.</returns>
        private string GenerateConnectionString()
        {
            if (!FilePath.EndsWith("\\"))
                FilePath += "\\";

            var connectionString = $"Data Source={FilePath}{FileName};";
            connectionString += "Pooling=false;";
            if (!Password.IsNullOrEmpty())
                connectionString += $"Password={Password};";

            //connectionString += "Version=3;";

            return connectionString;
        }

        /// <summary>
        /// Closes the database connection.
        /// </summary>
        /// <param name="connection">The connection.</param>
        public override void CloseConnection(IDbConnection connection)
        {
        }

        /// <summary>
        /// Gets the identity insert SQL.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="setOn">if set to <c>true</c> [set on].</param>
        /// <returns>System.String.</returns>
        public override string GetIdentityInsertSql(string tableName, bool setOn)
        {
            return string.Empty;
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
    }
}
