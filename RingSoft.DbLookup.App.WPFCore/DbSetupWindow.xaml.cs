using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using RingSoft.DbLookup.App.Library;
using RingSoft.DbLookup.App.Library.ViewModels;
using RingSoft.DbLookup.GetDataProcessor;

namespace RingSoft.DbLookup.App.WPFCore
{
    /// <summary>
    /// Interaction logic for DbSetupWindow.xaml
    /// </summary>
    public partial class DbSetupWindow : IDbSetupView
    {
        public DbSetupWindow()
        {
            InitializeComponent();

            Loaded += (sender, args) =>
            {
                DbSetupViewModel.OnViewLoaded(this);
                SqlServerPasswordBox.Password = DbSetupViewModel.SqlServerPassword;
                MySqlPasswordBox.Password = DbSetupViewModel.MySqlPassword;
            };
            OkButton.Click += (sender, args) =>
            {
                SavePasswords();
                DbSetupViewModel.OkButton_Click();
            };

            CancelButton.Click += (sender, args) => CloseWindow();

            NorthwindSqliteFileNameButton.Click += (sender, args) => DbSetupViewModel.GetNorthwindFileName();
            SqliteNorthwindTestConButton.Click +=
                (sender, args) => DbSetupViewModel.ValidateNorthwindConnection(NorthwindDbPlatforms.Sqlite);

            MegaDbSeedItemsTableButton.Click += (sender, args) =>
            {
                SavePasswords();
                DbSetupViewModel.MegaDbSeedDatabaseButton_Click();
            };

            SqlServerNorthwindComboBox.GotFocus += (sender, args) =>
            {
                SavePasswords();
                DbSetupViewModel.OnSqlServerDatabaseComboFocus();
                FillDatabaseCombo(SqlServerNorthwindComboBox, DbSetupViewModel.SqlServerDatabaseNames);
            };

            SqlServerNorthwindTestConnectionButton.Click += (sender, args) =>
            {
                SavePasswords();
                DbSetupViewModel.ValidateNorthwindConnection(NorthwindDbPlatforms.SqlServer);
            };

            SqlServerNorthwindCreateButton.Click += (sender, args) =>
            {
                SavePasswords();
                DbSetupViewModel.ShowSqlServerNorthwindScript();
            };

            SqlServerMegaDbComboBox.GotFocus += (sender, args) =>
            {
                SavePasswords();
                DbSetupViewModel.OnSqlServerDatabaseComboFocus();
                FillDatabaseCombo(SqlServerMegaDbComboBox, DbSetupViewModel.SqlServerDatabaseNames);
            };

            SqlServerMegaDbTestConnectionButton.Click += (sender, args) =>
            {
                SavePasswords();
                DbSetupViewModel.ValidateMegaDbConnection(MegaDbPlatforms.SqlServer);
            };

            SqlServerMegaDbCreateButton.Click += (sender, args) =>
            {
                SavePasswords();
                DbSetupViewModel.ShowSqlServerMegaDbScript();
            };

            MySqlNorthwindComboBox.GotFocus += (sender, args) =>
            {
                SavePasswords();
                DbSetupViewModel.OnMySqlDatabaseComboFocus();
                FillDatabaseCombo(MySqlNorthwindComboBox, DbSetupViewModel.MySqlDatabaseNames);
            };

            MySqlNorthwindTestConnectionButton.Click += (sender, args) =>
            {
                SavePasswords();
                DbSetupViewModel.ValidateNorthwindConnection(NorthwindDbPlatforms.MySql);
            };

            MySqlNorthwindCreateButton.Click += (sender, args) =>
            {
                SavePasswords();
                DbSetupViewModel.ShowMySqlNorthwindScript();
            };

            MySqlMegaDbComboBox.GotFocus += (sender, args) =>
            {
                SavePasswords();
                DbSetupViewModel.OnMySqlDatabaseComboFocus();
                FillDatabaseCombo(MySqlMegaDbComboBox, DbSetupViewModel.MySqlDatabaseNames);
            };

            MySqlMegaDbTestConnectionButton.Click += (sender, args) =>
            {
                SavePasswords();
                DbSetupViewModel.ValidateMegaDbConnection(MegaDbPlatforms.MySql);
            };

            MySqlMegaDbCreateButton.Click += (sender, args) =>
            {
                SavePasswords();
                DbSetupViewModel.ShowMySqlMegaDbScript();
            };
        }

