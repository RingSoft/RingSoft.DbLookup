using RingSoft.DbLookup.App.Library;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbLookup.Controls.WPF.AdvancedFind;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbMaintenance;
using System.Windows.Controls.Primitives;

namespace RingSoft.DbLookup.App.WPFCore.MegaDb
{
    /// <summary>
    /// Interaction logic for StockMasterWindow.xaml
    /// </summary>
    public partial class StockMasterWindow
    {
        public override DbMaintenanceViewModelBase ViewModel => StockMasterViewModel;
        public override DbMaintenanceButtonsControl MaintenanceButtonsControl => ButtonsControl;
        public override DbMaintenanceStatusBar DbStatusBar => StatusBar;

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

        private void ShowAdvancedFind()
        {
            var advancedFindWindow = new AdvancedFindWindow();
            advancedFindWindow.Loaded += (sender, args) => advancedFindWindow.ShowInTaskbar = true;
            advancedFindWindow.Show();
        }

    }
}
