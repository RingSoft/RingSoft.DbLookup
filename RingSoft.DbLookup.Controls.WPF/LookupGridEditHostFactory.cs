using RingSoft.DataEntryControls.WPF.DataEntryGrid;
using RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.Controls.WPF.AdvancedFind;

namespace RingSoft.DbLookup.Controls.WPF
{
    public class LookupGridEditHostFactory : DataEntryGridHostFactory
    {
        public override DataEntryGridEditingControlHostBase GetControlHost(DataEntryGrid grid, int editingControlHostId)
        {
            if (editingControlHostId == DataEntryGridAutoFillCellProps.AutoFillControlHostId)
                return new DataEntryGridAutoFillHost(grid);

            if (editingControlHostId == AdvancedFindFormulaCellProps.AdvancedFindFormulaHostId)
            {
                return new DataEntryGridFormulaHost(grid);
            }
            
            return base.GetControlHost(grid, editingControlHostId);
        }
    }
}
