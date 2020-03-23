using RSDbLookupApp.Library.MegaDb.Model;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RSDbLookupApp.Library.MegaDb
{

    public class ItemsTableSeederProgressArgs
    {
        public int CurrentRecord { get; }

        public int MaxRecords { get; }

        public string Message { get; }

        public bool AllowCancel { get; }

        public ItemsTableSeederProgressArgs(int currentRecord, int maxRecords, string message, bool allowCancel)
        {
            CurrentRecord = currentRecord;
            MaxRecords = maxRecords;
            Message = message;
            AllowCancel = allowCancel;
        }
    }
    public interface IMegaDbEfDataProcessor
    {
        event EventHandler<ItemsTableSeederProgressArgs> ItemsTableSeederProgress;

        Item GetItem(int itemId);

        bool SaveItem(Item item);

        bool DeleteItem(int itemId);

        Location GetLocation(int locationId);

        bool SaveLocation(Location location);

        bool DeleteLocation(int locationId);

        Manufacturer GetManufacturer(int manufacturerId);

        bool SaveManufacturer(Manufacturer manufacturer);

        bool DeleteManufacturer(int manufacturerId);

        Task<int> SeedItemsTable(int maxRecords, CancellationToken token, MegaDbPlatforms platformType);

        void OnItemsTableSeederProgress(ItemsTableSeederProgressArgs e);

        bool DoesItemsTableHaveData();

        StockMaster GetStockItem(string stockNumber, string location);

        bool SaveStockItem(StockMaster stockItem);

        bool DeleteStockItem(string stockNumber, string location);

        StockCostQuantity GetStockCostQuantity(string stockNumber, string location, DateTime purchaseDate);

        bool SaveStockCostQuantity(StockCostQuantity stockCostQuantity);

        bool DeleteStockCostQuantity(string stockNumber, string location, DateTime purchaseDate);
    }
}
