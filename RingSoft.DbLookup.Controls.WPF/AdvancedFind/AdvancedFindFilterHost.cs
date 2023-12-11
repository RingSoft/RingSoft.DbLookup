// ***********************************************************************
// Assembly         : RingSoft.DbLookup.Controls.WPF
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 02-20-2023
// ***********************************************************************
// <copyright file="AdvancedFindFilterHost.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.WPF.DataEntryGrid;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.Controls.WPF.AdvancedFind
{
    /// <summary>
    /// Class AdvancedFindFilterHost.
    /// Implements the <see cref="RingSoft.DbLookup.Controls.WPF.AdvancedFind.DataEntryGridAdvancedFindMemoHost" />
    /// </summary>
    /// <seealso cref="RingSoft.DbLookup.Controls.WPF.AdvancedFind.DataEntryGridAdvancedFindMemoHost" />
    public class AdvancedFindFilterHost : DataEntryGridAdvancedFindMemoHost
    {
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        protected string Text { get; set; }

        /// <summary>
        /// Gets or sets the cell props.
        /// </summary>
        /// <value>The cell props.</value>
        protected AdvancedFindFilterCellProps CellProps { get; set; }

        /// <summary>
        /// The dirty
        /// </summary>
        private bool _dirty;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdvancedFindFilterHost"/> class.
        /// </summary>
        /// <param name="grid">The grid.</param>
        public AdvancedFindFilterHost(DataEntryGrid grid) : base(grid)
        {
        }

        /// <summary>
        /// Gets the cell value.
        /// </summary>
        /// <returns>DataEntryGridEditingCellProps.</returns>
        public override DataEntryGridEditingCellProps GetCellValue()
        {
            return CellProps;
        }

        /// <summary>
        /// Determines whether [has data changed].
        /// </summary>
        /// <returns><c>true</c> if [has data changed]; otherwise, <c>false</c>.</returns>
        public override bool HasDataChanged()
        {
            return _dirty;
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
            CellProps = cellProps as AdvancedFindFilterCellProps;
            
            base.OnControlLoaded(control, cellProps, cellStyle);
            Control.Text = CellProps.Text;
            control.TextBox.IsReadOnly = true;

        }

        /// <summary>
        /// Shows the memo editor.
        /// </summary>
        protected override void ShowMemoEditor()
        {
            var filterWindow = new AdvancedFilterWindow();
            filterWindow.Initialize(CellProps.FilterReturn);
            filterWindow.Owner = Window.GetWindow(Control);
            filterWindow.ShowInTaskbar = false;
            var result = filterWindow.ShowDialog();
            if (result.HasValue && result.Value)
            {
                CellProps = new AdvancedFindFilterCellProps(Row, ColumnId, Text, filterWindow.FilterReturn);
                if (Row is AdvancedFindFilterRow advancedFindFilterRow)
                {
                    advancedFindFilterRow.SetCellValueFromLookupReturn(filterWindow.FilterReturn);
                    //advancedFindFilterRow.Condition = CellProps.FilterReturn.Condition;
                    advancedFindFilterRow.MakeSearchValueText(CellProps.FilterReturn.SearchValue);
                    Control.TextBox.Text = advancedFindFilterRow.SearchValueText;
                }

                _dirty = true;
                OnUpdateSource(CellProps);
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
                case Key.F2:
                    return true;
            }
            return base.CanGridProcessKey(key);
        }
    }
}
