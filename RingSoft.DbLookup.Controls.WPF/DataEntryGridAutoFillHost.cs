using System.Windows;
using System.Windows.Controls;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.WPF.DataEntryGrid;
using RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost;
using System.Windows.Input;

namespace RingSoft.DbLookup.Controls.WPF
{
    public class DataEntryGridAutoFillHost : DataEntryGridEditingControlHost<AutoFillControl>
    {
        public override bool IsDropDownOpen => Control.ContainsBoxIsOpen;

        public DataEntryGridAutoFillCellProps AutoFillCellProps { get; private set; }

        private bool _gridReadOnlyMode;

        public DataEntryGridAutoFillHost(DataEntryGrid grid) : base(grid)
        {
        }

                

        public override DataEntryGridEditingCellProps GetCellValue()
        {
            return new DataEntryGridAutoFillCellProps(Row, ColumnId,
                Control.Setup, Control.Value);
        }

        public override bool HasDataChanged()
        {
            if (AutoFillCellProps.AlwaysUpdateOnSelect)
            {
                return true;
            }

            if (Control.Value == null && AutoFillCellProps.AutoFillValue != null)
                return true;

            if (Control.Value != null && AutoFillCellProps.AutoFillValue == null)
                return true;

            if (Control.Value == null && AutoFillCellProps.AutoFillValue == null)
                return false;

            if (AutoFillCellProps.AutoFillValue != null && Control.Value != null)
            {
                var cellPrimaryKeyIsValid = AutoFillCellProps.AutoFillValue.PrimaryKeyValue.IsValid;
                var controlPrimaryKeyIsValid = AutoFillCellProps.AutoFillValue.PrimaryKeyValue.IsValid;

                if (controlPrimaryKeyIsValid != cellPrimaryKeyIsValid)
                    return true;

                if (controlPrimaryKeyIsValid)
                {
                    return !Control.Value.PrimaryKeyValue.IsEqualTo(AutoFillCellProps.AutoFillValue
                        .PrimaryKeyValue);
                }

                return Control.Value.Text != AutoFillCellProps.AutoFillValue.Text;
            }

            return false;
        }

        public override void UpdateFromCellProps(DataEntryGridCellProps cellProps)
        {
            AutoFillCellProps = (DataEntryGridAutoFillCellProps)cellProps;
        }
        
        protected override void OnControlLoaded(AutoFillControl control, DataEntryGridEditingCellProps cellProps,
            DataEntryGridCellStyle cellStyle)
        {
            AutoFillCellProps = (DataEntryGridAutoFillCellProps)cellProps;
            Control.Setup = AutoFillCellProps.AutoFillSetup;
            Control.Value = AutoFillCellProps.AutoFillValue;
            var displayStyle = GetCellDisplayStyle();
            if (displayStyle.SelectionBrush != null)
                Control.SelectionBrush = displayStyle.SelectionBrush;

            Control.ControlDirty += (sender, args) => OnControlDirty();

            Control.SetReadOnlyMode(_gridReadOnlyMode);
            Control.TabOutAfterLookupSelect = AutoFillCellProps.TabOnSelect;
            Control.LookupSelect += (sender, args) =>
            {
                if (!AutoFillCellProps.TabOnSelect)
                {
                    OnUpdateSource(GetCellValue());
                }
            };
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

        public override bool SetReadOnlyMode(bool readOnlyMode)
        {
            _gridReadOnlyMode = readOnlyMode;
            
            if (readOnlyMode)
                return true;

            return base.SetReadOnlyMode(readOnlyMode);
        }
    }
}
