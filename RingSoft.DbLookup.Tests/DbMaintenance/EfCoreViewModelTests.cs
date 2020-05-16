﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using RingSoft.DbLookup.App.Library;
using RingSoft.DbLookup.App.Library.EfCore;
using RingSoft.DbLookup.DataProcessor;

namespace RingSoft.DbLookup.Tests.DbMaintenance
{
    [TestClass]
    public class EfCoreViewModelTests : DbMaintTestsBase
    {
        [ClassInitialize]
        public static void Setup(TestContext testContext)
        {
            DbDataProcessor.UserInterface = new TestGetDataErrorViewer();
            RsDbLookupAppGlobals.Initialize("UnitTests");
            RsDbLookupAppGlobals.EfProcessor = new EfProcessorCore();
            SetupConfigurations();
        }

        [TestMethod]
        public void EFCore_TestOrderViewModel_DbMaintenanceProcess()
        {
            TestOrderViewModel();
        }

        [TestMethod]
        public void EFCore_TestCustomerViewModel_DbMaintenanceProcess()
        {
            TestCustomerViewModel();
        }

        [TestMethod]
        public void EFCore_TestEmployeeViewModel_DbMaintenanceProcess()
        {
            TestEmployeeViewModel();
        }

        [TestMethod]
        public void EFCore_TestItemViewModel_DbMaintenanceProcess()
        {
            TestItemViewModel();
        }
    }
}
