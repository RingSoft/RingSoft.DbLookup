﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Google.Protobuf.WellKnownTypes;
using MySqlX.XDevAPI.Common;
using MySqlX.XDevAPI.Relational;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DbLookup.TableProcessing;

namespace RingSoft.DbLookup.Lookup
{
    public class LookupDataMauiProcessInput<TEntity> where TEntity : class, new()
    {
        public IQueryable<TEntity> Query { get; set; }

        public List<LookupFieldColumnDefinition> OrderByList { get; set; }

        public TableFilterDefinition<TEntity> FilterDefinition { get; set; }

        public Expression LookupExpression { get; set; }

        public ParameterExpression Param { get; set; }

        public List<FieldFilterDefinition> FieldFilters { get; set; }
    }
    public class LookupDataMaui<TEntity> : LookupDataMauiBase where TEntity : class, new()
    {
        public TableDefinition<TEntity> TableDefinition { get; }

        public IQueryable<TEntity> BaseQuery { get; private set; }

        public IQueryable<TEntity> FilteredQuery { get; private set; }

        public IQueryable<TEntity> ProcessedQuery { get; private set; }

        public List<TEntity> CurrentList { get; } = new List<TEntity>();

        public override int RowCount => CurrentList.Count;

        public LookupDataMaui(LookupDefinitionBase lookupDefinition)
            : base(lookupDefinition)
        {
            if (lookupDefinition.TableDefinition is TableDefinition<TEntity> table)
            {
                TableDefinition = table;
            }
            OrderByList.Add(lookupDefinition.InitialOrderByColumn);
        }

        public override void GetInitData()
        {
            RefreshData();
            FireLookupDataChangedEvent(new LookupDataMauiOutput(LookupScrollPositions.Top));
        }

        public override string GetFormattedRowValue(int rowIndex, LookupColumnDefinitionBase column)
        {
            var row = CurrentList[rowIndex];
            return column.GetFormattedValue(row);
        }

        public override string GetDatabaseRowValue(int rowIndex, LookupColumnDefinitionBase column)
        {
            var row = CurrentList[rowIndex];
            return column.GetDatabaseValue(row);
        }

        public override int GetRecordCount()
        {
            var result = 0;
            if (FilteredQuery != null && FilteredQuery.Any())
            {
                result = FilteredQuery.Count();
            }

            return result;
        }

        public override void ClearData()
        {
            CurrentList.Clear();
            FireLookupDataChangedEvent(new LookupDataMauiOutput(LookupScrollPositions.Top));
        }

        public override PrimaryKeyValue GetPrimaryKeyValueForSearchText(string searchText)
        {
            throw new NotImplementedException();
        }

        public override void SelectPrimaryKey(PrimaryKeyValue primaryKeyValue)
        {
            if (LookupWindow != null)
            {
                LookupWindow.SelectPrimaryKey(primaryKeyValue);
            }
        }

        public override void ViewSelectedRow(object ownerWindow, object inputParameter, bool lookupReadOnlyMode = false)
        {
            var selectedIndex = LookupControl.SelectedIndex;
            if (selectedIndex >= 0 && selectedIndex < RowCount)
            {
                SelectedPrimaryKeyValue = GetSelectedPrimaryKeyValue();
                if (LookupWindow == null)
                {
                    var args = new LookupAddViewArgs(this, true, LookupFormModes.View, string.Empty, ownerWindow)
                    {
                        ParentWindowPrimaryKeyValue = ParentWindowPrimaryKeyValue,
                        InputParameter = inputParameter,
                        LookupReadOnlyMode = lookupReadOnlyMode
                    };
                    OnLookupView(args);
                    if (!args.Handled)
                    {
                        args.CallBackToken.RefreshData += LookupCallBack_RefreshData;
                        LookupDefinition.TableDefinition.Context.OnAddViewLookup(args);
                    }
                }
                else
                {
                    LookupWindow.SelectPrimaryKey(SelectedPrimaryKeyValue);
                }
            }
        }

