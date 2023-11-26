using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RingSoft.DbLookup.App.Library.MegaDb;
using RingSoft.DbLookup.App.Library.MegaDb.Model;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.EfCore;

namespace RingSoft.DbLookup.App.Library.EfCore.MegaDb
{
    public class MegaDbEfDataProcessorCore : IMegaDbEfDataProcessor
    {
        public event EventHandler<ItemsTableSeederProgressArgs> ItemsTableSeederProgress;

        public void SetDataContext()
        {
            SystemDataRepositoryEfCore.RepositoryMode = DataRepositoryModes.MegaDb;
        }

        public Item GetItem(int itemId)
        {
            var context = new MegaDbDbContextEfCore();
            return context.Items
                .Include(p => p.Location)
                .Include(p => p.Manufacturer)
                .FirstOrDefault(p => p.Id == itemId);
        }

        public bool SaveItem(Item item)
        {
            var context = new MegaDbDbContextEfCore();
            return context.SaveEntity(context.Items, item, "Saving Item");
        }

        public bool DeleteItem(int itemId)
        {
            var context = new MegaDbDbContextEfCore();
            var item = context.Items.FirstOrDefault(p => p.Id == itemId);
            return context.DeleteEntity(context.Items, item, "Deleting Item");
        }

        public Location GetLocation(int locationId)
        {
            var context = new MegaDbDbContextEfCore();
            return context.Locations.FirstOrDefault(p => p.Id == locationId);
        }

        public bool SaveLocation(Location location)
        {
            var context = new MegaDbDbContextEfCore();
            return context.SaveEntity(context.Locations, location, "Saving Location");
        }

        public bool DeleteLocation(int locationId)
        {
            var context = new MegaDbDbContextEfCore();
            var location = context.Locations.FirstOrDefault(p => p.Id == locationId);
            return context.DeleteEntity(context.Locations, location, "Deleting Location");
        }

        public Manufacturer GetManufacturer(int manufacturerId)
        {
            var context = new MegaDbDbContextEfCore();
            return context.Manufacturers.FirstOrDefault(f => f.Id == manufacturerId);
        }

        public bool SaveManufacturer(Manufacturer manufacturer)
        {
            var context = new MegaDbDbContextEfCore();
            return context.SaveEntity(context.Manufacturers, manufacturer, "Saving Manufacturer");
        }

        public bool DeleteManufacturer(int manufacturerId)
        {
            var context = new MegaDbDbContextEfCore();
            var manufacturer = context.Manufacturers.FirstOrDefault(p => p.Id == manufacturerId);
            return context.DeleteEntity(context.Manufacturers, manufacturer,"Deleting Manufacturer");
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
            try
            {
                var context = new MegaDbDbContextEfCore();
                return context.Items.Any();
            }
            catch (Exception e)
            {
                DbDataProcessor.DisplayDataException(e, "Seeding Items Table");
            }

            return true;
        }

        public StockMaster GetStockItem(string stockNumber, string location)
        {
            var context = new MegaDbDbContextEfCore();
            return context.Stocks.FirstOrDefault(f => f.StockNumber == stockNumber && f.Location == location);
        }

        public bool SaveStockItem(StockMaster stockItem)
        {
            using (var context = new MegaDbDbContextEfCore())
            {
                if (context.Stocks.FirstOrDefault(f =>
                        f.StockNumber == stockItem.StockNumber && f.Location == stockItem.Location) == null)
                    return context.AddNewEntity(context.Stocks, stockItem, "Saving Stock Item");
            }

            using (var context = new MegaDbDbContextEfCore())
            {
                return context.SaveEntity(context.Stocks, stockItem, "Saving Stock Item");
            }
        }

        public bool DeleteStockItem(string stockNumber, string location)
        {
            var context = new MegaDbDbContextEfCore();
            var stockItem = context.Stocks.FirstOrDefault(f => f.StockNumber == stockNumber && f.Location == location);
            return context.DeleteEntity(context.Stocks, stockItem, "Deleting Stock Item");
        }

        public StockCostQuantity GetStockCostQuantity(string stockNumber, string location, DateTime purchaseDate)
        {
            var context = new MegaDbDbContextEfCore();
            return context.StockCostQuantities.FirstOrDefault(f =>
                f.StockNumber == stockNumber && f.Location == location && f.PurchasedDateTime == purchaseDate);
        }

        public bool SaveStockCostQuantity(StockCostQuantity stockCostQuantity)
        {
            using (var context = new MegaDbDbContextEfCore())
            {
                if (context.StockCostQuantities.FirstOrDefault(f =>
                        f.StockNumber == stockCostQuantity.StockNumber && f.Location == stockCostQuantity.Location &&
                        f.PurchasedDateTime == stockCostQuantity.PurchasedDateTime) == null)
                    return context.AddNewEntity(context.StockCostQuantities, stockCostQuantity, "Saving Stock Item Purchase");
            }

            using (var context = new MegaDbDbContextEfCore())
            {
                return context.SaveEntity(context.StockCostQuantities, stockCostQuantity, "Saving Stock Item Purchase");
            }
        }

        public bool DeleteStockCostQuantity(string stockNumber, string location, DateTime purchaseDate)
        {
            var context = new MegaDbDbContextEfCore();
            var stockCostQuantity= context.StockCostQuantities.FirstOrDefault(f =>
                f.StockNumber == stockNumber && f.Location == location && f.PurchasedDateTime == purchaseDate);
            return context.DeleteEntity(context.StockCostQuantities, stockCostQuantity, "Deleting Stock Item Purchase");
        }
    }
}
