using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup.App.Library;
using RingSoft.DbLookup.App.Library.Northwind.ViewModels;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbMaintenance;
using System.Windows.Controls.Primitives;

namespace RingSoft.DbLookup.App.WPFCore.Northwind
{
    /// <summary>
    /// Interaction logic for OrdersGridWindow.xaml
    /// </summary>
    public partial class OrdersGridWindow : IOrderView
    {
        public object OwnerWindow => this;

        public override DbMaintenanceViewModelBase ViewModel => OrdersViewModel;
        public override DbMaintenanceButtonsControl MaintenanceButtonsControl => ButtonsControl;
        public override DbMaintenanceStatusBar DbStatusBar => StatusBar;

        public OrdersGridWindow()
        {
            InitializeComponent();

            OrdersViewModel.GridMode = true;

            Initialize();

            CustomerControl.LostFocus += (sender, args) => OrdersViewModel.OnCustomerIdLostFocus();

            CustomerControl.PreviewLostKeyboardFocus += (sender, args) =>
            {
                if (!this.IsWindowClosing(args.NewFocus))
                    if (!OrdersViewModel.ValidateCustomer())
                        args.Handled = true;
            };
        }

        public void SetFocusToGrid(OrderDetailsGridRow row, int columnId)
        {
            TabControl.SelectedIndex = 0;
            DetailsGrid.Focus();
            DetailsGrid.GotoCell(row, columnId);
        }

    }
}