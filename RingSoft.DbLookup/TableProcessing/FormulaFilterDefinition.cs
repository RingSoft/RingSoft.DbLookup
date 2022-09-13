using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.QueryBuilder;

namespace RingSoft.DbLookup.TableProcessing
{
    public class FormulaFilterDefinition : FilterItemType<FormulaFilterDefinition>
    {
        public override FilterItemTypes Type => FilterItemTypes.Formula;

        public string Description { get; internal set; }
        /// <summary>
        /// Gets the formula.
        /// </summary>
        /// <value>
        /// The formula.
        /// </value>
        public string Formula { get; set; }

        public Conditions? Condition { get; set; }

        public string FilterValue { get; set; }

        public FieldDataTypes DataType { get; set; }

        public string Alias { get; set; }

        internal FormulaFilterDefinition()
        {
            
        }

        internal override void CopyFrom(FilterItemDefinition source)
        {
            var sourceFormulaItem = (FormulaFilterDefinition) source;
            Formula = sourceFormulaItem.Formula;
            Condition = sourceFormulaItem.Condition;
            FilterValue = sourceFormulaItem.FilterValue;

            base.CopyFrom(source);
        }
    }
}
