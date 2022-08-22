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
    internal class DataEntryGridAdvancedFindMemoHost : DataEntryGridEditingControlHost<AutoFillMemoCellControl>
    {
        public AdvancedFindMemoCellProps OriginalCellProps { get; set; }

        private bool _dataChanged;
        
        public DataEntryGridAdvancedFindMemoHost(DataEntryGrid grid) : base(grid)
        {
        }

        public override DataEntryGridEditingCellProps GetCellValue()
        {
            return new AdvancedFindMemoCellProps(Row, ColumnId, Control.Text)
            {
                FormMode = OriginalCellProps.FormMode,
                ControlMode = OriginalCellProps.ControlMode,
                ReadOnlyMode = OriginalCellProps.ReadOnlyMode,
                CellLostFocusType = OriginalCellProps.CellLostFocusType
            };
        }

        public override bool HasDataChanged()
        {
            return _dataChanged || Control.Text != Control.OriginalFormula;
        }

        public override void UpdateFromCellProps(DataEntryGridCellProps cellProps)
        {
            if (cellProps is AdvancedFindMemoCellProps advancedFindFormulaCellProps)
            {
                Control.Text = advancedFindFormulaCellProps.Text;
            }
        }

        public override bool IsDropDownOpen => false;

        protected override void OnControlLoaded(AutoFillMemoCellControl control, DataEntryGridEditingCellProps cellProps,
            DataEntryGridCellStyle cellStyle)
        {
            var advancedFindCellProps = (AdvancedFindMemoCellProps) cellProps;
            OriginalCellProps = advancedFindCellProps;
            SetCellText(advancedFindCellProps);

            Control.MemoChanged += (sender, args) =>
            {
                _dataChanged = true;
                SetCellText(GetCellValue() as AdvancedFindMemoCellProps);
                Grid.CommitCellEdit(CellLostFocusTypes.KeyboardNavigation, false);
                Control.Focus();
            };

        }

        private void SetCellText(AdvancedFindMemoCellProps cellProps)
        {
            {
                Control.OriginalFormula = Control.Text = cellProps.Text;

                switch (cellProps.FormMode)
                {
                    case AdvancedFindMemoCellProps.MemoFormMode.Formula:
                        Control.TextBox.IsReadOnly = true;
                        Control.TextBox.Text = "<Formula>";
                        break;
                    case AdvancedFindMemoCellProps.MemoFormMode.Caption:
                        if (cellProps.Text.Contains('\n'))
                        {
                            Control.TextBox.IsReadOnly = true;
                            Control.TextBox.Text = "<Multi-Line Caption>";
                        }
                        else
                        {
                            Control.TextBox.Text = cellProps.Text;
                        }

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

            }
        }
    }
}
