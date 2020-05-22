namespace RingSoft.DbLookup.QueryBuilder
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

        /// <summary>
        /// Gets a value indicating whether that this formula has no condition or value.
        /// </summary>
        /// <value>
        ///   <c>true</c> no condition or value; otherwise, <c>false</c>.
        /// </value>
        public bool NoValue { get; internal set; }

        internal WhereFormulaItem()
        {
            
        }
    }
}
