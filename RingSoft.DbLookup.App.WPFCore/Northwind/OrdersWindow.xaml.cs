using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup.App.Library;
using RingSoft.DbLookup.App.Library.Northwind.ViewModels;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.App.WPFCore.Northwind
{
    /// <summary>
    /// Interaction logic for OrdersWindow.xaml
    /// </summary>
    public partial class OrdersWindow : IOrderView
    {
        public object OwnerWindow => this;

        public override DbMaintenanceViewModelBase ViewModel => OrdersViewModel;
        public override DbMaintenanceButtonsControl MaintenanceButtonsControl => ButtonsControl;

        public OrdersWindow()
        {
            InitializeComponent();
            Initialize();

            CustomerControl.LostFocus += (sender, args) => OrdersViewModel.OnCustomerIdLostFocus();
            AddModifyButton.Click += (sender, args) => OrdersViewModel.OnAddModify();

            CustomerControl.PreviewLostKeyboardFocus += (sender, args) =>
            {
                if (!this.IsWindowClosing(args.NewFocus))
                    if (!OrdersViewModel.ValidateCustomer())
                        args.Handled = true;
            };
        }

        public override void ResetViewForNewRecord()
        {
            TabControl.SelectedIndex = 0;
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

        public void OnGridValidationFailed()
        {
        }
    }
}
