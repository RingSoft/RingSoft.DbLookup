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

        public static TestDatabase Database { get; private set; }

        public static LookupDataMaui<Customer> LookupData { get; private set; }

        public static TestLookupControl LookupControl { get; private set; }

        [ClassInitialize]
        public static void Setup(TestContext testContext)
        {
            UserInterface = new TestUserInterface();
            ControlsGlobals.UserInterface = UserInterface;

            Database = new TestDatabase(new TestDbContext());
            LookupControl = new TestLookupControl();

            var lookupDefinition = Database.CustomerLookup.Clone();
            var lookupData = Database.CustomerLookup.GetLookupDataMaui(lookupDefinition, true);
            if (lookupData is LookupDataMaui<Customer> lookupDataCustomer)
            {
                LookupData = lookupDataCustomer;
            }
            LookupData.SetParentControls(LookupControl);
        }

        [TestMethod]
        public void TestGetInitData()
        {
            LookupData.GetInitData();
            Assert.AreEqual(1, LookupData.CurrentList.Count);
        }

    }
}
