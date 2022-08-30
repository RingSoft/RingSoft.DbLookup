using Microsoft.EntityFrameworkCore;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.App.Library.LibLookupContext;
using RingSoft.DbLookup.App.Library.Northwind;
using RingSoft.DbLookup.App.Library.Northwind.Model;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;

namespace RingSoft.DbLookup.App.Library.EfCore.Northwind
{
    public class NorthwindLookupContextEfCore : AppLookupContextEfCore, INorthwindLookupContext, IAdvancedFindLookupContext
    {
        public override AppLookupContextConfiguration LookupContextConfiguration => NorthwindContextConfiguration;
        public NorthwindLookupContextConfiguration NorthwindContextConfiguration { get; }
        protected override DbContext DbContext => NorthwindDbContext;
        internal NorthwindDbContextEfCore NorthwindDbContext { get; }

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
        public TableDefinition<AdvancedFind.AdvancedFind> AdvancedFinds { get; set; }
        public TableDefinition<AdvancedFindColumn> AdvancedFindColumns { get; set; }
        public TableDefinition<AdvancedFindFilter> AdvancedFindFilters { get; set; }
        public LookupDefinition<AdvancedFindLookup, AdvancedFind.AdvancedFind> AdvancedFindLookup { get; set; }


        public NorthwindLookupContextEfCore()
        {
            NorthwindContextConfiguration = new NorthwindLookupContextConfiguration(this);
            NorthwindDbContext = new NorthwindDbContextEfCore(this);
            Initialize();
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

        public void SetAdvancedFind()
        {
            SystemGlobals.AdvancedFindLookupContext = this;
            var configuration = new AdvancedFindLookupConfiguration(SystemGlobals.AdvancedFindLookupContext);
            configuration.InitializeModel();
            configuration.ConfigureLookups();

        }
    }
}
