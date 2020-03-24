using RingSoft.DbLookup.App.Library;
using RingSoft.DbLookup.App.Library.MegaDb;
using RingSoft.DbLookup.App.Library.Northwind;
using RSDbLookupApp.Library.EfCore.MegaDb;
using RSDbLookupApp.Library.EfCore.Northwind;

namespace RSDbLookupApp.Library.EfCore
{
    public class EfProcessorCore : IEfProcessor
    {
        public EntityFrameworkVersions EntityFrameworkVersion => EntityFrameworkVersions.EntityFrameworkCore3;
        public INorthwindLookupContext NorthwindLookupContext { get; }
        public INorthwindEfDataProcessor NorthwindEfDataProcessor { get; }
        public IMegaDbLookupContext MegaDbLookupContext { get; }
        public IMegaDbEfDataProcessor MegaDbEfDataProcessor { get; }

        public EfProcessorCore()
        {
            RsDbLookupAppGlobals.UpdateGlobalsProgressStatus(GlobalsProgressStatus.Northwind);
            NorthwindLookupContext = new NorthwindLookupContextEfCore();
            NorthwindEfDataProcessor = new NorthwindEfDataProcessorCore();

            RsDbLookupAppGlobals.UpdateGlobalsProgressStatus(GlobalsProgressStatus.MegaDb);
            MegaDbLookupContext = new MegaDbLookupContextEfCore();
            MegaDbEfDataProcessor = new MegaDbEfDataProcessorCore();
        }
    }
}
