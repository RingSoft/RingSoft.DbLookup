using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.WPF.DataEntryGrid;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.Controls.WPF.AdvancedFind
{
    public class AdvancedFindFilterHost : DataEntryGridAdvancedFindMemoHost
    {
        protected string Text { get; set; }

        protected AdvancedFindFilterCellProps CellProps { get; set; }

        public AdvancedFindFilterHost(DataEntryGrid grid) : base(grid)
        {
        }

        protected override void OnControlLoaded(AutoFillMemoCellControl control, DataEntryGridEditingCellProps cellProps,
            DataEntryGridCellStyle cellStyle)
        {
            CellProps = cellProps as AdvancedFindFilterCellProps;
            
            base.OnControlLoaded(control, cellProps, cellStyle);
            Control.Text = CellProps.Text;
            control.TextBox.IsReadOnly = true;

        }

        protected override void ShowMemoEditor()
        {
            
        }
    }
}
