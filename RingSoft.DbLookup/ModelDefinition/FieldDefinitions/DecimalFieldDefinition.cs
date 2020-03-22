using System;
using RingSoft.DbLookup.QueryBuilder;

namespace RingSoft.DbLookup.ModelDefinition.FieldDefinitions
{
    public enum DecimalFieldTypes
    {
        Decimal = 0,
        Currency = 1,
        Percent = 2
    }

    /// <summary>
    /// A decimal number (Decimal/Double/Float) field definition.
    /// </summary>
    /// <seealso cref="IntegerFieldDefinition" />
    public sealed class DecimalFieldDefinition : FieldDefinitionType<DecimalFieldDefinition>
    {
        public override FieldDataTypes FieldDataType => FieldDataTypes.Decimal;
        public override ValueTypes ValueType => ValueTypes.Numeric;

        /// <summary>
        /// Gets the decimal count.
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
        /// Gets the number format string.
        /// </summary>
        /// <value>
        /// The number format string.
        /// </value>
        public string NumberFormatString { get; internal set; }

        internal DecimalFieldDefinition()
        {
            DecimalCount = 2;
        }

        /// <summary>
        /// Sets the decimal count.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>This object.</returns>
        public DecimalFieldDefinition HasDecimalCount(int value)
        {
            DecimalCount = value;
            return this;
        }

        /// <summary>
        /// Sets the type of the decimal field.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>This object.</returns>
        public DecimalFieldDefinition HasDecimalFieldType(DecimalFieldTypes value)
        {
            DecimalFieldType = value;
            return this;
        }

        /// <summary>
        /// Sets the number format string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>This object.</returns>
        public DecimalFieldDefinition HasNumberFormatString(string value)
        {
            var number = 100000.12;
            var format = $"{{0:{value}}}";
            try
            {
                var checkFormat = string.Format(format, number);
                var unused = int.Parse(GblMethods.NumTextToString(checkFormat));
            }
            catch (Exception)
            {
                throw new ArgumentException("Invalid decimal format string.");
            }

            NumberFormatString = value;
            return this;
        }

        /// <summary>
        /// Formats the value to display.
        /// </summary>
        /// <param name="value">The value from the database.</param>
        /// <returns>
        /// The formatted value.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public override string FormatValue(string value)
        {
            var formatString = NumberFormatString;
            if (formatString.IsNullOrEmpty())
            {
                switch (DecimalFieldType)
                {
                    case DecimalFieldTypes.Decimal:
                        formatString = GblMethods.GetNumFormat(DecimalCount, false);
                        break;
                    case DecimalFieldTypes.Currency:
                        formatString = GblMethods.GetNumFormat(DecimalCount, true);
                        break;
                    case DecimalFieldTypes.Percent:
                        formatString = GblMethods.GetPercentFormat(DecimalCount);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return GblMethods.FormatValue(FieldDataType, value, formatString);
        }
    }
}
