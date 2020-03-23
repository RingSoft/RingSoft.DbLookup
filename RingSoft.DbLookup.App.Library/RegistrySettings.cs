using RingSoft.DbLookup.GetDataProcessor;

namespace RingSoft.DbLookup.App.Library
{
    public enum NorthwindDbPlatforms
    {
        SqlServer = 0,
        MySql = 1,
        Sqlite = 2
    }

    public enum MegaDbPlatforms
    {
        SqlServer = 0,
        MySql = 1,
    }

    public class RegistrySettings
    {
        private const string EntityFrameworkVersionKey = "EFVERSION";
        private const string SqlServerServerNameKey = "SQLSERVER_SERVERNAME";
        private const string SqlServerAuthTypeKey = "SQLSERVER_AUTHTYPE";
        private const string SqlServerUserNameKey = "SQLSERVER_USERNAME";
        private const string SqlServerPasswordKey = "SQLSERVER_PASSWORD";
        private const string SqlServerNorthwindDbNameKey = "SQLSERVER_NORTHWIND";
        private const string SqlServerMegaDbNameKey = "SQLSERVER_MEGADB";
        private const string MySqlServerNameKey = "MYSQL_SERVERNAME";
        private const string MySqlUserNameKey = "MYSQL_USERNAME";
        private const string MySqlPasswordKey = "MYSQL_PASSWORD";
        private const string MySqlNorthwindDbNameKey = "MYSQL_NORTHWIND";
        private const string MySqlMegaDbNameKey = "MYSQL_MEGADB";
        private const string NorthwindPlatformTypeKey = "NORTHWIND_PLATFORMTYPE";
        private const string NorthwindSqliteFileNameKey = "NORTHWIND_SQLITE_FILENAME";
        private const string MegaDbPlatformTypeKey = "MEGADB_PLATFORMTYPE";

        public const string SqlServerNorthwindDatabaseNameConst = "Northwind";
        public const string SqlServerMegaDbDatabaseNameConst = "MegaDb";
        public const string MySqlNorthwindDatabaseNameConst = "northwind";
        public const string MySqlMegaDbDatabaseNameConst = "megadb";
        
        public EntityFrameworkVersions EntityFrameworkVersion { get; set; }
        public string SqlServerServerName { get; set; }

        public SecurityTypes SqlServerSecurityType { get; set; }

        public string SqlServerUserName { get; set; }

        public string SqlServerPassword { get; set; }

        public string SqlServerNorthwindDbName { get; set; }

        public string SqlServerMegaDbName { get; set; }

        public string MySqlServerName { get; set; }

        public string MySqlUserName { get; set; }

        public string MySqlPassword { get; set; }

        public string MySqlNorthwindDbName { get; set; }

        public string MySqlMegaDbName { get; set; }

        public NorthwindDbPlatforms NorthwindPlatformType { get; set; }

        public string NorthwindSqliteFileName { get; set; }

        public MegaDbPlatforms MegaDbPlatformType { get; set; }

        public RegistrySettings()
        {
            LoadFromRegistry();
        }

        public static EntityFrameworkVersions GetEntityFrameworkVersion()
        {
            var efVersion = RsDbLookupAppGlobals.GetSetting(EntityFrameworkVersionKey,
                ((int)EntityFrameworkVersions.EntityFrameworkCore3).ToString());
            return (EntityFrameworkVersions)efVersion.ToInt();
        }

