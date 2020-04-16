using System.Windows;
using RingSoft.DbLookup.App.Library;
using RingSoft.DbLookup.App.Library.MegaDb;
using RingSoft.DbLookup.App.Library.ViewModels;

namespace RingSoft.DbLookup.App.WPFCore
{
    /// <summary>
    /// Interaction logic for MegaDbSeedWindow.xaml
    /// </summary>
    public partial class MegaDbSeedWindow : IMegaDbSeedView
    {
        public MegaDbSeedWindow(MegaDbPlatforms platformType)
        {
            InitializeComponent();

            Loaded += (sender, args) => MegaDbSeedViewModel.OnViewLoaded(this, platformType);
            StartProcessButton.Click += (sender, args) => MegaDbSeedViewModel.StartProcess();
            CloseButton.Click += (sender, args) => CloseWindow();
        }

        public string HotKeyPrefix => "_";
        public void ShowInformationMessage(string message, string title)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void ShowValidationMessage(string message, string title)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        public void ItemsTableSeederProgress(ItemsTableSeederProgressArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                ProgressBar.Value = e.CurrentRecord;
                ProgressTextBlock.Text = e.Message;
                StartProcessButton.IsEnabled = e.AllowCancel;
            });
        }

        public void CloseWindow()
        {
            Close();
        }
    }
}
