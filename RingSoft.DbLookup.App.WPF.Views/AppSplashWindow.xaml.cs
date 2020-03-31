using System.Windows;
using RingSoft.DbLookup.App.Library;

namespace RingSoft.DbLookup.App.WPF.Views
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
    }
}
