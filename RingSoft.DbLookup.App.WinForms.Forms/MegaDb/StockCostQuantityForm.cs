using System.Windows.Forms;
using RingSoft.DbLookup.App.Library.MegaDb.ViewModels;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.App.WinForms.Forms.MegaDb
{
    public partial class StockCostQuantityForm : DbMaintenanceForm
    {
        public override DbMaintenanceViewModelBase ViewModel => _costQuantityViewModel;

        private StockCostQuantityViewModel _costQuantityViewModel = new StockCostQuantityViewModel();

        public StockCostQuantityForm()
        {
            InitializeComponent();

            StockNumberLabel.DataBindings.Add(nameof(StockNumberLabel.Text), _costQuantityViewModel,
                nameof(_costQuantityViewModel.StockNumber), false, DataSourceUpdateMode.OnPropertyChanged);

            LocationLabel.DataBindings.Add(nameof(LocationLabel.Text), _costQuantityViewModel,
                nameof(_costQuantityViewModel.Location), false, DataSourceUpdateMode.OnPropertyChanged);

            PurchaseDateControl.DataBindings.Add(nameof(PurchaseDateControl.Value), _costQuantityViewModel,
                nameof(_costQuantityViewModel.PurchaseDate), false, DataSourceUpdateMode.OnPropertyChanged);
            PurchaseDateControl.DataBindings.Add(nameof(PurchaseDateControl.Enabled), _costQuantityViewModel,
                nameof(_costQuantityViewModel.PrimaryKeyControlsEnabled), false,
                DataSourceUpdateMode.OnPropertyChanged);

            QuantityTextBox.BindControlToDecimalFormat(_costQuantityViewModel, nameof(_costQuantityViewModel.Quantity),
                2, false);
            CostTextBox.BindControlToDecimalFormat(_costQuantityViewModel, nameof(_costQuantityViewModel.Cost));

            PurchaseDateControl.Leave += (sender, args) => _costQuantityViewModel.OnKeyControlLeave();
        }

        public override void ResetViewForNewRecord()
        {
            PurchaseDateControl.Focus();
            ActiveControl = PurchaseDateControl;
            base.ResetViewForNewRecord();
        }
    }
}
