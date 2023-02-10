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
    public enum ProcessTypes
    {
        [Description("Counting Header Records")]
        CountingHeaderRecords = 0,
        [Description("Importing Header Records")]
        ImportHeader = 1,
        [Description("Counting Detail Records")]
        CountingDetailRecords = 2,
        [Description("Importing Detail Records")]
        ImportDetails = 3,
        [Description("Opening Printing App")]
        OpeningApp = 4,
        [Description("Processing Header Chunks")]
        ProcessReportHeader = 5,
        [Description("Processing Detail Chunks")]
        ProcessReportDetails = 6,
        [Description("Starting Report")]
        StartingReport = 7,
    }
    public interface IPrintingProcessingView
    {
        void UpdateStatus();

        void CloseWindow();

        void EnableAbortButton(bool enable);
    }

    public class PrintingProcessingViewModel
    {
        public IPrintingProcessingView View { get; private set; }

        public ProcessTypes ProcessType { get; set; }

        public int TotalRecordCount { get; set; }

        public int RecordBeingProcessed { get; set; }

        public PrinterSetupArgs PrinterSetupArgs { get; private set; }

        public bool Abort { get; set; }

        public RelayCommand AbortCommand { get; set; }

        private Timer _timer = new Timer(1000);

        public PrintingProcessingViewModel()
        {
            AbortCommand = new RelayCommand(() =>
            {
                Abort = true;
            });
        }
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

        public void UpdateStatus(int currentRecord, int totalRecords, ProcessTypes processType)
        {
            ProcessType = processType;
            TotalRecordCount = totalRecords;
            RecordBeingProcessed = currentRecord;
            View.UpdateStatus();
        }

        private async void LaunchPrinter()
        {
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

        private void AbortProcess()
        {
            PrintingInteropGlobals.PropertiesProcessor.SetChunkAbort();
            _timer.Stop();
            _timer.Enabled = false;
            _timer.Dispose();
            PrintingInteropGlobals.DeleteAllChunks();
        }

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
