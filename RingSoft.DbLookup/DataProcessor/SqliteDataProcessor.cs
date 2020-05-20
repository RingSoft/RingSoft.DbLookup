using System;
using System.Data;
using System.Data.SQLite;
using RingSoft.DbLookup.DataProcessor.SelectSqlGenerator;

namespace RingSoft.DbLookup.DataProcessor
{
    /// <summary>
    /// Retrieves data from a Sqlite database file.
    /// </summary>
    /// <seealso cref="DbDataProcessor" />
    public class SqliteDataProcessor : DbDataProcessor
    {
        public override string ConnectionString => GenerateConnectionString();

        protected override DbSelectSqlGenerator SqlGenerator => _generator;

        /// <summary>
        /// Gets or sets the name of the Sqlite .sqlite file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        /// <value>
        /// The file path.
        /// </value>
        public string FilePath { get; set; }

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public string Password { get; set; }

        private readonly SqliteSelectSqlGenerator _generator = new SqliteSelectSqlGenerator();

        /// <summary>
        /// Implement this to create and open the database connection.
        /// </summary>
        protected override IDbConnection CreateConnection()
        {
            return new SQLiteConnection(GenerateConnectionString());
        }

        /// <summary>
        /// Implement this to create and return the data adapter used to fill a DataSet with the results of the SQL statement.
        /// </summary>
        /// <param name="connection">The connection object.</param>
        /// <param name="sqlStatement">The SQL statement.</param>
        /// <returns></returns>
        protected override IDataAdapter GetDataAdapter(IDbConnection connection, string sqlStatement)
        {
            SQLiteDataAdapter adapter = null;
            if (connection is SQLiteConnection sqliteConnection)
                adapter = new SQLiteDataAdapter(sqlStatement, sqliteConnection);

            return adapter;
        }

        protected override IDbCommand GetDbCommand(IDbConnection connection, string sqlStatement)
        {
            SQLiteCommand command = null;
            if (connection is SQLiteConnection sqliteConnection)
                    command = new SQLiteCommand(sqlStatement, sqliteConnection);

            return command;
        }

        public override DataProcessResult GetListOfDatabases()
        {
            throw new NotImplementedException("Not relevant for Sqlite.");
        }

        private string GenerateConnectionString()
        {
            if (!FilePath.EndsWith("\\"))
                FilePath += "\\";

            var connectionString = $"Data Source={FilePath}{FileName};";
            if (!Password.IsNullOrEmpty())
                connectionString += $"Password={Password};";

            //connectionString += "Version=3;";

            return connectionString;
        }
    }
}
