using RingSoft.DbLookup.App.Library;
using RingSoft.DbLookup.App.Library.Northwind.ViewModels;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;

namespace RingSoft.DbLookup.App.WPFCore.Northwind
{
    public class NorthwindWindowRegistry : DbMaintenanceWindowRegistry
    {
        public NorthwindWindowRegistry()
        {
            RegisterUserControl<CustomersUserControl>(
                RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.Customers);
            RegisterUserControl<OrdersGridUserControl>(
                RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.Orders);
            RegisterWindow<CustomersWindow>(
                RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.Customers);
            RegisterWindow<OrdersWindow>(
                RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.Orders);
            RegisterWindow<OrderDetailsWindow>(
                RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.OrderDetails);
            RegisterWindow<EmployeesWindow>(
                RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.Employees);

            RegisterUserControl<EmployeesUserControl>(
                RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.Employees);

            RegisterUserControl<OrdersGridUserControl>(
                RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.OrderDetails);

            RegisterWindow<ProductsWindow>(
                RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.Products);
            RegisterUserControl<ProductsUserControl>(
                RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.Products);
        }

    }
}
