using RingSoft.DbLookup.App.Library.EfCore.MegaDb;
using RingSoft.DbLookup.App.Library.EfCore.Northwind;
using RingSoft.DbLookup.App.Library.MegaDb;
using RingSoft.DbLookup.App.Library.Northwind;

namespace RingSoft.DbLookup.App.Library.EfCore
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
            RsDbLookupAppGlobals.UpdateGlobalsProgressStatus(GlobalsProgressStatus.InitNorthwind);
            NorthwindLookupContext = new NorthwindLookupContextEfCore();
            NorthwindEfDataProcessor = new NorthwindEfDataProcessorCore();

            RsDbLookupAppGlobals.UpdateGlobalsProgressStatus(GlobalsProgressStatus.ConnectingToNorthwind);
            NorthwindEfDataProcessor.GetProduct(1);

            RsDbLookupAppGlobals.UpdateGlobalsProgressStatus(GlobalsProgressStatus.InitMegaDb);
            MegaDbLookupContext = new MegaDbLookupContextEfCore();
            MegaDbEfDataProcessor = new MegaDbEfDataProcessorCore();

            var registrySettings = new RegistrySettings();
            if (registrySettings.MegaDbPlatformType != MegaDbPlatforms.None)
            {
                RsDbLookupAppGlobals.UpdateGlobalsProgressStatus(GlobalsProgressStatus.ConnectingToMegaDb);
                MegaDbEfDataProcessor.GetItem(1);
            }
        }
    }
}
