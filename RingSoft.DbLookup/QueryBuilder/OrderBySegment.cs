namespace RingSoft.DbLookupCore.QueryBuilder
{
    public enum OrderBySegmentTypes
    {
        General = 0,
        Formula = 1,
        Enum = 2
    }

    public enum OrderByTypes
    {
        Ascending = 0,
        Descending = 1
    }
    /// <summary>
    /// An segment of an ORDER BY clause.
    /// </summary>
    public class OrderBySegment
    {
        /// <summary>
        /// Gets the type of the order by segment.
        /// </summary>
        /// <value>
        /// The type of the order by segment.
        /// </value>
        public virtual OrderBySegmentTypes OrderBySegmentType => OrderBySegmentTypes.General;

        /// <summary>
        /// Gets the table object this Where Item is attached to.
        /// </summary>
        /// <value>
        /// The table.
        /// </value>
        public QueryTable Table { get; internal set; }

        /// <summary>
        /// Gets the name of the field.
        /// </summary>
        /// <value>
        /// The name of the field.
        /// </value>
        public string FieldName { get; internal set; }

        /// <summary>
        /// Gets the type of the order by. e.g.(ASC or DESC)
        /// </summary>
        /// <value>
        /// The type of the order by.
        /// </value>
        public OrderByTypes OrderByType { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether the order by is case sensitive.
        /// </summary>
        /// <value>
        ///   <c>true</c> if case sensitive; otherwise, <c>false</c>.
        /// </value>
        public bool CaseSensitive { get; internal set; }

        internal OrderBySegment()
        {
            
        }
    }
}
