using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.App.Library.EfCore.MegaDb;
using RingSoft.DbLookup.App.Library.EfCore.Northwind;
using RingSoft.DbLookup.App.Library.MegaDb;
using RingSoft.DbLookup.App.Library.Northwind;
using RingSoft.DbLookup.EfCore;

namespace RingSoft.DbLookup.App.Library.EfCore
{
    public class EfProcessorCore : IEfProcessor
    {
        public EntityFrameworkVersions EntityFrameworkVersion => EntityFrameworkVersions.EntityFrameworkCore3;
        public INorthwindLookupContext NorthwindLookupContext { get; }
        public INorthwindEfDataProcessor NorthwindEfDataProcessor { get; }
        public IMegaDbLookupContext MegaDbLookupContext { get; }
        public IMegaDbEfDataProcessor MegaDbEfDataProcessor { get; set; }

        public EfProcessorCore()
        {
            RsDbLookupAppGlobals.UpdateGlobalsProgressStatus(GlobalsProgressStatus.InitNorthwind);
            NorthwindLookupContext = new NorthwindLookupContextEfCore();
            NorthwindLookupContext.LookupContext.Initialize();
            RsDbLookupAppGlobals.EfProcessor = this;
            var dataRepository = new SystemDataRepositoryEfCore();
            NorthwindEfDataProcessor = new NorthwindEfDataProcessorCore();
            if (!RsDbLookupAppGlobals.UnitTest)
            {
                NorthwindEfDataProcessor.CheckDataExists();
            }

            if (!RsDbLookupAppGlobals.UnitTest)
            {
                RsDbLookupAppGlobals.UpdateGlobalsProgressStatus(GlobalsProgressStatus.ConnectingToNorthwind);

                if (NorthwindLookupContext.NorthwindContextConfiguration.TestConnection())
                    RsDbLookupAppGlobals.ConnectToNorthwind(NorthwindEfDataProcessor, NorthwindLookupContext);
            }

            RsDbLookupAppGlobals.UpdateGlobalsProgressStatus(GlobalsProgressStatus.InitMegaDb);
            MegaDbLookupContext = new MegaDbLookupContextEfCore();
            MegaDbLookupContext.LookupContext.Initialize();
            MegaDbEfDataProcessor = new MegaDbEfDataProcessorCore();

            var registrySettings = new RegistrySettings();
            if (registrySettings.MegaDbPlatformType != MegaDbPlatforms.None && !RsDbLookupAppGlobals.UnitTest)
            {
                RsDbLookupAppGlobals.UpdateGlobalsProgressStatus(GlobalsProgressStatus.ConnectingToMegaDb);
                if (MegaDbLookupContext.MegaDbContextConfiguration.TestConnection())
                    RsDbLookupAppGlobals.ConnectToMegaDb(MegaDbEfDataProcessor, MegaDbLookupContext);
            }
        }
    }
}
