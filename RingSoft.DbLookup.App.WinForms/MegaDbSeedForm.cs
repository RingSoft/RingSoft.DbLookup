using System;
using System.ComponentModel;
using System.Windows.Forms;
using RingSoft.DbLookup.App.Library;
using RingSoft.DbLookup.App.Library.ViewModels;

namespace RingSoft.DbLookup.App.WinForms
{
    public partial class MegaDbSeedForm : BaseForm, IMegaDbSeedView
    {
        private MegaDbPlatforms _platformType;
        private MegaDbSeedViewModel _viewModel = new MegaDbSeedViewModel();

        public MegaDbSeedForm(MegaDbPlatforms platformType)
        {
            _platformType = platformType;

            InitializeComponent();

            MaxRecordsTextBox.BindTextBoxToIntFormat(_viewModel, nameof(_viewModel.MaxRecords));
            StartProcessButton.DataBindings.Add(nameof(StartProcessButton.Enabled), _viewModel,
                nameof(_viewModel.StartButtonEnabled), false, DataSourceUpdateMode.OnPropertyChanged);
            StartProcessButton.DataBindings.Add(nameof(StartProcessButton.Text), _viewModel,
                nameof(_viewModel.StartButtonText), false, DataSourceUpdateMode.OnPropertyChanged);

            ProgressLabel.DataBindings.Add(nameof(ProgressLabel.Text), _viewModel, nameof(_viewModel.ProgressLabelText),
                false, DataSourceUpdateMode.OnPropertyChanged);
            ProgressBar.DataBindings.Add(nameof(ProgressBar.Maximum), _viewModel, nameof(_viewModel.MaxRecords), false,
                DataSourceUpdateMode.OnPropertyChanged);

            CloseButton.DataBindings.Add(nameof(CloseButton.Enabled), _viewModel, nameof(_viewModel.CloseButtonEnabled),
                false, DataSourceUpdateMode.OnPropertyChanged);

            StartProcessButton.Click += (sender, args) => _viewModel.StartProcess();
            CloseButton.Click += (sender, args) => CloseWindow();

            ProgressBar.Minimum = 0;
        }

        protected override void OnLoad(EventArgs e)
        {
            _viewModel.OnViewLoaded(this, _platformType);
            base.OnLoad(e);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (_viewModel.Processing)
                e.Cancel = true;

            base.OnClosing(e);
        }

        public void ShowInformationMessage(string message, string title)
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void ShowValidationMessage(string message, string title)
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        public void ItemsTableSeederProgress(Library.MegaDb.ItemsTableSeederProgressArgs e)
        {
            ProgressBar.Invoke((Action)(() =>
                ProgressBar.Value = e.CurrentRecord));
            ProgressLabel.Invoke((Action)(() =>
                ProgressLabel.Text = e.Message));
            StartProcessButton.Invoke((Action)(() =>
                StartProcessButton.Enabled = e.AllowCancel));
        }

        public void CloseWindow()
        {
            Close();
        }
    }
}