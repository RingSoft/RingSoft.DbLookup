using System;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;

namespace RingSoft.DbMaintenance
{
    public class AdvancedFindNewColumnRow : AdvancedFindColumnRow
    {
        public AdvancedFindNewColumnRow(AdvancedFindColumnsManager manager) : base(manager)
        {
            IsNew = true;
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (AdvancedFindColumnColumns)columnId;

            switch (column)
            {
                case AdvancedFindColumnColumns.Table:
                    break;
                case AdvancedFindColumnColumns.Field:
                    break;
                case AdvancedFindColumnColumns.Name:
                    return new DataEntryGridTextCellProps(this, columnId, "Click 'Add Column' to add a new column here.");
                case AdvancedFindColumnColumns.PercentWidth:
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
    }
}
