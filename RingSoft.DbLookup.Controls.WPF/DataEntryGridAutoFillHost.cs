using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.NorthwindApp.Library;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DataEntryControls.WPF.DataEntryGrid;
using RingSoft.DataEntryControls.WPF.DataEntryGrid.ControlHost;
using System.Windows.Input;
using System.Windows.Media;

namespace RingSoft.DbLookup.Controls.WPF
{
    public class DataEntryGridAutoFillHost : DataEntryGridControlHost<AutoFillControl>
    {
        public override bool IsDropDownOpen => Control.ContainsBoxIsOpen;

        public DataEntryGridAutoFillCellProps AutoFillCellProps { get; private set; }

        public DataEntryGridAutoFillHost(DataEntryGrid grid) : base(grid)
        {
        }

                

        public override DataEntryGridCellProps GetCellValue()
        {
            return new DataEntryGridAutoFillCellProps(Row, ColumnId,
                Control.Setup, Control.Value);
        }

        public override bool HasDataChanged()
        {
            if (Control.Value == null && AutoFillCellProps.AutoFillValue != null)
                return true;

            if (Control.Value != null && AutoFillCellProps.AutoFillValue == null)
                return true;

            if (Control.Value == null && AutoFillCellProps.AutoFillValue == null)
                return false;

            if (AutoFillCellProps.AutoFillValue != null && Control.Value != null)
            {
                if (Control.Value.PrimaryKeyValue.ContainsValidData() &&
                    AutoFillCellProps.AutoFillValue.PrimaryKeyValue.ContainsValidData())
                {
                    return !Control.Value.PrimaryKeyValue.IsEqualTo(AutoFillCellProps.AutoFillValue
                        .PrimaryKeyValue);
                }

                return Control.Value.Text != AutoFillCellProps.AutoFillValue.Text;
            }

            return false;
        }

        protected override void OnControlLoaded(AutoFillControl control, DataEntryGridCellProps cellProps,
            DataEntryGridCellStyle cellStyle)
        {
            AutoFillCellProps = (DataEntryGridAutoFillCellProps)cellProps;
            Control.Setup = AutoFillCellProps.AutoFillSetup;
            Control.Value = AutoFillCellProps.AutoFillValue;

            if (!cellStyle.SelectionColor.IsEmpty)
                Control.SelectionBrush = new SolidColorBrush(cellStyle.SelectionColor.GetMediaColor());

            Control.ControlDirty += (sender, args) => OnControlDirty();
        }

        public override bool CanGridProcessKey(Key key)
        {
            switch (key)
            {
                case Key.Escape:
                case Key.Up:
                case Key.Down:
                    if (Control.ContainsBoxIsOpen)
                        return false;
                    break;
            }
            return base.CanGridProcessKey(key);
        }
    }
}
