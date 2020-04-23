using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;

namespace RingSoft.DbLookup.ModelDefinition
{
    /// <summary>
    /// An table definition used in the Entity Framework.
    /// </summary>
    /// <typeparam name="TEntity">A database table class used in the Entity Framework.</typeparam>
    /// <seealso cref="TableDefinitionBase" />
    public sealed class TableDefinition<TEntity> : TableDefinitionBase where TEntity : new()
    {
        internal TableDefinition(LookupContextBase context, string tablePropertyName)
        {
            Context = context;
            context.AddTable(this);

            EntityName = typeof(TEntity).Name;
            FullEntityName = typeof(TEntity).FullName;

            TableName = GetTableNameOfEntity(typeof(TEntity));
            if (TableName.IsNullOrEmpty())
                TableName = tablePropertyName;

            foreach (var propertyInfo in typeof(TEntity).GetProperties())
            {
                FieldDefinition field = null;
                var fieldName = GetFieldNameOfProperty(propertyInfo);
                if (propertyInfo.PropertyType == typeof(DateTime)
                    || propertyInfo.PropertyType == typeof(DateTime?))
                {
                    field = AddDateField(fieldName);
                }
                else if (propertyInfo.PropertyType.BaseType == typeof(Enum))
                {
                    var enumFieldTranslation = new EnumFieldTranslation();
                    enumFieldTranslation.LoadFromEnum(propertyInfo.PropertyType);
                    field = AddEnumField(fieldName, enumFieldTranslation);
                }
                else if (propertyInfo.PropertyType == typeof(string))
                {
                    field = AddStringField(fieldName);
                }
                else if (propertyInfo.PropertyType == typeof(decimal)
                         || propertyInfo.PropertyType == typeof(decimal?)
                         || propertyInfo.PropertyType == typeof(double)
                         || propertyInfo.PropertyType == typeof(double?)
                         || propertyInfo.PropertyType == typeof(float)
                         || propertyInfo.PropertyType == typeof(float?))
                {
                    field = AddDecimalField(fieldName);
                }
                else if (propertyInfo.PropertyType == typeof(int)
                         || propertyInfo.PropertyType == typeof(int?)
                         || propertyInfo.PropertyType == typeof(long)
                         || propertyInfo.PropertyType == typeof(long?)
                         || propertyInfo.PropertyType == typeof(byte)
                         || propertyInfo.PropertyType == typeof(byte?)
                         || propertyInfo.PropertyType == typeof(short)
                         || propertyInfo.PropertyType == typeof(short?))
                {
                    field = AddIntegerField(fieldName);
                }
                else if (propertyInfo.PropertyType == typeof(bool))
                {
                    field = AddBoolField(fieldName);
                }
                else if (propertyInfo.PropertyType.IsValueType)
                {
                    throw new ArgumentException($"Property type {propertyInfo.PropertyType.Name} is not supported by this library.");
                }

                if (field != null)
                    field.PropertyName = propertyInfo.Name;
            }
        }

        private string GetTableNameOfEntity(Type entityType)
        {
            var tableName = string.Empty;

            var attributes = (TableAttribute[])entityType.GetCustomAttributes(typeof(TableAttribute), false);
            return attributes.Length > 0 ? attributes[0].Name : tableName;
        }


        private string GetFieldNameOfProperty(PropertyInfo propertyInfo)
        {
            var fieldName = propertyInfo.Name;

            var attributes = (ColumnAttribute[])propertyInfo.GetCustomAttributes(typeof(ColumnAttribute), false);

            if (attributes.Length > 0)
            {
                var attribute = attributes[0];
                if (attribute.Name.IsNullOrEmpty())
                    return fieldName;

                return attribute.Name;
            }
            else
            {
                return fieldName;
            }
        }

        #region Get Field Definitions

        /// <summary>
        /// Gets the string field definition for the property.
        /// </summary>
        /// <param name="entityProperty">The entity property.</param>
        /// <returns>The string field definition of the property.</returns>
        public StringFieldDefinition GetFieldDefinition(Expression<Func<TEntity, string>> entityProperty)
        {
            return GetPropertyField(entityProperty.GetFullPropertyName()) as StringFieldDefinition;
        }

        /// <summary>
        /// Gets the date field definition for the property.
        /// </summary>
        /// <param name="entityProperty">The entity property.</param>
        /// <returns>The date field definition of the property.</returns>
        public DateFieldDefinition GetFieldDefinition(Expression<Func<TEntity, DateTime>> entityProperty)
        {
            return GetPropertyField(entityProperty.GetFullPropertyName()) as DateFieldDefinition;
        }


