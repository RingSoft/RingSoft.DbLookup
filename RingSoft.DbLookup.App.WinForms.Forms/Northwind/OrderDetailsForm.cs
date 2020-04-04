using RingSoft.DbLookup.App.Library.Northwind.ViewModels;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.App.WinForms.Forms.Northwind
{
    public partial class OrderDetailsForm : DbMaintenanceForm
    {
        public override DbMaintenanceViewModelBase ViewModel => _orderDetailsViewModel;

        private readonly OrderDetailsViewModel _orderDetailsViewModel = new OrderDetailsViewModel();

        public OrderDetailsForm()
        {
            InitializeComponent();
        }
    }
}
