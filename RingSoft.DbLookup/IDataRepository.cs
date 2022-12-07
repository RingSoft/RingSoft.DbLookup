using System.Linq;

namespace RingSoft.DbLookup
{
    public interface IDataRepository
    {
        IDbContext GetDataContext();
    }
}
