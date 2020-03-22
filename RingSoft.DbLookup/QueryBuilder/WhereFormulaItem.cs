namespace RingSoft.DbLookupCore.QueryBuilder
{
    /// <summary>
    /// A Where Item that searches the results of a formula.
    /// </summary>
    /// <seealso cref="WhereItem" />
    public class WhereFormulaItem : WhereItem
    {
        public override WhereItemTypes WhereItemType => WhereItemTypes.Formula;

        /// <summary>
        /// Gets the formula.
        /// </summary>
        /// <value>
        /// The formula.
        /// </value>
        public string Formula { get; internal set; }

        internal WhereFormulaItem()
        {
            
        }
    }
}
