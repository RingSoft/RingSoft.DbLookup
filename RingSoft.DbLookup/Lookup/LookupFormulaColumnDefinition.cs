﻿using RingSoft.DbLookup.QueryBuilder;
using System;
using System.Globalization;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;

namespace RingSoft.DbLookup.Lookup
{
    /// <summary>
    /// A lookup column based on a formula.
    /// </summary>
    /// <seealso cref="LookupColumnDefinitionBase" />
    public class LookupFormulaColumnDefinition : LookupColumnDefinitionType<LookupFormulaColumnDefinition>
    {
        /// <summary>
        /// Gets the type of the column.
        /// </summary>
        /// <value>
        /// The type of the column.
        /// </value>
        public override LookupColumnTypes ColumnType => LookupColumnTypes.Formula;

        /// <summary>
        /// Gets the type of the data.
        /// </summary>
        /// <value>
        /// The type of the data.
        /// </value>
        public override FieldDataTypes DataType => _dataType;

        /// <summary>
        /// Gets the type of the value.
        /// </summary>
        /// <value>
        /// The type of the value.
        /// </value>
        public ValueTypes ValueType => GblMethods.GetValueTypeForFieldDataType(DataType);

        /// <summary>
        /// Gets the select SQL alias.
        /// </summary>
        /// <value>
        /// The select SQL alias.
        /// </value>
        public override string SelectSqlAlias => _selectSqlAlias;

        /// <summary>
        /// Gets the number format string.
        /// </summary>
        /// <value>
        /// The number format string.
        /// </value>
        public string NumberFormatString { get; internal set; }

        /// <summary>
        /// Gets the date format string.
        /// </summary>
        /// <value>
        /// The date format string.
        /// </value>
        public string DateFormatString { get; private set; }

        /// <summary>
        /// Gets the formula.
        /// </summary>
        /// <value>
        /// The formula.
        /// </value>
        public string Formula { get; internal set; }

        private CultureInfo _columnCulture;

        /// <summary>
        /// Gets the culture.
        /// </summary>
        /// <value>
        /// The culture.
        /// </value>
        public CultureInfo ColumnCulture
        {
            get
            {
                if (_columnCulture == null)
                {
                    if (DataType == FieldDataTypes.DateTime)
                        return LookupDefaults.DefaultDateCulture;
                    else
                    {
                        return LookupDefaults.DefaultNumberCulture;
                    }
                }

                return _columnCulture;
            }
            private set => _columnCulture = value;
        }

        /// <summary>
        /// Gets the number of digits to the right of the decimal point.
        /// </summary>
        /// <value>
        /// The decimal count.
        /// </value>
        public int DecimalCount { get; internal set; }

        /// <summary>
        /// Gets the type of the decimal field.
        /// </summary>
        /// <value>
        /// The type of the decimal field.
        /// </value>
        public DecimalFieldTypes DecimalFieldType { get; internal set; }

        /// <summary>
        /// Gets the type of the date.
        /// </summary>
        /// <value>
        /// The type of the date.
        /// </value>
        public DbDateTypes DateType { get; private set; }

        public bool ShowNegativeValuesInRed { get; private set; }

        private readonly string _selectSqlAlias;
        private FieldDataTypes _dataType;

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupFormulaColumnDefinition"/> class.
        /// </summary>
        /// <param name="formula">The formula.</param>
        /// <param name="dataType">Type of the data.</param>
        internal LookupFormulaColumnDefinition(string formula, FieldDataTypes dataType)
        {
            Formula = formula;
            _selectSqlAlias = Guid.NewGuid().ToString().Replace("-", "").ToUpper();
            _dataType = dataType;

            DateFormatString = LookupDefaults.DefaultDateCulture.DateTimeFormat.ShortDatePattern;
            DecimalCount = LookupDefaults.DefaultDecimalCount;

            SetupColumn();
        }

