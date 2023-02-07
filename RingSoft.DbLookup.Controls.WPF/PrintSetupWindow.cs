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

namespace RingSoft.DbLookup.Controls.WPF
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:RingSoft.DbLookup.Controls.WPF"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:RingSoft.DbLookup.Controls.WPF;assembly=RingSoft.DbLookup.Controls.WPF"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:PrintSetupWindow/>
    ///
    /// </summary>
    public class PrintSetupWindow : BaseWindow, IPrinterSetupView
    {
        public Border Border { get; private set; }

        public PrinterSetupViewModel ViewModel { get; private set; }

        static PrintSetupWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PrintSetupWindow), new FrameworkPropertyMetadata(typeof(PrintSetupWindow)));
        }

        public PrintSetupWindow(PrinterSetupArgs printerSetupArgs)
        {
            Loaded += (sender, args) =>
            {
                ViewModel.Initialize(this, printerSetupArgs);
            };
        }

        public override void OnApplyTemplate()
        {
            Border = GetTemplateChild(nameof(Border)) as Border;
            ViewModel = Border.TryFindResource("ViewModel") as PrinterSetupViewModel;

            base.OnApplyTemplate();
        }

        public void PrintOutput()
        {
            var window = new PrintingProcessingWindow(ViewModel.PrinterSetupArgs);
            window.Owner = this;
            window.ShowInTaskbar = false;
            window.ShowDialog();
            Close();
        }

        public void CloseWindow()
        {
            Close();
        }

        public void UpdateView()
        {
            
        }

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
