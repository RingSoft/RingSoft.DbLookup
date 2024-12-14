// ***********************************************************************
// Assembly         : RingSoft.DbLookup.Controls.WPF
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 07-08-2023
// ***********************************************************************
// <copyright file="AdvancedFilterWindow.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
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
using RingSoft.DbLookup.TableProcessing;
using RingSoft.DbMaintenance;
using TreeViewItem = System.Windows.Controls.TreeViewItem;

namespace RingSoft.DbLookup.Controls.WPF.AdvancedFind
{
    /// <summary>
    /// Class AdvancedFilterWindow.
    /// Implements the <see cref="BaseWindow" />
    /// </summary>
    /// <seealso cref="BaseWindow" />
    /// <font color="red">Badly formed XML comment.</font>
    public class AdvancedFilterWindow : BaseWindow, IAdvFilterView
    {
        /// <summary>
        /// Gets or sets the TreeView item.
        /// </summary>
        /// <value>The TreeView item.</value>
        public DbLookup.AdvancedFind.TreeViewItem TreeViewItem { get; set; }
        /// <summary>
        /// Gets or sets the input filter return.
        /// </summary>
        /// <value>The input filter return.</value>
        public AdvancedFilterReturn InputFilterReturn { get; set; }
        /// <summary>
        /// Gets or sets the lookup definition.
        /// </summary>
        /// <value>The lookup definition.</value>
        public LookupDefinitionBase LookupDefinition { get; set; }
        /// <summary>
        /// Gets or sets the view model.
        /// </summary>
        /// <value>The view model.</value>
        public AdvancedFilterViewModel ViewModel { get; set; }

        /// <summary>
        /// Gets or sets the field label.
        /// </summary>
        /// <value>The field label.</value>
        public Label FieldLabel { get; set; }
        /// <summary>
        /// Gets or sets the field control.
        /// </summary>
        /// <value>The field control.</value>
        public StringReadOnlyBox FieldControl { get; set; }
        /// <summary>
        /// Gets or sets the display label.
        /// </summary>
        /// <value>The display label.</value>
        public Label DisplayLabel { get; set; }
        /// <summary>
        /// Gets or sets the display control.
        /// </summary>
        /// <value>The display control.</value>
        public StringEditControl DisplayControl { get; set; }
        /// <summary>
        /// Gets or sets the border.
        /// </summary>
        /// <value>The border.</value>
        public Border Border { get; set; }
        /// <summary>
        /// Gets or sets the memo editor.
        /// </summary>
        /// <value>The memo editor.</value>
        public DataEntryMemoEditor MemoEditor { get; set; }
        /// <summary>
        /// Gets or sets the formula value type label.
        /// </summary>
        /// <value>The formula value type label.</value>
        public Label FormulaValueTypeLabel { get; set; }
        /// <summary>
        /// Gets or sets the formula value type ComboBox.
        /// </summary>
        /// <value>The formula value type ComboBox.</value>
        public TextComboBoxControl FormulaValueTypeComboBox { get; set; }
        /// <summary>
        /// Gets or sets the condition label.
        /// </summary>
        /// <value>The condition label.</value>
        public Label ConditionLabel { get; set; }
        /// <summary>
        /// Gets or sets the condition ComboBox.
        /// </summary>
        /// <value>The condition ComboBox.</value>
        public TextComboBoxControl ConditionComboBox { get; set; }
        /// <summary>
        /// Gets or sets the search for label.
        /// </summary>
        /// <value>The search for label.</value>
        public Label SearchForLabel { get; set; }
        /// <summary>
        /// Gets or sets the search for string control.
        /// </summary>
        /// <value>The search for string control.</value>
        public StringEditControl SearchForStringControl { get; set; }
        /// <summary>
        /// Gets or sets the search for automatic fill control.
        /// </summary>
        /// <value>The search for automatic fill control.</value>
        public AutoFillControl SearchForAutoFillControl { get; set; }
        /// <summary>
        /// Gets or sets the search for decimal control.
        /// </summary>
        /// <value>The search for decimal control.</value>
        public DecimalEditControl SearchForDecimalControl { get; set; }
        /// <summary>
        /// Gets or sets the search for integer control.
        /// </summary>
        /// <value>The search for integer control.</value>
        public IntegerEditControl SearchForIntegerControl { get; set; }
        /// <summary>
        /// Gets or sets the search for date control.
        /// </summary>
        /// <value>The search for date control.</value>
        public DateEditControl SearchForDateControl { get; set; }