        public override void AddNewRow(object ownerWindow, object inputParameter = null)
        {
            throw new NotImplementedException();
        }

        private void RefreshBaseQuery()
        {
            BaseQuery = LookupDefinition.TableDefinition.Context.GetQueryable<TEntity>(LookupDefinition);
        }

        public override void RefreshData()
        {
            RefreshBaseQuery();

            var param = GblMethods.GetParameterExpression<TEntity>();

            var whereExpression = LookupDefinition.FilterDefinition.GetWhereExpresssion<TEntity>(param);

            if (whereExpression == null)
            {
                FilteredQuery = ProcessedQuery = BaseQuery;
            }
            else
            {
                FilteredQuery = FilterItemDefinition.FilterQuery(BaseQuery, param, whereExpression);
            }

            FilteredQuery = ApplyOrderBys(FilteredQuery, true);

            ProcessedQuery = FilteredQuery.Take(LookupControl.PageSize);

            CurrentList.Clear();

            CurrentList.AddRange(ProcessedQuery);

            FireLookupDataChangedEvent(new LookupDataMauiOutput(LookupScrollPositions.Top));
        }

        public override void OnColumnClick(LookupColumnDefinitionBase column, bool resetSortOrder)
        {
            if (resetSortOrder)
            {
                OrderByList.Clear();
                if (column != LookupDefinition.InitialSortColumnDefinition)
                    OrderByList.Add(column);
                OrderByList.Add(LookupDefinition.InitialSortColumnDefinition);
            }
            else
            {
                if (OrderByList.Contains(column))
                {
                    OrderByList.Remove(column);
                }
                else
                {
                    OrderByList.Add(column);
                }
            }
            GetInitData();
        }

        private LookupScrollPositions GetScrollPosition(LookupDataMauiProcessInput<TEntity> input, IEnumerable<TEntity> list)
        {
            var result = LookupScrollPositions.Middle;

            var entity = list.LastOrDefault();

            var bottomList = new List<TEntity>();

            if (bottomList.Any())
            {
                result = LookupScrollPositions.Middle;
            }

            return result;
        }

        public override string GetSelectedText()
        {
            var entity = TableDefinition.GetEntityFromPrimaryKeyValue(SelectedPrimaryKeyValue);
            var text = LookupDefinition.InitialSortColumnDefinition.GetDatabaseValue(entity);
            return text;
        }

        public override PrimaryKeyValue GetSelectedPrimaryKeyValue()
        {
            if (LookupControl.SelectedIndex < 0)
            {
                return null;
            }

            var entity = CurrentList[LookupControl.SelectedIndex];
            if (entity != null)
            {
                SelectedPrimaryKeyValue = TableDefinition.GetPrimaryKeyValueFromEntity(entity);
                return SelectedPrimaryKeyValue;
            }

            return null;
        }

        public override void GotoTop()
        {
            GetInitData();
        }

        public override void GotoBottom()
        {
            throw new NotImplementedException();
        }

        public override void GotoNextRecord()
        {
            if (!CurrentList.Any())
            {
                return;
            }

            var selectedEntity = CurrentList.LastOrDefault();
            if (selectedEntity != null)
            {
                var entity = GetNearestEntity(selectedEntity, Conditions.GreaterThan);
                if (entity == null)
                {
                    GotoBottom();
                }
                else
                {
                    MakeList(selectedEntity, entity, LookupControl.PageSize, 0);
                }
            }
        }

        public override void GotoPreviousRecord()
        {
            throw new NotImplementedException();
        }

        public override void GotoNextPage()
        {
            if (!CurrentList.Any() || CurrentList.Count < LookupControl.PageSize)
            {
                return;
            }
            var selectedEntity = CurrentList.LastOrDefault();
            if (selectedEntity != null)
            {
                //GetNextPage(selectedEntity, LookupControl.PageSize);
            }
        }

