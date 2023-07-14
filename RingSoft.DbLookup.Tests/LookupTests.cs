using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.Tests.Model;

namespace RingSoft.DbLookup.Tests
{
    [TestClass]
    public class LookupTests
    {
        public static TestUserInterface UserInterface { get; private set; }

        public static TestLookupContext LookupContext { get; private set; }

        public static TestLookupControl LookupControl { get; private set; }

        public static LookupDataMaui<TimeClock> LookupData { get; private set; }

        public static LookupDefinition<TimeClockLookup, TimeClock> LookupDefinition { get; private set; }

        [ClassInitialize]
        public static void Setup(TestContext testContext)
        {
            UserInterface = new TestUserInterface();
            ControlsGlobals.UserInterface = UserInterface;

            LookupContext = new TestLookupContext(new TestDbContext());
            LookupControl = new TestLookupControl();
        }

        private void Reinitialize()
        {
            LookupDefinition = LookupContext.TimeClockLookup.Clone();
            LookupDefinition.InitialOrderByField = LookupContext.TimeClocks.GetFieldDefinition(p => p.Id);
            var lookupDataBase = LookupContext.TimeClockLookup.GetLookupDataMaui(LookupDefinition, true);
            lookupDataBase.SetParentControls(LookupControl);
            if (lookupDataBase is LookupDataMaui<TimeClock> lookupDataTimeClock)
            {
                LookupData = lookupDataTimeClock;
            }
        }

        [TestMethod]
        public void TestGetInitData()
        {
            Reinitialize();
            LookupData.GetInitData();
            Assert.AreEqual(10, LookupData.CurrentList.Count);
            Assert.AreEqual(LookupScrollPositions.Top, LookupData.ScrollPosition);
            Assert.AreEqual(1, LookupData.CurrentList.FirstOrDefault().Id);
            Assert.AreEqual(10, LookupData.CurrentList.LastOrDefault().Id);
            
            LookupData.GotoNextRecord();
            Assert.AreEqual(10, LookupData.CurrentList.Count);
            Assert.AreEqual(LookupScrollPositions.Middle, LookupData.ScrollPosition);
            Assert.AreEqual(2, LookupData.CurrentList.FirstOrDefault().Id);
            Assert.AreEqual(11, LookupData.CurrentList.LastOrDefault().Id);
            //
            LookupData.GotoNextPage();
            Assert.AreEqual(10, LookupData.CurrentList.Count);
            Assert.AreEqual(LookupScrollPositions.Middle, LookupData.ScrollPosition);
            Assert.AreEqual(12, LookupData.CurrentList.FirstOrDefault().Id);
            Assert.AreEqual(21, LookupData.CurrentList.LastOrDefault().Id);

            LookupData.GotoNextRecord();
            Assert.AreEqual(10, LookupData.CurrentList.Count);
            Assert.AreEqual(LookupScrollPositions.Middle, LookupData.ScrollPosition);
            Assert.AreEqual(13, LookupData.CurrentList.FirstOrDefault().Id);
            Assert.AreEqual(22, LookupData.CurrentList.LastOrDefault().Id);

            LookupData.GotoPreviousPage();
            Assert.AreEqual(10, LookupData.CurrentList.Count);
            Assert.AreEqual(LookupScrollPositions.Middle, LookupData.ScrollPosition);
            Assert.AreEqual(3, LookupData.CurrentList.FirstOrDefault().Id);
            Assert.AreEqual(12, LookupData.CurrentList.LastOrDefault().Id);

            LookupData.GotoPreviousRecord();
            Assert.AreEqual(10, LookupData.CurrentList.Count);
            Assert.AreEqual(LookupScrollPositions.Middle, LookupData.ScrollPosition);
            Assert.AreEqual(2, LookupData.CurrentList.FirstOrDefault().Id);
            Assert.AreEqual(11, LookupData.CurrentList.LastOrDefault().Id);

            LookupData.GotoPreviousRecord();
            Assert.AreEqual(10, LookupData.CurrentList.Count);
            Assert.AreEqual(LookupScrollPositions.Top, LookupData.ScrollPosition);
            Assert.AreEqual(1, LookupData.CurrentList.FirstOrDefault().Id);
            Assert.AreEqual(10, LookupData.CurrentList.LastOrDefault().Id);

            LookupData.GotoBottom();
            Assert.AreEqual(10, LookupData.CurrentList.Count);
            Assert.AreEqual(LookupScrollPositions.Bottom, LookupData.ScrollPosition);
            Assert.AreEqual(91, LookupData.CurrentList.FirstOrDefault().Id);
            Assert.AreEqual(100, LookupData.CurrentList.LastOrDefault().Id);

            LookupData.GotoTop();
            Assert.AreEqual(10, LookupData.CurrentList.Count);
            Assert.AreEqual(LookupScrollPositions.Top, LookupData.ScrollPosition);
            Assert.AreEqual(1, LookupData.CurrentList.FirstOrDefault().Id);
            Assert.AreEqual(10, LookupData.CurrentList.LastOrDefault().Id);

            LookupData.OnSearchForChange("T-20");
            Assert.AreEqual(10, LookupData.CurrentList.Count);
            Assert.AreEqual(LookupScrollPositions.Middle, LookupData.ScrollPosition);
            Assert.AreEqual(16, LookupData.CurrentList.FirstOrDefault().Id);
            Assert.AreEqual(25, LookupData.CurrentList.LastOrDefault().Id);
        }

        [TestMethod]
        public void TestSortCustomer()
        {
            Reinitialize();
            LookupData.GetInitData();
            Assert.AreEqual(10, LookupData.CurrentList.Count);
            Assert.AreEqual(LookupScrollPositions.Top, LookupData.ScrollPosition);
            Assert.AreEqual(1, LookupData.CurrentList.FirstOrDefault().Id);
            Assert.AreEqual(10, LookupData.CurrentList.LastOrDefault().Id);

            var column = LookupDefinition.GetFieldColumnDefinition(p => p.CustomerName);
            LookupData.OnColumnClick(column, true);
            //Anna/Bruce
            Assert.AreEqual(10, LookupData.CurrentList.Count);
            Assert.AreEqual(LookupScrollPositions.Top, LookupData.ScrollPosition);
            Assert.AreEqual(1, LookupData.CurrentList.FirstOrDefault().Id);
            Assert.AreEqual(32, LookupData.CurrentList.LastOrDefault().Id);

            LookupData.GotoNextRecord();
            //Anna/Bruce
            Assert.AreEqual(10, LookupData.CurrentList.Count);
            Assert.AreEqual(LookupScrollPositions.Middle, LookupData.ScrollPosition);
            Assert.AreEqual(16, LookupData.CurrentList.FirstOrDefault().Id);
            Assert.AreEqual(47, LookupData.CurrentList.LastOrDefault().Id);
            
            LookupData.GotoNextPage();
            //Bruce/Charley/Dave
            Assert.AreEqual(10, LookupData.CurrentList.Count);
            Assert.AreEqual(LookupScrollPositions.Middle, LookupData.ScrollPosition);
            Assert.AreEqual(62, LookupData.CurrentList.FirstOrDefault().Id);
            Assert.AreEqual(18, LookupData.CurrentList.LastOrDefault().Id);
        }

    }
}
