using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
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
    ///     <MyNamespace:PrintingProcessingWindow/>
    ///
    /// </summary>
    public class PrintingProcessingWindow : BaseWindow, IPrintingProcessingView
    {
        public Border Border { get; private set; }

        public PrintingProcessingViewModel ViewModel { get; private set; }

        public StringReadOnlyBox PartTextControl { get; private set; }

        public ProgressBar PartProgressBar { get; private set; }

        public StringReadOnlyBox CurrentControl { get; private set; }

        public ProgressBar CurrentProgressBar { get; private set; }

        public Button CancelButton { get; private set; }

        static PrintingProcessingWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PrintingProcessingWindow), new FrameworkPropertyMetadata(typeof(PrintingProcessingWindow)));
        }

        public PrintingProcessingWindow(PrinterSetupArgs printerSetupArgs)
        {
            Loaded += (s, e) =>
            {
                DataEntryControls.WPF.ControlsUserInterface.SetActiveWindow(this);
                ViewModel.Initialize(this, printerSetupArgs);
                Title = $"{printerSetupArgs.CodeDescription} Report Data Processing";
            };
        }

        public override void OnApplyTemplate()
        {
            Border = GetTemplateChild(nameof(Border)) as Border;
            ViewModel = Border.TryFindResource("ViewModel") as PrintingProcessingViewModel;
            PartTextControl = GetTemplateChild(nameof(PartTextControl)) as StringReadOnlyBox;
            PartProgressBar = GetTemplateChild(nameof(PartProgressBar)) as ProgressBar;
            CurrentControl = GetTemplateChild(nameof(CurrentControl)) as StringReadOnlyBox;
            CurrentProgressBar = GetTemplateChild(nameof(CurrentProgressBar)) as ProgressBar;
            CancelButton = GetTemplateChild(nameof(CancelButton)) as Button;

            base.OnApplyTemplate();
        }

        public void UpdateStatus()
        {
            var part = (int)ViewModel.ProcessType;
            var enumTranslation = new EnumFieldTranslation();
            enumTranslation.LoadFromEnum<ProcessTypes>();
            var enumItem = enumTranslation.TypeTranslations.FirstOrDefault(p => p.NumericValue == part);
            var totalParts = enumTranslation.TypeTranslations.Max(p => p.NumericValue);

            var currentRecord =
                GblMethods.FormatValue(FieldDataTypes.Integer, ViewModel.RecordBeingProcessed.ToString());
            var totalRecords = GblMethods.FormatValue(FieldDataTypes.Integer, ViewModel.TotalRecordCount.ToString());

            Dispatcher.Invoke(() =>
            {
                PartProgressBar.Maximum = totalParts;
                PartProgressBar.Minimum = 0;
                PartProgressBar.Value = enumItem.NumericValue;
                PartTextControl.Text = enumItem.TextValue;
                switch (ViewModel.ProcessType)
                {
                    case ProcessTypes.CountingHeaderRecords:
                    case ProcessTypes.CountingDetailRecords:
                    case ProcessTypes.OpeningApp:
                        CurrentControl.Text = string.Empty;
                        break;
                    case ProcessTypes.StartingReport:
                        ViewModel.AbortCommand.IsEnabled = false;
                        CurrentControl.Text = string.Empty;
                        break;
                    case ProcessTypes.ImportHeader:
                    case ProcessTypes.ImportDetails:
                    case ProcessTypes.ProcessReportHeader:
                    case ProcessTypes.ProcessReportDetails:
                        CurrentControl.Text = $"Processing Item {currentRecord} out of {totalRecords}";
                        CurrentProgressBar.Maximum = ViewModel.TotalRecordCount;
                        CurrentProgressBar.Minimum = 0;
                        CurrentProgressBar.Value = ViewModel.RecordBeingProcessed;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                CancelButton.Focus();
            });
        }

        public void CloseWindow()
        {
            Dispatcher.Invoke(() =>
            {
                Close();
            });
        }

        public void EnableAbortButton(bool enable)
        {
            Dispatcher.Invoke(() =>
            {
                CancelButton.IsEnabled = enable;
            });
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            ViewModel.OnWindowClosing();
            base.OnClosing(e);
        }
    }
}
