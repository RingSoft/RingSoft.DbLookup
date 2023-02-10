using System;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.Lookup;
using RingSoft.Printing.Interop;

namespace RingSoft.DbMaintenance
{
    public class AdvancedFindPrinterProcessor : IPrintProcessor
    {
        public AdvancedFindViewModel AdvancedFindViewModel { get; private set; }

        public PrinterSetupArgs PrinterSetup { get; private set; } = new PrinterSetupArgs();

        public AdvancedFindPrinterProcessor(AdvancedFindViewModel advancedFindViewModel)
        {
            AdvancedFindViewModel = advancedFindViewModel;
            PrinterSetup.DataProcessor = this;
        }

        public void PrintOutput()
        {
            if (AdvancedFindViewModel.KeyAutoFillValue == null || AdvancedFindViewModel.KeyAutoFillValue.Text.IsNullOrEmpty())
            {
                var message = "You must specify a Name to print report.";
                var caption = "Validation Error";
                AdvancedFindViewModel.View.OnValidationFail(
                    SystemGlobals.AdvancedFindLookupContext.AdvancedFinds.GetFieldDefinition(p => p.Name), message,
                    caption);
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

        public event EventHandler<PrinterDataProcessedEventArgs> PrintProcessingHeader;
        public void NotifyProcessingHeader(PrinterDataProcessedEventArgs args)
        {
            PrintProcessingHeader?.Invoke(this, args);
        }
    }
}
