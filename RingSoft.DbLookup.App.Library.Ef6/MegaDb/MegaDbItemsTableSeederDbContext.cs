using System;
using System.Threading;
using System.Threading.Tasks;
using RingSoft.DbLookup.App.Library.MegaDb;
using RingSoft.DbLookup.App.Library.MegaDb.Model;

namespace RingSoft.DbLookup.App.Library.Ef6.MegaDb
{
    public class MegaDbItemsTableSeederDbContext : IMegaDbDbContext
    {
        private MegaDbDbContextEf6 _context;

        public MegaDbItemsTableSeederDbContext()
        {
            _context = new MegaDbDbContextEf6();
        }
        public async Task<int> SaveChangesAsync(CancellationToken token)
        {
            int result;
            try
            {
                result = await _context.SaveChangesAsync(token);
            }
            catch (Exception)
            {
                result = 0;
            }
            return result;
        }

        public int SaveBatch()
        {
            var result = _context.SaveChanges();
            _context = new MegaDbDbContextEf6();
            return result;
        }

        public void AddItem(Item item)
        {
            _context.Items.Add(item);
        }
    }
}
