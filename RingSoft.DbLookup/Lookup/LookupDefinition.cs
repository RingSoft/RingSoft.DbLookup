using System;
using System.Linq.Expressions;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.TableProcessing;

namespace RingSoft.DbLookup.Lookup
{
    /// <summary>
    /// Contains all the data necessary for a lookup.
    /// </summary>
    /// <typeparam name="TLookupEntity">The type of the lookup entity.</typeparam>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <seealso cref="LookupEntityDefinition{TLookupEntity}" />
    public class LookupDefinition<TLookupEntity, TEntity> 
        : LookupEntityDefinition<TLookupEntity>
        where TLookupEntity : new() where TEntity : new()
    {
        /// <summary>
        /// Gets the filter definition.
        /// </summary>
        /// <value>
        /// The filter definition.
        /// </value>
        public new TableFilterDefinition<TEntity> FilterDefinition { get; }

        /// <summary>
        /// Gets the table definition.
        /// </summary>
        /// <value>
        /// The table definition.
        /// </value>
        public new TableDefinition<TEntity> TableDefinition { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupDefinition{TLookupEntity, TEntity}"/> class.
        /// </summary>
        /// <param name="tableDefinition">The table definition.</param>
        public LookupDefinition(TableDefinition<TEntity> tableDefinition) : base(tableDefinition)
        {
            TableDefinition = tableDefinition;
            base.FilterDefinition = FilterDefinition = new TableFilterDefinition<TEntity>(TableDefinition);
        }

        protected override LookupDefinitionBase BaseClone()
        {
            var clone = new LookupDefinition<TLookupEntity, TEntity>(TableDefinition);
            clone.CopyLookupData(this);
            return clone;
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns>A copy of this object.</returns>
        public new LookupDefinition<TLookupEntity, TEntity> Clone()
        {
            return BaseClone() as LookupDefinition<TLookupEntity, TEntity>;
        }

        /// <summary>
        /// Adds a visible column definition.
        /// </summary>
        /// <param name="lookupEntityProperty">The lookup entity property.</param>
        /// <param name="entityProperty">The entity property.</param>
        /// <returns></returns>
        public LookupFieldColumnDefinition AddVisibleColumnDefinition(
            Expression<Func<TLookupEntity, object>> lookupEntityProperty,
            Expression<Func<TEntity, object>> entityProperty)
        {
            return AddVisibleColumnDefinition(lookupEntityProperty, string.Empty, entityProperty, 0);
        }

        /// <summary>
        /// Adds the visible column definition.
        /// </summary>
        /// <param name="lookupEntityProperty">The lookup entity property.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="entityProperty">The entity property.</param>
        /// <param name="percentWidth">The percent of the lookup's total width.</param>
        /// <returns></returns>
        public LookupFieldColumnDefinition AddVisibleColumnDefinition(Expression<Func<TLookupEntity, object>> lookupEntityProperty,
            string caption, Expression<Func<TEntity, object>> entityProperty, double percentWidth)
        {
            var field = TableDefinition.GetPropertyField(entityProperty.GetFullPropertyName());
            return base.AddVisibleColumnDefinition(lookupEntityProperty, caption, field, percentWidth);
        }

        /// <summary>
        /// Includes the specified related property.
        /// </summary>
        /// <typeparam name="TRelatedEntity">The type of the related entity.</typeparam>
        /// <param name="relatedProperty">The related property.</param>
        /// <returns></returns>
        public LookupJoinTableEntity<TLookupEntity, TRelatedEntity> Include<TRelatedEntity>(Expression<Func<TEntity, TRelatedEntity>> relatedProperty)
            where TRelatedEntity : class

        {
            var returnEntity = new LookupJoinTableEntity<TLookupEntity, TRelatedEntity>(this, ((LookupDefinitionBase) this).TableDefinition, relatedProperty.GetFullPropertyName(), relatedProperty.ReturnType.Name);
            return returnEntity;
        }

        /// <summary>
        /// Includes the specified entity property.
        /// </summary>
        /// <param name="entityProperty">The entity property.</param>
        /// <returns></returns>
        public LookupJoinEntity<TLookupEntity> Include(Expression<Func<TEntity, object>> entityProperty)

        {
            var field = TableDefinition.GetPropertyField(entityProperty.GetFullPropertyName());
            var returnEntity = new LookupJoinEntity<TLookupEntity>(this, field);
            return returnEntity;
        }

        /// <summary>
        /// Adds a hidden column.
        /// </summary>
        /// <param name="lookupEntityProperty">The lookup entity property.</param>
        /// <param name="entityProperty">The entity property.</param>
        /// <returns></returns>
        public LookupFieldColumnDefinition AddHiddenColumn(Expression<Func<TLookupEntity, object>> lookupEntityProperty,
            Expression<Func<TEntity, object>> entityProperty)
        {
            var field = TableDefinition.GetPropertyField(entityProperty.GetFullPropertyName());
            return base.AddHiddenColumn(lookupEntityProperty, field);
        }
    }
}
