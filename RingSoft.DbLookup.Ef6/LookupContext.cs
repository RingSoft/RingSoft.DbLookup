using System;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;

namespace RingSoft.DbLookup.Ef6
{
    /// <summary>
    /// A LookupContextBase derived object that is compatible with the Entity Framework 6.
    /// </summary>
    /// <seealso cref="LookupContextBase" />
    public abstract class LookupContext : LookupContextBase
    {
        /// <summary>
        /// Gets the Entity Framework 6 database context used to set up the table and field definition properties of derived classes.
        /// </summary>
        /// <value>
        /// The database context.
        /// </value>
        protected abstract DbContext DbContext { get; }

        /// <summary>
        /// Derived classes use this to set table and field definition properties not automatically set up by this class.
        /// </summary>
        protected abstract void SetupModel();

        protected override void EfInitializeTableDefinitions()
        {
        }

        protected override void EfInitializeFieldDefinitions()
        {
            var objectContext = ((IObjectContextAdapter)DbContext).ObjectContext;
            var container = objectContext.MetadataWorkspace.GetEntityContainer(objectContext.DefaultContainerName,
                DataSpace.CSpace);

            foreach (var tableDefinition in TableDefinitions)
            {
                var tableProcessingArgs = new TableProcessingArgs(tableDefinition, $"Processing {tableDefinition} Fields");
                OnTableProcessing(tableProcessingArgs);
                var entitySet = container.EntitySets.FirstOrDefault(f => f.ElementType.Name == tableDefinition.EntityName);
                if (entitySet != null)
                    InitializeFields(entitySet, tableDefinition);
            }

            SetupModel();
        }

        private void InitializeFields(EntitySet entitySet, TableDefinitionBase tableDefinition)
        {
            var properties = entitySet.ElementType.DeclaredProperties;
            foreach (var property in properties)
            {
                var fieldDefinition =
                    tableDefinition.FieldDefinitions.FirstOrDefault(f => f.PropertyName == property.Name);
                if (fieldDefinition != null)
                {
                    fieldDefinition.IsRequired(!property.Nullable);
                    if (fieldDefinition is StringFieldDefinition stringField)
                    {
                        var maxLength = property.MaxLength;
                        stringField.HasMaxLength(maxLength ?? 0);
                    }
                }
            }

            foreach (var navigationProperty in entitySet.ElementType.NavigationProperties)
            {
                InitializeForeignKeys(navigationProperty, tableDefinition);
            }
        }

        private void InitializeForeignKeys(NavigationProperty navigationProperty, TableDefinitionBase foreignTableDefinition)
        {
            var primaryTableDefinition =
                foreignTableDefinition.Context.TableDefinitions.FirstOrDefault(t =>
                    t.EntityName == navigationProperty.TypeUsage.EdmType.Name);
            if (primaryTableDefinition != null)
            {
                var dependentProperties = navigationProperty.GetDependentProperties();
                var primaryIndex = 0;
                ForeignKeyDefinition foreignKeyDefinition = null;
                foreach (var dependentProperty in dependentProperties)
                {
                    var foreignFieldDefinition = 
                        foreignTableDefinition.FieldDefinitions.FirstOrDefault(
                        f => f.PropertyName == dependentProperty.Name);
                    var primaryFieldDefinition = primaryTableDefinition.PrimaryKeyFields[primaryIndex];

                    if (foreignFieldDefinition != null)
                    {
                        if (foreignKeyDefinition == null)
                        {
                            foreignKeyDefinition = foreignFieldDefinition.SetParentField(primaryFieldDefinition, navigationProperty.Name);
                        }
                        else
                        {
                            foreignKeyDefinition.AddFieldJoin(primaryFieldDefinition, foreignFieldDefinition);
                        }
                    }

                    primaryIndex++;
                }
            }
        }

        protected override void EfInitializePrimaryKeys()
        {
            var objectContext = ((IObjectContextAdapter)DbContext).ObjectContext;
            var container = objectContext.MetadataWorkspace.GetEntityContainer(objectContext.DefaultContainerName,
                DataSpace.CSpace);

            foreach (var tableDefinition in TableDefinitions)
            {
                var entitySet = container.EntitySets.FirstOrDefault(f => f.ElementType.Name == tableDefinition.EntityName);
                if (entitySet == null)
                {
                    throw new Exception($"Table Definition '{tableDefinition}' not found as a DbSet in the DbContext object.");
                }

                SetupEntityPrimaryKey(entitySet, tableDefinition);
            }
        }

        private void SetupEntityPrimaryKey(EntitySet entitySet, TableDefinitionBase tableDefinition)
        {
            foreach (var keyMember in entitySet.ElementType.KeyMembers)
            {
                var fieldDefinition =
                    tableDefinition.FieldDefinitions.FirstOrDefault(p => p.PropertyName == keyMember.Name);
                tableDefinition.AddFieldToPrimaryKey(fieldDefinition);
            }
        }
    }
}
