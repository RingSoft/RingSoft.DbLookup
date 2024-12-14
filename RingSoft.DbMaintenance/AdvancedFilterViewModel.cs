// ***********************************************************************
// Assembly         : RingSoft.DbMaintenance
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 05-26-2024
// ***********************************************************************
// <copyright file="AdvancedFilterViewModel.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DbLookup.TableProcessing;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace RingSoft.DbMaintenance
{
    /// <summary>
    /// Enum TrueFalseValues
    /// </summary>
    public enum TrueFalseValues
    {
        /// <summary>
        /// The true
        /// </summary>
        True = 1,
        /// <summary>
        /// The false
        /// </summary>
        False = 0
    }

    /// <summary>
    /// Enum ValidationFailControls
    /// </summary>
    public enum ValidationFailControls
    {
        /// <summary>
        /// The condition
        /// </summary>
        Condition = 1,
        /// <summary>
        /// The search value
        /// </summary>
        SearchValue = 2,
        /// <summary>
        /// The formula
        /// </summary>
        Formula = 3,
        /// <summary>
        /// The formula display value
        /// </summary>
        FormulaDisplayValue = 4,
    }

    /// <summary>
    /// Class ValidationFailArgs.
    /// </summary>
    public class ValidationFailArgs
    {
        /// <summary>
        /// Gets or sets the control.
        /// </summary>
        /// <value>The control.</value>
        public ValidationFailControls Control { get; set; }
    }

    public interface IAdvFilterView
    {
        string GetSearchForValue();

        bool SearchForHostExists();

        void SetSearchForValue(string value);
    }
    /// <summary>
    /// Class AdvancedFilterViewModel.
    /// Implements the <see cref="INotifyPropertyChanged" />
    /// </summary>
    /// <seealso cref="INotifyPropertyChanged" />
    public class AdvancedFilterViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// The table
        /// </summary>
        private string _table;

        /// <summary>
        /// Gets or sets the table.
        /// </summary>
        /// <value>The table.</value>
        public string Table
        {
            get => _table;
            set
            {
                if (_table == value)
                {
                    return;
                }
                _table = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The field
        /// </summary>
        private string _field;

        /// <summary>
        /// Gets or sets the field.
        /// </summary>
        /// <value>The field.</value>
        public string Field
        {
            get => _field;
            set
            {
                if (_field == value)
                {
                    return;
                }
                _field = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The formula
        /// </summary>
        private string _formula;

        /// <summary>
        /// Gets or sets the formula.
        /// </summary>
        /// <value>The formula.</value>
        public string Formula
        {
            get => _formula;
            set
            {
                if (_formula == value)
                {
                    return;
                }
                _formula = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The formula display value
        /// </summary>
        private string _formulaDisplayValue;

        /// <summary>
        /// Gets or sets the formula display value.
        /// </summary>
        /// <value>The formula display value.</value>
        public string FormulaDisplayValue
        {
            get => _formulaDisplayValue;
            set
            {
                if (_formulaDisplayValue == value)
                {
                    return;
                }
                _formulaDisplayValue = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The condition ComboBox setup
        /// </summary>
        private TextComboBoxControlSetup _conditionComboBoxSetup;

        /// <summary>
        /// Gets or sets the condition ComboBox setup.
        /// </summary>
        /// <value>The condition ComboBox setup.</value>
        public TextComboBoxControlSetup ConditionComboBoxSetup
        {
            get => _conditionComboBoxSetup;
            set
            {
                if (_conditionComboBoxSetup == value)
                {
                    return;
                }
                _conditionComboBoxSetup = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The condition ComboBox item
        /// </summary>
        private TextComboBoxItem _conditionComboBoxItem;

        /// <summary>
        /// Gets or sets the condition ComboBox item.
        /// </summary>
        /// <value>The condition ComboBox item.</value>
        public TextComboBoxItem ConditionComboBoxItem
        {
            get => _conditionComboBoxItem;
            set
            {
                if (_conditionComboBoxItem == value)
                {
                    return;
                }
                _conditionComboBoxItem = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the condition.
        /// </summary>
        /// <value>The condition.</value>
        public Conditions? Condition
        {
            get
            {
                if (ConditionComboBoxItem == null)
                {
                    return null;
                }
                return (Conditions)ConditionComboBoxItem.NumericValue;
            }
            set => ConditionComboBoxItem = ConditionComboBoxSetup.GetItem((int)value);
        }

        /// <summary>
        /// The formula value ComboBox setup
        /// </summary>
        private TextComboBoxControlSetup _formulaValueComboBoxSetup;

        /// <summary>
        /// Gets or sets the formula value ComboBox setup.
        /// </summary>
        /// <value>The formula value ComboBox setup.</value>
        public TextComboBoxControlSetup FormulaValueComboBoxSetup
        {
            get => _formulaValueComboBoxSetup;
            set
            {
                if (_formulaValueComboBoxSetup == value)
                {
                    return;
                }
                _formulaValueComboBoxSetup = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The formula value ComboBox item
        /// </summary>
        private TextComboBoxItem _formulaValueComboBoxItem;

        /// <summary>
        /// Gets or sets the formula value ComboBox item.
        /// </summary>
        /// <value>The formula value ComboBox item.</value>
        public TextComboBoxItem FormulaValueComboBoxItem
        {
            get => _formulaValueComboBoxItem;
            set
            {
                if (_formulaValueComboBoxItem == value)
                {
                    return;
                }
                _formulaValueComboBoxItem = value;
                OnPropertyChanged();
                SetupConditionForFormula();
            }
        }

        /// <summary>
        /// Gets or sets the type of the formula value.
        /// </summary>
        /// <value>The type of the formula value.</value>
        public FieldDataTypes FormulaValueType
        {
            get => (FieldDataTypes)FormulaValueComboBoxItem.NumericValue;
            set => FormulaValueComboBoxItem = FormulaValueComboBoxSetup.GetItem((int)value);
        }

        /// <summary>
        /// The string search value
        /// </summary>
        private string _stringSearchValue;

        /// <summary>
        /// Gets or sets the string search value.
        /// </summary>
        /// <value>The string search value.</value>
        public string StringSearchValue
        {
            get => _stringSearchValue;
            set
            {
                if (_stringSearchValue == value)
                {
                    return;
                }
                _stringSearchValue = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The search value automatic fill setup
        /// </summary>
        private AutoFillSetup _searchValueAutoFillSetup;

        /// <summary>
        /// Gets or sets the search value automatic fill setup.
        /// </summary>
        /// <value>The search value automatic fill setup.</value>
        public AutoFillSetup SearchValueAutoFillSetup
        {
            get => _searchValueAutoFillSetup;
            set
            {
                if (_searchValueAutoFillSetup == value)
                {
                    return;
                }
                _searchValueAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The search value automatic fill value
        /// </summary>
        private AutoFillValue _searchValueAutoFillValue;

        /// <summary>
        /// Gets or sets the search value automatic fill value.
        /// </summary>
        /// <value>The search value automatic fill value.</value>
        public AutoFillValue SearchValueAutoFillValue
        {
            get => _searchValueAutoFillValue;
            set
            {
                if (_searchValueAutoFillValue == value)
                {
                    return;
                }
                _searchValueAutoFillValue = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The decimal search value decimal
        /// </summary>
        private double _decimalSearchValueDecimal;

        /// <summary>
        /// Gets or sets the decimal search value decimal.
        /// </summary>
        /// <value>The decimal search value decimal.</value>
        public double DecimalSearchValueDecimal
        {
            get => _decimalSearchValueDecimal;
            set
            {
                if (_decimalSearchValueDecimal == value)
                {
                    return;
                }
                _decimalSearchValueDecimal = value;
                OnPropertyChanged();
            }
        }

        //Peter Ringering - 11/22/2024 12:58:14 PM - E-62
        private DecimalEditControlSetup _decimalValueSetup;

        public DecimalEditControlSetup DecimalValueSetup
        {
            get { return _decimalValueSetup; }
            set
            {
                if (_decimalValueSetup == value)
                    return;

                _decimalValueSetup = value;
                OnPropertyChanged();
            }
        }


        /// <summary>
        /// The integer search value
        /// </summary>
        private int _integerSearchValue;

        /// <summary>
        /// Gets or sets the integer search value.
        /// </summary>
        /// <value>The integer search value.</value>
        public int IntegerSearchValue
        {
            get => _integerSearchValue;
            set
            {
                if (_integerSearchValue == value)
                {
                    return;
                }
                _integerSearchValue = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The date filter type ComboBox control setup
        /// </summary>
        private TextComboBoxControlSetup _dateFilterTypeComboBoxControlSetup;

        /// <summary>
        /// Gets or sets the date filter type ComboBox control setup.
        /// </summary>
        /// <value>The date filter type ComboBox control setup.</value>
        public TextComboBoxControlSetup DateFilterTypeComboBoxControlSetup
        {
            get => _dateFilterTypeComboBoxControlSetup;
            set
            {
                if (_dateFilterTypeComboBoxControlSetup == value)
                    return;

                _dateFilterTypeComboBoxControlSetup = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The date filter type ComboBox item
        /// </summary>
        private TextComboBoxItem _dateFilterTypeComboBoxItem;

        /// <summary>
        /// Gets or sets the date filter type ComboBox item.
        /// </summary>
        /// <value>The date filter type ComboBox item.</value>
        public TextComboBoxItem DateFilterTypeComboBoxItem
        {
            get => _dateFilterTypeComboBoxItem;
            set
            {
                if (_dateFilterTypeComboBoxItem == value)
                    return;

                _dateFilterTypeComboBoxItem = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the type of the date filter.
        /// </summary>
        /// <value>The type of the date filter.</value>
        public DateFilterTypes DateFilterType
        {
            get => (DateFilterTypes)DateFilterTypeComboBoxItem.NumericValue;
            set => DateFilterTypeComboBoxItem = DateFilterTypeComboBoxControlSetup.GetItem((int)value);
        }

        /// <summary>
        /// The date search value
        /// </summary>
        private DateTime? _dateSearchValue;

        /// <summary>
        /// Gets or sets the date search value.
        /// </summary>
        /// <value>The date search value.</value>
        public DateTime? DateSearchValue
        {
            get => _dateSearchValue;
            set
            {
                if (_dateSearchValue == value)
                {
                    return;
                }
                _dateSearchValue = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The date value setup
        /// </summary>
        private IntegerEditControlSetup _dateValueSetup;

        /// <summary>
        /// Gets or sets the date value setup.
        /// </summary>
        /// <value>The date value setup.</value>
        public IntegerEditControlSetup DateValueSetup
        {
            get => _dateValueSetup;
            set
            {
                if (_dateValueSetup == value)
                {
                    return;
                }
                _dateValueSetup = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The date value
        /// </summary>
        private int _dateValue;

        /// <summary>
        /// Gets or sets the date value.
        /// </summary>
        /// <value>The date value.</value>
        public int DateValue
        {
            get => _dateValue;
            set
            {
                if (_dateValue == value)
                {
                    return;
                }
                _dateValue = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The value ComboBox setup
        /// </summary>
        private TextComboBoxControlSetup _valueComboBoxSetup;

        /// <summary>
        /// Gets or sets the value ComboBox setup.
        /// </summary>
        /// <value>The value ComboBox setup.</value>
        public TextComboBoxControlSetup ValueComboBoxSetup
        {
            get => _valueComboBoxSetup;
            set
            {
                if (_valueComboBoxSetup == value)
                {
                    return;
                }
                _valueComboBoxSetup = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The value ComboBox item
        /// </summary>
        private TextComboBoxItem  _valueComboBoxItem;

        /// <summary>
        /// Gets or sets the value ComboBox item.
        /// </summary>
        /// <value>The value ComboBox item.</value>
        public TextComboBoxItem  ValueComboBoxItem
        {
            get => _valueComboBoxItem;
            set
            {
                if (_valueComboBoxItem == value)
                {
                    return;
                }
                _valueComboBoxItem = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [true false value].
        /// </summary>
        /// <value><c>true</c> if [true false value]; otherwise, <c>false</c>.</value>
        bool TrueFalseValue
        {
            get => ValueComboBoxItem.NumericValue == (int)TrueFalseValues.True;
            set => ValueComboBoxItem = ValueComboBoxSetup.GetItem(value == true ? 1 : 0);
        }

        //public TreeViewItem TreeViewItem { get; set; }
        /// <summary>
        /// Gets or sets the filter return.
        /// </summary>
        /// <value>The filter return.</value>
        public AdvancedFilterReturn FilterReturn { get; set; }
        /// <summary>
        /// Gets or sets the lookup definition.
        /// </summary>
        /// <value>The lookup definition.</value>
        public LookupDefinitionBase LookupDefinition { get; set; }
        /// <summary>
        /// Gets or sets the field definition.
        /// </summary>
        /// <value>The field definition.</value>
        public FieldDefinition FieldDefinition { get; set; }
        /// <summary>
        /// Gets or sets the parent field definition.
        /// </summary>
        /// <value>The parent field definition.</value>
        public FieldDefinition ParentFieldDefinition { get; set; }
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public TreeViewType Type { get; set; }
        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        /// <value>The path.</value>
        public string Path { get; set; }

        public IAdvFilterView View { get; private set; }

        /// <summary>
        /// Occurs when [on validation fail].
        /// </summary>
        public event EventHandler <ValidationFailArgs> OnValidationFail;

        /// <summary>
        /// The string field ComboBox control setup
        /// </summary>
        private TextComboBoxControlSetup _stringFieldComboBoxControlSetup = new TextComboBoxControlSetup();
        /// <summary>
        /// The numeric field ComboBox control setup
        /// </summary>
        private TextComboBoxControlSetup _numericFieldComboBoxControlSetup = new TextComboBoxControlSetup();
        /// <summary>
        /// The memo field ComboBox control setup
        /// </summary>
        private TextComboBoxControlSetup _memoFieldComboBoxControlSetup = new TextComboBoxControlSetup();
        /// <summary>
        /// The enum field ComboBox control setup
        /// </summary>
        private TextComboBoxControlSetup _enumFieldComboBoxControlSetup = new TextComboBoxControlSetup();

        /// <summary>
        /// The form add
        /// </summary>
        private bool _formAdd;

        /// <summary>
        /// Initializes the specified filter return.
        /// </summary>
        /// <param name="filterReturn">The filter return.</param>
        /// <param name="lookupDefinition">The lookup definition.</param>
        public void Initialize(IAdvFilterView view, AdvancedFilterReturn filterReturn, LookupDefinitionBase lookupDefinition)
        {
            View = view;
            Path = filterReturn.Path;
            FilterReturn = filterReturn;
            FieldDefinition = filterReturn.FieldDefinition;
            if (filterReturn.Formula.IsNullOrEmpty())
            {
                Table = filterReturn.FieldDefinition.TableDefinition.Description;
                Field = filterReturn.FieldDefinition.Description;

                Type = TreeViewType.Field;
            }
            else
            {
                Type = TreeViewType.Formula;
            }

            Initialize(lookupDefinition);
            //Condition = filterReturn.Condition;
            
            if (filterReturn.PrimaryFieldDefinition != null)
            {
                Table = filterReturn.PrimaryFieldDefinition.Description;
                ParentFieldDefinition = filterReturn.PrimaryFieldDefinition;
            }
            else if (Type == TreeViewType.Formula)
            {
                Table = lookupDefinition.TableDefinition.Description;
            }
        }

        /// <summary>
        /// Loads the window.
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public void LoadWindow()
        {
            if (_formAdd)
            {
                DateFilterType = DateFilterTypes.SpecificDate;
                return;
            }

            FieldDataTypes fieldDataType = FieldDataTypes.String;
            switch (Type)
            {
                case TreeViewType.Field:
                    fieldDataType = FieldDefinition.FieldDataType;
                    break;
                case TreeViewType.Formula:
                    Formula = FilterReturn.Formula;
                    FormulaValueType = fieldDataType = FilterReturn.FormulaValueType;
                    FormulaDisplayValue = FilterReturn.FormulaDisplayValue;
                    ProcessDateReturn();

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Condition = FilterReturn.Condition;
            if (FieldDefinition != null && FieldDefinition.ParentJoinForeignKeyDefinition != null)
            {
                var process = false;
                var useLookup = true;
                if (FieldDefinition.ParentJoinForeignKeyDefinition.FieldJoins.Count == 1)
                {
                    switch (Condition)
                    {
                        case Conditions.Equals:
                        case Conditions.NotEquals:
                            process = true;
                            break;
                    }
                }
                else
                {
                    useLookup = false;
                }

                //if (useLookup)
                {
                    if (process)
                    {
                        SearchValueAutoFillValue =
                            LookupDefinition.TableDefinition.Context.OnAutoFillTextRequest(
                                FieldDefinition.ParentJoinForeignKeyDefinition.PrimaryTable, FilterReturn.SearchValue);
                    }
                    else
                    {
                        SearchValueAutoFillValue =
                            new AutoFillValue(
                                new PrimaryKeyValue(FieldDefinition.ParentJoinForeignKeyDefinition.PrimaryTable),
                                FilterReturn.SearchValue);
                    }
                }
            }

            if (View.SearchForHostExists())
            {
                View.SetSearchForValue(FilterReturn.SearchValue);
            }
            else
            {
                switch (fieldDataType)
                {
                    case FieldDataTypes.String:
                    case FieldDataTypes.Memo:
                        StringSearchValue = FilterReturn.SearchValue;
                        break;
                    case FieldDataTypes.Integer:
                        var integerField = FieldDefinition as IntegerFieldDefinition;
                        if (integerField != null)
                        {
                            if (integerField.EnumTranslation != null)
                            {
                                var setup = new TextComboBoxControlSetup();
                                setup.LoadFromEnum(integerField.EnumTranslation);
                                ValueComboBoxItem = null;
                                ValueComboBoxSetup = setup;
                                ValueComboBoxItem = ValueComboBoxSetup.GetItem(FilterReturn.SearchValue.ToInt());
                            }
                            else
                            {
                                IntegerSearchValue = FilterReturn.SearchValue.ToInt();
                            }
                        }
                        else
                        {
                            IntegerSearchValue = FilterReturn.SearchValue.ToInt();
                        }

                        break;
                    case FieldDataTypes.Decimal:
                        DecimalSearchValueDecimal = FilterReturn.SearchValue.ToDecimal();
                        break;
                    case FieldDataTypes.DateTime:
                        ProcessDateReturn();

                        break;
                    case FieldDataTypes.Bool:
                        TrueFalseValue = FilterReturn.SearchValue.ToBool();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <summary>
        /// Processes the date return.
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        private void ProcessDateReturn()
        {
            var process = false;
            switch (Type)
            {
                case TreeViewType.Field:
                    if (FieldDefinition is DateFieldDefinition)
                    {
                        process = true;
                    }
                    break;
                case TreeViewType.AdvancedFind:
                    break;
                case TreeViewType.Formula:
                    if (FormulaValueType == FieldDataTypes.DateTime)
                    {
                        process = true;
                    }
                    break;
                case TreeViewType.ForeignTable:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            if (!process)
            {
                return;
            }
            DateFilterType = FilterReturn.DateFilterType;
            DateTime searchDateTime = DateTime.MinValue;
            switch (FilterReturn.DateFilterType)
            {
                case DateFilterTypes.SpecificDate:
                    DateTime.TryParse(FilterReturn.SearchValue, out searchDateTime);
                    DateSearchValue = searchDateTime;
                    break;
                default:
                    DateValue = FilterReturn.SearchValue.ToInt();
                    break;
            }
        }

        /// <summary>
        /// Initializes the specified tree view item.
        /// </summary>
        /// <param name="treeViewItem">The tree view item.</param>
        /// <param name="lookupDefinition">The lookup definition.</param>
        public void Initialize(IAdvFilterView view, TreeViewItem treeViewItem, LookupDefinitionBase lookupDefinition)
        {
            View = view;
            Path = treeViewItem.MakePath();
            Type = treeViewItem.Type;
            FieldDefinition = treeViewItem.FieldDefinition;
            Initialize(lookupDefinition);
            _formAdd = true;
            
            if (treeViewItem.Parent == null)
            {
                Table = LookupDefinition.TableDefinition.Description;
            }
            else
            {
                Table = treeViewItem.Parent.Name;
                ParentFieldDefinition = treeViewItem.Parent.FieldDefinition;
            }
            Field = treeViewItem.Name;

        }

        /// <summary>
        /// Initializes the specified lookup definition.
        /// </summary>
        /// <param name="lookupDefinition">The lookup definition.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        private void Initialize( LookupDefinitionBase lookupDefinition)
        {
            LookupDefinition = lookupDefinition;
            switch (Type)
            {
                case TreeViewType.Field:
                    Field = FieldDefinition.Description;
                    break;
                case TreeViewType.Formula:
                    Field = "<Formula>";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            FormulaValueComboBoxSetup = new TextComboBoxControlSetup();
            DateSearchValue = null;
            ValueComboBoxSetup = new TextComboBoxControlSetup();
            //ValueComboBoxSetup.LoadFromEnum<TrueFalseValues>();
            
            DateFilterTypeComboBoxControlSetup = new TextComboBoxControlSetup();
            DateFilterTypeComboBoxControlSetup.LoadFromEnum<DateFilterTypes>();

            FormulaValueComboBoxSetup.LoadFromEnum<FieldDataTypes>();
            FormulaValueType = FieldDataTypes.String;

            _stringFieldComboBoxControlSetup.LoadFromEnum<Conditions>();

            _enumFieldComboBoxControlSetup.LoadFromEnum<Conditions>();

            _enumFieldComboBoxControlSetup.Items.Remove(
                _enumFieldComboBoxControlSetup.Items.FirstOrDefault(p =>
                    p.NumericValue == (int)Conditions.Contains));

            _enumFieldComboBoxControlSetup.Items.Remove(
                _enumFieldComboBoxControlSetup.Items.FirstOrDefault(p =>
                    p.NumericValue == (int)Conditions.NotContains));

            _enumFieldComboBoxControlSetup.Items.Remove(
                _enumFieldComboBoxControlSetup.Items.FirstOrDefault(p =>
                    p.NumericValue == (int)Conditions.GreaterThan));

            _enumFieldComboBoxControlSetup.Items.Remove(
                _enumFieldComboBoxControlSetup.Items.FirstOrDefault(p =>
                    p.NumericValue == (int)Conditions.GreaterThanEquals));

            _enumFieldComboBoxControlSetup.Items.Remove(
                _enumFieldComboBoxControlSetup.Items.FirstOrDefault(p =>
                    p.NumericValue == (int)Conditions.LessThan));

            _enumFieldComboBoxControlSetup.Items.Remove(
                _enumFieldComboBoxControlSetup.Items.FirstOrDefault(p =>
                    p.NumericValue == (int)Conditions.LessThanEquals));

            _enumFieldComboBoxControlSetup.Items.Remove(
                _enumFieldComboBoxControlSetup.Items.FirstOrDefault(p =>
                    p.NumericValue == (int)Conditions.BeginsWith));

            _enumFieldComboBoxControlSetup.Items.Remove(
                _enumFieldComboBoxControlSetup.Items.FirstOrDefault(p =>
                    p.NumericValue == (int)Conditions.EndsWith));

            _numericFieldComboBoxControlSetup.LoadFromEnum<Conditions>();

            _numericFieldComboBoxControlSetup.Items.Remove(
                _numericFieldComboBoxControlSetup.Items.FirstOrDefault(p =>
                    p.NumericValue == (int) Conditions.Contains));

            _numericFieldComboBoxControlSetup.Items.Remove(
                _numericFieldComboBoxControlSetup.Items.FirstOrDefault(p =>
                    p.NumericValue == (int) Conditions.BeginsWith));

            _numericFieldComboBoxControlSetup.Items.Remove(
                _numericFieldComboBoxControlSetup.Items.FirstOrDefault(p =>
                    p.NumericValue == (int) Conditions.EndsWith));

            _numericFieldComboBoxControlSetup.Items.Remove(
                _numericFieldComboBoxControlSetup.Items.FirstOrDefault(p =>
                    p.NumericValue == (int) Conditions.NotContains));

            _memoFieldComboBoxControlSetup.LoadFromEnum<Conditions>();
            _memoFieldComboBoxControlSetup.Items.Remove(
                _memoFieldComboBoxControlSetup.Items.FirstOrDefault(p =>
                    p.NumericValue == (int) Conditions.Equals));

            _memoFieldComboBoxControlSetup.Items.Remove(
                _memoFieldComboBoxControlSetup.Items.FirstOrDefault(p =>
                    p.NumericValue == (int) Conditions.NotEquals));

            _memoFieldComboBoxControlSetup.Items.Remove(
                _memoFieldComboBoxControlSetup.Items.FirstOrDefault(p =>
                    p.NumericValue == (int) Conditions.GreaterThan));

            _memoFieldComboBoxControlSetup.Items.Remove(
                _memoFieldComboBoxControlSetup.Items.FirstOrDefault(p =>
                    p.NumericValue == (int) Conditions.GreaterThanEquals));

            _memoFieldComboBoxControlSetup.Items.Remove(
                _memoFieldComboBoxControlSetup.Items.FirstOrDefault(p =>
                    p.NumericValue == (int) Conditions.LessThan));

            _memoFieldComboBoxControlSetup.Items.Remove(
                _memoFieldComboBoxControlSetup.Items.FirstOrDefault(p =>
                    p.NumericValue == (int) Conditions.LessThanEquals));

            _memoFieldComboBoxControlSetup.Items.Remove(
                _memoFieldComboBoxControlSetup.Items.FirstOrDefault(p =>
                    p.NumericValue == (int) Conditions.BeginsWith));

            _memoFieldComboBoxControlSetup.Items.Remove(
                _memoFieldComboBoxControlSetup.Items.FirstOrDefault(p =>
                    p.NumericValue == (int) Conditions.EndsWith));

            switch (Type)
            {
                case TreeViewType.Field:
                    if (FieldDefinition.ParentJoinForeignKeyDefinition != null)
                    {
                        SearchValueAutoFillSetup = new AutoFillSetup(FieldDefinition
                            .ParentJoinForeignKeyDefinition.PrimaryTable.LookupDefinition);
                        ParentFieldDefinition =
                            FieldDefinition.ParentJoinForeignKeyDefinition.FieldJoins[0].PrimaryField;
                        ConditionComboBoxSetup = _enumFieldComboBoxControlSetup;
                    }
                    switch (FieldDefinition.FieldDataType)
                    {
                        case FieldDataTypes.String:
                            if (FieldDefinition is StringFieldDefinition stringField)
                            {
                                if (stringField.MemoField)
                                {
                                    ConditionComboBoxSetup = _memoFieldComboBoxControlSetup;
                                }
                                else
                                {
                                    ConditionComboBoxSetup = _stringFieldComboBoxControlSetup;
                                }
                            }
                            break;
                        case FieldDataTypes.Integer:
                            if (FieldDefinition.ParentJoinForeignKeyDefinition != null)
                            {
                                ConditionComboBoxSetup = _enumFieldComboBoxControlSetup;
                            }
                            else
                            {
                                ConditionComboBoxSetup = _numericFieldComboBoxControlSetup;
                            }
                            //if (FieldDefinition != null && FieldDefinition.ParentJoinForeignKeyDefinition != null)
                            //{
                            //    SearchValueAutoFillValue =
                            //        LookupDefinition.TableDefinition.Context.OnAutoFillTextRequest(
                            //            FieldDefinition.ParentJoinForeignKeyDefinition.PrimaryTable, FilterReturn.SearchValue);
                            //}
                            if (FieldDefinition is IntegerFieldDefinition integerField)
                            {
                                if (integerField.EnumTranslation != null)
                                {
                                    var setup = new TextComboBoxControlSetup();
                                    setup.LoadFromEnum(integerField.EnumTranslation);
                                    ValueComboBoxItem = null;
                                    ValueComboBoxSetup = setup;
                                }
                            }
                            break;
                        //Peter Ringering - 11/22/2024 01:11:55 PM - E-62
                        case FieldDataTypes.Decimal:
                            ConditionComboBoxSetup = _numericFieldComboBoxControlSetup;
                            if (FieldDefinition is DecimalFieldDefinition decimalField)
                            {
                                var decimalValueSetup = new DecimalEditControlSetup();
                                switch (decimalField.DecimalFieldType)
                                {
                                    case DecimalFieldTypes.Decimal:
                                        decimalValueSetup.FormatType = DecimalEditFormatTypes.Number;
                                        break;
                                    case DecimalFieldTypes.Currency:
                                        decimalValueSetup.FormatType = DecimalEditFormatTypes.Currency;
                                        break;
                                    case DecimalFieldTypes.Percent:
                                        decimalValueSetup.FormatType = DecimalEditFormatTypes.Percent;
                                        break;
                                    default:
                                        throw new ArgumentOutOfRangeException();
                                }
                                DecimalValueSetup = decimalValueSetup;
                            }
                            break;
                        case FieldDataTypes.DateTime:
                            ConditionComboBoxSetup = _numericFieldComboBoxControlSetup;
                            break;
                        case FieldDataTypes.Bool:
                            ConditionComboBoxSetup = _enumFieldComboBoxControlSetup;
                            if (FieldDefinition is BoolFieldDefinition boolField)
                            {
                                var setup = new TextComboBoxControlSetup();
                                setup.LoadFromEnum(boolField.EnumField);
                                ValueComboBoxItem = null;
                                ValueComboBoxSetup = setup;
                            }
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    if (!FieldDefinition.AllowNulls)
                    {
                        var item = ConditionComboBoxSetup.GetItem((int)Conditions.EqualsNull);
                        ConditionComboBoxSetup.Items.Remove(item);
                        item = ConditionComboBoxSetup.GetItem((int)Conditions.NotEqualsNull);
                        ConditionComboBoxSetup.Items.Remove(item);
                    }
                    break;
                case TreeViewType.Formula:
                    ConditionComboBoxSetup = _stringFieldComboBoxControlSetup;
                    FormulaValueType = FieldDataTypes.String;
                    if (FormulaValueComboBoxItem != null)
                    {
                        SetupConditionForFormula();
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }

        /// <summary>
        /// Setups the condition for formula.
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        private void SetupConditionForFormula()
        {
            switch (FormulaValueType)
            {
                case FieldDataTypes.String:
                    ConditionComboBoxSetup = _stringFieldComboBoxControlSetup;
                    break;
                case FieldDataTypes.Decimal:
                case FieldDataTypes.Integer:
                case FieldDataTypes.DateTime:
                case FieldDataTypes.Bool:
                    ConditionComboBoxSetup = _numericFieldComboBoxControlSetup;
                    break;
                case FieldDataTypes.Memo:
                    ConditionComboBoxSetup = _memoFieldComboBoxControlSetup;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Gets the advanced filter return.
        /// </summary>
        /// <returns>AdvancedFilterReturn.</returns>
        public AdvancedFilterReturn GetAdvancedFilterReturn()
        {
            var result = new AdvancedFilterReturn();
            result.Path = Path;
            if (Condition.HasValue)
            {
                result.Condition = Condition.Value;
            }

            var filterType = Type;
            result.LookupDefinition = LookupDefinition;
            result.TableDescription = Table;
            GetFilterReturnProperties(filterType, FieldDefinition, result);

            return result;
        }

        /// <summary>
        /// Gets the filter return properties.
        /// </summary>
        /// <param name="filterType">Type of the filter.</param>
        /// <param name="fieldDefinition">The field definition.</param>
        /// <param name="result">The result.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        private void GetFilterReturnProperties(TreeViewType filterType, FieldDefinition fieldDefinition,
            AdvancedFilterReturn result)
        {
            switch (filterType)
            {
                case TreeViewType.Field:
                    result.FieldDefinition = fieldDefinition;
                    {
                        if (fieldDefinition.ParentJoinForeignKeyDefinition != null)
                        {
                            GetAutoFillValueResult(result);
                        }
                        else
                        {
                            var fieldDataType = fieldDefinition.FieldDataType;
                            GetSearchValue(fieldDataType, result);
                        }
                    }
                    if (ParentFieldDefinition != null)
                    {
                        result.PrimaryFieldDefinition = ParentFieldDefinition;
                    }
                    break;
                case TreeViewType.Formula:
                    //result.FormulaParentFieldDefinition = TreeViewItem.Parent.FieldDefinition;
                    GetSearchValue(FormulaValueType, result);
                    if (ParentFieldDefinition != null)
                    {
                        result.FieldDefinition = ParentFieldDefinition;
                        result.PrimaryFieldDefinition = ParentFieldDefinition;
                    }

                    result.Formula = Formula;
                    result.FormulaDisplayValue = FormulaDisplayValue;
                    result.FormulaValueType = FormulaValueType;

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        /// <summary>
        /// Gets the search value.
        /// </summary>
        /// <param name="fieldDataType">Type of the field data.</param>
        /// <param name="result">The result.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        private void GetSearchValue(FieldDataTypes fieldDataType, AdvancedFilterReturn result)
        {
            if (View.SearchForHostExists())
            {
                result.SearchValue = View.GetSearchForValue();
                return;
            }
            switch (fieldDataType)
            {
                case FieldDataTypes.String:
                    result.SearchValue = StringSearchValue;
                    break;
                case FieldDataTypes.Integer:
                    if (FieldDefinition is IntegerFieldDefinition integerField)
                    {
                        if (integerField.EnumTranslation != null)
                        {
                            result.SearchValue = ValueComboBoxItem.NumericValue.ToString();
                        }
                        else
                        {
                            result.SearchValue = IntegerSearchValue.ToString();
                        }
                    }
                    else if(!Formula.IsNullOrEmpty())
                    {
                        result.SearchValue = IntegerSearchValue.ToString();
                    }
                    break;
                case FieldDataTypes.Decimal:
                    result.SearchValue = DecimalSearchValueDecimal.ToString();
                    break;
                case FieldDataTypes.DateTime:
                    SetDateProperties(result);
                    break;
                case FieldDataTypes.Bool:
                    result.SearchValue = ValueComboBoxItem.NumericValue.ToString();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Sets the date properties.
        /// </summary>
        /// <param name="result">The result.</param>
        private void SetDateProperties(AdvancedFilterReturn result)
        {
            result.DateFilterType = DateFilterType;
            switch (DateFilterType)
            {
                case DateFilterTypes.SpecificDate:
                    result.SearchValue = DateSearchValue.ToString();
                    break;
                default:
                    result.SearchValue = DateValue.ToString();
                    break;
            }
        }

        /// <summary>
        /// Gets the automatic fill value result.
        /// </summary>
        /// <param name="result">The result.</param>
        private void GetAutoFillValueResult(AdvancedFilterReturn result)
        {
            if (SearchValueAutoFillValue.IsValid())
            {
                switch (Condition)
                {
                    case Conditions.Equals:
                    case Conditions.NotEquals:
                        if (!SearchValueAutoFillValue.IsValid())
                        {
                            var tableDefinition = SearchValueAutoFillValue.PrimaryKeyValue.TableDefinition;
                            var query = LookupDefinition.GetSelectQueryMaui();
                            foreach (var primaryKeyField in tableDefinition.PrimaryKeyFields)
                            {
                                query.AddColumn(primaryKeyField);
                            }

                            if (tableDefinition.LookupDefinition.InitialSortColumnDefinition is
                                LookupFieldColumnDefinition lookupFieldColumn)
                            {
                                query.AddFilter(lookupFieldColumn, Conditions.Equals, SearchValueAutoFillValue.Text);

                            }

                            var queryResult = query.GetData();
                            if (queryResult)
                            {
                                SearchValueAutoFillValue
                                    .PrimaryKeyValue
                                    .LoadFromIdValue(query.GetPropertyValue
                                        (0, tableDefinition.PrimaryKeyFields[0].PropertyName));
                            }
                        }
                        result.SearchValue = SearchValueAutoFillValue.PrimaryKeyValue.KeyValueFields[0].Value;
                        break;
                    default:
                        result.SearchValue = SearchValueAutoFillValue.Text;
                        break;
                }
            }
        }

        /// <summary>
        /// Validates the specified filter return.
        /// </summary>
        /// <param name="filterReturn">The filter return.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool Validate(AdvancedFilterReturn filterReturn)
        {
            if (Type == TreeViewType.Formula)
            {
                if (Formula.IsNullOrEmpty())
                {
                    var message = "Formula cannot be empty.";
                    var caption = "Invalid Formula";
                    ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
                    OnValidationFail?.Invoke(this, new ValidationFailArgs() {Control = ValidationFailControls.Formula});
                    return false;
                }

                if (FormulaDisplayValue.IsNullOrEmpty())
                {
                    var message = "Display Value cannot be empty.";
                    var caption = "Invalid Display Value";
                    ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
                    OnValidationFail?.Invoke(this,
                        new ValidationFailArgs() {Control = ValidationFailControls.FormulaDisplayValue});
                    return false;
                }
            }

            if (Condition == null)
            {
                var message = "You must select a condition.";
                var caption = "Invalid Condition";
                ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
                OnValidationFail?.Invoke(this, new ValidationFailArgs() { Control = ValidationFailControls.Condition });
                return false;
            }

            if (Type == TreeViewType.Field && FieldDefinition.ParentJoinForeignKeyDefinition != null
                                           && !SearchValueAutoFillValue.IsValid())
            {
                switch (Condition)
                {
                    case Conditions.Equals:
                    case Conditions.NotEquals:
                        var message = "You must select a valid search value.";
                        var caption = "Invalid Search Value";
                        ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
                        OnValidationFail?.Invoke(this, new ValidationFailArgs() { Control = ValidationFailControls.SearchValue });
                        return false;
                }

            }


            if (filterReturn.SearchValue.IsNullOrEmpty())
            {
                var result = false;
                switch (filterReturn.Condition)
                {
                    case Conditions.EqualsNull:
                    case Conditions.NotEqualsNull:
                        result = true;
                        break;
                    //default:
                    //    throw new ArgumentOutOfRangeException();
                }

                if (!result)
                {
                    var message = "Search Value cannot be empty.";
                    var caption = "Invalid Search Value";
                    ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
                    OnValidationFail?.Invoke(this,
                        new ValidationFailArgs() {Control = ValidationFailControls.SearchValue});
                    return false;
                }
            }

            return true;
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
    }
}
