using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.RecordLocking;
using System.Collections.Generic;
using System.Linq;

namespace RingSoft.DbLookup
{
    public abstract class SystemDataRepository : IAdvancedFindDbProcessor
    {
        public SystemDataRepository()
        {
            Initialize();
        }

        public void Initialize()
        {
            SystemGlobals.AdvancedFindDbProcessor = this;
            SystemGlobals.DataRepository = this;
        }

        public AdvancedFind.AdvancedFind GetAdvancedFind(int advancedFindId)
        {
            var advFind = new AdvancedFind.AdvancedFind
            {
                Id = advancedFindId,
            };
            return advFind.FillOutProperties(true);
        }

        public bool SaveAdvancedFind(AdvancedFind.AdvancedFind advancedFind, List<AdvancedFindColumn> columns,
            List<AdvancedFindFilter> filters)
        {
            var result = true;
            var context = GetDataContext();
            if (context.SaveEntity(advancedFind, $"Saving Advanced Find '{advancedFind.Name}.'"))
            {
                var columnsQuery = context.GetTable<AdvancedFindColumn>();
                var oldColumns = columnsQuery.Where(p => p.AdvancedFindId == advancedFind.Id);

                foreach (var advancedFindColumn in columns)
                {
                    advancedFindColumn.AdvancedFindId = advancedFind.Id;
                }
                context.RemoveRange(oldColumns);
                context.AddRange(columns);

                var filtersQuery = context.GetTable<AdvancedFindFilter>();
                var oldFilters = filtersQuery.Where(
                    p => p.AdvancedFindId == advancedFind.Id);

                foreach (var advancedFindFilter in filters)
                {
                    advancedFindFilter.AdvancedFindId = advancedFind.Id;
                }
                context.RemoveRange(oldFilters);
                context.AddRange(filters);

                result = context.Commit($"Saving Advanced Find '{advancedFind.Name}' Details");
            }
            return result;
        }

        public bool DeleteAdvancedFind(int advancedFindId)
        {
            var context = SystemGlobals.DataRepository.GetDataContext();
            var query = context.GetTable<AdvancedFind.AdvancedFind>();
            var advancedFind = query.FirstOrDefault(p => p.Id == advancedFindId);
            if (advancedFind != null)
            {
                var columnsQuery = context.GetTable<AdvancedFindColumn>();
                var oldColumns = columnsQuery.Where(
                    p => p.AdvancedFindId == advancedFindId);


                var filtersQuery = context.GetTable<AdvancedFindFilter>();
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
            var context = SystemGlobals.DataRepository.GetDataContext();
            var query = context.GetTable<RecordLock>();
            return query.FirstOrDefault(p => p.Table == table && p.PrimaryKey == primaryKey);
        }

        public abstract IDbContext GetDataContext();

        public abstract IDbContext GetDataContext(DbDataProcessor dataProcessor);
    }
}
