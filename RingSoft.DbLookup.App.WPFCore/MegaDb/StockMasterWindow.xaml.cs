using RingSoft.DbLookup.App.Library;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbLookup.Controls.WPF.AdvancedFind;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbMaintenance;
using System.Windows.Controls.Primitives;
using RingSoft.DbLookup.App.Library.MegaDb.ViewModels;

namespace RingSoft.DbLookup.App.WPFCore.MegaDb
{
    /// <summary>
    /// Interaction logic for StockMasterWindow.xaml
    /// </summary>
    public partial class StockMasterWindow : IStockMasterView
    {
        public override DbMaintenanceViewModelBase ViewModel => StockMasterViewModel;
        public override DbMaintenanceButtonsControl MaintenanceButtonsControl => ButtonsControl;
        public override DbMaintenanceStatusBar DbStatusBar => StatusBar;

        public StockMasterWindow()
        {
            InitializeComponent();
        }

        public void ShowAdvancedFind()
        {
            var advancedFindWindow = new AdvancedFindWindow();
            advancedFindWindow.Loaded += (sender, args) => advancedFindWindow.ShowInTaskbar = true;
            advancedFindWindow.Show();
        }

    }
}
