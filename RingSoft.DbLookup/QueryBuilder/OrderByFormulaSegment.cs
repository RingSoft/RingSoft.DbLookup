namespace RingSoft.DbLookup.QueryBuilder
{
    /// <summary>
    /// An Order By segment for a formula.
    /// </summary>
    /// <seealso cref="WhereItem" />
    public class OrderByFormulaSegment : OrderBySegment
    {
        public override OrderBySegmentTypes OrderBySegmentType => OrderBySegmentTypes.Formula;

        /// <summary>
        /// Gets the formula.
        /// </summary>
        /// <value>
        /// The formula.
        /// </value>
        public string Formula { get; internal set; }

        internal OrderByFormulaSegment()
        {
            
        }
    }
}
