using System;

namespace RingSoft.DbLookup.DataProcessor
{
    public enum RsMessageBoxIcons
    {
        Error = 0,
        Exclamation = 1,
        Information = 2
    }

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

        void ShowMessageBox(string text, string caption, RsMessageBoxIcons icon);
    }

    internal class DefaultDataProcessResultViewer : IDataProcessResultViewer
    {
        public void ShowDataProcessResult(DataProcessResult dataProcessResult)
        {
            Console.WriteLine(dataProcessResult.Message);
        }

        public void ShowMessageBox(string text, string caption, RsMessageBoxIcons icon)
        {
            Console.WriteLine(text);
        }
    }
}