        private void SavePasswords()
        {
            DbSetupViewModel.SqlServerPassword = SqlServerPasswordBox.Password;
            DbSetupViewModel.MySqlPassword = MySqlPasswordBox.Password;
        }

        private void FillDatabaseCombo(ComboBox comboBox, List<string> list)
        {
            var text = comboBox.Text;
            comboBox.Items.Clear();
            comboBox.Text = text;
            var itemIndex = 0;
            foreach (var databaseName in list)
            {
                comboBox.Items.Add(databaseName);
                if (databaseName == comboBox.Text)
                    comboBox.SelectedIndex = itemIndex;

                itemIndex++;
            }
        }

        public void ShowScriptDialog(DbDataProcessor dataProcessor, string scriptFileName, string sql, bool splitGo,
            string defaultDbName, string dbName)
        {
            var sqlScriptWindow = new SqlScriptWindow(dataProcessor, scriptFileName, sql, splitGo, defaultDbName, dbName);
            sqlScriptWindow.Owner = this;
            sqlScriptWindow.ShowDialog();
        }

        public string ShowOpenFileDialog(string initialDirectory, string fileName, string defaultExt, string filter)
        {
            var dialog = new OpenFileDialog
            {
                CheckFileExists = true,
                CheckPathExists = true,
                InitialDirectory = initialDirectory,
                FileName = fileName,
                DefaultExt = defaultExt,
                Filter = filter
            };

            if (dialog.ShowDialog() == true)
            {
                return dialog.FileName;
            }

            return string.Empty;
        }

        public void ShowInformationMessage(string message, string title)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void ShowValidationMessage(string message, string title)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        public void ShowCriticalMessage(string message, string title)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public bool ShowYesNoMessage(string message, string title)
        {
            if (MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Question) ==
                MessageBoxResult.Yes)
                return true;

            return false;
        }

        public void ValidationFailSetFocus_Northwind(NorthwindDbPlatforms platform)
        {
            switch (platform)
            {
                case NorthwindDbPlatforms.SqlServer:
                    TabControl.SelectedIndex = 1;
                    SqlServerNorthwindComboBox.Focus();
                    break;
                case NorthwindDbPlatforms.MySql:
                    TabControl.SelectedIndex = 2;
                    MySqlNorthwindComboBox.Focus();
                    break;
                case NorthwindDbPlatforms.Sqlite:
                    TabControl.SelectedIndex = 0;
                    NorthwindSqliteFileNameTextBox.Focus();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(platform), platform, null);
            }
        }

        public void ValidationFailSetFocus_MegaDb(MegaDbPlatforms platform)
        {
            switch (platform)
            {
                case MegaDbPlatforms.SqlServer:
                    TabControl.SelectedIndex = 1;
                    SqlServerMegaDbComboBox.Focus();
                    break;
                case MegaDbPlatforms.MySql:
                    TabControl.SelectedIndex = 2;
                    MySqlMegaDbComboBox.Focus();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(platform), platform, null);
            }
        }

        public void ExitApplication()
        {
            Application.Current.Shutdown();
        }

        public void CloseWindow()
        {
            Close();
        }

        public void ShowMegaDbItemsTableSeederForm(MegaDbPlatforms megaDbPlatform)
        {
            var seederWindow = new MegaDbSeedWindow(megaDbPlatform);
            seederWindow.Owner = this;
            seederWindow.ShowDialog();
        }
    }
}
