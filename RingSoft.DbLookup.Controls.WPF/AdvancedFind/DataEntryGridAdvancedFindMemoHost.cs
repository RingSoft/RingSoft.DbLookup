// ***********************************************************************
// Assembly         : RingSoft.DbLookup.Controls.WPF
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 02-20-2023
// ***********************************************************************
// <copyright file="DataEntryGridAdvancedFindMemoHost.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
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
    /// <summary>
    /// Class DataEntryGridAdvancedFindMemoHost.
    /// Implements the <see cref="RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost.DataEntryGridEditingControlHost{RingSoft.DbLookup.Controls.WPF.AdvancedFind.AutoFillMemoCellControl}" />
    /// </summary>
    /// <seealso cref="RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost.DataEntryGridEditingControlHost{RingSoft.DbLookup.Controls.WPF.AdvancedFind.AutoFillMemoCellControl}" />
    public class DataEntryGridAdvancedFindMemoHost : DataEntryGridEditingControlHost<AutoFillMemoCellControl>
    {
        /// <summary>
        /// Gets or sets the original cell props.
        /// </summary>
        /// <value>The original cell props.</value>
        public AdvancedFindMemoCellProps OriginalCellProps { get; set; }

        /// <summary>
        /// Gets a value indicating whether [edit mode].
        /// </summary>
        /// <value><c>true</c> if [edit mode]; otherwise, <c>false</c>.</value>
        public bool EditMode { get; private set; }

        /// <summary>
        /// The data changed
        /// </summary>
        private bool _dataChanged;
        /// <summary>
        /// The memo mode
        /// </summary>
        private bool _memoMode;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataEntryGridAdvancedFindMemoHost"/> class.
        /// </summary>
        /// <param name="grid">The grid.</param>
        public DataEntryGridAdvancedFindMemoHost(DataEntryGrid grid) : base(grid)
        {
        }

        /// <summary>
        /// Gets the cell value.
        /// </summary>
        /// <returns>DataEntryGridEditingCellProps.</returns>
        public override DataEntryGridEditingCellProps GetCellValue()
        {
            return GetNewCellProps(Control.Text);
        }

        /// <summary>
        /// Gets the new cell props.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>AdvancedFindMemoCellProps.</returns>
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

        /// <summary>
        /// Determines whether [has data changed].
        /// </summary>
        /// <returns><c>true</c> if [has data changed]; otherwise, <c>false</c>.</returns>
        public override bool HasDataChanged()
        {
            return _dataChanged || Control.Text != Control.OriginalText;
        }

        /// <summary>
        /// Updates from cell props.
        /// </summary>
        /// <param name="cellProps">The cell props.</param>
        public override void UpdateFromCellProps(DataEntryGridCellProps cellProps)
        {
            if (cellProps is AdvancedFindMemoCellProps advancedFindMemoCellProps)
            {
                Control.Text = advancedFindMemoCellProps.Text;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is drop down open.
        /// </summary>
        /// <value><c>true</c> if this instance is drop down open; otherwise, <c>false</c>.</value>
        public override bool IsDropDownOpen => false;

        /// <summary>
        /// Called when [control loaded].
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="cellProps">The cell props.</param>
        /// <param name="cellStyle">The cell style.</param>
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

        /// <summary>
        /// Shows the memo editor.
        /// </summary>
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

        /// <summary>
        /// Sets the cell text.
        /// </summary>
        /// <param name="cellProps">The cell props.</param>
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

        /// <summary>
        /// Determines whether this instance [can grid process key] the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns><c>true</c> if this instance [can grid process key] the specified key; otherwise, <c>false</c>.</returns>
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
