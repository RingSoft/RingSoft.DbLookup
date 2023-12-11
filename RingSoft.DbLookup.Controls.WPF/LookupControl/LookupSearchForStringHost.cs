// ***********************************************************************
// Assembly         : RingSoft.DbLookup.Controls.WPF
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 03-04-2023
// ***********************************************************************
// <copyright file="LookupSearchForStringHost.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup.Lookup;

// ReSharper disable once CheckNamespace
namespace RingSoft.DbLookup.Controls.WPF
{
    /// <summary>
    /// Class LookupSearchForStringHost.
    /// Implements the <see cref="RingSoft.DbLookup.Controls.WPF.LookupSearchForHost{RingSoft.DataEntryControls.WPF.StringEditControl}" />
    /// </summary>
    /// <seealso cref="RingSoft.DbLookup.Controls.WPF.LookupSearchForHost{RingSoft.DataEntryControls.WPF.StringEditControl}" />
    public class LookupSearchForStringHost : LookupSearchForHost<StringEditControl>
    {
        /// <summary>
        /// Gets or sets the search text.
        /// </summary>
        /// <value>The search text.</value>
        public override string SearchText
        {
            get => Control.Text;
            set => Control.Text = value;
        }

        /// <summary>
        /// Constructs the control.
        /// </summary>
        /// <returns>TControl.</returns>
        protected override StringEditControl ConstructControl()
        {
            var result = new StringEditControl();
            return result;
        }

        /// <summary>
        /// Initializes the specified control.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="columnDefinition">The column definition.</param>
        protected override void Initialize(StringEditControl control, LookupColumnDefinitionBase columnDefinition)
        {
            
        }

        /// <summary>
        /// Initializes the specified control.
        /// </summary>
        /// <param name="control">The control.</param>
        protected override void Initialize(StringEditControl control)
        {
            Control.TextChanged += (sender, args) => OnTextChanged();
        }

        /// <summary>
        /// Selects all.
        /// </summary>
        public override void SelectAll()
        {
            Control.SelectAll();
        }
    }
}
