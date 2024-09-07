// ***********************************************************************
// Assembly         : RingSoft.DbLookup.Controls.WPF
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-16-2023
// ***********************************************************************
// <copyright file="LookupSearchForDropDownHost.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DataEntryControls.WPF;
using System.Windows.Input;

// ReSharper disable once CheckNamespace
namespace RingSoft.DbLookup.Controls.WPF
{
    /// <summary>
    /// Class LookupSearchForDropDownHost.
    /// Implements the <see cref="RingSoft.DbLookup.Controls.WPF.LookupSearchForHost{TDropDownControl}" />
    /// </summary>
    /// <typeparam name="TDropDownControl">The type of the t drop down control.</typeparam>
    /// <seealso cref="RingSoft.DbLookup.Controls.WPF.LookupSearchForHost{TDropDownControl}" />
    public abstract class LookupSearchForDropDownHost<TDropDownControl> : LookupSearchForHost<TDropDownControl>
        where TDropDownControl : DropDownEditControl
    {
        /// <summary>
        /// Selects all.
        /// </summary>
        public override void SelectAll()
        {
            Control.TextBox?.SelectAll();
        }

        /// <summary>
        /// Sets the focus to control.
        /// </summary>
        public override void SetFocusToControl()
        {
            Control.TextBox?.Focus();
            base.SetFocusToControl();
        }

        /// <summary>
        /// Determines whether this instance [can process search for key] the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns><c>true</c> if this instance [can process search for key] the specified key; otherwise, <c>false</c>.</returns>
        public override bool CanProcessSearchForKey(Key key)
        {
            if (Control.IsPopupOpen())
                return false;

            switch (key)
            {
                case Key.Home:
                    return Control.SelectionStart == 0 && Control.SelectionLength == 0;
                case Key.End:
                    return Control.SelectionStart == Control.Text.Length;
            }

            return base.CanProcessSearchForKey(key);
        }
    }
}
