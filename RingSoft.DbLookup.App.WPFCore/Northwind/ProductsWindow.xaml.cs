using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.App.WPFCore.Northwind
{
    /// <summary>
    /// Interaction logic for ProductsWindow.xaml
    /// </summary>
    public partial class ProductsWindow
    {
        public override DbMaintenanceViewModelBase ViewModel => ProductViewModel;
        public override DbMaintenanceButtonsControl MaintenanceButtonsControl => ButtonsControl;
        public override DbMaintenanceStatusBar DbStatusBar => StatusBar;

        public ProductsWindow()
        {
            InitializeComponent();
            RegisterFormKeyControl(ProductNameControl);
        }
    }
}
