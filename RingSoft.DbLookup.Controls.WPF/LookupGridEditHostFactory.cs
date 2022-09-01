using RingSoft.DataEntryControls.WPF.DataEntryGrid;
using RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.Controls.WPF.AdvancedFind;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.Controls.WPF
{
    public class LookupGridEditHostFactory : DataEntryGridHostFactory
    {
        public override DataEntryGridEditingControlHostBase GetControlHost(DataEntryGrid grid, int editingControlHostId)
        {
            if (editingControlHostId == DataEntryGridAutoFillCellProps.AutoFillControlHostId)
                return new DataEntryGridAutoFillHost(grid);

            if (editingControlHostId == AdvancedFindColumnFormulaCellProps.ColumnFormulaCellId)
            {
                return new DataEntryGridAdvancedFindFormulaColumnHost(grid);
            }

            if (editingControlHostId == AdvancedFindMemoCellProps.AdvancedFindMemoHostId)
            {
                return new DataEntryGridAdvancedFindMemoHost(grid);
            }

            if (editingControlHostId == AdvancedFindFilterCellProps.FilterControlId)
            {
                return new AdvancedFindFilterHost(grid);
            }

            if (editingControlHostId == AdvancedFilterParenthesesCellProps.ParenthesesHostId)
            {
                return new AdvancedFilterParenthesesHost(grid);
            }


            return base.GetControlHost(grid, editingControlHostId);
        }
    }
}