        /// <summary>
        /// Gets the date field definition for the property.
        /// </summary>
        /// <param name="entityProperty">The entity property.</param>
        /// <returns>The date field definition of the property.</returns>
        public DateFieldDefinition GetFieldDefinition(Expression<Func<TEntity, DateTime?>> entityProperty)
        {
            return GetPropertyField(entityProperty.GetFullPropertyName()) as DateFieldDefinition;
        }

        /// <summary>
        /// Gets the enumerator field definition for the property.
        /// </summary>
        /// <param name="entityProperty">The entity property.</param>
        /// <returns>The enumerator field definition of the property.</returns>
        public EnumFieldDefinition GetFieldDefinition(Expression<Func<TEntity, Enum>> entityProperty)
        {
            return GetPropertyField(entityProperty.GetFullPropertyName()) as EnumFieldDefinition;
        }

        /// <summary>
        /// Gets the whole number field definition for the property.
        /// </summary>
        /// <param name="entityProperty">The entity property.</param>
        /// <returns>The integer field definition of the property.</returns>
        public IntegerFieldDefinition GetFieldDefinition(Expression<Func<TEntity, int>> entityProperty)
        {
            return GetPropertyField(entityProperty.GetFullPropertyName()) as IntegerFieldDefinition;
        }

        /// <summary>
        /// Gets the whole number field definition for the property.
        /// </summary>
        /// <param name="entityProperty">The entity property.</param>
        /// <returns>The integer field definition of the property.</returns>
        public IntegerFieldDefinition GetFieldDefinition(Expression<Func<TEntity, int?>> entityProperty)
        {
            return GetPropertyField(entityProperty.GetFullPropertyName()) as IntegerFieldDefinition;
        }

        /// <summary>
        /// Gets the whole number field definition for the property.
        /// </summary>
        /// <param name="entityProperty">The entity property.</param>
        /// <returns>The integer field definition of the property.</returns>
        public IntegerFieldDefinition GetFieldDefinition(Expression<Func<TEntity, long>> entityProperty)
        {
            return GetPropertyField(entityProperty.GetFullPropertyName()) as IntegerFieldDefinition;
        }

        /// <summary>
        /// Gets the whole number field definition for the property.
        /// </summary>
        /// <param name="entityProperty">The entity property.</param>
        /// <returns>The integer field definition of the property.</returns>
        public IntegerFieldDefinition GetFieldDefinition(Expression<Func<TEntity, long?>> entityProperty)
        {
            return GetPropertyField(entityProperty.GetFullPropertyName()) as IntegerFieldDefinition;
        }

        /// <summary>
        /// Gets the whole number field definition for the property.
        /// </summary>
        /// <param name="entityProperty">The entity property.</param>
        /// <returns>The integer field definition of the property.</returns>
        public IntegerFieldDefinition GetFieldDefinition(Expression<Func<TEntity, short>> entityProperty)
        {
            return GetPropertyField(entityProperty.GetFullPropertyName()) as IntegerFieldDefinition;
        }

        /// <summary>
        /// Gets the whole number field definition for the property.
        /// </summary>
        /// <param name="entityProperty">The entity property.</param>
        /// <returns>The integer field definition of the property.</returns>
        public IntegerFieldDefinition GetFieldDefinition(Expression<Func<TEntity, short?>> entityProperty)
        {
            return GetPropertyField(entityProperty.GetFullPropertyName()) as IntegerFieldDefinition;
        }

        /// <summary>
        /// Gets the whole number field definition for the property.
        /// </summary>
        /// <param name="entityProperty">The entity property.</param>
        /// <returns>The integer field definition of the property.</returns>
        public IntegerFieldDefinition GetFieldDefinition(Expression<Func<TEntity, byte>> entityProperty)
        {
            return GetPropertyField(entityProperty.GetFullPropertyName()) as IntegerFieldDefinition;
        }

        /// <summary>
        /// Gets the whole number field definition for the property.
        /// </summary>
        /// <param name="entityProperty">The entity property.</param>
        /// <returns>The integer field definition of the property.</returns>
        public IntegerFieldDefinition GetFieldDefinition(Expression<Func<TEntity, byte?>> entityProperty)
        {
            return GetPropertyField(entityProperty.GetFullPropertyName()) as IntegerFieldDefinition;
        }

        /// <summary>
        /// Gets the decimal number field definition for the property.
        /// </summary>
        /// <param name="entityProperty">The entity property.</param>
        /// <returns>The Decimal field definition of the property.</returns>
        public DecimalFieldDefinition GetFieldDefinition(Expression<Func<TEntity, decimal>> entityProperty)
        {
            return GetPropertyField(entityProperty.GetFullPropertyName()) as DecimalFieldDefinition;
        }

