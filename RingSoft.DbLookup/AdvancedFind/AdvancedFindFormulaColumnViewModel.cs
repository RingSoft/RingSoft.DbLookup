// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-19-2022
// ***********************************************************************
// <copyright file="AdvancedFindFormulaColumnViewModel.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.ComponentModel;
using System.Runtime.CompilerServices;
using RingSoft.DataEntryControls.Engine;

namespace RingSoft.DbLookup.AdvancedFind
{
    /// <summary>
    /// Class AdvancedFindFormulaColumnViewModel.
    /// Implements the <see cref="INotifyPropertyChanged" />
    /// </summary>
    /// <seealso cref="INotifyPropertyChanged" />
    public class AdvancedFindFormulaColumnViewModel : INotifyPropertyChanged
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
        /// The data type ComboBox control setup
        /// </summary>
        private TextComboBoxControlSetup _dataTypeComboBoxControlSetup;

        /// <summary>
        /// Gets or sets the data type ComboBox control setup.
        /// </summary>
        /// <value>The data type ComboBox control setup.</value>
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

        /// <summary>
        /// The data type ComboBox item
        /// </summary>
        private TextComboBoxItem _dataTypeComboBoxItem;

        /// <summary>
        /// Gets or sets the data type ComboBox item.
        /// </summary>
        /// <value>The data type ComboBox item.</value>
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

        /// <summary>
        /// Gets or sets the type of the data.
        /// </summary>
        /// <value>The type of the data.</value>
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

        /// <summary>
        /// The decimal format combo setup
        /// </summary>
        private TextComboBoxControlSetup _decimalFormatComboSetup;

        /// <summary>
        /// Gets or sets the decimal format combo setup.
        /// </summary>
        /// <value>The decimal format combo setup.</value>
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

        /// <summary>
        /// The decimal format ComboBox item
        /// </summary>
        private TextComboBoxItem _decimalFormatComboBoxItem;

        /// <summary>
        /// Gets or sets the decimal format ComboBox item.
        /// </summary>
        /// <value>The decimal format ComboBox item.</value>
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

        /// <summary>
        /// Gets or sets the type of the decimal format.
        /// </summary>
        /// <value>The type of the decimal format.</value>
        public DecimalEditFormatTypes DecimalFormatType
        {
            get => (DecimalEditFormatTypes) DecimalFormatComboBoxItem.NumericValue;
            set => DecimalFormatComboBoxItem = DecimalFormatComboSetup.GetItem((int) value);
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Initialize()
        {
            DataTypeComboBoxControlSetup = new TextComboBoxControlSetup();
            DataTypeComboBoxControlSetup.LoadFromEnum<FieldDataTypes>();
            DecimalFormatComboSetup = new TextComboBoxControlSetup();
            DecimalFormatComboSetup.LoadFromEnum<DecimalEditFormatTypes>();
            DataType = FieldDataTypes.String;
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
