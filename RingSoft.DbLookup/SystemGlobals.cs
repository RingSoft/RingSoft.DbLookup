using System;
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

        private static IAdvancedFindLookupContext _context;

        public static IAdvancedFindLookupContext AdvancedFindLookupContext
        {
            get
            {
                if (_context == null)
                {
                    throw new Exception("Need To Instantiate IAdvancedFindLookupContext.");
                }
                return _context;
            }
            set => _context = value;
        }

        public static AlertLevels WindowAlertLevel { get; set; }
    }
}
