using System.Windows;
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

            Loaded += (sender, args) => DbSetupViewModel.OnViewLoaded(this);
            OkButton.Click += (sender, args) => DbSetupViewModel.OkButton_Click();
            CancelButton.Click += (sender, args) => CloseWindow();

            NorthwindSqliteFileNameButton.Click += (sender, args) => DbSetupViewModel.GetNorthwindFileName();
            SqliteNorthwindTestConButton.Click +=
                (sender, args) => DbSetupViewModel.ValidateNorthwindConnection(NorthwindDbPlatforms.Sqlite);
        }

        public void ShowScriptDialog(DbDataProcessor dataProcessor, string scriptFileName, string sql, bool splitGo,
            string defaultDbName, string dbName)
        {
            throw new System.NotImplementedException();
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
            throw new System.NotImplementedException();
        }

        public void ValidationFailSetFocus_MegaDb(MegaDbPlatforms platform)
        {
            throw new System.NotImplementedException();
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
            throw new System.NotImplementedException();
        }
    }
}
