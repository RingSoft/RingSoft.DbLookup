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

        public static TestLookupControl LookupControl { get; private set; }

        public static LookupDataMaui<Customer> LookupData { get; private set; }

        [ClassInitialize]
        public static void Setup(TestContext testContext)
        {
            UserInterface = new TestUserInterface();
            ControlsGlobals.UserInterface = UserInterface;

            Database = new TestDatabase(new TestDbContext());
            LookupControl = new TestLookupControl();
        }

        private void Reinitialize()
        {
            var lookupDefinition = Database.CustomerLookup.Clone();
            var lookupDataBase = Database.CustomerLookup.GetLookupDataMaui(lookupDefinition, true);
            lookupDataBase.SetParentControls(LookupControl);
            if (lookupDataBase is LookupDataMaui<Customer> lookupDataCustomer)
            {
                LookupData = lookupDataCustomer;
            }
        }

        [TestMethod]
        public void TestGetInitData()
        {
            Reinitialize();
            LookupData.GetInitData();
            Assert.AreEqual(1, LookupData.CurrentList.Count);
        }

    }
}
