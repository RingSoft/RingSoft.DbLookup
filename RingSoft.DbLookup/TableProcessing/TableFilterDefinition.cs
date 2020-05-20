using System;
using System.Linq.Expressions;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.QueryBuilder;

namespace RingSoft.DbLookup.TableProcessing
{
    /// <summary>
    /// A filter definition based on an entity.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <seealso cref="TableFilterDefinitionBase" />
    public class TableFilterDefinition<TEntity> 
        : TableFilterDefinitionBase 
        where TEntity : new()
    {
        private TableDefinition<TEntity> _entityTableDefinition;

        /// <summary>
        /// Initializes a new instance of the <see cref="TableFilterDefinition{TEntity}"/> class.
        /// </summary>
        /// <param name="entityTableDefinition">The entity table definition.</param>
        public TableFilterDefinition(TableDefinition<TEntity> entityTableDefinition)
        {
            _entityTableDefinition = entityTableDefinition;
        }

        /// <summary>
        /// Adds a fixed filter.
        /// </summary>
        /// <param name="propertyExpression">The property expression.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <returns>The added filter.</returns>
        public FieldFilterDefinition AddFixedFilter(Expression<Func<TEntity, string>> propertyExpression,
            Conditions condition, string value)
        {
            var fieldDefinition = _entityTableDefinition.GetFieldDefinition(propertyExpression);
            return AddFixedFilter(fieldDefinition, condition, value);
        }

        /// <summary>
        /// Adds a fixed filter.
        /// </summary>
        /// <param name="propertyExpression">The property expression.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <returns>The added filter.</returns>
        public FieldFilterDefinition AddFixedFilter(Expression<Func<TEntity, int>> propertyExpression,
            Conditions condition, int value)
        {
            var fieldDefinition = _entityTableDefinition.GetFieldDefinition(propertyExpression);
            return AddFixedFilter(fieldDefinition, condition, value);
        }

        /// <summary>
        /// Adds a fixed filter.
        /// </summary>
        /// <param name="propertyExpression">The property expression.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <returns>The added filter.</returns>
        public FieldFilterDefinition AddFixedFilter(Expression<Func<TEntity, int?>> propertyExpression,
            Conditions condition, int value)
        {
            var fieldDefinition = _entityTableDefinition.GetFieldDefinition(propertyExpression);
            return AddFixedFilter(fieldDefinition, condition, value);
        }

        /// <summary>
        /// Adds a fixed filter.
        /// </summary>
        /// <param name="propertyExpression">The property expression.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <returns>The added filter.</returns>
        public FieldFilterDefinition AddFixedFilter(Expression<Func<TEntity, long>> propertyExpression,
            Conditions condition, int value)
        {
            var fieldDefinition = _entityTableDefinition.GetFieldDefinition(propertyExpression);
            return AddFixedFilter(fieldDefinition, condition, value);
        }

        /// <summary>
        /// Adds a fixed filter.
        /// </summary>
        /// <param name="propertyExpression">The property expression.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <returns>The added filter.</returns>
        public FieldFilterDefinition AddFixedFilter(Expression<Func<TEntity, long?>> propertyExpression,
            Conditions condition, int value)
        {
            var fieldDefinition = _entityTableDefinition.GetFieldDefinition(propertyExpression);
            return AddFixedFilter(fieldDefinition, condition, value);
        }

        /// <summary>
        /// Adds a fixed filter.
        /// </summary>
        /// <param name="propertyExpression">The property expression.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <returns>The added filter.</returns>
        public FieldFilterDefinition AddFixedFilter(Expression<Func<TEntity, byte>> propertyExpression,
            Conditions condition, int value)
        {
            var fieldDefinition = _entityTableDefinition.GetFieldDefinition(propertyExpression);
            return AddFixedFilter(fieldDefinition, condition, value);
        }

        /// <summary>
        /// Adds a fixed filter.
        /// </summary>
        /// <param name="propertyExpression">The property expression.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <returns>The added filter.</returns>
        public FieldFilterDefinition AddFixedFilter(Expression<Func<TEntity, byte?>> propertyExpression,
            Conditions condition, int value)
        {
            var fieldDefinition = _entityTableDefinition.GetFieldDefinition(propertyExpression);
            return AddFixedFilter(fieldDefinition, condition, value);
        }

        /// <summary>
        /// Adds a fixed filter.
        /// </summary>
        /// <param name="propertyExpression">The property expression.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <returns>The added filter.</returns>
        public FieldFilterDefinition AddFixedFilter(Expression<Func<TEntity, short>> propertyExpression,
            Conditions condition, int value)
        {
            var fieldDefinition = _entityTableDefinition.GetFieldDefinition(propertyExpression);
            return AddFixedFilter(fieldDefinition, condition, value);
        }

        /// <summary>
        /// Adds a fixed filter.
        /// </summary>
        /// <param name="propertyExpression">The property expression.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <returns>The added filter.</returns>
        public FieldFilterDefinition AddFixedFilter(Expression<Func<TEntity, short?>> propertyExpression,
            Conditions condition, int value)
        {
            var fieldDefinition = _entityTableDefinition.GetFieldDefinition(propertyExpression);
            return AddFixedFilter(fieldDefinition, condition, value);
        }

        /// <summary>
        /// Adds a fixed filter.
        /// </summary>
        /// <param name="propertyExpression">The property expression.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <returns>The added filter.</returns>
        public FieldFilterDefinition AddFixedFilter(Expression<Func<TEntity, double>> propertyExpression,
            Conditions condition, double value)
        {
            var fieldDefinition = _entityTableDefinition.GetFieldDefinition(propertyExpression);
            return AddFixedFilter(fieldDefinition, condition, value);
        }

        /// <summary>
        /// Adds a fixed filter.
        /// </summary>
        /// <param name="propertyExpression">The property expression.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <returns>The added filter.</returns>
        public FieldFilterDefinition AddFixedFilter(Expression<Func<TEntity, double?>> propertyExpression,
            Conditions condition, double value)
        {
            var fieldDefinition = _entityTableDefinition.GetFieldDefinition(propertyExpression);
            return AddFixedFilter(fieldDefinition, condition, value);
        }

        /// <summary>
        /// Adds a fixed filter.
        /// </summary>
        /// <param name="propertyExpression">The property expression.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <returns>The added filter.</returns>
        public FieldFilterDefinition AddFixedFilter(Expression<Func<TEntity, float>> propertyExpression,
            Conditions condition, float value)
        {
            var fieldDefinition = _entityTableDefinition.GetFieldDefinition(propertyExpression);
            return AddFixedFilter(fieldDefinition, condition, value);
        }

        /// <summary>
        /// Adds a fixed filter.
        /// </summary>
        /// <param name="propertyExpression">The property expression.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <returns>The added filter.</returns>
        public FieldFilterDefinition AddFixedFilter(Expression<Func<TEntity, float?>> propertyExpression,
            Conditions condition, float value)
        {
            var fieldDefinition = _entityTableDefinition.GetFieldDefinition(propertyExpression);
            return AddFixedFilter(fieldDefinition, condition, value);
        }

        /// <summary>
        /// Adds a fixed filter.
        /// </summary>
        /// <param name="propertyExpression">The property expression.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <returns>The added filter.</returns>
        public FieldFilterDefinition AddFixedFilter(Expression<Func<TEntity, decimal>> propertyExpression,
            Conditions condition, decimal value)
        {
            var fieldDefinition = _entityTableDefinition.GetFieldDefinition(propertyExpression);
            return AddFixedFilter(fieldDefinition, condition, value);
        }

        /// <summary>
        /// Adds a fixed filter.
        /// </summary>
        /// <param name="propertyExpression">The property expression.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <returns>The added filter.</returns>
        public FieldFilterDefinition AddFixedFilter(Expression<Func<TEntity, decimal?>> propertyExpression,
            Conditions condition, decimal value)
        {
            var fieldDefinition = _entityTableDefinition.GetFieldDefinition(propertyExpression);
            return AddFixedFilter(fieldDefinition, condition, value);
        }

        /// <summary>
        /// Adds a fixed filter.
        /// </summary>
        /// <param name="propertyExpression">The property expression.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <returns>The added filter.</returns>
        public FieldFilterDefinition AddFixedFilter(Expression<Func<TEntity, DateTime>> propertyExpression,
            Conditions condition, DateTime value)
        {
            var fieldDefinition = _entityTableDefinition.GetFieldDefinition(propertyExpression);
            return AddFixedFilter(fieldDefinition, condition, value);
        }

        /// <summary>
        /// Adds a fixed filter.
        /// </summary>
        /// <param name="propertyExpression">The property expression.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <returns>The added filter.</returns>
        public FieldFilterDefinition AddFixedFilter(Expression<Func<TEntity, DateTime?>> propertyExpression,
            Conditions condition, DateTime value)
        {
            var fieldDefinition = _entityTableDefinition.GetFieldDefinition(propertyExpression);
            return AddFixedFilter(fieldDefinition, condition, value);
        }

        /// <summary>
        /// Adds a fixed filter.
        /// </summary>
        /// <param name="propertyExpression">The property expression.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns>The added filter.</returns>
        public FieldFilterDefinition AddFixedFilter(Expression<Func<TEntity, bool>> propertyExpression,
            Conditions condition, bool value)
        {
            var fieldDefinition = _entityTableDefinition.GetFieldDefinition(propertyExpression);
            return AddFixedFilter(fieldDefinition, condition, value);
        }

        /// <summary>
        /// Adds a fixed filter.
        /// </summary>
        /// <param name="propertyExpression">The property expression.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <returns>The added filter.</returns>
        public FieldFilterDefinition AddFixedFilter(Expression<Func<TEntity, Enum>> propertyExpression,
            Conditions condition, Enum value)
        {
            var fieldDefinition = _entityTableDefinition.GetFieldDefinition(propertyExpression);
            return AddFixedFilter(fieldDefinition, condition, value);
        }

        /// <summary>
        /// Adds a fixed filter.
        /// </summary>
        /// <param name="propertyExpression">The property expression.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <returns>The added filter.</returns>
        public FieldFilterDefinition AddFixedFilter(Expression<Func<TEntity, Enum>> propertyExpression,
            Conditions condition, string value)
        {
            var fieldDefinition = _entityTableDefinition.GetFieldDefinition(propertyExpression);
            return AddFixedFilter(fieldDefinition, condition, value);
        }

        /// <summary>
        /// Includes the specified related property.
        /// </summary>
        /// <typeparam name="TRelatedEntity">The type of the related entity.</typeparam>
        /// <param name="relatedProperty">The related property.</param>
        /// <returns></returns>
        public RelatedTableFilterDefinition<TRelatedEntity> Include<TRelatedEntity>(
            Expression<Func<TEntity, TRelatedEntity>> relatedProperty)
            where TRelatedEntity : new()
        {
            var returnEntity = new RelatedTableFilterDefinition<TRelatedEntity>(this,
                _entityTableDefinition, relatedProperty.GetFullPropertyName(), string.Empty);

            return returnEntity;
        }

    }
}
