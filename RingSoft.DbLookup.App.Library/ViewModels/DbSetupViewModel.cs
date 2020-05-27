using RingSoft.DbLookup.App.Library.LibLookupContext;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbMaintenance;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace RingSoft.DbLookup.App.Library.ViewModels
{
    public class DbSetupViewModel : INotifyPropertyChanged
    {
        public EntityFrameworkVersions EntityFrameworkVersion
        {
            get => _registrySettings.EntityFrameworkVersion;
            set
            {
                if (_registrySettings.EntityFrameworkVersion == value)
                    return;

                _registrySettings.EntityFrameworkVersion = value;
                SetupEntityFrameworkVersionControls();
                OnPropertyChanged(nameof(EntityFrameworkVersion));
            }
        }

        public NorthwindDbPlatforms NorthwindDbPlatform
        {
            get => _registrySettings.NorthwindPlatformType;
            set
            {
                if (_registrySettings.NorthwindPlatformType == value)
                    return;

                _registrySettings.NorthwindPlatformType = value;
                SetupNorthwindSqliteControls();
                OnPropertyChanged(nameof(NorthwindDbPlatform));
            }
        }

        public bool NorthwindSqlServerRadioEnabled
        {
            get => _northwindSqlServerRadioEnabled;
            set
            {
                if (_northwindSqlServerRadioEnabled == value)
                    return;

                _northwindSqlServerRadioEnabled = value;
                OnPropertyChanged(nameof(NorthwindSqlServerRadioEnabled));
            }
        }

        public bool NorthwindMySqlRadioEnabled
        {
            get => _northwindMySqlRadioEnabled;
            set
            {
                if (_northwindMySqlRadioEnabled == value)
                    return;
                _northwindMySqlRadioEnabled = value;
                OnPropertyChanged(nameof(NorthwindMySqlRadioEnabled));
            }
        }

        public bool NorthwindSqliteRadioEnabled
        {
            get => _northwindSqliteRadioEnabled;
            set
            {
                if (_northwindSqliteRadioEnabled == value)
                    return;

                _northwindSqliteRadioEnabled = value;
                OnPropertyChanged(nameof(NorthwindSqliteRadioEnabled));
            }
        }

        public bool NorthwindSqliteControlsEnabled
        {
            get => _northwindSqliteControlsEnabled;
            set
            {
                if (_northwindSqliteControlsEnabled == value)
                    return;

                _northwindSqliteControlsEnabled = value;
                OnPropertyChanged(nameof(NorthwindSqliteControlsEnabled));
            }
        }

        public string NorthwindSqliteFileName
        {
            get => _registrySettings.NorthwindSqliteFileName;
            set
            {
                if (_registrySettings.NorthwindSqliteFileName == value)
                    return;

                _registrySettings.NorthwindSqliteFileName = value;
                OnPropertyChanged(nameof(NorthwindSqliteFileName));
            }
        }

        public MegaDbPlatforms MegaDbDbPlatform
        {
            get => _registrySettings.MegaDbPlatformType;
            set
            {
                if (_registrySettings.MegaDbPlatformType == value)
                    return;

                _registrySettings.MegaDbPlatformType = value;
                OnPropertyChanged(nameof(MegaDbDbPlatform));
            }
        }

        public bool MegaDbSqlServerRadioEnabled
        {
            get => _megaDbSqlServerRadioEnabled;
            set
            {
                if (_megaDbSqlServerRadioEnabled == value)
                    return;

                _megaDbSqlServerRadioEnabled = value;
                OnPropertyChanged(nameof(MegaDbSqlServerRadioEnabled));
            }
        }

        public bool MegaDbMySqlRadioEnabled
        {
            get => _megaDbMySqlRadioEnabled;
            set
            {
                if (_megaDbMySqlRadioEnabled == value)
                    return;

                _megaDbMySqlRadioEnabled = value;
                OnPropertyChanged(nameof(MegaDbMySqlRadioEnabled));
            }
        }

        public string SqlServerServerName
        {
            get => _registrySettings.SqlServerServerName;
            set
            {
                if (_registrySettings.SqlServerServerName == value)
                    return;

                _registrySettings.SqlServerServerName = value;
                OnPropertyChanged(nameof(SqlServerServerName));
            }
        }

        public SecurityTypes SqlServerSecurityType
        {
            get => _registrySettings.SqlServerSecurityType;
            set
            {
                if (_registrySettings.SqlServerSecurityType == value)
                    return;

                _registrySettings.SqlServerSecurityType = value;
                SetupSqlServerSecurityTypeControls();
                OnPropertyChanged(nameof(SqlServerSecurityType));
            }
        }

        public string SqlServerUserName
        {
            get => _registrySettings.SqlServerUserName;
            set
            {
                if (_registrySettings.SqlServerUserName == value)
                    return;

                _registrySettings.SqlServerUserName = value;
                OnPropertyChanged(nameof(SqlServerUserName));
            }
        }

        public string SqlServerPassword
        {
            get => _registrySettings.SqlServerPassword;
            set
            {
                if (_registrySettings.SqlServerPassword == value)
                    return;

                _registrySettings.SqlServerPassword = value;
                OnPropertyChanged(nameof(SqlServerPassword));
            }
        }

        public string SqlServerNorthwindDatabaseName
        {
            get => _registrySettings.SqlServerNorthwindDbName;
            set
            {
                if (_registrySettings.SqlServerNorthwindDbName == value)
                    return;

                _registrySettings.SqlServerNorthwindDbName = value;
                OnPropertyChanged(nameof(SqlServerNorthwindDatabaseName));
            }
        }

        public string SqlServerMegaDbDatabaseName
        {
            get => _registrySettings.SqlServerMegaDbName;
            set
            {
                if (_registrySettings.SqlServerMegaDbName == value)
                    return;

                _registrySettings.SqlServerMegaDbName = value;
                OnPropertyChanged(nameof(SqlServerMegaDbDatabaseName));
            }
        }

        public bool SqlServerSecurityControlsEnabled
        {
            get => _sqlServerSecurityControlsEnabled;
            set
            {
                if (_sqlServerSecurityControlsEnabled == value)
                    return;

                _sqlServerSecurityControlsEnabled = value;
                OnPropertyChanged(nameof(SqlServerSecurityControlsEnabled));
            }
        }

        public string MySqlServerName
        {
            get => _registrySettings.MySqlServerName;
            set
            {
                if (_registrySettings.MySqlServerName == value)
                    return;

                _registrySettings.MySqlServerName = value;
                OnPropertyChanged(nameof(MySqlServerName));
            }
        }

        public string MySqlUserName
        {
            get => _registrySettings.MySqlUserName;
            set
            {
                if (_registrySettings.MySqlUserName == value)
                    return;

                _registrySettings.MySqlUserName = value;
                OnPropertyChanged(nameof(MySqlUserName));
            }
        }

        public string MySqlPassword
        {
            get => _registrySettings.MySqlPassword;
            set
            {
                if (_registrySettings.MySqlPassword == value)
                    return;

                _registrySettings.MySqlPassword = value;
                OnPropertyChanged(nameof(MySqlPassword));
            }
        }

        public string MySqlNorthwindDatabaseName
        {
            get => _registrySettings.MySqlNorthwindDbName;
            set
            {
                if (_registrySettings.MySqlNorthwindDbName == value)
                    return;

                _registrySettings.MySqlNorthwindDbName = value;
                OnPropertyChanged(nameof(MySqlNorthwindDatabaseName));
            }
        }

        public string MySqlMegaDbDatabaseName
        {
            get => _registrySettings.MySqlMegaDbName;
            set
            {
                if (_registrySettings.MySqlMegaDbName == value)
                    return;

                _registrySettings.MySqlMegaDbName = value;
                OnPropertyChanged(nameof(MySqlMegaDbDatabaseName));
            }
        }

        public List<string> SqlServerDatabaseNames => _sqlServerDatabaseNames;

        public List<string> MySqlDatabaseNames => _mySqlDatabaseNames;

        private RegistrySettings _registrySettings = new RegistrySettings();

        private bool _northwindSqlServerRadioEnabled = true;
        private bool _northwindMySqlRadioEnabled = true;
        private bool _northwindSqliteRadioEnabled = true;
        private bool _northwindSqliteControlsEnabled = true;

        private bool _megaDbSqlServerRadioEnabled = true;
        private bool _megaDbMySqlRadioEnabled = true;

        private bool _sqlServerSecurityControlsEnabled = true;

        private List<string> _sqlServerDatabaseNames = new List<string>();
        private List<string> _mySqlDatabaseNames = new List<string>();
        private IDbSetupView _view;
        private EntityFrameworkVersions _originalEntityFrameworkVersion;

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnViewLoaded(IDbSetupView view)
        {
            _view = view;
            _originalEntityFrameworkVersion = EntityFrameworkVersion;
            SetupEntityFrameworkVersionControls();
            SetupSqlServerSecurityTypeControls();
        }

        private void SetupEntityFrameworkVersionControls()
        {
            var enable = true;
            switch (EntityFrameworkVersion)
            {
                case EntityFrameworkVersions.EntityFrameworkCore3:
                    break;
                case EntityFrameworkVersions.EntityFramework6:
                    enable = false;
                    NorthwindDbPlatform = NorthwindDbPlatforms.SqlServer;
                    MegaDbDbPlatform = MegaDbPlatforms.SqlServer;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            NorthwindSqlServerRadioEnabled = enable;
            NorthwindMySqlRadioEnabled = enable;
            NorthwindSqliteRadioEnabled = enable;
            MegaDbSqlServerRadioEnabled = enable;
            MegaDbMySqlRadioEnabled = enable;

            SetupNorthwindSqliteControls();
        }

        private void SetupNorthwindSqliteControls()
        {
            NorthwindSqliteControlsEnabled = NorthwindDbPlatform == NorthwindDbPlatforms.Sqlite;
        }

        private void SetupSqlServerSecurityTypeControls()
        {
            SqlServerSecurityControlsEnabled = SqlServerSecurityType == SecurityTypes.SqlLogin;
        }

        public void OnSqlServerDatabaseComboFocus()
        {
            FillListWithDatabaseNames(ref _sqlServerDatabaseNames, GetSqlServerDataProcessor());
        }

        public void OnMySqlDatabaseComboFocus()
        {
            FillListWithDatabaseNames(ref _mySqlDatabaseNames, GetMySqlDataProcessor());
        }

        public SqlServerDataProcessor GetSqlServerDataProcessor()
        {
            var dataProcessor = new SqlServerDataProcessor()
            {
                Server = SqlServerServerName,
                SecurityType = SqlServerSecurityType,
                Database = "master",
                UserName = SqlServerUserName,
                Password = SqlServerPassword
            };
            return dataProcessor;
        }

        public MySqlDataProcessor GetMySqlDataProcessor()
        {
            var dataProcessor = new MySqlDataProcessor()
            {
                Server = MySqlServerName,
                Database = "mysql",
                UserName = MySqlUserName,
                Password = MySqlPassword
            };
            return dataProcessor;
        }


        private void FillListWithDatabaseNames(ref List<string> list, DbDataProcessor dataProcessor)
        {
            list.Clear();
            var dbList = new List<string>();
            var result = dataProcessor.GetListOfDatabases();
            if (result.ResultCode == GetDataResultCodes.Success)
            {
                var dataTable = result.DataSet.Tables[0];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    var text = dataRow.GetRowValue(dataTable.Columns[0].ColumnName);
                    dbList.Add(text);
                }

                list = dbList.OrderBy(o => o).ToList();
            }
        }

        private RegistrySettings GetRegistrySettings()
        {
            var registrySettings = new RegistrySettings
            {
                EntityFrameworkVersion = EntityFrameworkVersion,
                SqlServerServerName = SqlServerServerName,
                SqlServerSecurityType = SqlServerSecurityType,
                SqlServerUserName = SqlServerUserName,
                SqlServerPassword = SqlServerPassword,
                SqlServerNorthwindDbName = SqlServerNorthwindDatabaseName,
                SqlServerMegaDbName = SqlServerMegaDbDatabaseName,
                MySqlServerName = MySqlServerName,
                MySqlUserName = MySqlUserName,
                MySqlPassword = MySqlPassword,
                MySqlNorthwindDbName = MySqlNorthwindDatabaseName,
                MySqlMegaDbName = MySqlMegaDbDatabaseName,
                NorthwindPlatformType = NorthwindDbPlatform,
                NorthwindSqliteFileName = NorthwindSqliteFileName,
                MegaDbPlatformType = MegaDbDbPlatform
            };

            return registrySettings;
        }

        public void ShowSqlServerNorthwindScript()
        {
            SqlServerNorthwindDatabaseName = ScrubNewDatabaseName(SqlServerNorthwindDatabaseName);
            ShowScript(GetSqlServerDataProcessor(), RsDbLookupAppGlobals.SqlServerNorthwindScript,
                RegistrySettings.SqlServerNorthwindDatabaseNameConst, SqlServerNorthwindDatabaseName, true);
        }

        public void ShowSqlServerMegaDbScript()
        {
            SqlServerMegaDbDatabaseName = ScrubNewDatabaseName(SqlServerMegaDbDatabaseName);
            ShowMegaDbScript(GetSqlServerDataProcessor(), RsDbLookupAppGlobals.SqlServerMegaDbScript,
                RegistrySettings.SqlServerMegaDbDatabaseNameConst, SqlServerMegaDbDatabaseName, true,
                MegaDbPlatforms.SqlServer);
        }

        public void ShowMySqlNorthwindScript()
        {
            MySqlNorthwindDatabaseName = MySqlNorthwindDatabaseName.ToLower();
            MySqlNorthwindDatabaseName = ScrubNewDatabaseName(MySqlNorthwindDatabaseName);
            
            ShowScript(GetMySqlDataProcessor(), RsDbLookupAppGlobals.MySqlNorthwindScript,
                RegistrySettings.MySqlNorthwindDatabaseNameConst, MySqlNorthwindDatabaseName);
        }

        public void ShowMySqlMegaDbScript()
        {
            MySqlMegaDbDatabaseName = MySqlMegaDbDatabaseName.ToLower();
            MySqlMegaDbDatabaseName = ScrubNewDatabaseName(MySqlMegaDbDatabaseName);
            ShowMegaDbScript(GetMySqlDataProcessor(), RsDbLookupAppGlobals.MySqlMegaDbScript,
                RegistrySettings.MySqlMegaDbDatabaseNameConst, MySqlMegaDbDatabaseName, false,
                MegaDbPlatforms.MySql);
        }

        private string ScrubNewDatabaseName(string newDatabaseName)
        {
            var result = newDatabaseName;
            result = result.Replace(' ', '_');
            result = result.Replace('.', '_');

            return result;
        }

        private void ShowMegaDbScript(DbDataProcessor dataProcessor, string scriptFileName, string defaultDbName,
            string dbName, bool splitGo, MegaDbPlatforms platform)
        {
            if (ShowScript(dataProcessor, scriptFileName, defaultDbName, dbName, splitGo, false))
            {
                MegaDbSeedDatabaseButton_Click(platform);
            }
        }

        private bool ShowScript(DbDataProcessor dataProcessor, string scriptFileName, string defaultDbName,
            string dbName, bool splitGo = false, bool showExecSuccessMessage = true)
        {
            var sql = RsDbLookupAppGlobals.OpenTextFile(scriptFileName);
            return _view.ShowScriptDialog(dataProcessor, scriptFileName, sql, splitGo, defaultDbName, dbName,
                showExecSuccessMessage);
        }

        public void GetNorthwindFileName()
        {
            var initialDirectory = Path.GetDirectoryName(NorthwindSqliteFileName);
            var fileName = Path.GetFileName(NorthwindSqliteFileName);
            var defaultExt = ".sqlite";
            var filter = @"Sqlite (*.sqlite)| *.sqlite";

            fileName = _view.ShowOpenFileDialog(initialDirectory, fileName, defaultExt, filter);
            if (!fileName.IsNullOrEmpty())
                NorthwindSqliteFileName = fileName;
        }

        public bool ValidateMegaDbConnection(MegaDbPlatforms platform, bool showMessageBox = true)
        {
            var registrySettings = GetRegistrySettings();
            registrySettings.MegaDbPlatformType = platform;
            return ValidateConnection(RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext, registrySettings, showMessageBox);
        }

        public bool ValidateNorthwindConnection(NorthwindDbPlatforms platform, bool showMessageBox = true)
        {
            var registrySettings = GetRegistrySettings();
            registrySettings.NorthwindPlatformType = platform;
            return ValidateConnection(RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext, registrySettings, showMessageBox);
        }

        private bool ValidateConnection(IAppLookupContext lookupContext, RegistrySettings registrySettings, bool showMessageBox = true)
        {
            var result = lookupContext.LookupContextConfiguration.TestConnection(registrySettings);
            if (result && showMessageBox)
                _view.ShowInformationMessage("Connection Successful!", "Test Connection");
            else if (showMessageBox)
                _view.ShowCriticalMessage("Connection Failed!", "Test Connection");

            return result;
        }

        public void OkButton_Click()
        {
            if (!ValidateDatabaseSettings())
                return;

            SaveRegistry();

            if (EntityFrameworkVersion != _originalEntityFrameworkVersion)
            {
                var message = "Changing the Entity Framework version will not occur until you restart this application.\r\n\r\n";
                message += "Do you wish to close this application now?";
                if (_view.ShowYesNoMessage(message, @"Entity Framework Version Change"))
                {
                    _view.ExitApplication();
                }
            }
            _view.CloseWindow();
        }

        private void SaveRegistry()
        {
            _registrySettings.SaveToRegistry();
            AppLookupContextConfiguration.RegistrySettings.LoadFromRegistry();
            RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.LookupContextConfiguration.Reinitialize();
            RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext.LookupContextConfiguration.Reinitialize();
        }

        private bool ValidateDatabaseSettings()
        {
            if (!ValidateNorthwindSettings())
                return false;

            if (!ValidateMegaDbSettings())
                return false;

            return true;
        }

        private bool ValidateNorthwindSettings()
        {
            var validateResult = ValidateNorthwindConnection(NorthwindDbPlatform, false);

            if (!validateResult)
            {
                _view.ValidationFailSetFocus_Northwind(NorthwindDbPlatform);
                var message = "Northwind validation failed.  Please correct the values before continuing.";
                _view.ShowValidationMessage(message, "Validation Failure!");
                return false;
            }

            return true;
        }

        private bool ValidateMegaDbSettings()
        {
            return ValidateMegaDbSettings(MegaDbDbPlatform);
        }

        private bool ValidateMegaDbSettings(MegaDbPlatforms platform)
        {
            var validateResult = ValidateMegaDbConnection(platform, false);

            if (!validateResult)
            {
                _view.ValidationFailSetFocus_MegaDb(platform);
                var message = "Mega Database validation failed.  Please correct the values before continuing.";
                _view.ShowValidationMessage(message, "Validation Failure!");
                return false;
            }

            return true;
        }

        public void MegaDbSeedDatabaseButton_Click()
        {
            MegaDbSeedDatabaseButton_Click(MegaDbDbPlatform);
        }

        public void MegaDbSeedDatabaseButton_Click(MegaDbPlatforms platform)
        {
            if (!ValidateMegaDbSettings(platform))
            {
                return;
            }

            SaveRegistry();
            _view.ShowInformationMessage("Database Settings Saved.", "Database Setup");

            var processorType = platform == MegaDbPlatforms.SqlServer
                ? DataProcessorTypes.SqlServer
                : DataProcessorTypes.MySql;

            var origProcessorType = RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext.LookupContextConfiguration
                .DataProcessorType;
            RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext.LookupContextConfiguration.DataProcessorType =
                processorType;

            if (RsDbLookupAppGlobals.EfProcessor.MegaDbEfDataProcessor.DoesItemsTableHaveData())
            {
                var message = "The Items table already has data.  You must restart this application and then "
                              + "immediately recreate the Mega Database before seeding the Items table.\r\n\r\n"
                              + "Do you wish to close this application now?";
                if (_view.ShowYesNoMessage(message, "Items Table Seed Process"))
                {
                    _view.ExitApplication();
                }

                RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext.LookupContextConfiguration.DataProcessorType =
                    origProcessorType;
                return;
            }

            switch (EntityFrameworkVersion)
            {
                case EntityFrameworkVersions.EntityFrameworkCore3:
                    break;
                case EntityFrameworkVersions.EntityFramework6:
                    var message =
                        "For optimal performance, it is recommended that you switch to the Entity Framework Core version "
                        + "and restart this application before seeding the Items table.\r\n\r\nDo you wish to do so now?";
                    if (_view.ShowYesNoMessage(message, @"Items Table Seed Process"))
                    {
                        EntityFrameworkVersion = EntityFrameworkVersions.EntityFrameworkCore3;
                        SaveRegistry();
                        _view.ExitApplication();
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            _view.ShowMegaDbItemsTableSeederForm();
            RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext.LookupContextConfiguration.DataProcessorType =
                origProcessorType;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
