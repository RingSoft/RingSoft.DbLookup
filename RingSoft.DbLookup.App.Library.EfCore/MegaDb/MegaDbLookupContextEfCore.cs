using Microsoft.EntityFrameworkCore;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.App.Library.LibLookupContext;
using RingSoft.DbLookup.App.Library.MegaDb;
using RingSoft.DbLookup.App.Library.MegaDb.Model;
using RingSoft.DbLookup.EfCore;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.RecordLocking;

namespace RingSoft.DbLookup.App.Library.EfCore.MegaDb
{
    public class MegaDbLookupContextEfCore : AppLookupContextEfCore, IMegaDbLookupContext, IAdvancedFindLookupContext
    {
        public override AppLookupContextConfiguration LookupContextConfiguration => MegaDbContextConfiguration;
        protected override DbContext DbContext => MegaDbDbContext;
        public MegaDbLookupContextConfiguration MegaDbContextConfiguration { get; }
        internal MegaDbDbContextEfCore MegaDbDbContext { get; }
        public TableDefinition<Item> Items { get; set; }
        public TableDefinition<Location> Locations { get; set; }
        public TableDefinition<Manufacturer> Manufacturers { get; set; }
        public TableDefinition<StocksTable> StocksTable { get; set; }
        public TableDefinition<MliLocationsTable> MliLocationsTable { get; set; }
        public TableDefinition<StockMaster> StockMasters { get; set; }
        public TableDefinition<StockCostQuantity> StockCostQuantities { get; set; }

        public MegaDbLookupContextEfCore()
        {
            MegaDbContextConfiguration = new MegaDbLookupContextConfiguration(this);
            MegaDbDbContext = new MegaDbDbContextEfCore(this);
            ((LookupContext)this).Initialize();
        }

        protected override void SetupModel()
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
