using System.Data.Entity;
using RingSoft.DbLookup.App.Library.LibLookupContext;
using RingSoft.DbLookup.App.Library.MegaDb;
using RingSoft.DbLookup.App.Library.MegaDb.Model;
using RingSoft.DbLookup.ModelDefinition;

namespace RingSoft.DbLookup.App.Library.Ef6.MegaDb
{
    public class MegaDbLookupContextEf6 : AppLookupContextEf6, IMegaDbLookupContext
    {
        public override AppLookupContextConfiguration LookupContextConfiguration => MegaDbContextConfiguration;
        protected override DbContext DbContext => MegaDbDbContext;
        public MegaDbLookupContextConfiguration MegaDbContextConfiguration { get; }
        internal MegaDbDbContextEf6 MegaDbDbContext { get; }
        public TableDefinition<Item> Items { get; set; }
        public TableDefinition<Location> Locations { get; set; }
        public TableDefinition<Manufacturer> Manufacturers { get; set; }
        public TableDefinition<StockMaster> Stocks { get; set; }
        public TableDefinition<StockCostQuantity> StockCostQuantities { get; set; }

        public MegaDbLookupContextEf6()
        {
            MegaDbContextConfiguration = new MegaDbLookupContextConfiguration(this);
            MegaDbDbContext = new MegaDbDbContextEf6(this);
            Initialize();
        }
        protected override void InitializeTableDefinitions()
        {
        }

        protected override void InitializeFieldDefinitions()
        {
            MegaDbContextConfiguration.InitializeModel();
        }
        public bool ValidateRegistryDbConnectionSettings(RegistrySettings registrySettings)
        {
            return true;
        }

        protected override void InitializeLookupDefinitions()
        {
            MegaDbContextConfiguration.ConfigureLookups();
        }
    }
}
