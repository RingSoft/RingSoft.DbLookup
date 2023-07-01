using System;
using System.Collections.Generic;
using System.Linq;
using RingSoft.DbLookup.ModelDefinition;

namespace RingSoft.DbLookup.Lookup
{
    public class TestLookupDataBase<TEntity> : ILookupDataBase where TEntity : class, new()
    {
        public IQueryable<TEntity> TableToProcess { get; }

        public TableDefinition<TEntity> TableDefinition { get; }

        public event EventHandler<LookupDataMauiPrintOutput> PrintOutput;

        public TestLookupDataBase(IQueryable<TEntity> listToQuery, TableDefinition<TEntity> tableDefinition)
        {
            TableDefinition = tableDefinition;
            TableToProcess = listToQuery;
        }

        public int GetRecordCount()
        {
            return TableToProcess.Count();
        }

        public void DoPrintOutput(int pageSize)
        {
            var args = new LookupDataMauiPrintOutput();
            foreach (var entity in TableToProcess)
            {
                var primaryKey = TableDefinition.GetPrimaryKeyValueFromEntity(entity);
                args.Result.Add(primaryKey);
            }

            PrintOutput?.Invoke(this, args);
        }
    }
}
