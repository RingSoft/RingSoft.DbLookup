﻿using System;
using System.Linq;
using System.Linq.Expressions;
using RingSoft.DbLookup.ModelDefinition;

namespace RingSoft.DbLookup.Lookup
{
    /// <summary>
    /// A join to an entity definition.
    /// </summary>
    /// <typeparam name="TLookupEntity">The type of the lookup entity.</typeparam>
    /// <typeparam name="TRelatedEntity">The type of the related entity.</typeparam>
    /// <seealso cref="LookupJoin" />
    public class LookupJoinTableEntity<TLookupEntity, TRelatedEntity>  : LookupJoin
        where TLookupEntity : new() where TRelatedEntity : class
    {
        private LookupEntityDefinition<TLookupEntity> _lookupEntityDefinition;

        internal LookupJoinTableEntity(LookupEntityDefinition<TLookupEntity> lookupEntityDefinition,
            TableDefinitionBase tableDefinition, string propertyName) : base(lookupEntityDefinition)
        {
            _lookupEntityDefinition = lookupEntityDefinition;
            SetJoinDefinition(tableDefinition, propertyName);
        }

        private LookupJoinTableEntity(LookupEntityDefinition<TLookupEntity> lookupEntityDefinition) : base(lookupEntityDefinition)
        {
            _lookupEntityDefinition = lookupEntityDefinition;
        }

        private void SetJoinDefinition(TableDefinitionBase tableDefinition, string propertyName)
        {
            var foreignFieldDefinition = tableDefinition.FieldDefinitions.FirstOrDefault(f => f.ParentJoinForeignKeyDefinition != null &&
                                                                                              f.ParentJoinForeignKeyDefinition.ForeignObjectPropertyName == propertyName);

            if (foreignFieldDefinition == null)
                throw new ArgumentException($"Property '{propertyName}' was not configured by the Entity Framework.");

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
        public LookupJoinTableEntity<TLookupEntity, TParentRelatedEntity> Include<TParentRelatedEntity>(Expression<Func<TRelatedEntity, TParentRelatedEntity>> relatedProperty)
            where TParentRelatedEntity : class

        {
            var parentTable = JoinDefinition.ForeignKeyDefinition.PrimaryTable;
            var relatedPropertyName = relatedProperty.GetFullPropertyName();

            var returnEntity = new LookupJoinTableEntity<TLookupEntity, TParentRelatedEntity>(_lookupEntityDefinition);
            returnEntity.JoinDefinition = JoinDefinition;
            returnEntity.SetJoinDefinition(parentTable, relatedPropertyName);
            
            return returnEntity;
        }
    }
}
