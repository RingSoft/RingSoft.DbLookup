using RingSoft.DbLookup;
using RingSoft.DbLookup.TableProcessing;

namespace RingSoft.DbMaintenance
{
    public class AdvancedFindFormulaFilterRow : AdvancedFindFilterRow
    {
        public string Formula { get; private set; }
        public FieldDataTypes FormulaDataType { get; private set; }

        public AdvancedFindFormulaFilterRow(AdvancedFindFiltersManager manager) : base(manager)
        {
        }

        public override void LoadFromFilterDefinition(FilterItemDefinition filter, bool isFixed, int rowIndex)
        {
            if (filter is FormulaFilterDefinition formulaFilter)
            {
                Field = $"{formulaFilter.Description} Formula";
            }
            base.LoadFromFilterDefinition(filter, isFixed, rowIndex);
        }
    }
}
