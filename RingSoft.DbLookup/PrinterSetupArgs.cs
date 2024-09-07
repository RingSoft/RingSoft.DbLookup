// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 01-27-2023
//
// Last Modified By : petem
// Last Modified On : 12-16-2023
// ***********************************************************************
// <copyright file="PrinterSetupArgs.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Data;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.TableProcessing;
using RingSoft.Printing.Interop;

namespace RingSoft.DbLookup
{
    /// <summary>
    /// Enum PrintColumnTypes
    /// </summary>
    public enum PrintColumnTypes
    {
        /// <summary>
        /// The string
        /// </summary>
        String = 0,
        /// <summary>
        /// The number
        /// </summary>
        Number = 1,
        /// <summary>
        /// The memo
        /// </summary>
        Memo = 2,
    }
    /// <summary>
    /// Interface IPrintProcessor
    /// </summary>
    public interface IPrintProcessor
    {
        /// <summary>
        /// Processes the print output data.
        /// </summary>
        /// <param name="setupArgs">The setup arguments.</param>
        void ProcessPrintOutputData(PrinterSetupArgs setupArgs);

        /// <summary>
        /// Occurs when [print processing header].
        /// </summary>
        event EventHandler<PrinterDataProcessedEventArgs> PrintProcessingHeader;

        /// <summary>
        /// Notifies the processing header.
        /// </summary>
        /// <param name="args">The <see cref="PrinterDataProcessedEventArgs" /> instance containing the event data.</param>
        void NotifyProcessingHeader(PrinterDataProcessedEventArgs args);
    }

    /// <summary>
    /// Class PrinterDataProcessedEventArgs.
    /// </summary>
    public class PrinterDataProcessedEventArgs
    {
        /// <summary>
        /// Gets or sets the printer setup.
        /// </summary>
        /// <value>The printer setup.</value>
        public PrinterSetupArgs PrinterSetup { get; set; }

        /// <summary>
        /// Gets or sets the header row.
        /// </summary>
        /// <value>The header row.</value>
        public PrintingInputHeaderRow HeaderRow { get; set; }

        /// <summary>
        /// Gets or sets the lookup data.
        /// </summary>
        /// <value>The lookup data.</value>
        public LookupDataMauiBase LookupData { get; set; }

        /// <summary>
        /// Gets or sets the primary key.
        /// </summary>
        /// <value>The primary key.</value>
        public PrimaryKeyValue PrimaryKey { get; set; }
    }

    /// <summary>
    /// Class PrintingColumnMap.
    /// </summary>
    public class PrintingColumnMap
    {
        /// <summary>
        /// Gets the column definition.
        /// </summary>
        /// <value>The column definition.</value>
        public LookupColumnDefinitionBase ColumnDefinition { get; private set; }

        /// <summary>
        /// Gets the name of the field.
        /// </summary>
        /// <value>The name of the field.</value>
        public string FieldName { get; private set; }

        /// <summary>
        /// Gets the index of the string field.
        /// </summary>
        /// <value>The index of the string field.</value>
        public int StringFieldIndex { get; private set; }

        /// <summary>
        /// Gets the index of the numeric field.
        /// </summary>
        /// <value>The index of the numeric field.</value>
        public int NumericFieldIndex { get; private set; }

        /// <summary>
        /// Gets the index of the memo field.
        /// </summary>
        /// <value>The index of the memo field.</value>
        public int MemoFieldIndex { get; private set; }

        /// <summary>
        /// Gets the type of the column.
        /// </summary>
        /// <value>The type of the column.</value>
        public PrintColumnTypes ColumnType { get; private set; }

        /// <summary>
        /// Maps the string.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <param name="index">The index.</param>
        /// <param name="fieldName">Name of the field.</param>
        public void MapString(LookupColumnDefinitionBase column, int index, string fieldName)
        {
            StringFieldIndex = index;
            ColumnDefinition = column;
            FieldName = fieldName;
            ColumnType = PrintColumnTypes.String;
        }

