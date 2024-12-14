// ***********************************************************************
// Assembly         : RingSoft.DbLookup.Controls.WPF
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 03-04-2023
// ***********************************************************************
// <copyright file="LookupSearchForHost.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Windows.Controls;
using System.Windows.Input;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;

// ReSharper disable once CheckNamespace
namespace RingSoft.DbLookup.Controls.WPF
{
    /// <summary>
    /// Hosts a control to display as the Search For control.
    /// Implements the <see cref="RingSoft.DbLookup.Controls.WPF.LookupSearchForHost" />
    /// </summary>
    /// <typeparam name="TControl">The type of the t control.</typeparam>
    /// <seealso cref="RingSoft.DbLookup.Controls.WPF.LookupSearchForHost" />
    public abstract class LookupSearchForHost<TControl> : LookupSearchForHost
        where TControl : Control
    {
        /// <summary>
        /// Gets the control.
        /// </summary>
        /// <value>The control.</value>
        public new TControl Control { get; private set; }

        /// <summary>
        /// Constructs the control.
        /// </summary>
        /// <returns>TControl.</returns>
        protected abstract TControl ConstructControl();

        /// <summary>
        /// Internals the initialize.
        /// </summary>
        /// <param name="columnDefinition">The column definition.</param>
        internal override void InternalInitialize(LookupColumnDefinitionBase columnDefinition)
        {
            Control = ConstructControl();
            base.Control = Control;

            Initialize(Control, columnDefinition);
            base.InternalInitialize(columnDefinition);
            Initialize(Control);
        }

        /// <summary>
        /// Internals the initialize.
        /// </summary>
        internal override void InternalInitialize()
        {
            Control = ConstructControl();
            base.Control = Control;

            base.InternalInitialize();
            Initialize(Control);
        }

        /// <summary>
        /// Initializes the specified control.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="columnDefinition">The column definition.</param>
        protected abstract void Initialize(TControl control, LookupColumnDefinitionBase columnDefinition);
        /// <summary>
        /// Initializes the specified control.
        /// </summary>
        /// <param name="control">The control.</param>
        protected abstract void Initialize(TControl control);
    }

    /// <summary>
    /// Hosts a control to display as the Search For control base class.
    /// Implements the <see cref="RingSoft.DbLookup.Controls.WPF.LookupSearchForHost" />
    /// </summary>
    /// <seealso cref="RingSoft.DbLookup.Controls.WPF.LookupSearchForHost" />
    public abstract class LookupSearchForHost
    {
        /// <summary>
        /// Gets or sets the control.
        /// </summary>
        /// <value>The control.</value>
        public Control Control { get; protected internal set; }

        /// <summary>
        /// Gets the lookup column.
        /// </summary>
        /// <value>The lookup column.</value>
        public LookupColumnDefinitionBase LookupColumn { get; private set; }

        /// <summary>
        /// Gets or sets the search text.
        /// </summary>
        /// <value>The search text.</value>
        public abstract string SearchText { get; set; }

        /// <summary>
        /// Occurs when [preview key down].
        /// </summary>
        public event EventHandler<KeyEventArgs> PreviewKeyDown;

        /// <summary>
        /// Occurs when [text changed].
        /// </summary>
        public event EventHandler TextChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupSearchForHost"/> class.
        /// </summary>
        protected internal LookupSearchForHost()
        {
        }

        /// <summary>
        /// Internals the initialize.
        /// </summary>
        /// <param name="columnDefinition">The column definition.</param>
        internal virtual void InternalInitialize(LookupColumnDefinitionBase columnDefinition)
        {
            Control.PreviewKeyDown += (sender, args) => OnPreviewKeyDown(args);
            LookupColumn = columnDefinition;
            if (columnDefinition is LookupFieldColumnDefinition fieldColumn)
            {
                Initialize(fieldColumn.FieldDefinition);
            }
        }

        internal virtual void Initialize(FieldDefinition fieldDefinition)
        {

        }

    /// <summary>
        /// Internals the initialize.
        /// </summary>
        internal virtual void InternalInitialize()
        {
            Control.PreviewKeyDown += (sender, args) => OnPreviewKeyDown(args);
        }

        /// <summary>
        /// Selects all.
        /// </summary>
        public abstract void SelectAll();

        /// <summary>
        /// Handles the <see cref="E:PreviewKeyDown" /> event.
        /// </summary>
        /// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
        protected void OnPreviewKeyDown(KeyEventArgs e)
        {
            PreviewKeyDown?.Invoke(Control, e);
        }

        /// <summary>
        /// Called when [text changed].
        /// </summary>
        protected void OnTextChanged()
        {
            TextChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Sets the focus to control.
        /// </summary>
        public virtual void SetFocusToControl()
        {
        }

        /// <summary>
        /// Determines whether this instance [can process search for key] the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns><c>true</c> if this instance [can process search for key] the specified key; otherwise, <c>false</c>.</returns>
        public virtual bool CanProcessSearchForKey(Key key)
        {
            return true;
        }
    }
}
