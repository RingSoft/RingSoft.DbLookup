using Microsoft.VisualStudio.TestTools.UnitTesting;
using RingSoft.DbLookup.App.Library;
using RingSoft.DbLookup.App.Library.Ef6;
using RingSoft.DbLookup.DataProcessor;

namespace RingSoft.DbLookup.Tests.DbMaintenance
{
    [TestClass]
    public class Ef6ViewModelTests : DbMaintTestsBase
    {
        [ClassInitialize]
        public static void Setup(TestContext testContext)
        {
            DbDataProcessor.UserInterface = new TestGetDataErrorViewer();
            RsDbLookupAppGlobals.Initialize("UnitTests");
            RsDbLookupAppGlobals.EfProcessor = new EfProcessor6();
            SetupConfigurations();
        }

        [TestMethod]
        public void EF6_TestOrderViewModel_DbMaintenanceProcess()
        {
            TestOrderViewModel();
        }

        [TestMethod]
        public void EF6_TestCustomerViewModel_DbMaintenanceProcess()
        {
            TestCustomerViewModel();
        }

        [TestMethod]
        public void EF6_TestEmployeeViewModel_DbMaintenanceProcess()
        {
            TestEmployeeViewModel();
        }

        [TestMethod]
        public void EF6_TestItemViewModel_DbMaintenanceProcess()
        {
            TestItemViewModel();
        }
    }
}
