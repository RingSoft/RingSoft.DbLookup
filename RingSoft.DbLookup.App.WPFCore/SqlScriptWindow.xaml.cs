using System.Windows;
using Microsoft.Win32;
using RingSoft.DbLookup.App.Library.ViewModels;
using RingSoft.DbLookup.DataProcessor;

namespace RingSoft.DbLookup.App.WPFCore
{
    /// <summary>
    /// Interaction logic for SqlScriptWindow.xaml
    /// </summary>
    public partial class SqlScriptWindow : ISqlScriptView
    {
        public SqlScriptWindow(DbDataProcessor dataProcessor, string fileName, string sqlText, bool splitGo,
            string defaultDbName, string dbName)
        {
            InitializeComponent();

            Loaded += (sender, args) =>
            {
                SqlScriptViewModel.OnViewLoaded(this, dataProcessor, fileName, sqlText, splitGo, defaultDbName,
                    dbName);
                ExecuteButton.Focus();
            };

            EditMenu.SubmenuOpened += (sender, args) => CopyMenu.IsEnabled = SqlScriptTextBox.SelectionLength > 0;
            CopyMenu.Click += (sender, args) => Clipboard.SetText(SqlScriptTextBox.SelectedText);
            SaveAsMenu.Click += (sender, args) => SqlScriptViewModel.SaveAs();
            CloseButton.Click += (sender, args) => CloseWindow();
            ExitMenu.Click += (sender, args) => CloseWindow();
            ExecuteButton.Click += (sender, args) => SqlScriptViewModel.ExecuteButton_Click();
        }

        public string ShowSaveFileDialog(string initialDirectory, string fileName, string defaultExt, string filter)
        {
            var saveFileDialog = new SaveFileDialog
            {
                FileName = fileName,
                InitialDirectory = initialDirectory,
                DefaultExt = defaultExt,
                Filter = filter
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                return saveFileDialog.FileName;
            }

            return string.Empty;
        }

        public void CloseWindow()
        {
            Close();
        }
    }
}
