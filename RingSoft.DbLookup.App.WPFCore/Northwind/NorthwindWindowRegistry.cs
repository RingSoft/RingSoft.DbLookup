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

            RegisterWindow<ProductsWindow>(
                RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.Products);
        }

        protected override DbMaintenanceWindow CreateMaintenanceWindow(TableDefinitionBase tableDefinition, LookupAddViewArgs addViewArgs,
            object inputParameter)
        {
            if (tableDefinition == RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.Orders)
            {
                if (inputParameter is NorthwindViewModelInput northwindViewModelInput)
                {
                    if (northwindViewModelInput.OrderInput.GridMode)
                        return new OrdersGridWindow();
                    
                    return new OrdersWindow();
                }
            }
            else if (tableDefinition ==
                     RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.OrderDetails)
            {
                if (inputParameter is not NorthwindViewModelInput northwindViewModelInput)
                    return null;
                if (northwindViewModelInput.OrderInput.GridMode)
                    return new OrdersGridWindow();
                else
                {
                    if (northwindViewModelInput.OrderInput.FromProductOrders)
                    {
                        return new OrdersWindow();
                    }
                    return new OrderDetailsWindow();
                }
            }

            return base.CreateMaintenanceWindow(tableDefinition, addViewArgs, inputParameter);
        }
    }
}
