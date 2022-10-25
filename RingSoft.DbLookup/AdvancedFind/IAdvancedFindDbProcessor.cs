using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using RingSoft.DbLookup.RecordLocking;

namespace RingSoft.DbLookup.AdvancedFind
{
    public interface IAdvancedFindDbProcessor
    {
        DbLookup.AdvancedFind.AdvancedFind GetAdvancedFind(int advancedFindId);

        bool SaveAdvancedFind(DbLookup.AdvancedFind.AdvancedFind advancedFind, List<AdvancedFindColumn> columns,
            List<AdvancedFindFilter> filters);

        bool DeleteAdvancedFind(int advancedFindId);

        RecordLock GetRecordLock(string table, string primaryKey);
    }
}
