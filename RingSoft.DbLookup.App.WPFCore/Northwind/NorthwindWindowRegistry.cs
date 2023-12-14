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
            RegisterWindow<CustomersWindow>(
                RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.Customers);
            RegisterWindow<OrdersWindow>(
                RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.Orders);
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
                    else
                    {
                        return new OrderDetailsWindow();
                    }
                }
            }

            return base.CreateMaintenanceWindow(tableDefinition, addViewArgs, inputParameter);
        }
    }
}
