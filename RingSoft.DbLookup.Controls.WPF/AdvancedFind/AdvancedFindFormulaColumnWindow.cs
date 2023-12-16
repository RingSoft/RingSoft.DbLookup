// ***********************************************************************
// Assembly         : RingSoft.DbLookup.Controls.WPF
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-19-2022
// ***********************************************************************
// <copyright file="AdvancedFindFormulaColumnWindow.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DataEntryControls.WPF.DataEntryGrid;
using RingSoft.DbLookup.AdvancedFind;
using System.Windows;
using System.Windows.Controls;

namespace RingSoft.DbLookup.Controls.WPF.AdvancedFind
{
    /// <summary>
    /// Class AdvancedFindFormulaColumnWindow.
    /// Implements the <see cref="DataEntryGridMemoEditor" />
    /// </summary>
    /// <seealso cref="DataEntryGridMemoEditor" />
    /// <font color="red">Badly formed XML comment.</font>
    public class AdvancedFindFormulaColumnWindow : DataEntryGridMemoEditor
    {

        /// <summary>
        /// Gets or sets the border.
        /// </summary>
        /// <value>The border.</value>
        public Border Border { get; set; }
        /// <summary>
        /// Gets or sets the field data type ComboBox.
        /// </summary>
        /// <value>The field data type ComboBox.</value>
        public TextComboBoxControl FieldDataTypeComboBox { get; set; }
        /// <summary>
        /// Gets or sets the format type label.
        /// </summary>
        /// <value>The format type label.</value>
        public Label FormatTypeLabel { get; set; }
        /// <summary>
        /// Gets or sets the format type ComboBox.
        /// </summary>
        /// <value>The format type ComboBox.</value>
        public TextComboBoxControl FormatTypeComboBox { get; set; }
        /// <summary>
        /// Gets or sets the memo editor.
        /// </summary>
        /// <value>The memo editor.</value>
        public DataEntryMemoEditor MemoEditor { get; set; }

        /// <summary>
        /// Gets or sets the parent table.
        /// </summary>
        /// <value>The parent table.</value>
        public string ParentTable { get; set; }
        /// <summary>
        /// Gets or sets the type of the data.
        /// </summary>
        /// <value>The type of the data.</value>
        public FieldDataTypes DataType { get; set; }
        /// <summary>
        /// Gets or sets the decimal format.
        /// </summary>
        /// <value>The decimal format.</value>
        public DecimalEditFormatTypes DecimalFormat { get; set; }
        /// <summary>
        /// Gets or sets the view model.
        /// </summary>
        /// <value>The view model.</value>
        public AdvancedFindFormulaColumnViewModel ViewModel { get; set; }

        /// <summary>
        /// Initializes static members of the <see cref="AdvancedFindFormulaColumnWindow"/> class.
        /// </summary>
        static AdvancedFindFormulaColumnWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AdvancedFindFormulaColumnWindow), new FrameworkPropertyMetadata(typeof(AdvancedFindFormulaColumnWindow)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdvancedFindFormulaColumnWindow"/> class.
        /// </summary>
        /// <param name="gridMemoValue">The grid memo value.</param>
        public AdvancedFindFormulaColumnWindow(DataEntryGridMemoValue gridMemoValue) : base(gridMemoValue)
        {
            Loaded += (sender, args) =>
            {
                SnugWidth = 700;
                SnugHeight = 500;
                SnugWindow();
            };
        }

        /// <summary>
        /// Called when [apply template].
        /// </summary>
        public override void OnApplyTemplate()
        {
            Border = GetTemplateChild(nameof(Border)) as Border;
            FieldDataTypeComboBox = GetTemplateChild(nameof(FieldDataTypeComboBox)) as TextComboBoxControl;
            FormatTypeLabel = GetTemplateChild(nameof(FormatTypeLabel)) as Label;
            FormatTypeComboBox = GetTemplateChild(nameof(FormatTypeComboBox)) as TextComboBoxControl;
            MemoEditor = GetTemplateChild(nameof(MemoEditor)) as DataEntryMemoEditor;

            base.OnApplyTemplate();

            ViewModel = Border.TryFindResource("ViewModel") as AdvancedFindFormulaColumnViewModel;
            ViewModel.Initialize();
            ViewModel.Table = ParentTable;
            ViewModel.DataType = DataType;

            SetDecimalFormatType();
            FieldDataTypeComboBox.SelectionChanged += (sender, args) => { SetDecimalFormatType(); };

            if (DataType == FieldDataTypes.Decimal)
            {
                ViewModel.DecimalFormatType = DecimalFormat;
            }
            FieldDataTypeComboBox.SelectionChanged += FieldDataTypeComboBox_SelectionChanged;
            MemoEditor.CollapseDateButton();
        }

        /// <summary>
        /// Sets the type of the decimal format.
        /// </summary>
        private void SetDecimalFormatType()
        {
            if (ViewModel.DataType == FieldDataTypes.Decimal)
            {
                FormatTypeComboBox.Visibility = Visibility.Visible;
                FormatTypeLabel.Visibility = Visibility.Visible;
            }
            else
            {
                FormatTypeComboBox.Visibility = Visibility.Collapsed;
                FormatTypeLabel.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Validates this instance.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        protected override bool Validate()
        {
            if (FormatTypeComboBox.Visibility == Visibility.Visible && ViewModel.DecimalFormatComboBoxItem == null)
            {
                var message = "You must select a data format type.";
                var caption = "Invalid Data Format Type";
                ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
                FormatTypeComboBox.Focus();
                return false;

            }

            if (MemoEditor.Text.IsNullOrEmpty())
            {
                var message = "Formula cannot be empty.";
                var caption = "Invalid Formula";
                ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
                MemoEditor.TextBox.Focus();
                return false;
            }

            return base.Validate();
        }

        /// <summary>
        /// Handles the SelectionChanged event of the FieldDataTypeComboBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SelectionChangedEventArgs"/> instance containing the event data.</param>
        private void FieldDataTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ViewModel.DataType == FieldDataTypes.Decimal)
            {
                FormatTypeComboBox.IsEnabled = true;
            }
            else
            {
                FormatTypeComboBox.IsEnabled = false;
                FormatTypeComboBox.SelectedItem = null;
            }
        }
    }
}
