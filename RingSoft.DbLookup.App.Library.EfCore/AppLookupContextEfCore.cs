using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.App.Library.LibLookupContext;
using RingSoft.DbLookup.App.Library.Northwind.Model;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.EfCore;
using RingSoft.DbLookup.ModelDefinition;

namespace RingSoft.DbLookup.App.Library.EfCore
{
    public abstract class AppLookupContextEfCore : LookupContext
    {
        public abstract AppLookupContextConfiguration LookupContextConfiguration { get; }

        public DataProcessorTypes DataProcessorType
        {
            get { return LookupContextConfiguration.DataProcessorType; }
            set { LookupContextConfiguration.DataProcessorType = value; }
        }

        public override DbDataProcessor DataProcessor => LookupContextConfiguration.DataProcessor;

        public override AutoFillValue OnAutoFillTextRequest(TableDefinitionBase tableDefinition, string idValue)
        {
            if (tableDefinition == RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.Employees)
            {
                var employee = RsDbLookupAppGlobals.EfProcessor.NorthwindEfDataProcessor.GetEmployee(idValue.ToInt());
                var primaryKeyValue = RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.Employees
                    .GetPrimaryKeyValueFromEntity(employee);
                return new AutoFillValue(primaryKeyValue, employee.FirstName + " " + employee.LastName);
            }
            else if (tableDefinition == RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.Customers)
            {
                var customer = RsDbLookupAppGlobals.EfProcessor.NorthwindEfDataProcessor.GetCustomer(idValue);
                var primaryKeyValue = RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.Customers
                    .GetPrimaryKeyValueFromEntity(customer);
                return new AutoFillValue(primaryKeyValue, customer.CustomerID);
            }
            else if (tableDefinition == RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.Shippers)
            {
                var shipVia = RsDbLookupAppGlobals.EfProcessor.NorthwindEfDataProcessor.GetShipper(idValue.ToInt());
                var primaryKeyValue = RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.Shippers
                    .GetPrimaryKeyValueFromEntity(shipVia);
                return new AutoFillValue(primaryKeyValue, shipVia.CompanyName);
            }
            else if (tableDefinition == RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.Orders)
            {
                var order = RsDbLookupAppGlobals.EfProcessor.NorthwindEfDataProcessor.GetOrder(idValue.ToInt(), false);
                var primaryKeyValue = RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.Orders
                    .GetPrimaryKeyValueFromEntity(order);
                var orderTable = (TableDefinition<Order>) tableDefinition;
                var orderDateField = orderTable.GetFieldDefinition(p => p.OrderDate);
                return new AutoFillValue(primaryKeyValue, orderDateField.FormatValue(order.OrderDate.ToString()) + " - " + order.CustomerID);
            }
            return base.OnAutoFillTextRequest(tableDefinition, idValue);
        }
    }
}
