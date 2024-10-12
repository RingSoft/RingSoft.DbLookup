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

            RegisterUserControl<EmployeesUserControl>(
                RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.Employees);

            RegisterUserControl<OrdersGridUserControl>(
                RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.OrderDetails);

            RegisterUserControl<ProductsUserControl>(
                RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.Products);
        }

    }
}