        /// <summary>
        /// Maps the number.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <param name="index">The index.</param>
        /// <param name="fieldName">Name of the field.</param>
        public void MapNumber(LookupColumnDefinitionBase column, int index, string fieldName)
        {
            NumericFieldIndex = index;
            ColumnDefinition = column;
            FieldName = fieldName;
            ColumnType = PrintColumnTypes.Number;
        }

        /// <summary>
        /// Maps the memo.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <param name="index">The index.</param>
        /// <param name="fieldName">Name of the field.</param>
        public void MapMemo(LookupColumnDefinitionBase column, int index, string fieldName)
        {
            MemoFieldIndex = index;
            ColumnDefinition = column;
            FieldName = fieldName;
            ColumnType = PrintColumnTypes.Memo;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return ColumnDefinition.Caption;
        }
    }
    /// <summary>
    /// Class PrinterSetupArgs.
    /// </summary>
    public class PrinterSetupArgs
    {
        /// <summary>
        /// Gets or sets the code automatic fill value.
        /// </summary>
        /// <value>The code automatic fill value.</value>
        public AutoFillValue CodeAutoFillValue { get; set; }

        /// <summary>
        /// Gets or sets the code automatic fill setup.
        /// </summary>
        /// <value>The code automatic fill setup.</value>
        public AutoFillSetup CodeAutoFillSetup { get; set; }

        /// <summary>
        /// Gets or sets the code description.
        /// </summary>
        /// <value>The code description.</value>
        public string CodeDescription { get; set; }

        /// <summary>
        /// Gets or sets the lookup definition.
        /// </summary>
        /// <value>The lookup definition.</value>
        public LookupDefinitionBase LookupDefinition { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [process data].
        /// </summary>
        /// <value><c>true</c> if [process data]; otherwise, <c>false</c>.</value>
        public bool ProcessData { get; set; } = true;

        /// <summary>
        /// Gets or sets the printing properties.
        /// </summary>
        /// <value>The printing properties.</value>
        public PrintingProperties PrintingProperties { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [custom criteria setup].
        /// </summary>
        /// <value><c>true</c> if [custom criteria setup]; otherwise, <c>false</c>.</value>
        public bool CustomCriteriaSetup { get; set; }

        /// <summary>
        /// Gets or sets the data processor.
        /// </summary>
        /// <value>The data processor.</value>
        public IPrintProcessor DataProcessor { get; set; }

        /// <summary>
        /// Gets the column maps.
        /// </summary>
        /// <value>The column maps.</value>
        public List<PrintingColumnMap> ColumnMaps { get; private set; } = new List<PrintingColumnMap>();

        /// <summary>
        /// Gets the report filters.
        /// </summary>
        /// <value>The report filters.</value>
        public List<FilterItemDefinition> ReportFilters { get; private set; } = new List<FilterItemDefinition>();

        /// <summary>
        /// Gets the printing processing view model.
        /// </summary>
        /// <value>The printing processing view model.</value>
        public PrintingProcessingViewModel PrintingProcessingViewModel { get; internal set; }

        /// <summary>
        /// Gets or sets the total records.
        /// </summary>
        /// <value>The total records.</value>
        public int TotalRecords { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrinterSetupArgs" /> class.
        /// </summary>
        public PrinterSetupArgs()
        {
            PrintingProperties = new PrintingProperties();
            PrintingInteropGlobals.PropertiesProcessor.Properties = PrintingProperties;
        }

        /// <summary>
        /// Clears the report filters.
        /// </summary>
        public void ClearReportFilters()
        {
            foreach (var genericCodeFilter in ReportFilters)
            {
                LookupDefinition.FilterDefinition.RemoveFixedFilter(genericCodeFilter);
            }
            ReportFilters.Clear();
        }

        /// <summary>
        /// Adds the report filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        public void AddReportFilter(FilterItemDefinition filter)
        {
            ReportFilters.Add(filter);
        }
    }
}
