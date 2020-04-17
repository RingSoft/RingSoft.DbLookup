using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.App.WPFCore.MegaDb
{
    /// <summary>
    /// Interaction logic for StockCostQuantityWindow.xaml
    /// </summary>
    public partial class StockCostQuantityWindow
    {
        public override DbMaintenanceViewModelBase ViewModel => StockCostQuantityViewModel;
        public override DbMaintenanceButtonsControl MaintenanceButtonsControl => ButtonsControl;

        public StockCostQuantityWindow()
        {
            InitializeComponent();

            PurchaseDateControl.LostFocus += (sender, args) => StockCostQuantityViewModel.OnKeyControlLeave();

            Initialize();
        }

        public override void ResetViewForNewRecord()
        {
            PurchaseDateControl.Focus();
            base.ResetViewForNewRecord();
        }
    }
}
