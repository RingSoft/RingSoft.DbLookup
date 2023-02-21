using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.WPF.DataEntryGrid;
using RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost;
using RingSoft.DbLookup.AdvancedFind;

namespace RingSoft.DbLookup.Controls.WPF.AdvancedFind
{
    public class DataEntryGridAdvancedFindMemoHost : DataEntryGridEditingControlHost<AutoFillMemoCellControl>
    {
        public AdvancedFindMemoCellProps OriginalCellProps { get; set; }

        public bool EditMode { get; private set; }

        private bool _dataChanged;
        private bool _memoMode;
        
        public DataEntryGridAdvancedFindMemoHost(DataEntryGrid grid) : base(grid)
        {
        }

        public override DataEntryGridEditingCellProps GetCellValue()
        {
            return GetNewCellProps(Control.Text);
        }

        private AdvancedFindMemoCellProps GetNewCellProps(string text)
        {
            return new AdvancedFindMemoCellProps(Row, ColumnId, text)
            {
                FormMode = OriginalCellProps.FormMode,
                ControlMode = OriginalCellProps.ControlMode,
                ReadOnlyMode = OriginalCellProps.ReadOnlyMode,
                CellLostFocusType = OriginalCellProps.CellLostFocusType
            };

        }

        public override bool HasDataChanged()
        {
            return _dataChanged || Control.Text != Control.OriginalText;
        }

        public override void UpdateFromCellProps(DataEntryGridCellProps cellProps)
        {
            if (cellProps is AdvancedFindMemoCellProps advancedFindMemoCellProps)
            {
                Control.Text = advancedFindMemoCellProps.Text;
            }
        }

        public override bool IsDropDownOpen => false;

        protected override void OnControlLoaded(AutoFillMemoCellControl control, DataEntryGridEditingCellProps cellProps,
            DataEntryGridCellStyle cellStyle)
        {
            var advancedFindCellProps = (AdvancedFindMemoCellProps) cellProps;
            OriginalCellProps = advancedFindCellProps;
            SetCellText(advancedFindCellProps);

            Control.ShowMemoEditorWindow += (sender, args) =>
            {
                ShowMemoEditor();
                Control.Focus();

            };

        }

        protected virtual void ShowMemoEditor()
        {
            var memoEditor = new AdvancedFindGridMemoEditor(new DataEntryGridMemoValue(0) { Text = Control.Text });
            memoEditor.Owner = Window.GetWindow(Control);
            memoEditor.ShowInTaskbar = false;
            if (memoEditor.ShowDialog())
            {
                _dataChanged = OriginalCellProps.Text != memoEditor.MemoEditor.Text;
                var advancedFindCellProps = GetNewCellProps(memoEditor.MemoEditor.Text);
                SetCellText(advancedFindCellProps);
                Grid.CommitCellEdit(CellLostFocusTypes.KeyboardNavigation, false);
            }

        }

        private void SetCellText(AdvancedFindMemoCellProps cellProps)
        {
            {
                Control.OriginalText = Control.Text = cellProps.Text;

                if (cellProps.Text.Contains('\n'))
                {
                    _memoMode = true;
                    Control.TextBox.IsReadOnly = true;
                    Control.TextBox.Text = "<Multi-Line Caption>";
                }
                else
                {
                    Control.TextBox.Text = cellProps.Text;
                    _memoMode = false;
                }


            }
        }

        public override bool CanGridProcessKey(Key key)
        {
            switch (key)
            {
                case Key.Left:
                    if (EditMode && Control.TextBox.SelectionStart > 0)
                    {
                        return false;
                    }
                    break;
                case Key.Right:
                    if (EditMode && Control.TextBox.SelectionStart < Control.Text.Length)
                    {
                        return false;
                    }
                    break;
                case Key.F2:
                    if (_memoMode)
                    {
                        return true;
                    }
                    EditMode = true;
                    Control.TextBox.SelectionStart = Control.Text.Length;
                    Control.TextBox.SelectionLength = 0;
                    break;

            }
            return base.CanGridProcessKey(key);
        }
    }
}
