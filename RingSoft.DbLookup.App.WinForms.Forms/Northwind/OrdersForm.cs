using RingSoft.DbLookup.App.Library.Northwind.ViewModels;
using RingSoft.DbLookup.GetDataProcessor;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.App.WinForms.Forms.Northwind
{
    public partial class OrdersForm : DbMaintenanceForm
    {
        public override DbMaintenanceViewModelBase ViewModel => _orderViewModel;

        private readonly OrderViewModel _orderViewModel = new OrderViewModel();

        public OrdersForm()
        {
            InitializeComponent();
        }
    }
}
