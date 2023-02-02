using System;
using System.Windows;
using System.Windows.Controls;
using RingSoft.DataEntryControls.WPF;
using RingSoft.Printing.Interop;

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
    ///     <MyNamespace:GenericReportFilterWindow/>
    ///
    /// </summary>
    public class GenericReportFilterWindow : BaseWindow, IGenericReportFilterView
    {
        public Border Border { get; private set; }

        public GenericReportFilterViewModel ViewModel { get; private set; }

        public CheckBox CurrentCheckBox { get; private set; }

        public AutoFillControl CurrentControl { get; private set; }

        public AutoFillControl BeginningControl { get; private set; }

        public AutoFillControl EndingControl { get; private set; }

        public Label ReportTypeLabel { get; private set; }
        public TextComboBoxControl ReportTypeControl { get; private set; }

        static GenericReportFilterWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GenericReportFilterWindow), new FrameworkPropertyMetadata(typeof(GenericReportFilterWindow)));
        }

        public GenericReportFilterWindow(PrinterSetupArgs printerSetup)
        {
            Loaded += (s, e) =>
            {
                ViewModel.Initialize(this, printerSetup);
                Title = $"{printerSetup.CodeDescription} Report Filter Options";
            };
        }

        public override void OnApplyTemplate()
        {
            Border = GetTemplateChild(nameof(Border)) as Border;
            ViewModel = Border.TryFindResource("ViewModel") as GenericReportFilterViewModel;

            CurrentCheckBox = GetTemplateChild(nameof(CurrentCheckBox)) as CheckBox;
            CurrentControl = GetTemplateChild(nameof(CurrentControl)) as AutoFillControl;
            BeginningControl = GetTemplateChild(nameof(BeginningControl)) as AutoFillControl;
            EndingControl = GetTemplateChild(nameof(EndingControl)) as AutoFillControl;
            ReportTypeLabel = GetTemplateChild(nameof(ReportTypeLabel)) as Label;
            ReportTypeControl = GetTemplateChild(nameof(ReportTypeControl)) as TextComboBoxControl;

            base.OnApplyTemplate();
        }

        public void RefreshView()
        {
            BeginningControl.IsEnabled = EndingControl.IsEnabled = !ViewModel.IsCurrentOnly;
            CurrentControl.IsEnabled = ViewModel.IsCurrentOnly;
            if (ViewModel.PrinterSetup.PrintingProperties.ReportType == ReportTypes.Custom)
            {
                ReportTypeLabel.Visibility = ReportTypeControl.Visibility = Visibility.Collapsed;
            }
        }

        public void CloseWindow()
        {
            Close();
        }

        public void PrintOutput()
        {
            LookupControlsGlobals.PrintDocument(ViewModel.PrinterSetup);
        }

        public void FocusControl(GenericFocusControls control)
        {
            switch (control)
            {
                case GenericFocusControls.Current:
                    CurrentControl.Focus();
                    break;
                case GenericFocusControls.Start:
                    BeginningControl.Focus();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(control), control, null);
            }
        }
    }
}
