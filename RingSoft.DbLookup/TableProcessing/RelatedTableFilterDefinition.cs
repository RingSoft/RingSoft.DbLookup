// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 06-28-2023
// ***********************************************************************
// <copyright file="RelatedTableFilterDefinition.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Linq;
using System.Linq.Expressions;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.QueryBuilder;

namespace RingSoft.DbLookup.TableProcessing
{
    /// <summary>
    /// A table filter joined by a related entity.
    /// </summary>
    /// <typeparam name="TRelatedEntity">The type of the related entity.</typeparam>
    /// <seealso cref="RelatedTableFilterDefinitionBase" />
    public class RelatedTableFilterDefinition<TRelatedEntity> : RelatedTableFilterDefinitionBase
        where TRelatedEntity : class, new()
    {
        /// <summary>
        /// The entity table definition
        /// </summary>
        private TableDefinition<TRelatedEntity> _entityTableDefinition;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelatedTableFilterDefinition{TRelatedEntity}" /> class.
        /// </summary>
        /// <param name="tableFilterDefinition">The table filter definition.</param>
        /// <param name="tableDefinition">The table definition.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="parentAlias">The parent alias.</param>
        /// <exception cref="System.ArgumentException">Property '{propertyName}' was not configured by the Entity Framework.</exception>
        /// <exception cref="System.ArgumentException"></exception>
        public RelatedTableFilterDefinition(TableFilterDefinitionBase tableFilterDefinition, TableDefinitionBase tableDefinition, 
            string propertyName, string parentAlias) : base(tableFilterDefinition)
        { 
            var foreignFieldDefinition = tableDefinition.FieldDefinitions.FirstOrDefault(f =>
                f.ParentJoinForeignKeyDefinition != null &&
                f.ParentJoinForeignKeyDefinition.ForeignObjectPropertyName == propertyName);

            if (foreignFieldDefinition == null)
                throw new ArgumentException($"Property '{propertyName}' was not configured by the Entity Framework.");

            SetJoin(foreignFieldDefinition, parentAlias);

            _entityTableDefinition = TableDefinition as TableDefinition<TRelatedEntity>;

            if (_entityTableDefinition == null)
                throw new ArgumentException(
                    $"{foreignFieldDefinition.ParentJoinForeignKeyDefinition.PrimaryTable.EntityName} is not an EntityTableDefinition");
        }

        /// <summary>
        /// Includes the specified related property.
        /// </summary>
        /// <typeparam name="TParentRelatedEntity">The type of the parent related entity.</typeparam>
        /// <param name="relatedProperty">The related property.</param>
        /// <returns>RelatedTableFilterDefinition&lt;TParentRelatedEntity&gt;.</returns>
        public RelatedTableFilterDefinition<TParentRelatedEntity> Include<TParentRelatedEntity>(
            Expression<Func<TRelatedEntity, TParentRelatedEntity>> relatedProperty)
            where TParentRelatedEntity : class, new()
        {
            var parentTable = TableFieldJoinDefinition.ForeignKeyDefinition.PrimaryTable;

            var returnEntity = new RelatedTableFilterDefinition<TParentRelatedEntity>(TableFilterDefinition,
                parentTable, relatedProperty.GetFullPropertyName(), TableFieldJoinDefinition.ForeignKeyDefinition.Alias);

            return returnEntity;
        }

        /// <summary>
        /// Configures the field filter definition.
        /// </summary>
        /// <param name="fieldFilterDefinition">The field filter definition.</param>
        private void ConfigureFieldFilterDefinition(FieldFilterDefinition fieldFilterDefinition)
        {
            fieldFilterDefinition.JoinDefinition = TableFieldJoinDefinition;
        }

        /// <summary>
        /// Adds a fixed filter.
        /// </summary>
        /// <param name="propertyExpression">The property expression.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <returns>The added filter.</returns>
        public FieldFilterDefinition AddFixedFilter(Expression<Func<TRelatedEntity, string>> propertyExpression,
            Conditions condition, string value)
        {
            var fieldDefinition = _entityTableDefinition.GetFieldDefinition(propertyExpression);
            var result = TableFilterDefinition.AddFixedFilter(fieldDefinition, condition, value);
            ConfigureFieldFilterDefinition(result);
            return result;
        }

        /// <summary>
        /// Adds a fixed filter.
        /// </summary>
        /// <param name="propertyExpression">The property expression.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <returns>The added filter.</returns>
        public FieldFilterDefinition AddFixedFilter(Expression<Func<TRelatedEntity, int>> propertyExpression,
            Conditions condition, int value)
        {
            var fieldDefinition = _entityTableDefinition.GetFieldDefinition(propertyExpression);
            var result = TableFilterDefinition.AddFixedFilter(fieldDefinition, condition, value);
            ConfigureFieldFilterDefinition(result);
            return result;
        }

        /// <summary>
        /// Adds a fixed filter.
        /// </summary>
        /// <param name="propertyExpression">The property expression.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <returns>The added filter.</returns>
        public FieldFilterDefinition AddFixedFilter(Expression<Func<TRelatedEntity, int?>> propertyExpression,
            Conditions condition, int value)
        {
            var fieldDefinition = _entityTableDefinition.GetFieldDefinition(propertyExpression);
            var result = TableFilterDefinition.AddFixedFilter(fieldDefinition, condition, value);
            ConfigureFieldFilterDefinition(result);
            return result;
        }

        /// <summary>
        /// Adds a fixed filter.
        /// </summary>
        /// <param name="propertyExpression">The property expression.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <returns>The added filter.</returns>
        public FieldFilterDefinition AddFixedFilter(Expression<Func<TRelatedEntity, long>> propertyExpression,
            Conditions condition, int value)
        {
            var fieldDefinition = _entityTableDefinition.GetFieldDefinition(propertyExpression);
            var result = TableFilterDefinition.AddFixedFilter(fieldDefinition, condition, value);
            ConfigureFieldFilterDefinition(result);
            return result;
        }

        /// <summary>
        /// Adds a fixed filter.
        /// </summary>
        /// <param name="propertyExpression">The property expression.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <returns>The added filter.</returns>
        public FieldFilterDefinition AddFixedFilter(Expression<Func<TRelatedEntity, long?>> propertyExpression,
            Conditions condition, int value)
        {
            var fieldDefinition = _entityTableDefinition.GetFieldDefinition(propertyExpression);
            var result = TableFilterDefinition.AddFixedFilter(fieldDefinition, condition, value);
            ConfigureFieldFilterDefinition(result);
            return result;
        }

        /// <summary>
        /// Adds a fixed filter.
        /// </summary>
        /// <param name="propertyExpression">The property expression.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <returns>The added filter.</returns>
        public FieldFilterDefinition AddFixedFilter(Expression<Func<TRelatedEntity, byte>> propertyExpression,
            Conditions condition, int value)
        {
            var fieldDefinition = _entityTableDefinition.GetFieldDefinition(propertyExpression);
            var result = TableFilterDefinition.AddFixedFilter(fieldDefinition, condition, value);
            ConfigureFieldFilterDefinition(result);
            return result;
        }

        /// <summary>
        /// Adds a fixed filter.
        /// </summary>
        /// <param name="propertyExpression">The property expression.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <returns>The added filter.</returns>
        public FieldFilterDefinition AddFixedFilter(Expression<Func<TRelatedEntity, byte?>> propertyExpression,
            Conditions condition, int value)
        {
            var fieldDefinition = _entityTableDefinition.GetFieldDefinition(propertyExpression);
            var result = TableFilterDefinition.AddFixedFilter(fieldDefinition, condition, value);
            ConfigureFieldFilterDefinition(result);
            return result;
        }

        /// <summary>
        /// Adds a fixed filter.
        /// </summary>
        /// <param name="propertyExpression">The property expression.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <returns>The added filter.</returns>
        public FieldFilterDefinition AddFixedFilter(Expression<Func<TRelatedEntity, short>> propertyExpression,
            Conditions condition, int value)
        {
            var fieldDefinition = _entityTableDefinition.GetFieldDefinition(propertyExpression);
            var result = TableFilterDefinition.AddFixedFilter(fieldDefinition, condition, value);
            ConfigureFieldFilterDefinition(result);
            return result;
        }

        /// <summary>
        /// Adds a fixed filter.
        /// </summary>
        /// <param name="propertyExpression">The property expression.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <returns>The added filter.</returns>
        public FieldFilterDefinition AddFixedFilter(Expression<Func<TRelatedEntity, short?>> propertyExpression,
            Conditions condition, int value)
        {
            var fieldDefinition = _entityTableDefinition.GetFieldDefinition(propertyExpression);
            var result = TableFilterDefinition.AddFixedFilter(fieldDefinition, condition, value);
            ConfigureFieldFilterDefinition(result);
            return result;
        }


        /// <summary>
        /// Adds a fixed filter.
        /// </summary>
        /// <param name="propertyExpression">The property expression.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <returns>The added filter.</returns>
        public FieldFilterDefinition AddFixedFilter(Expression<Func<TRelatedEntity, float>> propertyExpression,
            Conditions condition, float value)
        {
            var fieldDefinition = _entityTableDefinition.GetFieldDefinition(propertyExpression);
            var result = TableFilterDefinition.AddFixedFilter(fieldDefinition, condition, value);
            ConfigureFieldFilterDefinition(result);
            return result;
        }

        /// <summary>
        /// Adds a fixed filter.
        /// </summary>
        /// <param name="propertyExpression">The property expression.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <returns>The added filter.</returns>
        public FieldFilterDefinition AddFixedFilter(Expression<Func<TRelatedEntity, float?>> propertyExpression,
            Conditions condition, float value)
        {
            var fieldDefinition = _entityTableDefinition.GetFieldDefinition(propertyExpression);
            var result = TableFilterDefinition.AddFixedFilter(fieldDefinition, condition, value);
            ConfigureFieldFilterDefinition(result);
            return result;
        }

        /// <summary>
        /// Adds a fixed filter.
        /// </summary>
        /// <param name="propertyExpression">The property expression.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <returns>The added filter.</returns>
        public FieldFilterDefinition AddFixedFilter(Expression<Func<TRelatedEntity, double>> propertyExpression,
            Conditions condition, double value)
        {
            var fieldDefinition = _entityTableDefinition.GetFieldDefinition(propertyExpression);
            var result = TableFilterDefinition.AddFixedFilter(fieldDefinition, condition, value);
            ConfigureFieldFilterDefinition(result);
            return result;
        }

        /// <summary>
        /// Adds a fixed filter.
        /// </summary>
        /// <param name="propertyExpression">The property expression.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <returns>The added filter.</returns>
        public FieldFilterDefinition AddFixedFilter(Expression<Func<TRelatedEntity, double?>> propertyExpression,
            Conditions condition, double value)
        {
            var fieldDefinition = _entityTableDefinition.GetFieldDefinition(propertyExpression);
            var result = TableFilterDefinition.AddFixedFilter(fieldDefinition, condition, value);
            ConfigureFieldFilterDefinition(result);
            return result;
        }

        /// <summary>
        /// Adds a fixed filter.
        /// </summary>
        /// <param name="propertyExpression">The property expression.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <returns>The added filter.</returns>
        public FieldFilterDefinition AddFixedFilter(Expression<Func<TRelatedEntity, DateTime>> propertyExpression,
            Conditions condition, DateTime value)
        {
            var fieldDefinition = _entityTableDefinition.GetFieldDefinition(propertyExpression);
            var result = TableFilterDefinition.AddFixedFilter(fieldDefinition, condition, value);
            ConfigureFieldFilterDefinition(result);
            return result;
        }

        /// <summary>
        /// Adds a fixed filter.
        /// </summary>
        /// <param name="propertyExpression">The property expression.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <returns>The added filter.</returns>
        public FieldFilterDefinition AddFixedFilter(Expression<Func<TRelatedEntity, DateTime?>> propertyExpression,
            Conditions condition, DateTime value)
        {
            var fieldDefinition = _entityTableDefinition.GetFieldDefinition(propertyExpression);
            var result = TableFilterDefinition.AddFixedFilter(fieldDefinition, condition, value);
            ConfigureFieldFilterDefinition(result);
            return result;
        }

        /// <summary>
        /// Adds a fixed filter.
        /// </summary>
        /// <param name="propertyExpression">The property expression.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns>The added filter.</returns>
        public FieldFilterDefinition AddFixedFilter(Expression<Func<TRelatedEntity, bool>> propertyExpression,
            Conditions condition, bool value)
        {
            var fieldDefinition = _entityTableDefinition.GetFieldDefinition(propertyExpression);
            var result = TableFilterDefinition.AddFixedFilter(fieldDefinition, condition, value);
            ConfigureFieldFilterDefinition(result);
            return result;
        }
    }
}
