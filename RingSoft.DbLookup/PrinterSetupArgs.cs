using System;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.Lookup;
using RingSoft.Printing.Interop;

namespace RingSoft.DbLookup
{
    public interface IPrintProcessor
    {
        void ProcessPrintOutputData(PrinterSetupArgs setupArgs);

        event EventHandler<PrinterDataProcessedEventArgs> ProcessingRecord;
    }

    public class PrinterDataProcessedEventArgs
    {
        public ChunkType ChunkType { get; set; }

        public int RecordCount { get; set; }

        public int ProcessingRecord { get; set; }

        public int TotalTables { get; set; }

        public int TableIdBeingProcessed { get; set; }
    }
    public class PrinterSetupArgs
    {
        public AutoFillSetup CodeAutoFillSetup { get; set; }

        public AutoFillValue CodeAutoFillValue { get; set; }

        public LookupDefinitionBase LookupDefinition { get; set; }

        public bool ProcessData { get; set; } = true;

        public PrintingProperties PrintingProperties { get; set; }

        public bool CustomCriteriaSetup { get; set; }

        public IPrintProcessor DataProcessor { get; set; }


        public PrinterSetupArgs()
        {
            PrintingProperties = new PrintingProperties();
        }
    }
}
