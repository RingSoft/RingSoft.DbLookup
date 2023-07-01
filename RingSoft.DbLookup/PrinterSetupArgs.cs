using System;
using System.Collections.Generic;
using System.Data;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.TableProcessing;
using RingSoft.Printing.Interop;

namespace RingSoft.DbLookup
{
    public enum PrintColumnTypes
    {
        String = 0,
        Number = 1,
        Memo = 2,
    }
    public interface IPrintProcessor
    {
        void ProcessPrintOutputData(PrinterSetupArgs setupArgs);

        event EventHandler<PrinterDataProcessedEventArgs> PrintProcessingHeader;

        void NotifyProcessingHeader(PrinterDataProcessedEventArgs args);
    }

    public class PrinterDataProcessedEventArgs
    {
        public PrinterSetupArgs PrinterSetup { get; set; }

        public PrintingInputHeaderRow HeaderRow { get; set; }

        public LookupDataMauiBase LookupData { get; set; }

        public PrimaryKeyValue PrimaryKey { get; set; }
    }

    public class PrintingColumnMap
    {
        public LookupColumnDefinitionBase ColumnDefinition { get; private set; }

        public string FieldName { get; private set; }

        public int StringFieldIndex { get; private set; }

        public int NumericFieldIndex { get; private set; }

        public int MemoFieldIndex { get; private set; }

        public PrintColumnTypes ColumnType { get; private set; }

        public void MapString(LookupColumnDefinitionBase column, int index, string fieldName)
        {
            StringFieldIndex = index;
            ColumnDefinition = column;
            FieldName = fieldName;
            ColumnType = PrintColumnTypes.String;
        }

        public void MapNumber(LookupColumnDefinitionBase column, int index, string fieldName)
        {
            NumericFieldIndex = index;
            ColumnDefinition = column;
            FieldName = fieldName;
            ColumnType = PrintColumnTypes.Number;
        }

        public void MapMemo(LookupColumnDefinitionBase column, int index, string fieldName)
        {
            MemoFieldIndex = index;
            ColumnDefinition = column;
            FieldName = fieldName;
            ColumnType = PrintColumnTypes.Memo;
        }

    }
    public class PrinterSetupArgs
    {
        public AutoFillValue CodeAutoFillValue { get; set; }

        public AutoFillSetup CodeAutoFillSetup { get; set; }

        public string CodeDescription { get; set; }

        public LookupDefinitionBase LookupDefinition { get; set; }

        public bool ProcessData { get; set; } = true;

        public PrintingProperties PrintingProperties { get; set; }

        public bool CustomCriteriaSetup { get; set; }

        public IPrintProcessor DataProcessor { get; set; }

        public List<PrintingColumnMap> ColumnMaps { get; private set; } = new List<PrintingColumnMap>();

        public List<FilterItemDefinition> ReportFilters { get; private set; } = new List<FilterItemDefinition>();

        public PrintingProcessingViewModel PrintingProcessingViewModel { get; internal set; }

        public int TotalRecords { get; set; }

        public PrinterSetupArgs()
        {
            PrintingProperties = new PrintingProperties();
            PrintingInteropGlobals.PropertiesProcessor.Properties = PrintingProperties;
        }

        public void ClearReportFilters()
        {
            foreach (var genericCodeFilter in ReportFilters)
            {
                LookupDefinition.FilterDefinition.RemoveFixedFilter(genericCodeFilter);
            }
            ReportFilters.Clear();
        }

        public void AddReportFilter(FilterItemDefinition filter)
        {
            ReportFilters.Add(filter);
        }
    }
}
