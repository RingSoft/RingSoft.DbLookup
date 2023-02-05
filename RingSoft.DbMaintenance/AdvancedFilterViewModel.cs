using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using MySqlX.XDevAPI.Common;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DbLookup.TableProcessing;

namespace RingSoft.DbMaintenance
{
    public enum TrueFalseValues
    {
        True = 1,
        False = 0
    }

    public enum ValidationFailControls
    {
        Condition = 1,
        SearchValue = 2,
        Formula = 3,
        FormulaDisplayValue = 4,
    }

    public class ValidationFailArgs
    {
        public ValidationFailControls Control { get; set; }
    }
    public class AdvancedFilterViewModel : INotifyPropertyChanged
    {
        private string _table;

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

        private string _field;

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

        private string _formula;

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

        private string _formulaDisplayValue;

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

        private TextComboBoxControlSetup _conditionComboBoxSetup;

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

        private TextComboBoxItem _conditionComboBoxItem;

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

        private TextComboBoxControlSetup _formulaValueComboBoxSetup;

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

        private TextComboBoxItem _formulaValueComboBoxItem;

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

        public FieldDataTypes FormulaValueType
        {
            get => (FieldDataTypes)FormulaValueComboBoxItem.NumericValue;
            set => FormulaValueComboBoxItem = FormulaValueComboBoxSetup.GetItem((int)value);
        }

        private string _stringSearchValue;

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

        private AutoFillSetup _searchValueAutoFillSetup;

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

        private AutoFillValue _searchValueAutoFillValue;

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

        private decimal _decimalSearchValueDecimal;

        public decimal DecimalSearchValueDecimal
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

        private int _integerSearchValue;

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

        private TextComboBoxControlSetup _dateFilterTypeComboBoxControlSetup;

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

        private TextComboBoxItem _dateFilterTypeComboBoxItem;

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

        public DateFilterTypes DateFilterType
        {
            get => (DateFilterTypes)DateFilterTypeComboBoxItem.NumericValue;
            set => DateFilterTypeComboBoxItem = DateFilterTypeComboBoxControlSetup.GetItem((int)value);
        }

        private DateTime? _dateSearchValue;

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

        private IntegerEditControlSetup _dateValueSetup;

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

        private int _dateValue;

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

        private TextComboBoxControlSetup _valueComboBoxSetup;

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

        private TextComboBoxItem  _valueComboBoxItem;

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

        bool TrueFalseValue
        {
            get => ValueComboBoxItem.NumericValue == (int)TrueFalseValues.True;
            set => ValueComboBoxItem = ValueComboBoxSetup.GetItem(value == true ? 1 : 0);
        }

        //public TreeViewItem TreeViewItem { get; set; }
        public AdvancedFilterReturn FilterReturn { get; set; }
        public LookupDefinitionBase LookupDefinition { get; set; }
        public FieldDefinition FieldDefinition { get; set; }
        public FieldDefinition ParentFieldDefinition { get; set; }
        public TreeViewType Type { get; set; }
        public string Path { get; set; }

        public event EventHandler <ValidationFailArgs> OnValidationFail;

        private TextComboBoxControlSetup _stringFieldComboBoxControlSetup = new TextComboBoxControlSetup();
        private TextComboBoxControlSetup _numericFieldComboBoxControlSetup = new TextComboBoxControlSetup();
        private TextComboBoxControlSetup _memoFieldComboBoxControlSetup = new TextComboBoxControlSetup();

        private bool _formAdd;

        public void Initialize(AdvancedFilterReturn filterReturn, LookupDefinitionBase lookupDefinition)
        {
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

        public void Initialize(TreeViewItem treeViewItem, LookupDefinitionBase lookupDefinition)
        {
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
                                ConditionComboBoxSetup = _stringFieldComboBoxControlSetup;
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
                        case FieldDataTypes.Decimal:
                        case FieldDataTypes.DateTime:
                            ConditionComboBoxSetup = _numericFieldComboBoxControlSetup;
                            break;
                        case FieldDataTypes.Bool:
                            ConditionComboBoxSetup = _numericFieldComboBoxControlSetup;
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


        private void GetSearchValue(FieldDataTypes fieldDataType, AdvancedFilterReturn result)
        {
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

        private void GetAutoFillValueResult(AdvancedFilterReturn result)
        {
            if (SearchValueAutoFillValue != null)
            {
                switch (Condition)
                {
                    case Conditions.Equals:
                    case Conditions.NotEquals:
                        if (!SearchValueAutoFillValue.IsValid())
                        {
                            var tableDefinition = SearchValueAutoFillValue.PrimaryKeyValue.TableDefinition;
                            var query = new SelectQuery(tableDefinition.TableName);
                            foreach (var primaryKeyField in tableDefinition.PrimaryKeyFields)
                            {
                                query.AddSelectColumn(primaryKeyField.FieldName);
                            }

                            if (tableDefinition.LookupDefinition.InitialSortColumnDefinition is
                                LookupFieldColumnDefinition lookupFieldColumn)
                            {
                                query.AddWhereItem(lookupFieldColumn.FieldDefinition.FieldName, Conditions.Equals,
                                    SearchValueAutoFillValue.Text);
                            }
                            else  if (tableDefinition.LookupDefinition.InitialSortColumnDefinition is
                                LookupFormulaColumnDefinition lookupFormulaColumn)
                            {
                                var formula =
                                    lookupFormulaColumn.OriginalFormula.Replace("{Alias}", tableDefinition.TableName);
                                query.AddWhereItemFormula(formula, Conditions.Equals,
                                    SearchValueAutoFillValue.Text);
                            }

                            var queryResult = tableDefinition.Context.DataProcessor.GetData(query);
                            if (queryResult.ResultCode == GetDataResultCodes.Success)
                            {
                                if (queryResult.DataSet.Tables[0].Rows.Count > 0)
                                {
                                    SearchValueAutoFillValue.PrimaryKeyValue.PopulateFromDataRow(queryResult.DataSet
                                        .Tables[0].Rows[0]);
                                }
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
