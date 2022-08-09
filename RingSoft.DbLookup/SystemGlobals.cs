using RingSoft.DbLookup.AdvancedFind;

namespace RingSoft.DbLookup
{
    public static class SystemGlobals
    {
        public static IAdvancedFindDbProcessor AdvancedFindDbProcessor { get; set; }

        public static IAdvancedFindLookupContext  AdvancedFindLookupContext { get; set; }
    }
}
