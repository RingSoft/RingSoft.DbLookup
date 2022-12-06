using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.RecordLocking;

namespace RingSoft.DbLookup.EfCore
{
    public class AdvancedFindDataProcessorEfCore : IAdvancedFindDbProcessor, IDataRepository
    {
        public static void ConfigureAdvancedFind(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new RecordLockConfiguration());
            modelBuilder.ApplyConfiguration(new AdvancedFindConfiguration());
            modelBuilder.ApplyConfiguration(new AdvancedFindColumnConfiguration());
            modelBuilder.ApplyConfiguration(new AdvancedFindFilterConfiguration());

        }

        public AdvancedFind.AdvancedFind GetAdvancedFind(int advancedFindId)
        {
            IQueryable<AdvancedFind.AdvancedFind> context =
                SystemGlobals.DataRepository.GetTable<AdvancedFind.AdvancedFind>();
            return context.Include(p => p.Columns)
                .Include(p => p.Filters)
                .FirstOrDefault(p => p.Id == advancedFindId);
        }

        public bool SaveAdvancedFind(AdvancedFind.AdvancedFind advancedFind, List<AdvancedFindColumn> columns,
            List<AdvancedFindFilter> filters)
        {
            var result = true;
            var context = GetDataContext();
            if (context.SaveEntity(advancedFind, $"Saving Advanced Find '{advancedFind.Name}.'"))
            {
                var columnsQuery = GetTable<AdvancedFindColumn>();
                var oldColumns = columnsQuery.Where(p => p.AdvancedFindId == advancedFind.Id);
                
                foreach (var advancedFindColumn in columns)
                {
                    advancedFindColumn.AdvancedFindId = advancedFind.Id;
                }
                
                var filtersQuery = GetTable<AdvancedFindFilter>();
                var oldFilters = filtersQuery.Where(
                    p => p.AdvancedFindId == advancedFind.Id);
                
                foreach (var advancedFindFilter in filters)
                {
                    advancedFindFilter.AdvancedFindId = advancedFind.Id;
                }
                context.RemoveRange(oldColumns);
                context.RemoveRange(oldFilters);
                context.AddRange(columns);
                context.AddRange(filters);

                result = context.Commit($"Saving Advanced Find '{advancedFind.Name}' Details");
            }
            return result;
        }

        public bool DeleteAdvancedFind(int advancedFindId)
        {
            var context = SystemGlobals.DataRepository.GetDataContext();
            var query = SystemGlobals.DataRepository.GetTable<AdvancedFind.AdvancedFind>();
            var advancedFind = query.FirstOrDefault(p => p.Id == advancedFindId);
            if (advancedFind != null)
            {
                var columnsQuery = SystemGlobals.DataRepository.GetTable<AdvancedFindColumn>();
                var oldColumns = columnsQuery.Where(
                    p => p.AdvancedFindId == advancedFindId);
                

                var filtersQuery = SystemGlobals.DataRepository.GetTable<AdvancedFindFilter>();
                var oldFilters = filtersQuery.Where(
                    p => p.AdvancedFindId == advancedFindId);

                context.RemoveRange(oldColumns);
                context.RemoveRange(oldFilters);

                if (context.DeleteNoCommitEntity(advancedFind, $"Deleting Advanced Find '{advancedFind.Name}'."))
                {
                    return context.Commit($"Deleting Advanced Find '{advancedFind.Name}'.");
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        public RecordLock GetRecordLock(string table, string primaryKey)
        {
            var query = SystemGlobals.DataRepository.GetTable<RecordLock>();
            return query.FirstOrDefault(p => p.Table == table && p.PrimaryKey == primaryKey);
        }

        public IDbContext GetDataContext()
        {
            return EfCoreGlobals.DbAdvancedFindContextCore.GetNewDbContext();
        }

        public IQueryable<TEntity> GetTable<TEntity>() where TEntity : class
        {
            var context = EfCoreGlobals.DbAdvancedFindContextCore.GetNewDbContext();
            var dbSet = context.GetDbContextEf().Set<TEntity>();
            return dbSet;
        }
    }
}
