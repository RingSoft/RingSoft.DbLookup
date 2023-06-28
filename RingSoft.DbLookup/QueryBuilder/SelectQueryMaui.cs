using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using MySqlX.XDevAPI.Relational;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.TableProcessing;
using FieldDefinition = RingSoft.DbLookup.ModelDefinition.FieldDefinitions.FieldDefinition;

namespace RingSoft.DbLookup.QueryBuilder
{
    public class SelectQueryColumnFieldMap
    {
        public LookupFieldColumnDefinition Column { get; internal set; }

        public FieldDefinition Field { get; internal set; }

        public TreeViewItem TreeViewItem { get; internal set; }
    }
    public abstract class SelectQueryMauiBase
    {
        public LookupDefinitionBase LookupDefinition { get; }

        public List<SelectQueryColumnFieldMap> ColumnMaps { get; }

        public int MaxRecords { get; private set; }

        public TableFilterDefinitionBase Filter { get; internal set; }

        public SelectQueryMauiBase(LookupDefinitionBase lookupDefinition)
        {
            LookupDefinition = lookupDefinition.Clone();
            LookupDefinition.AdvancedFindTree = new AdvancedFindTree(LookupDefinition);
            LookupDefinition.AdvancedFindTree.LoadTree(LookupDefinition.TableDefinition.TableName);
            ColumnMaps = new List<SelectQueryColumnFieldMap>();
        }

        public void SetMaxRecords(int value)
        {
            MaxRecords = value;
        }

        public abstract LookupFieldColumnDefinition AddColumn(FieldDefinition fieldDefinition
        , FieldDefinition parentField = null);

        public abstract LookupFieldColumnDefinition AddColumn(TableDefinitionBase tableDefinition
            , FieldDefinition fieldDefinition);

        public abstract FieldFilterDefinition AddFilter(
            LookupFieldColumnDefinition column
            , Conditions condition
            , string value);

        public abstract FieldFilterDefinition AddFilter(
            LookupFieldColumnDefinition column
            , Conditions condition
            , PrimaryKeyValueField value
            , FieldDefinition parentField = null
            , FieldDefinition filterField = null);

        public abstract bool GetData(IDbContext context = null);

        public abstract int RecordCount();

        public abstract bool SetNull(LookupFieldColumnDefinition column, IDbContext context);

        public abstract bool DeleteAllData(IDbContext context);
    }
    public class SelectQueryMaui<TEntity> : SelectQueryMauiBase where TEntity : class, new()
    {
        public TableDefinition<TEntity> TableDefinition { get; }
        public new TableFilterDefinition<TEntity> Filter { get; }

        public IQueryable<TEntity> Result { get; private set; }

        public SelectQueryMaui(LookupDefinitionBase lookupDefinition)
        : base(lookupDefinition)
        {
            TableDefinition = GblMethods.GetTableDefinition<TEntity>();
            Filter = new TableFilterDefinition<TEntity>(TableDefinition);
            base.Filter = Filter;
        }

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
                if (parentField != null && filterField != null && filterField.TableDefinition != parentField.TableDefinition)
                {
                    subItem = LookupDefinition.AdvancedFindTree.FindFieldInTree(
                        LookupDefinition.AdvancedFindTree.TreeRoot
                        , filterField);
                }
                else
                {
                    subItem = LookupDefinition.AdvancedFindTree.FindTableInTree(
                        value.FieldDefinition.TableDefinition
                        , map.TreeViewItem, true, parentField);
                }

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
            }
            catch (Exception e)
            {
                ControlsGlobals.UserInterface.ShowMessageBox(e.Message, "Error!", RsMessageBoxIcons.Error);
                return false;
            }
            return true;
        }

        public override int RecordCount()
        {
            if (Result == null)
            {
                return 0;
            }

            return Result.Count();
        }

        public override bool SetNull(LookupFieldColumnDefinition column, IDbContext context)
        {
            var maxRecords = MaxRecords;
            SetMaxRecords(0);
            var result = GetData(context);
            if (result)
            {
                var records = Result.ToList();
                foreach (var entity in records)
                {
                    //DeleteProperties(entity);

                    if (column != null)
                    {
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

        public override bool DeleteAllData(IDbContext context)
        {
            var maxRecords = MaxRecords;
            SetMaxRecords(0);
            var result = GetData(context);
            if (result)
            {
                var records = Result.ToList();
                foreach (var entity in records)
                {
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
