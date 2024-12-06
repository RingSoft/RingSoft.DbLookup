using System.ComponentModel;
using System.Windows;
using RingSoft.DataEntryControls.WPF;

namespace RingSoft.DbLookup.Controls.WPF
{
    /// <summary>
    /// Interaction logic for DbMaintenanceUcWindow.xaml
    /// </summary>
    public partial class DbMaintenanceUcWindow : IUserControlHost
    {
        private DbMaintenanceUserControl UserControl { get; }

        public HostTypes HostType => HostTypes.Window;

        public DbMaintenanceUcWindow(DbMaintenanceUserControl userControl)
        {
            UserControl = userControl;
            if (!double.IsNaN(userControl.WindowWidth) && !double.IsNaN(userControl.WindowHeight))
            {
                Width = userControl.WindowWidth;
                Height = userControl.WindowHeight;
            }
            else
            {
                SizeToContent = SizeToContent.WidthAndHeight;
            }

            InitializeComponent();
            Title = userControl.Title;
        }

        public void CloseHost()
        {
            Close();
        }

        public void ChangeTitle(string title)
        {
            Title = title;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            UserControl.ViewModel.OnWindowClosing(e);
            base.OnClosing(e);
        }
    }
}
