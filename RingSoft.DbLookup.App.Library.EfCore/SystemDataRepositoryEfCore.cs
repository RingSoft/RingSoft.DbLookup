using System.Linq;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.EfCore;

namespace RingSoft.DbLookup.App.Library.EfCore
{
    public class SystemDataRepositoryEfCore : SystemDataRepository
    {

        public override IDbContext GetDataContext()
        {
            return EfCoreGlobals.DbAdvancedFindContextCore.GetNewDbContext();
        }

        public override IDbContext GetDataContext(DbDataProcessor dataProcessor)
        {
            return EfCoreGlobals.DbAdvancedFindContextCore.GetNewDbContext();
        }

        public IQueryable<TEntity> GetTable<TEntity>() where TEntity : class, new()
        {
            var context = EfCoreGlobals.DbAdvancedFindContextCore.GetNewDbContext();
            var dbSet = context.GetTable<TEntity>();
            return dbSet;
        }
    }
}
