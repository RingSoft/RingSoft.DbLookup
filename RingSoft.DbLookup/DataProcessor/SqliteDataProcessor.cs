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

        public override string GetDatabaseListSql()
        {
            return string.Empty;
        }

        public override string GetDropDatabaseSql(string databaseName)
        {
            return string.Empty;
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
        }

        public override string GetIdentityInsertSql(string tableName, bool setOn)
        {
            return string.Empty;
        }

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
