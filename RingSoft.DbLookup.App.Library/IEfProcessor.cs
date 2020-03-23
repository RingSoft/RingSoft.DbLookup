using RSDbLookupApp.Library.MegaDb;
using RSDbLookupApp.Library.Northwind;

namespace RSDbLookupApp.Library
{

    public enum EntityFrameworkVersions
    {
        EntityFrameworkCore3 = 0,
        EntityFramework6 = 1
    }

    public interface IEfProcessor
    {
        EntityFrameworkVersions EntityFrameworkVersion { get; }
        INorthwindLookupContext NorthwindLookupContext { get; }
        INorthwindEfDataProcessor NorthwindEfDataProcessor { get; }
        IMegaDbLookupContext MegaDbLookupContext { get; }
        IMegaDbEfDataProcessor MegaDbEfDataProcessor { get; }
    }
}
