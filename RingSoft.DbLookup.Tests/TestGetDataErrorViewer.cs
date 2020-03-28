using System.Diagnostics;
using RingSoft.DbLookup.GetDataProcessor;

namespace RingSoft.DbLookup.Tests
{
    public class TestGetDataErrorViewer : IGetDataResultErrorViewer
    {
        public void ShowGetDataError(GetDataResult getDataResult)
        {
            Debug.WriteLine($"GetDataError!  {getDataResult.ErrorMessage}");
        }
    }
}
