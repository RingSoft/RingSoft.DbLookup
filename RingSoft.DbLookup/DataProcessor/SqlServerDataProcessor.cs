using System;
using System.Data;
using System.Data.SqlClient;
using RingSoft.DbLookup.DataProcessor.SelectSqlGenerator;
using RingSoft.DbLookup.QueryBuilder;

namespace RingSoft.DbLookup.DataProcessor
{
    public enum SecurityTypes
    {
        WindowsAuthentication = 0,
        SqlLogin = 1
    }

    /// <summary>
    /// Processes data for the Microsoft SQL Server database platform.
    /// </summary>
    /// <seealso cref="DbDataProcessor" />
    public class SqlServerDataProcessor : DbDataProcessor
    {
        public override string ConnectionString => GenerateConnectionString();

        /// <summary>
        /// Implement this to create and return the SQL generator which will be used in GetData.
        /// </summary>
        /// <value>
        /// The SQL generator.
        /// </value>
        public override DbSelectSqlGenerator SqlGenerator => _sqlGenerator;

        /// <summary>
        /// Gets or sets the server name.
        /// </summary>
        /// <value>
        /// The server name.
        /// </value>
        public string Server { get; set; }

        /// <summary>
        /// Gets or sets the database.
        /// </summary>
        /// <value>
        /// The database.
        /// </value>
        public string Database { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
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

        /// <summary>
        /// Gets or sets the type of the SQL Server security.
        /// </summary>
        /// <value>
        /// The type of the security.
        /// </value>
        public SecurityTypes SecurityType { get; set; }

        private readonly SqlServerSelectSqlGenerator _sqlGenerator = new SqlServerSelectSqlGenerator();

        /// <summary>
        /// Implement this to create and open the database connection.
        /// </summary>
        protected override IDbConnection CreateConnection()
        {
            return new SqlConnection(GenerateConnectionString());
        }

        protected override IDataAdapter GetDataAdapter(IDbConnection connection, string sqlStatement)
        {
            SqlDataAdapter adapter = null;
            if (connection is SqlConnection sqlConnection)
                adapter = new SqlDataAdapter(sqlStatement, sqlConnection);

            return adapter;
        }

        protected override IDbCommand GetDbCommand(IDbConnection connection, string sqlStatement)
        {
            IDbCommand command = null;
            if (connection is SqlConnection sqlConnection)
                command = new SqlCommand(sqlStatement, sqlConnection);

            return command;
        }

        public override DataProcessResult GetListOfDatabases()
        {
            var originalDatabase = Database;
            Database = "master";
            var query = new SelectQuery("")
            {
                RawSql = "SELECT name FROM master.dbo.sysdatabases"
            };

            var result = GetData(query);
            Database = originalDatabase;
            return result;
        }

        public override DataProcessResult DropDatabase()
        {
            var originalDatabase = Database;
            Database = "master";
            var result = ExecuteSql($"DROP DATABASE IF EXISTS {SqlGenerator.FormatSqlObject(originalDatabase)}", 
                false, true, false);
            Database = originalDatabase;
            return result;
        }

        private string GenerateConnectionString()
        {
            var connectionString = $"data source={Server};initial catalog={Database};";

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

            return connectionString;
        }

        protected override void ClearConnectionPools()
        {
            SqlConnection.ClearAllPools();
            base.ClearConnectionPools();
        }

    }
}
