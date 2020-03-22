namespace RingSoft.DbLookup.QueryBuilder
{
    /// <summary>
    /// A column that has a numeric database value translated into a text value.
    /// </summary>
    /// <seealso cref="SelectColumn" />
    public class SelectEnumColumn : SelectColumn
    {
        public override ColumnTypes ColumnType => ColumnTypes.Enum;

        /// <summary>
        /// Gets the enum translation.
        /// </summary>
        /// <value>
        /// The enum translation.
        /// </value>
        public EnumFieldTranslation EnumTranslation { get; internal set; }

        internal SelectEnumColumn()
        {
            
        }
    }
}
