using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.EfCore;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.Testing;
using RingSoft.DbLookup.Tests.Model;

namespace RingSoft.DbLookup.Tests
{
    public class TestDatabase : TestLookupContextBase
    {
        public TableDefinition<Customer> Customers { get; set; }

        public LookupDefinition<CustomerLookup, Customer> CustomerLookup { get; set; }

        public TestDatabase(DbContext context) : base(context)
        {
            PopulateDatabase();
        }

        public override DbDataProcessor DataProcessor { get; }
        
        protected override void InitializeLookupDefinitions()
        {
            CustomerLookup = new LookupDefinition<CustomerLookup, Customer>(Customers);

            CustomerLookup
                .AddVisibleColumnDefinition(p => p.Name
                    , "Name"
                    , p => p.Name, 50);

            CustomerLookup
                .AddVisibleColumnDefinition(p => p.Name
                    , "Contact"
                    , p => p.ContactName, 50);

            Customers.HasLookupDefinition(CustomerLookup);
        }

        protected override void SetupModel()
        {
            
        }

        private void PopulateDatabase()
        {
            DataRepository.DataContext.AddEntity(new DataRepositoryRegistryItem<Customer>(new Customer()));

            var customers = new List<Customer>();
            customers.Add(new Customer()
            {
                Id = 1,
                Name = "Test",
                ContactName = "Test"
            });
            DataRepository.DataContext.AddRange(customers);
        }

    }
}
