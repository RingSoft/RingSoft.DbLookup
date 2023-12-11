// ***********************************************************************
// Assembly         : RingSoft.DbLookup.Controls.WPF
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-19-2022
// ***********************************************************************
// <copyright file="DataEntryGridAdvancedFindFormulaColumnHost.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Windows;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.WPF.DataEntryGrid;
using RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.Controls.WPF.AdvancedFind
{
    /// <summary>
    /// Class DataEntryGridAdvancedFindFormulaColumnHost.
    /// Implements the <see cref="RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost.DataEntryGridEditingControlHost{RingSoft.DbLookup.Controls.WPF.AdvancedFind.AutoFillMemoCellControl}" />
    /// </summary>
    /// <seealso cref="RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost.DataEntryGridEditingControlHost{RingSoft.DbLookup.Controls.WPF.AdvancedFind.AutoFillMemoCellControl}" />
    public class DataEntryGridAdvancedFindFormulaColumnHost : DataEntryGridEditingControlHost<AutoFillMemoCellControl>
    {
        /// <summary>
        /// Gets or sets the original cell props.
        /// </summary>
        /// <value>The original cell props.</value>
        public AdvancedFindColumnFormulaCellProps OriginalCellProps { get; set; }

        /// <summary>
        /// Gets or sets the lookup formula column definition.
        /// </summary>
        /// <value>The lookup formula column definition.</value>
        public LookupFormulaColumnDefinition LookupFormulaColumnDefinition { get; set; }

        /// <summary>
        /// The data type
        /// </summary>
        private FieldDataTypes _dataType;
        /// <summary>
        /// The format type
        /// </summary>
        private DecimalFieldTypes _formatType;
        /// <summary>
        /// The formula
        /// </summary>
        private string _formula;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataEntryGridAdvancedFindFormulaColumnHost"/> class.
        /// </summary>
        /// <param name="grid">The grid.</param>
        public DataEntryGridAdvancedFindFormulaColumnHost(DataEntryGrid grid) : base(grid)
        {
        }

        /// <summary>
        /// Gets a value indicating whether this instance is drop down open.
        /// </summary>
        /// <value><c>true</c> if this instance is drop down open; otherwise, <c>false</c>.</value>
        public override bool IsDropDownOpen => false;
        /// <summary>
        /// Gets the cell value.
        /// </summary>
        /// <returns>DataEntryGridEditingCellProps.</returns>
        public override DataEntryGridEditingCellProps GetCellValue()
        {
            return new AdvancedFindColumnFormulaCellProps(Row, ColumnId, LookupFormulaColumnDefinition);
        }

        /// <summary>
        /// Determines whether [has data changed].
        /// </summary>
        /// <returns><c>true</c> if [has data changed]; otherwise, <c>false</c>.</returns>
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

        /// <summary>
        /// Updates from cell props.
        /// </summary>
        /// <param name="cellProps">The cell props.</param>
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

        /// <summary>
        /// Called when [control loaded].
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="cellProps">The cell props.</param>
        /// <param name="cellStyle">The cell style.</param>
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
                var memoEditor = new AdvancedFindFormulaColumnWindow(new DataEntryGridMemoValue(0){Text = LookupFormulaColumnDefinition.OriginalFormula })
                {
                    ParentTable = LookupFormulaColumnDefinition.TableDescription
                };
                if (memoEditor.ParentTable.IsNullOrEmpty())
                {
                    var gridRow = cellProps.Row as AdvancedFindColumnRow;
                    if (gridRow != null)
                    {
                        memoEditor.ParentTable = gridRow.Table;
                    }
                }

                memoEditor.DataType = LookupFormulaColumnDefinition.DataType;
                memoEditor.DecimalFormat = LookupFormulaColumnDefinition.DecimalFieldType.ConvertDecimalFieldTypeToDecimalEditFormatType();
                memoEditor.Owner = Window.GetWindow(control);
                memoEditor.ShowInTaskbar = false;
                if (memoEditor.ShowDialog())
                {
                    LookupFormulaColumnDefinition.UpdateFormula(memoEditor.MemoEditor.Text);
                    LookupFormulaColumnDefinition.HasDataType(memoEditor.ViewModel.DataType);
                    if (memoEditor.ViewModel.DataType == FieldDataTypes.Decimal)
                    {
                        if (memoEditor.ViewModel.DecimalFormatComboBoxItem != null)
                        {
                            LookupFormulaColumnDefinition.HasDecimalFieldType(
                                (DecimalFieldTypes) (int) memoEditor.ViewModel.DecimalFormatType);
                        }
                    }
                    Grid.CommitCellEdit(CellLostFocusTypes.KeyboardNavigation, false);
                }

                Control.Focus();

            };

        }
    }
}
