﻿using System;
using System.Windows.Forms;
using RingSoft.DbLookup.App.Library.ViewModels;
using RingSoft.DbLookup.GetDataProcessor;

namespace RingSoft.DbLookup.App.WinForms
{
    public partial class SqlScriptForm : BaseForm, ISqlScriptView
    {
        private string _fileName;
        private DbDataProcessor _dataProcessor;
        private string _sqlText;
        private bool _splitGo;
        private string _defaultDbName;
        private string _dbName;

        private SqlScriptViewModel _viewModel = new SqlScriptViewModel();

        public SqlScriptForm(DbDataProcessor dataProcessor, string fileName, string sqlText, bool splitGo,
            string defaultDbName, string dbName)
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

            EditMenu.DropDownOpening += EditMenu_DropDownOpening;
            CopyMenu.Click += CopyMenu_Click;
            SaveAsMenu.Click += (sender, args) => _viewModel.SaveAs();
            CloseButton.Click += CloseButton_Click;
            ExitMenu.Click += (sender, args) => CloseButton.PerformClick();
            ExecuteButton.Click += (sender, args) => _viewModel.ExecuteButton_Click();
        }

        protected override void OnLoad(EventArgs e)
        {
            _viewModel.OnViewLoaded(this, _dataProcessor, _fileName, _sqlText, _splitGo, _defaultDbName, _dbName);
            base.OnLoad(e);
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Close();
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

        public void CloseWindow()
        {
            Close();
        }

        private void CopyMenu_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(SqlScriptTextBox.SelectedText);
        }

        private void EditMenu_DropDownOpening(object sender, EventArgs e)
        {
            CopyMenu.Enabled = SqlScriptTextBox.SelectionLength > 0;
        }
    }
}