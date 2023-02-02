using MySqlX.XDevAPI.Common;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
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
        public string Formula { get; set; }

        public Conditions? Condition { get; set; }

        public string FilterValue { get; set; }

        public FieldDataTypes DataType { get; set; }

        public string Alias { get; set; }

        public string Description { get; set; }

        internal FormulaFilterDefinition(TableFilterDefinitionBase tableFilterDefinition) : base(tableFilterDefinition)
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

        public override string GetReportText()
        {
            var result = GetConditionText(Condition.Value) + " ";
            result += FilterValue;
            return result;
        }

        public override void LoadFromEntity(AdvancedFindFilter entity, LookupDefinitionBase lookupDefinition)
        {
            base.LoadFromEntity(entity, lookupDefinition);
        }

        public override AdvancedFindFilter SaveToEntity(LookupDefinitionBase lookupDefinition)
        {
            return base.SaveToEntity(lookupDefinition);
        }

        public override void LoadFromFilterReturn(AdvancedFilterReturn filterReturn, TreeViewItem treeViewItem)
        {
            base.LoadFromFilterReturn(filterReturn, treeViewItem);
        }

        public override AdvancedFilterReturn SaveToFilterReturn()
        {
            return base.SaveToFilterReturn();
        }

        public override string ToString()
        {
            return Formula;
        }
    }
}
