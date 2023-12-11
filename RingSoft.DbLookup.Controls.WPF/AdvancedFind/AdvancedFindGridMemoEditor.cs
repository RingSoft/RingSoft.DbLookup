// ***********************************************************************
// Assembly         : RingSoft.DbLookup.Controls.WPF
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-19-2022
// ***********************************************************************
// <copyright file="AdvancedFindGridMemoEditor.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Windows;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.WPF.DataEntryGrid;

namespace RingSoft.DbLookup.Controls.WPF.AdvancedFind
{
    /// <summary>
    /// Class AdvancedFindGridMemoEditor.
    /// Implements the <see cref="DataEntryGridMemoEditor" />
    /// </summary>
    /// <seealso cref="DataEntryGridMemoEditor" />
    public class AdvancedFindGridMemoEditor : DataEntryGridMemoEditor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdvancedFindGridMemoEditor"/> class.
        /// </summary>
        /// <param name="gridMemoValue">The grid memo value.</param>
        public AdvancedFindGridMemoEditor(DataEntryGridMemoValue gridMemoValue) : base(gridMemoValue)
        {
        }

        /// <summary>
        /// Called when [apply template].
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (MemoEditor != null)
            {
                MemoEditor.CollapseDateButton();
            }
        }

        /// <summary>
        /// Validates this instance.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        protected override bool Validate()
        {
            if (MemoEditor.Text.IsNullOrEmpty())
            {
                var message = "Column Header cannot be empty.";
                var caption = "Invalid Column Header";
                ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
                MemoEditor.TextBox.Focus();
                return false;
            }
            return base.Validate();
        }
    }
}
