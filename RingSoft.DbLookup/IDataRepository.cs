using System.Linq;

namespace RingSoft.DbLookup
{
    public interface IDataRepository
    {
        IDbContext GetDataContext();

        IQueryable<TEntity> GetTable<TEntity>() where TEntity : class;
    }
}
