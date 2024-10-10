using System.Windows;
using RingSoft.DataEntryControls.WPF;

namespace RingSoft.DbLookup.Controls.WPF
{
    /// <summary>
    /// Interaction logic for DbMaintenanceUcWindow.xaml
    /// </summary>
    public partial class DbMaintenanceUcWindow : IUserControlHost
    {
        public DbMaintenanceUcWindow()
        {
            InitializeComponent();
        }

        public void CloseHost()
        {
            Close();
        }
    }
}
