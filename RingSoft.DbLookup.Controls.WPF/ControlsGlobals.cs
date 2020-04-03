using System;
using System.Windows.Input;
using RingSoft.DbLookup.GetDataProcessor;

namespace RingSoft.DbLookup.Controls.WPF
{
    public class ControlsUserInterface : IDataProcessResultViewer, IWindowCursor
    {
        public void ShowDataProcessResult(DataProcessResult dataProcessResult)
        {
            var dataProcessResultWindow = new DataProcessResultWindow(dataProcessResult);
            dataProcessResultWindow.ShowDialog();
        }

        public void SetWindowCursor(WindowCursorTypes cursor)
        {
            switch (cursor)
            {
                case WindowCursorTypes.Default:
                    Mouse.OverrideCursor = null;
                    break;
                case WindowCursorTypes.Wait:
                    Mouse.OverrideCursor = Cursors.Wait;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(cursor), cursor, null);
            }
        }
    }
    public static class ControlsGlobals
    {
        private static ControlsUserInterface _userInterface = new ControlsUserInterface();

        public static void InitUi()
        {
            DbDataProcessor.DataProcessResultViewer = _userInterface;
            DbDataProcessor.WindowCursor = _userInterface;
        }
    }
}
