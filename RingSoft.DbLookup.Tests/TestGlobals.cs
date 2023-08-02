using RingSoft.DbLookup.App.Library.EfCore;
using RingSoft.DbLookup.App.Library;
using RingSoft.DbLookup.Testing;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.Tests
{
    public class TestGlobals<TViewModel, TView> : DbMaintenanceTestGlobals<TViewModel, TView>
        where TViewModel : DbMaintenanceViewModelBase
        where TView : IDbMaintenanceView, new()
    {
        public TestGlobals() : base(new DbLookupAppTestDataRepository(new DataRepositoryRegistry()))
        {
            RsDbLookupAppGlobals.EfProcessor = new EfProcessorCore();
            SystemGlobals.DataRepository = DataRepository;
        }
    }
}
