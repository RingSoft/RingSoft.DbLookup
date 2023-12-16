// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 01-29-2023
//
// Last Modified By : petem
// Last Modified On : 12-10-2023
// ***********************************************************************
// <copyright file="PrintingProcessingViewModel.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using Newtonsoft.Json;
using RingSoft.DataEntryControls.Engine;
using RingSoft.Printing.Interop;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Timer = System.Timers.Timer;

namespace RingSoft.DbLookup
{
    /// <summary>
    /// Enum ProcessTypes
    /// </summary>
    public enum ProcessTypes
    {
        /// <summary>
        /// The counting header records
        /// </summary>
        [Description("Counting Header Records")]
        CountingHeaderRecords = 0,
        /// <summary>
        /// The import header
        /// </summary>
        [Description("Importing Header Records")]
        ImportHeader = 1,
        /// <summary>
        /// The counting detail records
        /// </summary>
        [Description("Counting Detail Records")]
        CountingDetailRecords = 2,
        /// <summary>
        /// The import details
        /// </summary>
        [Description("Importing Detail Records")]
        ImportDetails = 3,
        /// <summary>
        /// The opening application
        /// </summary>
        [Description("Opening Printing App")]
        OpeningApp = 4,
        /// <summary>
        /// The process report header
        /// </summary>
        [Description("Processing Header Chunks")]
        ProcessReportHeader = 5,
        /// <summary>
        /// The process report details
        /// </summary>
        [Description("Processing Detail Chunks")]
        ProcessReportDetails = 6,
        /// <summary>
        /// The starting report
        /// </summary>
        [Description("Starting Report")]
        StartingReport = 7,
    }
    /// <summary>
    /// Interface IPrintingProcessingView
    /// </summary>
    public interface IPrintingProcessingView
    {
        /// <summary>
        /// Updates the status.
        /// </summary>
        void UpdateStatus();

        /// <summary>
        /// Closes the window.
        /// </summary>
        void CloseWindow();

        /// <summary>
        /// Enables the abort button.
        /// </summary>
        /// <param name="enable">if set to <c>true</c> [enable].</param>
        void EnableAbortButton(bool enable);
    }

    /// <summary>
    /// Class PrintingProcessingViewModel.
    /// </summary>
    public class PrintingProcessingViewModel
    {
        /// <summary>
        /// Gets the view.
        /// </summary>
        /// <value>The view.</value>
        public IPrintingProcessingView View { get; private set; }

        /// <summary>
        /// Gets or sets the type of the process.
        /// </summary>
        /// <value>The type of the process.</value>
        public ProcessTypes ProcessType { get; set; }

        /// <summary>
        /// Gets or sets the total record count.
        /// </summary>
        /// <value>The total record count.</value>
        public int TotalRecordCount { get; set; }

        /// <summary>
        /// Gets or sets the record being processed.
        /// </summary>
        /// <value>The record being processed.</value>
        public int RecordBeingProcessed { get; set; }

