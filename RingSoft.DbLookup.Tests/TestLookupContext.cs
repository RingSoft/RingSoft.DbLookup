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

        public TableDefinition<Error> Errors { get; set; }

        public TableDefinition<TimeClock> TimeClocks { get; set; }

        public LookupDefinition<CustomerLookup, Customer> CustomerLookup { get; set; }

        public LookupDefinition<ErrorLookup, Error> ErrorLookup { get; set; }

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

            ErrorLookup = new LookupDefinition<ErrorLookup, Error>(Errors);

            ErrorLookup
                .AddVisibleColumnDefinition(p => p.ErrorId
                    , "ErrorId"
                    , p => p.ErrorId, 50);

            Errors.HasLookupDefinition(ErrorLookup);


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

            TimeClockLookup.Include(p => p.Error)
                .AddVisibleColumnDefinition(
                    p => p.ErrorId
                    , "Error"
                    , p => p.ErrorId, 25);

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
            DataRepository.DataContext.AddEntity(new DataRepositoryRegistryItem<Error>(new Error()));
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

            customers.Add(new Customer()
            {
                Id = 9,
                Name = "Peter Blankard (President)",
            });

            customers.Add(new Customer()
            {
                Id = 10,
                Name = "Smiley Whitesock (Sales Manager)",
            });

            customers.Add(new Customer()
            {
                Id = 11,
                Name = "Sparky McCllean",
            });

            customers.Add(new Customer()
            {
                Id = 12,
                Name = "Susan Jones (PD Group Lead)",
            });

            customers.Add(new Customer()
            {
                Id = 13,
                Name = "Charley Damsen",
            });

            customers.Add(new Customer()
            {
                Id = 14,
                Name = "Smitty James",
            });

            customers.Add(new Customer()
            {
                Id = 15,
                Name = "Larry Talbert",
            });

            DataRepository.DataContext.AddRange(customers);

            var errors = new List<Error>();
            for (int i = 0; i < 4; i++)
            {
                var error = new Error()
                {
                    Id = i + 1,
                    ErrorId = $"E-{i + 1}",
                };
                errors.Add(error);
            }

            DataRepository.DataContext.AddRange(errors);

            var timeClocks = new List<TimeClock>();
            var customerIndex = 1;
            var piDateIndex = 1;
            var errorIndex = 1;
            var startDate = DateTime.Parse("01/01/1980 12:00:00 AM");

            for (int i = 1; i < 101; i++)
            {
                var punchInDate = startDate.AddDays(piDateIndex);
                DateTime? punchOutDate = null;
                Error error = null;
                if (i < 50)
                {
                    punchOutDate = punchInDate.AddHours(1);
                    error = errors.FirstOrDefault(p => p.Id == errorIndex);
                }
                var customer = customers.FirstOrDefault(p => p.Id == customerIndex);
                var timeClock = new TimeClock()
                {
                    Id = i,
                    TimeClockId =$"T-{i}",
                    PunchInDate = punchInDate,
                    PunchOutDate = punchOutDate,
                    CustomerId = customer.Id,
                    Customer = customer,
                    Error = error,
                    ErrorId = error?.Id,
                };
                timeClocks.Add(timeClock);
                customerIndex++;
                piDateIndex++;
                errorIndex++;
                if (customerIndex == 16)
                {
                    customerIndex = 1;
                }

                if (piDateIndex == 5)
                {
                    piDateIndex = 1;
                }

                if (errorIndex == 5)
                {
                    errorIndex = 1;
                }
            }
            DataRepository.DataContext.AddRange(timeClocks);
        }

    }
}
