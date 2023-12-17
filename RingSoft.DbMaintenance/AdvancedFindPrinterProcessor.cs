// ***********************************************************************
// Assembly         : RingSoft.DbMaintenance
// Author           : petem
// Created          : 01-30-2023
//
// Last Modified By : petem
// Last Modified On : 09-20-2023
// ***********************************************************************
// <copyright file="AdvancedFindPrinterProcessor.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.Lookup;
using RingSoft.Printing.Interop;

namespace RingSoft.DbMaintenance
{
    /// <summary>
    /// Class AdvancedFindPrinterProcessor.
    /// Implements the <see cref="IPrintProcessor" />
    /// </summary>
    /// <seealso cref="IPrintProcessor" />
    public class AdvancedFindPrinterProcessor : IPrintProcessor
    {
        /// <summary>
        /// Gets the advanced find view model.
        /// </summary>
        /// <value>The advanced find view model.</value>
        public AdvancedFindViewModel AdvancedFindViewModel { get; private set; }

        /// <summary>
        /// Gets the printer setup.
        /// </summary>
        /// <value>The printer setup.</value>
        public PrinterSetupArgs PrinterSetup { get; private set; } = new PrinterSetupArgs();

        /// <summary>
        /// Initializes a new instance of the <see cref="AdvancedFindPrinterProcessor"/> class.
        /// </summary>
        /// <param name="advancedFindViewModel">The advanced find view model.</param>
        public AdvancedFindPrinterProcessor(AdvancedFindViewModel advancedFindViewModel)
        {
            AdvancedFindViewModel = advancedFindViewModel;
            PrinterSetup.DataProcessor = this;
        }

        /// <summary>
        /// Prints the output.
        /// </summary>
        public void PrintOutput()
        {
            if (AdvancedFindViewModel.KeyAutoFillValue == null || AdvancedFindViewModel.KeyAutoFillValue.Text.IsNullOrEmpty())
            {
                var message = "You must specify a Name to print report.";
                var caption = "Validation Error";
                AdvancedFindViewModel.KeyAutoFillUiCommand.SetFocus();
                ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
                return;
            }
            PrinterSetup.PrintingProperties.ReportTitle =
                $"{AdvancedFindViewModel.KeyAutoFillValue.Text} Lookup Report";

            PrinterSetup.PrintingProperties.ReportType = ReportTypes.Details;
            PrinterSetup.PrintingProperties.PrintCurrentCode = false;
            PrinterSetup.PrintingProperties.BeginCode = "Start";
            PrinterSetup.PrintingProperties.EndCode = "End";

            AdvancedFindViewModel.View.PrintOutput(PrinterSetup);
        }

        /// <summary>
        /// Processes the print output data.
        /// </summary>
        /// <param name="setupArgs">The setup arguments.</param>
        public void ProcessPrintOutputData(PrinterSetupArgs setupArgs)
        {
            var stringFieldIndex = 1;
            var numericFieldIndex = 1;
            var memoFieldIndex = 1;

            foreach (var visibleColumn in AdvancedFindViewModel.LookupDefinition.VisibleColumns)
            {
                var columnMap = DbMaintenanceViewModelBase.MapLookupColumns(ref stringFieldIndex, ref numericFieldIndex,
                    ref memoFieldIndex, visibleColumn);
                setupArgs.ColumnMaps.Add(columnMap);
            }

            setupArgs.LookupDefinition = AdvancedFindViewModel.LookupDefinition;

            DbMaintenanceViewModelBase.ProcessLookupPrintOutput(setupArgs, this);
        }

        /// <summary>
        /// Occurs when [print processing header].
        /// </summary>
        public event EventHandler<PrinterDataProcessedEventArgs> PrintProcessingHeader;
        /// <summary>
        /// Notifies the processing header.
        /// </summary>
        /// <param name="args">The <see cref="PrinterDataProcessedEventArgs" /> instance containing the event data.</param>
        public void NotifyProcessingHeader(PrinterDataProcessedEventArgs args)
        {
            PrintProcessingHeader?.Invoke(this, args);
        }
    }
}
