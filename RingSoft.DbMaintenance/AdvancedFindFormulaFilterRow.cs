using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AdvancedFind;
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

        public override void LoadFromFilterReturn(AdvancedFilterReturn advancedFilterReturn)
        {
            TreeViewItem treeViewItem = null;

            if (!advancedFilterReturn.Path.IsNullOrEmpty())
            {
                treeViewItem =
                    Manager.ViewModel.LookupDefinition.AdvancedFindTree.ProcessFoundTreeViewItem(advancedFilterReturn.Path,
                        TreeViewType.Formula);
            }

            SetupTable(treeViewItem);
            Field = $"{advancedFilterReturn.FormulaDisplayValue} Formula";

            if (FilterItemDefinition == null)
            {
                FilterItemDefinition = Manager.ViewModel.LookupDefinition.FilterDefinition
                    .AddUserFilter(advancedFilterReturn.Formula
                    , advancedFilterReturn.Condition
                    , advancedFilterReturn.SearchValue);
            }

            FilterItemDefinition.LoadFromFilterReturn(advancedFilterReturn, treeViewItem);

            base.LoadFromFilterReturn(advancedFilterReturn);
        }
    }
}
