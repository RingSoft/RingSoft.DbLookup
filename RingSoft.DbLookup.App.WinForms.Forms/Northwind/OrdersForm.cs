using RingSoft.DbLookup.App.Library;
using RingSoft.DbLookup.App.Library.Northwind.ViewModels;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbMaintenance;
using System.Windows.Forms;

namespace RingSoft.DbLookup.App.WinForms.Forms.Northwind
{
    public partial class OrdersForm : DbMaintenanceForm
    {
        public override DbMaintenanceViewModelBase ViewModel => _orderViewModel;

        private readonly OrderViewModel _orderViewModel = new OrderViewModel();

        public OrdersForm()
        {
            InitializeComponent();

            OrderIdLabel.DataBindings.Add(nameof(OrderIdLabel.Text), _orderViewModel, nameof(_orderViewModel.OrderId),
                false, DataSourceUpdateMode.OnPropertyChanged);

            CustomerControl.DataBindings.Add(nameof(CustomerControl.Setup), _orderViewModel,
                nameof(_orderViewModel.CustomersAutoFillSetup), true, DataSourceUpdateMode.Never);
            CustomerControl.DataBindings.Add(nameof(CustomerControl.Value), _orderViewModel,
                nameof(_orderViewModel.Customer), true, DataSourceUpdateMode.OnPropertyChanged);

            CompanyNameLabel.DataBindings.Add(nameof(CompanyNameLabel.Text), _orderViewModel,
                nameof(_orderViewModel.CompanyName), false, DataSourceUpdateMode.OnPropertyChanged);

            EmployeeControl.DataBindings.Add(nameof(EmployeeControl.Setup), _orderViewModel,
                nameof(_orderViewModel.EmployeeAutoFillSetup), true, DataSourceUpdateMode.Never);
            EmployeeControl.DataBindings.Add(nameof(EmployeeControl.Value), _orderViewModel,
                nameof(_orderViewModel.Employee), true, DataSourceUpdateMode.OnPropertyChanged);

            ShipViaControl.DataBindings.Add(nameof(ShipViaControl.Setup), _orderViewModel,
                nameof(_orderViewModel.ShipViaAutoFillSetup), true, DataSourceUpdateMode.Never);
            ShipViaControl.DataBindings.Add(nameof(ShipViaControl.Value), _orderViewModel,
                nameof(_orderViewModel.ShipVia), true, DataSourceUpdateMode.OnPropertyChanged);

            RequiredDateControl.DataBindings.Add(nameof(RequiredDateControl.Value), _orderViewModel,
                nameof(_orderViewModel.RequiredDate), false, DataSourceUpdateMode.OnPropertyChanged);

            OrderDateControl.DataBindings.Add(nameof(OrderDateControl.Value), _orderViewModel,
                nameof(_orderViewModel.OrderDate), false, DataSourceUpdateMode.OnPropertyChanged);

            ShippedDateControl.DataBindings.Add(nameof(ShippedDateControl.Value), _orderViewModel,
                nameof(_orderViewModel.ShippedDate), false, DataSourceUpdateMode.OnPropertyChanged);

            FreightTextBox.BindControlToDecimalFormat(_orderViewModel, nameof(_orderViewModel.Freight));

            NameTextBox.DataBindings.Add(nameof(NameTextBox.Text), _orderViewModel, nameof(_orderViewModel.ShipName),
                true, DataSourceUpdateMode.OnPropertyChanged);
            AddressTextBox.DataBindings.Add(nameof(AddressTextBox.Text), _orderViewModel,
                nameof(_orderViewModel.Address), true, DataSourceUpdateMode.OnPropertyChanged);
            CityTextBox.DataBindings.Add(nameof(CityTextBox.Text), _orderViewModel, nameof(_orderViewModel.City), true,
                DataSourceUpdateMode.OnPropertyChanged);
            RegionTextBox.DataBindings.Add(nameof(RegionTextBox.Text), _orderViewModel, nameof(_orderViewModel.Region),
                true, DataSourceUpdateMode.OnPropertyChanged);
            PostalCodeTextBox.DataBindings.Add(nameof(PostalCodeTextBox.Text), _orderViewModel,
                nameof(_orderViewModel.PostalCode), true, DataSourceUpdateMode.OnPropertyChanged);
            CountryTextBox.DataBindings.Add(nameof(CountryTextBox.Text), _orderViewModel,
                nameof(_orderViewModel.Country), true, DataSourceUpdateMode.OnPropertyChanged);

            OrderDetailsControl.DataBindings.Add(nameof(OrderDetailsControl.LookupDefinition), _orderViewModel,
                nameof(_orderViewModel.OrderDetailsLookupDefinition), true, DataSourceUpdateMode.Never);
            OrderDetailsControl.DataBindings.Add(nameof(OrderDetailsControl.Command), _orderViewModel,
                nameof(_orderViewModel.OrderDetailsLookupCommand), true, DataSourceUpdateMode.OnPropertyChanged);
            OrderDetailsControl.DataBindings.Add(nameof(OrderDetailsControl.DataSourceChanged), _orderViewModel,
                nameof(_orderViewModel.OrderDetailsDataSourceChanged), true, DataSourceUpdateMode.OnPropertyChanged);

            SubTotalLabel.BindControlToDecimalFormat(_orderViewModel, nameof(_orderViewModel.SubTotal));
            TotalDiscountLabel.BindControlToDecimalFormat(_orderViewModel, nameof(_orderViewModel.TotalDiscount));
            TotalLabel.BindControlToDecimalFormat(_orderViewModel, nameof(_orderViewModel.Total));

            CustomerControl.Leave += (sender, args) => _orderViewModel.OnCustomerIdLostFocus();
            AddModifyButton.Click += (sender, args) => _orderViewModel.OnAddModify();
        }

        public override void ResetViewForNewRecord()
        {
            TabControl.SelectedTab = DetailsPage;
            CustomerControl.Focus();

            base.ResetViewForNewRecord();
        }

        public override void OnValidationFail(FieldDefinition fieldDefinition, string text, string caption)
        {
            var table = RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.Orders;

            if (fieldDefinition == table.GetFieldDefinition(p => p.CustomerID))
                CustomerControl.Focus();
            else if (fieldDefinition == table.GetFieldDefinition(p => p.EmployeeID))
                EmployeeControl.Focus();
            else if (fieldDefinition == table.GetFieldDefinition(p => p.ShipVia))
                ShipViaControl.Focus();

            base.OnValidationFail(fieldDefinition, text, caption);
        }
    }
}
