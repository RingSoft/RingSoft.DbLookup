using RingSoft.DbLookup.App.Library.LookupContext;
using RingSoft.DbLookup.App.Library.MegaDb.Model;
using RingSoft.DbLookup.ModelDefinition;

namespace RingSoft.DbLookup.App.Library.MegaDb
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
