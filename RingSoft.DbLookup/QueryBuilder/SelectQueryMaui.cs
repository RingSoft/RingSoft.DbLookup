// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 06-17-2023
//
// Last Modified By : petem
// Last Modified On : 07-10-2023
// ***********************************************************************
// <copyright file="SelectQueryMaui.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.TableProcessing;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using FieldDefinition = RingSoft.DbLookup.ModelDefinition.FieldDefinitions.FieldDefinition;

namespace RingSoft.DbLookup.QueryBuilder
{
    /// <summary>
    /// Class SelectQueryColumnFieldMap.
    /// </summary>
    public class SelectQueryColumnFieldMap
    {
        /// <summary>
        /// Gets the column.
        /// </summary>
        /// <value>The column.</value>
        public LookupFieldColumnDefinition Column { get; internal set; }

        /// <summary>
        /// Gets the field.
        /// </summary>
        /// <value>The field.</value>
        public FieldDefinition Field { get; internal set; }

        /// <summary>
        /// Gets the TreeView item.
        /// </summary>
        /// <value>The TreeView item.</value>
        public TreeViewItem TreeViewItem { get; internal set; }
    }
    /// <summary>
    /// Class SelectQueryMauiBase.
    /// </summary>
    public abstract class SelectQueryMauiBase
    {
        /// <summary>
        /// Gets the lookup definition.
        /// </summary>
        /// <value>The lookup definition.</value>
        public LookupDefinitionBase LookupDefinition { get; }

        /// <summary>
        /// Gets the column maps.
        /// </summary>
        /// <value>The column maps.</value>
        public List<SelectQueryColumnFieldMap> ColumnMaps { get; }

        /// <summary>
        /// Gets the maximum records.
        /// </summary>
        /// <value>The maximum records.</value>
        public int MaxRecords { get; private set; }

        /// <summary>
        /// Gets the filter.
        /// </summary>
        /// <value>The filter.</value>
        public TableFilterDefinitionBase Filter { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectQueryMauiBase"/> class.
        /// </summary>
        /// <param name="lookupDefinition">The lookup definition.</param>
        public SelectQueryMauiBase(LookupDefinitionBase lookupDefinition)
        {
            LookupDefinition = lookupDefinition.Clone();
            LookupDefinition.AdvancedFindTree = new AdvancedFindTree(LookupDefinition);
            LookupDefinition.AdvancedFindTree.LoadTree(LookupDefinition.TableDefinition.TableName);
            ColumnMaps = new List<SelectQueryColumnFieldMap>();
        }

        /// <summary>
        /// Sets the maximum records.
        /// </summary>
        /// <param name="value">The value.</param>
        public void SetMaxRecords(int value)
        {
            MaxRecords = value;
        }

        /// <summary>
        /// Adds the column.
        /// </summary>
        /// <param name="fieldDefinition">The field definition.</param>
        /// <param name="parentField">The parent field.</param>
        /// <returns>LookupFieldColumnDefinition.</returns>
        public abstract LookupFieldColumnDefinition AddColumn(FieldDefinition fieldDefinition
        , FieldDefinition parentField = null);

        /// <summary>
        /// Adds the column.
        /// </summary>
        /// <param name="tableDefinition">The table definition.</param>
        /// <param name="fieldDefinition">The field definition.</param>
        /// <returns>LookupFieldColumnDefinition.</returns>
        public abstract LookupFieldColumnDefinition AddColumn(TableDefinitionBase tableDefinition
            , FieldDefinition fieldDefinition);

        /// <summary>
        /// Adds the filter.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <returns>FieldFilterDefinition.</returns>
        public abstract FieldFilterDefinition AddFilter(
            LookupFieldColumnDefinition column
            , Conditions condition
            , string value);

        /// <summary>
        /// Adds the filter.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <param name="parentField">The parent field.</param>
        /// <param name="filterField">The filter field.</param>
        /// <returns>FieldFilterDefinition.</returns>
        public abstract FieldFilterDefinition AddFilter(
            LookupFieldColumnDefinition column
            , Conditions condition
            , PrimaryKeyValueField value
            , FieldDefinition parentField = null
            , FieldDefinition filterField = null);

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public abstract bool GetData(IDbContext context = null);

        /// <summary>
        /// Records the count.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public abstract int RecordCount();

        /// <summary>
        /// Sets the null.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <param name="context">The context.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public abstract bool SetNull(LookupFieldColumnDefinition column, IDbContext context, ITwoTierProcessingProcedure procedure = null);

        /// <summary>
        /// Deletes all data.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public abstract bool DeleteAllData(IDbContext context, ITwoTierProcessingProcedure procedure = null);

        /// <summary>
        /// Gets the data result.
        /// </summary>
        /// <returns>List&lt;PrimaryKeyValue&gt;.</returns>
        public abstract List<PrimaryKeyValue> GetDataResult();

        /// <summary>
        /// Gets the property value.
        /// </summary>
        /// <param name="rowIndex">Index of the row.</param>
        /// <param name="property">The property.</param>
        /// <returns>System.String.</returns>
        public abstract string GetPropertyValue(int rowIndex, string property);
    }
    /// <summary>
    /// Class SelectQueryMaui.
    /// Implements the <see cref="RingSoft.DbLookup.QueryBuilder.SelectQueryMauiBase" />
    /// </summary>
    /// <typeparam name="TEntity">The type of the t entity.</typeparam>
    /// <seealso cref="RingSoft.DbLookup.QueryBuilder.SelectQueryMauiBase" />
    public class SelectQueryMaui<TEntity> : SelectQueryMauiBase where TEntity : class, new()
    {
        /// <summary>
        /// Gets the table definition.
        /// </summary>
        /// <value>The table definition.</value>
        public TableDefinition<TEntity> TableDefinition { get; }
        /// <summary>
        /// Gets the filter.
        /// </summary>
        /// <value>The filter.</value>
        public new TableFilterDefinition<TEntity> Filter { get; }

