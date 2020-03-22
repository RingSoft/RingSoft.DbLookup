using RingSoft.DbLookupCore.QueryBuilder;

namespace RingSoft.DbLookupCore.ModelDefinition.FieldDefinitions
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
        /// Gets the type of the value for use in queries.
        /// </summary>
        /// <value>
        /// The type of the value.
        /// </value>
        public override ValueTypes ValueType => ValueTypes.Bool;

        /// <summary>
        /// Gets the true text.
        /// </summary>
        /// <value>
        /// The true text.
        /// </value>
        public string TrueText { get; private set; } = "True";

        /// <summary>
        /// Gets the false text.
        /// </summary>
        /// <value>
        /// The false text.
        /// </value>
        public string FalseText { get; private set; } = "False";

        /// <summary>
        /// Sets the displayed text for True and False values.
        /// </summary>
        /// <param name="trueText">The true text.</param>
        /// <param name="falseText">The false text.</param>
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
