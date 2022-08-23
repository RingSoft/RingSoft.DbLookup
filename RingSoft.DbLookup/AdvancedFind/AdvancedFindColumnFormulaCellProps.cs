using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup.Lookup;

namespace RingSoft.DbLookup.AdvancedFind
{
    public class AdvancedFindColumnFormulaCellProps : DataEntryGridEditingCellProps
    {
        public LookupFormulaColumnDefinition LookupFormulaColumn { get; set; }

        public const int ColumnFormulaCellId = 53;
        public AdvancedFindColumnFormulaCellProps(DataEntryGridRow row, int columnId,
            LookupFormulaColumnDefinition formulaColumn) : base(row, columnId)
        {
            LookupFormulaColumn = formulaColumn;
        }

        protected override string GetDataValue(DataEntryGridRow row, int columnId, bool controlMode)
        {
            return "<Formula>";
        }

        public override int EditingControlId => ColumnFormulaCellId;
    }
}
