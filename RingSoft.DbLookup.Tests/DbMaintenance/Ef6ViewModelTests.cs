using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RingSoft.DataEntryControls.Engine;
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
            //Necessary so .NET Core 3.x is compatible with Entity Framework 6.
            DbProviderFactories.RegisterFactory("System.Data.SqlClient", SqlClientFactory.Instance);

            var testViewer = new TestGetDataErrorViewer();
            DbDataProcessor.UserInterface = testViewer;
            ControlsGlobals.UserInterface = testViewer;

            RsDbLookupAppGlobals.UnitTest = true;
            
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
