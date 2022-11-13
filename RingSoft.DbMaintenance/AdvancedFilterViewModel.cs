using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.QueryBuilder;

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
    public class AdvancedFilterReturn
    {
        public FieldDefinition FieldDefinition { get; set; }
        public string PrimaryTableName { get; set; }
        public FieldDefinition PrimaryFieldDefinition { get; set; }
        public Conditions Condition { get; set; }
        public string SearchValue { get; set; }
        public string Formula { get; set; }
        public string FormulaDisplayValue { get; set; }
        public FieldDataTypes FormulaValueType { get; set; }
        public LookupDefinitionBase LookupDefinition { get; set; }
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

        public event EventHandler <ValidationFailArgs> OnValidationFail;

        private TextComboBoxControlSetup _stringFieldComboBoxControlSetup = new TextComboBoxControlSetup();
        private TextComboBoxControlSetup _numericFieldComboBoxControlSetup = new TextComboBoxControlSetup();
        private TextComboBoxControlSetup _memoFieldComboBoxControlSetup = new TextComboBoxControlSetup();

        private bool _formAdd;

        public void Initialize(AdvancedFilterReturn filterReturn, LookupDefinitionBase lookupDefinition)
        {
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

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Condition = FilterReturn.Condition;
            if (FieldDefinition != null && FieldDefinition.ParentJoinForeignKeyDefinition != null)
            {
                var process = false;
                switch (Condition)
                {
                    case Conditions.Equals:
                    case Conditions.NotEquals:
                        process = true;
                        break;
                }

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


            switch (fieldDataType)
            {
                case FieldDataTypes.String:
                case FieldDataTypes.Memo:
                    StringSearchValue = FilterReturn.SearchValue;
                    break;
                case FieldDataTypes.Integer:
                    if (FieldDefinition is IntegerFieldDefinition integerField)
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
                    break;
                case FieldDataTypes.Decimal:
                    DecimalSearchValueDecimal = FilterReturn.SearchValue.ToDecimal();
                    break;
                case FieldDataTypes.DateTime:
                    DateTime searchDateTime = DateTime.MinValue;
                    DateTime.TryParse(FilterReturn.SearchValue, out searchDateTime);
                    DateSearchValue = searchDateTime;
                    break;
                case FieldDataTypes.Bool:
                    TrueFalseValue = FilterReturn.SearchValue.ToBool();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        public void Initialize(TreeViewItem treeViewItem, LookupDefinitionBase lookupDefinition)
        {
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
            if (Condition.HasValue)
            {
                result.Condition = Condition.Value;
            }

            var filterType = Type;
            result.LookupDefinition = LookupDefinition;
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
                    break;
                case FieldDataTypes.Decimal:
                    result.SearchValue = DecimalSearchValueDecimal.ToString();
                    break;
                case FieldDataTypes.DateTime:
                    result.SearchValue = DateSearchValue.ToString();
                    break;
                case FieldDataTypes.Bool:
                    result.SearchValue = ValueComboBoxItem.NumericValue.ToString();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
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

            if (filterReturn.SearchValue.IsNullOrEmpty())
            {
                var result = false;
                switch (filterReturn.Condition)
                {
                    case Conditions.EqualsNull:
                    case Conditions.NotEqualsNull:
                        result = true;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
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
