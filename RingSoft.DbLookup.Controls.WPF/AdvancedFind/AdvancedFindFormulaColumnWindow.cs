using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DataEntryControls.WPF.DataEntryGrid;
using RingSoft.DbLookup.AdvancedFind;
using System.Windows;
using System.Windows.Controls;

namespace RingSoft.DbLookup.Controls.WPF.AdvancedFind
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:RingSoft.DbLookup.Controls.WPF.AdvancedFind"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:RingSoft.DbLookup.Controls.WPF.AdvancedFind;assembly=RingSoft.DbLookup.Controls.WPF.AdvancedFind"
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
    ///     <MyNamespace:AdvancedFindFormulaColumnWindow/>
    ///
    /// </summary>
    public class AdvancedFindFormulaColumnWindow : DataEntryGridMemoEditor
    {

        public Border Border { get; set; }
        public TextComboBoxControl FieldDataTypeComboBox { get; set; }
        public Label FormatTypeLabel { get; set; }
        public TextComboBoxControl FormatTypeComboBox { get; set; }
        public DataEntryMemoEditor MemoEditor { get; set; }

        public string ParentTable { get; set; }
        public string ParentField { get; set; }
        public FieldDataTypes DataType { get; set; }
        public DecimalEditFormatTypes DecimalFormat { get; set; }
        public AdvancedFindFormulaColumnViewModel ViewModel { get; set; }

        static AdvancedFindFormulaColumnWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AdvancedFindFormulaColumnWindow), new FrameworkPropertyMetadata(typeof(AdvancedFindFormulaColumnWindow)));
        }

        public AdvancedFindFormulaColumnWindow(DataEntryGridMemoValue gridMemoValue) : base(gridMemoValue)
        {
        }

        public override void OnApplyTemplate()
        {
            Border = GetTemplateChild(nameof(Border)) as Border;
            FieldDataTypeComboBox = GetTemplateChild(nameof(FieldDataTypeComboBox)) as TextComboBoxControl;
            FormatTypeLabel = GetTemplateChild(nameof(FormatTypeLabel)) as Label;
            FormatTypeComboBox = GetTemplateChild(nameof(FormatTypeComboBox)) as TextComboBoxControl;
            MemoEditor = GetTemplateChild(nameof(MemoEditor)) as DataEntryMemoEditor;

            base.OnApplyTemplate();

            ViewModel = Border.TryFindResource("ViewModel") as AdvancedFindFormulaColumnViewModel;
            ViewModel.Initialize();
            ViewModel.Table = ParentTable;
            ViewModel.Field = ParentField;
            ViewModel.DataType = DataType;

            SetDecimalFormatType();
            FieldDataTypeComboBox.SelectionChanged += (sender, args) => { SetDecimalFormatType(); };

            if (DataType == FieldDataTypes.Decimal)
            {
                ViewModel.DecimalFormatType = DecimalFormat;
            }
            FieldDataTypeComboBox.SelectionChanged += FieldDataTypeComboBox_SelectionChanged;
            MemoEditor.CollapseDateButton();
        }

        private void SetDecimalFormatType()
        {
            if (ViewModel.DataType == FieldDataTypes.Decimal)
            {
                FormatTypeComboBox.Visibility = Visibility.Visible;
                FormatTypeLabel.Visibility = Visibility.Visible;
            }
            else
            {
                FormatTypeComboBox.Visibility = Visibility.Collapsed;
                FormatTypeLabel.Visibility = Visibility.Collapsed;
            }
        }

        protected override bool Validate()
        {
            if (FormatTypeComboBox.Visibility == Visibility.Visible && ViewModel.DecimalFormatComboBoxItem == null)
            {
                var message = "You must select a data format type.";
                var caption = "Invalid Data Format Type";
                ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
                FormatTypeComboBox.Focus();
                return false;

            }

            if (MemoEditor.Text.IsNullOrEmpty())
            {
                var message = "Formula cannot be empty.";
                var caption = "Invalid Formula";
                ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
                MemoEditor.TextBox.Focus();
                return false;
            }

            return base.Validate();
        }

        private void FieldDataTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ViewModel.DataType == FieldDataTypes.Decimal)
            {
                FormatTypeComboBox.IsEnabled = true;
            }
            else
            {
                FormatTypeComboBox.IsEnabled = false;
                FormatTypeComboBox.SelectedItem = null;
            }
        }
    }
}
