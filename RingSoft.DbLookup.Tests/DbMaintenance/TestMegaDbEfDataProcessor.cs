using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RingSoft.DbLookup.App.Library.MegaDb;
using RingSoft.DbLookup.App.Library.MegaDb.Model;

namespace RingSoft.DbLookup.Tests
{
    public class TestMegaDbEfDataProcessor : IMegaDbEfDataProcessor
    {
        public const int TestItemId1 = 1;
        public const int TestItemId2 = 2;

        public List<Item> Items { get; }

        public List<Location> Locations { get; }

        public List<Manufacturer> Manufacturers { get; }
        
        public event EventHandler<ItemsTableSeederProgressArgs> ItemsTableSeederProgress;

        public TestMegaDbEfDataProcessor()
        {
            Locations = new List<Location>
            {
                new Location {Id = 1, Name = "Location"}
            };

            Manufacturers = new List<Manufacturer>
            {
                new Manufacturer {Id = 1, Name = "Manufacturer"}
            };

            Items = new List<Item>
            { 
                new Item
                {
                    Id = TestItemId1,
                    Name = "Test Item 1",
                    Location = GetLocation(1),
                    LocationId = 1,
                    Manufacturer = GetManufacturer(1),
                    ManufacturerId = 1
                },
                new Item
                {
                    Id = TestItemId2,
                    Name = "Test Item 2",
                    Location = GetLocation(1),
                    LocationId = 1,
                    Manufacturer = GetManufacturer(1),
                    ManufacturerId = 1
                }
            };
        }

        public Item GetItem(int itemId)
        {
            return Items.FirstOrDefault(f => f.Id == itemId);
        }

        public bool SaveItem(Item item)
        {
            if (item.Id == 0)
                item.Id = Items.Count + 1;

            var existingItem = Items.FirstOrDefault(f => f.Id == item.Id);
            if (existingItem == null)
                Items.Add(item);
            else
            {
                existingItem.Name = item.Name;
                existingItem.Location = GetLocation(item.LocationId);
                existingItem.LocationId = item.LocationId;
                existingItem.Manufacturer = GetManufacturer(item.ManufacturerId);
                existingItem.ManufacturerId = item.ManufacturerId;
            }
            return true;
        }

        public bool DeleteItem(int itemId)
        {
            Items.Remove(GetItem(itemId));
            return true;
        }

        public Location GetLocation(int locationId)
        {
            return Locations.FirstOrDefault(f => f.Id == locationId);
        }

        public bool SaveLocation(Location location)
        {
            throw new NotImplementedException();
        }

        public bool DeleteLocation(int locationId)
        {
            throw new NotImplementedException();
        }

        public Manufacturer GetManufacturer(int manufacturerId)
        {
            return Manufacturers.FirstOrDefault(f => f.Id == manufacturerId);
        }

        public bool SaveManufacturer(Manufacturer manufacturer)
        {
            throw new NotImplementedException();
        }

        public bool DeleteManufacturer(int manufacturerId)
        {
            throw new NotImplementedException();
        }

        public Task<int> SeedItemsTable(int maxRecords, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public void OnItemsTableSeederProgress(ItemsTableSeederProgressArgs e)
        {
            throw new NotImplementedException();
        }

        public bool DoesItemsTableHaveData()
        {
            throw new NotImplementedException();
        }

        public StockMaster GetStockItem(string stockNumber, string location)
        {
            throw new NotImplementedException();
        }

        public bool SaveStockItem(StockMaster stockItem)
        {
            throw new NotImplementedException();
        }

        public bool DeleteStockItem(string stockNumber, string location)
        {
            throw new NotImplementedException();
        }

        public StockCostQuantity GetStockCostQuantity(string stockNumber, string location, DateTime purchaseDate)
        {
            throw new NotImplementedException();
        }

        public bool SaveStockCostQuantity(StockCostQuantity stockCostQuantity)
        {
            throw new NotImplementedException();
        }

        public bool DeleteStockCostQuantity(string stockNumber, string location, DateTime purchaseDate)
        {
            throw new NotImplementedException();
        }

        public void SetAdvancedFindDbContext()
        {
            
        }

        public void SetAdvancedFindLookupContext()
        {
            
        }
    }
}
