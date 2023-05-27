using System.Windows;
using System.Windows.Controls;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.WPF.DataEntryGrid;
using RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost;
using System.Windows.Input;
using System;

namespace RingSoft.DbLookup.Controls.WPF
{
    public class DataEntryGridAutoFillHost : DataEntryGridEditingControlHost<AutoFillControl>
    {
        public override bool IsDropDownOpen => Control.ContainsBoxIsOpen;

        public override bool AllowReadOnlyEdit => true;

        public bool EditMode { get; private set; }

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

                if (Control.Value.Text != AutoFillCellProps.AutoFillValue.Text)
                {
                    return true;
                }
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
            Control.SetReadOnlyMode(true);
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
            switch (cellStyle.State)
            {
                case DataEntryGridCellStates.Enabled:
                    break;
                case DataEntryGridCellStates.ReadOnly:
                case DataEntryGridCellStates.Disabled:
                    _gridReadOnlyMode = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (_gridReadOnlyMode)
            {
                Control.TextBox.Focusable = false;
                Control.KeyDown += (sender, args) =>
                {
                    if (args.Key == Key.F5)
                    {
                        Control.ShowLookupWindow();
                        args.Handled = true;
                    }
                };
                Control.SetReadOnlyMode(true);
                //Control.Button.Focus();
            }
        }

        protected override void ImportDataGridCellProperties(DataGridCell dataGridCell)
        {
            base.ImportDataGridCellProperties(dataGridCell);
            if (_gridReadOnlyMode)
            {
                dataGridCell.BorderThickness = new Thickness(1);
            }
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
                case Key.Left:
                    if (EditMode && Control.SelectionStart > 0)
                    {
                        return false;
                    }
                    break;
                case Key.Right:
                    if (EditMode && Control.SelectionStart < Control.EditText.Length)
                    {
                        return false;
                    }
                    break;
                case Key.F2:
                    EditMode = true;
                    Control.SelectionStart = Control.EditText.Length;
                    Control.SelectionLength = 0;
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
