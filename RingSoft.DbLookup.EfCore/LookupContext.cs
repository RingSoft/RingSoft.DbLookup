// ***********************************************************************
// Assembly         : RingSoft.DbLookup.EfCore
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-09-2023
// ***********************************************************************
// <copyright file="LookupContext.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.RecordLocking;
using RingSoft.DbLookup.TableProcessing;

namespace RingSoft.DbLookup.EfCore
{
    /// <summary>
    /// A LookupContextBase derived object that is compatible with the Entity Framework Core.
    /// </summary>
    /// <seealso cref="LookupContextBase" />
    public abstract class LookupContext : LookupContextBase, IAdvancedFindLookupContext
    {

        /// <summary>
        /// Gets the Entity Framework Core database context used to set up the table and field definition properties of inheriting classes.
        /// </summary>
        /// <value>The database context.</value>
        protected abstract DbContext DbContext { get; }

        /// <summary>
        /// The adv initalizing
        /// </summary>
        private bool _advInitalizing;

        //protected override void InitializeAdvFind()
        //{


        //    base.InitializeAdvFind();
        //}

        /// <summary>
        /// Has the Entity Framework platform set up this object's table and field properties based on DbContext's model setup.
        /// Derived classes constructor must execute this method.
        /// </summary>
        public override void Initialize()
        {
            if (_advInitalizing)
            {
                return;
            }

            var initialized = Initialized;
            base.Initialize();
            if (!initialized)
            {
                _advInitalizing = true;
                var configuration = new AdvancedFindLookupConfiguration(this);
                configuration.InitializeModel();
                configuration.ConfigureLookups();
                _advInitalizing = false;
            }
        }

        /// <summary>
        /// Efs the initialize table definitions.
        /// </summary>
        /// <exception cref="System.Exception">DbContext must be instantiated before initialization.</exception>
        /// <exception cref="System.Exception">Table Definition '{tableDefinition}' is not a DbSet in the DbContext class.</exception>
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


        /// <summary>
        /// Efs the initialize field definitions.
        /// </summary>
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

        /// <summary>
        /// Initializes the fields.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="tableDefinition">The table definition.</param>
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

