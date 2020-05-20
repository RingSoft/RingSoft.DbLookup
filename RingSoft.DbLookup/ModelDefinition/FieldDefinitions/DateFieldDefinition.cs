using System;
using RingSoft.DbLookup.QueryBuilder;

namespace RingSoft.DbLookup.ModelDefinition.FieldDefinitions
{
    /// <summary>
    /// A date field definition.
    /// </summary>
    public sealed class DateFieldDefinition : FieldDefinitionType<DateFieldDefinition>
    {
        public override FieldDataTypes FieldDataType => FieldDataTypes.DateTime;

        /// <summary>
        /// Gets the type of the date.
        /// </summary>
        /// <value>
        /// The type of the date.
        /// </value>
        public DbDateTypes DateType { get; private set; }

        /// <summary>
        /// Gets the date format string.
        /// </summary>
        /// <value>
        /// The date format string.
        /// </value>
        public string DateFormatString { get; private set; }

        internal DateFieldDefinition()
        {
            DateType = DbDateTypes.DateOnly;
        }

        /// <summary>
        /// Sets the type of the date.
        /// </summary>
        /// <param name="value">The new DbDateTypes value.</param>
        /// <returns>This object.</returns>
        public DateFieldDefinition HasDateType(DbDateTypes value)
        {
            DateType = value;
            return this;
        }

        /// <summary>
        /// Sets the date format string to override the default formatting based on the DateType property.
        /// </summary>
        /// <param name="value">The new format string value.</param>
        /// <returns>This object.</returns>
        public DateFieldDefinition HasDateFormatString(string value)
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
        /// Formats the value to display.
        /// </summary>
        /// <param name="value">The value from the database.</param>
        /// <returns>
        /// The formatted value.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">value - null</exception>
        public override string FormatValue(string value)
        {
            var formatString = DateFormatString;
            if (formatString.IsNullOrEmpty())
            {
                switch (DateType)
                {
                    case DbDateTypes.DateOnly:
                        formatString = "MM/dd/yyyy";
                        break;
                    case DbDateTypes.DateTime:
                        formatString = "MM/dd/yyyy hh:mm:ss tt";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(value), value, null);
                }
            }

            return GblMethods.FormatValue(FieldDataType, value, formatString);
        }
    }
}
