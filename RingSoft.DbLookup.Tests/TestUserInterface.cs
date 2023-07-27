using System.Threading.Tasks;
using RingSoft.DataEntryControls.Engine;

namespace RingSoft.DbLookup.Tests
{
    public class TestUserInterface : IControlsUserInterface
    {
        public WindowCursorTypes Cursor { get; private set; }

        public MessageBoxButtonsResult MessageBoxResult { get; set; }

        public TestUserInterface()
        {
            ControlsGlobals.UserInterface = this;

        }

        public void SetWindowCursor(WindowCursorTypes cursor)
        {
            Cursor = cursor;
        }

        public async Task ShowMessageBox(string text, string caption, RsMessageBoxIcons icon)
        {
            throw new System.NotImplementedException();
        }

        public async Task<MessageBoxButtonsResult> ShowYesNoMessageBox(string text, string caption, bool playSound = false)
        {
            return MessageBoxResult;
        }

        public async Task<MessageBoxButtonsResult> ShowYesNoCancelMessageBox(string text, string caption, bool playSound = false)
        {
                return MessageBoxResult;
        }
    }
}
