// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 06-29-2023
// ***********************************************************************
// <copyright file="DateFieldDefinition.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Globalization;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.QueryBuilder;

namespace RingSoft.DbLookup.ModelDefinition.FieldDefinitions
{
    /// <summary>
    /// A date field definition.
    /// </summary>
    public sealed class DateFieldDefinition : FieldDefinitionType<DateFieldDefinition>
    {
        /// <summary>
        /// Gets the type of the field data.
        /// </summary>
        /// <value>The type of the field data.</value>
        public override FieldDataTypes FieldDataType => FieldDataTypes.DateTime;

        /// <summary>
        /// Gets the type of the date.
        /// </summary>
        /// <value>The type of the date.</value>
        public DbDateTypes DateType { get; private set; }

        /// <summary>
        /// Gets the date format string.
        /// </summary>
        /// <value>The date format string.</value>
        public string DateFormatString { get; private set; }

        /// <summary>
        /// Gets the culture.
        /// </summary>
        /// <value>The culture.</value>
        public CultureInfo Culture { get; private set; } = LookupDefaults.DefaultDateCulture;

        /// <summary>
        /// Gets a value indicating whether [convert to local time].
        /// </summary>
        /// <value><c>true</c> if [convert to local time]; otherwise, <c>false</c>.</value>
        public bool ConvertToLocalTime { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateFieldDefinition"/> class.
        /// </summary>
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
        /// <exception cref="System.ArgumentException">Invalid date format string.</exception>
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
        /// Sets the culture identifier.
        /// </summary>
        /// <param name="cultureId">The culture identifier.</param>
        /// <returns>DateFieldDefinition.</returns>
        public DateFieldDefinition HasCultureId(string cultureId)
        {
            Culture = new CultureInfo(cultureId);
            return this;
        }

        /// <summary>
        /// Formats the value to display.
        /// </summary>
        /// <param name="value">The value from the database.</param>
        /// <returns>The formatted value.</returns>
        /// <exception cref="ArgumentOutOfRangeException">value - null</exception>
        public override string FormatValue(string value)
        {
            var convertToLocalTime = ConvertToLocalTime;
            if (!convertToLocalTime)
            {
                convertToLocalTime = SystemGlobals.ConvertAllDatesToUniversalTime;
            }

            return FormatDateValue(value, DateFormatString, DateType, Culture, convertToLocalTime);
        }

        /// <summary>
        /// Formats the date value.
        /// </summary>
        /// <param name="dateValue">The date value.</param>
        /// <param name="dateFormatString">The date format string.</param>
        /// <param name="dateType">Type of the date.</param>
        /// <param name="culture">The culture.</param>
        /// <param name="convertToLocalTime">if set to <c>true</c> [convert to local time].</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">dateValue - null</exception>
        public static string FormatDateValue(string dateValue, string dateFormatString, DbDateTypes dateType,
            CultureInfo culture, bool convertToLocalTime = false)
        {
            var formatString = dateFormatString;
            if (formatString.IsNullOrEmpty())
            {
                switch (dateType)
                {
                    case DbDateTypes.DateOnly:
                        formatString = culture.DateTimeFormat.ShortDatePattern;
                        break;
                    case DbDateTypes.DateTime:
                    case DbDateTypes.Millisecond:
                        formatString = culture.DateTimeFormat.ShortDatePattern + ' ' + culture.DateTimeFormat.LongTimePattern;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(dateValue), dateValue, null);
                }
            }

            return GblMethods.FormatValue(FieldDataTypes.DateTime, dateValue, formatString, culture, convertToLocalTime);
        }

        /// <summary>
        /// Does the convert to local time.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns>DateFieldDefinition.</returns>
        public DateFieldDefinition DoConvertToLocalTime(bool value = true)
        {
            ConvertToLocalTime = value;
            return this;
        }
    }
}
