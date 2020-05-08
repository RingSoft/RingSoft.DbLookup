using System.Data;

namespace RingSoft.DbLookup.AutoFill
{
    /// <summary>
    /// Argument sent in the AutoFillChanged event.
    /// </summary>
    public class AutoFillDataChangedArgs
    {
        /// <summary>
        /// Gets a value indicating whether to refresh the contains list.
        /// </summary>
        /// <value>
        ///   <c>true</c> if refresh contains list; otherwise, <c>false</c>.
        /// </value>
        public bool RefreshContainsList { get; internal set; }

        /// <summary>
        /// Gets the text result.
        /// </summary>
        /// <value>
        /// The text result.
        /// </value>
        public string TextResult { get; }

        /// <summary>
        /// Gets the start index of the cursor.
        /// </summary>
        /// <value>
        /// The start index of the cursor.
        /// </value>
        public int CursorStartIndex { get; }

        /// <summary>
        /// Gets the length of the text select.
        /// </summary>
        /// <value>
        /// The length of the text select.
        /// </value>
        public int TextSelectLength { get; }

        /// <summary>
        /// Gets the contains box data table.
        /// </summary>
        /// <value>
        /// The contains box data table.
        /// </value>
        public DataTable ContainsBoxDataTable { get; internal set; }
        
        public AutoFillDataChangedArgs(string textResult, int cursorStartIndex, int textSelectLength)
        {
            TextResult = textResult;
            CursorStartIndex = cursorStartIndex;
            TextSelectLength = textSelectLength;
        }
    }
}
