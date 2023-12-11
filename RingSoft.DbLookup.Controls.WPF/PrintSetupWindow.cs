// ***********************************************************************
// Assembly         : RingSoft.DbLookup.Controls.WPF
// Author           : petem
// Created          : 02-06-2023
//
// Last Modified By : petem
// Last Modified On : 02-07-2023
// ***********************************************************************
// <copyright file="PrintSetupWindow.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.WPF;
using RingSoft.Printing.Interop;

namespace RingSoft.DbLookup.Controls.WPF
{
    /// <summary>
    /// Class PrintSetupWindow.
    /// Implements the <see cref="BaseWindow" />
    /// Implements the <see cref="RingSoft.DbLookup.IPrinterSetupView" />
    /// </summary>
    /// <seealso cref="BaseWindow" />
    /// <seealso cref="RingSoft.DbLookup.IPrinterSetupView" />
    /// <font color="red">Badly formed XML comment.</font>
    public class PrintSetupWindow : BaseWindow, IPrinterSetupView
    {
        /// <summary>
        /// Gets the border.
        /// </summary>
        /// <value>The border.</value>
        public Border Border { get; private set; }

        /// <summary>
        /// Gets the view model.
        /// </summary>
        /// <value>The view model.</value>
        public PrinterSetupViewModel ViewModel { get; private set; }

        /// <summary>
        /// Gets the file panel.
        /// </summary>
        /// <value>The file panel.</value>
        public StackPanel FilePanel { get; private set; }

        /// <summary>
        /// Gets the number copies grid.
        /// </summary>
        /// <value>The number copies grid.</value>
        public Grid NumberCopiesGrid { get; private set; }

        /// <summary>
        /// Gets the file type grid.
        /// </summary>
        /// <value>The file type grid.</value>
        public Grid FileTypeGrid { get; private set; }

        /// <summary>
        /// Initializes static members of the <see cref="PrintSetupWindow"/> class.
        /// </summary>
        static PrintSetupWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PrintSetupWindow), new FrameworkPropertyMetadata(typeof(PrintSetupWindow)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintSetupWindow"/> class.
        /// </summary>
        /// <param name="printerSetupArgs">The printer setup arguments.</param>
        public PrintSetupWindow(PrinterSetupArgs printerSetupArgs)
        {
            Loaded += (sender, args) =>
            {
                ViewModel.Initialize(this, printerSetupArgs);
            };
        }

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
        /// </summary>
        public override void OnApplyTemplate()
        {
            Border = GetTemplateChild(nameof(Border)) as Border;
            ViewModel = Border.TryFindResource("ViewModel") as PrinterSetupViewModel;
            FilePanel = GetTemplateChild(nameof(FilePanel)) as StackPanel;
            NumberCopiesGrid = GetTemplateChild(nameof(NumberCopiesGrid)) as Grid;
            FileTypeGrid = GetTemplateChild(nameof(FileTypeGrid)) as Grid;

            base.OnApplyTemplate();
        }

        /// <summary>
        /// Prints the output.
        /// </summary>
        public void PrintOutput()
        {
            var window = new PrintingProcessingWindow(ViewModel.PrinterSetupArgs);
            window.Owner = this;
            window.ShowInTaskbar = false;
            window.ShowDialog();
            Close();
        }

        /// <summary>
        /// Closes the window.
        /// </summary>
        public void CloseWindow()
        {
            Close();
        }

        /// <summary>
        /// Updates the view.
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public void UpdateView()
        {
            NumberCopiesGrid.Visibility = Visibility.Collapsed;
            FileTypeGrid.Visibility = Visibility.Collapsed;

            switch (ViewModel.OutputType)
            {
                case ReportOutputTypes.Printer:
                    FilePanel.IsEnabled = false;
                    NumberCopiesGrid.Visibility = Visibility.Visible;
                    NumberCopiesGrid.IsEnabled = true;
                    break;
                case ReportOutputTypes.Screen:
                    FilePanel.IsEnabled = false;
                    NumberCopiesGrid.Visibility = Visibility.Visible;
                    NumberCopiesGrid.IsEnabled = false;
                    break;
                case ReportOutputTypes.File:
                    FilePanel.IsEnabled = true;
                    FileTypeGrid.Visibility = Visibility.Visible;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Gets the file.
        /// </summary>
        /// <returns>System.String.</returns>
        public string GetFile()
        {
            var file = new FileInfo(ViewModel.OutputFileName);
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

            var extension = ViewModel.GetExtension();
            if (extension != null)
            {
                extension = extension.TrimStart('.');
            }

            var fileName = string.Empty;
            if (file != null)
            {
                fileName = file.Name;
            }
            var saveFileDialog = new SaveFileDialog
            {
                FileName = fileName,
                InitialDirectory = folderName,
                DefaultExt = extension,
                Filter = $"{ViewModel.FileTypeComboBoxItem.TextValue}|*.{extension}"
            };

            var result = saveFileDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                return saveFileDialog.FileName;
            }

            return ViewModel.OutputFileName;
        }
    }
}
