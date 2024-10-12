using System.Windows.Controls;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.App.WPFCore.Northwind
{
    /// <summary>
    /// Interaction logic for ProductsUserControl.xaml
    /// </summary>
    public partial class ProductsUserControl
    {
        public ProductsUserControl()
        {
            InitializeComponent();
            RegisterFormKeyControl(ProductNameControl);
        }

        protected override DbMaintenanceViewModelBase OnGetViewModel()
        {
            return ProductViewModel;
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
            return "Products";
        }
    }
}
