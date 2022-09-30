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
        /// <value>
        /// The type of the field data.
        /// </value>
        public override FieldDataTypes FieldDataType => FieldDataTypes.Bool;

        /// <summary>
        /// Gets the displayed true value text.
        /// </summary>
        /// <value>
        /// The true text.
        /// </value>
        public string TrueText { get; private set; } = "True";

        /// <summary>
        /// Gets the displayed false value text.
        /// </summary>
        /// <value>
        /// The false text.
        /// </value>
        public string FalseText { get; private set; } = "False";

        public EnumFieldTranslation EnumField { get; private set; }

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
        /// <returns></returns>
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
        /// <returns>
        /// The formatted value.
        /// </returns>
        public override string FormatValue(string value)
        {
            return GblMethods.FormatValue(FieldDataType, value);
        }
    }
}
