using System;
using System.Linq;
using RingSoft.DbLookup.QueryBuilder;

namespace RingSoft.DbLookup.ModelDefinition.FieldDefinitions
{
    /// <summary>
    /// A database field definition.
    /// </summary>
    public abstract class FieldDefinition
    {
        /// <summary>
        /// Gets the table definition.
        /// </summary>
        /// <value>
        /// The table definition.
        /// </value>
        public TableDefinitionBase TableDefinition { get; internal set; }

        /// <summary>
        /// Gets the name of the field.
        /// </summary>
        /// <value>
        /// The name of the field.
        /// </value>
        public string FieldName { get; internal set; }

        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        /// <value>
        /// The name of the property.
        /// </value>
        public string PropertyName { get; internal set; }

        /// <summary>
        /// Gets the type of the field data.
        /// </summary>
        /// <value>
        /// The type of the field data.
        /// </value>
        public abstract FieldDataTypes FieldDataType { get; }

        /// <summary>
        /// Gets the type of the value for use in queries.
        /// </summary>
        /// <value>
        /// The type of the value.
        /// </value>
        public abstract ValueTypes ValueType { get; }

        /// <summary>
        /// Gets the parent join foreign key definition.
        /// </summary>
        /// <value>
        /// The parent join foreign key definition.
        /// </value>
        public ForeignKeyDefinition ParentJoinForeignKeyDefinition { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether [allow nulls].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [allow nulls]; otherwise, <c>false</c>.
        /// </value>
        public bool AllowNulls { get; internal set; }

        /// <summary>
        /// Gets the field description that the user will see.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; internal set; }

        internal FieldDefinition()
        {
            AllowNulls = true;
        }

        /// <summary>
        /// Sets the name of the field.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns>This object.</returns>
        public FieldDefinition HasFieldName(string fieldName)
        {
            var field = TableDefinition.FieldDefinitions.FirstOrDefault(f => f.FieldName == fieldName);
            if (field != null && field != this)
            {
                throw new ArgumentException($"Field name '{fieldName}' already exists in this table.");
            }
            FieldName = fieldName;
            return this;
        }

        /// <summary>
        /// Sets the field description.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <returns>This object.</returns>
        public FieldDefinition HasDescription(string description)
        {
            Description = description;
            return this;
        }

        /// <summary>
        /// Sets the parent foreign key definition.
        /// </summary>
        /// <param name="parentFieldDefinition">The parent field definition.</param>
        /// <returns>This object.</returns>
        public ForeignKeyDefinition SetParentField(FieldDefinition parentFieldDefinition)
        {
            return SetParentField(parentFieldDefinition, string.Empty);
        }

        /// <summary>
        /// Sets the parent foreign key definition.
        /// </summary>
        /// <param name="parentFieldDefinition">The parent field definition.</param>
        /// <param name="propertyName">The object's property name</param>
        /// <returns>This object.</returns>
        public ForeignKeyDefinition SetParentField(FieldDefinition parentFieldDefinition, string propertyName)
        {
            if (ParentJoinForeignKeyDefinition == null)
            {
                ParentJoinForeignKeyDefinition = new ForeignKeyDefinition
                {
                    PrimaryTable = parentFieldDefinition.TableDefinition,
                    ForeignTable = TableDefinition,
                    ForeignObjectPropertyName = propertyName
                };
                ParentJoinForeignKeyDefinition.AddFieldJoin(parentFieldDefinition, this);
            }
            else
            {
                throw new Exception(
                    $"Table Definition '{TableDefinition.TableName}' already has a parent join defined.  You need to add a field to the ParentJoinForeignKey object instead.");
            }

            return ParentJoinForeignKeyDefinition;
        }

        /// <summary>
        /// Determines whether this field will allow nulls.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns></returns>
        public FieldDefinition IsRequired(bool value = true)
        {
            AllowNulls = !value;
            return this;
        }

        public override string ToString()
        {
            if (!Description.IsNullOrEmpty())
                return Description;

            if (ParentJoinForeignKeyDefinition != null)
                if (!ParentJoinForeignKeyDefinition.ForeignObjectPropertyName.IsNullOrEmpty())
                    return ParentJoinForeignKeyDefinition.ForeignObjectPropertyName;

            if (!PropertyName.IsNullOrEmpty())
                return PropertyName;

            return FieldName;
        }

        /// <summary>
        /// Formats the value to display.
        /// </summary>
        /// <param name="value">The value from the database.</param>
        /// <returns>The formatted value.</returns>
        public abstract string FormatValue(string value);

        /// <summary>
        /// Validates a value to see if it's a valid value to save to the database.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <returns>True if the value is safe to save to the database.</returns>
        public virtual bool ValidateValueForSavingToDb(string value)
        {
            if (!AllowNulls && value.IsNullOrEmpty())
                return false;

            return true;
        }
    }
}
