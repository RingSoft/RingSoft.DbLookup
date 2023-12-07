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
