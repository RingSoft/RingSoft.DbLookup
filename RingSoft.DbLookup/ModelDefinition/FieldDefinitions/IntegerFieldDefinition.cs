// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 08-02-2024
// ***********************************************************************
// <copyright file="IntegerFieldDefinition.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Globalization;
using System.Linq;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.QueryBuilder;

namespace RingSoft.DbLookup.ModelDefinition.FieldDefinitions
{
    /// <summary>
    /// A whole number (Integer/Long/Byte) field definition.
    /// </summary>
    /// <seealso cref="IntegerFieldDefinition" />
    public sealed class IntegerFieldDefinition : FieldDefinitionType<IntegerFieldDefinition>
    {
        /// <summary>
        /// Gets the type of the field data.
        /// </summary>
        /// <value>The type of the field data.</value>
        public override FieldDataTypes FieldDataType => FieldDataTypes.Integer;

        /// <summary>
        /// Gets the number format string.
        /// </summary>
        /// <value>The number format string.</value>
        public string NumberFormatString { get; private set; }

        /// <summary>
        /// Gets the culture.
        /// </summary>
        /// <value>The culture.</value>
        public CultureInfo Culture { get; private set; } = LookupDefaults.DefaultNumberCulture;

        /// <summary>
        /// The enum field
        /// </summary>
        private EnumFieldTranslation _enumField;
        /// <summary>
        /// Gets the enum translation object which maps an enumerator value to its associated description.
        /// </summary>
        /// <value>The enum translation.</value>
        public EnumFieldTranslation EnumTranslation { get; private set; }

        /// <summary>
        /// Gets the content template identifier.
        /// </summary>
        /// <value>The content template identifier.</value>
        public int ContentTemplateId { get; private set; }

        public bool OverrideIdentity { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="IntegerFieldDefinition" /> class.
        /// </summary>
        internal IntegerFieldDefinition()
        {
            //var unused = HasNumberFormatString(GblMethods.GetNumFormat(0, false));
        }

        /// <summary>
        /// Sets the number format string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>This object.</returns>
        /// <exception cref="System.ArgumentException">Invalid integer format string.</exception>
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
        /// Sets the culture identifier.
        /// </summary>
        /// <param name="cultureId">The culture identifier.</param>
        /// <returns>IntegerFieldDefinition.</returns>
        public IntegerFieldDefinition HasCultureId(string cultureId)
        {
            Culture = new CultureInfo(cultureId);
            return this;
        }

        /// <summary>
        /// Formats the value to display.
        /// </summary>
        /// <param name="value">The value from the database.</param>
        /// <returns>The formatted value.</returns>
        public override string FormatValue(string value)
        {
            if (EnumTranslation != null)
            {
                var numValue = value.ToInt();
                var trans = EnumTranslation
                    .TypeTranslations
                    .FirstOrDefault(p => p.NumericValue == numValue);
                if (trans != null)
                {
                    return trans.TextValue;
                }
            }

            if (TableDefinition.PrimaryKeyFields.Contains(this))
                return value;

            var formatString = NumberFormatString;
            if (formatString.IsNullOrEmpty())
                formatString = GblMethods.GetNumFormat(0, false);

            return GblMethods.FormatValue(FieldDataType, value, formatString, Culture);
        }

        /// <summary>
        /// Validates a value to see if it's a valid value to save to the database.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <returns>True if the value is safe to save to the database.</returns>
        public override bool ValidateValueForSavingToDb(string value)
        {
            //if (!AllowNulls)
            //{
            //    var intValue = value.ToInt();
            //    if (ParentJoinForeignKeyDefinition != null && intValue == 0)
            //        return false;
            //}

            return base.ValidateValueForSavingToDb(value);
        }

        /// <summary>
        /// Determines whether this instance is enum.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>IntegerFieldDefinition.</returns>
        public IntegerFieldDefinition IsEnum<T>() where T : Enum
        {
            EnumTranslation = new EnumFieldTranslation();
            EnumTranslation.LoadFromEnum<T>();
            return this;
        }

        /// <summary>
        /// Sets the enum translation.
        /// </summary>
        /// <param name="enumFieldTranslation">The enum field translation.</param>
        internal void SetEnumTranslation(EnumFieldTranslation enumFieldTranslation)
        {
            EnumTranslation = enumFieldTranslation;
        }

        /// <summary>
        /// Determines whether [has content template identifier] [the specified content template identifier].
        /// </summary>
        /// <param name="contentTemplateId">The content template identifier.</param>
        /// <returns>IntegerFieldDefinition.</returns>
        public IntegerFieldDefinition HasContentTemplateId(int contentTemplateId)
        {
            ContentTemplateId = contentTemplateId;
            if (LookupControlColumnId == LookupDefaults.TextColumnId)
                LookupControlColumnId = LookupDefaults.CustomContentColumnId;

            return this;
        }

        public IntegerFieldDefinition DoOverrideIdentity()
        {
            OverrideIdentity = true;
            return this;
        }

        /// <summary>
        /// Gets the user value.
        /// </summary>
        /// <param name="dbIdValue">The database identifier value.</param>
        /// <returns>System.String.</returns>
        public override string GetUserValue(string dbIdValue, bool dbIdValueIsText = false)
        {
            if (ParentJoinForeignKeyDefinition == null && EnumTranslation != null)
            {
                var enumText = EnumTranslation.TypeTranslations.FirstOrDefault(p => p.NumericValue == dbIdValue.ToInt())
                    .TextValue;
                return enumText;
            }

            if (ParentJoinForeignKeyDefinition == null && TableDefinition.PrimaryKeyFields.Contains(this))
            {
                return dbIdValue;
            }
            return base.GetUserValue(dbIdValue, dbIdValueIsText);
        }

        /// <summary>
        /// Formats the value for column map.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>System.String.</returns>
        public override string FormatValueForColumnMap(string value)
        {
            if (EnumTranslation != null)
            {
                var enumField = EnumTranslation.TypeTranslations
                    .FirstOrDefault(p => p.NumericValue == value.ToInt());
                return enumField.TextValue;
            }

            return base.FormatValueForColumnMap(value);
        }
    }
}
