using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.QueryBuilder;

namespace RingSoft.DbMaintenance
{
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


        public Conditions Condition
        {
            get => (Conditions) ConditionComboBoxItem.NumericValue;
            set => ConditionComboBoxSetup.GetItem((int) value);
        }

        public TreeViewItem TreeViewItem { get; set; }
        public LookupDefinitionBase LookupDefinition { get; set; }

        private TextComboBoxControlSetup _stringFieldComboBoxControlSetup = new TextComboBoxControlSetup();
        private TextComboBoxControlSetup _numericFieldComboBoxControlSetup = new TextComboBoxControlSetup();

        public void Initialize(TreeViewItem treeViewItem, LookupDefinitionBase lookupDefinition)
        {
            LookupDefinition = lookupDefinition;
            TreeViewItem = treeViewItem;

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
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
