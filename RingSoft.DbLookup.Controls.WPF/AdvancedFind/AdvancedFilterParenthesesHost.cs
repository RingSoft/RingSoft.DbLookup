using System.Media;
using System.Windows.Input;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DataEntryControls.WPF.DataEntryGrid;
using RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost;
using RingSoft.DbLookup.AdvancedFind;

namespace RingSoft.DbLookup.Controls.WPF.AdvancedFind
{
    public class AdvancedFilterParenthesesHost : DataEntryGridTextBoxHost
    {
        public AdvancedFilterParenthesesCellProps CellProps { get; set; }

        public AdvancedFilterParenthesesHost(DataEntryGrid grid) : base(grid)
        {
        }

        protected override void OnControlLoaded(StringEditControl control, DataEntryGridEditingCellProps cellProps,
            DataEntryGridCellStyle cellStyle)
        {
            CellProps = cellProps as AdvancedFilterParenthesesCellProps;

            control.KeyDown += (sender, args) =>
            {
                if (args.Key.GetCharFromKey() != CellProps.LimitChar)
                {
                    if (!(args.Key == Key.LeftShift || args.Key == Key.RightShift))
                    {
                        args.Handled = true;
                        SystemSounds.Exclamation.Play();
                    }
                }
            };

            base.OnControlLoaded(control, cellProps, cellStyle);
        }
    }
}