        internal override void CopyFrom(LookupColumnDefinitionBase source)
        {
            if (source is LookupFormulaColumnDefinition formulaSource)
            {
                NumberFormatString = formulaSource.NumberFormatString;
                DateFormatString = formulaSource.DateFormatString;
                ColumnCulture = formulaSource.ColumnCulture;
                DecimalCount = formulaSource.DecimalCount;
                DecimalFieldType = formulaSource.DecimalFieldType;
                DateType = formulaSource.DateType;
                ShowNegativeValuesInRed = formulaSource.ShowNegativeValuesInRed;
            }
            base.CopyFrom(source);
        }

        public LookupFormulaColumnDefinition UpdateFormula(string formula)
        {
            Formula = formula;
            return this;
        }


        /// <summary>
        /// Sets the number format string.
        /// </summary>
        /// <param name="value">The number format string.</param>
        /// <returns>This object.</returns>
        public LookupFormulaColumnDefinition HasNumberFormatString(string value)
        {
            var number = 100000.12;
            var format = $"{{0:{value}}}";
            try
            {
                var checkFormat = string.Format(format, number);
                var unused = double.Parse(GblMethods.NumTextToString(checkFormat));
            }
            catch (Exception)
            {
                throw new ArgumentException("Invalid format string.");
            }

            NumberFormatString = value;
            return this;
        }

        /// <summary>
        /// Sets the date format string.
        /// </summary>
        /// <param name="value">The date format string.</param>
        /// <returns>This object.</returns>
        public LookupFormulaColumnDefinition HasDateFormatString(string value)
        {
            var date = new DateTime(2000, 01, 01);
            var format = $"{{0:{value}}}";
            try
            {
                var dateCheckFormat = string.Format(format, date);
                var unused = DateTime.Parse(dateCheckFormat);
            }
            catch (Exception)
            {
                throw new ArgumentException("Invalid date format string.");
            }
            DateFormatString = value;
            return this;
        }

        /// <summary>
        /// Sets the column culture identifier.
        /// </summary>
        /// <param name="cultureId">The culture identifier.</param>
        /// <returns></returns>
        public LookupFormulaColumnDefinition HasColumnCultureId(string cultureId)
        {
            ColumnCulture = new CultureInfo(cultureId);
            DecimalFieldDefinition.FormatCulture(ColumnCulture);
            return this;
        }

        /// <summary>
        /// Formats the value to display in the lookup view.
        /// </summary>
        /// <param name="value">The value from the database.</param>
        /// <returns>
        /// The formatted value.
        /// </returns>
        /// <exception cref="NotImplementedException"></exception>
        public override string FormatValue(string value)
        {
            switch (DataType)
            {
                case FieldDataTypes.String:
                    break;
                case FieldDataTypes.Integer:
                case FieldDataTypes.Decimal:
                    return DecimalFieldDefinition.FormatNumericValue(value, NumberFormatString, DecimalFieldType,
                        DecimalCount, ColumnCulture);
                case FieldDataTypes.DateTime:
                    return DateFieldDefinition.FormatDateValue(value, DateFormatString, DateType, ColumnCulture);
                case FieldDataTypes.Bool:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return GblMethods.FormatValue(DataType, value);
        }

        /// <summary>
        /// Sets the number of digits to the right of the decimal point.
        /// </summary>
        /// <param name="value">The new digits value.</param>
        /// <returns>This object.</returns>
        public LookupFormulaColumnDefinition HasDecimalCount(int value)
        {
            DecimalCount = value;
            return this;
        }

        /// <summary>
        /// Sets the type of this decimal field.
        /// </summary>
        /// <param name="value">The new DecimalFieldTypes value.</param>
        /// <returns>This object.</returns>
        public LookupFormulaColumnDefinition HasDecimalFieldType(DecimalFieldTypes value)
        {
            DecimalFieldType = value;
            return this;
        }

        /// <summary>
        /// Sets the type of the date.
        /// </summary>
        /// <param name="value">The new DbDateTypes value.</param>
        /// <returns>This object.</returns>
        public LookupFormulaColumnDefinition HasDateType(DbDateTypes value)
        {
            DateType = value;
            return this;
        }

        public LookupFormulaColumnDefinition DoShowNegativeValuesInRed(bool value = true)
        {
            ShowNegativeValuesInRed = value;
            return this;
        }
    }
}
