using RingSoft.DbLookup.QueryBuilder;

namespace RingSoft.DbLookup.TableProcessing
{
    public class FormulaFilterDefinition : FilterItemType<FormulaFilterDefinition>
    {
        public override FilterItemTypes Type => FilterItemTypes.Formula;

        /// <summary>
        /// Gets the formula.
        /// </summary>
        /// <value>
        /// The formula.
        /// </value>
        public string Formula { get; internal set; }

        internal FormulaFilterDefinition()
        {
            
        }

        internal override void CopyFrom(FilterItemDefinition source)
        {
            var sourceFormulaItem = (FormulaFilterDefinition) source;
            Formula = sourceFormulaItem.Formula;

            base.CopyFrom(source);
        }
    }
}
