using System;
using System.Linq;
using System.Linq.Expressions;
using RingSoft.DbLookup.ModelDefinition;

namespace RingSoft.DbLookup.Lookup
{
    /// <summary>
    /// A join to an entity definition.
    /// </summary>
    /// <typeparam name="TLookupEntity">The type of the lookup entity.</typeparam>
    /// <typeparam name="TEntity">The entity.</typeparam>
    /// <typeparam name="TRelatedEntity">The type of the related entity.</typeparam>
    /// <seealso cref="LookupJoin" />
    public class LookupJoinTableEntity<TLookupEntity, TEntity, TRelatedEntity>  : LookupJoin
        where TLookupEntity : new() where TEntity : new() where TRelatedEntity : class
    {
        private LookupDefinition<TLookupEntity, TEntity> _lookupEntityDefinition;

        internal LookupJoinTableEntity(LookupDefinition<TLookupEntity, TEntity> lookupEntityDefinition,
            TableDefinitionBase tableDefinition, string propertyName, string propertyType) : base(lookupEntityDefinition)
        {
            _lookupEntityDefinition = lookupEntityDefinition;
            SetJoinDefinition(tableDefinition, propertyName, propertyType);
        }

        private LookupJoinTableEntity(LookupDefinition<TLookupEntity, TEntity> lookupEntityDefinition) : base(lookupEntityDefinition)
        {
            _lookupEntityDefinition = lookupEntityDefinition;
        }

        private void SetJoinDefinition(TableDefinitionBase tableDefinition, string propertyName, string propertyType)
        {
            var foreignFieldDefinition = tableDefinition.FieldDefinitions.FirstOrDefault(f => f.ParentJoinForeignKeyDefinition != null &&
                                                                                              f.ParentJoinForeignKeyDefinition.ForeignObjectPropertyName == propertyName);

            if (foreignFieldDefinition == null)
            {
                var propertyTable =
                    tableDefinition.Context.TableDefinitions.FirstOrDefault(t => t.EntityName == propertyType);

                if (propertyTable == null)
                    throw new ArgumentException($"Property type '{propertyType}' is not setup as a Table Definition in the Lookup Context.");

                throw new ArgumentException($"Property '{propertyName}' was not configured by the Entity Framework.");
            }

            SetJoinDefinition(foreignFieldDefinition);
        }

        /// <summary>
        /// Adds a visible column definition.
        /// </summary>
        /// <param name="lookupEntityProperty">The lookup entity property.</param>
        /// <param name="entityProperty">The entity property.</param>
        /// <returns></returns>
        public LookupFieldColumnDefinition AddVisibleColumnDefinition(
            Expression<Func<TLookupEntity, object>> lookupEntityProperty,
            Expression<Func<TRelatedEntity, object>> entityProperty)
        {
            return AddVisibleColumnDefinition(lookupEntityProperty, string.Empty, entityProperty, 0);
        }

        /// <summary>
        /// Adds a visible column definition.
        /// </summary>
        /// <param name="lookupEntityProperty">The lookup entity property.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="entityProperty">The entity property.</param>
        /// <param name="percentWidth">The percent of the lookup's total width.</param>
        /// <returns></returns>
        public LookupFieldColumnDefinition AddVisibleColumnDefinition(Expression<Func<TLookupEntity, object>> lookupEntityProperty,
            string caption, Expression<Func<TRelatedEntity, object>> entityProperty, double percentWidth)
        {
            var parentTable = JoinDefinition.ForeignKeyDefinition.PrimaryTable;
            var relatedPropertyName = entityProperty.GetFullPropertyName();
            var relatedField = parentTable.FieldDefinitions.FirstOrDefault(f => f.PropertyName == relatedPropertyName);

            var columnDefinition = AddVisibleColumnDefinition(caption, relatedField, percentWidth);
            //columnDefinition.ChildField = relatedField;
            columnDefinition.ChildField = JoinDefinition.ForeignKeyDefinition.FieldJoins[0].ForeignField;
            columnDefinition.PropertyName = lookupEntityProperty.GetFullPropertyName();

            return columnDefinition;
        }

        /// <summary>
        /// Adds a hidden column.
        /// </summary>
        /// <param name="lookupEntityProperty">The lookup entity property.</param>
        /// <param name="entityProperty">The entity property.</param>
        /// <returns></returns>
        public LookupFieldColumnDefinition AddHiddenColumn(Expression<Func<TLookupEntity, object>> lookupEntityProperty,
            Expression<Func<TRelatedEntity, object>> entityProperty)
        {
            var parentTable = JoinDefinition.ForeignKeyDefinition.PrimaryTable;
            var relatedPropertyName = entityProperty.GetFullPropertyName();
            var relatedField = parentTable.FieldDefinitions.FirstOrDefault(f => f.PropertyName == relatedPropertyName);

            var columnDefinition = AddHiddenColumn(relatedField);
            columnDefinition.PropertyName = lookupEntityProperty.GetFullPropertyName();

            return columnDefinition;
        }

        /// <summary>
        /// Includes the specified related property.
        /// </summary>
        /// <typeparam name="TParentRelatedEntity">The type of the parent related entity.</typeparam>
        /// <param name="relatedProperty">The related property.</param>
        /// <returns></returns>
        public LookupJoinTableEntity<TLookupEntity, TEntity, TParentRelatedEntity> Include<TParentRelatedEntity>(Expression<Func<TRelatedEntity, TParentRelatedEntity>> relatedProperty)
            where TParentRelatedEntity : class

        {
            var parentTable = JoinDefinition.ForeignKeyDefinition.PrimaryTable;
            var relatedPropertyName = relatedProperty.GetFullPropertyName();
            var relatedPropertyType = relatedProperty.ReturnType.Name;

            var returnEntity = new LookupJoinTableEntity<TLookupEntity, TEntity, TParentRelatedEntity>(_lookupEntityDefinition);
            returnEntity.JoinDefinition = JoinDefinition;
            returnEntity.SetJoinDefinition(parentTable, relatedPropertyName, relatedPropertyType);
            
            return returnEntity;
        }
    }
}
