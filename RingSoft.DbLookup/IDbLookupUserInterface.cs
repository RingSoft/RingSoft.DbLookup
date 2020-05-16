using System;
using RingSoft.DbLookup.DataProcessor;

namespace RingSoft.DbLookup
{
    public enum WindowCursorTypes
    {
        Default = 0,
        Wait = 1
    }

    public enum RsMessageBoxIcons
    {
        Error = 0,
        Exclamation = 1,
        Information = 2
    }

    /// <summary>
    /// Implement this to so DbLookup classes can interact with the user interface.
    /// </summary>
    public interface IDbLookupUserInterface
    {
        /// <summary>
        /// Sets the window cursor.
        /// </summary>
        /// <param name="cursor">The cursor.</param>
        void SetWindowCursor(WindowCursorTypes cursor);

        /// <summary>
        /// Shows the data process execution result.
        /// </summary>
        /// <param name="dataProcessResult">The data process result.</param>
        void ShowDataProcessResult(DataProcessResult dataProcessResult);

        void ShowMessageBox(string text, string caption, RsMessageBoxIcons icon);
    }

    internal class DefaultUserInterface : IDbLookupUserInterface
    {
        public void SetWindowCursor(WindowCursorTypes cursor)
        {
            
        }

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
