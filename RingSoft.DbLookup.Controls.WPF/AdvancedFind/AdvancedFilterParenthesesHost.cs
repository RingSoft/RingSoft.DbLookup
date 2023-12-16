// ***********************************************************************
// Assembly         : RingSoft.DbLookup.Controls.WPF
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-19-2022
// ***********************************************************************
// <copyright file="AdvancedFilterParenthesesHost.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Media;
using System.Windows.Input;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DataEntryControls.WPF.DataEntryGrid;
using RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost;
using RingSoft.DbLookup.AdvancedFind;

namespace RingSoft.DbLookup.Controls.WPF.AdvancedFind
{
    /// <summary>
    /// Class AdvancedFilterParenthesesHost.
    /// Implements the <see cref="DataEntryGridTextBoxHost" />
    /// </summary>
    /// <seealso cref="DataEntryGridTextBoxHost" />
    public class AdvancedFilterParenthesesHost : DataEntryGridTextBoxHost
    {
        /// <summary>
        /// Gets or sets the cell props.
        /// </summary>
        /// <value>The cell props.</value>
        public AdvancedFilterParenthesesCellProps CellProps { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdvancedFilterParenthesesHost"/> class.
        /// </summary>
        /// <param name="grid">The grid.</param>
        public AdvancedFilterParenthesesHost(DataEntryGrid grid) : base(grid)
        {
        }

        /// <summary>
        /// Called when [control loaded].
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="cellProps">The cell props.</param>
        /// <param name="cellStyle">The cell style.</param>
        protected override void OnControlLoaded(StringEditControl control, DataEntryGridEditingCellProps cellProps,
            DataEntryGridCellStyle cellStyle)
        {
            CellProps = cellProps as AdvancedFilterParenthesesCellProps;

            control.KeyDown += (sender, args) =>
            {
                if (args.Key.GetCharFromKey() != CellProps.LimitChar)
                {
                    if (!(args.Key == Key.LeftShift || args.Key == Key.RightShift))
                    {
                        args.Handled = true;
                        SystemSounds.Exclamation.Play();
                    }
                }
            };

            base.OnControlLoaded(control, cellProps, cellStyle);
        }
    }
}
