using RingSoft.DbLookup.DataProcessor.SelectSqlGenerator;

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


        private string GenerateConnectionString()
        {
            //OleDB
            //Provider=MySQLProv;Data Source=mydb;User Id=myUsername;Password=myPassword;SslMode=none
            var connectionString = $"server={Server};database={Database};uid={UserName};password={Password};";
            return connectionString;
        }

        public override string GetDatabaseListSql()
        {
            return "SHOW DATABASES;";
        }

        public override string GetDropDatabaseSql(string databaseName)
        {
            return $"DROP DATABASE IF EXISTS {SqlGenerator.FormatSqlObject(databaseName)}";
        }

        public override DataProcessResult DropDatabase()
        {
            var originalDatabase = Database;
            Database = "sys";
            var result = ExecuteSql(GetDropDatabaseSql(originalDatabase), false,
                true, false);
            Database = originalDatabase;
            return result;

        }
    }
}
