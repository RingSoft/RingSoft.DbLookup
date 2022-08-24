using System.ComponentModel;
using System.Runtime.CompilerServices;
using RingSoft.DataEntryControls.Engine;

namespace RingSoft.DbLookup.AdvancedFind
{
    public class AdvancedFindFormulaColumnViewModel : INotifyPropertyChanged
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

        private TextComboBoxControlSetup _dataTypeComboBoxControlSetup;

        public TextComboBoxControlSetup DataTypeComboBoxControlSetup
        {
            get => _dataTypeComboBoxControlSetup;
            set
            {
                if (_dataTypeComboBoxControlSetup == value)
                {
                    return;
                }
                _dataTypeComboBoxControlSetup = value;
                OnPropertyChanged();
            }
        }

        private TextComboBoxItem _dataTypeComboBoxItem;

        public TextComboBoxItem DataTypeComboBoxItem
        {
            get => _dataTypeComboBoxItem;
            set
            {
                if (_dataTypeComboBoxItem == value)
                {
                    return;
                }
                _dataTypeComboBoxItem = value;
                OnPropertyChanged();
            }
        }

        public FieldDataTypes DataType
        {
            get => (FieldDataTypes)DataTypeComboBoxItem.NumericValue;
            set
            {
                DataTypeComboBoxItem = DataTypeComboBoxControlSetup.GetItem((int) value);
                if (value == FieldDataTypes.Decimal)
                {
                    DecimalFormatType = DecimalEditFormatTypes.Number;
                }
            }
        }

        private TextComboBoxControlSetup _decimalFormatComboSetup;

        public TextComboBoxControlSetup DecimalFormatComboSetup
        {
            get => _decimalFormatComboSetup;
            set
            {
                if (_decimalFormatComboSetup == value)
                {
                    return;
                }
                _decimalFormatComboSetup = value;
                OnPropertyChanged();
            }
        }

        private TextComboBoxItem _decimalFormatComboBoxItem;

        public TextComboBoxItem DecimalFormatComboBoxItem
        {
            get => _decimalFormatComboBoxItem;
            set
            {
                if (_decimalFormatComboBoxItem == value)
                {
                    return;
                }
                _decimalFormatComboBoxItem = value;
                OnPropertyChanged();
            }
        }

        public DecimalEditFormatTypes DecimalFormatType
        {
            get => (DecimalEditFormatTypes) DecimalFormatComboBoxItem.NumericValue;
            set => DecimalFormatComboBoxItem = DecimalFormatComboSetup.GetItem((int) value);
        }

        public void Initialize()
        {
            DataTypeComboBoxControlSetup = new TextComboBoxControlSetup();
            DataTypeComboBoxControlSetup.LoadFromEnum<FieldDataTypes>();
            DecimalFormatComboSetup = new TextComboBoxControlSetup();
            DecimalFormatComboSetup.LoadFromEnum<DecimalEditFormatTypes>();
            DataType = FieldDataTypes.String;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
