using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RingSoft.DbLookup.App.Library.EfCore;
using RingSoft.DbLookup.App.Library;
using RingSoft.DbLookup.App.Library.Northwind.Model;
using RingSoft.DbLookup.App.Library.Northwind.ViewModels;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.RecordLocking;
using RingSoft.DbLookup.Testing;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.Tests.ViewModelTests
{
    [TestClass]
    public class CustomerViewModelTests
    {
        public static TestGlobals<CustomerViewModel, TestDbMaintenanceView> Globals { get; private set; }

        [ClassInitialize]
        public static void Setup(TestContext testContext)
        {
            Globals = new TestGlobals<CustomerViewModel, TestDbMaintenanceView>();
            RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.SetAdvancedFind();
            Globals.Initialize();
        }

        [TestMethod]
        public void TestCustomerSave()
        {
            Globals.ClearData();
            Assert.AreEqual(false, Globals.ViewModel.RecordDirty);

            Globals.ViewModel.KeyAutoFillValue = new AutoFillValue(
                new PrimaryKeyValue(Globals.ViewModel.TableDefinition), "TEST");

            Assert.AreEqual(true, Globals.ViewModel.RecordDirty);
            Globals.ViewModel.CompanyName = "Company";

            Globals.ViewModel.SaveCommand.Execute(null);
            Globals.ViewModel.SaveCommand.Execute(null);
            Globals.ViewModel.SaveCommand.Execute(null);

            var context = SystemGlobals.DataRepository.GetDataContext();
            var table = context.GetTable<Customer>();
            Assert.AreEqual(1, table.Count());

            var recordLocksTable = context.GetTable<RecordLock>();
            Assert.AreEqual(1, recordLocksTable.Count());

            Globals.ViewModel.CompanyName = "Company1";

            Globals.ViewModel.SaveCommand.Execute(null);
            table = context.GetTable<Customer>();
            Assert.AreEqual(1, table.Count());
        }
    }
}
