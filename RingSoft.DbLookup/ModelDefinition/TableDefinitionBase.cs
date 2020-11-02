using System;
using System.Collections.Generic;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;

namespace RingSoft.DbLookup.ModelDefinition
{
    /// <summary>
    /// A database table definition.
    /// </summary>
    public abstract class TableDefinitionBase
    {
        /// <summary>
        /// Gets the context.
        /// </summary>
        /// <value>
        /// The context.
        /// </value>
        public LookupContextBase Context { get; internal set; }

        /// <summary>
        /// Gets the name of the table.
        /// </summary>
        /// <value>
        /// The name of the table.
        /// </value>
        public string TableName { get; internal set; }

        /// <summary>
        /// Gets the name of the entity.
        /// </summary>
        /// <value>
        /// The name of the entity.
        /// </value>
        public string EntityName { get; internal set; }

        /// <summary>
        /// Gets the full name of the entity.
        /// </summary>
        /// <value>
        /// The full name of the entity.
        /// </value>
        public string FullEntityName { get; internal set; }

        /// <summary>
        /// Gets the field definitions.
        /// </summary>
        /// <value>
        /// The field definitions.
        /// </value>
        public IReadOnlyList<FieldDefinition> FieldDefinitions => _fields;

        /// <summary>
        /// Gets the primary key fields.
        /// </summary>
        /// <value>
        /// The primary key fields.
        /// </value>
        public IReadOnlyList<FieldDefinition> PrimaryKeyFields => _primaryKeyFields;

        /// <summary>
        /// Gets the lookup definition.  Used in mapping foreign key fields to controls.
        /// </summary>
        /// <value>
        /// The lookup definition.
        /// </value>
        public LookupDefinitionBase LookupDefinition { get; private set; }

        /// <summary>
        /// Gets the table description that the user will see.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; internal set; }

        /// <summary>
        /// Gets the record description.
        /// </summary>
        /// <value>
        /// The record description.
        /// </value>
        public string RecordDescription { get; internal set; }


        private readonly List<FieldDefinition> _fields = new List<FieldDefinition>();
        private readonly List<FieldDefinition> _primaryKeyFields = new List<FieldDefinition>();

        public TableDefinitionBase(LookupContextBase context, string tableName)
        {
            Context = context;

            TableName = tableName;
            context.AddTable(this);
        }

        internal protected TableDefinitionBase()
        {
            
        }

        #region Field Definitions

        /// <summary>
        /// Adds a string field.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns>The added string field definition.</returns>
        public StringFieldDefinition AddStringField(string fieldName)
        {
            var field = new StringFieldDefinition();
            AddField(field, fieldName);
            return field;
        }

        /// <summary>
        /// Adds the date field.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns>The added dater field definition.</returns>
        public DateFieldDefinition AddDateField(string fieldName)
        {
            var field = new DateFieldDefinition();
            AddField(field, fieldName);
            return field;
        }

        /// <summary>
        /// Adds an enumerator field.
        /// </summary>
        /// <typeparam name="T">An enumerator class.</typeparam>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns>The added enum field definition.</returns>
        public EnumFieldDefinition AddEnumField<T>(string fieldName) where T : Enum
        {
            var enumFieldTranslation = new EnumFieldTranslation();
            enumFieldTranslation.LoadFromEnum<T>();
            return AddEnumField(fieldName, enumFieldTranslation);
        }

        internal EnumFieldDefinition AddEnumField(string fieldName, EnumFieldTranslation translation)
        {
            var field = new EnumFieldDefinition
            {
                EnumTranslation = translation
            };
            AddField(field, fieldName);
            return field;
        }

        /// <summary>
        /// Adds an integer (whole number) field.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns>The added integer field definition.</returns>
        public IntegerFieldDefinition AddIntegerField(string fieldName)
        {
            var field = new IntegerFieldDefinition();
            AddField(field, fieldName);
            return field;
        }

        /// <summary>
        /// Adds a decimal field.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns>The added decimal field definition.</returns>
        public DecimalFieldDefinition AddDecimalField(string fieldName)
        {
            var field = new DecimalFieldDefinition();
            AddField(field, fieldName);
            return field;
        }

        /// <summary>
        /// Adds a boolean field.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns>The added boolean field definition.</returns>
        public BoolFieldDefinition AddBoolField(string fieldName)
        {
            var field = new BoolFieldDefinition();
            AddField(field, fieldName);
            return field;
        }

        private void AddField(FieldDefinition field, string fieldName)
        {
            field.TableDefinition = this;
            field.FieldName = fieldName;
            _fields.Add(field);
        }

        #endregion

        /// <summary>
        /// Adds the field to primary key.
        /// </summary>
        /// <param name="fieldDefinition">The field definition.</param>
        public TableDefinitionBase AddFieldToPrimaryKey(FieldDefinition fieldDefinition)
        {
            _primaryKeyFields.Add(fieldDefinition);
            fieldDefinition.AllowNulls = false;
            return this;
        }

        public override string ToString()
        {
            if (!Description.IsNullOrEmpty())
                return Description;

            if (!EntityName.IsNullOrEmpty())
                return EntityName;

            return TableName;
        }

        /// <summary>
        /// Sets the name of the table.
        /// </summary>
        /// <param name="name">The database table name.</param>
        /// <returns></returns>
        public TableDefinitionBase HasTableName(string name)
        {
            TableName = name;
            return this;
        }

        /// <summary>
        /// Sets this table's default lookup definition.
        /// </summary>
        /// <param name="lookupDefinition">The lookup definition.</param>
        /// <returns></returns>
        public TableDefinitionBase HasLookupDefinition(LookupDefinitionBase lookupDefinition)
        {
            if (lookupDefinition.TableDefinition != this)
                throw new ArgumentException(
                    $"Lookup's table definition '{lookupDefinition.TableDefinition}' does not match this table definition. ({this})");
            LookupDefinition = lookupDefinition;
            return this;
        }

        /// <summary>
        /// Sets the table description.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <returns>This object.</returns>
        public TableDefinitionBase HasDescription(string description)
        {
            Description = description;
            return this;
        }

        /// <summary>
        /// Sets the description to describe an individual record name.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <returns></returns>
        public TableDefinitionBase HasRecordDescription(string description)
        {
            RecordDescription = description;
            return this;
        }
    }
}
