using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using RingSoft.DataEntryControls.Engine;
using RingSoft.Printing.Interop;

namespace RingSoft.DbLookup
{
    public interface IPrinterSetupView
    {
        void PrintOutput();

        void CloseWindow();

        void UpdateView();

        string GetFile();
    }
    public class PrinterSetupViewModel : INotifyPropertyChanged
    {
        private ReportOutputTypes _outputType;

        public ReportOutputTypes OutputType
        {
            get => _outputType;
            set
            {
                if (_outputType == value)
                {
                    return;
                }
                _outputType = value;
                OnPropertyChanged();
                View?.UpdateView();
            }
        }

        private string _outputFileName;

        public string OutputFileName
        {
            get => _outputFileName;
            set
            {
                if (_outputFileName == value)
                {
                    return;
                }
                _outputFileName = value;
                OnPropertyChanged();
            }
        }

        private TextComboBoxControlSetup _fileTypeBoxControlSetup;

        public TextComboBoxControlSetup FileTypeBoxControlSetup
        {
            get => _fileTypeBoxControlSetup;
            set
            {
                if (_fileTypeBoxControlSetup == value)
                    return;

                _fileTypeBoxControlSetup = value;
                OnPropertyChanged();
            }
        }

        private TextComboBoxItem _fileTypeComboBoxItem;

        public TextComboBoxItem FileTypeComboBoxItem
        {
            get => _fileTypeComboBoxItem;
            set
            {
                if (_fileTypeComboBoxItem == value)
                    return;

                _fileTypeComboBoxItem = value;
                OnPropertyChanged();
                SetFileType();
            }
        }

        public ExportFileTypes FileType
        {
            get => (ExportFileTypes)FileTypeComboBoxItem.NumericValue;
            set => FileTypeComboBoxItem = FileTypeBoxControlSetup.GetItem((int)value);
        }

        public IPrinterSetupView View { get; private set; }

        public RelayCommand OkCommand { get; private set; }

        public RelayCommand CancelCommand { get; private set; }

        public RelayCommand FileCommand { get; private set; }

        public PrinterSetupArgs PrinterSetupArgs { get; private set; }

        public PrinterSetupViewModel()
        {
            FileTypeBoxControlSetup = new TextComboBoxControlSetup();
            FileTypeBoxControlSetup.LoadFromEnum<ExportFileTypes>();

            OkCommand = new RelayCommand(OnOk);
            CancelCommand = new RelayCommand(() =>
            {
                View.CloseWindow();
            });

            FileCommand = new RelayCommand(() =>
            {
                OutputFileName = View.GetFile();
                SetFileType();
            });
        }
        public void Initialize(IPrinterSetupView view, PrinterSetupArgs printerSetupArgs)
        {
            View = view;
            PrinterSetupArgs = printerSetupArgs;
            OutputType = ReportOutputTypes.Printer;
            FileType = ExportFileTypes.Pdf;
            View.UpdateView();
        }

        private void OnOk()
        {
            PrinterSetupArgs.PrintingProperties.ReportOutputType = OutputType;
            View.PrintOutput();
        }

        private void SetFileType()
        {
            var fileName = OutputFileName;
            var extension = string.Empty;

            extension = GetExtension();

            if (fileName.IsNullOrEmpty())
            {
                var folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                fileName = $"{PrinterSetupArgs.CodeDescription}{extension}";
                OutputFileName = $"{folder}\\{fileName}";
            }
            else
            {
                if (!fileName.EndsWith(extension))
                {
                    var file = new FileInfo(fileName);
                    var folder = file.Directory;
                    var folderName = string.Empty;
                    if (folder != null)
                    {
                        folderName = folder.ToString();
                    }

                    if (folderName.IsNullOrEmpty())
                    {
                        folderName = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    }

                    if (!folderName.EndsWith("\\"))
                    {
                        folderName += "\\";
                    }

                    fileName = file.Name;
                    var extPos = fileName.LastIndexOf('.');
                    fileName = fileName.LeftStr(extPos);

                    OutputFileName = $"{folderName}{fileName}{extension}";
                }
            }
        }

        public string GetExtension()
        {
            string extension;
            switch (FileType)
            {
                case ExportFileTypes.Pdf:
                    extension = ".pdf";
                    break;
                case ExportFileTypes.Crystal:
                    extension = ".rpt";
                    break;
                case ExportFileTypes.Excel:
                    extension = ".xls";
                    break;
                case ExportFileTypes.Html:
                    extension = ".html";
                    break;
                case ExportFileTypes.Rtf:
                    extension = ".rtf";
                    break;
                case ExportFileTypes.Word:
                    extension = ".doc";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return extension;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
