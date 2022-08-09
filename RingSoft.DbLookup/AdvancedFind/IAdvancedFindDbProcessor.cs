using System.Runtime.InteropServices.ComTypes;

namespace RingSoft.DbLookup.AdvancedFind
{
    public interface IAdvancedFindDbProcessor
    {
        DbLookup.AdvancedFind.AdvancedFind GetAdvancedFind(int advancedFindId);
    }
}
