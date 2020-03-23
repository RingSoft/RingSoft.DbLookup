using RingSoft.DbLookup.App.Library.MegaDb.Model;

namespace RingSoft.DbLookup.App.Library.DevLogix
{
    public interface IDevLogixEfDataProcessor
    {
        StockMaster GetStockMaster(string stockNumber, string location);
    }
}
