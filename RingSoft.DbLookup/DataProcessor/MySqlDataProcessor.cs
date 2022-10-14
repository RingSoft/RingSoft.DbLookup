using System;
using System.Data;
using MySql.Data.MySqlClient;
using RingSoft.DbLookup.DataProcessor.SelectSqlGenerator;
using RingSoft.DbLookup.QueryBuilder;

namespace RingSoft.DbLookup.DataProcessor
{
    /// <summary>
    /// Processes data for the MySQL database platform.
    /// </summary>
    /// <seealso cref="DbDataProcessor" />

    public class MySqlDataProcessor : DbDataProcessor
    {
        public override string ConnectionString => GenerateConnectionString();

        public override DbSelectSqlGenerator SqlGenerator => _generator;

        /// <summary>
        /// Gets or sets the server name.
        /// </summary>
        /// <value>
        /// The server name.
        /// </value>
        public string Server { get; set; }

        /// <summary>
        /// Gets or sets the database name.
        /// </summary>
        /// <value>
        /// The database.
        /// </value>
        public string Database { get; set; }

        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public string Password { get; set; }

        private readonly MySqlSelectSqlGenerator _generator = new MySqlSelectSqlGenerator();

        /// <summary>
        /// Implement this to create and open the database connection.
        /// </summary>
        protected override IDbConnection CreateConnection()
        {
            var connectionString = GenerateConnectionString();
            return new MySqlConnection(connectionString);
        }

        protected override IDataAdapter GetDataAdapter(IDbConnection connection, string sqlStatement)
        {
            IDataAdapter adapter = null;
            if (connection is MySqlConnection mySqlConnection)
                adapter = new MySqlDataAdapter(sqlStatement, mySqlConnection);

            return adapter;
        }

        protected override IDbCommand GetDbCommand(IDbConnection connection, string sqlStatement)
        {
            MySqlCommand command = null;
            if (connection is MySqlConnection mySqlConnection)
                command = new MySqlCommand(sqlStatement, mySqlConnection);

            return command;
        }

        private string GenerateConnectionString()
        {
            //OleDB
            //Provider=MySQLProv;Data Source=mydb;User Id=myUsername;Password=myPassword;SslMode=none
            var connectionString = $"server={Server};database={Database};uid={UserName};password={Password};";
            return connectionString;
        }

        /// <summary>
        /// Gets the list of databases.
        /// </summary>
        /// <returns>A GetDataResult object containing a list of databases or an error.</returns>
        public override DataProcessResult GetListOfDatabases()
        {
            var originalDatabase = Database;
            Database = "mysql";
            var query = new SelectQuery("")
            {
                RawSql = "SHOW DATABASES;"
            };

            var result = GetData(query);
            Database = originalDatabase;
            return result;
        }

        public override bool DropDatabase()
        {
            var originalDatabase = Database;
            Database = "sys";
            var result = ExecuteSql($"DROP DATABASE IF EXISTS {SqlGenerator.FormatSqlObject(originalDatabase)}");
            Database = originalDatabase;
            return result.ResultCode == GetDataResultCodes.Success;

        }
    }
}
