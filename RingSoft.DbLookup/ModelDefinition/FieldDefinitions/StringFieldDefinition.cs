using RingSoft.DbLookup.QueryBuilder;

namespace RingSoft.DbLookup.ModelDefinition.FieldDefinitions
{
    /// <summary>
    /// A string field definition.
    /// </summary>
    /// <seealso />
    public sealed class StringFieldDefinition : FieldDefinitionType<StringFieldDefinition>
    {
        public override FieldDataTypes FieldDataType => FieldDataTypes.String;

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
        /// <value>
        ///   <c>true</c> if this instance is memo; otherwise, <c>false</c>.
        /// </value>
        public bool MemoField { get; private set; }

        /// <summary>
        /// Gets the maximum length.
        /// </summary>
        /// <value>
        /// The maximum length.
        /// </value>
        public int MaxLength { get; private set; }

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
        /// <returns></returns>
        public StringFieldDefinition HasMaxLength(int value)
        {
            MaxLength = value;
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
