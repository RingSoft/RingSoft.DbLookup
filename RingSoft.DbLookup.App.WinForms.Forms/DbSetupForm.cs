using System;
using System.Collections.Generic;
using System.Windows.Forms;
using RingSoft.DbLookup.App.Library;
using RingSoft.DbLookup.App.Library.ViewModels;
using RingSoft.DbLookup.Controls.WinForms;
using RingSoft.DbLookup.DataProcessor;

namespace RingSoft.DbLookup.App.WinForms.Forms
{
    public partial class DbSetupForm : BaseForm, IDbSetupView
    {
        private DbSetupViewModel _viewModel = new DbSetupViewModel();

        public DbSetupForm()
        {
            InitializeComponent();

            EntityFrameworkCoreRadioButton.AddCheckedBinding(_viewModel, nameof(_viewModel.EntityFrameworkVersion),
                EntityFrameworkVersions.EntityFrameworkCore3);
            EntityFramework6RadioButton.AddCheckedBinding(_viewModel, nameof(_viewModel.EntityFrameworkVersion),
                EntityFrameworkVersions.EntityFramework6);

            NorthwindSqlServerRadioButton.AddCheckedBinding(_viewModel, nameof(_viewModel.NorthwindDbPlatform),
                NorthwindDbPlatforms.SqlServer);
            NorthwindSqlServerRadioButton.DataBindings.Add(nameof(NorthwindSqlServerRadioButton.Enabled), _viewModel,
                nameof(_viewModel.NorthwindSqlServerRadioEnabled), false, DataSourceUpdateMode.OnPropertyChanged);

            NorthwindMySqlRadioButton.AddCheckedBinding(_viewModel, nameof(_viewModel.NorthwindDbPlatform),
                NorthwindDbPlatforms.MySql);
            NorthwindMySqlRadioButton.DataBindings.Add(nameof(NorthwindMySqlRadioButton.Enabled), _viewModel,
                nameof(_viewModel.NorthwindMySqlRadioEnabled), false, DataSourceUpdateMode.OnPropertyChanged);

            NorthwindSqliteRadioButton.AddCheckedBinding(_viewModel, nameof(_viewModel.NorthwindDbPlatform),
                NorthwindDbPlatforms.Sqlite);
            NorthwindSqliteRadioButton.DataBindings.Add(nameof(NorthwindSqliteRadioButton.Enabled), _viewModel,
                nameof(_viewModel.NorthwindSqliteRadioEnabled), false, DataSourceUpdateMode.OnPropertyChanged);

            NorthwindSqliteFileNameTextBox.DataBindings.Add(nameof(NorthwindSqliteFileNameTextBox.Text), _viewModel,
                nameof(_viewModel.NorthwindSqliteFileName), false, DataSourceUpdateMode.OnPropertyChanged);
            NorthwindSqliteFileNameTextBox.DataBindings.Add(nameof(NorthwindSqliteFileNameTextBox.Enabled), _viewModel,
                nameof(_viewModel.NorthwindSqliteControlsEnabled), false, DataSourceUpdateMode.OnPropertyChanged);

            NorthwindSqliteFileNameLabel.DataBindings.Add(nameof(NorthwindSqliteFileNameLabel.Enabled), _viewModel,
                nameof(_viewModel.NorthwindSqliteControlsEnabled), false, DataSourceUpdateMode.OnPropertyChanged);
            NorthwindSqliteFileNameButton.DataBindings.Add(nameof(NorthwindSqliteFileNameButton.Enabled), _viewModel,
                nameof(_viewModel.NorthwindSqliteControlsEnabled), false, DataSourceUpdateMode.OnPropertyChanged);
            SqliteNorthwindTestConButton.DataBindings.Add(nameof(SqliteNorthwindTestConButton.Enabled), _viewModel,
                nameof(_viewModel.NorthwindSqliteControlsEnabled), false, DataSourceUpdateMode.OnPropertyChanged);

            MegaDbSqlServerRadioButton.AddCheckedBinding(_viewModel, nameof(_viewModel.MegaDbDbPlatform), MegaDbPlatforms.SqlServer);
            MegaDbSqlServerRadioButton.DataBindings.Add(nameof(MegaDbSqlServerRadioButton.Enabled), _viewModel,
                nameof(_viewModel.MegaDbSqlServerRadioEnabled), false, DataSourceUpdateMode.OnPropertyChanged);

            MegaDbMySqlRadioButton.AddCheckedBinding(_viewModel, nameof(_viewModel.MegaDbDbPlatform), MegaDbPlatforms.MySql);
            MegaDbMySqlRadioButton.DataBindings.Add(nameof(MegaDbMySqlRadioButton.Enabled), _viewModel,
                nameof(_viewModel.MegaDbMySqlRadioEnabled), false, DataSourceUpdateMode.OnPropertyChanged);

            SqlServerServerTextBox.DataBindings.Add(nameof(SqlServerServerTextBox.Text), _viewModel,
                nameof(_viewModel.SqlServerServerName), false, DataSourceUpdateMode.OnPropertyChanged);

            WindowsAuthRadioButton.AddCheckedBinding(_viewModel, nameof(_viewModel.SqlServerSecurityType),
                SecurityTypes.WindowsAuthentication);
            SqlLoginRadioButton.AddCheckedBinding(_viewModel, nameof(_viewModel.SqlServerSecurityType),
                SecurityTypes.SqlLogin);

            SqlServerUserNameTextBox.DataBindings.Add(nameof(SqlServerUserNameTextBox.Text), _viewModel,
                nameof(_viewModel.SqlServerUserName), false, DataSourceUpdateMode.OnPropertyChanged);
            SqlServerUserNameTextBox.DataBindings.Add(nameof(SqlServerUserNameTextBox.Enabled), _viewModel,
                nameof(_viewModel.SqlServerSecurityControlsEnabled), false, DataSourceUpdateMode.OnPropertyChanged);
            SqlServerUserNameLabel.DataBindings.Add(nameof(SqlServerUserNameLabel.Enabled), _viewModel,
                nameof(_viewModel.SqlServerSecurityControlsEnabled), false, DataSourceUpdateMode.OnPropertyChanged);

            SqlServerPasswordTextBox.DataBindings.Add(nameof(SqlServerPasswordTextBox.Text), _viewModel,
                nameof(_viewModel.SqlServerPassword), false, DataSourceUpdateMode.OnPropertyChanged);
            SqlServerPasswordTextBox.DataBindings.Add(nameof(SqlServerPasswordTextBox.Enabled), _viewModel,
                nameof(_viewModel.SqlServerSecurityControlsEnabled), false, DataSourceUpdateMode.OnPropertyChanged);
            SqlServerPasswordLabel.DataBindings.Add(nameof(SqlServerPasswordLabel.Enabled), _viewModel,
                nameof(_viewModel.SqlServerSecurityControlsEnabled), false, DataSourceUpdateMode.OnPropertyChanged);

            SqlServerNorthwindComboBox.DataBindings.Add(nameof(SqlServerNorthwindComboBox.Text), _viewModel,
                nameof(_viewModel.SqlServerNorthwindDatabaseName), false, DataSourceUpdateMode.OnPropertyChanged);

            SqlServerMegaDbComboBox.DataBindings.Add(nameof(SqlServerMegaDbComboBox.Text), _viewModel,
                nameof(_viewModel.SqlServerMegaDbDatabaseName), false, DataSourceUpdateMode.OnPropertyChanged);

            MySqlServerTextBox.DataBindings.Add(nameof(MySqlServerTextBox.Text), _viewModel,
                nameof(_viewModel.MySqlServerName), false, DataSourceUpdateMode.OnPropertyChanged);
            MySqlUserNameTextBox.DataBindings.Add(nameof(MySqlUserNameTextBox.Text), _viewModel,
                nameof(_viewModel.MySqlUserName), false, DataSourceUpdateMode.OnPropertyChanged);
            MySqlPasswordTextBox.DataBindings.Add(nameof(MySqlPasswordTextBox.Text), _viewModel,
                nameof(_viewModel.MySqlPassword), false, DataSourceUpdateMode.OnPropertyChanged);
            MySqlNorthwindComboBox.DataBindings.Add(nameof(MySqlNorthwindComboBox.Text), _viewModel,
                nameof(_viewModel.MySqlNorthwindDatabaseName), false, DataSourceUpdateMode.OnPropertyChanged);
            MySqlMegaDbComboBox.DataBindings.Add(nameof(MySqlMegaDbComboBox.Text), _viewModel,
                nameof(_viewModel.MySqlMegaDbDatabaseName), false, DataSourceUpdateMode.OnPropertyChanged);

            SqlServerNorthwindCreateButton.Click +=
                (sender, args) => _viewModel.ShowSqlServerNorthwindScript();
            SqlServerMegaDbCreateButton.Click +=
                (sender, args) => _viewModel.ShowSqlServerMegaDbScript();

            MySqlNorthwindCreateButton.Click += (sender, args) =>
                _viewModel.ShowMySqlNorthwindScript();
            MySqlMegaDbCreateButton.Click += (sender, args) =>
                _viewModel.ShowMySqlMegaDbScript();

            NorthwindSqliteFileNameButton.Click += (sender, args) => _viewModel.GetNorthwindFileName();

            SqlServerNorthwindComboBox.Enter += (sender, args) =>
            {
                _viewModel.OnSqlServerDatabaseComboFocus();
                FillDatabaseCombo(SqlServerNorthwindComboBox, _viewModel.SqlServerDatabaseNames);
            };

            SqlServerMegaDbComboBox.Enter += (sender, args) =>
            {
                _viewModel.OnSqlServerDatabaseComboFocus();
                FillDatabaseCombo(SqlServerMegaDbComboBox, _viewModel.SqlServerDatabaseNames);
            };

            MySqlNorthwindComboBox.Enter += (sender, args) =>
            {
                _viewModel.OnMySqlDatabaseComboFocus();
                FillDatabaseCombo(MySqlNorthwindComboBox, _viewModel.MySqlDatabaseNames);
            };

            MySqlMegaDbComboBox.Enter += (sender, args) =>
            {
                _viewModel.OnMySqlDatabaseComboFocus();
                FillDatabaseCombo(MySqlMegaDbComboBox, _viewModel.MySqlDatabaseNames);
            };

            SqlServerNorthwindTestConButton.Click +=
                (sender, args) => _viewModel.ValidateNorthwindConnection(NorthwindDbPlatforms.SqlServer);
            MySqlNorthwindTestConButton.Click +=
                (sender, args) => _viewModel.ValidateNorthwindConnection(NorthwindDbPlatforms.MySql);
            SqliteNorthwindTestConButton.Click +=
                (sender, args) => _viewModel.ValidateNorthwindConnection(NorthwindDbPlatforms.Sqlite);

            SqlServerMegaDbTestConButton.Click += (sender, args) => _viewModel.ValidateMegaDbConnection(MegaDbPlatforms.SqlServer);
            MySqlMegaDbTestConButton.Click += (sender, args) => _viewModel.ValidateMegaDbConnection(MegaDbPlatforms.MySql);

            OkButton.Click += (sender, args) => _viewModel.OkButton_Click();
            CancelFormButton.Click += CancelButton_Click;
        }

