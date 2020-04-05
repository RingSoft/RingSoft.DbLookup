using System.Windows.Forms;
using RingSoft.DbLookup.App.Library;
using RingSoft.DbLookup.App.Library.MegaDb.ViewModels;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.App.WinForms.Forms.MegaDb
{
    public partial class StockMasterForm : DbMaintenanceForm
    {
        private StockMasterViewModel _stockMasterViewModel = new StockMasterViewModel();

        public override DbMaintenanceViewModelBase ViewModel => _stockMasterViewModel;

        public StockMasterForm()
        {
            InitializeComponent();

            StockNumberControl.DataBindings.Add(nameof(StockNumberControl.Setup), _stockMasterViewModel,
                nameof(_stockMasterViewModel.StockNumberAutoFillSetup), true, DataSourceUpdateMode.OnPropertyChanged);
            StockNumberControl.DataBindings.Add(nameof(StockNumberControl.Value), _stockMasterViewModel,
                nameof(_stockMasterViewModel.StockNumberAutoFillValue), true, DataSourceUpdateMode.OnPropertyChanged);
            StockNumberControl.DataBindings.Add(nameof(StockNumberControl.Enabled), _stockMasterViewModel,
                nameof(_stockMasterViewModel.PrimaryKeyControlsEnabled), false, DataSourceUpdateMode.OnPropertyChanged);

            StockNumberLabel.DataBindings.Add(nameof(StockNumberLabel.Enabled), _stockMasterViewModel,
                nameof(_stockMasterViewModel.PrimaryKeyControlsEnabled), false, DataSourceUpdateMode.OnPropertyChanged);

            LocationControl.DataBindings.Add(nameof(LocationControl.Setup), _stockMasterViewModel,
                nameof(_stockMasterViewModel.LocationAutoFillSetup), true, DataSourceUpdateMode.OnPropertyChanged);
            LocationControl.DataBindings.Add(nameof(LocationControl.Value), _stockMasterViewModel,
                nameof(_stockMasterViewModel.LocationAutoFillValue), true, DataSourceUpdateMode.OnPropertyChanged);
            LocationControl.DataBindings.Add(nameof(LocationControl.Enabled), _stockMasterViewModel,
                nameof(_stockMasterViewModel.PrimaryKeyControlsEnabled), false, DataSourceUpdateMode.OnPropertyChanged);

            LocationLabel.DataBindings.Add(nameof(LocationLabel.Enabled), _stockMasterViewModel,
                nameof(_stockMasterViewModel.PrimaryKeyControlsEnabled), false, DataSourceUpdateMode.OnPropertyChanged);

            PriceTextBox.BindControlToDecimalFormat(_stockMasterViewModel, nameof(_stockMasterViewModel.Price));

            CostQuantityLookupControl.DataBindings.Add(nameof(CostQuantityLookupControl.LookupDefinition),
                _stockMasterViewModel, nameof(_stockMasterViewModel.StockCostQuantityLookupDefinition), true,
                DataSourceUpdateMode.Never);
            CostQuantityLookupControl.DataBindings.Add(nameof(CostQuantityLookupControl.Command), _stockMasterViewModel,
                nameof(_stockMasterViewModel.StockCostQuantityCommand), true, DataSourceUpdateMode.OnPropertyChanged);

            StockNumberControl.Leave += (sender, args) => _stockMasterViewModel.OnKeyControlLeave();
            LocationControl.Leave += (sender, args) => _stockMasterViewModel.OnKeyControlLeave();

            AddModifyButton.Click += (sender, args) => { _stockMasterViewModel.OnAddModify(); };
        }

        public override void ResetViewForNewRecord()
        {
            StockNumberControl.Focus();
            base.ResetViewForNewRecord();
        }

        public override void OnValidationFail(FieldDefinition fieldDefinition, string text, string caption)
        {
            var table = RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext.Stocks;

            if (fieldDefinition == table.GetFieldDefinition(p => p.StockNumber))
                StockNumberControl.Focus();
            else if (fieldDefinition == table.GetFieldDefinition(p => p.Location))
                LocationControl.Focus();

            base.OnValidationFail(fieldDefinition, text, caption);
        }
    }
}
