namespace RingSoft.DbLookup.ModelDefinition.FieldDefinitions
{
    /// <summary>
    /// A generic field definition type class.
    /// </summary>
    /// <typeparam name="TFieldDefinition">A class that derives from this class.  Used in fluent API methods.</typeparam>
    /// <seealso cref="FieldDefinition" />
    public abstract class FieldDefinitionType<TFieldDefinition> : FieldDefinition  where TFieldDefinition : FieldDefinitionType<TFieldDefinition>
    {
        internal FieldDefinitionType()
        {
            
        }

        /// <summary>
        /// Sets the name of the field.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns></returns>
        public new TFieldDefinition HasFieldName(string fieldName)
        {
            base.HasFieldName(fieldName);
            return (TFieldDefinition)this;
        }

        /// <summary>
        /// Sets the field description.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <returns>This object.</returns>
        public new TFieldDefinition HasDescription(string description)
        {
            base.HasDescription(description);
            return (TFieldDefinition) this;
        }

        /// <summary>
        /// Sets the parent foreign key definition.
        /// </summary>
        /// <param name="parentFieldDefinition">The parent field definition.</param>
        /// <returns>This object.</returns>
        public new TFieldDefinition SetParentField(FieldDefinition parentFieldDefinition)
        {
            base.SetParentField(parentFieldDefinition);
            return (TFieldDefinition)this;
        }

        /// <summary>
        /// Determines whether this field will allow nulls.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns></returns>
        public new TFieldDefinition IsRequired(bool value = true)
        {
            base.IsRequired(value);
            return (TFieldDefinition) this;
        }
    }
}
