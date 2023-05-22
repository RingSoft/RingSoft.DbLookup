using System;
using System.Collections.Generic;
using System.Linq;

namespace RingSoft.DbLookup.Lookup
{
    public class TestLookupDataBase<TEntity> : ILookupDataBase where TEntity : class, new()
    {
        public IQueryable<TEntity> TableToProcess { get; }

        public event EventHandler<LookupDataChangedArgs> PrintDataChanged;

        public TestLookupDataBase(IQueryable<TEntity> listToQuery)
        {
            TableToProcess = listToQuery;
        }

        public int GetRecordCountWait()
        {
            return TableToProcess.Count();
        }

        public void GetPrintData()
        {
            var dataTable = TableToProcess.ConvertEnumerableToDataTable();

            var args = new LookupDataChangedArgs(dataTable, 0, LookupScrollPositions.Top);
            PrintDataChanged?.Invoke(this, args);
        }
    }
}
