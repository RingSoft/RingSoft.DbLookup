using System.Threading;
using System.Threading.Tasks;
using RSDbLookupApp.Library.MegaDb.Model;

namespace RSDbLookupApp.Library.MegaDb
{
    public interface IMegaDbDbContext
    {
        Task<int> SaveChangesAsync(CancellationToken token);

        int SaveBatch();

        void AddItem(Item item);
    }
}
