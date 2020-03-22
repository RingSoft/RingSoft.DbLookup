using System;

namespace RingSoft.DbLookupCore.GetDataProcessor
{
    /// <summary>
    /// Implement this interface to display an error to the user.
    /// </summary>
    public interface IGetDataResultErrorViewer
    {
        /// <summary>
        /// Shows the get data error.
        /// </summary>
        /// <param name="getDataResult">The get data result.</param>
        void ShowGetDataError(GetDataResult getDataResult);
    }

    internal class DefaultGetDataResultErrorViewer : IGetDataResultErrorViewer
    {
        public void ShowGetDataError(GetDataResult getDataResult)
        {
            Console.WriteLine(getDataResult.ErrorMessage);
        }
    }
}
