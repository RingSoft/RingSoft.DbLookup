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

        public static string UserName { get; set; }

        public static string ProgramDataFolder { get; set; }

        private static IAdvancedFindLookupContext _context;

        public static IAdvancedFindLookupContext AdvancedFindLookupContext
        {
            get
            {
                if (_context == null)
                {
                    var message = $"Need To Instantiate {nameof(IAdvancedFindLookupContext)}.";
                    throw new ApplicationException(message);
                }
                return _context;
            }
            set => _context = value;
        }

        private static IDataRepository _dataRepository;

        public static IDataRepository DataRepository
        {
            get
            {
                if (_dataRepository == null)
                {
                    var message = $"Need To Instantiate {nameof(IDataRepository)}.";
                    throw new ApplicationException(message);
                }
                return _dataRepository;
            }
            set => _dataRepository = value;
        }


        public static AlertLevels WindowAlertLevel { get; set; }
    }
}
