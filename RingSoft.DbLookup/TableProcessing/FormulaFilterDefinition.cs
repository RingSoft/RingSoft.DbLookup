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

        protected internal override string GetReportBeginTextPrintMode(LookupDefinitionBase lookupDefinition)
        {
            if (!Path.IsNullOrEmpty())
            {
                var foundItem = lookupDefinition.AdvancedFindTree.ProcessFoundTreeViewItem(Path);
                if (foundItem != null)
                {
                    return $"{foundItem.Name}.{Description}";
                }

            }
            return $"{lookupDefinition.TableDefinition.Description}.{Description} Formula ";
        }

        public override string GetReportText(LookupDefinitionBase lookupDefinition, bool printMode)
        {
            var result = string.Empty;
            if (printMode)
            {
                result += GetReportBeginTextPrintMode(lookupDefinition) + " ";
            }

            if (Condition == null)
            {
                result = "<Complex Formula>";
            }
            else
            {
                result += GetConditionText(Condition.Value) + " ";
                switch (Condition)
                {
                    case Conditions.EqualsNull:
                    case Conditions.NotEqualsNull:
                        result = result.Trim();
                        break;
                    default:
                        if (ValueType == ValueTypes.DateTime)
                        {
                            var dateSearchValue = GetDateReportText();
                            result += dateSearchValue;
                        }
                        else
                        {
                            result += Value;
                        }
                        break;
                }
            }

            return result;
        }

        public override bool LoadFromEntity(AdvancedFindFilter entity, LookupDefinitionBase lookupDefinition,
            string path = "")
        {
            Formula = entity.Formula;
            Description = entity.FormulaDisplayValue;
            DataType = (FieldDataTypes)entity.FormulaDataType;
            Condition = (Conditions)entity.Operand;
            FilterValue = entity.SearchForValue;
            Path = entity.Path;
            ValueType = DataType.ConvertFieldTypeIntoValueType();

            var result = base.LoadFromEntity(entity, lookupDefinition);

            if (Path.IsNullOrEmpty())
            {
                Alias = TableFilterDefinition.TableDefinition.TableName;
            }
            else
            {
                var foundItem = lookupDefinition.AdvancedFindTree.ProcessFoundTreeViewItem(Path, TreeViewType.Formula);
                Alias = lookupDefinition.AdvancedFindTree.MakeIncludes(foundItem).LookupJoin.JoinDefinition.Alias;
            }

            return result;
        }

        public override void SaveToEntity(AdvancedFindFilter entity)
        {
            entity.Formula = Formula;
            entity.FormulaDisplayValue = Description;
            entity.FormulaDataType = (byte)DataType;
            entity.Operand = (byte)Condition.Value;

            base.SaveToEntity(entity);
            //entity.SearchForValue = FilterValue;
        }

        public override string LoadFromFilterReturn(AdvancedFilterReturn filterReturn, TreeViewItem treeViewItem)
        {
            Formula = filterReturn.Formula;
            Condition = filterReturn.Condition;
            Description = filterReturn.FormulaDisplayValue;
            DataType = filterReturn.FormulaValueType;
            Path = filterReturn.Path;
            FilterValue = filterReturn.SearchValue;
            ValueType = filterReturn.FormulaValueType.ConvertFieldTypeIntoValueType();

            if (Path.IsNullOrEmpty())
            {
                Alias = TableFilterDefinition.TableDefinition.TableName;
            }
            else
            {
                Alias = treeViewItem.BaseTree.MakeIncludes(treeViewItem).LookupJoin.JoinDefinition.Alias;
            }
            var value = base.LoadFromFilterReturn(filterReturn, treeViewItem);
            DisplayValue = Value = value;
            return value;
        }

        public override string ToString()
        {
            return Formula;
        }

        public override void SaveToFilterReturn(AdvancedFilterReturn filterReturn)
        {
            filterReturn.Formula = Formula;
            if (Condition != null) filterReturn.Condition = Condition.Value;
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