        /// <summary>
        /// Gets the result.
        /// </summary>
        /// <value>The result.</value>
        public IQueryable<TEntity> Result { get; private set; }
        /// <summary>
        /// Gets the list result.
        /// </summary>
        /// <value>The list result.</value>
        public List<TEntity> ListResult { get; private set; } = new List<TEntity>();

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectQueryMaui{TEntity}"/> class.
        /// </summary>
        /// <param name="lookupDefinition">The lookup definition.</param>
        public SelectQueryMaui(LookupDefinitionBase lookupDefinition)
        : base(lookupDefinition)
        {
            TableDefinition = GblMethods.GetTableDefinition<TEntity>();
            Filter = new TableFilterDefinition<TEntity>(TableDefinition);
            base.Filter = Filter;
        }

        /// <summary>
        /// Adds the column.
        /// </summary>
        /// <param name="fieldDefinition">The field definition.</param>
        /// <param name="parentField">The parent field.</param>
        /// <returns>LookupFieldColumnDefinition.</returns>
        public override LookupFieldColumnDefinition AddColumn(FieldDefinition fieldDefinition,
            FieldDefinition parentField = null)
        {
            TreeViewItem treeItem = null;

            if (parentField != null)
            {
                treeItem = LookupDefinition.AdvancedFindTree.FindFieldInTree(
                    LookupDefinition.AdvancedFindTree.TreeRoot
                    , parentField, false, null, true);
            }

            if (treeItem == null)
            {
                treeItem = LookupDefinition.AdvancedFindTree.FindFieldInTree(
                    LookupDefinition.AdvancedFindTree.TreeRoot
                    , fieldDefinition, false, null, false);
            }
            
            var newColumn = treeItem.CreateColumn(LookupDefinition.VisibleColumns.Count - 1);
            var joinRes = LookupDefinition.AdvancedFindTree.MakeIncludes(treeItem);
            var result = newColumn as LookupFieldColumnDefinition;
            var map = ColumnMaps.FirstOrDefault(p => p.Field == fieldDefinition);
            if (map == null)
            {
                map = new SelectQueryColumnFieldMap()
                {
                    Column = result,
                    Field = fieldDefinition,
                    TreeViewItem = treeItem,
                };
                ColumnMaps.Add(map);
            }
            return result;
        }

