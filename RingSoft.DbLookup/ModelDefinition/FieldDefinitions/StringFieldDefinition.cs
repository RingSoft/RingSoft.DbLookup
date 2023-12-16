// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 11-04-2023
// ***********************************************************************
// <copyright file="StringFieldDefinition.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.QueryBuilder;

namespace RingSoft.DbLookup.ModelDefinition.FieldDefinitions
{
    /// <summary>
    /// A string field definition.
    /// </summary>
    /// <seealso />
    public sealed class StringFieldDefinition : FieldDefinitionType<StringFieldDefinition>
    {
        /// <summary>
        /// Gets the type of the field data.
        /// </summary>
        /// <value>The type of the field data.</value>
        public override FieldDataTypes FieldDataType => FieldDataTypes.String;

        /// <summary>
        /// Gets the type of the value for use in queries.
        /// </summary>
        /// <value>The type of the value.</value>
        public override ValueTypes ValueType
        {
            get
            {
                if (MemoField)
                    return ValueTypes.Memo;
                return ValueTypes.String;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is a memo.
        /// </summary>
        /// <value><c>true</c> if this instance is memo; otherwise, <c>false</c>.</value>
        public bool MemoField { get; private set; }

        /// <summary>
        /// Gets the maximum length.
        /// </summary>
        /// <value>The maximum length.</value>
        public int MaxLength { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringFieldDefinition"/> class.
        /// </summary>
        internal StringFieldDefinition()
        {
        }

        /// <summary>
        /// Used to make this field a memo type.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns>This object.</returns>
        public StringFieldDefinition IsMemo(bool value = true)
        {
            MemoField = value;
            return this;
        }

        /// <summary>
        /// Sets the maximum length.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>StringFieldDefinition.</returns>
        public StringFieldDefinition HasMaxLength(int value)
        {
            MaxLength = value;
            return this;
        }

        /// <summary>
        /// Formats the value to display.
        /// </summary>
        /// <param name="value">The value from the database.</param>
        /// <returns>The formatted value.</returns>
        public override string FormatValue(string value)
        {
            return GblMethods.FormatValue(FieldDataType, value);
        }

        /// <summary>
        /// Validates a value to see if it's a valid value to save to the database.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <returns>True if the value is safe to save to the database.</returns>
        public override bool ValidateValueForSavingToDb(string value)
        {
            if (ParentJoinForeignKeyDefinition != null && value.IsNullOrEmpty())
            {
                return true;
            }
            return base.ValidateValueForSavingToDb(value);
        }
    }
}
