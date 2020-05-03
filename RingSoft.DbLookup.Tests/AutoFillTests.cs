using Microsoft.VisualStudio.TestTools.UnitTesting;
using RingSoft.DbLookup.App.Library;
using RingSoft.DbLookup.App.Library.EfCore.DevLogix;
using RingSoft.DbLookup.App.Library.LibLookupContext;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.GetDataProcessor;

namespace RingSoft.DbLookup.Tests
{
    [TestClass]
    public class AutoFillTests
    {
        private static DevLogixLookupContextEfCore _context;
        private static AutoFillFieldDefinition _autoFillDefinition;

        [ClassInitialize]
        public static void Setup(TestContext testContext)
        {
            RsDbLookupAppGlobals.Initialize("UnitTests");
            DbDataProcessor.DataProcessResultViewer = new TestGetDataErrorViewer();
            _context = new DevLogixLookupContextEfCore()
            {
                DataProcessorType = DataProcessorTypes.Sqlite
            };
            _autoFillDefinition = new AutoFillFieldDefinition(_context.Issues.GetFieldDefinition(p => p.Description));
        }

        [TestMethod]
        public void IssuesAutoFill_FirstChar()
        {
            var autoFillData = new AutoFillData(_autoFillDefinition);
            autoFillData.OnKeyCharPressed('c', "", 0, 0);

            Assert.AreEqual("Change PunchTo", autoFillData.TextResult);
            Assert.AreEqual(1, autoFillData.CursorStartIndex);
            Assert.AreEqual(13, autoFillData.TextSelectLength);
            Assert.AreEqual(389, _context.Issues.GetEntityFromPrimaryKeyValue(autoFillData.PrimaryKeyValue).Id);
        }

        [TestMethod]
        public void IssuesAutoFill_SecondChar()
        {
            var autoFillData = new AutoFillData(_autoFillDefinition);

            autoFillData.OnKeyCharPressed('h', "Change PunchTo", 1, 13);

            Assert.AreEqual("Change PunchTo", autoFillData.TextResult);
            Assert.AreEqual(2, autoFillData.CursorStartIndex);
            Assert.AreEqual(12, autoFillData.TextSelectLength);
            Assert.AreEqual(389, _context.Issues.GetEntityFromPrimaryKeyValue(autoFillData.PrimaryKeyValue).Id);
        }

        [TestMethod]
        public void IssuesAutoFill_CharInMiddleSelLengthInMiddle()
        {
            var autoFillData = new AutoFillData(_autoFillDefinition);

            autoFillData.OnKeyCharPressed('o', "Change PunchTo", 2, 9);

            Assert.AreEqual("ChohTo", autoFillData.TextResult);
            Assert.AreEqual(3, autoFillData.CursorStartIndex);
            Assert.AreEqual(0, autoFillData.TextSelectLength);
            Assert.AreEqual(0, _context.Issues.GetEntityFromPrimaryKeyValue(autoFillData.PrimaryKeyValue).Id);
        }

        [TestMethod]
        public void IssuesAutoFill_CharInMiddleSelLengthAtEnd()
        {
            var autoFillData = new AutoFillData(_autoFillDefinition);

            autoFillData.OnKeyCharPressed('r', "Populate Lookup", 1, 14);

            Assert.AreEqual("Pr", autoFillData.TextResult);
            Assert.AreEqual(2, autoFillData.CursorStartIndex);
            Assert.AreEqual(0, autoFillData.TextSelectLength);
            Assert.AreEqual(0, _context.Issues.GetEntityFromPrimaryKeyValue(autoFillData.PrimaryKeyValue).Id);
        }

        [TestMethod]
        public void IssuesAutoFill_BackspaceFromEnd()
        {
            var autoFillData = new AutoFillData(_autoFillDefinition);

            autoFillData.OnBackspaceKeyDown("Pr", 2, 0);

            Assert.AreEqual("P", autoFillData.TextResult);
            Assert.AreEqual(1, autoFillData.CursorStartIndex);
            Assert.AreEqual(0, autoFillData.TextSelectLength);
            Assert.AreEqual(0, _context.Issues.GetEntityFromPrimaryKeyValue(autoFillData.PrimaryKeyValue).Id);
        }

        [TestMethod]
        public void IssuesAutoFill_BackspaceWithCursorInMiddleSelLengthInMiddle()
        {
            var autoFillData = new AutoFillData(_autoFillDefinition);

            autoFillData.OnBackspaceKeyDown("Populate Lookup", 2, 10);

            Assert.AreEqual("Pokup", autoFillData.TextResult);
            Assert.AreEqual(2, autoFillData.CursorStartIndex);
            Assert.AreEqual(0, autoFillData.TextSelectLength);
            Assert.AreEqual(0, _context.Issues.GetEntityFromPrimaryKeyValue(autoFillData.PrimaryKeyValue).Id);
        }

        [TestMethod]
        public void IssuesAutoFill_BackspaceFromEndToStart()
        {
            var autoFillData = new AutoFillData(_autoFillDefinition);

            autoFillData.OnBackspaceKeyDown("P", 1, 0);

            Assert.AreEqual("", autoFillData.TextResult);
            Assert.AreEqual(0, autoFillData.CursorStartIndex);
            Assert.AreEqual(0, autoFillData.TextSelectLength);
            Assert.AreEqual(0, _context.Issues.GetEntityFromPrimaryKeyValue(autoFillData.PrimaryKeyValue).Id);
        }

        [TestMethod]
        public void IssuesAutoFill_DeleteWithCursorInMiddleSelLengthInMiddle()
        {
            var autoFillData = new AutoFillData(_autoFillDefinition);

            autoFillData.OnDeleteKeyDown("Add % Complete", 2, 8);

            Assert.AreEqual("Adlete", autoFillData.TextResult);
            Assert.AreEqual(2, autoFillData.CursorStartIndex);
            Assert.AreEqual(0, autoFillData.TextSelectLength);
            Assert.AreEqual(0, _context.Issues.GetEntityFromPrimaryKeyValue(autoFillData.PrimaryKeyValue).Id);
        }

        [TestMethod]
        public void IssuesAutoFill_DeleteWithCursorInMiddleSelLengthToEnd()
        {
            var autoFillData = new AutoFillData(_autoFillDefinition);

            autoFillData.OnDeleteKeyDown("Change PunchTo", 1, 13);

            Assert.AreEqual("C", autoFillData.TextResult);
            Assert.AreEqual(1, autoFillData.CursorStartIndex);
            Assert.AreEqual(0, autoFillData.TextSelectLength);
            Assert.AreEqual(0, _context.Issues.GetEntityFromPrimaryKeyValue(autoFillData.PrimaryKeyValue).Id);
        }
    }
}
