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
    }
}
