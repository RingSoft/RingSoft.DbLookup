using RingSoft.DbLookup.App.Library.DevLogix.Model;
using RingSoft.DbLookup.App.Library.Northwind.Model;
using RingSoft.DbLookup.Testing;

namespace RingSoft.DbLookup.Tests
{
    public class DbLookupAppTestDataRepository : TestDataRepository
    {
        public DbLookupAppTestDataRepository(DataRepositoryRegistry context) : base(context)
        {
            DataContext.AddEntity(new DataRepositoryRegistryItem<Customer>(new Customer()));
        }
    }
}
