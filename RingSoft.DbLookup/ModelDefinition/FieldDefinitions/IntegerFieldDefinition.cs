using System;
using System.Linq;

namespace RingSoft.DbLookup.ModelDefinition.FieldDefinitions
{
    /// <summary>
    /// A whole number (Integer/Long/Byte) field definition.
    /// </summary>
    /// <seealso cref="IntegerFieldDefinition" />
    public sealed class IntegerFieldDefinition : FieldDefinitionType<IntegerFieldDefinition>
    {
        public override FieldDataTypes FieldDataType => FieldDataTypes.Integer;

        /// <summary>
        /// Gets the number format string.
        /// </summary>
        /// <value>
        /// The number format string.
        /// </value>
        public string NumberFormatString { get; private set; }

        internal IntegerFieldDefinition()
        {
            //var unused = HasNumberFormatString(GblMethods.GetNumFormat(0, false));
        }

        /// <summary>
        /// Sets the number format string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>This object.</returns>
        public IntegerFieldDefinition HasNumberFormatString(string value)
        {
            var number = 100000;
            var format = $"{{0:{value}}}";
            try
            {
                var checkFormat = string.Format(format, number);
                var unused = int.Parse(GblMethods.NumTextToString(checkFormat));
            }
            catch (Exception)
            {
                throw new ArgumentException("Invalid integer format string.");
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
        public override string FormatValue(string value)
        {
            if (TableDefinition.PrimaryKeyFields.Contains(this))
                return value;

            var formatString = NumberFormatString;
            if (formatString.IsNullOrEmpty())
                formatString = GblMethods.GetNumFormat(0, false);

            return GblMethods.FormatValue(FieldDataType, value, formatString);
        }

        /// <summary>
        /// Validates a value to see if it's a valid value to save to the database.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <returns>
        /// True if the value is safe to save to the database.
        /// </returns>
        public override bool ValidateValueForSavingToDb(string value)
        {
            if (!AllowNulls)
            {
                var intValue = value.ToInt();
                if (ParentJoinForeignKeyDefinition != null && intValue == 0)
                    return false;
            }

            return base.ValidateValueForSavingToDb(value);
        }
    }
}
