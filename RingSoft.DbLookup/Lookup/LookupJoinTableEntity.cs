// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-19-2023
// ***********************************************************************
// <copyright file="LookupJoinTableEntity.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.TableProcessing;
using System;
using System.Linq;
using System.Linq.Expressions;

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
        where TLookupEntity : new() where TEntity :class,  new() where TRelatedEntity : class
    {
        /// <summary>
        /// Gets the parent join definition.
        /// </summary>
        /// <value>The parent join definition.</value>
        public TableFieldJoinDefinition ParentJoinDefinition { get; internal set; }

        /// <summary>
        /// The lookup entity definition
        /// </summary>
        private LookupDefinition<TLookupEntity, TEntity> _lookupEntityDefinition;

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupJoinTableEntity{TLookupEntity, TEntity, TRelatedEntity}" /> class.
        /// </summary>
        /// <param name="lookupEntityDefinition">The lookup entity definition.</param>
        /// <param name="tableDefinition">The table definition.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="propertyType">Type of the property.</param>
        internal LookupJoinTableEntity(LookupDefinition<TLookupEntity, TEntity> lookupEntityDefinition,
            TableDefinitionBase tableDefinition, string propertyName, string propertyType) : base(lookupEntityDefinition)
        {
            _lookupEntityDefinition = lookupEntityDefinition;
            SetJoinDefinition(tableDefinition, propertyName, propertyType);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupJoinTableEntity{TLookupEntity, TEntity, TRelatedEntity}" /> class.
        /// </summary>
        /// <param name="lookupEntityDefinition">The lookup entity definition.</param>
        private LookupJoinTableEntity(LookupDefinition<TLookupEntity, TEntity> lookupEntityDefinition) : base(lookupEntityDefinition)
        {
            _lookupEntityDefinition = lookupEntityDefinition;
        }

        /// <summary>
        /// Sets the join definition.
        /// </summary>
        /// <param name="tableDefinition">The table definition.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="propertyType">Type of the property.</param>
        /// <exception cref="System.ArgumentException">Property type '{propertyType}' is not setup as a Table Definition in the Lookup Context.</exception>
        /// <exception cref="System.ArgumentException">Property '{propertyName}' was not configured by the Entity Framework.</exception>
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
        /// <returns>LookupFieldColumnDefinition.</returns>
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
        /// <returns>LookupFieldColumnDefinition.</returns>
        public LookupFieldColumnDefinition AddVisibleColumnDefinition(Expression<Func<TLookupEntity, object>> lookupEntityProperty,
            string caption, Expression<Func<TRelatedEntity, object>> entityProperty, double percentWidth)
        {
            var parentTable = JoinDefinition.ForeignKeyDefinition.PrimaryTable;
            var relatedPropertyName = entityProperty.GetFullPropertyName();
            var relatedField = parentTable.FieldDefinitions.FirstOrDefault(f => f.PropertyName == relatedPropertyName);

            var columnDefinition = AddVisibleColumnDefinition(caption, relatedField, percentWidth);
            //columnDefinition.ChildField = relatedField;
            columnDefinition.RelatedTableName = typeof(TRelatedEntity).FullName;
            columnDefinition.ChildField = JoinDefinition.ForeignKeyDefinition.FieldJoins[0].ForeignField;
            columnDefinition.PropertyName = lookupEntityProperty.GetFullPropertyName();
            columnDefinition.ParentObject = this;
            columnDefinition.Path = MakePath() + relatedField.MakePath();

            return columnDefinition;
        }

        //protected override void MakeColumnPath(LookupFieldColumnDefinition columnDefinition)
        //{
        //    var childJoinField = ChildJoinField;
        //    var path = columnDefinition.FieldDefinition.MakePath();
        //    var parentObject = columnDefinition.ParentObject;
        //    while (parentObject != null && parentObject.ChildJoinField != null)
        //    {
        //        path = parentObject.ChildJoinField.MakePath() + path;
        //        parentObject = parentObject.ParentObject;
        //    }

        //    path = ParentField.MakePath() + path;
        //    columnDefinition.Path = path;
        //}

        /// <summary>
        /// Adds the visible column definition.
        /// </summary>
        /// <param name="lookupEntityProperty">The lookup entity property.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="lookupFormula">The lookup formula.</param>
        /// <param name="percentWidth">Width of the percent.</param>
        /// <param name="dataType">Type of the data.</param>
        /// <param name="allowNulls">if set to <c>true</c> [allow nulls].</param>
        /// <returns>LookupFormulaColumnDefinition.</returns>
        public LookupFormulaColumnDefinition AddVisibleColumnDefinition(Expression<Func<TLookupEntity, object>> lookupEntityProperty,
            string caption, ILookupFormula lookupFormula, double percentWidth, FieldDataTypes dataType, bool allowNulls = false)
        {
            var parentTable = JoinDefinition.ForeignKeyDefinition.PrimaryTable;
            
            var columnDefinition = AddVisibleColumnDefinition(caption, lookupFormula, percentWidth, dataType, allowNulls);
            //columnDefinition.ChildField = relatedField;
            columnDefinition.ChildField = JoinDefinition.ForeignKeyDefinition.FieldJoins[0].ForeignField;
            if (JoinDefinition.ForeignKeyDefinition != null)
            {
                columnDefinition.PrimaryTable = JoinDefinition.ForeignKeyDefinition.PrimaryTable;
                columnDefinition.PrimaryField = JoinDefinition.ForeignKeyDefinition.FieldJoins[0].PrimaryField;
            }
            columnDefinition.PropertyName = lookupEntityProperty.GetFullPropertyName();
            columnDefinition.ParentObject = this;
            MakeColumnPath(columnDefinition);

            return columnDefinition;
        }

        /// <summary>
        /// Adds a hidden column.
        /// </summary>
        /// <param name="lookupEntityProperty">The lookup entity property.</param>
        /// <param name="entityProperty">The entity property.</param>
        /// <returns>LookupFieldColumnDefinition.</returns>
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
        /// <returns>LookupJoinTableEntity&lt;TLookupEntity, TEntity, TParentRelatedEntity&gt;.</returns>
        public LookupJoinTableEntity<TLookupEntity, TEntity, TParentRelatedEntity> Include<TParentRelatedEntity>(Expression<Func<TRelatedEntity, TParentRelatedEntity>> relatedProperty)
            where TParentRelatedEntity : class

        {
            var parentTable = JoinDefinition.ForeignKeyDefinition.PrimaryTable;
            var relatedPropertyName = relatedProperty.GetFullPropertyName();
            var relatedPropertyType = relatedProperty.ReturnType.Name;

            var returnEntity = new LookupJoinTableEntity<TLookupEntity, TEntity, TParentRelatedEntity>(_lookupEntityDefinition);
            returnEntity.JoinDefinition = JoinDefinition;

            returnEntity.SetJoinDefinition(parentTable, relatedPropertyName, relatedPropertyType);

            //returnEntity.ParentField = ParentField;
            returnEntity.ParentField = returnEntity.JoinDefinition.ForeignKeyDefinition.FieldJoins[0].ForeignField;
            returnEntity.ParentObject = this;
            if (returnEntity.JoinDefinition != null) returnEntity.JoinDefinition.ParentObject = this;

            return returnEntity;
        }
    }
}
