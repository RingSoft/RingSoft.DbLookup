using System;
using System.Linq;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;

namespace RingSoft.DbMaintenance
{
    public class AdvancedFindNewFilterRow : AdvancedFindFilterRow
    {
        private bool _allowDelete;
        public AdvancedFindNewFilterRow(AdvancedFindFiltersManager manager) : base(manager)
        {
            IsNew = true;
        }

        public void SetAllowDelete(bool allowDelete)
        {
            _allowDelete = allowDelete;
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (AdvancedFindFiltersManager.FilterColumns)columnId;

            switch (column)
            {
                case AdvancedFindFiltersManager.FilterColumns.LeftParentheses:
                    break;
                case AdvancedFindFiltersManager.FilterColumns.Table:
                    break;
                case AdvancedFindFiltersManager.FilterColumns.Field:
                    break;
                case AdvancedFindFiltersManager.FilterColumns.Search:
                    return new DataEntryGridTextCellProps(this, columnId, "Click 'Add Filter' to add a filter here.");
                case AdvancedFindFiltersManager.FilterColumns.RightParentheses:
                    break;
                case AdvancedFindFiltersManager.FilterColumns.EndLogic:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return new DataEntryGridTextCellProps(this, columnId);
        }

        public override DataEntryGridCellStyle GetCellStyle(int columnId)
        {
            return new DataEntryGridCellStyle { State = DataEntryGridCellStates.Disabled };
        }

        public override void Dispose()
        {
            //base.Dispose();
        }
    }
}
