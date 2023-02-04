using System;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup.AdvancedFind;

namespace RingSoft.DbMaintenance
{
    public class AdvancedFindFormulaColumnRow : AdvancedFindColumnRow
    {
        public AdvancedFindFormulaColumnRow(AdvancedFindColumnsManager manager) : base(manager)
        {
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (AdvancedFindColumnColumns)columnId;
            switch (column)
            {
                case AdvancedFindColumnColumns.Field:
                    return new AdvancedFindMemoCellProps(this, columnId, "<Formula>");
            }
            return base.GetCellProps(columnId);
        }
    }
}
