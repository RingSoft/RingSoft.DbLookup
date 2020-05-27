using RingSoft.DbLookup.App.Library.ViewModels;
using RingSoft.DbLookup.Controls.WinForms;
using RingSoft.DbLookup.DataProcessor;
using System;
using System.Windows.Forms;

namespace RingSoft.DbLookup.App.WinForms.Forms
{
    public partial class SqlScriptForm : BaseForm, ISqlScriptView
    {
        private string _fileName;
        private DbDataProcessor _dataProcessor;
        private string _sqlText;
        private bool _splitGo;
        private string _defaultDbName;
        private string _dbName;
        private bool _executeResult;

        private SqlScriptViewModel _viewModel = new SqlScriptViewModel();

        public SqlScriptForm(DbDataProcessor dataProcessor, string fileName, string sqlText, bool splitGo,
            string defaultDbName, string dbName, bool showExecSuccessMessage)
        {
            _dataProcessor = dataProcessor;
            _fileName = fileName;
            _splitGo = splitGo;
            _sqlText = sqlText;
            _defaultDbName = defaultDbName;
            _dbName = dbName;

            InitializeComponent();

            SqlScriptTextBox.DataBindings.Add(nameof(SqlScriptTextBox.Text), _viewModel, nameof(_viewModel.SqlText),
                false, DataSourceUpdateMode.OnPropertyChanged);

            EditMenu.DropDownOpening += (sender, args) => CopyMenu.Enabled = SqlScriptTextBox.SelectionLength > 0;
            CopyMenu.Click += (sender, args) => Clipboard.SetText(SqlScriptTextBox.SelectedText);
            SaveAsMenu.Click += (sender, args) => _viewModel.SaveAs();
            CloseButton.Click += CloseButton_Click;
            ExitMenu.Click += (sender, args) => CloseButton.PerformClick();
            ExecuteButton.Click += (sender, args) => _viewModel.ExecuteButton_Click(showExecSuccessMessage);
        }

        public new bool ShowDialog()
        {
            base.ShowDialog();
            return _executeResult;
        }

        protected override void OnLoad(EventArgs e)
        {
            _viewModel.OnViewLoaded(this, _dataProcessor, _fileName, _sqlText, _splitGo, _defaultDbName, _dbName);
            base.OnLoad(e);
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            CloseWindow(false);
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

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                return saveFileDialog.FileName;
            }

            return string.Empty;
        }

        public void CloseWindow(bool result)
        {
            _executeResult = result;
            Close();
        }
    }
}
