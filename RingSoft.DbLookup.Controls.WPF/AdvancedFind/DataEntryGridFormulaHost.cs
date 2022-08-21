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
    internal class DataEntryGridFormulaHost : DataEntryGridEditingControlHost<AutoFillFormulaCellControl>
    {
        public DataEntryGridFormulaHost(DataEntryGrid grid) : base(grid)
        {
        }

        public override DataEntryGridEditingCellProps GetCellValue()
        {
            return new AdvancedFindFormulaCellProps(Row, ColumnId, Control.Formula);
        }

        public override bool HasDataChanged()
        {
            return Control.Formula != Control.OriginalFormula;
        }

        public override void UpdateFromCellProps(DataEntryGridCellProps cellProps)
        {
            if (cellProps is AdvancedFindFormulaCellProps advancedFindFormulaCellProps)
            {
                Control.Formula = advancedFindFormulaCellProps.Formula;
            }
        }

        public override bool IsDropDownOpen => false;

        protected override void OnControlLoaded(AutoFillFormulaCellControl control, DataEntryGridEditingCellProps cellProps,
            DataEntryGridCellStyle cellStyle)
        {
            if (cellProps is AdvancedFindFormulaCellProps advancedFindFormulaCellProps)
            {
                control.OriginalFormula = control.Formula = advancedFindFormulaCellProps.Formula;
            }

            control.TextBox.Text = "<Formula>";
            control.TextBox.IsReadOnly = true;
        }
    }
}
