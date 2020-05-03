using System;
using RingSoft.DbLookup.GetDataProcessor;

namespace RingSoft.DbLookup.App.Library.LibLookupContext
{
    public enum DataProcessorTypes
    {
        Sqlite,
        SqlServer,
        MySql
    }

    public abstract class AppLookupContextConfiguration
    {
        public static RegistrySettings RegistrySettings { get; } = new RegistrySettings();

        public DataProcessorTypes DataProcessorType { get; set; }

        public SqliteDataProcessor SqliteDataProcessor { get; private set; }

        public SqlServerDataProcessor SqlServerDataProcessor { get; private set; }

        public MySqlDataProcessor MySqlDataProcessor { get; private set; }

        public DbDataProcessor DataProcessor
        {
            get
            {
                switch (DataProcessorType)
                {
                    case DataProcessorTypes.Sqlite:
                        return SqliteDataProcessor;
                    case DataProcessorTypes.SqlServer:
                        return SqlServerDataProcessor;
                    case DataProcessorTypes.MySql:
                        return MySqlDataProcessor;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public AppLookupContextConfiguration()
        {
            SqliteDataProcessor = new SqliteDataProcessor();

            SqlServerDataProcessor = new SqlServerDataProcessor();

            MySqlDataProcessor = new MySqlDataProcessor();
        }

        public void Reinitialize()
        {
            Reinitialize(RegistrySettings);
        }

        public virtual void Reinitialize(RegistrySettings registrySettings)
        {
            SqlServerDataProcessor.Server = registrySettings.SqlServerServerName;
            SqlServerDataProcessor.SecurityType = registrySettings.SqlServerSecurityType;
            SqlServerDataProcessor.UserName = registrySettings.SqlServerUserName;
            SqlServerDataProcessor.Password = registrySettings.SqlServerPassword;

            MySqlDataProcessor.Server = registrySettings.MySqlServerName;
            MySqlDataProcessor.UserName = registrySettings.MySqlUserName;
            MySqlDataProcessor.Password = registrySettings.MySqlPassword;
        }

        public virtual bool TestConnection()
        {
            return true;
        }

        public virtual bool TestConnection(RegistrySettings registrySettings)
        {
            Reinitialize();
            return true;
        }
    }
}
