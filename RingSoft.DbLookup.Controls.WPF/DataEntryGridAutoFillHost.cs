// ***********************************************************************
// Assembly         : RingSoft.DbLookup.Controls.WPF
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 10-29-2023
// ***********************************************************************
// <copyright file="DataEntryGridAutoFillHost.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Windows;
using System.Windows.Controls;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.WPF.DataEntryGrid;
using RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost;
using System.Windows.Input;
using System;

namespace RingSoft.DbLookup.Controls.WPF
{
    /// <summary>
    /// A data grid edit host that hosts an AutoFillControl.
    /// Implements the <see cref="RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost.DataEntryGridEditingControlHost{RingSoft.DbLookup.Controls.WPF.AutoFillControl}" />
    /// </summary>
    /// <seealso cref="RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost.DataEntryGridEditingControlHost{RingSoft.DbLookup.Controls.WPF.AutoFillControl}" />
    public class DataEntryGridAutoFillHost : DataEntryGridEditingControlHost<AutoFillControl>
    {
        /// <summary>
        /// Gets a value indicating whether this instance is drop down open.
        /// </summary>
        /// <value><c>true</c> if this instance is drop down open; otherwise, <c>false</c>.</value>
        public override bool IsDropDownOpen => Control.ContainsBoxIsOpen;

        /// <summary>
        /// Gets a value indicating whether [allow read only edit].
        /// </summary>
        /// <value><c>true</c> if [allow read only edit]; otherwise, <c>false</c>.</value>
        public override bool AllowReadOnlyEdit => true;

        /// <summary>
        /// Gets a value indicating whether [edit mode].
        /// </summary>
        /// <value><c>true</c> if [edit mode]; otherwise, <c>false</c>.</value>
        public bool EditMode { get; private set; }

        /// <summary>
        /// Gets the automatic fill cell props.
        /// </summary>
        /// <value>The automatic fill cell props.</value>
        public DataEntryGridAutoFillCellProps AutoFillCellProps { get; private set; }

        /// <summary>
        /// The grid read only mode
        /// </summary>
        private bool _gridReadOnlyMode;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataEntryGridAutoFillHost"/> class.
        /// </summary>
        /// <param name="grid">The grid.</param>
        public DataEntryGridAutoFillHost(DataEntryGrid grid) : base(grid)
        {
        }



        /// <summary>
        /// Gets the cell value.
        /// </summary>
        /// <returns>DataEntryGridEditingCellProps.</returns>
        public override DataEntryGridEditingCellProps GetCellValue()
        {
            return new DataEntryGridAutoFillCellProps(Row, ColumnId,
                Control.Setup, Control.Value);
        }

        /// <summary>
        /// Determines whether [has data changed].
        /// </summary>
        /// <returns><c>true</c> if [has data changed]; otherwise, <c>false</c>.</returns>
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
                var cellPrimaryKeyIsValid = AutoFillCellProps.AutoFillValue.PrimaryKeyValue.IsValid();
                var controlPrimaryKeyIsValid = AutoFillCellProps.AutoFillValue.PrimaryKeyValue.IsValid();

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

        /// <summary>
        /// Updates from cell props.
        /// </summary>
        /// <param name="cellProps">The cell props.</param>
        public override void UpdateFromCellProps(DataEntryGridCellProps cellProps)
        {
            AutoFillCellProps = (DataEntryGridAutoFillCellProps)cellProps;
        }

        /// <summary>
        /// Called when [control loaded].
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="cellProps">The cell props.</param>
        /// <param name="cellStyle">The cell style.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
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

        /// <summary>
        /// Imports the data grid cell properties.
        /// </summary>
        /// <param name="dataGridCell">The data grid cell.</param>
        protected override void ImportDataGridCellProperties(DataGridCell dataGridCell)
        {
            base.ImportDataGridCellProperties(dataGridCell);
            if (_gridReadOnlyMode)
            {
                dataGridCell.BorderThickness = new Thickness(1);
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

        /// <summary>
        /// Sets the read only mode.
        /// </summary>
        /// <param name="readOnlyMode">if set to <c>true</c> [read only mode].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public override bool SetReadOnlyMode(bool readOnlyMode)
        {
            _gridReadOnlyMode = readOnlyMode;
            
            if (readOnlyMode)
                return true;

            return base.SetReadOnlyMode(readOnlyMode);
        }
    }
}
