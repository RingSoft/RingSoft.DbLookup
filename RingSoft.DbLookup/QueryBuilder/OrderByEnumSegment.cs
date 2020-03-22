namespace RingSoft.DbLookup.QueryBuilder
{
    /// <summary>
    /// An Order By segment for the text value of an enumerator.
    /// </summary>
    /// <seealso cref="WhereItem" />
    public class OrderByEnumSegment : OrderBySegment
    {
        public override OrderBySegmentTypes OrderBySegmentType => OrderBySegmentTypes.Enum;

        /// <summary>
        /// Gets the enum translation.
        /// </summary>
        /// <value>
        /// The enum translation.
        /// </value>
        public EnumFieldTranslation EnumTranslation { get; internal set; }
    }
}
