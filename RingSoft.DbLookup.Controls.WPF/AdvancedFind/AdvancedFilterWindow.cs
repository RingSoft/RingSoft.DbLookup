using System;
using System.Collections.Generic;
using System.Text;
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
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DbMaintenance;
using TreeViewItem = System.Windows.Controls.TreeViewItem;

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
    ///     <MyNamespace:AdvancedFilterWindow/>
    ///
    /// </summary>
    public class AdvancedFilterWindow : BaseWindow
    {
        public DbLookup.AdvancedFind.TreeViewItem TreeViewItem { get; set; }
        public AdvancedFilterReturn InputFilterReturn { get; set; }
        public LookupDefinitionBase LookupDefinition { get; set; }
        public AdvancedFilterViewModel ViewModel { get; set; }

        public Label DisplayLabel { get; set; }
        public StringEditControl DisplayControl { get; set; }
        public Border Border { get; set; }
        public DataEntryMemoEditor MemoEditor { get; set; }
        public Label FormulaValueTypeLabel { get; set; }
        public TextComboBoxControl FormulaValueTypeComboBox { get; set; }
        public Label ConditionLabel { get; set; }
        public TextComboBoxControl ConditionComboBox { get; set; }
        public  Label SearchForLabel { get; set; }
        public StringEditControl SearchForStringControl { get; set; }
        public AutoFillControl SearchForAutoFillControl { get; set; }
        public DecimalEditControl SearchForDecimalControl { get; set; }
        public IntegerEditControl SearchForIntegerControl { get; set; }
        public DateEditControl SearchForDateControl { get; set; }
        public TextComboBoxControl SearchForBoolComboBoxControl { get; set; }
        public Button OKButton { get; set; }
        public Button CancelButton { get; set; }

        private bool _formAdd;

        public AdvancedFilterReturn FilterReturn { get; set; }

        static AdvancedFilterWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AdvancedFilterWindow), new FrameworkPropertyMetadata(typeof(AdvancedFilterWindow)));
        }

        public AdvancedFilterWindow()
        {
            Loaded += (sender, args) => ViewModel.LoadWindow();
        }
        public void Initialize(DbLookup.AdvancedFind.TreeViewItem treeViewItem, LookupDefinitionBase lookupDefinition)
        {
            _formAdd = true;
            TreeViewItem = treeViewItem;
            LookupDefinition = lookupDefinition;
        }

        public void Initialize(AdvancedFilterReturn inputFilterReturn)
        {
            InputFilterReturn = inputFilterReturn;
            LookupDefinition = inputFilterReturn.LookupDefinition;
        }

        public override void OnApplyTemplate()
        {
            Border = GetTemplateChild(nameof(Border)) as Border;
            MemoEditor = GetTemplateChild(nameof(MemoEditor)) as DataEntryMemoEditor;
            DisplayLabel = GetTemplateChild(nameof(DisplayLabel)) as Label;
            DisplayControl = GetTemplateChild(nameof(DisplayControl)) as StringEditControl;
            FormulaValueTypeLabel = GetTemplateChild(nameof(FormulaValueTypeLabel)) as Label;
            FormulaValueTypeComboBox = GetTemplateChild(nameof(FormulaValueTypeComboBox)) as TextComboBoxControl;
            ConditionLabel = GetTemplateChild(nameof(ConditionLabel)) as Label;
            ConditionComboBox = GetTemplateChild(nameof(ConditionComboBox)) as TextComboBoxControl;
            SearchForLabel = GetTemplateChild(nameof(SearchForLabel)) as Label;
            SearchForStringControl = GetTemplateChild(nameof(SearchForStringControl)) as StringEditControl;
            SearchForAutoFillControl = GetTemplateChild(nameof(SearchForAutoFillControl)) as AutoFillControl;
            SearchForDecimalControl = GetTemplateChild(nameof(SearchForDecimalControl)) as DecimalEditControl;
            SearchForIntegerControl = GetTemplateChild(nameof(SearchForIntegerControl)) as IntegerEditControl;
            SearchForDateControl = GetTemplateChild(nameof(SearchForDateControl)) as DateEditControl;
            SearchForBoolComboBoxControl = GetTemplateChild(nameof(SearchForBoolComboBoxControl)) as TextComboBoxControl;

            OKButton = GetTemplateChild(nameof(OKButton)) as Button;
            CancelButton = GetTemplateChild(nameof(CancelButton)) as Button;

            ViewModel = Border.TryFindResource("ViewModel") as AdvancedFilterViewModel;

            ViewModel.OnValidationFail += ViewModel_OnValidationFail;

            if (_formAdd)
            {
                ViewModel.Initialize(TreeViewItem, LookupDefinition);
            }
            else
            {
                ViewModel.Initialize(InputFilterReturn, LookupDefinition);
            }

            SearchForStringControl.Visibility = Visibility.Collapsed;
            SearchForAutoFillControl.Visibility = Visibility.Collapsed;
            SearchForDecimalControl.Visibility = Visibility.Collapsed;
            SearchForIntegerControl.Visibility = Visibility.Collapsed;
            SearchForDateControl.Visibility = Visibility.Collapsed;
            SearchForBoolComboBoxControl.Visibility = Visibility.Collapsed;


            FormulaValueTypeComboBox.SelectionChanged += (sender, args) =>
            {
                SearchForStringControl.Visibility = Visibility.Collapsed;
                SearchForAutoFillControl.Visibility = Visibility.Collapsed;
                SearchForDecimalControl.Visibility = Visibility.Collapsed;
                SearchForIntegerControl.Visibility = Visibility.Collapsed;
                SearchForDateControl.Visibility = Visibility.Collapsed;
                SearchForBoolComboBoxControl.Visibility = Visibility.Collapsed;

                switch (ViewModel.FormulaValueType)
                {
                    case FieldDataTypes.String:
                    case FieldDataTypes.Memo:
                        SearchForStringControl.Visibility = Visibility.Visible;
                        break;
                    case FieldDataTypes.Decimal:
                        SearchForDecimalControl.Visibility = Visibility.Visible;
                        break;
                    case FieldDataTypes.Integer:
                        SearchForIntegerControl.Visibility = Visibility.Visible;
                        break;
                    case FieldDataTypes.DateTime:
                        SearchForDateControl.Visibility = Visibility.Visible;
                        SearchForDateControl.DateFormatType = DateFormatTypes.DateTime;
                        break;
                    case FieldDataTypes.Bool:
                        SearchForBoolComboBoxControl.Visibility = Visibility.Visible;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

            };
            MemoEditor.CollapseDateButton();

            OKButton.Click += (sender, args) =>
            {
                FilterReturn = ViewModel.GetAdvancedFilterReturn();
                if (ViewModel.Validate(FilterReturn))
                {
                    DialogResult = true;
                    Close();
                }
            };

            CancelButton.Click += (sender, args) => Close();

            FormulaValueTypeLabel.Visibility = Visibility.Collapsed;
            FormulaValueTypeComboBox.Visibility = Visibility.Collapsed;

            //if (_formAdd)
            {
                SetupControlNew();
            }
            base.OnApplyTemplate();
        }

        private void ViewModel_OnValidationFail(object sender, ValidationFailArgs e)
        {
            switch (e.Control)
            {
                case ValidationFailControls.Condition:
                    ConditionComboBox.Focus();
                    break;
                case ValidationFailControls.SearchValue:
                    SearchValueFailFocus();
                    break;
                case ValidationFailControls.Formula:
                    MemoEditor.TextBox.Focus();
                    break;
                case ValidationFailControls.FormulaDisplayValue:
                    DisplayControl.Focus();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void SearchValueFailFocus()
        {
            if (SearchForStringControl.Visibility == Visibility.Visible)
            {
                SearchForStringControl.Focus();
            }

            if (SearchForAutoFillControl.Visibility == Visibility.Visible)
            {
                SearchForAutoFillControl.Focus();
            }

            if (SearchForDecimalControl.Visibility == Visibility.Visible)
            {
                SearchForDecimalControl.Focus();
            }

            if (SearchForIntegerControl.Visibility == Visibility.Visible)
            {
                SearchForIntegerControl.Focus();
            }

            if (SearchForDateControl.Visibility == Visibility.Visible)
            {
                SearchForDateControl.Focus();
            }

            if (SearchForBoolComboBoxControl.Visibility == Visibility.Visible)
            {
                SearchForBoolComboBoxControl.Visibility = Visibility.Visible;
            }
        }

        private void SetupControlNew()
        {
            switch (ViewModel.Type)
            {
                case TreeViewType.Field:
                    if (ViewModel.FieldDefinition.ParentJoinForeignKeyDefinition != null)
                    {
                        SearchForAutoFillControl.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        switch (ViewModel.FieldDefinition.FieldDataType)
                        {
                            case FieldDataTypes.String:
                                SearchForStringControl.Visibility = Visibility.Visible;
                                break;
                            case FieldDataTypes.Integer:
                                if (ViewModel.FieldDefinition is IntegerFieldDefinition integerField)
                                {
                                    if (integerField.EnumTranslation != null)
                                    {
                                        SearchForBoolComboBoxControl.Visibility = Visibility.Visible;
                                    }
                                    else
                                    {
                                        SearchForIntegerControl.Visibility = Visibility.Visible;
                                    }
                                }
                                break;
                            case FieldDataTypes.Decimal:
                                SearchForDecimalControl.Visibility = Visibility.Visible;
                                break;
                            case FieldDataTypes.DateTime:
                                var dateField = ViewModel.FieldDefinition as DateFieldDefinition;
                                SearchForDateControl.Visibility = Visibility.Visible;
                                var dateType = DateFormatTypes.DateOnly;
                                switch (dateField.DateType)
                                {
                                    case DbDateTypes.DateOnly:
                                        dateType = DateFormatTypes.DateOnly;
                                        break;
                                    case DbDateTypes.DateTime:
                                        dateType = DateFormatTypes.DateTime;
                                        break;
                                    default:
                                        throw new ArgumentOutOfRangeException();
                                }

                                SearchForDateControl.DateFormatType = dateType;
                                break;
                            case FieldDataTypes.Bool:
                                SearchForBoolComboBoxControl.Visibility = Visibility.Visible;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                    }
                    MemoEditor.Visibility = Visibility.Collapsed;
                    DisplayLabel.Visibility = Visibility.Collapsed;
                    DisplayControl.Visibility = Visibility.Collapsed;
                    break;
                case TreeViewType.Formula:
                    FormulaValueTypeLabel.Visibility = Visibility.Visible;
                    FormulaValueTypeComboBox.Visibility = Visibility.Visible;
                    SearchForStringControl.Visibility = Visibility.Visible;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }
    }
}
