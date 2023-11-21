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
            SystemGlobals.LookupContext = LookupContext;
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
        public void TestGetInitDataAsc()
        {
            Reinitialize();
            LookupData.GetInitData();
            Assert.AreEqual(10, LookupData.CurrentList.Count);
            Assert.AreEqual(LookupScrollPositions.Top, LookupData.ScrollPosition);

            Assert.AreEqual(1, LookupData.CurrentList.FirstOrDefault().Id);
            Assert.AreEqual(10, LookupData.CurrentList.LastOrDefault().Id);
            //
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
            //
            LookupData.GotoNextRecord();
            Assert.AreEqual(10, LookupData.CurrentList.Count);
            Assert.AreEqual(LookupScrollPositions.Middle, LookupData.ScrollPosition);

            Assert.AreEqual(13, LookupData.CurrentList.FirstOrDefault().Id);
            Assert.AreEqual(22, LookupData.CurrentList.LastOrDefault().Id);
            //
            LookupData.GotoPreviousPage();
            Assert.AreEqual(10, LookupData.CurrentList.Count);
            Assert.AreEqual(LookupScrollPositions.Middle, LookupData.ScrollPosition);

            Assert.AreEqual(3, LookupData.CurrentList.FirstOrDefault().Id);
            Assert.AreEqual(12, LookupData.CurrentList.LastOrDefault().Id);
            //
            LookupData.GotoPreviousRecord();
            Assert.AreEqual(10, LookupData.CurrentList.Count);
            Assert.AreEqual(LookupScrollPositions.Middle, LookupData.ScrollPosition);

            Assert.AreEqual(2, LookupData.CurrentList.FirstOrDefault().Id);
            Assert.AreEqual(11, LookupData.CurrentList.LastOrDefault().Id);
            //
            LookupData.GotoPreviousRecord();
            Assert.AreEqual(10, LookupData.CurrentList.Count);
            Assert.AreEqual(LookupScrollPositions.Top, LookupData.ScrollPosition);

            Assert.AreEqual(1, LookupData.CurrentList.FirstOrDefault().Id);
            Assert.AreEqual(10, LookupData.CurrentList.LastOrDefault().Id);
            //
            LookupData.GotoBottom();
            Assert.AreEqual(10, LookupData.CurrentList.Count);
            Assert.AreEqual(LookupScrollPositions.Bottom, LookupData.ScrollPosition);

            Assert.AreEqual(91, LookupData.CurrentList.FirstOrDefault().Id);
            Assert.AreEqual(100, LookupData.CurrentList.LastOrDefault().Id);
            //
            LookupData.GotoTop();
            Assert.AreEqual(10, LookupData.CurrentList.Count);
            Assert.AreEqual(LookupScrollPositions.Top, LookupData.ScrollPosition);

            Assert.AreEqual(1, LookupData.CurrentList.FirstOrDefault().Id);
            Assert.AreEqual(10, LookupData.CurrentList.LastOrDefault().Id);
            //
            LookupData.OnSearchForChange("T-20");
            Assert.AreEqual(10, LookupData.CurrentList.Count);
            Assert.AreEqual(LookupScrollPositions.Middle, LookupData.ScrollPosition);

            Assert.AreEqual(16, LookupData.CurrentList.FirstOrDefault().Id);
            Assert.AreEqual(25, LookupData.CurrentList.LastOrDefault().Id);
        }

        [TestMethod]
        public void TestGetInitDataDesc()
        {
            Reinitialize();
            LookupData.GetInitData();
            Assert.AreEqual(10, LookupData.CurrentList.Count);
            Assert.AreEqual(LookupScrollPositions.Top, LookupData.ScrollPosition);

            Assert.AreEqual(1, LookupData.CurrentList.FirstOrDefault().Id);
            Assert.AreEqual(10, LookupData.CurrentList.LastOrDefault().Id);

            var column = LookupDefinition.GetFieldColumnDefinition(p => p.TimeClockId);
            LookupData.OnColumnClick(column, true);

            Assert.AreEqual(10, LookupData.CurrentList.Count);
            Assert.AreEqual(LookupScrollPositions.Top, LookupData.ScrollPosition);

            Assert.AreEqual(99, LookupData.CurrentList.FirstOrDefault().Id);
            Assert.AreEqual(90, LookupData.CurrentList.LastOrDefault().Id);

            LookupData.GotoNextRecord();
            Assert.AreEqual(10, LookupData.CurrentList.Count);
            Assert.AreEqual(LookupScrollPositions.Middle, LookupData.ScrollPosition);

            Assert.AreEqual(98, LookupData.CurrentList.FirstOrDefault().Id);
            Assert.AreEqual(9, LookupData.CurrentList.LastOrDefault().Id);

            LookupData.GotoNextPage();
            Assert.AreEqual(10, LookupData.CurrentList.Count);
            Assert.AreEqual(LookupScrollPositions.Middle, LookupData.ScrollPosition);

            Assert.AreEqual(89, LookupData.CurrentList.FirstOrDefault().Id);
            Assert.AreEqual(80, LookupData.CurrentList.LastOrDefault().Id);

            LookupData.GotoNextRecord();
            Assert.AreEqual(10, LookupData.CurrentList.Count);
            Assert.AreEqual(LookupScrollPositions.Middle, LookupData.ScrollPosition);

            Assert.AreEqual(88, LookupData.CurrentList.FirstOrDefault().Id);
            Assert.AreEqual(8, LookupData.CurrentList.LastOrDefault().Id);

            LookupData.GotoPreviousPage();
            Assert.AreEqual(10, LookupData.CurrentList.Count);
            Assert.AreEqual(LookupScrollPositions.Middle, LookupData.ScrollPosition);

            Assert.AreEqual(97, LookupData.CurrentList.FirstOrDefault().Id);
            Assert.AreEqual(89, LookupData.CurrentList.LastOrDefault().Id);

            LookupData.GotoPreviousRecord();
            Assert.AreEqual(10, LookupData.CurrentList.Count);
            Assert.AreEqual(LookupScrollPositions.Middle, LookupData.ScrollPosition);

            Assert.AreEqual(98, LookupData.CurrentList.FirstOrDefault().Id);
            Assert.AreEqual(9, LookupData.CurrentList.LastOrDefault().Id);

            LookupData.GotoPreviousRecord();
            Assert.AreEqual(10, LookupData.CurrentList.Count);
            Assert.AreEqual(LookupScrollPositions.Top, LookupData.ScrollPosition);

            Assert.AreEqual(99, LookupData.CurrentList.FirstOrDefault().Id);
            Assert.AreEqual(90, LookupData.CurrentList.LastOrDefault().Id);

            LookupData.GotoBottom();
            Assert.AreEqual(10, LookupData.CurrentList.Count);
            Assert.AreEqual(LookupScrollPositions.Bottom, LookupData.ScrollPosition);

            Assert.AreEqual(17, LookupData.CurrentList.FirstOrDefault().Id);
            Assert.AreEqual(1, LookupData.CurrentList.LastOrDefault().Id);

            LookupData.GotoTop();
            Assert.AreEqual(10, LookupData.CurrentList.Count);
            Assert.AreEqual(LookupScrollPositions.Top, LookupData.ScrollPosition);

            Assert.AreEqual(99, LookupData.CurrentList.FirstOrDefault().Id);
            Assert.AreEqual(90, LookupData.CurrentList.LastOrDefault().Id);

            LookupData.OnSearchForChange("T-20");
            Assert.AreEqual(10, LookupData.CurrentList.Count);
            Assert.AreEqual(LookupScrollPositions.Middle, LookupData.ScrollPosition);

            Assert.AreEqual(24, LookupData.CurrentList.FirstOrDefault().Id);
            Assert.AreEqual(16, LookupData.CurrentList.LastOrDefault().Id);
        }

        [TestMethod]
        public void TestSortCustomerAsc()
        {
            Reinitialize();
            LookupData.GetInitData();
            Assert.AreEqual(10, LookupData.CurrentList.Count);
            Assert.AreEqual(LookupScrollPositions.Top, LookupData.ScrollPosition);
            Assert.AreEqual(1, LookupData.CurrentList.FirstOrDefault().Id);
            Assert.AreEqual(10, LookupData.CurrentList.LastOrDefault().Id);

            var column = LookupDefinition.GetFieldColumnDefinition(p => p.CustomerName);
            LookupData.OnColumnClick(column, true);
            Assert.AreEqual(10, LookupData.CurrentList.Count);
            Assert.AreEqual(LookupScrollPositions.Top, LookupData.ScrollPosition);
            //Anna(7)/Bruce(3)
            Assert.AreEqual(1, LookupData.CurrentList.FirstOrDefault().Id);
            Assert.AreEqual(32, LookupData.CurrentList.LastOrDefault().Id);
            //*
            LookupData.GotoNextRecord();
            Assert.AreEqual(10, LookupData.CurrentList.Count);
            Assert.AreEqual(LookupScrollPositions.Middle, LookupData.ScrollPosition);
            //Anna(6)/Bruce(4)
            Assert.AreEqual(16, LookupData.CurrentList.FirstOrDefault().Id);
            Assert.AreEqual(47, LookupData.CurrentList.LastOrDefault().Id); 
            //*            
            LookupData.GotoNextPage();
            Assert.AreEqual(10, LookupData.CurrentList.Count);
            Assert.AreEqual(LookupScrollPositions.Middle, LookupData.ScrollPosition);
            //Bruce(3)/Charley(6)/Dave(1)
            Assert.AreEqual(62, LookupData.CurrentList.FirstOrDefault().Id);
            Assert.AreEqual(18, LookupData.CurrentList.LastOrDefault().Id);
            //*
            LookupData.GotoNextRecord();
            Assert.AreEqual(10, LookupData.CurrentList.Count);
            Assert.AreEqual(LookupScrollPositions.Middle, LookupData.ScrollPosition);
            //Bruce(2)/Charley(6)/Dave(2)
            Assert.AreEqual(77, LookupData.CurrentList.FirstOrDefault().Id);
            Assert.AreEqual(3, LookupData.CurrentList.LastOrDefault().Id); 
            //*
            LookupData.GotoPreviousPage();
            Assert.AreEqual(10, LookupData.CurrentList.Count);
            Assert.AreEqual(LookupScrollPositions.Middle, LookupData.ScrollPosition);
            //Anna(5)/Bruce(5)
            Assert.AreEqual(31, LookupData.CurrentList.FirstOrDefault().Id);
            Assert.AreEqual(62, LookupData.CurrentList.LastOrDefault().Id);
            //*
            LookupData.GotoPreviousRecord();
            Assert.AreEqual(10, LookupData.CurrentList.Count);
            Assert.AreEqual(LookupScrollPositions.Middle, LookupData.ScrollPosition);
            //Anna(6)/Bruce(4)
            Assert.AreEqual(16, LookupData.CurrentList.FirstOrDefault().Id);
            Assert.AreEqual(47, LookupData.CurrentList.LastOrDefault().Id);
            //*
            LookupData.GotoPreviousRecord();
            Assert.AreEqual(10, LookupData.CurrentList.Count);
            Assert.AreEqual(LookupScrollPositions.Top, LookupData.ScrollPosition);
            //Anna(7)/Bruce(3)
            Assert.AreEqual(1, LookupData.CurrentList.FirstOrDefault().Id);
            Assert.AreEqual(32, LookupData.CurrentList.LastOrDefault().Id);
            //*
            LookupData.GotoBottom();
            Assert.AreEqual(10, LookupData.CurrentList.Count);
            Assert.AreEqual(LookupScrollPositions.Bottom, LookupData.ScrollPosition);
            //Sparky(4)/Susan(6)
            Assert.AreEqual(41, LookupData.CurrentList.FirstOrDefault().Id);
            Assert.AreEqual(87, LookupData.CurrentList.LastOrDefault().Id);
            //*
            LookupData.GotoTop();
            Assert.AreEqual(10, LookupData.CurrentList.Count);
            Assert.AreEqual(LookupScrollPositions.Top, LookupData.ScrollPosition);
            //Anna(7)/Bruce(3)
            Assert.AreEqual(1, LookupData.CurrentList.FirstOrDefault().Id);
            Assert.AreEqual(32, LookupData.CurrentList.LastOrDefault().Id);
            //*
            LookupData.OnSearchForChange("P");
            Assert.AreEqual(10, LookupData.CurrentList.Count);
            Assert.AreEqual(LookupScrollPositions.Middle, LookupData.ScrollPosition);
            //Laura(4)/Peter(6)
            Assert.AreEqual(68, LookupData.CurrentList.FirstOrDefault().Id);
            Assert.AreEqual(9, LookupData.CurrentList.LastOrDefault().Id);
        }

        [TestMethod]
        public void TestSortCustomerDesc()
        {
            Reinitialize();
            LookupData.GetInitData();
            Assert.AreEqual(10, LookupData.CurrentList.Count);
            Assert.AreEqual(LookupScrollPositions.Top, LookupData.ScrollPosition);
            Assert.AreEqual(1, LookupData.CurrentList.FirstOrDefault().Id);
            Assert.AreEqual(10, LookupData.CurrentList.LastOrDefault().Id);

            var column = LookupDefinition.GetFieldColumnDefinition(p => p.CustomerName);
            LookupData.OnColumnClick(column, true);
            LookupData.OnColumnClick(column, true);
            Assert.AreEqual(10, LookupData.CurrentList.Count);
            Assert.AreEqual(LookupScrollPositions.Top, LookupData.ScrollPosition);
            //Susan(6)/Sparky(4)
            Assert.AreEqual(87, LookupData.CurrentList.FirstOrDefault().Id);
            Assert.AreEqual(41, LookupData.CurrentList.LastOrDefault().Id);

            LookupData.GotoNextRecord();
            Assert.AreEqual(10, LookupData.CurrentList.Count);
            Assert.AreEqual(LookupScrollPositions.Middle, LookupData.ScrollPosition);
            //Susan(5)/Sparky(5)
            Assert.AreEqual(72, LookupData.CurrentList.FirstOrDefault().Id);
            Assert.AreEqual(26, LookupData.CurrentList.LastOrDefault().Id);

            LookupData.GotoNextPage();
            Assert.AreEqual(10, LookupData.CurrentList.Count);
            Assert.AreEqual(LookupScrollPositions.Middle, LookupData.ScrollPosition);
            //Sparky(1)/Smitty(6)/Smilley(3)
            Assert.AreEqual(11, LookupData.CurrentList.FirstOrDefault().Id);
            Assert.AreEqual(55, LookupData.CurrentList.LastOrDefault().Id);

            LookupData.GotoNextRecord();
            Assert.AreEqual(10, LookupData.CurrentList.Count);
            Assert.AreEqual(LookupScrollPositions.Middle, LookupData.ScrollPosition);
            //Smitty(6)/Smilley(4)
            Assert.AreEqual(89, LookupData.CurrentList.FirstOrDefault().Id);
            Assert.AreEqual(40, LookupData.CurrentList.LastOrDefault().Id);

            LookupData.GotoPreviousPage();
            Assert.AreEqual(10, LookupData.CurrentList.Count);
            Assert.AreEqual(LookupScrollPositions.Middle, LookupData.ScrollPosition);
            //Susan(4)/Sparky(6)
            Assert.AreEqual(57, LookupData.CurrentList.FirstOrDefault().Id);
            Assert.AreEqual(11, LookupData.CurrentList.LastOrDefault().Id);

            LookupData.GotoPreviousRecord();
            Assert.AreEqual(10, LookupData.CurrentList.Count);
            Assert.AreEqual(LookupScrollPositions.Middle, LookupData.ScrollPosition);
            //Susan(5)/Sparky(5)
            Assert.AreEqual(72, LookupData.CurrentList.FirstOrDefault().Id);
            Assert.AreEqual(26, LookupData.CurrentList.LastOrDefault().Id);

            LookupData.GotoPreviousRecord();
            Assert.AreEqual(10, LookupData.CurrentList.Count);
            Assert.AreEqual(LookupScrollPositions.Top, LookupData.ScrollPosition);
            //Susan(6)/Sparky(4)
            Assert.AreEqual(87, LookupData.CurrentList.FirstOrDefault().Id);
            Assert.AreEqual(41, LookupData.CurrentList.LastOrDefault().Id);

            LookupData.GotoBottom();
            Assert.AreEqual(10, LookupData.CurrentList.Count);
            Assert.AreEqual(LookupScrollPositions.Bottom, LookupData.ScrollPosition);
            //Bruce(3)/Anna(7)
            Assert.AreEqual(32, LookupData.CurrentList.FirstOrDefault().Id);
            Assert.AreEqual(1, LookupData.CurrentList.LastOrDefault().Id);

            LookupData.GotoTop();
            Assert.AreEqual(10, LookupData.CurrentList.Count);
            Assert.AreEqual(LookupScrollPositions.Top, LookupData.ScrollPosition);
            //Susan(6)/Sparky(4)
            Assert.AreEqual(87, LookupData.CurrentList.FirstOrDefault().Id);
            Assert.AreEqual(41, LookupData.CurrentList.LastOrDefault().Id);

            LookupData.OnSearchForChange("P");
            Assert.AreEqual(10, LookupData.CurrentList.Count);
            Assert.AreEqual(LookupScrollPositions.Middle, LookupData.ScrollPosition);
            //Smiley(4)/Peter(6)
            Assert.AreEqual(40, LookupData.CurrentList.FirstOrDefault().Id);
            Assert.AreEqual(39, LookupData.CurrentList.LastOrDefault().Id);
        }

        [TestMethod]
        public void TestSortPunchInDateAsc()
        {
            Reinitialize();
            LookupData.GetInitData();
            Assert.AreEqual(10, LookupData.CurrentList.Count);
            Assert.AreEqual(LookupScrollPositions.Top, LookupData.ScrollPosition);
            Assert.AreEqual(1, LookupData.CurrentList.FirstOrDefault().Id);
            Assert.AreEqual(10, LookupData.CurrentList.LastOrDefault().Id);

            var column = LookupDefinition.GetFieldColumnDefinition(p => p.PunchInDate);
            LookupData.OnColumnClick(column, true);
            Assert.AreEqual(10, LookupData.CurrentList.Count);
            Assert.AreEqual(LookupScrollPositions.Top, LookupData.ScrollPosition);
            
            Assert.AreEqual(1, LookupData.CurrentList.FirstOrDefault().Id);
            Assert.AreEqual(45, LookupData.CurrentList.LastOrDefault().Id);

            LookupData.GotoNextPage();
            Assert.AreEqual(10, LookupData.CurrentList.Count);
            Assert.AreEqual(LookupScrollPositions.Middle, LookupData.ScrollPosition);

            Assert.AreEqual(49, LookupData.CurrentList.FirstOrDefault().Id);
            Assert.AreEqual(81, LookupData.CurrentList.LastOrDefault().Id);
        }
    }
}
