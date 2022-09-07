using Org.BouncyCastle.Crypto.Tls;
using RingSoft.DbLookup.AdvancedFind;

namespace RingSoft.DbLookup
{
    public enum AlertLevels
    {
        Green = 0,
        Yellow = 1,
        Red = 2,
    }

    public static class SystemGlobals
    {
        public static IAdvancedFindDbProcessor AdvancedFindDbProcessor { get; set; }

        public static IAdvancedFindLookupContext  AdvancedFindLookupContext { get; set; }

        public static AlertLevels WindowAlertLevel { get; set; }
    }
}
