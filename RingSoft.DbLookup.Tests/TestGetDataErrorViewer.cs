using System;
using System.Diagnostics;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.DataProcessor;

namespace RingSoft.DbLookup.Tests
{
    public class TestGetDataErrorViewer : IDbLookupUserInterface, IControlsUserInterface
    {
        public void ShowDataProcessResult(DataProcessResult dataProcessResult)
        {
            Debug.WriteLine($"GetDataError!  {dataProcessResult.Message}");
        }

        public void SetWindowCursor(WindowCursorTypes cursor)
        {
            
        }

        public void ShowMessageBox(string text, string caption, RsMessageBoxIcons icon)
        {
            Console.WriteLine(text);
        }

        public MessageBoxButtonsResult ShowYesNoMessageBox(string text, string caption)
        {
            Console.WriteLine(text);
            return MessageBoxButtonsResult.Yes;
        }

        public MessageBoxButtonsResult ShowYesNoCancelMessageBox(string text, string caption)
        {
            Console.WriteLine(text);
            return MessageBoxButtonsResult.Yes;
        }
    }
}
