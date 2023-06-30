using System.Linq;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.Lookup;

namespace RingSoft.DbLookup
{
    public interface IDataRepository
    {
        IDbContext GetDataContext();

        //ILookupDataBase GetLookupDataBase<TEntity>(LookupDefinitionBase lookupDefinition, LookupUserInterface lookupUi)
        //where TEntity : class, new();

        IDbContext GetDataContext(DbDataProcessor  dataProcessor);
    }
}
