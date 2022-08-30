using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.QueryBuilder;

namespace RingSoft.DbMaintenance
{
    public class AdvancedFilterReturn
    {
        public FieldDefinition FieldDefinition { get; set; }
        public FieldDefinition FormulaParentFieldDefinition { get; set; }
        public Conditions Condition { get; set; }
        public string SearchValue { get; set; }
        public string Formula { get; set; }
        public string FormulaDisplayValue { get; set; }
        public FieldDataTypes FormulaValueType { get; set; }
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
            }
        }

        public Conditions Condition
        {
            get => (Conditions)ConditionComboBoxItem.NumericValue;
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




        public TreeViewItem TreeViewItem { get; set; }
        public LookupDefinitionBase LookupDefinition { get; set; }

        private TextComboBoxControlSetup _stringFieldComboBoxControlSetup = new TextComboBoxControlSetup();
        private TextComboBoxControlSetup _numericFieldComboBoxControlSetup = new TextComboBoxControlSetup();
        private TextComboBoxControlSetup _memoFieldComboBoxControlSetup = new TextComboBoxControlSetup();

        public void Initialize(TreeViewItem treeViewItem, LookupDefinitionBase lookupDefinition)
        {
            LookupDefinition = lookupDefinition;
            TreeViewItem = treeViewItem;
            FormulaValueComboBoxSetup = new TextComboBoxControlSetup();

            if (treeViewItem.Parent == null)
            {
                Table = LookupDefinition.TableDefinition.Description;
                if (treeViewItem.ParentJoin != null)
                {
                    Field = treeViewItem.ParentJoin.FieldJoins[0].PrimaryField.Description;
                }
                else
                {
                    switch (treeViewItem.Type)
                    {
                        case TreeViewType.Field:
                            Field = treeViewItem.FieldDefinition.Description;
                            break;
                        case TreeViewType.Formula:
                            Field = treeViewItem.Name;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            else
            {
                Table = treeViewItem.Parent.Name;
                Field = treeViewItem.Name;
            }

            FormulaValueComboBoxSetup.LoadFromEnum<FieldDataTypes>();
            FormulaValueType = FieldDataTypes.String;

            _stringFieldComboBoxControlSetup.LoadFromEnum<Conditions>();
            _numericFieldComboBoxControlSetup.LoadFromEnum<Conditions>();
            
            _numericFieldComboBoxControlSetup.Items.Remove(
                _numericFieldComboBoxControlSetup.Items.FirstOrDefault(p =>
                    p.NumericValue == (int) Conditions.Contains));

            _numericFieldComboBoxControlSetup.Items.Remove(
                _numericFieldComboBoxControlSetup.Items.FirstOrDefault(p =>
                    p.NumericValue == (int)Conditions.BeginsWith));

            _numericFieldComboBoxControlSetup.Items.Remove(
                _numericFieldComboBoxControlSetup.Items.FirstOrDefault(p =>
                    p.NumericValue == (int)Conditions.EndsWith));

            _numericFieldComboBoxControlSetup.Items.Remove(
                _numericFieldComboBoxControlSetup.Items.FirstOrDefault(p =>
                    p.NumericValue == (int)Conditions.NotContains));

            _memoFieldComboBoxControlSetup.LoadFromEnum<Conditions>();
            _memoFieldComboBoxControlSetup.Items.Remove(
                _memoFieldComboBoxControlSetup.Items.FirstOrDefault(p =>
                    p.NumericValue == (int)Conditions.Equals));

            _memoFieldComboBoxControlSetup.Items.Remove(
                _memoFieldComboBoxControlSetup.Items.FirstOrDefault(p =>
                    p.NumericValue == (int)Conditions.NotEquals));

            _memoFieldComboBoxControlSetup.Items.Remove(
                _memoFieldComboBoxControlSetup.Items.FirstOrDefault(p =>
                    p.NumericValue == (int)Conditions.GreaterThan));

            _memoFieldComboBoxControlSetup.Items.Remove(
                _memoFieldComboBoxControlSetup.Items.FirstOrDefault(p =>
                    p.NumericValue == (int)Conditions.GreaterThanEquals));

            _memoFieldComboBoxControlSetup.Items.Remove(
                _memoFieldComboBoxControlSetup.Items.FirstOrDefault(p =>
                    p.NumericValue == (int)Conditions.LessThan));

            _memoFieldComboBoxControlSetup.Items.Remove(
                _memoFieldComboBoxControlSetup.Items.FirstOrDefault(p =>
                    p.NumericValue == (int)Conditions.LessThanEquals));

            _memoFieldComboBoxControlSetup.Items.Remove(
                _memoFieldComboBoxControlSetup.Items.FirstOrDefault(p =>
                    p.NumericValue == (int)Conditions.BeginsWith));

            _memoFieldComboBoxControlSetup.Items.Remove(
                _memoFieldComboBoxControlSetup.Items.FirstOrDefault(p =>
                    p.NumericValue == (int)Conditions.EndsWith));

            switch (TreeViewItem.Type)
            {
                case TreeViewType.Field:
                    if (TreeViewItem.FieldDefinition.ParentJoinForeignKeyDefinition != null)
                    {
                        SearchValueAutoFillSetup = new AutoFillSetup(TreeViewItem.FieldDefinition
                            .ParentJoinForeignKeyDefinition.PrimaryTable.LookupDefinition);
                    }
                    switch (TreeViewItem.FieldDefinition.FieldDataType)
                    {
                        case FieldDataTypes.String:
                            ConditionComboBoxSetup = _stringFieldComboBoxControlSetup;
                            break;
                        case FieldDataTypes.Integer:
                            if (TreeViewItem.FieldDefinition.ParentJoinForeignKeyDefinition != null)
                            {
                                ConditionComboBoxSetup = _stringFieldComboBoxControlSetup;
                            }
                            else
                            {
                                ConditionComboBoxSetup = _numericFieldComboBoxControlSetup;
                            }
                            break;
                        case FieldDataTypes.Decimal:
                        case FieldDataTypes.DateTime:
                        case FieldDataTypes.Bool:
                            ConditionComboBoxSetup = _numericFieldComboBoxControlSetup;
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
            result.Condition = Condition;
            result.FieldDefinition = TreeViewItem.FieldDefinition;
            switch (TreeViewItem.Type)
            {
                case TreeViewType.Field:
                    if (TreeViewItem.FieldDefinition != null)
                    {
                        if (TreeViewItem.FieldDefinition.ParentJoinForeignKeyDefinition != null)
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
                        else
                        {
                            switch (TreeViewItem.FieldDefinition.FieldDataType)
                            {
                                case FieldDataTypes.String:
                                    result.SearchValue = StringSearchValue;
                                    break;
                                case FieldDataTypes.Integer:
                                    result.SearchValue = IntegerSearchValue.ToString();
                                    break;
                                case FieldDataTypes.Decimal:
                                    result.SearchValue = DecimalSearchValueDecimal.ToString();
                                    break;
                                case FieldDataTypes.DateTime:
                                    break;
                                case FieldDataTypes.Bool:
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                        }
                    }

                    break;
                case TreeViewType.Formula:
                    //result.FormulaParentFieldDefinition = TreeViewItem.Parent.FieldDefinition;
                    switch (FormulaValueType)
                    {
                        case FieldDataTypes.String:
                        case FieldDataTypes.Memo:
                            result.SearchValue = StringSearchValue;
                            break;
                        case FieldDataTypes.Integer:
                            result.SearchValue = IntegerSearchValue.ToString();
                            break;
                        case FieldDataTypes.Decimal:
                            result.SearchValue = DecimalSearchValueDecimal.ToString();
                            break;
                        case FieldDataTypes.DateTime:
                            break;
                        case FieldDataTypes.Bool:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    if (TreeViewItem.Parent != null)
                    {
                        result.FormulaParentFieldDefinition = TreeViewItem?.Parent.FieldDefinition;
                    }

                    result.Formula = Formula;
                    result.FormulaDisplayValue = FormulaDisplayValue;
                    result.FormulaValueType = FormulaValueType;

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return result;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
