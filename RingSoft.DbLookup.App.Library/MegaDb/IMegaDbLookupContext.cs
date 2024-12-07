using RingSoft.DbLookup.App.Library.LibLookupContext;
using RingSoft.DbLookup.App.Library.MegaDb.Model;
using RingSoft.DbLookup.ModelDefinition;
using System;

namespace RingSoft.DbLookup.App.Library.MegaDb
{
    public interface IMegaDbLookupContext : IAppLookupContext
    {
        MegaDbLookupContextConfiguration MegaDbContextConfiguration { get; }

        TableDefinition<Item> Items { get; set; }

        TableDefinition<Location> Locations { get; set; }

        TableDefinition<Manufacturer> Manufacturers { get; set; }

        TableDefinition<StocksTable> StocksTable { get; set; }

        TableDefinition<MliLocationsTable> MliLocationsTable { get; set; }

        TableDefinition<StockMaster> StockMasters { get; set; }

        TableDefinition<StockCostQuantity> StockCostQuantities { get; set; }

        TableDefinition<AdvancedFind.AdvancedFind> AdvancedFinds { get; set; }
    }
}
