namespace RingSoft.DbLookup.ModelDefinition.FieldDefinitions
{
    /// <summary>
    /// An enumerator field definition.
    /// </summary>
    /// <seealso cref="DateFieldDefinition" />
    public sealed class EnumFieldDefinition : FieldDefinitionType<EnumFieldDefinition>

    {
        public override FieldDataTypes FieldDataType => FieldDataTypes.Enum;
        
        /// <summary>
        /// Gets the enum translation object which maps an enumerator value to its associated description.
        /// </summary>
        /// <value>
        /// The enum translation.
        /// </value>
        public EnumFieldTranslation EnumTranslation { get; internal set; }

        internal EnumFieldDefinition()
        {
            
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
