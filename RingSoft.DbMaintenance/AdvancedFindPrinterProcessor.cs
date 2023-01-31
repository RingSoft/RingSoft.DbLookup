using System;
using RingSoft.DbLookup;
using RingSoft.DbLookup.Lookup;

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

            DbMaintenanceViewModelBase.ProcessLookupPrintOutput(setupArgs);
        }

        public event EventHandler<PrinterDataProcessedEventArgs> ProcessingRecord;
    }
}
