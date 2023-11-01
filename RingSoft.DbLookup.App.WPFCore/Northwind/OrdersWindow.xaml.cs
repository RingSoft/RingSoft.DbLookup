using System.Windows;
using System.Windows.Interop;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup.App.Library;
using RingSoft.DbLookup.App.Library.Northwind.ViewModels;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbLookup.Controls.WPF.AdvancedFind;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.App.WPFCore.Northwind
{
    /// <summary>
    /// Interaction logic for OrdersWindow.xaml
    /// </summary>
    public partial class OrdersWindow : IOrderView
    {
        public override DbMaintenanceViewModelBase ViewModel => OrdersViewModel;
        public override DbMaintenanceButtonsControl MaintenanceButtonsControl => ButtonsControl;
        public override DbMaintenanceStatusBar DbStatusBar  => StatusBar;

        public OrdersWindow()
        {
            InitializeComponent();
            Initialize();
        }

        public void ShowAdvancedFind()
        {
            var advancedFindWindow = new AdvancedFindWindow();
            advancedFindWindow.Loaded += (sender, args) => advancedFindWindow.ShowInTaskbar = true;
            advancedFindWindow.Show();
        }
    }
}