        public void LoadFromRegistry()
        {
            EntityFrameworkVersion = GetEntityFrameworkVersion();

            SqlServerServerName = RsDbLookupAppGlobals.GetSetting(SqlServerServerNameKey, "localhost\\SQLEXPRESS");
            var authType = RsDbLookupAppGlobals.GetSetting(SqlServerAuthTypeKey,
                ((int)SecurityTypes.WindowsAuthentication).ToString());
            SqlServerSecurityType = (SecurityTypes)authType.ToInt();
            SqlServerUserName = RsDbLookupAppGlobals.GetSetting(SqlServerUserNameKey, "sa");
            SqlServerPassword = RsDbLookupAppGlobals.GetSetting(SqlServerPasswordKey, "");
            SqlServerNorthwindDbName =
                RsDbLookupAppGlobals.GetSetting(SqlServerNorthwindDbNameKey, SqlServerNorthwindDatabaseNameConst);
            SqlServerMegaDbName =
                RsDbLookupAppGlobals.GetSetting(SqlServerMegaDbNameKey, SqlServerMegaDbDatabaseNameConst);

            MySqlServerName = RsDbLookupAppGlobals.GetSetting(MySqlServerNameKey, "localhost");
            MySqlUserName = RsDbLookupAppGlobals.GetSetting(MySqlUserNameKey, "root");
            MySqlPassword = RsDbLookupAppGlobals.GetSetting(MySqlPasswordKey, "");
            MySqlNorthwindDbName =
                RsDbLookupAppGlobals.GetSetting(MySqlNorthwindDbNameKey, MySqlNorthwindDatabaseNameConst);
            MySqlMegaDbName = RsDbLookupAppGlobals.GetSetting(MySqlMegaDbNameKey, MySqlMegaDbDatabaseNameConst);

            var northwindPlatformType = RsDbLookupAppGlobals.GetSetting(NorthwindPlatformTypeKey,
                ((int)NorthwindDbPlatforms.Sqlite).ToString());
            NorthwindPlatformType = (NorthwindDbPlatforms) northwindPlatformType.ToInt();
            NorthwindSqliteFileName = RsDbLookupAppGlobals.GetSetting(NorthwindSqliteFileNameKey,
                $@"{RsDbLookupAppGlobals.AssemblyDirectory}\Northwind\Northwind.sqlite");

            var megaDbPlatformType = RsDbLookupAppGlobals.GetSetting(MegaDbPlatformTypeKey,
                ((int)MegaDbPlatforms.SqlServer).ToString());
            MegaDbPlatformType = (MegaDbPlatforms) megaDbPlatformType.ToInt();
        }

        public void SaveToRegistry()
        {
            RsDbLookupAppGlobals.SaveSetting(EntityFrameworkVersionKey, ((int)EntityFrameworkVersion).ToString());

            RsDbLookupAppGlobals.SaveSetting(SqlServerServerNameKey, SqlServerServerName);
            RsDbLookupAppGlobals.SaveSetting(SqlServerAuthTypeKey, ((int)SqlServerSecurityType).ToString());
            RsDbLookupAppGlobals.SaveSetting(SqlServerUserNameKey, SqlServerUserName);
            RsDbLookupAppGlobals.SaveSetting(SqlServerPasswordKey, SqlServerPassword);
            RsDbLookupAppGlobals.SaveSetting(SqlServerNorthwindDbNameKey, SqlServerNorthwindDbName);
            RsDbLookupAppGlobals.SaveSetting(SqlServerMegaDbNameKey, SqlServerMegaDbName);

            RsDbLookupAppGlobals.SaveSetting(MySqlServerNameKey, MySqlServerName);
            RsDbLookupAppGlobals.SaveSetting(MySqlUserNameKey, MySqlUserName);
            RsDbLookupAppGlobals.SaveSetting(MySqlPasswordKey, MySqlPassword);
            RsDbLookupAppGlobals.SaveSetting(MySqlNorthwindDbNameKey, MySqlNorthwindDbName);
            RsDbLookupAppGlobals.SaveSetting(MySqlMegaDbNameKey, MySqlMegaDbName);

            RsDbLookupAppGlobals.SaveSetting(NorthwindPlatformTypeKey, ((int)NorthwindPlatformType).ToString());
            RsDbLookupAppGlobals.SaveSetting(NorthwindSqliteFileNameKey, NorthwindSqliteFileName);
            RsDbLookupAppGlobals.SaveSetting(MegaDbPlatformTypeKey, ((int)MegaDbPlatformType).ToString());
        }
    }
}
