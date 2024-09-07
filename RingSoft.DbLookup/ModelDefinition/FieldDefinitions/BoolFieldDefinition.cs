// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-16-2023
// ***********************************************************************
// <copyright file="BoolFieldDefinition.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DataEntryControls.Engine;

namespace RingSoft.DbLookup.ModelDefinition.FieldDefinitions
{
    /// <summary>
    /// A boolean (True/False) field definition.
    /// </summary>
    public sealed class BoolFieldDefinition : FieldDefinitionType<BoolFieldDefinition>
    {
        /// <summary>
        /// Gets the type of the field data.
        /// </summary>
        /// <value>The type of the field data.</value>
        public override FieldDataTypes FieldDataType => FieldDataTypes.Bool;

        /// <summary>
        /// Gets the displayed true value text.
        /// </summary>
        /// <value>The true text.</value>
        public string TrueText { get; private set; } = "True";

        /// <summary>
        /// Gets the displayed false value text.
        /// </summary>
        /// <value>The false text.</value>
        public string FalseText { get; private set; } = "False";

        /// <summary>
        /// Gets the enum field.
        /// </summary>
        /// <value>The enum field.</value>
        public EnumFieldTranslation EnumField { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BoolFieldDefinition" /> class.
        /// </summary>
        public BoolFieldDefinition()
        {
            EnumField = new EnumFieldTranslation();
            EnumField.LoadFromBoolean("True", "False");
        }
        /// <summary>
        /// Sets the displayed text for True and False values.
        /// </summary>
        /// <param name="trueText">The new True text.</param>
        /// <param name="falseText">The new False text.</param>
        /// <returns>BoolFieldDefinition.</returns>
        public BoolFieldDefinition HasTrueFalseText(string trueText, string falseText)
        {
            TrueText = trueText;
            FalseText = falseText;
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
        /// Gets the user value.
        /// </summary>
        /// <param name="dbIdValue">The database identifier value.</param>
        /// <returns>System.String.</returns>
        public override string GetUserValue(string dbIdValue)
        {
            var boolValue = dbIdValue.ToBool();
            if (boolValue)
            {
                return "True";
            }

            return "False";
        }
    }
}
