using System;
using System.Linq;
using System.Linq.Expressions;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.TableProcessing;

namespace RingSoft.DbLookup.Lookup
{
    /// <summary>
    /// Contains all the data necessary to show a lookup.
    /// </summary>
    /// <typeparam name="TLookupEntity">The type of the lookup entity.</typeparam>
    /// <typeparam name="TEntity">The type of entity used by the Entity Framework platform.</typeparam>
    /// <seealso cref="LookupEntityDefinition{TLookupEntity}" />
    public class LookupDefinition<TLookupEntity, TEntity> : LookupEntityDefinition<TLookupEntity>
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
            LookupEntityName = typeof(TLookupEntity).Name;
            TableDefinition = tableDefinition;
            base.FilterDefinition = FilterDefinition = new TableFilterDefinition<TEntity>(TableDefinition);
        }

        protected internal override LookupDefinitionBase BaseClone()
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
        /// Adds a visible formula column definition.
        /// </summary>
        /// <param name="lookupEntityProperty">The lookup entity property.</param>
        /// <param name="formula">The formula.</param>
        /// <returns></returns>
        public LookupFormulaColumnDefinition AddVisibleColumnDefinition(
            Expression<Func<TLookupEntity, object>> lookupEntityProperty, string formula)
        {
            return AddVisibleColumnDefinition(lookupEntityProperty, string.Empty, formula, 0);
        }

        /// <summary>
        /// Adds a visible formula column definition.
        /// </summary>
        /// <param name="lookupEntityProperty">The lookup entity property.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="formula">The formula.</param>
        /// <param name="percentWidth">The percent of the lookup's total width.</param>
        /// <returns></returns>
        public LookupFormulaColumnDefinition AddVisibleColumnDefinition(
            Expression<Func<TLookupEntity, object>> lookupEntityProperty, string caption, string formula,
            double percentWidth)
        {
            var columnName = caption;
            if (columnName.IsNullOrEmpty())
                columnName = lookupEntityProperty.GetFullPropertyName();

            ValidateProperty(lookupEntityProperty, false, columnName);
            var column = base.AddVisibleColumnDefinition(caption, formula, percentWidth,
                GetFieldDataTypeForProperty(lookupEntityProperty));
            column.PropertyName = lookupEntityProperty.GetFullPropertyName();
            return column;
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
        /// Adds a hidden column.
        /// </summary>
        /// <param name="lookupEntityProperty">The lookup entity property.</param>
        /// <param name="entityProperty">The entity property.</param>
        /// <returns></returns>
        public LookupFieldColumnDefinition AddHiddenColumn(Expression<Func<TLookupEntity, object>> lookupEntityProperty,
            Expression<Func<TEntity, object>> entityProperty)
        {
            var field = TableDefinition.GetPropertyField(entityProperty.GetFullPropertyName());
            return AddHiddenColumn(lookupEntityProperty, field);
        }

        /// <summary>
        /// Adds a hidden column.
        /// </summary>
        /// <param name="lookupEntityProperty">The lookup entity property.</param>
        /// <param name="fieldDefinition">The field definition.</param>
        /// <returns></returns>
        private LookupFieldColumnDefinition AddHiddenColumn(Expression<Func<TLookupEntity, object>> lookupEntityProperty, FieldDefinition fieldDefinition)
        {
            ValidateProperty(lookupEntityProperty, true, string.Empty);

            var column = base.AddHiddenColumn(fieldDefinition);
            column.PropertyName = lookupEntityProperty.GetFullPropertyName();
            return column;
        }

        public LookupFormulaColumnDefinition AddHiddenColumn(Expression<Func<TLookupEntity, object>> lookupEntityProperty, string formula)
        {
            ValidateProperty(lookupEntityProperty, true, lookupEntityProperty.GetFullPropertyName());
            var column = base.AddHiddenColumn(formula, GetFieldDataTypeForProperty(lookupEntityProperty));
            column.PropertyName = lookupEntityProperty.GetFullPropertyName();
            return column;
        }


        /// <summary>
        /// Gets the column definition.
        /// </summary>
        /// <param name="lookupEntityProperty">The lookup entity property.</param>
        /// <returns></returns>
        public LookupColumnDefinitionBase GetColumnDefinition(Expression<Func<TLookupEntity, object>> lookupEntityProperty)
        {
            var propertyName = lookupEntityProperty.GetFullPropertyName();
            var column = VisibleColumns.FirstOrDefault(c => c.PropertyName == propertyName);
            if (column == null)
                column = HiddenColumns.FirstOrDefault(c => c.PropertyName == propertyName);
            return column;
        }
    }
}
