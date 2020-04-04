using System.Windows.Forms;
using RingSoft.DbLookup.App.Library;
using RingSoft.DbLookup.App.Library.Northwind.ViewModels;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.App.WinForms.Forms.Northwind
{
    public partial class CustomerForm : DbMaintenanceForm
    {
        public override DbMaintenanceViewModelBase ViewModel => _customerViewModel;

        private CustomerViewModel _customerViewModel = new CustomerViewModel();

        public CustomerForm()
        {
            InitializeComponent();

            RegisterFormKeyControl(CustomerIdControl);
            CustomerIdControl.DataBindings.Add(nameof(CustomerIdControl.Enabled), _customerViewModel,
                nameof(_customerViewModel.PrimaryKeyControlsEnabled), false, DataSourceUpdateMode.OnPropertyChanged);
            CustomerIdLabel.DataBindings.Add(nameof(CustomerIdLabel.Enabled), _customerViewModel,
                nameof(_customerViewModel.PrimaryKeyControlsEnabled), false, DataSourceUpdateMode.OnPropertyChanged);

            CompanyNameTextBox.DataBindings.Add(nameof(CompanyNameTextBox.Text), _customerViewModel,
                nameof(_customerViewModel.CompanyName), false, DataSourceUpdateMode.OnPropertyChanged);
            ContactNameTextBox.DataBindings.Add(nameof(ContactNameTextBox.Text), _customerViewModel,
                nameof(_customerViewModel.ContactName), true, DataSourceUpdateMode.OnPropertyChanged);
            ContactTitleTextBox.DataBindings.Add(nameof(ContactTitleTextBox.Text), _customerViewModel,
                nameof(_customerViewModel.ContactTitle), true, DataSourceUpdateMode.OnPropertyChanged);
            AddressTextBox.DataBindings.Add(nameof(AddressTextBox.Text), _customerViewModel,
                nameof(_customerViewModel.Address), true, DataSourceUpdateMode.OnPropertyChanged);
            CityTextBox.DataBindings.Add(nameof(CityTextBox.Text), _customerViewModel, nameof(_customerViewModel.City), true,
                DataSourceUpdateMode.OnPropertyChanged);
            RegionTextBox.DataBindings.Add(nameof(RegionTextBox.Text), _customerViewModel, nameof(_customerViewModel.Region),
                true, DataSourceUpdateMode.OnPropertyChanged);
            PostalCodeTextBox.DataBindings.Add(nameof(PostalCodeTextBox.Text), _customerViewModel,
                nameof(_customerViewModel.PostalCode), true, DataSourceUpdateMode.OnPropertyChanged);
            CountryTextBox.DataBindings.Add(nameof(CountryTextBox.Text), _customerViewModel,
                nameof(_customerViewModel.Country), true, DataSourceUpdateMode.OnPropertyChanged);
            PhoneTextBox.DataBindings.Add(nameof(PhoneTextBox.Text), _customerViewModel,
                nameof(_customerViewModel.Phone), true, DataSourceUpdateMode.OnPropertyChanged);
            FaxTextBox.DataBindings.Add(nameof(FaxTextBox.Text), _customerViewModel, nameof(_customerViewModel.Fax),
                true, DataSourceUpdateMode.OnPropertyChanged);

            OrdersControl.DataBindings.Add(nameof(OrdersControl.LookupDefinition), _customerViewModel,
                nameof(_customerViewModel.OrdersLookupDefinition), true, DataSourceUpdateMode.Never);
            OrdersControl.DataBindings.Add(nameof(OrdersControl.Command), _customerViewModel,
                nameof(_customerViewModel.OrdersLookupCommand), true, DataSourceUpdateMode.OnPropertyChanged);

            AddModifyButton.Click += (sender, args) => _customerViewModel.OnAddModify();
        }

        public override void ResetViewForNewRecord()
        {
            TabControl.SelectedTab = ContactPage;
            CustomerIdControl.Focus();
            base.ResetViewForNewRecord();
        }

        public override void OnValidationFail(FieldDefinition fieldDefinition, string text, string caption)
        {
            var table = RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.Customers;
            if (fieldDefinition == table.GetFieldDefinition(p => p.CustomerID))
                CustomerIdControl.Focus();
            else if (fieldDefinition == table.GetFieldDefinition(p => p.CompanyName))
                CompanyNameTextBox.Focus();

            base.OnValidationFail(fieldDefinition, text, caption);
        }
    }
}
