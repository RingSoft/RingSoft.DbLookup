using System;

namespace RingSoft.DbLookup.GetDataProcessor
{
    /// <summary>
    /// Implement this interface to display an error to the user.
    /// </summary>
    public interface IDataProcessResultViewer
    {
        /// <summary>
        /// Shows the get data error.
        /// </summary>
        /// <param name="dataProcessResult">The get data result.</param>
        void ShowDataProcessResult(DataProcessResult dataProcessResult);
    }

    internal class DefaultDataProcessResultViewer : IDataProcessResultViewer
    {
        public void ShowDataProcessResult(DataProcessResult dataProcessResult)
        {
            Console.WriteLine(dataProcessResult.Message);
        }
    }
}
