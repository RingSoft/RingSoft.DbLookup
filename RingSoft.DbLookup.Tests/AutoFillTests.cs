using Microsoft.VisualStudio.TestTools.UnitTesting;
using RingSoft.DbLookup.App.Library;
using RingSoft.DbLookup.App.Library.EfCore.DevLogix;
using RingSoft.DbLookup.App.Library.LibLookupContext;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.DataProcessor;

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
            DbDataProcessor.UserInterface = new TestGetDataErrorViewer();
            _context = new DevLogixLookupContextEfCore()
            {
                DataProcessorType = DataProcessorTypes.Sqlite
            };
            _autoFillDefinition = new AutoFillFieldDefinition(_context.Issues.GetFieldDefinition(p => p.Description));
        }

        [TestMethod]
        public void IssuesAutoFill_FirstChar()
        {
            var autoFillControl = new TestAutoFillControl();
            var autoFillData = new AutoFillData(autoFillControl, _autoFillDefinition);
            autoFillData.OnKeyCharPressed('c');

            Assert.AreEqual("Change PunchTo", autoFillData.TextResult);
            Assert.AreEqual(1, autoFillData.CursorStartIndex);
            Assert.AreEqual(13, autoFillData.TextSelectLength);
            Assert.AreEqual(389, _context.Issues.GetEntityFromPrimaryKeyValue(autoFillData.PrimaryKeyValue).Id);
        }

        [TestMethod]
        public void IssuesAutoFill_SecondChar()
        {
            var autoFillControl = new TestAutoFillControl
            {
                EditText = "Change PunchTo",
                SelectionStart = 1,
                SelectionLength = 13
            };
            var autoFillData = new AutoFillData(autoFillControl, _autoFillDefinition);

            autoFillData.OnKeyCharPressed('h');

            Assert.AreEqual("Change PunchTo", autoFillData.TextResult);
            Assert.AreEqual(2, autoFillData.CursorStartIndex);
            Assert.AreEqual(12, autoFillData.TextSelectLength);
            Assert.AreEqual(389, _context.Issues.GetEntityFromPrimaryKeyValue(autoFillData.PrimaryKeyValue).Id);
        }

        [TestMethod]
        public void IssuesAutoFill_CharInMiddleSelLengthInMiddle()
        {
            var autoFillControl = new TestAutoFillControl
            {
                EditText = "Change PunchTo",
                SelectionStart = 2,
                SelectionLength = 9
            };
            var autoFillData = new AutoFillData(autoFillControl, _autoFillDefinition);

            autoFillData.OnKeyCharPressed('o');

            Assert.AreEqual("ChohTo", autoFillData.TextResult);
            Assert.AreEqual(3, autoFillData.CursorStartIndex);
            Assert.AreEqual(0, autoFillData.TextSelectLength);
            Assert.AreEqual(0, _context.Issues.GetEntityFromPrimaryKeyValue(autoFillData.PrimaryKeyValue).Id);
        }

        [TestMethod]
        public void IssuesAutoFill_CharInMiddleSelLengthAtEnd()
        {
            var autoFillControl = new TestAutoFillControl
            {
                EditText = "Populate Lookup",
                SelectionStart = 1,
                SelectionLength = 14
            };
            var autoFillData = new AutoFillData(autoFillControl, _autoFillDefinition);

            autoFillData.OnKeyCharPressed('r');

            Assert.AreEqual("Pr", autoFillData.TextResult);
            Assert.AreEqual(2, autoFillData.CursorStartIndex);
            Assert.AreEqual(0, autoFillData.TextSelectLength);
            Assert.AreEqual(0, _context.Issues.GetEntityFromPrimaryKeyValue(autoFillData.PrimaryKeyValue).Id);
        }

        [TestMethod]
        public void IssuesAutoFill_BackspaceFromEnd()
        {
            var autoFillControl = new TestAutoFillControl
            {
                EditText = "Pr",
                SelectionStart = 2,
                SelectionLength = 0
            };
            var autoFillData = new AutoFillData(autoFillControl, _autoFillDefinition);

            autoFillData.OnBackspaceKeyDown();

            Assert.AreEqual("P", autoFillData.TextResult);
            Assert.AreEqual(1, autoFillData.CursorStartIndex);
            Assert.AreEqual(0, autoFillData.TextSelectLength);
            Assert.AreEqual(0, _context.Issues.GetEntityFromPrimaryKeyValue(autoFillData.PrimaryKeyValue).Id);
        }

        [TestMethod]
        public void IssuesAutoFill_BackspaceWithCursorInMiddleSelLengthInMiddle()
        {
            var autoFillControl = new TestAutoFillControl
            {
                EditText = "Populate Lookup",
                SelectionStart = 2,
                SelectionLength = 10
            };
            var autoFillData = new AutoFillData(autoFillControl, _autoFillDefinition);

            autoFillData.OnBackspaceKeyDown();

            Assert.AreEqual("Pokup", autoFillData.TextResult);
            Assert.AreEqual(2, autoFillData.CursorStartIndex);
            Assert.AreEqual(0, autoFillData.TextSelectLength);
            Assert.AreEqual(0, _context.Issues.GetEntityFromPrimaryKeyValue(autoFillData.PrimaryKeyValue).Id);
        }

        [TestMethod]
        public void IssuesAutoFill_BackspaceFromEndToStart()
        {
            var autoFillControl = new TestAutoFillControl
            {
                EditText = "P",
                SelectionStart = 1,
                SelectionLength = 0
            };
            var autoFillData = new AutoFillData(autoFillControl, _autoFillDefinition);

            autoFillData.OnBackspaceKeyDown();

            Assert.AreEqual("", autoFillData.TextResult);
            Assert.AreEqual(0, autoFillData.CursorStartIndex);
            Assert.AreEqual(0, autoFillData.TextSelectLength);
            Assert.AreEqual(0, _context.Issues.GetEntityFromPrimaryKeyValue(autoFillData.PrimaryKeyValue).Id);
        }

        [TestMethod]
        public void IssuesAutoFill_DeleteWithCursorInMiddleSelLengthInMiddle()
        {
            var autoFillControl = new TestAutoFillControl
            {
                EditText = "Add % Complete",
                SelectionStart = 2,
                SelectionLength = 8
            };
            var autoFillData = new AutoFillData(autoFillControl, _autoFillDefinition);

            autoFillData.OnDeleteKeyDown();

            Assert.AreEqual("Adlete", autoFillData.TextResult);
            Assert.AreEqual(2, autoFillData.CursorStartIndex);
            Assert.AreEqual(0, autoFillData.TextSelectLength);
            Assert.AreEqual(0, _context.Issues.GetEntityFromPrimaryKeyValue(autoFillData.PrimaryKeyValue).Id);
        }

        [TestMethod]
        public void IssuesAutoFill_DeleteWithCursorInMiddleSelLengthToEnd()
        {
            var autoFillControl = new TestAutoFillControl
            {
                EditText = "Change PunchTo",
                SelectionStart = 1,
                SelectionLength = 13
            };
            var autoFillData = new AutoFillData(autoFillControl, _autoFillDefinition);

            autoFillData.OnDeleteKeyDown();

            Assert.AreEqual("C", autoFillData.TextResult);
            Assert.AreEqual(1, autoFillData.CursorStartIndex);
            Assert.AreEqual(0, autoFillData.TextSelectLength);
            Assert.AreEqual(0, _context.Issues.GetEntityFromPrimaryKeyValue(autoFillData.PrimaryKeyValue).Id);
        }
    }
}
