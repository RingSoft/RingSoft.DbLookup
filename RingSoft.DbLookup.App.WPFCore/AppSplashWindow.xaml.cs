using System.Windows;
using RingSoft.DbLookup.App.Library;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbLookup.DataProcessor;

namespace RingSoft.DbLookup.App.WPFCore
{
    /// <summary>
    /// Interaction logic for AppSplashWindow.xaml
    /// </summary>
    public partial class AppSplashWindow : IAppSplashWindow
    {
        public bool IsDisposed => false;
        public bool Disposing => false;

        public AppSplashWindow()
        {
            InitializeComponent();
        }

        public void SetProgress(string progressText)
        {
            Dispatcher.Invoke(() => ProgressTextBlock.Text = progressText);
        }

        public void CloseSplash()
        {
            Dispatcher.Invoke(() => Close());
        }

        public void ShowErrorMessageBox(string message, string caption)
        {
            Dispatcher.Invoke(() =>
            {
                MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Error);
            });

        }
    }
}
