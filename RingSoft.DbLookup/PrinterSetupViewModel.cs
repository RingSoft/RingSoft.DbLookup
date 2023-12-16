// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 02-06-2023
//
// Last Modified By : petem
// Last Modified On : 02-07-2023
// ***********************************************************************
// <copyright file="PrinterSetupViewModel.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
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
    /// <summary>
    /// Interface IPrinterSetupView
    /// </summary>
    public interface IPrinterSetupView
    {
        /// <summary>
        /// Prints the output.
        /// </summary>
        void PrintOutput();

        /// <summary>
        /// Closes the window.
        /// </summary>
        void CloseWindow();

        /// <summary>
        /// Updates the view.
        /// </summary>
        void UpdateView();

        /// <summary>
        /// Gets the file.
        /// </summary>
        /// <returns>System.String.</returns>
        string GetFile();
    }
    /// <summary>
    /// Class PrinterSetupViewModel.
    /// Implements the <see cref="INotifyPropertyChanged" />
    /// </summary>
    /// <seealso cref="INotifyPropertyChanged" />
    public class PrinterSetupViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// The output type
        /// </summary>
        private ReportOutputTypes _outputType;

        /// <summary>
        /// Gets or sets the type of the output.
        /// </summary>
        /// <value>The type of the output.</value>
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

        /// <summary>
        /// The output file name
        /// </summary>
        private string _outputFileName;

        /// <summary>
        /// Gets or sets the name of the output file.
        /// </summary>
        /// <value>The name of the output file.</value>
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

        /// <summary>
        /// The number of copies
        /// </summary>
        private int _numberOfCopies;

        /// <summary>
        /// Gets or sets the number of copies.
        /// </summary>
        /// <value>The number of copies.</value>
        public int NumberOfCopies
        {
            get => _numberOfCopies;
            set
            {
                if (_numberOfCopies == value)
                    return;

                _numberOfCopies = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The file type box control setup
        /// </summary>
        private TextComboBoxControlSetup _fileTypeBoxControlSetup;

        /// <summary>
        /// Gets or sets the file type box control setup.
        /// </summary>
        /// <value>The file type box control setup.</value>
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

        /// <summary>
        /// The file type ComboBox item
        /// </summary>
        private TextComboBoxItem _fileTypeComboBoxItem;

        /// <summary>
        /// Gets or sets the file type ComboBox item.
        /// </summary>
        /// <value>The file type ComboBox item.</value>
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

        /// <summary>
        /// Gets or sets the type of the file.
        /// </summary>
        /// <value>The type of the file.</value>
        public ExportFileTypes FileType
        {
            get => (ExportFileTypes)FileTypeComboBoxItem.NumericValue;
            set => FileTypeComboBoxItem = FileTypeBoxControlSetup.GetItem((int)value);
        }

        /// <summary>
        /// Gets the view.
        /// </summary>
        /// <value>The view.</value>
        public IPrinterSetupView View { get; private set; }

        /// <summary>
        /// Gets the ok command.
        /// </summary>
        /// <value>The ok command.</value>
        public RelayCommand OkCommand { get; private set; }

        /// <summary>
        /// Gets the cancel command.
        /// </summary>
        /// <value>The cancel command.</value>
        public RelayCommand CancelCommand { get; private set; }

        /// <summary>
        /// Gets the file command.
        /// </summary>
        /// <value>The file command.</value>
        public RelayCommand FileCommand { get; private set; }

        /// <summary>
        /// Gets the printer setup arguments.
        /// </summary>
        /// <value>The printer setup arguments.</value>
        public PrinterSetupArgs PrinterSetupArgs { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrinterSetupViewModel"/> class.
        /// </summary>
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
        /// <summary>
        /// Initializes the specified view.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="printerSetupArgs">The printer setup arguments.</param>
        public void Initialize(IPrinterSetupView view, PrinterSetupArgs printerSetupArgs)
        {
            View = view;
            PrinterSetupArgs = printerSetupArgs;
            OutputType = ReportOutputTypes.Printer;
            FileType = ExportFileTypes.Pdf;
            NumberOfCopies = 1;

            View.UpdateView();
        }

        /// <summary>
        /// Called when [ok].
        /// </summary>
        private void OnOk()
        {
            PrinterSetupArgs.PrintingProperties.ReportOutputType = OutputType;
            PrinterSetupArgs.PrintingProperties.ExportFileType = FileType;

            PrinterSetupArgs.PrintingProperties.ExportPathFileName = OutputFileName;
            PrinterSetupArgs.PrintingProperties.NumberOfCopies = NumberOfCopies;
            View.PrintOutput();
        }

        /// <summary>
        /// Sets the type of the file.
        /// </summary>
        private void SetFileType()
        {
            var fileName = OutputFileName;
            var extension = string.Empty;

            extension = GetExtension();

            if (fileName.IsNullOrEmpty())
            {
                var folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                fileName = $"{PrinterSetupArgs.PrintingProperties.ReportTitle}{extension}";
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

        /// <summary>
        /// Gets the extension.
        /// </summary>
        /// <returns>System.String.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
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

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Sets the field.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
