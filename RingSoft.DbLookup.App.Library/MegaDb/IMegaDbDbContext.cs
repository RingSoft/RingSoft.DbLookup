using System.Threading;
using System.Threading.Tasks;
using RingSoft.DbLookup.App.Library.MegaDb.Model;

namespace RingSoft.DbLookup.App.Library.MegaDb
{
    public interface IMegaDbDbContext
    {
        Task<int> SaveChangesAsync(CancellationToken token);

        int SaveBatch();

        void AddItem(Item item);
    }
}
