using RSDbLookupApp.Library.DevLogix.Model;
using RSDbLookupApp.Library.MegaDb.Model;

namespace RSDbLookupApp.Library.DevLogix
{
    public interface IDevLogixEfDataProcessor
    {
        StockMaster GetStockMaster(string stockNumber, string location);
    }
}