        private void FillDatabaseCombo(ComboBox comboBox, List<string> list)
        {
            comboBox.Items.Clear();
            var itemIndex = 0;
            foreach (var databaseName in list)
            {
                comboBox.Items.Add(databaseName);
                if (databaseName == comboBox.Text)
                    comboBox.SelectedIndex = itemIndex;

                itemIndex++;
            }
        }

        public bool ShowScriptDialog(DbDataProcessor dataProcessor, string scriptFileName, string sql, bool splitGo,
            string defaultDbName, string dbName, bool showExecSuccessMessage)
        {
            var sqlScriptForm = new SqlScriptForm(dataProcessor, scriptFileName, sql, splitGo, defaultDbName, dbName,
                showExecSuccessMessage);

            return sqlScriptForm.ShowDialog();
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

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                return dialog.FileName;
            }

            return string.Empty;

        }

        public void ShowInformationMessage(string message, string title)
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void ShowValidationMessage(string message, string title)
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        public void ShowCriticalMessage(string message, string title)
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public bool ShowYesNoMessage(string message, string title)
        {
            if (MessageBox.Show(message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                return true;

            return false;
        }

        public void ValidationFailSetFocus_Northwind(NorthwindDbPlatforms platform)
        {
            switch (platform)
            {
                case NorthwindDbPlatforms.SqlServer:
                    TabControl.SelectedTab = SqlServerPage;
                    SqlServerNorthwindComboBox.Focus();
                    break;
                case NorthwindDbPlatforms.MySql:
                    TabControl.SelectedTab = MySqlPage;
                    MySqlNorthwindComboBox.Focus();
                    break;
                case NorthwindDbPlatforms.Sqlite:
                    TabControl.SelectedTab = GeneralPage;
                    NorthwindSqliteFileNameTextBox.Focus();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }

        public void ValidationFailSetFocus_MegaDb(MegaDbPlatforms platform)
        {
            switch (platform)
            {
                case MegaDbPlatforms.SqlServer:
                    TabControl.SelectedTab = SqlServerPage;
                    SqlServerMegaDbComboBox.Focus();
                    break;
                case MegaDbPlatforms.MySql:
                    TabControl.SelectedTab = MySqlPage;
                    MySqlMegaDbComboBox.Focus();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void ExitApplication()
        {
            Application.Exit();
        }

        public void CloseWindow()
        {
            Close();
        }

        public void ShowMegaDbItemsTableSeederForm()
        {
            var seederForm = new MegaDbSeedForm();
            seederForm.ShowDialog();
        }

        protected override void OnLoad(EventArgs e)
        {
            _viewModel.OnViewLoaded(this);
            base.OnLoad(e);
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
