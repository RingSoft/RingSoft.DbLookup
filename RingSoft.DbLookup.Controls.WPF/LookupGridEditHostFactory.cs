using RingSoft.DataEntryControls.WPF.DataEntryGrid;
using RingSoft.DataEntryControls.WPF.DataEntryGrid.ControlHost;

namespace RingSoft.DbLookup.Controls.WPF
{
    public class LookupGridEditHostFactory : DataEntryGridHostFactory
    {
        public override DataEntryGridControlHostBase GetControlHost(DataEntryGrid grid, int editingControlHostId)
        {
            if (editingControlHostId == DataEntryGridAutoFillCellProps.AutoFillControlHostId)
                return new DataEntryGridAutoFillHost(grid);
            
            return base.GetControlHost(grid, editingControlHostId);
        }
    }
}
