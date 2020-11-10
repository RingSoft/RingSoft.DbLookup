﻿using RingSoft.DbLookup.App.Library;
using RingSoft.DbLookup.App.Library.Northwind.ViewModels;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.App.WPFCore.Northwind
{
    /// <summary>
    /// Interaction logic for OrdersGridWindow.xaml
    /// </summary>
    public partial class OrdersGridWindow : IOrderView
    {
        public override DbMaintenanceViewModelBase ViewModel => OrdersViewModel;
        public override DbMaintenanceButtonsControl MaintenanceButtonsControl => ButtonsControl;

        public OrdersGridWindow()
        {
            InitializeComponent();

            OrdersViewModel.GridMode = true;

            Initialize();

            CustomerControl.LostFocus += (sender, args) => OrdersViewModel.OnCustomerIdLostFocus();

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
            var focusSuccess = true;

            if (fieldDefinition == table.GetFieldDefinition(p => p.CustomerID))
                focusSuccess = CustomerControl.Focus();
            else if (fieldDefinition == table.GetFieldDefinition(p => p.EmployeeID))
                focusSuccess = EmployeeControl.Focus();
            else if (fieldDefinition == table.GetFieldDefinition(p => p.ShipVia))
                focusSuccess = ShipViaControl.Focus();

            if (focusSuccess)
                base.OnValidationFail(fieldDefinition, text, caption);
        }

        public void OnGridValidationFailed()
        {
            TabControl.SelectedIndex = 0;
            DetailsGrid.Focus();
        }
    }
}