using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.Printing.Interop;
using System;

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
        public static IAdvancedFindDbProcessor AdvancedFindDbProcessor { get; internal set; }

        public static string UserName { get; set; }

        private static string _programDataFolder;

        public static string ProgramDataFolder
        {
            get => _programDataFolder;
            set
            {
                _programDataFolder = value;
                PrintingInteropGlobals.Initialize(_programDataFolder);
            }
        }

        private static IAdvancedFindLookupContext _context;

        public static IAdvancedFindLookupContext AdvancedFindLookupContext
        {
            get
            {
                if (_context == null)
                {
                    var message = $"Need To instantiate {nameof(LookupContextBase)}.";
                    throw new ApplicationException(message);
                }
                return _context;
            }
            internal set => _context = value;
        }

        private static IDataRepository _dataRepository;

        public static IDataRepository DataRepository
        {
            get
            {
                if (_dataRepository == null)
                {
                    var message = $"Need To implement and instantiate {nameof(SystemDataRepositoryBase)}.";
                    throw new ApplicationException(message);
                }
                return _dataRepository;
            }
            internal set => _dataRepository = value;
        }


        public static AlertLevels WindowAlertLevel { get; set; }

        public static bool ConvertAllDatesToUniversalTime { get; set; }

        public static DbDateTypes AllDatesFormat { get; set; } = DbDateTypes.DateOnly;

        private static LookupContextBase _lookupContext;
        public static LookupContextBase LookupContext
        {
            get
            {
                if (_lookupContext == null)
                {
                    throw new Exception($"Need to Need To implement and instantiate {nameof(LookupContextBase)} and run Initialize.");
                }
                return _lookupContext;
            }
            internal set => _lookupContext = value;
        }

        public static bool UnitTestMode { get; set; }

        public static bool ValidateDeletedData { get; set; } = true;
    }
}
