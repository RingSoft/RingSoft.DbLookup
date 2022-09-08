using System.IO;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.DataProcessor;

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
        None = 0,
        SqlServer = 1,
        MySql = 2,
    }

    public class RegistrySettings
    {
        private const string EntityFrameworkVersionKey = "EfVersion";
        private const string SqlServerServerNameKey = "SqlServerServerName";
        private const string SqlServerAuthTypeKey = "SqlServerAuthType";
        private const string SqlServerUserNameKey = "SqlServerUserName";
        private const string SqlServerPasswordKey = "SqlServerPassword";
        private const string SqlServerNorthwindDbNameKey = "SqlServerNorthwindDbName";
        private const string SqlServerMegaDbNameKey = "SqlServerMegaDbName";
        private const string MySqlServerNameKey = "MySqlServerName";
        private const string MySqlUserNameKey = "MySqlUserName";
        private const string MySqlPasswordKey = "MySqlPassword";
        private const string MySqlNorthwindDbNameKey = "MySqlNorthwindDbName";
        private const string MySqlMegaDbNameKey = "MySqlMegaDbName";
        private const string NorthwindPlatformTypeKey = "NorthwindPlatformType";
        private const string NorthwindSqliteFileNameKey = "NorthwindSqliteFileName";
        private const string MegaDbPlatformTypeKey = "MegaDbPlatformType";

        public const string SqlServerNorthwindDatabaseNameConst = "Northwind";
        public const string SqlServerMegaDbDatabaseNameConst = "MegaDb";
        public const string MySqlNorthwindDatabaseNameConst = "northwind";
        public const string MySqlMegaDbDatabaseNameConst = "megadb";

        public const string RegistryRoot = "Registry";

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

        private static XmlProcessor _registryXml = new XmlProcessor(RegistryRoot);

        public RegistrySettings()
        {
            LoadFromRegistry();
        }

        internal static void LoadFromRegistryFile()
        {
            if (File.Exists(RsDbLookupAppGlobals.RegistryFileName))
            {
                var xml = RsDbLookupAppGlobals.OpenTextFile(RsDbLookupAppGlobals.RegistryFileName);
                _registryXml.LoadFromXml(xml);
            }
        }

        public static EntityFrameworkVersions GetEntityFrameworkVersion()
        {
            //var efVersion = _registryXml.GetElementValue(EntityFrameworkVersionKey,
            //    ((int)EntityFrameworkVersions.EntityFrameworkCore3).ToString());
            //return (EntityFrameworkVersions)efVersion.ToInt();
            return EntityFrameworkVersions.EntityFrameworkCore3;
        }

        public void LoadFromRegistry()
        {
            EntityFrameworkVersion = GetEntityFrameworkVersion();

            SqlServerServerName = _registryXml.GetElementValue(SqlServerServerNameKey, "localhost\\SQLEXPRESS");
            var authType = _registryXml.GetElementValue(SqlServerAuthTypeKey,
                ((int)SecurityTypes.WindowsAuthentication).ToString());
            SqlServerSecurityType = (SecurityTypes)authType.ToInt();
            SqlServerUserName = _registryXml.GetElementValue(SqlServerUserNameKey, "sa");
            SqlServerPassword =
                RsDbLookupAppGlobals.DecryptString(_registryXml.GetElementValue(SqlServerPasswordKey, ""));
            SqlServerNorthwindDbName =
                _registryXml.GetElementValue(SqlServerNorthwindDbNameKey, SqlServerNorthwindDatabaseNameConst);
            SqlServerMegaDbName =
                _registryXml.GetElementValue(SqlServerMegaDbNameKey, SqlServerMegaDbDatabaseNameConst);

            MySqlServerName = _registryXml.GetElementValue(MySqlServerNameKey, "localhost");
            MySqlUserName = _registryXml.GetElementValue(MySqlUserNameKey, "root");
            MySqlPassword = RsDbLookupAppGlobals.DecryptString(_registryXml.GetElementValue(MySqlPasswordKey, ""));
            MySqlNorthwindDbName =
                _registryXml.GetElementValue(MySqlNorthwindDbNameKey, MySqlNorthwindDatabaseNameConst);
            MySqlMegaDbName = _registryXml.GetElementValue(MySqlMegaDbNameKey, MySqlMegaDbDatabaseNameConst);

            var northwindPlatformType = _registryXml.GetElementValue(NorthwindPlatformTypeKey,
                ((int)NorthwindDbPlatforms.Sqlite).ToString());
            NorthwindPlatformType = (NorthwindDbPlatforms) northwindPlatformType.ToInt();
            NorthwindSqliteFileName = _registryXml.GetElementValue(NorthwindSqliteFileNameKey,
                $"{RsDbLookupAppGlobals.AppDataDirectory}\\Northwind\\Northwind.sqlite");

            var megaDbPlatformType = _registryXml.GetElementValue(MegaDbPlatformTypeKey,
                ((int)MegaDbPlatforms.None).ToString());
            MegaDbPlatformType = (MegaDbPlatforms) megaDbPlatformType.ToInt();
        }

        public void SaveToRegistry()
        {
            _registryXml.SetElementValue(EntityFrameworkVersionKey, ((int)EntityFrameworkVersion).ToString());

            _registryXml.SetElementValue(SqlServerServerNameKey, SqlServerServerName);
            _registryXml.SetElementValue(SqlServerAuthTypeKey, ((int)SqlServerSecurityType).ToString());
            _registryXml.SetElementValue(SqlServerUserNameKey, SqlServerUserName);
            _registryXml.SetElementValue(SqlServerPasswordKey, RsDbLookupAppGlobals.EncryptString(SqlServerPassword));
            _registryXml.SetElementValue(SqlServerNorthwindDbNameKey, SqlServerNorthwindDbName);
            _registryXml.SetElementValue(SqlServerMegaDbNameKey, SqlServerMegaDbName);

            _registryXml.SetElementValue(MySqlServerNameKey, MySqlServerName);
            _registryXml.SetElementValue(MySqlUserNameKey, MySqlUserName);
            _registryXml.SetElementValue(MySqlPasswordKey, RsDbLookupAppGlobals.EncryptString(MySqlPassword));
            _registryXml.SetElementValue(MySqlNorthwindDbNameKey, MySqlNorthwindDbName);
            _registryXml.SetElementValue(MySqlMegaDbNameKey, MySqlMegaDbName);

            _registryXml.SetElementValue(NorthwindPlatformTypeKey, ((int)NorthwindPlatformType).ToString());
            _registryXml.SetElementValue(NorthwindSqliteFileNameKey, NorthwindSqliteFileName);
            _registryXml.SetElementValue(MegaDbPlatformTypeKey, ((int)MegaDbPlatformType).ToString());

            var xml = _registryXml.OutputXml();
            RsDbLookupAppGlobals.WriteTextFile(RsDbLookupAppGlobals.RegistryFileName, xml);
        }
    }
}