        /// <summary>
        /// Adds the column.
        /// </summary>
        /// <param name="tableDefinition">The table definition.</param>
        /// <param name="fieldDefinition">The field definition.</param>
        /// <returns>LookupFieldColumnDefinition.</returns>
        public override LookupFieldColumnDefinition AddColumn(TableDefinitionBase tableDefinition
            , FieldDefinition fieldDefinition)
        {
            var treeItem = LookupDefinition.AdvancedFindTree.FindTableInTree(tableDefinition);

            var newColumn = treeItem.CreateColumn(LookupDefinition.VisibleColumns.Count - 1);
            var joinRes = LookupDefinition.AdvancedFindTree.MakeIncludes(treeItem);
            var result = newColumn as LookupFieldColumnDefinition;
            var map = ColumnMaps.FirstOrDefault(p => p.Field == fieldDefinition);
            if (map == null)
            {
                map = new SelectQueryColumnFieldMap()
                {
                    Column = result,
                    Field = fieldDefinition,
                    TreeViewItem = treeItem,
                };
                ColumnMaps.Add(map);
            }
            return result;
        }

        /// <summary>
        /// Adds the filter.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <returns>FieldFilterDefinition.</returns>
        public override FieldFilterDefinition AddFilter(
            LookupFieldColumnDefinition column
            , Conditions condition
            , string value)
        {
            FieldFilterDefinition result = null;

            result = Filter.AddFixedFilter(column.FieldDefinition, condition, value);
            result.PropertyName = column.GetPropertyJoinName(true);
            result.LookupColumn = column;
            return result;
        }