        /// <summary>
        /// Gets the printer setup arguments.
        /// </summary>
        /// <value>The printer setup arguments.</value>
        public PrinterSetupArgs PrinterSetupArgs { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="PrintingProcessingViewModel"/> is abort.
        /// </summary>
        /// <value><c>true</c> if abort; otherwise, <c>false</c>.</value>
        public bool Abort { get; set; }

        /// <summary>
        /// Gets or sets the abort command.
        /// </summary>
        /// <value>The abort command.</value>
        public RelayCommand AbortCommand { get; set; }

        /// <summary>
        /// The timer
        /// </summary>
        private Timer _timer = new Timer(1000);

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintingProcessingViewModel"/> class.
        /// </summary>
        public PrintingProcessingViewModel()
        {
            AbortCommand = new RelayCommand(() =>
            {
                Abort = true;
            });
        }
        /// <summary>
        /// Initializes the specified view.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="printerSetupArgs">The printer setup arguments.</param>
        public async void Initialize(IPrintingProcessingView view, PrinterSetupArgs printerSetupArgs)
        {
            View = view;
            PrinterSetupArgs = printerSetupArgs;
            printerSetupArgs.PrintingProcessingViewModel = this;
            PrintingInteropGlobals.DeleteAllChunks();
            await Task.Run(() =>
            {
                PrinterSetupArgs.DataProcessor.ProcessPrintOutputData(printerSetupArgs);
                if (Abort)
                {
                    ClearInputData();
                }
                else
                {
                    LaunchPrinter();
                }
            });
        }

        /// <summary>
        /// Updates the status.
        /// </summary>
        /// <param name="currentRecord">The current record.</param>
        /// <param name="totalRecords">The total records.</param>
        /// <param name="processType">Type of the process.</param>
        public void UpdateStatus(int currentRecord, int totalRecords, ProcessTypes processType)
        {
            ProcessType = processType;
            TotalRecordCount = totalRecords;
            RecordBeingProcessed = currentRecord;
            View.UpdateStatus();
        }

        /// <summary>
        /// Launches the printer.
        /// </summary>
        /// <exception cref="System.Exception">SystemGlobals.ProgramDataFolder not set at app startup.</exception>
        private async void LaunchPrinter()
        {
            if (SystemGlobals.ProgramDataFolder.IsNullOrEmpty())
            {
                throw new Exception("SystemGlobals.ProgramDataFolder not set at app startup.");
            }
            PrintingInteropGlobals.PropertiesProcessor.Properties = PrinterSetupArgs.PrintingProperties;

            var result = PrintingInteropGlobals.WriteJsons();
            if (!string.IsNullOrEmpty(result))
            {
                ControlsGlobals.UserInterface.ShowMessageBox(result, "Error", RsMessageBoxIcons.Error);
            }

            var headerChunks = PrinterSetupArgs.PrintingProperties.HeaderChunkCount;
            var detailsChunks = PrinterSetupArgs.PrintingProperties.DetailsChunkCount;
            PrintingInteropGlobals.PropertiesProcessor.DeleteChunkAbortFile();
            PrintingInteropGlobals.PropertiesProcessor.DeleteChunkOutputFile();

            var arguments = new List<string>();
            arguments.Add("-p");
            var programDataFolder = SystemGlobals.ProgramDataFolder;
            if (!programDataFolder.EndsWith("\\"))
            {
                programDataFolder += "\\";
            }
            arguments.Add($"{programDataFolder}{PrintingInteropGlobals.InputFileName}");
            var jsonFile = $"{PrintingInteropGlobals.ProgramDataFolder}{PrintingInteropGlobals.InitializeJsonFileName}";
            var test = System.IO.File.ReadAllText(jsonFile);
            var printInput = JsonConvert.DeserializeObject<PrintingInitializer>(test);
            FileInfo outputFile = new FileInfo($"{SystemGlobals.ProgramDataFolder}\\{PrinterPropertiesProcessor.ChunkOutputFileName}");
            
            System.Diagnostics.Process.Start(printInput.ExePath, arguments);
            var outputFileExists = outputFile.Exists;
            await Task.Run(() =>
            {
                while (!outputFileExists)
                {
                    View.EnableAbortButton(false);
                    ProcessType = ProcessTypes.OpeningApp;
                    System.Threading.Thread.Sleep(100);
                    outputFile = new FileInfo($"{SystemGlobals.ProgramDataFolder}{PrinterPropertiesProcessor.ChunkOutputFileName}");
                    outputFileExists = outputFile.Exists;
                }
            });
            View.EnableAbortButton(true);
            var headerFinished = false;
            var detailsFinished = false;
            _timer.Elapsed += (sender, args) =>
            {
                var chunkBeingProcessed = PrintingInteropGlobals.PropertiesProcessor.GetChunkProcessed();
                if (chunkBeingProcessed == null)
                {
                    AbortProcess();
                }
                else
                {
                    switch (chunkBeingProcessed.ChunkType)
                    {
                        case ChunkType.Header:
                            TotalRecordCount = headerChunks;
                            RecordBeingProcessed = chunkBeingProcessed.ChunkId + 1;
                            ProcessType = ProcessTypes.ProcessReportHeader;
                            headerFinished = headerChunks == chunkBeingProcessed.ChunkId + 1;
                            if (headerChunks <= 1)
                            {
                                headerFinished = true;
                            }

                            if (detailsChunks == 0)
                            {
                                detailsFinished = true;
                            }

                            break;
                        case ChunkType.Details:
                            TotalRecordCount = detailsChunks;
                            RecordBeingProcessed = chunkBeingProcessed.ChunkId + 1;
                            ProcessType = ProcessTypes.ProcessReportDetails;
                            detailsFinished = detailsChunks == chunkBeingProcessed.ChunkId + 1;
                            if (detailsChunks <= 1)
                            {
                                detailsFinished = true;
                            }

                            headerFinished = true;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }


                    View.UpdateStatus();

                    if (Abort || (headerFinished && detailsFinished))
                    {
                        AbortProcess();
                        ClearInputData();
                    }
                }
            };
            if (!Abort)
            {
                _timer.Start();
            }
        }

        /// <summary>
        /// Clears the input data.
        /// </summary>
        private void ClearInputData()
        {
            PrinterSetupArgs.ClearReportFilters();
            PrinterSetupArgs.PrintingProperties.HeaderChunkCount =
                PrinterSetupArgs.PrintingProperties.DetailsChunkCount = 0;
            if (!Abort)
            {
                ProcessType = ProcessTypes.StartingReport;
                View.UpdateStatus();
                Thread.Sleep(new TimeSpan(0,0,0,5));
            }
            View.CloseWindow();
        }

        /// <summary>
        /// Aborts the process.
        /// </summary>
        private void AbortProcess()
        {
            PrintingInteropGlobals.PropertiesProcessor.SetChunkAbort();
            _timer.Stop();
            _timer.Enabled = false;
            _timer.Dispose();
            PrintingInteropGlobals.DeleteAllChunks();
        }

        /// <summary>
        /// Called when [window closing].
        /// </summary>
        public void OnWindowClosing()
        {
            if (!Abort)
            {
                Abort = true;
                AbortProcess();
            }
        }
    }
}
