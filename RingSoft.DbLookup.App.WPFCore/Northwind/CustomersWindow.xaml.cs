using RingSoft.DbLookup.App.Library;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.App.WPFCore.Northwind
{
    /// <summary>
    /// Interaction logic for CustomersWindow.xaml
    /// </summary>
    public partial class CustomersWindow
    {
        public override DbMaintenanceViewModelBase ViewModel => CustomersViewModel;
        public override DbMaintenanceButtonsControl MaintenanceButtonsControl => ButtonsControl;

        public CustomersWindow()
        {
            InitializeComponent();

            RegisterFormKeyControl(CustomerControl);

            AddModifyButton.Click += (sender, args) => CustomersViewModel.OnAddModify();

            Initialize();
        }

        public override void ResetViewForNewRecord()
        {
            TabControl.SelectedIndex = 0;
            CustomerControl.Focus();
            base.ResetViewForNewRecord();
        }

        public override void OnValidationFail(FieldDefinition fieldDefinition, string text, string caption)
        {
            var table = RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.Customers;

            if (fieldDefinition == table.GetFieldDefinition(p => p.CustomerID))
                CustomerControl.Focus();

            base.OnValidationFail(fieldDefinition, text, caption);
        }
    }
}
