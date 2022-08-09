using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.App.Library.MegaDb;
using RingSoft.DbLookup.App.Library.Northwind;

namespace RingSoft.DbLookup.App.Library
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
        IMegaDbEfDataProcessor MegaDbEfDataProcessor { get; set; }
    }
}
