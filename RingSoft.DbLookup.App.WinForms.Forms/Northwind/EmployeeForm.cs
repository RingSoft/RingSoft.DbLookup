using System.Windows.Forms;
using RingSoft.DbLookup.App.Library;
using RingSoft.DbLookup.App.Library.Northwind.ViewModels;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.App.WinForms.Forms.Northwind
{
    public partial class EmployeeForm : DbMaintenanceForm
    {
        public override DbMaintenanceViewModelBase ViewModel => _employeeViewModel;

        private EmployeeViewModel _employeeViewModel = new EmployeeViewModel();

        public EmployeeForm()
        {
            InitializeComponent();

            EmployeeIdLabel.DataBindings.Add(nameof(EmployeeIdLabel.Text), _employeeViewModel,
                nameof(_employeeViewModel.EmployeeId), false, DataSourceUpdateMode.OnPropertyChanged);
            FirstNameTextBox.DataBindings.Add(nameof(FirstNameTextBox.Text), _employeeViewModel,
                nameof(_employeeViewModel.FirstName), false, DataSourceUpdateMode.OnPropertyChanged);
            LastNameTextBox.DataBindings.Add(nameof(LastNameTextBox.Text), _employeeViewModel,
                nameof(_employeeViewModel.LastName), false, DataSourceUpdateMode.OnPropertyChanged);
            CourtesyTextBox.DataBindings.Add(nameof(CourtesyTextBox.Text), _employeeViewModel,
                nameof(_employeeViewModel.TitleOfCourtesy), true, DataSourceUpdateMode.OnPropertyChanged);

            BirthDateControl.DataBindings.Add(nameof(BirthDateControl.Value), _employeeViewModel,
                nameof(_employeeViewModel.BirthDate), true, DataSourceUpdateMode.OnPropertyChanged);
            HireDateControl.DataBindings.Add(nameof(HireDateControl.Value), _employeeViewModel,
                nameof(_employeeViewModel.HireDate), true, DataSourceUpdateMode.OnPropertyChanged);
            TitleTextBox.DataBindings.Add(nameof(TitleTextBox.Text), _employeeViewModel,
                nameof(_employeeViewModel.Title), true, DataSourceUpdateMode.OnPropertyChanged);

            SupervisorControl.DataBindings.Add(nameof(SupervisorControl.Setup), _employeeViewModel,
                nameof(_employeeViewModel.ReportsToAutoFillSetup), true, DataSourceUpdateMode.Never);
            SupervisorControl.DataBindings.Add(nameof(SupervisorControl.Value), _employeeViewModel,
                nameof(_employeeViewModel.ReportsTo), true, DataSourceUpdateMode.OnPropertyChanged);

            AddressTextBox.DataBindings.Add(nameof(AddressTextBox.Text), _employeeViewModel,
                nameof(_employeeViewModel.Address), true, DataSourceUpdateMode.OnPropertyChanged);
            CityTextBox.DataBindings.Add(nameof(CityTextBox.Text), _employeeViewModel, nameof(_employeeViewModel.City), true,
                DataSourceUpdateMode.OnPropertyChanged);
            RegionTextBox.DataBindings.Add(nameof(RegionTextBox.Text), _employeeViewModel, nameof(_employeeViewModel.Region),
                true, DataSourceUpdateMode.OnPropertyChanged);
            PostalCodeTextBox.DataBindings.Add(nameof(PostalCodeTextBox.Text), _employeeViewModel,
                nameof(_employeeViewModel.PostalCode), true, DataSourceUpdateMode.OnPropertyChanged);
            CountryTextBox.DataBindings.Add(nameof(CountryTextBox.Text), _employeeViewModel,
                nameof(_employeeViewModel.Country), true, DataSourceUpdateMode.OnPropertyChanged);
            HomePhoneTextBox.DataBindings.Add(nameof(HomePhoneTextBox.Text), _employeeViewModel,
                nameof(_employeeViewModel.HomePhone), true, DataSourceUpdateMode.OnPropertyChanged);
            ExtensionTextBox.DataBindings.Add(nameof(ExtensionTextBox.Text), _employeeViewModel,
                nameof(_employeeViewModel.Extension), true, DataSourceUpdateMode.OnPropertyChanged);

            OrdersControl.DataBindings.Add(nameof(OrdersControl.LookupDefinition), _employeeViewModel,
                nameof(_employeeViewModel.OrdersLookupDefinition), true, DataSourceUpdateMode.Never);
            OrdersControl.DataBindings.Add(nameof(OrdersControl.Command), _employeeViewModel,
                nameof(_employeeViewModel.OrdersLookupCommand), true, DataSourceUpdateMode.OnPropertyChanged);

            AddModifyButton.Click += (sender, args) => _employeeViewModel.OnAddModify();
        }

        public override void ResetViewForNewRecord()
        {
            TabControl.SelectedTab = ContactPage;
            FirstNameTextBox.Focus();
            base.ResetViewForNewRecord();
        }

        public override void OnValidationFail(FieldDefinition fieldDefinition, string text, string caption)
        {
            var table = RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.Employees;

            if (fieldDefinition == table.GetFieldDefinition(p => p.FirstName))
                FirstNameTextBox.Focus();
            else if (fieldDefinition == table.GetFieldDefinition(p => p.LastName))
                LastNameTextBox.Focus();
            else if (fieldDefinition == table.GetFieldDefinition(p => p.ReportsTo))
                SupervisorControl.Focus();

            base.OnValidationFail(fieldDefinition, text, caption);
        }
    }
}
