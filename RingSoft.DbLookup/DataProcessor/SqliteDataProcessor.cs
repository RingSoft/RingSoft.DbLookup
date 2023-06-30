using System;
using System.Data;
using System.Data.SQLite;
using RingSoft.DbLookup.DataProcessor.SelectSqlGenerator;
using RingSoft.DataEntryControls.Engine;

namespace RingSoft.DbLookup.DataProcessor
{
    /// <summary>
    /// Processes data for a Sqlite file name.
    /// </summary>
    /// <seealso cref="DbDataProcessor" />
    public class SqliteDataProcessor : DbDataProcessor
    {
        public override string ConnectionString => GenerateConnectionString();

        public override DbSelectSqlGenerator SqlGenerator => _generator;

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

        public override void CloseConnection(IDbConnection connection)
        {
            if (connection is SQLiteConnection sqLiteConnection && !IsClosed)
            {
                IsClosed = true;
                sqLiteConnection.Close();
                sqLiteConnection.Dispose();
                GC.Collect();
            }
        }

        public override string GetIdentityInsertSql(string tableName, bool setOn)
        {
            return string.Empty;
        }
    }
}