        /// <summary>
        /// Gets the decimal number field definition for the property.
        /// </summary>
        /// <param name="entityProperty">The entity property.</param>
        /// <returns>The Decimal field definition of the property.</returns>
        public DecimalFieldDefinition GetFieldDefinition(Expression<Func<TEntity, decimal?>> entityProperty)
        {
            return GetPropertyField(entityProperty.GetFullPropertyName()) as DecimalFieldDefinition;
        }

        /// <summary>
        /// Gets the decimal number field definition for the property.
        /// </summary>
        /// <param name="entityProperty">The entity property.</param>
        /// <returns>The Decimal field definition of the property.</returns>
        public DecimalFieldDefinition GetFieldDefinition(Expression<Func<TEntity, double>> entityProperty)
        {
            return GetPropertyField(entityProperty.GetFullPropertyName()) as DecimalFieldDefinition;
        }

        /// <summary>
        /// Gets the decimal number field definition for the property.
        /// </summary>
        /// <param name="entityProperty">The entity property.</param>
        /// <returns>The Decimal field definition of the property.</returns>
        public DecimalFieldDefinition GetFieldDefinition(Expression<Func<TEntity, double?>> entityProperty)
        {
            return GetPropertyField(entityProperty.GetFullPropertyName()) as DecimalFieldDefinition;
        }

        /// <summary>
        /// Gets the decimal number field definition for the property.
        /// </summary>
        /// <param name="entityProperty">The entity property.</param>
        /// <returns>The Decimal field definition of the property.</returns>
        public DecimalFieldDefinition GetFieldDefinition(Expression<Func<TEntity, float>> entityProperty)
        {
            return GetPropertyField(entityProperty.GetFullPropertyName()) as DecimalFieldDefinition;
        }

        /// <summary>
        /// Gets the decimal number field definition for the property.
        /// </summary>
        /// <param name="entityProperty">The entity property.</param>
        /// <returns>The Decimal field definition of the property.</returns>
        public DecimalFieldDefinition GetFieldDefinition(Expression<Func<TEntity, float?>> entityProperty)
        {
            return GetPropertyField(entityProperty.GetFullPropertyName()) as DecimalFieldDefinition;
        }

        /// <summary>
        /// Gets the boolean field definition for the property.
        /// </summary>
        /// <param name="entityProperty">The entity property.</param>
        /// <returns>The Decimal field definition of the property.</returns>
        public BoolFieldDefinition GetFieldDefinition(Expression<Func<TEntity, bool>> entityProperty)
        {
            return GetPropertyField(entityProperty.GetFullPropertyName()) as BoolFieldDefinition;
        }

        /// <summary>
        /// Gets the field definition associated with the property name.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Property '{propertyName}' not found.</exception>
        public FieldDefinition GetPropertyField(string propertyName)
        {
            var field = FieldDefinitions.FirstOrDefault(f => f.PropertyName == propertyName);
            if (field == null)
                throw new ArgumentException($"Property '{propertyName}' not found.");

            return field;
        }

        #endregion

        /// <summary>
        /// Adds the property to primary key.
        /// </summary>
        /// <param name="entityProperty">The entity property.</param>
        public TableDefinition<TEntity> AddPropertyToPrimaryKey(Expression<Func<TEntity, object>> entityProperty)
        {
            var field = GetPropertyField(entityProperty.GetFullPropertyName());
            AddFieldToPrimaryKey(field);
            return this;
        }

        /// <summary>
        /// Gets the entity from primary key value.
        /// </summary>
        /// <param name="primaryKeyValue">The primary key value.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">The passed in primary key value's table definition does not match this object</exception>
        public TEntity GetEntityFromPrimaryKeyValue(PrimaryKeyValue primaryKeyValue)
        {
            if (primaryKeyValue.TableDefinition != this)
                throw new ArgumentException("The passed in primary key value's table definition does not match this object");

            var entity = (TEntity)Activator.CreateInstance(typeof(TEntity));

            foreach (var keyValueField in primaryKeyValue.KeyValueFields)
            {
                GblMethods.SetPropertyValue(entity, keyValueField.FieldDefinition.PropertyName, keyValueField.Value);
            }

            return entity;
        }

        /// <summary>
        /// Gets the primary key value from entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public PrimaryKeyValue GetPrimaryKeyValueFromEntity(TEntity entity)
        {
            var primaryKeyValue = new PrimaryKeyValue(this);
            if (entity == null)
                return primaryKeyValue;

            foreach (var primaryKeyValueField in primaryKeyValue.KeyValueFields)
            {
                primaryKeyValueField.Value =
                    GblMethods.GetPropertyValue(entity, primaryKeyValueField.FieldDefinition.PropertyName);
            }
            return primaryKeyValue;
        }
    }
}
