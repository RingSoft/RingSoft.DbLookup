using System.Windows;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.App.WPF.Views.MegaDb
{
    /// <summary>
    /// Interaction logic for ItemsWindow.xaml
    /// </summary>
    public partial class ItemsWindow
    {
        public override DbMaintenanceViewModelBase ViewModel => ItemsViewModel;
        public override DbMaintenanceButtonsControl MaintenanceButtonsControl => ButtonsControl;

        public ItemsWindow()
        {
            InitializeComponent();

            Initialize();
        }
    }
}
