using Microsoft.VisualStudio.TestTools.UnitTesting;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.App.Library;
using RingSoft.DbLookup.App.Library.EfCore;
using RingSoft.DbLookup.App.Library.MegaDb;
using RingSoft.DbLookup.App.Library.MegaDb.Model;
using RingSoft.DbLookup.App.Library.MegaDb.ViewModels;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.Tests.DbMaintenance;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.Tests
{
    [TestClass]
    public class ViewModelTests
    {
        private static TestMegaDbEfDataProcessor _testMegaDbDbProcessor = new TestMegaDbEfDataProcessor();
        private static IMegaDbLookupContext _lookupContext = RsDbLookupAppGlobals.EfProcessor.MegaDbLookupContext;

        [ClassInitialize]
        public static void Setup(TestContext testContext)
        {
            var testViewer = new TestGetDataErrorViewer();
            DbDataProcessor.UserInterface = testViewer;
            ControlsGlobals.UserInterface = testViewer;

            RsDbLookupAppGlobals.UnitTest = true;

            RsDbLookupAppGlobals.Initialize("UnitTests");
            RsDbLookupAppGlobals.EfProcessor = new EfProcessorCore();
            RsDbLookupAppGlobals.EfProcessor.MegaDbEfDataProcessor = _testMegaDbDbProcessor;
        }

        [TestMethod]
        public void AddItemToViewModel()
        {
            var itemViewModel = new ItemViewModel();
            itemViewModel.OnViewLoaded(new TestDbMaintenanceView());

            itemViewModel.UnitTestLoadFromEntity(_testMegaDbDbProcessor.GetItem(TestMegaDbEfDataProcessor.TestItemId1));
            var item = itemViewModel.UnitTestGetEntityData();
            Assert.AreEqual(TestMegaDbEfDataProcessor.TestItemId1, item.Id);

            itemViewModel.OnNewButton();
            var location = _testMegaDbDbProcessor.GetLocation(1);
            var manufacturer = _testMegaDbDbProcessor.GetManufacturer(1);

            itemViewModel.KeyAutoFillValue = new AutoFillValue(null, "New Item");
            itemViewModel.LocationAutoFillValue =
                new AutoFillValue(_lookupContext.Locations.GetPrimaryKeyValueFromEntity(location), location.Name);
            itemViewModel.ManufacturerAutoFillValue =
                new AutoFillValue(_lookupContext.Manufacturers.GetPrimaryKeyValueFromEntity(manufacturer),
                    manufacturer.Name);

            var result = itemViewModel.DoSave();
            Assert.AreEqual(DbMaintenanceResults.Success, result);
            Assert.AreEqual(3, _testMegaDbDbProcessor.Items.Count);
        }
    }
}