        /// <summary>
        /// Adds the filter.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <param name="parentField">The parent field.</param>
        /// <param name="filterField">The filter field.</param>
        /// <returns>FieldFilterDefinition.</returns>
        public override FieldFilterDefinition AddFilter(
            LookupFieldColumnDefinition column
            , Conditions condition
            , PrimaryKeyValueField value
            , FieldDefinition parentField = null
            , FieldDefinition filterField = null)
        {
            FieldFilterDefinition result = null;
            var map = ColumnMaps.FirstOrDefault(p => p.Column == column);
            if (map != null)
            {
                TreeViewItem subItem = null;
                var useDbField = true; 

                subItem = LookupDefinition.AdvancedFindTree.FindTableInTree(
                    value.FieldDefinition.TableDefinition
                    , map.TreeViewItem, true, parentField, filterField);

                if (subItem != null)
                {
                    var newColumn = subItem.CreateColumn() as LookupFieldColumnDefinition;
                    result = Filter.AddFixedFilter(newColumn.FieldDefinition, condition, value.Value);
                    result.PropertyName = newColumn.GetPropertyJoinName(true);
                    result.LookupColumn = newColumn;
                    return result;
                }

                result = AddFilter(column, condition, value.Value);
                return result;

            }

            return result;
        }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public override bool GetData(IDbContext context = null)
        {
            try
            {
                var test = this;
                var query = TableDefinition.Context.GetQueryable<TEntity>(LookupDefinition, context);
                var param = GblMethods.GetParameterExpression<TEntity>();
                var expr = Filter.GetWhereExpresssion<TEntity>(param);
                if (expr == null)
                {
                    Result = query;
                }
                else
                {
                    Result = FilterItemDefinition.FilterQuery(query, param, expr);
                }

                if (MaxRecords > 0)
                {
                    Result = Result.Take(MaxRecords);
                }

                ListResult = Result.ToList();
            }
            catch (Exception e)
            {
                ControlsGlobals.UserInterface.ShowMessageBox(e.Message, "Error!", RsMessageBoxIcons.Error);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Records the count.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public override int RecordCount()
        {
            if (Result == null)
            {
                return 0;
            }

            return Result.Count();
        }

        /// <summary>
        /// Sets the null.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <param name="context">The context.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public override bool SetNull(LookupFieldColumnDefinition column, IDbContext context, ITwoTierProcessingProcedure procedure = null)
        {
            var maxRecords = MaxRecords;
            SetMaxRecords(0);
            var result = GetData(context);
            if (result)
            {
                var records = Result.ToList();
                var index = 0;
                var totalProcedure = records.Count;
                foreach (var entity in records)
                {
                    //DeleteProperties(entity);
                    if (column != null)
                    {
                        index++;
                        var bottomText = $"Setting NULL to Record {index} / {totalProcedure}";
                        if (procedure != null)
                        {
                            procedure.SetProgress(0, 0, "", totalProcedure, index, bottomText);
                        }
                        GblMethods.SetPropertyValue(entity, column.GetPropertyJoinName(true), null);
                        result = context.SaveEntity(entity, "Setting Null");
                        if (!result)
                        {
                            return result;
                        }
                    }
                }
            }
            SetMaxRecords(maxRecords);
            return result;
        }

        /// <summary>
        /// Deletes all data.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public override bool DeleteAllData(IDbContext context, ITwoTierProcessingProcedure procedure = null)
        {
            var maxRecords = MaxRecords;
            SetMaxRecords(0);
            var result = GetData(context);
            if (result)
            {
                var index = 0;
                var records = Result.ToList();
                var totalProcedure = records.Count;
                foreach (var entity in records)
                {
                    index++;
                    var bottomText = $"Deleting Record {index} / {totalProcedure}";
                    if (procedure != null)
                    {
                        procedure.SetProgress(0, 0, "", totalProcedure, index, bottomText);
                    }

                    DeleteProperties(entity);

                    result = context.DeleteEntity(entity, "Deleting Record");
                    var test = Filter;
                    if (!result)
                    {
                        var query = LookupDefinition
                            .TableDefinition
                            .Context
                            .GetQueryable<TEntity>(LookupDefinition);
                        return result;
                    }
                }
            }

            SetMaxRecords(maxRecords);
            return result;
        }

        /// <summary>
        /// Gets the data result.
        /// </summary>
        /// <returns>List&lt;PrimaryKeyValue&gt;.</returns>
        public override List<PrimaryKeyValue> GetDataResult()
        {
            var result = new List<PrimaryKeyValue>();
            foreach (var entity in Result)
            {
                var primaryKeyValue = TableDefinition.GetPrimaryKeyValueFromEntity(entity);
                result.Add(primaryKeyValue);
            }
            return result;
        }

        /// <summary>
        /// Gets the property value.
        /// </summary>
        /// <param name="rowIndex">Index of the row.</param>
        /// <param name="property">The property.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.Exception">Invalid Row Index</exception>
        public override string GetPropertyValue(int rowIndex, string property)
        {
            if (rowIndex > ListResult.Count - 1)
            {
                throw new Exception("Invalid Row Index");
            }
            
            return GblMethods.GetPropertyValue(ListResult[rowIndex], property);
        }
        /// <summary>
        /// Deletes the properties.
        /// </summary>
        /// <param name="entity">The entity.</param>
        private void DeleteProperties(TEntity entity)
        {
            foreach (var fieldDefinition in TableDefinition.FieldDefinitions
                         .Where(p => p.ParentJoinForeignKeyDefinition != null))
            {
                GblMethods.SetPropertyValue(entity, fieldDefinition
                    .ParentJoinForeignKeyDefinition
                    .ForeignObjectPropertyName, null);
            }
        }
    }
}
