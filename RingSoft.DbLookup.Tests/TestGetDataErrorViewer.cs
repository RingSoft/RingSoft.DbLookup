using System.Diagnostics;
using RingSoft.DbLookup.GetDataProcessor;

namespace RingSoft.DbLookup.Tests
{
    public class TestGetDataErrorViewer : IDataProcessResultViewer
    {
        public void ShowDataProcessResult(DataProcessResult dataProcessResult)
        {
            Debug.WriteLine($"GetDataError!  {dataProcessResult.Message}");
        }

        public void ShowMessageBox(string text, string caption, RsMessageBoxIcons icon)
        {
            Debug.WriteLine(text);
        }
    }
}
