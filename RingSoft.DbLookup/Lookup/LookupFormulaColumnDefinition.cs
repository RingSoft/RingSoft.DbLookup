using System;
using RingSoft.DbLookup.QueryBuilder;

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
        public string NumberFormatString { get; internal set; } = GblMethods.GetNumFormat(2, false);

        /// <summary>
        /// Gets the date format string.
        /// </summary>
        /// <value>
        /// The date format string.
        /// </value>
        public string DateFormatString { get; private set; } = "MM/dd/yyyy";

        /// <summary>
        /// Gets the formula.
        /// </summary>
        /// <value>
        /// The formula.
        /// </value>
        public string Formula { get; internal set; }

        private readonly string _selectSqlAlias;
        private FieldDataTypes _dataType;

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupFormulaColumnDefinition"/> class.
        /// </summary>
        /// <param name="formula">The formula.</param>
        /// <param name="dataType">Type of the data.</param>
        public LookupFormulaColumnDefinition(string formula, FieldDataTypes dataType)
        {
            Formula = formula;
            _selectSqlAlias = Guid.NewGuid().ToString().Replace("-", "").ToUpper();
            _dataType = dataType;
            SetupColumn();
        }

        internal override void CopyFrom(LookupColumnDefinitionBase source)
        {
            if (source is LookupFormulaColumnDefinition formulaSource)
            {
                NumberFormatString = formulaSource.NumberFormatString;
                DateFormatString = formulaSource.DateFormatString;
            }
            base.CopyFrom(source);
        }

        /// <summary>
        /// Sets the number format string.
        /// </summary>
        /// <param name="value">The value.</param>
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
        /// <param name="value">The value.</param>
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
                    return GblMethods.FormatValue(DataType, value, NumberFormatString);
                case FieldDataTypes.Enum:
                    break;
                case FieldDataTypes.DateTime:
                    return GblMethods.FormatValue(DataType, value, DateFormatString);
                case FieldDataTypes.Bool:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return GblMethods.FormatValue(DataType, value);
        }
    }
}
