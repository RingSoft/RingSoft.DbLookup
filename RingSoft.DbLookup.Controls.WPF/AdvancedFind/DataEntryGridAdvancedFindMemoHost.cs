using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.WPF.DataEntryGrid;
using RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost;
using RingSoft.DbLookup.AdvancedFind;

namespace RingSoft.DbLookup.Controls.WPF.AdvancedFind
{
    public class DataEntryGridAdvancedFindMemoHost : DataEntryGridEditingControlHost<AutoFillMemoCellControl>
    {
        public AdvancedFindMemoCellProps OriginalCellProps { get; set; }

        private bool _dataChanged;
        
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
                    Control.TextBox.IsReadOnly = true;
                    Control.TextBox.Text = "<Multi-Line Caption>";
                }
                else
                {
                    Control.TextBox.Text = cellProps.Text;
                }


            }
        }
    }
}
