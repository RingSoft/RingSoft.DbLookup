// ***********************************************************************
// Assembly         : RingSoft.DbLookup.Controls.WPF
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 01-16-2023
// ***********************************************************************
// <copyright file="DeleteRecordWindowItemControl.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.Controls.WPF
{
    /// <summary>
    /// Class DeleteRecordWindowItemControl.
    /// Implements the <see cref="Control" />
    /// </summary>
    /// <seealso cref="Control" />
    /// <font color="red">Badly formed XML comment.</font>
    public class DeleteRecordWindowItemControl : Control
    {
        /// <summary>
        /// Gets the border.
        /// </summary>
        /// <value>The border.</value>
        public Border Border { get; private set; }
        /// <summary>
        /// Gets the delete all CheckBox.
        /// </summary>
        /// <value>The delete all CheckBox.</value>
        public CheckBox DeleteAllCheckBox { get; private set; }
        /// <summary>
        /// Gets the null all CheckBox.
        /// </summary>
        /// <value>The null all CheckBox.</value>
        public CheckBox NullAllCheckBox { get; private set; }

        /// <summary>
        /// Gets the view model.
        /// </summary>
        /// <value>The view model.</value>
        public DeleteRecordItemViewModel ViewModel { get; private set; }
        /// <summary>
        /// Gets the delete table.
        /// </summary>
        /// <value>The delete table.</value>
        public DeleteTable DeleteTable { get; private set; }

        /// <summary>
        /// Initializes static members of the <see cref="DeleteRecordWindowItemControl"/> class.
        /// </summary>
        static DeleteRecordWindowItemControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DeleteRecordWindowItemControl), new FrameworkPropertyMetadata(typeof(DeleteRecordWindowItemControl)));

            IsTabStopProperty.OverrideMetadata(typeof(DeleteRecordWindowItemControl), new FrameworkPropertyMetadata(false));

            KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(DeleteRecordWindowItemControl),
                new FrameworkPropertyMetadata(KeyboardNavigationMode.Local));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteRecordWindowItemControl"/> class.
        /// </summary>
        /// <param name="deleteTable">The delete table.</param>
        public DeleteRecordWindowItemControl(DeleteTable deleteTable)
        {
            DeleteTable = deleteTable;
        }

        /// <summary>
        /// Sets the initial focus CheckBox.
        /// </summary>
        public void SetInitialFocusCheckBox()
        {
            DeleteAllCheckBox.Focus();
        }

        /// <summary>
        /// Called when [apply template].
        /// </summary>
        public override void OnApplyTemplate()
        {
            Border = GetTemplateChild(nameof(Border)) as Border;
            DeleteAllCheckBox = GetTemplateChild(nameof(DeleteAllCheckBox)) as CheckBox;
            NullAllCheckBox = GetTemplateChild(nameof(NullAllCheckBox)) as CheckBox;
            
            ViewModel = Border.TryFindResource("ViewModel") as DeleteRecordItemViewModel;

            ViewModel.Initialize(DeleteTable);
            if (DeleteTable.ChildField.AllowNulls && DeleteTable.ChildField.AllowUserNulls)
            {
                DeleteAllCheckBox.Visibility = Visibility.Collapsed;
            }
            else
            {
                NullAllCheckBox.Visibility = Visibility.Collapsed;
            }

            ViewModel.DeleteAllRecords = DeleteTable.DeleteAllData;
            ViewModel.NullAllRecords = DeleteTable.NullAllData;

            base.OnApplyTemplate();
        }
    }
}
