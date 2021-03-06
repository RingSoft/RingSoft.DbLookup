﻿using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;

namespace RingSoft.DbLookup.EfCore
{
    /// <summary>
    /// A LookupContextBase derived object that is compatible with the Entity Framework Core.
    /// </summary>
    /// <seealso cref="LookupContextBase" />
    public abstract class LookupContext : LookupContextBase
    {

        /// <summary>
        /// Gets the Entity Framework Core database context used to set up the table and field definition properties of inheriting classes.
        /// </summary>
        /// <value>
        /// The database context.
        /// </value>
        protected abstract DbContext DbContext { get; }

        protected override void EfInitializeTableDefinitions()
        {
            if (DbContext == null)
                throw new Exception("DbContext must be instantiated before initialization.");

            var dbSetName = $"{nameof(DbSet<object>)}`1";
            var properties = DbContext.GetType().GetProperties().Where(w => w.PropertyType.Name == dbSetName).ToList();
            foreach (var tableDefinition in TableDefinitions)
            {
                var entityType = DbContext.Model.FindEntityType(tableDefinition.FullEntityName);
                var dbSetExists = properties.Any(p =>
                    p.PropertyType.GenericTypeArguments.Any(g => g.Name == tableDefinition.EntityName));
                if (entityType == null || !dbSetExists)
                {
                    throw new Exception($"Table Definition '{tableDefinition}' is not a DbSet in the DbContext class.");
                }
                else
                {
                    tableDefinition.HasTableName(entityType.GetTableName());
                }
            }
        }


        protected override void EfInitializeFieldDefinitions()
        {
            foreach (var tableDefinition in TableDefinitions)
            {
                var entityType = DbContext.Model.FindEntityType(tableDefinition.FullEntityName);
                if (entityType != null)
                    InitializeFields(entityType, tableDefinition);
            }
            SetupModel();
        }

        private void InitializeFields(IEntityType entityType, TableDefinitionBase tableDefinition)
        {
            var properties = entityType.GetProperties();
            foreach (var property in properties)
            {
                var fieldDefinition =
                    tableDefinition.FieldDefinitions.FirstOrDefault(f => f.PropertyName == property.Name);
                if (fieldDefinition != null)
                {
                    InitializeProperty(property, fieldDefinition);
                }
            }
        }

        private void InitializeProperty(IProperty property, FieldDefinition fieldDefinition)
        {
            fieldDefinition.IsRequired(!property.IsNullable);
            fieldDefinition.HasFieldName(property.GetColumnName());
            if (fieldDefinition.FieldDataType == FieldDataTypes.String)
            {
                if (fieldDefinition is StringFieldDefinition stringField)
                {
                    var maxLength = property.GetMaxLength();
                    stringField.HasMaxLength(maxLength ?? 0);
                }
            }

            var foreignKeys = property.GetContainingForeignKeys();
            foreach (var foreignKey in foreignKeys)
            {
                var parentTable =
                    TableDefinitions.FirstOrDefault(t => t.EntityName == foreignKey.PrincipalEntityType.ClrType.Name);
                if (parentTable != null)
                {
                    ForeignKeyDefinition foreignKeyDefinition = null;
                    foreach (var foreignKeyProperty in foreignKey.Properties)
                    {
                        var principalProperty = foreignKeyProperty.FindFirstPrincipal();
                        var primaryFieldDefinition =
                            parentTable.FieldDefinitions.FirstOrDefault(f => f.PropertyName == principalProperty.Name);
                        if (primaryFieldDefinition != null)
                        {
                            if (foreignKeyDefinition == null)
                            {
                                foreignKeyDefinition = fieldDefinition.SetParentField(primaryFieldDefinition, foreignKey.DependentToPrincipal.Name);
                            }
                            else
                            {
                                var foreignFieldDefinition =
                                    fieldDefinition.TableDefinition.FieldDefinitions.FirstOrDefault(f =>
                                        f.PropertyName == foreignKeyProperty.Name);
                                if (foreignFieldDefinition != null)
                                    foreignKeyDefinition.AddFieldJoin(primaryFieldDefinition, foreignFieldDefinition);
                            }
                        }
                    }
                }
                //var primaryProperty = foreignKey.PrincipalKey.Properties[0];
            }
        }

        protected override void EfInitializePrimaryKeys()
        {
            foreach (var tableDefinition in TableDefinitions)
            {
                var entityType = DbContext.Model.FindEntityType(tableDefinition.FullEntityName);
                if (entityType != null)
                    SetupEntityPrimaryKey(entityType, tableDefinition);
            }
        }

        private void SetupEntityPrimaryKey(IEntityType entityType, TableDefinitionBase tableDefinition)
        {
            var primaryKey = entityType.FindPrimaryKey();
            if (primaryKey != null)
            {
                foreach (var primaryKeyProperty in primaryKey.Properties)
                {
                    var fieldDefinition =
                        tableDefinition.FieldDefinitions.FirstOrDefault(p => p.PropertyName == primaryKeyProperty.Name);
                    tableDefinition.AddFieldToPrimaryKey(fieldDefinition);
                }
            }
        }
    }
}
