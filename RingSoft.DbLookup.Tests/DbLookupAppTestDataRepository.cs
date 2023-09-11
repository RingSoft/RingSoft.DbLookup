using RingSoft.DbLookup.App.Library.Northwind.Model;
using RingSoft.DbLookup.Testing;

namespace RingSoft.DbLookup.Tests
{
    public class DbLookupAppTestDataRepository : TestDataRepository
    {
        public DbLookupAppTestDataRepository(DataRepositoryRegistry context) : base(context)
        {
            DataContext.AddEntity(new DataRepositoryRegistryItem<Customer>());
            DataContext.AddEntity(new DataRepositoryRegistryItem<Employee>());
            DataContext.AddEntity(new DataRepositoryRegistryItem<Shipper>());
            DataContext.AddEntity(new DataRepositoryRegistryItem<Order>());
            DataContext.AddEntity(new DataRepositoryRegistryItem<Category>());
            DataContext.AddEntity(new DataRepositoryRegistryItem<Product>());
            DataContext.AddEntity(new DataRepositoryRegistryItem<Order_Detail>());
        }
    }
}