        /// <summary>
        /// Initializes the property.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="fieldDefinition">The field definition.</param>
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
                                    foreignKeyDefinition.AddFieldJoin(primaryFieldDefinition, foreignFieldDefinition, false);
                            }
                        }
                    }

                    if (foreignKey.PrincipalToDependent != null)
                    {
                        foreignKeyDefinition.CollectionName = foreignKey.PrincipalToDependent.Name;
                    }
                    parentTable.ChildKeys.Add(foreignKeyDefinition);
                }
                //var primaryProperty = foreignKey.PrincipalKey.Properties[0];
            }
        }

        /// <summary>
        /// Efs the initialize primary keys.
        /// </summary>
        protected override void EfInitializePrimaryKeys()
        {
            foreach (var tableDefinition in TableDefinitions)
            {
                var entityType = DbContext.Model.FindEntityType(tableDefinition.FullEntityName);
                if (entityType != null)
                    SetupEntityPrimaryKey(entityType, tableDefinition);
            }
        }

        /// <summary>
        /// Setups the entity primary key.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="tableDefinition">The table definition.</param>
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

        /// <summary>
        /// Gets the lookup data maui.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="lookupDefinition">The lookup definition.</param>
        /// <returns>LookupDataMauiBase.</returns>
        public override LookupDataMauiBase GetLookupDataMaui<TEntity>(LookupDefinitionBase lookupDefinition) 
            where TEntity : class
        {
            var lookupMaui = new LookupDataMaui<TEntity>(lookupDefinition);
            return lookupMaui;
        }



        /// <summary>
        /// Gets the queryable.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="lookupDefinition">The lookup definition.</param>
        /// <param name="context">The context.</param>
        /// <returns>IQueryable&lt;TEntity&gt;.</returns>
        public override IQueryable<TEntity> GetQueryable<TEntity>(LookupDefinitionBase lookupDefinition
            , IDbContext context = null) where TEntity : class
        {
            if (context == null)
            {
                context = SystemGlobals.DataRepository.GetDataContext();
            }

            var tableDef = GblMethods.GetTableDefinition<TEntity>();

            //This causes too many includes in DevLogix Add/Edit Errors
            //var query = GetQueryableTable(tableDef, true, context);

            var query = context.GetTable<TEntity>();
            var navProperties = lookupDefinition.GetAllNavigationProperties();

            var includes = navProperties.GetAllIncludePropertiesFromNavProperties();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return query;
        }

        /// <summary>
        /// Gets the queryable table.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="tableDefinition">The table definition.</param>
        /// <param name="getRelatedEntities">if set to <c>true</c> [get related entities].</param>
        /// <param name="context">The context.</param>
        /// <returns>IQueryable&lt;TEntity&gt;.</returns>
        public override IQueryable<TEntity> GetQueryableTable<TEntity>(TableDefinition<TEntity> tableDefinition
            , bool getRelatedEntities, IDbContext context = null) where TEntity : class
        {
            if (context == null)
            {
                context = SystemGlobals.DataRepository.GetDataContext();
            }

            var query = context.GetTable<TEntity>();
            if (getRelatedEntities)
            {
                var includes = new List<string>();
                var parentObjects = tableDefinition
                    .FieldDefinitions
                    .Where(p => p.ParentJoinForeignKeyDefinition != null);

                foreach (var fieldDefinition in parentObjects)
                {
                    includes.AddRange(GetIncludes(fieldDefinition.ParentJoinForeignKeyDefinition));
                }

                var gridKeys = tableDefinition.ChildKeys
                    .Where(p => p.ForeignTable.HeaderTable == tableDefinition);

                foreach (var key in gridKeys)
                {
                    includes.Add(key.CollectionName);
                    parentObjects = key
                        .ForeignTable
                        .FieldDefinitions
                        .Where(p => p.ParentJoinForeignKeyDefinition != null
                                    && p.ParentJoinForeignKeyDefinition.PrimaryTable != tableDefinition);
                    foreach (var fieldDefinition in parentObjects)
                    {
                        includes.AddRange(GetIncludes(fieldDefinition
                                .ParentJoinForeignKeyDefinition
                            , key.CollectionName));
                    }
                }

                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return query;
        }

        /// <summary>
        /// Gets the includes.
        /// </summary>
        /// <param name="foreignKeyDefinition">The foreign key definition.</param>
        /// <param name="parentInclude">The parent include.</param>
        /// <returns>List&lt;System.String&gt;.</returns>
        public List<string> GetIncludes(ForeignKeyDefinition foreignKeyDefinition
            , string parentInclude = "")
        {
            var include = foreignKeyDefinition.ForeignObjectPropertyName;
            if (!parentInclude.IsNullOrEmpty())
            {
                include = $"{parentInclude}.{include}";
            }
            var result = new List<string>();
            result.Add(include);
            var parentObjects = foreignKeyDefinition.PrimaryTable
                .FieldDefinitions
                .Where(p => p.ParentJoinForeignKeyDefinition != null);
            foreach (var fieldDefinition in parentObjects)
            {
                if (fieldDefinition.AllowRecursion)
                {
                    result.AddRange(GetIncludes(fieldDefinition.ParentJoinForeignKeyDefinition
                        , include));
                }
                else
                {
                    parentInclude = include;
                    include = fieldDefinition.ParentJoinForeignKeyDefinition.ForeignObjectPropertyName;
                    if (!parentInclude.IsNullOrEmpty())
                    {
                        include = $"{parentInclude}.{include}";
                    }
                    result.Add(include);
                }
            }
            return result;
        }

        /// <summary>
        /// Gets the automatic fill data maui.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="setup">The setup.</param>
        /// <param name="control">The control.</param>
        /// <returns>AutoFillDataMauiBase.</returns>
        public override AutoFillDataMauiBase GetAutoFillDataMaui<TEntity>(AutoFillSetup setup, IAutoFillControl control)
        {
            var autoFillMaui = new AutoFillDataMaui<TEntity>(setup, control);
            return autoFillMaui;
        }
    }
}
