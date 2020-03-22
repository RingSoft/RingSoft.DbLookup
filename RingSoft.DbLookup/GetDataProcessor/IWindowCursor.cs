namespace RingSoft.DbLookup.GetDataProcessor
{
    public enum WindowCursorTypes
    {
        Default = 0,
        Wait = 1
    }

    /// <summary>
    /// Implement this interface to set the window cursor.
    /// </summary>
    public interface IWindowCursor
    {
        /// <summary>
        /// Sets the window cursor.
        /// </summary>
        /// <param name="cursor">The cursor.</param>
        void SetWindowCursor(WindowCursorTypes cursor);
    }

    internal class DefaultWindowCursor : IWindowCursor
    {
        public void SetWindowCursor(WindowCursorTypes cursor)
        {
            
        }
    }
}