        private TEntity GetNearestEntity(TEntity entity, Conditions condition)
        {
            TEntity result = null;
            var input = GetProcessInput(entity);

            AddPrimaryKeyFieldsToFilter(entity, input);

            while (input.FilterDefinition.FixedFilters.Any())
            {
                var filterItem = input.FilterDefinition.FixedFilters.OfType<FieldFilterDefinition>().LastOrDefault();
                if (filterItem != null)
                {
                    filterItem.Condition = condition;
                }

                var query = GetQueryFromFilter(input.FilterDefinition, input, true);

                query = query.Take(1);

                if (query.Count() == 1)
                {
                    return query.FirstOrDefault();
                }
                else
                {
                    var lastFilter = input.FilterDefinition.FixedFilters.LastOrDefault();
                    if (lastFilter != null)
                    {
                        RemoveFilter(input, lastFilter);
                    }
                }
            }

            return result;
        }

        private void AddPrimaryKeyFieldsToFilter(TEntity entity, LookupDataMauiProcessInput<TEntity> input)
        {
            foreach (var primaryKeyField in TableDefinition.PrimaryKeyFields)
            {
                var fieldFilter = input.FilterDefinition.AddFixedFilter(primaryKeyField, Conditions.Equals
                    , GblMethods.GetPropertyValue(entity, primaryKeyField.PropertyName));
                fieldFilter.SetPropertyName = primaryKeyField.PropertyName;
            }

            input.FieldFilters = input.FilterDefinition.FixedFilters.OfType<FieldFilterDefinition>().ToList();
        }


        private LookupDataMauiProcessInput<TEntity> GetProcessInput(TEntity entity, bool getFilter = true
        , TableFilterDefinition<TEntity> oldFilter = null)
        {
            TableFilterDefinition<TEntity> filterDefinition = null;
            if (oldFilter == null)
            {
                filterDefinition = new TableFilterDefinition<TEntity>(TableDefinition);
            }
            else
            {
                //filterDefinition = oldFilter;
                //getFilter = false;
            }

            var query = TableDefinition.Context.GetQueryable<TEntity>(LookupDefinition);
            var param = GblMethods.GetParameterExpression<TEntity>();
            var orderBys = OrderByList.OfType<LookupFieldColumnDefinition>().ToList();
            var lookupExpr = LookupDefinition.FilterDefinition.GetWhereExpresssion<TEntity>(param);

            var result = new LookupDataMauiProcessInput<TEntity>()
            {
                FilterDefinition = filterDefinition,
                LookupExpression = lookupExpr,
                OrderByList = orderBys,
                Query = query,
                Param = param,
            };

            if (getFilter)
            {
                foreach (var orderBy in orderBys)
                {
                    var value = orderBy.GetDatabaseValue(entity);
                    var field = orderBy.FieldDefinition;
                    var filterItem = filterDefinition.AddFixedFilter(field, Conditions.Equals, value);
                    filterItem.SetPropertyName = orderBy.GetPropertyJoinName();
                    filterItem.LookupColumn = orderBy;
                }
            }

            if (filterDefinition.FixedFilters.Count > 0)
            {
                result.FieldFilters = filterDefinition.FixedFilters.OfType<FieldFilterDefinition>().ToList();
            }

            return result;
        }


        private IQueryable<TEntity> ApplyOrderBys(IQueryable<TEntity> query, bool ascending)
        {
            var orderBys = OrderByList.OfType<LookupFieldColumnDefinition>();
            var first = true;

            var orderByType = OrderMethods.OrderBy;
            var thenByType = OrderMethods.ThenBy;

            if (!ascending)
            {
                orderByType = OrderMethods.OrderByDescending;
                thenByType = OrderMethods.ThenByDescending;
            }
            foreach (var orderBy in orderBys)
            {
                if (first)
                {
                    query = GblMethods.ApplyOrder(query, orderByType, orderBy.GetPropertyJoinName());
                    first = false;
                }
                else
                {
                    query = GblMethods.ApplyOrder(query, thenByType, orderBy.GetPropertyJoinName());
                }
            }

            foreach (var primaryKeyField in TableDefinition.PrimaryKeyFields)
            {
                query = GblMethods.ApplyOrder(query, OrderMethods.ThenBy, primaryKeyField.PropertyName);
            }

            return query;
        }

