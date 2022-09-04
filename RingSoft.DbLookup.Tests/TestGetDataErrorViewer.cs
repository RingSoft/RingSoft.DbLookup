using System;
using System.Diagnostics;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.Lookup;

namespace RingSoft.DbLookup.Tests
{
    public class TestGetDataErrorViewer : IDbLookupUserInterface, IControlsUserInterface
    {
        public void ShowDataProcessResult(DataProcessResult dataProcessResult)
        {
            Debug.WriteLine($"GetDataError!  {dataProcessResult.Message}");
        }

        public void ShowAddOnTheFlyWindow(LookupAddViewArgs e)
        {
            
        }

        public void SetWindowCursor(WindowCursorTypes cursor)
        {
            
        }

        public void ShowMessageBox(string text, string caption, RsMessageBoxIcons icon)
        {
            Console.WriteLine(text);
        }

        public MessageBoxButtonsResult ShowYesNoMessageBox(string text, string caption, bool playSound = false)
        {
            Console.WriteLine(text);
            return MessageBoxButtonsResult.Yes;
        }

        public MessageBoxButtonsResult ShowYesNoCancelMessageBox(string text, string caption, bool playSound = false)
        {
            Console.WriteLine(text);
            return MessageBoxButtonsResult.Yes;
        }
    }
}
