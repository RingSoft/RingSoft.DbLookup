using RingSoft.DataEntryControls.WPF.DataEntryGrid;
using RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost;

namespace RingSoft.DbLookup.Controls.WPF
{
    public class LookupGridEditHostFactory : DataEntryGridHostFactory
    {
        public override DataEntryGridEditingControlHostBase GetControlHost(DataEntryGrid grid, int editingControlHostId)
        {
            if (editingControlHostId == DataEntryGridAutoFillCellProps.AutoFillControlHostId)
                return new DataEntryGridAutoFillHost(grid);
            
            return base.GetControlHost(grid, editingControlHostId);
        }
    }
}
