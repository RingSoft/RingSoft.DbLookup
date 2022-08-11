using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;

namespace RingSoft.DbLookup.AdvancedFind
{
    public interface IAdvancedFindDbProcessor
    {
        DbLookup.AdvancedFind.AdvancedFind GetAdvancedFind(int advancedFindId);

        bool SaveAdvancedFind(DbLookup.AdvancedFind.AdvancedFind advancedFind, List<AdvancedFindColumn> columns,
            List<AdvancedFindFilter> filters);

    }
}
