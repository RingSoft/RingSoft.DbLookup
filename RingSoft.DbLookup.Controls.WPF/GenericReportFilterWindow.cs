// ***********************************************************************
// Assembly         : RingSoft.DbLookup.Controls.WPF
// Author           : petem
// Created          : 01-30-2023
//
// Last Modified By : petem
// Last Modified On : 02-28-2023
// ***********************************************************************
// <copyright file="GenericReportFilterWindow.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Windows;
using System.Windows.Controls;
using RingSoft.DataEntryControls.WPF;
using RingSoft.Printing.Interop;

namespace RingSoft.DbLookup.Controls.WPF
{
    /// <summary>
    /// Class GenericReportFilterWindow.
    /// Implements the <see cref="BaseWindow" />
    /// Implements the <see cref="RingSoft.DbLookup.IGenericReportFilterView" />
    /// </summary>
    /// <seealso cref="BaseWindow" />
    /// <seealso cref="RingSoft.DbLookup.IGenericReportFilterView" />
    /// <font color="red">Badly formed XML comment.</font>
    public class GenericReportFilterWindow : BaseWindow, IGenericReportFilterView
    {
        /// <summary>
        /// Gets the border.
        /// </summary>
        /// <value>The border.</value>
        public Border Border { get; private set; }

        /// <summary>
        /// Gets the view model.
        /// </summary>
        /// <value>The view model.</value>
        public GenericReportFilterViewModel ViewModel { get; private set; }

        /// <summary>
        /// Gets the current CheckBox.
        /// </summary>
        /// <value>The current CheckBox.</value>
        public CheckBox CurrentCheckBox { get; private set; }

        /// <summary>
        /// Gets the current control.
        /// </summary>
        /// <value>The current control.</value>
        public AutoFillControl CurrentControl { get; private set; }

        /// <summary>
        /// Gets the beginning control.
        /// </summary>
        /// <value>The beginning control.</value>
        public AutoFillControl BeginningControl { get; private set; }

        /// <summary>
        /// Gets the ending control.
        /// </summary>
        /// <value>The ending control.</value>
        public AutoFillControl EndingControl { get; private set; }

        /// <summary>
        /// Gets the report type label.
        /// </summary>
        /// <value>The report type label.</value>
        public Label ReportTypeLabel { get; private set; }
        /// <summary>
        /// Gets the report type control.
        /// </summary>
        /// <value>The report type control.</value>
        public TextComboBoxControl ReportTypeControl { get; private set; }

        /// <summary>
        /// Initializes static members of the <see cref="GenericReportFilterWindow"/> class.
        /// </summary>
        static GenericReportFilterWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GenericReportFilterWindow), new FrameworkPropertyMetadata(typeof(GenericReportFilterWindow)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericReportFilterWindow"/> class.
        /// </summary>
        /// <param name="printerSetup">The printer setup.</param>
        public GenericReportFilterWindow(PrinterSetupArgs printerSetup)
        {
            Loaded += (s, e) =>
            {
                ViewModel.Initialize(this, printerSetup);
                Title = $"{printerSetup.CodeDescription} Report Filter Options";
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericReportFilterWindow"/> class.
        /// </summary>
        /// <param name="input">The input.</param>
        public GenericReportFilterWindow(GenericReportLookupFilterInput input)
        {
            Loaded += (sender, args) =>
            {
                ViewModel.Initialize(this, input);
                Title = $"{input.CodeNameToFilter} {input.ProcessText} Filter Options";
            };
        }

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
        /// </summary>
        public override void OnApplyTemplate()
        {
            Border = GetTemplateChild(nameof(Border)) as Border;
            ViewModel = Border.TryFindResource("ViewModel") as GenericReportFilterViewModel;

            CurrentCheckBox = GetTemplateChild(nameof(CurrentCheckBox)) as CheckBox;
            CurrentControl = GetTemplateChild(nameof(CurrentControl)) as AutoFillControl;
            BeginningControl = GetTemplateChild(nameof(BeginningControl)) as AutoFillControl;
            EndingControl = GetTemplateChild(nameof(EndingControl)) as AutoFillControl;
            ReportTypeLabel = GetTemplateChild(nameof(ReportTypeLabel)) as Label;
            ReportTypeControl = GetTemplateChild(nameof(ReportTypeControl)) as TextComboBoxControl;

            base.OnApplyTemplate();
        }

        /// <summary>
        /// Refreshes the view.
        /// </summary>
        public void RefreshView()
        {
            BeginningControl.IsEnabled = EndingControl.IsEnabled = !ViewModel.IsCurrentOnly;
            CurrentControl.IsEnabled = ViewModel.IsCurrentOnly;
            if (ViewModel.LookupMode)
            {
                ReportTypeLabel.Visibility = ReportTypeControl.Visibility = Visibility.Collapsed;
            }
            else
            {
                if (ViewModel.PrinterSetup.PrintingProperties.ReportType == ReportTypes.Custom)
                {
                    ReportTypeLabel.Visibility = ReportTypeControl.Visibility = Visibility.Collapsed;
                }
            }
        }

        /// <summary>
        /// Closes the window.
        /// </summary>
        public void CloseWindow()
        {
            Close();
        }

        /// <summary>
        /// Prints the output.
        /// </summary>
        public void PrintOutput()
        {
            LookupControlsGlobals.PrintDocument(ViewModel.PrinterSetup);
        }

        /// <summary>
        /// Focuses the control.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">control - null</exception>
        public void FocusControl(GenericFocusControls control)
        {
            switch (control)
            {
                case GenericFocusControls.Current:
                    CurrentControl.Focus();
                    break;
                case GenericFocusControls.Start:
                    BeginningControl.Focus();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(control), control, null);
            }
        }
    }
}
