// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 06-28-2023
// ***********************************************************************
// <copyright file="TableFilterDefinition.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
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
    public class TableFilterDefinition<TEntity> : TableFilterDefinitionBase 
        where TEntity : class, new()
    {
        /// <summary>
        /// The entity table definition
        /// </summary>
        private TableDefinition<TEntity> _entityTableDefinition;

        /// <summary>
        /// Initializes a new instance of the <see cref="TableFilterDefinition{TEntity}"/> class.
        /// </summary>
        /// <param name="entityTableDefinition">The entity table definition.</param>
        public TableFilterDefinition(TableDefinition<TEntity> entityTableDefinition) : base(entityTableDefinition)
        {
            _entityTableDefinition = entityTableDefinition;
        }

        /// <summary>
        /// Adds a fixed filter.
        /// </summary>
        /// <param name="entityProperty">The entity property.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <returns>The added filter.</returns>
        public FieldFilterDefinition AddFixedFilter(Expression<Func<TEntity, string>> entityProperty,
            Conditions condition, string value)
        {
            var fieldDefinition = _entityTableDefinition.GetFieldDefinition(entityProperty);
            return AddFixedFilter(fieldDefinition, condition, value);
        }

        /// <summary>
        /// Adds a fixed filter.
        /// </summary>
        /// <param name="entityProperty">The entity property.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <returns>The added filter.</returns>
        public FieldFilterDefinition AddFixedFilter(Expression<Func<TEntity, int>> entityProperty,
            Conditions condition, int value)
        {
            var fieldDefinition = _entityTableDefinition.GetFieldDefinition(entityProperty);
            return AddFixedFilter(fieldDefinition, condition, value);
        }

        /// <summary>
        /// Adds a fixed filter.
        /// </summary>
        /// <param name="entityProperty">The entity property.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <returns>The added filter.</returns>
        public FieldFilterDefinition AddFixedFilter(Expression<Func<TEntity, int?>> entityProperty,
            Conditions condition, int value)
        {
            var fieldDefinition = _entityTableDefinition.GetFieldDefinition(entityProperty);
            return AddFixedFilter(fieldDefinition, condition, value);
        }

        /// <summary>
        /// Adds a fixed filter.
        /// </summary>
        /// <param name="entityProperty">The entity property.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <returns>The added filter.</returns>
        public FieldFilterDefinition AddFixedFilter(Expression<Func<TEntity, long>> entityProperty,
            Conditions condition, int value)
        {
            var fieldDefinition = _entityTableDefinition.GetFieldDefinition(entityProperty);
            return AddFixedFilter(fieldDefinition, condition, value);
        }

        /// <summary>
        /// Adds a fixed filter.
        /// </summary>
        /// <param name="entityProperty">The entity property.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <returns>The added filter.</returns>
        public FieldFilterDefinition AddFixedFilter(Expression<Func<TEntity, long?>> entityProperty,
            Conditions condition, int value)
        {
            var fieldDefinition = _entityTableDefinition.GetFieldDefinition(entityProperty);
            return AddFixedFilter(fieldDefinition, condition, value);
        }

        /// <summary>
        /// Adds a fixed filter.
        /// </summary>
        /// <param name="entityProperty">The entity property.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <returns>The added filter.</returns>
        public FieldFilterDefinition AddFixedFilter(Expression<Func<TEntity, byte>> entityProperty,
            Conditions condition, int value)
        {
            var fieldDefinition = _entityTableDefinition.GetFieldDefinition(entityProperty);
            return AddFixedFilter(fieldDefinition, condition, value);
        }

        /// <summary>
        /// Adds a fixed filter.
        /// </summary>
        /// <param name="entityProperty">The entity property.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <returns>The added filter.</returns>
        public FieldFilterDefinition AddFixedFilter(Expression<Func<TEntity, byte?>> entityProperty,
            Conditions condition, int value)
        {
            var fieldDefinition = _entityTableDefinition.GetFieldDefinition(entityProperty);
            return AddFixedFilter(fieldDefinition, condition, value);
        }

        /// <summary>
        /// Adds a fixed filter.
        /// </summary>
        /// <param name="entityProperty">The entity property.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <returns>The added filter.</returns>
        public FieldFilterDefinition AddFixedFilter(Expression<Func<TEntity, short>> entityProperty,
            Conditions condition, int value)
        {
            var fieldDefinition = _entityTableDefinition.GetFieldDefinition(entityProperty);
            return AddFixedFilter(fieldDefinition, condition, value);
        }

        /// <summary>
        /// Adds a fixed filter.
        /// </summary>
        /// <param name="entityProperty">The entity property.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <returns>The added filter.</returns>
        public FieldFilterDefinition AddFixedFilter(Expression<Func<TEntity, short?>> entityProperty,
            Conditions condition, int value)
        {
            var fieldDefinition = _entityTableDefinition.GetFieldDefinition(entityProperty);
            return AddFixedFilter(fieldDefinition, condition, value);
        }


        /// <summary>
        /// Adds a fixed filter.
        /// </summary>
        /// <param name="entityProperty">The entity property.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <returns>The added filter.</returns>
        public FieldFilterDefinition AddFixedFilter(Expression<Func<TEntity, float>> entityProperty,
            Conditions condition, float value)
        {
            var fieldDefinition = _entityTableDefinition.GetFieldDefinition(entityProperty);
            return AddFixedFilter(fieldDefinition, condition, value);
        }

        /// <summary>
        /// Adds a fixed filter.
        /// </summary>
        /// <param name="entityProperty">The entity property.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <returns>The added filter.</returns>
        public FieldFilterDefinition AddFixedFilter(Expression<Func<TEntity, float?>> entityProperty,
            Conditions condition, float value)
        {
            var fieldDefinition = _entityTableDefinition.GetFieldDefinition(entityProperty);
            return AddFixedFilter(fieldDefinition, condition, value);
        }

        /// <summary>
        /// Adds a fixed filter.
        /// </summary>
        /// <param name="entityProperty">The entity property.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <returns>The added filter.</returns>
        public FieldFilterDefinition AddFixedFilter(Expression<Func<TEntity, double>> entityProperty,
            Conditions condition, double value)
        {
            var fieldDefinition = _entityTableDefinition.GetFieldDefinition(entityProperty);
            return AddFixedFilter(fieldDefinition, condition, value);
        }

        /// <summary>
        /// Adds a fixed filter.
        /// </summary>
        /// <param name="entityProperty">The entity property.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <returns>The added filter.</returns>
        public FieldFilterDefinition AddFixedFilter(Expression<Func<TEntity, double?>> entityProperty,
            Conditions condition, double value)
        {
            var fieldDefinition = _entityTableDefinition.GetFieldDefinition(entityProperty);
            return AddFixedFilter(fieldDefinition, condition, value);
        }

        /// <summary>
        /// Adds a fixed filter.
        /// </summary>
        /// <param name="entityProperty">The entity property.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <returns>The added filter.</returns>
        public FieldFilterDefinition AddFixedFilter(Expression<Func<TEntity, DateTime>> entityProperty,
            Conditions condition, DateTime value)
        {
            var fieldDefinition = _entityTableDefinition.GetFieldDefinition(entityProperty);
            return AddFixedFilter(fieldDefinition, condition, value);
        }

        /// <summary>
        /// Adds a fixed filter.
        /// </summary>
        /// <param name="entityProperty">The entity property.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <returns>The added filter.</returns>
        public FieldFilterDefinition AddFixedFilter(Expression<Func<TEntity, DateTime?>> entityProperty,
            Conditions condition, DateTime value)
        {
            var fieldDefinition = _entityTableDefinition.GetFieldDefinition(entityProperty);
            return AddFixedFilter(fieldDefinition, condition, value);
        }

        /// <summary>
        /// Adds a fixed filter.
        /// </summary>
        /// <param name="entityProperty">The entity property.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns>The added filter.</returns>
        public FieldFilterDefinition AddFixedFilter(Expression<Func<TEntity, bool>> entityProperty,
            Conditions condition, bool value)
        {
            var fieldDefinition = _entityTableDefinition.GetFieldDefinition(entityProperty);
            return AddFixedFilter(fieldDefinition, condition, value);
        }

        /// <summary>
        /// Includes the specified related property.
        /// </summary>
        /// <typeparam name="TRelatedEntity">The type of the related entity.</typeparam>
        /// <param name="relatedProperty">The related property.</param>
        /// <returns>RelatedTableFilterDefinition&lt;TRelatedEntity&gt;.</returns>
        public RelatedTableFilterDefinition<TRelatedEntity> Include<TRelatedEntity>(
            Expression<Func<TEntity, TRelatedEntity>> relatedProperty)
            where TRelatedEntity : class, new()
        {
            var returnEntity = new RelatedTableFilterDefinition<TRelatedEntity>(this,
                _entityTableDefinition, relatedProperty.GetFullPropertyName(), string.Empty);

            return returnEntity;
        }

    }
}
