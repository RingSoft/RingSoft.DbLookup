using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.RecordLocking;

namespace RingSoft.DbLookup.EfCore
{
    public class SystemDataRepositoryEfCore : SystemDataRepositoryBase
    {
        public static void ConfigureAdvancedFind(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new RecordLockConfiguration());
            modelBuilder.ApplyConfiguration(new AdvancedFindConfiguration());
            modelBuilder.ApplyConfiguration(new AdvancedFindColumnConfiguration());
            modelBuilder.ApplyConfiguration(new AdvancedFindFilterConfiguration());

        }

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
