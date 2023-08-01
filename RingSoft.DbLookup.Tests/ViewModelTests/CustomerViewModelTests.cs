using Microsoft.VisualStudio.TestTools.UnitTesting;
using RingSoft.DbLookup.App.Library.EfCore;
using RingSoft.DbLookup.App.Library;
using RingSoft.DbLookup.App.Library.Northwind.ViewModels;
using RingSoft.DbLookup.Testing;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.Tests.ViewModelTests
{
    [TestClass]
    public class CustomerViewModelTests
    {
        public static TestGlobals<CustomerViewModel, TestDbMaintenanceView> Globals { get; } =
            new TestGlobals<CustomerViewModel, TestDbMaintenanceView>();

        [ClassInitialize]
        public static void Setup(TestContext testContext)
        {
            Globals.Initialize();
        }

        [TestMethod]
        public void TestCustomerSave()
        {
            Globals.ClearData();
            Assert.AreEqual(false, Globals.ViewModel.RecordDirty);
        }
    }
}
