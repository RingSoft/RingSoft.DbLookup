using RingSoft.DbLookup.App.Library.Ef6.MegaDb;
using RingSoft.DbLookup.App.Library.Ef6.Northwind;
using RingSoft.DbLookup.App.Library.MegaDb;
using RingSoft.DbLookup.App.Library.Northwind;

namespace RingSoft.DbLookup.App.Library.Ef6
{
    public class EfProcessor6 : IEfProcessor
    {
        public EntityFrameworkVersions EntityFrameworkVersion => EntityFrameworkVersions.EntityFramework6;
        public INorthwindLookupContext NorthwindLookupContext { get; }
        public INorthwindEfDataProcessor NorthwindEfDataProcessor { get; }
        public IMegaDbLookupContext MegaDbLookupContext { get; }
        public IMegaDbEfDataProcessor MegaDbEfDataProcessor { get; }

        public EfProcessor6()
        {
            RsDbLookupAppGlobals.UpdateGlobalsProgressStatus(GlobalsProgressStatus.InitNorthwind);
            NorthwindLookupContext = new NorthwindLookupContextEf6();
            NorthwindEfDataProcessor = new NorthwindEfDataProcessor6();
            
            RsDbLookupAppGlobals.UpdateGlobalsProgressStatus(GlobalsProgressStatus.ConnectingToNorthwind);

            if (!RsDbLookupAppGlobals.UnitTest)
                RsDbLookupAppGlobals.ConnectToNorthwind(NorthwindEfDataProcessor, NorthwindLookupContext);

            RsDbLookupAppGlobals.UpdateGlobalsProgressStatus(GlobalsProgressStatus.InitMegaDb);
            MegaDbLookupContext = new MegaDbLookupContextEf6();
            MegaDbEfDataProcessor = new MegaDbEfDataProcessor6();

            var registrySettings = new RegistrySettings();
            if (registrySettings.MegaDbPlatformType != MegaDbPlatforms.None)
            {
                RsDbLookupAppGlobals.UpdateGlobalsProgressStatus(GlobalsProgressStatus.ConnectingToMegaDb);

                if (!RsDbLookupAppGlobals.UnitTest)
                    RsDbLookupAppGlobals.ConnectToMegaDb(MegaDbEfDataProcessor, MegaDbLookupContext);
            }
        }
    }
}