        private void LookupCallBack_RefreshData(object sender, EventArgs e)
        {
            RefreshData();
            OnDataSourceChanged();
        }

        private void MakeList(TEntity selectedEntity, TEntity entity, int topCount, int bottomCount)
        {
            CurrentList.Clear();
            if (topCount > 0)
            {
                var previousPage = GetPage(entity, topCount, true);
                if (previousPage != null && previousPage.Any())
                {
                    CurrentList.InsertRange(0, previousPage);
                }
                else
                {
                    GotoTop();
                }
            }

            if (bottomCount > 0)
            {
                var nextPage = GetNextPage(entity, bottomCount);
                if (nextPage != null && nextPage.Any())
                {
                    CurrentList.AddRange(nextPage);
                }
                else
                {
                    GotoBottom();
                }
            }
            FireLookupDataChangedEvent(new LookupDataMauiOutput(LookupScrollPositions.Middle));
            LookupControl.SetLookupIndex(CurrentList.Count - 1);
        }

        private List<TEntity> GetPage(TEntity nextEntity, int count
            , bool previous, TableFilterDefinition<TEntity> filter = null)
        {
            var result = new List<TEntity>();
            var input = GetProcessInput(nextEntity, true);

            var query = GetFilterPageQuery(nextEntity, count, input
                , true, out var addedPrimaryKeyToFilter, false);

            result.AddRange(query);
            
            count -= result.Count;

            if (count > 0)
            {
                nextEntity = result.FirstOrDefault();
                var newList = AddAditionalList(input, result, count, addedPrimaryKeyToFilter
                    , nextEntity, !previous);

                if (previous)
                {
                    result.InsertRange(0, newList);
                }
                else
                {
                    result.AddRange(newList);
                }
            }

            return result;
        }

        private IEnumerable<TEntity> GetFilterPageQuery(TEntity entity, int count, LookupDataMauiProcessInput<TEntity> input,
            bool checkPrimaryKey, out bool addedPrimaryKeyToFilter, bool ascending)
        {
            addedPrimaryKeyToFilter = false;
            var hasMoreThan1Record = DoesFilterHaveMoreThan1Record(entity, input, checkPrimaryKey);

            if (hasMoreThan1Record && checkPrimaryKey)
            {
                addedPrimaryKeyToFilter = true;
                AddPrimaryKeyFieldsToFilter(entity, input);
            }

            var query = GetQueryFromFilter(input.FilterDefinition, input, ascending);
            query = query.Take(count);
            var result = GetOutputResult(query, ascending, count);
            return result;
        }

        private bool DoesFilterHaveMoreThan1Record(TEntity entity, LookupDataMauiProcessInput<TEntity> input, bool addFilters)
        {
            var lastFilter = input.FieldFilters.LastOrDefault();
            var hasMoreThan1Record = false;
            foreach (var columnDefinition in input.OrderByList)
            {
                var value = columnDefinition.GetDatabaseValue(entity);
                var orderByFieldFilter = input.FieldFilters
                    .FirstOrDefault(p => p.FieldDefinition == columnDefinition.FieldDefinition);

                if (orderByFieldFilter != null)
                {
                    if (orderByFieldFilter != lastFilter)
                    {
                        orderByFieldFilter.Condition = Conditions.Equals;
                        orderByFieldFilter.Value = value;
                    }
                }
                else if (addFilters)
                {
                    AddFilter(input, columnDefinition, Conditions.Equals, value);
                }

                hasMoreThan1Record = HasMoreThan1Record(input);
                if (!hasMoreThan1Record)
                {
                    break;
                }
            }

            return hasMoreThan1Record;
        }

