using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.EfCore;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DbLookup.Testing;
using RingSoft.DbLookup.Tests.Model;

namespace RingSoft.DbLookup.Tests
{
    public class TestLookupContext : TestLookupContextBase
    {
        public TableDefinition<Customer> Customers { get; set; }

        public TableDefinition<TimeClock> TimeClocks { get; set; }

        public LookupDefinition<CustomerLookup, Customer> CustomerLookup { get; set; }
        
        public LookupDefinition<TimeClockLookup, TimeClock> TimeClockLookup { get; set; }

        public TestLookupContext(DbContext context) : base(context)
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

            Customers.HasLookupDefinition(CustomerLookup);

            TimeClockLookup = new LookupDefinition<TimeClockLookup, TimeClock>(TimeClocks);
            TimeClockLookup.AddVisibleColumnDefinition(
                p => p.TimeClockId
                , "Time Clock ID"
                , p => p.TimeClockId, 25);
            TimeClockLookup.AddVisibleColumnDefinition(
                p => p.PunchInDate
                , "Punch In Date"
                , p => p.PunchInDate, 25);

            TimeClockLookup.AddVisibleColumnDefinition(
                p => p.PunchOutDate
                , "Punch Out Date"
                , p => p.PunchOutDate, 25);

            TimeClockLookup.Include(p => p.Customer)
                .AddVisibleColumnDefinition(
                p => p.CustomerName
                , "Customer"
                , p => p.Name, 25);
            TimeClocks.HasLookupDefinition(TimeClockLookup);
        }

        protected override void SetupModel()
        {
            TimeClocks.GetFieldDefinition(p => p.PunchInDate)
                .HasDateType(DbDateTypes.DateTime)
                .DoConvertToLocalTime();

            TimeClocks.GetFieldDefinition(p => p.PunchOutDate)
                .HasDateType(DbDateTypes.DateTime)
                .DoConvertToLocalTime();
        }

        private void PopulateDatabase()
        {
            DataRepository.DataContext.AddEntity(new DataRepositoryRegistryItem<Customer>(new Customer()));
            DataRepository.DataContext.AddEntity(new DataRepositoryRegistryItem<TimeClock>(new TimeClock()));

            var customers = new List<Customer>();
            customers.Add(new Customer()
            {
                Id = 1,
                Name = "Anna Smallsa (Tech Support Manager)",
            });

            customers.Add(new Customer()
            {
                Id = 2,
                Name = "Bruce Applegate",
            });

            customers.Add(new Customer()
            {
                Id = 3,
                Name = "Dave Smitty (Master User)",
            });

            customers.Add(new Customer()
            {
                Id = 4,
                Name = "Jane Posley",
            });

            customers.Add(new Customer()
            {
                Id = 5,
                Name = "Janet Lostly",
            });

            customers.Add(new Customer()
            {
                Id = 6,
                Name = "John Doe (QA Director)",
            });

            customers.Add(new Customer()
            {
                Id = 7,
                Name = "Jonsey Smithers",
            });

            customers.Add(new Customer()
            {
                Id = 8,
                Name = "Laura Curtis",
            });

            DataRepository.DataContext.AddRange(customers);

            var timeClocks = new List<TimeClock>();
            var customerIndex = 1;
            var piDateIndex = 1;

            for (int i = 1; i < 101; i++)
            {
                var punchInDate = DateTime.Now.AddDays(piDateIndex);
                DateTime? punchOutDate = null;
                Customer customer = null;
                if (i < 50)
                {
                    punchOutDate = punchInDate.AddHours(1);
                    customer = customers.FirstOrDefault(p => p.Id == customerIndex);
                }
                else
                {
                    
                }
                var timeClock = new TimeClock()
                {
                    Id = i,
                    TimeClockId =$"T-{i}",
                    PunchInDate = punchInDate,
                    PunchOutDate = punchOutDate,
                    CustomerId = customer?.Id,
                    Customer = customer,
                };
                timeClocks.Add(timeClock);
                customerIndex++;
                piDateIndex++;
                if (customerIndex == 9)
                {
                    customerIndex = 1;
                }

                if (piDateIndex == 5)
                {
                    piDateIndex = 1;
                }
            }
            DataRepository.DataContext.AddRange(timeClocks);
        }

    }
}
