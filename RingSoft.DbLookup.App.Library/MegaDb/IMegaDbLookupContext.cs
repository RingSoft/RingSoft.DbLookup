using RingSoft.DbLookup.ModelDefinition;
using RSDbLookupApp.Library.MegaDb.Model;

namespace RSDbLookupApp.Library.MegaDb
{
    public interface IMegaDbLookupContext : IAppLookupContext
    {
        MegaDbLookupContextConfiguration MegaDbContextConfiguration { get; }

        TableDefinition<Item> Items { get; set; }

        TableDefinition<Location> Locations { get; set; }

        TableDefinition<Manufacturer> Manufacturers { get; set; }

        TableDefinition<StockMaster> Stocks { get; set; }

        TableDefinition<StockCostQuantity> StockCostQuantities { get; set; }
    }
}
