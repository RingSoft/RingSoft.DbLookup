using System.Data;

namespace RingSoft.DbLookup.Lookup
{
    public class LookupDataChangedArgs
    {
        /// <summary>
        /// Gets the lookup results data table.
        /// </summary>
        /// <value>
        /// The lookup results data table.
        /// </value>
        public DataTable OutputTable { get; }

        /// <summary>
        /// Gets the index of the selected row.
        /// </summary>
        /// <value>
        /// The index of the selected row.
        /// </value>
        public int SelectedRowIndex { get; }

        /// <summary>
        /// Gets the scroll position.
        /// </summary>
        /// <value>
        /// The scroll position.
        /// </value>
        public LookupScrollPositions ScrollPosition { get; }

        /// <summary>
        /// Gets a value indicating whether records are being counted.
        /// </summary>
        /// <value>
        ///   <c>true</c> if counting records; otherwise, <c>false</c>.
        /// </value>
        public bool CountingRecords { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether search for is changing.
        /// </summary>
        /// <value>
        ///   <c>true</c> if search for is changing; otherwise, <c>false</c>.
        /// </value>
        public bool SearchForChanging { get; internal set; }

        public bool Abort { get; set; }

        internal LookupDataChangedArgs(DataTable outputTable, int selectedRowIndex, LookupScrollPositions scrollPosition)
        {
            OutputTable = outputTable;
            SelectedRowIndex = selectedRowIndex;
            ScrollPosition = scrollPosition;
        }
    }
}
