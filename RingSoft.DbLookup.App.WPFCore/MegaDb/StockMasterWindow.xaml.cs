using RingSoft.DbLookup.App.Library;
using RingSoft.DbLookup.Controls.WPF.AdvancedFind;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.App.WPFCore.MegaDb
{
    /// <summary>
    /// Interaction logic for StockMasterWindow.xaml
    /// </summary>
    public partial class StockMasterWindow
    {
        public override DbMaintenanceViewModelBase ViewModel => StockMasterViewModel;
        public override DbMaintenanceButtonsControl MaintenanceButtonsControl => ButtonsControl;

        public StockMasterWindow()
        {
            InitializeComponent();

            StockNumberControl.LostFocus += (sender, args) => StockMasterViewModel.OnKeyControlLeave();
            LocationControl.LostFocus += (sender, args) => StockMasterViewModel.OnKeyControlLeave();

            AddModifyButton.Click += (sender, args) => { StockMasterViewModel.OnAddModify(); };

            AdvancedFindButton.Click += (sender, args) => ShowAdvancedFind();

            Initialize();
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

        private void ShowAdvancedFind()
        {
            var advancedFindWindow = new AdvancedFindWindow();
            advancedFindWindow.Loaded += (sender, args) => advancedFindWindow.ShowInTaskbar = true;
            advancedFindWindow.Show();
        }

    }
}
