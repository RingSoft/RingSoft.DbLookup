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

        public static TestLookupContext Database { get; private set; }

        public static TestLookupControl LookupControl { get; private set; }

        public static LookupDataMaui<TimeClock> LookupData { get; private set; }

        [ClassInitialize]
        public static void Setup(TestContext testContext)
        {
            UserInterface = new TestUserInterface();
            ControlsGlobals.UserInterface = UserInterface;

            Database = new TestLookupContext(new TestDbContext());
            LookupControl = new TestLookupControl();
        }

        private void Reinitialize()
        {
            var lookupDefinition = Database.TimeClockLookup.Clone();
            var lookupDataBase = Database.TimeClockLookup.GetLookupDataMaui(lookupDefinition, true);
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
        }

    }
}
