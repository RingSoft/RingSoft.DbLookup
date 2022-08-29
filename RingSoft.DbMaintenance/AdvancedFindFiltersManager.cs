using System.Linq;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.Lookup;

namespace RingSoft.DbMaintenance
{
    public class AdvancedFindFiltersManager: DbMaintenanceDataEntryGridManager<AdvancedFindFilter>
    {
        public const int LeftParenthesesColumnId = 1;
        public const int TableColumnId = 2;
        public const int FieldColumnId = 3;
        public const int SearchColumnId = 4;
        public const int RightParenthesesColumnId = 5;
        public const int EndLogicColumnId = 6;

        public enum FilterColumns
        {
            LeftParentheses = LeftParenthesesColumnId,
            Table = TableColumnId,
            Field = FieldColumnId,
            Search = SearchColumnId,
            RightParentheses = RightParenthesesColumnId,
            EndLogic = EndLogicColumnId
        }

        public AdvancedFindViewModel ViewModel { get; set; }

        public AdvancedFindFiltersManager(AdvancedFindViewModel viewModel) : base(viewModel)
        {
            ViewModel = viewModel;
        }

        protected override DataEntryGridRow GetNewRow()
        {
            return new AdvancedFindFilterRow(this);
        }

        protected override DbMaintenanceDataEntryGridRow<AdvancedFindFilter> ConstructNewRowFromEntity(AdvancedFindFilter entity)
        {
            return new AdvancedFindFilterRow(this);
        }

        public void LoadFromLookupDefinition(LookupDefinitionBase lookupDefinition)
        {
            var rowIndex = 0;
            foreach (var fixedFilter in lookupDefinition.FilterDefinition.FixedFilters)
            {
                var row = GetNewRow() as AdvancedFindFilterRow;
                row.LoadFromFilterDefinition(fixedFilter, true, rowIndex);
                AddRow(row);
                if (rowIndex == lookupDefinition.FilterDefinition.FixedFilters.Count - 1)
                {
                    var theEnd = !lookupDefinition.FilterDefinition.UserFilters.Any();
                    row.FinishOffFilter(true, theEnd);
                }
                rowIndex++;
            }
        }
    }
}
