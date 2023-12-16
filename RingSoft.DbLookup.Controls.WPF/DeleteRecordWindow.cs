// ***********************************************************************
// Assembly         : RingSoft.DbLookup.Controls.WPF
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 03-07-2023
// ***********************************************************************
// <copyright file="DeleteRecordWindow.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbMaintenance;
using System.Windows;
using System.Windows.Controls;
using RingSoft.DbLookup.Lookup;

namespace RingSoft.DbLookup.Controls.WPF
{
    /// <summary>
    /// Class DeleteRecordWindow.
    /// Implements the <see cref="BaseWindow" />
    /// Implements the <see cref="RingSoft.DbLookup.IDeleteRecordView" />
    /// </summary>
    /// <seealso cref="BaseWindow" />
    /// <seealso cref="RingSoft.DbLookup.IDeleteRecordView" />
    /// <font color="red">Badly formed XML comment.</font>
    public class DeleteRecordWindow : BaseWindow, IDeleteRecordView
    {
        //public TabControl TabControl { get; private set; }
        /// <summary>
        /// Gets or sets the border.
        /// </summary>
        /// <value>The border.</value>
        public Border Border { get; set; }
        /// <summary>
        /// Gets the view model.
        /// </summary>
        /// <value>The view model.</value>
        public DeleteRecordViewModel ViewModel { get; private set; }
        /// <summary>
        /// Gets the delete all CheckBox.
        /// </summary>
        /// <value>The delete all CheckBox.</value>
        public CheckBox DeleteAllCheckBox { get; private set; }
        /// <summary>
        /// Gets the lookup control.
        /// </summary>
        /// <value>The lookup control.</value>
        public LookupControl LookupControl { get; private set; }

        /// <summary>
        /// The old size
        /// </summary>
        private Size _oldSize;
        /// <summary>
        /// Gets the delete tabs.
        /// </summary>
        /// <value>The delete tabs.</value>
        public List<DeleteRecordWindowItemControl> DeleteTabs { get; private set; } =
            new List<DeleteRecordWindowItemControl>();


        /// <summary>
        /// Initializes static members of the <see cref="DeleteRecordWindow"/> class.
        /// </summary>
        static DeleteRecordWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DeleteRecordWindow), new FrameworkPropertyMetadata(typeof(DeleteRecordWindow)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteRecordWindow"/> class.
        /// </summary>
        /// <param name="deleteTables">The delete tables.</param>
        public DeleteRecordWindow(DeleteTables deleteTables)
        {
            var loaded = false;
            Loaded += (sender, args) =>
            {
                ViewModel.Initialize(this, deleteTables);

                DeleteAllCheckBox.Focus();
                loaded = true;
            };
            SizeChanged += (sender, args) =>
            {
                if (LookupControl != null && loaded)
                {
                    var widthDif = Width - _oldSize.Width;
                    var heightDif = Height - _oldSize.Height;
                    if (Math.Round(widthDif) > 1)
                    {
                        LookupControl.Width = LookupControl.ActualWidth + widthDif;
                    }

                    if (Math.Round(heightDif) > 1)
                    {
                        LookupControl.Height = LookupControl.ActualHeight + heightDif;
                    }
                }

                _oldSize = args.NewSize;
            };

        }

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
        /// </summary>
        public override void OnApplyTemplate()
        {
            Border = GetTemplateChild(nameof(Border)) as Border;
            DeleteAllCheckBox = GetTemplateChild(nameof(DeleteAllCheckBox)) as CheckBox;
            LookupControl = GetTemplateChild(nameof(LookupControl)) as LookupControl;
            ViewModel = Border.TryFindResource("ViewModel") as DeleteRecordViewModel;

            base.OnApplyTemplate();
        }

        /// <summary>
        /// Closes the window.
        /// </summary>
        /// <param name="result">if set to <c>true</c> [result].</param>
        public void CloseWindow(bool result)
        {
            DialogResult = result;
            Close();
        }
    }
}
