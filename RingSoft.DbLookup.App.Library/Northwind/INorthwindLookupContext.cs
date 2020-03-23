using RingSoft.DbLookup.ModelDefinition;
using RSDbLookupApp.Library.Northwind.Model;

namespace RSDbLookupApp.Library.Northwind
{
    public interface INorthwindLookupContext : IAppLookupContext
    {
        NorthwindLookupContextConfiguration NorthwindContextConfiguration { get; }

        TableDefinition<Category> Categories { get; set; }
        TableDefinition<Customer> Customers { get; set; }
        TableDefinition<Employee> Employees { get; set; }
        TableDefinition<EmployeeTerritory> EmployeeTerritories { get; set; }
        TableDefinition<Order> Orders { get; set; }
        TableDefinition<Order_Detail> OrderDetails { get; set; }
        TableDefinition<Product> Products { get; set; }
        TableDefinition<Region> Regions { get; set; }
        TableDefinition<Shipper> Shippers { get; set; }
        TableDefinition<Supplier> Suppliers { get; set; }
        TableDefinition<Territory> Territories { get; set; }
    }
}