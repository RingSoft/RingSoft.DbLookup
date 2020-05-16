using Microsoft.VisualStudio.TestTools.UnitTesting;
using RingSoft.DbLookup.App.Library;
using RingSoft.DbLookup.App.Library.DevLogix;
using RingSoft.DbLookup.App.Library.EfCore.DevLogix;
using RingSoft.DbLookup.App.Library.LibLookupContext;
using RingSoft.DbLookup.DataProcessor;

namespace RingSoft.DbLookup.Tests.DevLogixLookups
{
    [TestClass]
    public class DevLogixSqlServerLookupTests : DevLogixLookupTestsBase
    {
        protected override IDevLogixLookupContext LookupContext => _context;

        private static DevLogixLookupContextEfCore _context;

        [ClassInitialize]
        public static void Setup(TestContext testContext)
        {
            DbDataProcessor.UserInterface = new TestGetDataErrorViewer();
            RsDbLookupAppGlobals.Initialize("UnitTests");
            _context = new DevLogixLookupContextEfCore()
            {
                DataProcessorType = DataProcessorTypes.SqlServer
            };
        }

        [TestMethod]
        public void SqlServer_ErrorsLookup_SortByErrorNumber_Ascending()
        {
            Base_ErrorsLookupSortByErrorNumber_Ascending();
        }

        [TestMethod]
        public void SqlServer_ErrorsLookup_SortByErrorNumber_Descending()
        {
            Base_ErrorsLookupSortByErrorNumber_Descending();
        }

        [TestMethod]
        public void SqlServer_ErrorsLookup_SortByDate_Ascending()
        {
            Base_ErrorsLookupSortByDate_Ascending();
        }

        [TestMethod]
        public void SqlServer_ErrorsLookup_SortByDate_Descending()
        {
            Base_ErrorsLookupSortByDate_Descending();
        }

        [TestMethod]
        public void SqlServer_ErrorsLookup_SortByDeveloperType_Ascending()
        {
            Base_ErrorsLookupSortByDeveloperType_Ascending();
        }

        [TestMethod]
        public void SqlServer_ErrorsLookup_SortByDeveloperType_Descending()
        {
            Base_ErrorsLookupSortByDeveloperType_Descending();
        }

        [TestMethod]
        public void SqlServer_ErrorsLookup_SortByHoursSpent_Ascending()
        {
            Base_ErrorsLookupSortByHoursSpent_Ascending();
        }

        [TestMethod]
        public void SqlServer_ErrorsLookup_SortByHoursSpent_Descending()
        {
            Base_ErrorsLookupSortByHoursSpent_Descending();
        }

        //Issues Lookup---------------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void SqlServer_IssuesLookup_SortByTask_Ascending()
        {
            Base_IssuesLookupSortByTask_Ascending();
        }

        [TestMethod]
        public void SqlServer_IssuesLookup_SortByTask_Descending()
        {
            Base_IssuesLookupSortByTask_Descending();
        }

        [TestMethod]
        public void SqlServer_IssuesLookup_SortByResolved_Ascending()
        {
            Base_IssuesLookupSortByResolved_Ascending();
        }

        [TestMethod]
        public void SqlServer_IssuesLookup_SortByResolved_Descending()
        {
            Base_IssuesLookupSortByResolved_Descending();
        }
    }
}
