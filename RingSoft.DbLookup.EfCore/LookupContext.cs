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
        public TableDefinition<AdvancedFind.AdvancedFind> AdvancedFinds { get; set; }
        public TableDefinition<AdvancedFindColumn> AdvancedFindColumns { get; set; }
        public TableDefinition<AdvancedFindFilter> AdvancedFindFilters { get; set; }
        public LookupContextBase Context => this;
        public TableDefinition<RecordLock> RecordLocks { get; set; }

        public LookupDefinition<AdvancedFindLookup, AdvancedFind.AdvancedFind> AdvancedFindLookup { get; set; }
        public LookupDefinition<AdvancedFindLookup, AdvancedFindColumn> AdvancedFindColumnLookup { get; set; }
        public LookupDefinition<AdvFindFilterLookup, AdvancedFindFilter> AdvancedFindFilterLookup { get; set; }
        public LookupDefinition<RecordLockingLookup, RecordLock> RecordLockingLookup { get; set; }

        /// <summary>
        /// Gets the Entity Framework Core database context used to set up the table and field definition properties of inheriting classes.
        /// </summary>
        /// <value>
        /// The database context.
        /// </value>
        protected abstract DbContext DbContext { get; }

        private bool _advInitalizing;

        //protected override void InitializeAdvFind()
        //{
        

        //    base.InitializeAdvFind();
        //}

        public override void Initialize()
        {
            if (_advInitalizing)
            {
                return;
            }
            SystemGlobals.AdvancedFindLookupContext = this;
            SystemGlobals.LookupContext = this;
            base.Initialize();
            _advInitalizing = true;
            var configuration = new AdvancedFindLookupConfiguration(this);
            configuration.InitializeModel();
            configuration.ConfigureLookups();
            _advInitalizing = false;
        }

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

        public override LookupDataMauiBase GetLookupDataMaui<TEntity>(LookupDefinitionBase lookupDefinition) 
            where TEntity : class
        {
            var lookupMaui = new LookupDataMaui<TEntity>(lookupDefinition);
            return lookupMaui;
        }

        

        public override IQueryable<TEntity> GetQueryable<TEntity>(LookupDefinitionBase lookupDefinition
            , IDbContext context = null) where TEntity : class
        {
            if (context == null)
            {
                context = SystemGlobals.DataRepository.GetDataContext();
            }
            
            var query = context.GetTable<TEntity>();
            var navProperties = lookupDefinition.GetAllNavigationProperties();

            var includes = navProperties.GetAllIncludePropertiesFromNavProperties();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return query;
        }

        public override IQueryable<TEntity> GetQueryableTable<TEntity>(TableDefinition<TEntity> tableDefinition
            , bool getRelatedEntities, IDbContext context = null) where TEntity : class
        {
            if (context == null)
            {
                context = SystemGlobals.DataRepository.GetDataContext();
            }

            var query = context.GetTable<TEntity>();
            
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

            return query;
        }

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

        public override AutoFillDataMauiBase GetAutoFillDataMaui<TEntity>(AutoFillSetup setup, IAutoFillControl control)
        {
            var autoFillMaui = new AutoFillDataMaui<TEntity>(setup, control);
            return autoFillMaui;
        }
    }
}
