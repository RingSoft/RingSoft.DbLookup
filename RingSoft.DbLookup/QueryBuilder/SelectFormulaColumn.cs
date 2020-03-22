namespace RingSoft.DbLookup.QueryBuilder
{
    public class SelectFormulaColumn : SelectColumn
    {
        public override ColumnTypes ColumnType => ColumnTypes.Formula;

        /// <summary>
        /// Gets the formula.
        /// </summary>
        /// <value>
        /// The formula.
        /// </value>
        public string Formula { get; internal set; }

        internal SelectFormulaColumn()
        {
            
        }
    }
}
