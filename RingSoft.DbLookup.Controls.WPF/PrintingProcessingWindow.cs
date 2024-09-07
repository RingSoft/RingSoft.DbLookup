// ***********************************************************************
// Assembly         : RingSoft.DbLookup.Controls.WPF
// Author           : petem
// Created          : 01-29-2023
//
// Last Modified By : petem
// Last Modified On : 12-16-2023
// ***********************************************************************
// <copyright file="PrintingProcessingWindow.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.WPF;

namespace RingSoft.DbLookup.Controls.WPF
{
    /// <summary>
    /// Printing Processing Window.
    /// Implements the <see cref="BaseWindow" />
    /// Implements the <see cref="RingSoft.DbLookup.IPrintingProcessingView" />
    /// </summary>
    /// <seealso cref="BaseWindow" />
    /// <seealso cref="RingSoft.DbLookup.IPrintingProcessingView" />
    public class PrintingProcessingWindow : BaseWindow, IPrintingProcessingView
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
        public PrintingProcessingViewModel ViewModel { get; private set; }

        /// <summary>
        /// Gets the part text control.
        /// </summary>
        /// <value>The part text control.</value>
        public StringReadOnlyBox PartTextControl { get; private set; }

        /// <summary>
        /// Gets the part progress bar.
        /// </summary>
        /// <value>The part progress bar.</value>
        public ProgressBar PartProgressBar { get; private set; }

        /// <summary>
        /// Gets the current control.
        /// </summary>
        /// <value>The current control.</value>
        public StringReadOnlyBox CurrentControl { get; private set; }

        /// <summary>
        /// Gets the current progress bar.
        /// </summary>
        /// <value>The current progress bar.</value>
        public ProgressBar CurrentProgressBar { get; private set; }

        /// <summary>
        /// Gets the cancel button.
        /// </summary>
        /// <value>The cancel button.</value>
        public Button CancelButton { get; private set; }

        /// <summary>
        /// Initializes static members of the <see cref="PrintingProcessingWindow" /> class.
        /// </summary>
        static PrintingProcessingWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PrintingProcessingWindow), new FrameworkPropertyMetadata(typeof(PrintingProcessingWindow)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintingProcessingWindow" /> class.
        /// </summary>
        /// <param name="printerSetupArgs">The printer setup arguments.</param>
        public PrintingProcessingWindow(PrinterSetupArgs printerSetupArgs)
        {
            Loaded += (s, e) =>
            {
                DataEntryControls.WPF.ControlsUserInterface.SetActiveWindow(this);
                ViewModel.Initialize(this, printerSetupArgs);
                Title = $"{printerSetupArgs.CodeDescription} Report Data Processing";
            };
        }

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
        /// </summary>
        public override void OnApplyTemplate()
        {
            Border = GetTemplateChild(nameof(Border)) as Border;
            ViewModel = Border.TryFindResource("ViewModel") as PrintingProcessingViewModel;
            PartTextControl = GetTemplateChild(nameof(PartTextControl)) as StringReadOnlyBox;
            PartProgressBar = GetTemplateChild(nameof(PartProgressBar)) as ProgressBar;
            CurrentControl = GetTemplateChild(nameof(CurrentControl)) as StringReadOnlyBox;
            CurrentProgressBar = GetTemplateChild(nameof(CurrentProgressBar)) as ProgressBar;
            CancelButton = GetTemplateChild(nameof(CancelButton)) as Button;

            base.OnApplyTemplate();
        }

        /// <summary>
        /// Updates the status.
        /// </summary>
        public void UpdateStatus()
        {
            var part = (int)ViewModel.ProcessType;
            var enumTranslation = new EnumFieldTranslation();
            enumTranslation.LoadFromEnum<ProcessTypes>();
            var enumItem = enumTranslation.TypeTranslations.FirstOrDefault(p => p.NumericValue == part);
            var totalParts = enumTranslation.TypeTranslations.Max(p => p.NumericValue);

            var currentRecord =
                GblMethods.FormatValue(FieldDataTypes.Integer, ViewModel.RecordBeingProcessed.ToString());
            var totalRecords = GblMethods.FormatValue(FieldDataTypes.Integer, ViewModel.TotalRecordCount.ToString());

            Dispatcher.Invoke(() =>
            {
                PartProgressBar.Maximum = totalParts;
                PartProgressBar.Minimum = 0;
                PartProgressBar.Value = enumItem.NumericValue;
                PartTextControl.Text = enumItem.TextValue;
                switch (ViewModel.ProcessType)
                {
                    case ProcessTypes.CountingHeaderRecords:
                    case ProcessTypes.CountingDetailRecords:
                        CurrentControl.Text = enumItem.TextValue;
                        break;
                    case ProcessTypes.OpeningApp:
                        CurrentControl.Text = string.Empty;
                        break;
                    case ProcessTypes.StartingReport:
                        ViewModel.AbortCommand.IsEnabled = false;
                        CurrentControl.Text = string.Empty;
                        break;
                    case ProcessTypes.ImportHeader:
                    case ProcessTypes.ImportDetails:
                    case ProcessTypes.ProcessReportHeader:
                    case ProcessTypes.ProcessReportDetails:
                        CurrentControl.Text = $"Processing Item {currentRecord} out of {totalRecords}";
                        CurrentProgressBar.Maximum = ViewModel.TotalRecordCount;
                        CurrentProgressBar.Minimum = 0;
                        CurrentProgressBar.Value = ViewModel.RecordBeingProcessed;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                CancelButton.Focus();
            });
        }

        /// <summary>
        /// Closes the window.
        /// </summary>
        public void CloseWindow()
        {
            Dispatcher.Invoke(() =>
            {
                Close();
            });
        }

        /// <summary>
        /// Enables the abort button.
        /// </summary>
        /// <param name="enable">if set to <c>true</c> [enable].</param>
        public void EnableAbortButton(bool enable)
        {
            if (Dispatcher != null)
            {
                Dispatcher.Invoke(() =>
                {
                    if (CancelButton != null)
                        CancelButton.IsEnabled = enable;
                });
            }
        }

        /// <summary>
        /// Handles the <see cref="E:Closing" /> event.
        /// </summary>
        /// <param name="e">The <see cref="CancelEventArgs" /> instance containing the event data.</param>
        protected override void OnClosing(CancelEventArgs e)
        {
            ViewModel.OnWindowClosing();
            base.OnClosing(e);
        }
    }
}
