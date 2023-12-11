// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 07-02-2023
// ***********************************************************************
// <copyright file="DecimalFieldDefinition.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Globalization;
using RingSoft.DataEntryControls.Engine;

namespace RingSoft.DbLookup.ModelDefinition.FieldDefinitions
{
    /// <summary>
    /// Enum DecimalFieldTypes
    /// </summary>
    public enum DecimalFieldTypes
    {
        /// <summary>
        /// The decimal
        /// </summary>
        Decimal = 0,
        /// <summary>
        /// The currency
        /// </summary>
        Currency = 1,
        /// <summary>
        /// The percent
        /// </summary>
        Percent = 2
    }

    /// <summary>
    /// A double number (Decimal/Double/Float) field definition.
    /// </summary>
    /// <seealso cref="IntegerFieldDefinition" />
    public sealed class DecimalFieldDefinition : FieldDefinitionType<DecimalFieldDefinition>
    {
        /// <summary>
        /// Gets the type of the field data.
        /// </summary>
        /// <value>The type of the field data.</value>
        public override FieldDataTypes FieldDataType => FieldDataTypes.Decimal;

        /// <summary>
        /// Gets the number of digits to the right of the double point.
        /// </summary>
        /// <value>The double count.</value>
        public int DecimalCount { get; internal set; }

        /// <summary>
        /// Gets the type of the double field.
        /// </summary>
        /// <value>The type of the double field.</value>
        public DecimalFieldTypes DecimalFieldType { get; internal set; }

        /// <summary>
        /// Gets the number format string.  Default value is empty.
        /// </summary>
        /// <value>The number format string.</value>
        public string NumberFormatString { get; internal set; }

        /// <summary>
        /// Gets the culture.
        /// </summary>
        /// <value>The culture.</value>
        public CultureInfo Culture { get; private set; } = LookupDefaults.DefaultNumberCulture;

        /// <summary>
        /// Gets a value indicating whether [show negative values in red].
        /// </summary>
        /// <value><c>true</c> if [show negative values in red]; otherwise, <c>false</c>.</value>
        public bool ShowNegativeValuesInRed { get; private set; }

        /// <summary>
        /// Gets a value indicating whether [show positive values in green].
        /// </summary>
        /// <value><c>true</c> if [show positive values in green]; otherwise, <c>false</c>.</value>
        public bool ShowPositiveValuesInGreen { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DecimalFieldDefinition"/> class.
        /// </summary>
        internal DecimalFieldDefinition()
        {
            DecimalCount = LookupDefaults.DefaultDecimalCount;
            FormatCulture(Culture);
        }

        /// <summary>
        /// Sets the number of digits to the right of the double point.
        /// </summary>
        /// <param name="value">The new digits value.</param>
        /// <returns>This object.</returns>
        public DecimalFieldDefinition HasDecimalCount(int value)
        {
            DecimalCount = value;
            return this;
        }

        /// <summary>
        /// Sets the type of this double field.
        /// </summary>
        /// <param name="value">The new DecimalFieldTypes value.</param>
        /// <returns>This object.</returns>
        public DecimalFieldDefinition HasDecimalFieldType(DecimalFieldTypes value)
        {
            DecimalFieldType = value;
            return this;
        }

        /// <summary>
        /// Sets the number format string.
        /// </summary>
        /// <param name="value">The new format string value.</param>
        /// <returns>This object.</returns>
        /// <exception cref="System.ArgumentException">Invalid double format string.</exception>
        public DecimalFieldDefinition HasNumberFormatString(string value)
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
                throw new ArgumentException("Invalid double format string.");
            }

            NumberFormatString = value;
            return this;
        }

        /// <summary>
        /// Sets the culture identifier.
        /// </summary>
        /// <param name="cultureId">The culture identifier.</param>
        /// <returns>DecimalFieldDefinition.</returns>
        public DecimalFieldDefinition HasCultureId(string cultureId)
        {
            Culture = new CultureInfo(cultureId);
            FormatCulture(Culture);
            return this;
        }

        /// <summary>
        /// Formats the culture.
        /// </summary>
        /// <param name="culture">The culture.</param>
        public static void FormatCulture(CultureInfo culture)
        {
            DecimalEditControlSetup.FormatCulture(culture);
        }

        /// <summary>
        /// Formats the value to display.
        /// </summary>
        /// <param name="value">The value from the database.</param>
        /// <returns>The formatted value.</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public override string FormatValue(string value)
        {
            return FormatNumericValue(value, NumberFormatString, DecimalFieldType, DecimalCount, Culture);
        }

        /// <summary>
        /// Formats the numeric value.
        /// </summary>
        /// <param name="numericValue">The numeric value.</param>
        /// <param name="numberFormatString">The number format string.</param>
        /// <param name="decimalFieldType">Type of the decimal field.</param>
        /// <param name="decimalCount">The decimal count.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public static string FormatNumericValue(string numericValue, string numberFormatString,
            DecimalFieldTypes decimalFieldType, int decimalCount, CultureInfo culture)
        {
            var formatString = numberFormatString;
            if (formatString.IsNullOrEmpty())
            {
                switch (decimalFieldType)
                {
                    case DecimalFieldTypes.Decimal:
                        formatString = GblMethods.GetNumFormat(decimalCount, false);
                        break;
                    case DecimalFieldTypes.Currency:
                        formatString = GblMethods.GetNumFormat(decimalCount, true);
                        break;
                    case DecimalFieldTypes.Percent:
                        formatString = GblMethods.GetPercentFormat(decimalCount);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return GblMethods.FormatValue(FieldDataTypes.Decimal, numericValue, formatString, culture);
        }

        /// <summary>
        /// Does the show negative values in red.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns>DecimalFieldDefinition.</returns>
        public DecimalFieldDefinition DoShowNegativeValuesInRed(bool value = true)
        {
            ShowNegativeValuesInRed = value;
            return this;
        }

        /// <summary>
        /// Does the show positive values in green.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns>DecimalFieldDefinition.</returns>
        public DecimalFieldDefinition DoShowPositiveValuesInGreen(bool value = true)
        {
            ShowPositiveValuesInGreen = value;
            return this;
        }

    }
}
