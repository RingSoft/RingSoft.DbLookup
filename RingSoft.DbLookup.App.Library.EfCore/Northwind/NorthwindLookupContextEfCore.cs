﻿using Microsoft.EntityFrameworkCore;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.App.Library.LibLookupContext;
using RingSoft.DbLookup.App.Library.Northwind;
using RingSoft.DbLookup.App.Library.Northwind.Model;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.RecordLocking;

namespace RingSoft.DbLookup.App.Library.EfCore.Northwind
{
    public class NorthwindLookupContextEfCore : AppLookupContextEfCore, INorthwindLookupContext, IAdvancedFindLookupContext
    {
        public LookupContextBase LookupContext => this;
        public override AppLookupContextConfiguration LookupContextConfiguration => NorthwindContextConfiguration;
        public NorthwindLookupContextConfiguration NorthwindContextConfiguration { get; }
        //protected override DbContext DbContext => NorthwindDbContext;
        //internal NorthwindDbContextEfCore NorthwindDbContext { get; }

        public TableDefinition<Category> Categories { get; set; }
        public TableDefinition<Customer> Customers { get; set; }
        public TableDefinition<Employee> Employees { get; set; }
        public TableDefinition<EmployeeTerritory> EmployeeTerritories { get; set; }
        public TableDefinition<Order> Orders { get; set; }
        public TableDefinition<Order_Detail> OrderDetails { get; set; }
        public TableDefinition<Product> Products { get; set; }
        public TableDefinition<Region> Regions { get; set; }
        public TableDefinition<Shipper> Shippers { get; set; }
        public TableDefinition<Supplier> Suppliers { get; set; }
        public TableDefinition<Territory> Territories { get; set; }

        public NorthwindLookupContextEfCore()
        {
            NorthwindContextConfiguration = new NorthwindLookupContextConfiguration(this);
            SetDbContext(new NorthwindDbContextEfCore(this));
        }

        public void LaunchAddOnTheFly(LookupAddViewArgs args)
        {
            OnAddViewLookup(args);
        }

        protected override void SetupModel()
        {
            NorthwindContextConfiguration.InitializeModel();
        }

        public bool ValidateRegistryDbConnectionSettings(RegistrySettings registrySettings)
        {
            return true;
        }

        protected override void InitializeLookupDefinitions()
        {
            NorthwindContextConfiguration.ConfigureLookups();
        }

        public string GetOrderFormula()
        {
            return NorthwindContextConfiguration.GetOrderFormula();
        }
    }
}
