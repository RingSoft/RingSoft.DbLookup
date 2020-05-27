using System;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RingSoft.DbLookup.App.Library.MegaDb;
using RingSoft.DbLookup.App.Library.MegaDb.Model;
using RingSoft.DbLookup.Ef6;

namespace RingSoft.DbLookup.App.Library.Ef6.MegaDb
{
    public class MegaDbEfDataProcessor6 : IMegaDbEfDataProcessor
    {
        public event EventHandler<ItemsTableSeederProgressArgs> ItemsTableSeederProgress;
        public Item GetItem(int itemId)
        {
            var context = new MegaDbDbContextEf6();
            return context.Items
                .Include(p => p.Location)
                .Include(p => p.Manufacturer)
                .FirstOrDefault(p => p.Id == itemId);
        }

        public bool SaveItem(Item item)
        {
            var context = new MegaDbDbContextEf6();
            return context.SaveEntity(context.Items, item, "Saving Item");
        }

        public bool DeleteItem(int itemId)
        {
            var context = new MegaDbDbContextEf6();
            var item = context.Items.FirstOrDefault(p => p.Id == itemId);
            return context.DeleteEntity(context.Items, item, "Deleting Item");
        }

        public Location GetLocation(int locationId)
        {
            var context = new MegaDbDbContextEf6();
            return context.Locations.FirstOrDefault(p => p.Id == locationId);
        }

        public bool SaveLocation(Location location)
        {
            var context = new MegaDbDbContextEf6();
            return context.SaveEntity(context.Locations, location, "Saving Location");
        }

        public bool DeleteLocation(int locationId)
        {
            var context = new MegaDbDbContextEf6();
            var location = context.Locations.FirstOrDefault(p => p.Id == locationId);
            return context.DeleteEntity(context.Locations, location, "Deleting Location");
        }

        public Manufacturer GetManufacturer(int manufacturerId)
        {
            var context = new MegaDbDbContextEf6();
            return context.Manufacturers.FirstOrDefault(f => f.Id == manufacturerId);
        }

        public bool SaveManufacturer(Manufacturer manufacturer)
        {
            var context = new MegaDbDbContextEf6();
            return context.SaveEntity(context.Manufacturers, manufacturer, "Saving Manufacturer");
        }

        public bool DeleteManufacturer(int manufacturerId)
        {
            var context = new MegaDbDbContextEf6();
            var manufacturer = context.Manufacturers.FirstOrDefault(p => p.Id == manufacturerId);
            return context.DeleteEntity(context.Manufacturers, manufacturer, "Deleting Manufacturer");
        }

        public async Task<int> SeedItemsTable(int maxRecords, CancellationToken token)
        {
            var context = new MegaDbItemsTableSeederDbContext();
            return await MegaDbMethods.SeedItemsTable(context, this, maxRecords, token);
        }

        public void OnItemsTableSeederProgress(ItemsTableSeederProgressArgs e)
        {
            ItemsTableSeederProgress?.Invoke(this, e);
        }

        public bool DoesItemsTableHaveData()
        {
            var context = new MegaDbDbContextEf6();
            return context.Items.Any();
        }

        public StockMaster GetStockItem(string stockNumber, string location)
        {
            var context = new MegaDbDbContextEf6();
            return context.Stocks.FirstOrDefault(f => f.StockNumber == stockNumber && f.Location == location);
        }

        public bool SaveStockItem(StockMaster stockItem)
        {
            var context = new MegaDbDbContextEf6();
            return context.SaveEntity(context.Stocks, stockItem, "Saving Stock Item");
        }

        public bool DeleteStockItem(string stockNumber, string location)
        {
            var context = new MegaDbDbContextEf6();
            var stockItem = context.Stocks.FirstOrDefault(f => f.StockNumber == stockNumber && f.Location == location);
            return context.DeleteEntity(context.Stocks, stockItem, "Deleting Stock Item");
        }

        public StockCostQuantity GetStockCostQuantity(string stockNumber, string location, DateTime purchaseDate)
        {
            var context = new MegaDbDbContextEf6();
            return context.StockCostQuantities.FirstOrDefault(f =>
                f.StockNumber == stockNumber && f.Location == location && f.PurchasedDateTime == purchaseDate);
        }

        public bool SaveStockCostQuantity(StockCostQuantity stockCostQuantity)
        {
            var context = new MegaDbDbContextEf6();
            return context.SaveEntity(context.StockCostQuantities, stockCostQuantity, "Saving Stock Item Purchase");
        }

        public bool DeleteStockCostQuantity(string stockNumber, string location, DateTime purchaseDate)
        {
            var context = new MegaDbDbContextEf6();
            var stockCostQuantity = context.StockCostQuantities.FirstOrDefault(f =>
                f.StockNumber == stockNumber && f.Location == location && f.PurchasedDateTime == purchaseDate);
            return context.DeleteEntity(context.StockCostQuantities, stockCostQuantity, "Deleting Stock Item Purchase");
        }
    }
}
