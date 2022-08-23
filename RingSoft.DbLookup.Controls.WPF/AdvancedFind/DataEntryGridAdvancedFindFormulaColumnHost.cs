using System;
using System.Windows;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.WPF.DataEntryGrid;
using RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;

namespace RingSoft.DbLookup.Controls.WPF.AdvancedFind
{
    public class DataEntryGridAdvancedFindFormulaColumnHost : DataEntryGridEditingControlHost<AutoFillMemoCellControl>
    {
        public AdvancedFindColumnFormulaCellProps OriginalCellProps { get; set; }

        public LookupFormulaColumnDefinition LookupFormulaColumnDefinition { get; set; }

        private FieldDataTypes _dataType;
        private DecimalFieldTypes _formatType;
        private string _formula;

        public DataEntryGridAdvancedFindFormulaColumnHost(DataEntryGrid grid) : base(grid)
        {
        }

        public override bool IsDropDownOpen => false;
        public override DataEntryGridEditingCellProps GetCellValue()
        {
            return new AdvancedFindColumnFormulaCellProps(Row, ColumnId, LookupFormulaColumnDefinition);
        }

        public override bool HasDataChanged()
        {
            if (LookupFormulaColumnDefinition.Formula != _formula ||
                LookupFormulaColumnDefinition.DataType != _dataType ||
                LookupFormulaColumnDefinition.DecimalFieldType != _formatType)
            {
                return true;
            }
            return false;
        }

        public override void UpdateFromCellProps(DataEntryGridCellProps cellProps)
        {
            if (cellProps is AdvancedFindColumnFormulaCellProps advancedFindColumnFormulaCellProps)
            {
                OriginalCellProps = advancedFindColumnFormulaCellProps;
                LookupFormulaColumnDefinition = advancedFindColumnFormulaCellProps.LookupFormulaColumn;
                _formula = advancedFindColumnFormulaCellProps.LookupFormulaColumn.OriginalFormula;
                _dataType = advancedFindColumnFormulaCellProps.LookupFormulaColumn.DataType;
                _formatType = advancedFindColumnFormulaCellProps.LookupFormulaColumn.DecimalFieldType;
            }
        }

        protected override void OnControlLoaded(AutoFillMemoCellControl control, DataEntryGridEditingCellProps cellProps,
            DataEntryGridCellStyle cellStyle)
        {
            Control.TextBox.IsReadOnly = true;
            Control.TextBox.Text = "<Formula>";

            if (cellProps is AdvancedFindColumnFormulaCellProps advancedFindColumnFormulaCellProps)
            {
                OriginalCellProps = advancedFindColumnFormulaCellProps;
                LookupFormulaColumnDefinition = advancedFindColumnFormulaCellProps.LookupFormulaColumn;
                _formula = advancedFindColumnFormulaCellProps.LookupFormulaColumn.OriginalFormula;
                _dataType = advancedFindColumnFormulaCellProps.LookupFormulaColumn.DataType;
                _formatType = advancedFindColumnFormulaCellProps.LookupFormulaColumn.DecimalFieldType;
            }

            Control.ShowMemoEditorWindow += (sender, args) =>
            {
                var memoEditor = new AdvancedFindFormulaColumnWindow(new DataEntryGridMemoValue(0){Text = LookupFormulaColumnDefinition.OriginalFormula }){ParentTable = LookupFormulaColumnDefinition.ParentTable.Description};
                memoEditor.Owner = Window.GetWindow(control);
                memoEditor.ShowInTaskbar = false;
                if (memoEditor.ShowDialog())
                {
                    LookupFormulaColumnDefinition.UpdateFormula(memoEditor.MemoEditor.Text);
                    Grid.CommitCellEdit(CellLostFocusTypes.KeyboardNavigation, false);
                }

                Control.Focus();

            };

        }
    }
}