        private List<TEntity> AddAditionalList(LookupDataMauiProcessInput<TEntity> input, List<TEntity> inputList
        , int count, bool addedPrimaryKey, TEntity topEntity, bool ascending, int filterIndex = 0)
        {
            FieldFilterDefinition lastFilter = null;
            input = GetProcessInput(topEntity);
            if (addedPrimaryKey)
            {
                AddPrimaryKeyFieldsToFilter(topEntity, input);
            }

            var result = new List<TEntity>();

            if (filterIndex > 1 && input.FieldFilters.Count > 1)
            {
                lastFilter= input.FieldFilters.LastOrDefault();
                if (lastFilter != null && input.FieldFilters.Count > 1)
                {
                    RemoveFilter(input, lastFilter);
                }
            }

            lastFilter = input.FieldFilters.LastOrDefault();
            if (lastFilter != null)
            {
                lastFilter.Condition = Conditions.LessThan;
            }

            var output = GetFilterPageQuery(topEntity, count, input
                , false, out addedPrimaryKey, ascending);

            if (output.Count() == 0)
            {
                if (input.FieldFilters.Count > 1)
                {
                    var entity = topEntity;
                    lastFilter = (input.FieldFilters.LastOrDefault());
                    RemoveFilter(input, lastFilter);
                    filterIndex++;
                    var newList = AddAditionalList(input, result, count, false, entity, ascending, filterIndex);
                    result.InsertRange(0, newList);
                }
                return result;
            }

            result.InsertRange(0, output);
            count -= result.Count;

            if (count > 0)
            {
                var entity = result.FirstOrDefault();
                filterIndex++;
                var newList = AddAditionalList(input, result, count, false, entity, ascending, filterIndex);
                result.InsertRange(0, newList);
            }
            return result;
        }

        private IEnumerable<TEntity> GetOutputResult(IQueryable<TEntity> query, bool ascending, int count)
        {
            if (query.Count() == 1)
            {
                return query;
            }
            var result = new List<TEntity>();

            if (ascending)
            {
                result.AddRange(query);
            }
            else
            {
                foreach (var entity in query)
                {
                    if (result.Count == count)
                    {
                        break;
                    }
                    else
                    {
                        result.Insert(0, entity);
                    }
                }
            }

            return result;
        }

        private void AddFilter(LookupDataMauiProcessInput<TEntity> input,
            LookupColumnDefinitionBase column, Conditions condition, string value)
        {
            if (column is LookupFieldColumnDefinition fieldColumn)
            {
                var filterItem = input.FilterDefinition
                    .AddFixedFilter(fieldColumn.FieldDefinition, Conditions.Equals, value);
                input.FieldFilters.Add(filterItem);
                filterItem.PropertyName = column.GetPropertyJoinName();
                filterItem.LookupColumn = column;
            }

            input.FieldFilters = input.FilterDefinition.FixedFilters.OfType<FieldFilterDefinition>().ToList();
        }

        private void RemoveFilter(LookupDataMauiProcessInput<TEntity> input, FilterItemDefinition filterItem)
        {
            if (filterItem is FieldFilterDefinition fieldFilter)
            {
                input.FieldFilters.Remove(fieldFilter);
            }
            input.FilterDefinition.RemoveFixedFilter(filterItem);
            input.FieldFilters = input.FilterDefinition.FixedFilters.OfType<FieldFilterDefinition>().ToList();
        }

        private bool HasMoreThan1Record(LookupDataMauiProcessInput<TEntity> input)
        {
            var result = false;
            var query = GetQueryFromFilter(input.FilterDefinition, input, true);
            var count = query.Count();
            result = count > 1;
            return result;
        }

        private IQueryable<TEntity> GetQueryFromFilter(TableFilterDefinition<TEntity> newFilter
            , LookupDataMauiProcessInput<TEntity> input, bool ascending)
        {
            var filterExpr = newFilter.GetWhereExpresssion<TEntity>(input.Param);

            var fullExpr =
                FilterItemDefinition.AppendExpression(input.LookupExpression
                    , filterExpr, EndLogics.And);

            var query = FilterItemDefinition.FilterQuery(input.Query, input.Param, fullExpr);
            query = ApplyOrderBys(query, ascending);
            return query;
        }

        private List<TEntity> GetNextPage(TEntity entity, int count)
        {
            var result = new List<TEntity>();
            return result;
        }

    }
}
