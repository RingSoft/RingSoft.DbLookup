// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 07-01-2023
// ***********************************************************************
// <copyright file="MySqlDataProcessor.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DbLookup.DataProcessor.SelectSqlGenerator;

namespace RingSoft.DbLookup.DataProcessor
{
    /// <summary>
    /// Processes data for the MySQL database platform.
    /// </summary>
    /// <seealso cref="DbDataProcessor" />

    public class MySqlDataProcessor : DbDataProcessor
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
        /// Gets or sets the server name.
        /// </summary>
        /// <value>The server name.</value>
        public string Server { get; set; }

        /// <summary>
        /// Gets or sets the database name.
        /// </summary>
        /// <value>The database.</value>
        public string Database { get; set; }

        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        /// <value>The name of the user.</value>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        public string Password { get; set; }

        /// <summary>
        /// The generator
        /// </summary>
        private readonly MySqlSelectSqlGenerator _generator = new MySqlSelectSqlGenerator();


        /// <summary>
        /// Generates the connection string.
        /// </summary>
        /// <returns>System.String.</returns>
        private string GenerateConnectionString()
        {
            //OleDB
            //Provider=MySQLProv;Data Source=mydb;User Id=myUsername;Password=myPassword;SslMode=none
            var connectionString = $"server={Server};database={Database};uid={UserName};password={Password};";
            return connectionString;
        }

        /// <summary>
        /// Gets the database list SQL.
        /// </summary>
        /// <returns>System.String.</returns>
        public override string GetDatabaseListSql()
        {
            return "SHOW DATABASES;";
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
            Database = "sys";
            var result = ExecuteSql(GetDropDatabaseSql(originalDatabase), false,
                true, false);
            Database = originalDatabase;
            return result;

        }

        /// <summary>
        /// Tests the connection.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override bool TestConnection()
        {
            throw new System.NotImplementedException();
        }
    }
}
