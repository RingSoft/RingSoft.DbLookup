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
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.Lookup;
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
        public DbMaintenance.TreeViewItem TreeViewItem { get; set; }
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
        public Button OKButton { get; set; }
        public Button CancelButton { get; set; }

        public AdvancedFilterReturn FilterReturn { get; set; }

        static AdvancedFilterWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AdvancedFilterWindow), new FrameworkPropertyMetadata(typeof(AdvancedFilterWindow)));
        }

        public void Initialize(DbMaintenance.TreeViewItem treeViewItem, LookupDefinitionBase lookupDefinition)
        {
            TreeViewItem = treeViewItem;
            LookupDefinition = lookupDefinition;
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
            OKButton = GetTemplateChild(nameof(OKButton)) as Button;
            CancelButton = GetTemplateChild(nameof(CancelButton)) as Button;

            ViewModel = Border.TryFindResource("ViewModel") as AdvancedFilterViewModel;

            ViewModel.Initialize(TreeViewItem, LookupDefinition);
            MemoEditor.CollapseDateButton();

            OKButton.Click += (sender, args) =>
            {
                FilterReturn = ViewModel.GetAdvancedFilterReturn();
                DialogResult = true;
                Close();
            };

            CancelButton.Click += (sender, args) => Close();

            SearchForStringControl.Visibility = Visibility.Collapsed;
            SearchForAutoFillControl.Visibility = Visibility.Collapsed;
            FormulaValueTypeLabel.Visibility = Visibility.Collapsed;
            FormulaValueTypeComboBox.Visibility = Visibility.Collapsed;

            switch (TreeViewItem.Type)
            {
                case TreeViewType.Field:
                    if (TreeViewItem.FieldDefinition.ParentJoinForeignKeyDefinition != null)
                    {
                        SearchForAutoFillControl.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        switch (TreeViewItem.FieldDefinition.FieldDataType)
                        {
                            case FieldDataTypes.String:
                                SearchForStringControl.Visibility = Visibility.Visible;
                                break;
                            case FieldDataTypes.Integer:
                                break;
                            case FieldDataTypes.Decimal:
                                break;
                            case FieldDataTypes.DateTime:
                                break;
                            case FieldDataTypes.Bool:
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
                    switch (ViewModel.FormulaValueType)
                    {
                        case ValueTypes.String:
                        case ValueTypes.Memo:
                            SearchForStringControl.Visibility = Visibility.Visible;
                            break;
                        case ValueTypes.Numeric:
                            break;
                        case ValueTypes.DateTime:
                            break;
                        case ValueTypes.Bool:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            base.OnApplyTemplate();
        }
    }
}