        /// <summary>
        /// Gets or sets the date filter type ComboBox control.
        /// </summary>
        /// <value>The date filter type ComboBox control.</value>
        public TextComboBoxControl DateFilterTypeComboBoxControl { get; set; }
        /// <summary>
        /// Gets or sets the date panel.
        /// </summary>
        /// <value>The date panel.</value>
        public StackPanel DatePanel { get; set; }

        /// <summary>
        /// Gets or sets the date value control.
        /// </summary>
        /// <value>The date value control.</value>
        public IntegerEditControl DateValueControl { get; set; }

        /// <summary>
        /// Gets or sets the search for bool ComboBox control.
        /// </summary>
        /// <value>The search for bool ComboBox control.</value>
        public TextComboBoxControl SearchForBoolComboBoxControl { get; set; }

        public StackPanel SearchForValuePanel { get; set; }

        public LookupSearchForHost SearchValueHost { get; set; }

        /// <summary>
        /// Gets or sets the ok button.
        /// </summary>
        /// <value>The ok button.</value>
        public Button OKButton { get; set; }
        /// <summary>
        /// Gets or sets the cancel button.
        /// </summary>
        /// <value>The cancel button.</value>
        public Button CancelButton { get; set; }

        /// <summary>
        /// The form add
        /// </summary>
        private bool _formAdd;
        /// <summary>
        /// The loading
        /// </summary>
        private bool _loading = true;

        /// <summary>
        /// Gets or sets the filter return.
        /// </summary>
        /// <value>The filter return.</value>
        public AdvancedFilterReturn FilterReturn { get; set; }

