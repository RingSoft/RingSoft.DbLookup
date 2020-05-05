using Microsoft.EntityFrameworkCore;
using RingSoft.DbLookup.App.Library.LibLookupContext;
using RingSoft.DbLookup.App.Library.MegaDb;
using RingSoft.DbLookup.App.Library.MegaDb.Model;
using RingSoft.DbLookup.ModelDefinition;

namespace RingSoft.DbLookup.App.Library.EfCore.MegaDb
{
    public class MegaDbLookupContextEfCore : AppLookupContextEfCore, IMegaDbLookupContext
    {
        public override AppLookupContextConfiguration LookupContextConfiguration => MegaDbContextConfiguration;
        protected override DbContext DbContext => MegaDbDbContext;
        public MegaDbLookupContextConfiguration MegaDbContextConfiguration { get; }
        internal MegaDbDbContextEfCore MegaDbDbContext { get; }
        public TableDefinition<Item> Items { get; set; }
        public TableDefinition<Location> Locations { get; set; }
        public TableDefinition<Manufacturer> Manufacturers { get; set; }
        public TableDefinition<StockMaster> Stocks { get; set; }
        public TableDefinition<StockCostQuantity> StockCostQuantities { get; set; }

        public MegaDbLookupContextEfCore()
        {
            MegaDbContextConfiguration = new MegaDbLookupContextConfiguration(this);
            MegaDbDbContext = new MegaDbDbContextEfCore(this);
            Initialize();
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
