using System.Windows.Controls;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.App.WPFCore.MegaDb
{
    /// <summary>
    /// Interaction logic for StockMasterUserControl.xaml
    /// </summary>
    public partial class StockMasterUserControl
    {
        public StockMasterUserControl()
        {
            InitializeComponent();
        }

        protected override DbMaintenanceViewModelBase OnGetViewModel()
        {
            return StockMasterViewModel;
        }

        protected override Control OnGetMaintenanceButtons()
        {
            return ButtonsControl;
        }

        protected override DbMaintenanceStatusBar OnGetStatusBar()
        {
            return StatusBar;
        }

        protected override string GetTitle()
        {
            return "Stock Master";
        }

        protected override void ShowRecordTitle()
        {
            Host.ChangeTitle($"{Title} - {StockMasterViewModel.StockNumberAutoFillValue.Text} {StockMasterViewModel.LocationAutoFillValue.Text}");
        }

        public override void SetInitialFocus()
        {
            if (StockMasterViewModel.MaintenanceMode == DbMaintenanceModes.AddMode)
            {
                StockMasterViewModel.StockUiCommand.SetFocus();
            }
            else
            {
                StockMasterViewModel.PriceUiCommand.SetFocus();
            }
            base.SetInitialFocus();
        }
    }
}
