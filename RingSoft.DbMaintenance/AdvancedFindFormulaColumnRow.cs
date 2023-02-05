using System;
using MySqlX.XDevAPI.Relational;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.Lookup;

namespace RingSoft.DbMaintenance
{
    public class AdvancedFindFormulaColumnRow : AdvancedFindColumnRow
    {
        public LookupFormulaColumnDefinition FormulaColumn { get; private set; }

        public AdvancedFindFormulaColumnRow(AdvancedFindColumnsManager manager) : base(manager)
        {
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (AdvancedFindColumnColumns)columnId;
            switch (column)
            {
                case AdvancedFindColumnColumns.Field:
                    return new AdvancedFindColumnFormulaCellProps(this, columnId, FormulaColumn);
            }

            return base.GetCellProps(columnId);
        }

        public override void SetCellValue(DataEntryGridEditingCellProps value)
        {
            var column = (AdvancedFindColumnColumns)value.ColumnId;
            switch (column)
            {
                case AdvancedFindColumnColumns.Table:
                    break;
                case AdvancedFindColumnColumns.Field:
                    if (value is AdvancedFindColumnFormulaCellProps formulaCellProps)
                    {
                        Manager.ViewModel.ResetLookup();
                        return;
                    }
                    break;
                case AdvancedFindColumnColumns.Name:
                    break;
                case AdvancedFindColumnColumns.PercentWidth:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            base.SetCellValue(value);
        }

        public override void LoadFromColumnDefinition(LookupColumnDefinitionBase column)
        {
            if (column is LookupFormulaColumnDefinition lookupFormulaColumn)
            {
                FormulaColumn = lookupFormulaColumn;
            }
            base.LoadFromColumnDefinition(column);
        }

        public override DataEntryGridCellStyle GetCellStyle(int columnId)
        {
            var column = (AdvancedFindColumnColumns)columnId;
            switch (column)
            {
                case AdvancedFindColumnColumns.Table:
                    break;
                case AdvancedFindColumnColumns.Field:
                    return new DataEntryGridCellStyle() { State = DataEntryGridCellStates.Enabled };
                case AdvancedFindColumnColumns.Name:
                    break;
                case AdvancedFindColumnColumns.PercentWidth:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return base.GetCellStyle(columnId);
        }
    }
}
