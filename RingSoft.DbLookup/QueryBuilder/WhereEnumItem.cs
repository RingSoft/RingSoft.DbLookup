using RingSoft.DataEntryControls.Engine;

namespace RingSoft.DbLookup.QueryBuilder
{
    /// <summary>
    /// A Where Item that searches the text value of an enumerator.
    /// </summary>
    /// <seealso cref="WhereItem" />
    public class WhereEnumItem : WhereItem
    {
        public override WhereItemTypes WhereItemType => WhereItemTypes.Enum;

        /// <summary>
        /// Gets the enum translation.
        /// </summary>
        /// <value>
        /// The enum translation.
        /// </value>
        public EnumFieldTranslation EnumTranslation { get; internal set; }

        internal WhereEnumItem()
        {
            
        }
    }
}
