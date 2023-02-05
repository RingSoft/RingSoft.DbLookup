using MySqlX.XDevAPI.Common;
using RingSoft.DataEntryControls.Engine;
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

        public override TreeViewType TreeViewType => TreeViewType.Formula;

        internal override void CopyFrom(FilterItemDefinition source)
        {
            var sourceFormulaItem = (FormulaFilterDefinition) source;
            Formula = sourceFormulaItem.Formula;
            Condition = sourceFormulaItem.Condition;
            FilterValue = sourceFormulaItem.FilterValue;

            base.CopyFrom(source);
        }

        public override string GetReportText(LookupDefinitionBase lookupDefinition, bool printMode = false)
        {
            var result = string.Empty;
            if (printMode)
            {
                result += GetReportBeginTextPrintMode(lookupDefinition);
            }

            result += GetConditionText(Condition.Value) + " ";
            switch (Condition)
            {
                case Conditions.EqualsNull:
                case Conditions.NotEqualsNull:
                    result = result.Trim();
                    break;
                default:
                    result += FilterValue;
                    break;
            }
            return result;
        }

        public override bool LoadFromEntity(AdvancedFindFilter entity, LookupDefinitionBase lookupDefinition)
        {
            return false;
            return base.LoadFromEntity(entity, lookupDefinition);
        }

        public override void SaveToEntity(AdvancedFindFilter entity)
        {
            base.SaveToEntity(entity);
        }

        public override string LoadFromFilterReturn(AdvancedFilterReturn filterReturn, TreeViewItem treeViewItem)
        {
            Formula = filterReturn.Formula;
            Condition = filterReturn.Condition;
            Description = filterReturn.FormulaDisplayValue;
            DataType = filterReturn.FormulaValueType;
            Path = filterReturn.Path;
            FilterValue = filterReturn.SearchValue;

            if (Path.IsNullOrEmpty())
            {
                Alias = TableFilterDefinition.TableDefinition.TableName;
            }
            else
            {
                Alias = treeViewItem.BaseTree.MakeIncludes(treeViewItem).LookupJoin.JoinDefinition.Alias;
            }
            var value = base.LoadFromFilterReturn(filterReturn, treeViewItem);
            Value = value;
            return value;
        }

        public override string ToString()
        {
            return Formula;
        }

        public override void SaveToFilterReturn(AdvancedFilterReturn filterReturn)
        {
            filterReturn.Formula = Formula;
            filterReturn.Condition = Condition.Value;
            filterReturn.FormulaDisplayValue = Description;
            filterReturn.FormulaValueType = DataType;
            filterReturn.SearchValue = FilterValue;

            base.SaveToFilterReturn(filterReturn);
        }

        public override FilterItemDefinition GetNewFilterItemDefinition()
        {
            return new FormulaFilterDefinition(TableFilterDefinition);
        }

        internal override string GetNewPath()
        {
            return string.Empty;
        }
    }
}
