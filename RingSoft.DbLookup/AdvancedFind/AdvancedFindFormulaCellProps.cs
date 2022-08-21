using System;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;

namespace RingSoft.DbLookup.AdvancedFind
{
    public class AdvancedFindFormulaCellProps : DataEntryGridEditingCellProps
    {
        public string Formula { get; set; }

        public const int AdvancedFindFormulaHostId = 52;

        public AdvancedFindFormulaCellProps(DataEntryGridRow row, int columnId, string formula) : base(row, columnId)
        {
            Formula = formula;
        }

        protected override string GetDataValue(DataEntryGridRow row, int columnId, bool controlMode)
        {
            return "<Formula>";
        }

        public override int EditingControlId => AdvancedFindFormulaHostId;
    }
}
