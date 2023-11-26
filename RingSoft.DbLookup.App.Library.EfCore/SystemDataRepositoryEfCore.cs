using RingSoft.DbLookup.App.Library.EfCore.MegaDb;
using RingSoft.DbLookup.App.Library.EfCore.Northwind;
using RingSoft.DbLookup.DataProcessor;

namespace RingSoft.DbLookup.App.Library.EfCore
{
    public enum DataRepositoryModes
    {
        Northwind = 1,
        MegaDb = 2,
    }

    public class SystemDataRepositoryEfCore : SystemDataRepository
    {
        public static DataRepositoryModes RepositoryMode { get; set; }

        public override IDbContext GetDataContext()
        {
            if (RepositoryMode == DataRepositoryModes.Northwind)
            {
                return new NorthwindDbContextEfCore();
            }

            return new MegaDbDbContextEfCore();
        }

        public override IDbContext GetDataContext(DbDataProcessor dataProcessor)
        {
            return GetDataContext();
        }
    }
}