        /// <summary>
        /// Initializes static members of the <see cref="AdvancedFilterWindow"/> class.
        /// </summary>
        static AdvancedFilterWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AdvancedFilterWindow), new FrameworkPropertyMetadata(typeof(AdvancedFilterWindow)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdvancedFilterWindow"/> class.
        /// </summary>
        public AdvancedFilterWindow()
        {
            Loaded += (sender, args) =>
            {
                ViewModel.LoadWindow();
                MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
                _loading = false;
            };
        }
        /// <summary>
        /// Initializes the specified tree view item.
        /// </summary>
        /// <param name="treeViewItem">The tree view item.</param>
        /// <param name="lookupDefinition">The lookup definition.</param>
        public void Initialize(DbLookup.AdvancedFind.TreeViewItem treeViewItem, LookupDefinitionBase lookupDefinition)
        {
            _formAdd = true;
            TreeViewItem = treeViewItem;
            LookupDefinition = lookupDefinition;
        }

        /// <summary>
        /// Initializes the specified input filter return.
        /// </summary>
        /// <param name="inputFilterReturn">The input filter return.</param>
        public void Initialize(AdvancedFilterReturn inputFilterReturn)
        {
            InputFilterReturn = inputFilterReturn;
            LookupDefinition = inputFilterReturn.LookupDefinition;
        }

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
        /// </summary>
        public override void OnApplyTemplate()
        {
            Border = GetTemplateChild(nameof(Border)) as Border;
            MemoEditor = GetTemplateChild(nameof(MemoEditor)) as DataEntryMemoEditor;
            FieldLabel = GetTemplateChild(nameof(FieldLabel)) as Label;
            FieldControl = GetTemplateChild(nameof(FieldControl)) as StringReadOnlyBox;
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
            DatePanel = GetTemplateChild(nameof(DatePanel)) as StackPanel;
            DateFilterTypeComboBoxControl = GetTemplateChild(nameof(DateFilterTypeComboBoxControl)) as TextComboBoxControl;
            DateValueControl = GetTemplateChild(nameof(DateValueControl)) as IntegerEditControl;
            SearchForValuePanel = GetTemplateChild(nameof(SearchForValuePanel)) as StackPanel;

            OKButton = GetTemplateChild(nameof(OKButton)) as Button;

            CancelButton = GetTemplateChild(nameof(CancelButton)) as Button;

            ViewModel = Border.TryFindResource("ViewModel") as AdvancedFilterViewModel;

            ViewModel.OnValidationFail += ViewModel_OnValidationFail;

            if (_formAdd)
            {
                ViewModel.Initialize(this, TreeViewItem, LookupDefinition);
            }
            else
            {
                ViewModel.Initialize(this, InputFilterReturn, LookupDefinition);
            }

            HideSearchValues();

            ConditionComboBox.SelectionChanged += (sender, args) =>
            {
                HideSearchValues();
                switch (ViewModel.Type)
                {
                    case TreeViewType.Field:
                        SetFieldSearch();
                        break;
                    case TreeViewType.Formula:
                        ShowSearchValue(ViewModel.FormulaValueType);
                        break;
                }
            };

            
            FormulaValueTypeComboBox.SelectionChanged += (sender, args) =>
            {
                HideSearchValues();
                ShowSearchValue(ViewModel.FormulaValueType);
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

            DateFilterTypeComboBoxControl.SelectionChanged += (sender, args) =>
            {
                SetupDateControls();
                if (!_loading)
                {
                    if (DateValueControl.Visibility == Visibility.Visible)
                    {
                        DateValueControl.Focus();
                    }

                    if (SearchForDateControl.Visibility == Visibility.Visible)
                    {
                        SearchForDateControl.Focus();
                    }
                }
            };
            //if (_formAdd)
            {
                SetupControlNew();
            }
            base.OnApplyTemplate();
        }

        /// <summary>
        /// Hides the search values.
        /// </summary>
        private void HideSearchValues()
        {
            SearchForStringControl.Visibility = Visibility.Collapsed;
            SearchForAutoFillControl.Visibility = Visibility.Collapsed;
            SearchForDecimalControl.Visibility = Visibility.Collapsed;
            SearchForIntegerControl.Visibility = Visibility.Collapsed;
            DatePanel.Visibility = Visibility.Collapsed;
            SearchForBoolComboBoxControl.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Setups the date controls.
        /// </summary>
        private void SetupDateControls()
        {
            DateValueControl.Visibility = Visibility.Collapsed;
            SearchForDateControl.Visibility = Visibility.Collapsed;
            var dateFilterType = DateFilterTypes.SpecificDate;
            if (ViewModel != null && ViewModel.DateFilterTypeComboBoxControlSetup != null
                && ViewModel.DateFilterTypeComboBoxItem != null)
            {
                dateFilterType = ViewModel.DateFilterType;
            }
            switch (dateFilterType)
            {
                case DateFilterTypes.SpecificDate:
                    SearchForDateControl.Visibility = Visibility.Visible;
                    break;
                default:
                    DateValueControl.Visibility = Visibility.Visible;
                    break;
            }
        }
        /// <summary>
        /// Checks the condition.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool CheckCondition()
        {
            switch (ViewModel.Condition)
            {
                case Conditions.EqualsNull:
                case Conditions.NotEqualsNull:
                    return false;
                
                default:
                    return true;
            }
        }

        /// <summary>
        /// Views the model on validation fail.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
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

        /// <summary>
        /// Searches the value fail focus.
        /// </summary>
        private void SearchValueFailFocus()
        {
            //if (SearchForStringControl.Visibility == Visibility.Visible)
            //{
            //    SearchForStringControl.Focus();
            //}

            if (SearchForAutoFillControl.Visibility == Visibility.Visible)
            {
                SearchForAutoFillControl.Focus();
            }

            if (SearchValueHost != null && SearchValueHost.Control.Visibility == Visibility.Visible)
            {
                SearchValueHost.Control.Focus();
            }

            //if (SearchForDecimalControl.Visibility == Visibility.Visible)
            //{
            //    SearchForDecimalControl.Focus();
            //}

            //if (SearchForIntegerControl.Visibility == Visibility.Visible)
            //{
            //    SearchForIntegerControl.Focus();
            //}

            if (DatePanel.Visibility == Visibility.Visible)
            {
                if (SearchForDateControl.Visibility == Visibility.Visible)
                {
                    SearchForDateControl.Focus();
                }

                if (DateValueControl.Visibility == Visibility.Visible)
                {
                    DateValueControl.Focus();
                }

            }

            if (SearchForBoolComboBoxControl.Visibility == Visibility.Visible)
            {
                SearchForBoolComboBoxControl.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Setups the control new.
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        private void SetupControlNew()
        {
            switch (ViewModel.Type)
            {
                case TreeViewType.Field:
                    SetFieldSearch();
                    MemoEditor.Visibility = Visibility.Collapsed;
                    DisplayLabel.Visibility = Visibility.Collapsed;
                    DisplayControl.Visibility = Visibility.Collapsed;
                    break;
                case TreeViewType.Formula:
                    FieldLabel.Visibility = FieldControl.Visibility = Visibility.Collapsed;
                    FormulaValueTypeLabel.Visibility = Visibility.Visible;
                    FormulaValueTypeComboBox.Visibility = Visibility.Visible;
                    if (CheckCondition())
                    {
                        SearchForStringControl.Visibility = Visibility.Visible;
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }

        /// <summary>
        /// Sets the field search.
        /// </summary>
        private void SetFieldSearch()
        {
            if (ViewModel.FieldDefinition.ParentJoinForeignKeyDefinition != null)
            {
                if (CheckCondition())
                {
                    SearchForLabel.Visibility = Visibility.Visible;
                    SearchForAutoFillControl.Visibility = Visibility.Visible;
                }
                else
                {
                    SearchForLabel.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                //if (ViewModel.FieldDefinition is IntegerFieldDefinition integerField)
                //{
                //    if (integerField.EnumTranslation != null)
                //    {
                //        if (CheckCondition())
                //        {
                //            SearchForBoolComboBoxControl.Visibility = Visibility.Visible;
                //            return;
                //        }
                //    }
                //}
                ShowSearchValue(ViewModel.FieldDefinition.FieldDataType);
            }
        }

        /// <summary>
        /// Shows the search value.
        /// </summary>
        /// <param name="dataType">Type of the data.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        private void ShowSearchValue(FieldDataTypes dataType)
        {
            if (CheckCondition())
            {
                SearchForLabel.Visibility = Visibility.Visible;
                SearchForValuePanel.Visibility = Visibility.Visible;
            }
            else
            {
                SearchForLabel.Visibility = Visibility.Collapsed;
                SearchForValuePanel.Visibility = Visibility.Collapsed;
            }

            switch (dataType)
            {
                //case FieldDataTypes.Bool:
                case FieldDataTypes.DateTime:
                    break;
                default:
                    if (SearchValueHost == null && CheckCondition())
                    {
                        SearchValueHost = LookupControlsGlobals
                            .LookupControlSearchForFactory
                            .CreateSearchForHost(ViewModel.FieldDefinition);

                        SearchForValuePanel.Children.Add(SearchValueHost.Control);
                        SearchForValuePanel.UpdateLayout();
                    }
                    return;
            }

            switch (dataType)
            {
                //case FieldDataTypes.String:
                //    if (CheckCondition())
                //    {
                //        SearchForStringControl.Visibility = Visibility.Visible;
                //    }

                //    break;
                //case FieldDataTypes.Integer:
                //    if (CheckCondition())
                //    {
                //        SearchForIntegerControl.Visibility = Visibility.Visible;
                //    }
                //    break;
                //case FieldDataTypes.Decimal:
                //    if (CheckCondition())
                //    {
                //        SearchForDecimalControl.Visibility = Visibility.Visible;
                //    }

                //    break;
                case FieldDataTypes.DateTime:
                    var dateField = ViewModel.FieldDefinition as DateFieldDefinition;
                    if (CheckCondition())
                    {
                        DatePanel.Visibility = Visibility.Visible;
                    }

                    var dateType = DateFormatTypes.DateOnly;
                    var dbDateType = DbDateTypes.DateOnly;
                    if (dateField != null)
                    {
                        dbDateType = dateField.DateType;
                    }
                    switch (dbDateType)
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
                    SetupDateControls();
                    break;
                case FieldDataTypes.Bool:
                    if (CheckCondition())
                    {
                        SearchForBoolComboBoxControl.Visibility = Visibility.Visible;
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public string GetSearchForValue()
        {
            if (SearchValueHost != null && SearchForValuePanel.Visibility == Visibility.Visible)
            {
                return SearchValueHost.SearchText;
            }

            return string.Empty;
        }

        public bool SearchForHostExists()
        {
            return SearchForValuePanel.Visibility == Visibility.Visible && SearchValueHost != null;
        }

        public void SetSearchForValue(string value)
        {
            if (SearchValueHost != null)
            {
                SearchValueHost.SetValue(value);
            }
        }
    }
}
