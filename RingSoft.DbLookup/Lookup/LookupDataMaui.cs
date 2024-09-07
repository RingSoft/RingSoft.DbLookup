// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 06-01-2023
//
// Last Modified By : petem
// Last Modified On : 12-19-2023
// ***********************************************************************
// <copyright file="LookupDataMaui.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DbLookup.TableProcessing;
using RingSoft.Printing.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace RingSoft.DbLookup.Lookup
{
    /// <summary>
    /// Enum LookupOperations
    /// </summary>
    public enum LookupOperations
    {
        /// <summary>
        /// The get initialize data
        /// </summary>
        GetInitData = 0,
        /// <summary>
        /// The page up
        /// </summary>
        PageUp = 1,
        /// <summary>
        /// The page down
        /// </summary>
        PageDown = 2,
        /// <summary>
        /// The record up
        /// </summary>
        RecordUp = 3,
        /// <summary>
        /// The record down
        /// </summary>
        RecordDown = 4,
        /// <summary>
        /// The goto bottom
        /// </summary>
        GotoBottom = 5,
        /// <summary>
        /// The goto top
        /// </summary>
        GotoTop = 6,
        /// <summary>
        /// The wheel forward
        /// </summary>
        WheelForward = 7,
        /// <summary>
        /// The wheel backward
        /// </summary>
        WheelBackward = 8,
        /// <summary>
        /// The search for change
        /// </summary>
        SearchForChange = 9,
    }
    /// <summary>
    /// Splits data into pages based on the lookup definition properties.
    /// </summary>
    /// <typeparam name="TEntity">The type of the t entity.</typeparam>
    public class LookupDataMauiProcessInput<TEntity> where TEntity : class, new()
    {
        /// <summary>
        /// Gets or sets the query.
        /// </summary>
        /// <value>The query.</value>
        public IQueryable<TEntity> Query { get; set; }

        /// <summary>
        /// Gets or sets the order by list.
        /// </summary>
        /// <value>The order by list.</value>
        public List<LookupFieldColumnDefinition> OrderByList { get; set; }

        /// <summary>
        /// Gets or sets the filter definition.
        /// </summary>
        /// <value>The filter definition.</value>
        public TableFilterDefinition<TEntity> FilterDefinition { get; set; }

        /// <summary>
        /// Gets or sets the lookup expression.
        /// </summary>
        /// <value>The lookup expression.</value>
        public Expression LookupExpression { get; set; }

        /// <summary>
        /// Gets or sets the parameter.
        /// </summary>
        /// <value>The parameter.</value>
        public ParameterExpression Param { get; set; }

        /// <summary>
        /// Gets or sets the field filters.
        /// </summary>
        /// <value>The field filters.</value>
        public List<FieldFilterDefinition> FieldFilters { get; set; }
    }
    /// <summary>
    /// Class LookupDataMaui.
    /// Implements the <see cref="RingSoft.DbLookup.Lookup.LookupDataMauiBase" />
    /// </summary>
    /// <typeparam name="TEntity">The type of the t entity.</typeparam>
    /// <seealso cref="RingSoft.DbLookup.Lookup.LookupDataMauiBase" />
    public class LookupDataMaui<TEntity> : LookupDataMauiBase where TEntity : class, new()
    {
        /// <summary>
        /// Gets the table definition.
        /// </summary>
        /// <value>The table definition.</value>
        public TableDefinition<TEntity> TableDefinition { get; }

        /// <summary>
        /// Gets the base query.
        /// </summary>
        /// <value>The base query.</value>
        public IQueryable<TEntity> BaseQuery { get; private set; }

        /// <summary>
        /// Gets the filtered query.
        /// </summary>
        /// <value>The filtered query.</value>
        public IQueryable<TEntity> FilteredQuery { get; private set; }

        /// <summary>
        /// Gets the processed query.
        /// </summary>
        /// <value>The processed query.</value>
        public IQueryable<TEntity> ProcessedQuery { get; private set; }

        /// <summary>
        /// Gets the current list.
        /// </summary>
        /// <value>The current list.</value>
        public List<TEntity> CurrentList { get; } = new List<TEntity>();

        /// <summary>
        /// Gets the row count.
        /// </summary>
        /// <value>The row count.</value>
        public override int RowCount => CurrentList.Count;

        /// <summary>
        /// The order by type
        /// </summary>
        private OrderByTypes _orderByType = OrderByTypes.Ascending;
        /// <summary>
        /// The add initial field
        /// </summary>
        private bool _addInitialField = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupDataMaui{TEntity}" /> class.
        /// </summary>
        /// <param name="lookupDefinition">The lookup definition.</param>
        public LookupDataMaui(LookupDefinitionBase lookupDefinition)
            : base(lookupDefinition)
        {
            if (lookupDefinition.TableDefinition is TableDefinition<TEntity> table)
            {
                TableDefinition = table;
            }

            if (lookupDefinition.InitialOrderByColumn is LookupFieldColumnDefinition fieldColumn)
            {
                OrderByList.Add(fieldColumn);
            }
            OrderByList.AddRange(lookupDefinition.AdditOrderByColumns);

            _orderByType = lookupDefinition.InitialOrderByType;
        }

        /// <summary>
        /// Gets the initialize data.
        /// </summary>
        public override void GetInitData()
        {
            RefreshData();
            FireLookupDataChangedEvent(GetOutputArgs());
        }

        /// <summary>
        /// Gets the formatted row value.
        /// </summary>
        /// <param name="rowIndex">Index of the row.</param>
        /// <param name="column">The column.</param>
        /// <returns>System.String.</returns>
        public override string GetFormattedRowValue(int rowIndex, LookupColumnDefinitionBase column)
        {
            var row = CurrentList[rowIndex];
            return column.GetFormattedValue(row);
        }

        /// <summary>
        /// Gets the database row value.
        /// </summary>
        /// <param name="rowIndex">Index of the row.</param>
        /// <param name="column">The column.</param>
        /// <returns>System.String.</returns>
        public override string GetDatabaseRowValue(int rowIndex, LookupColumnDefinitionBase column)
        {
            var row = CurrentList[rowIndex];
            return column.GetDatabaseValue(row);
        }

        /// <summary>
        /// Gets the record count.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public override int GetRecordCount()
        {
            if (FilteredQuery == null)
            {
                MakeFilteredQuery();
            }
            var result = 0;
            try
            {
                if (FilteredQuery != null && FilteredQuery.Any())
                {
                    result = FilteredQuery.Count();
                }

            }
            catch (Exception e)
            {
                DbDataProcessor.DisplayDataException(e, "Counting Records");
            }
            
            return result;
        }

        /// <summary>
        /// Clears the data.
        /// </summary>
        public override void ClearData()
        {
            CurrentList.Clear();
            FireLookupDataChangedEvent(GetOutputArgs());
        }

        /// <summary>
        /// Gets the primary key value for search text.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <returns>PrimaryKeyValue.</returns>
        public override PrimaryKeyValue GetPrimaryKeyValueForSearchText(string searchText)
        {
            MakeFilteredQuery(false);
            var param = GblMethods.GetParameterExpression<TEntity>();

            var lookupExpr = LookupDefinition.FilterDefinition.GetWhereExpresssion<TEntity>(param);
            var searchColumn = OrderByList.FirstOrDefault() as LookupFieldColumnDefinition;

            var type = searchColumn.FieldToDisplay.FieldType;
            var value = searchText.GetPropertyFilterValue(searchColumn.FieldToDisplay.FieldDataType,
                searchColumn.FieldDefinition.FieldType);


            var filterExpr = FilterItemDefinition.GetBinaryExpression<TEntity>(param,
                searchColumn.GetPropertyJoinName()
                , Conditions.Equals, type, searchText);

            var fullExpr = filterExpr;
            if (lookupExpr != null)
            {
                fullExpr = FilterItemDefinition.AppendExpression(lookupExpr, filterExpr, EndLogics.And);
            }

            var query = FilterItemDefinition.FilterQuery<TEntity>(FilteredQuery, param, fullExpr);
            query = ApplyOrderBys(query, true);

            var entity = query.FirstOrDefault();

            var result = TableDefinition.GetPrimaryKeyValueFromEntity(entity);
            return result;
        }

        /// <summary>
        /// Selects the primary key.
        /// </summary>
        /// <param name="primaryKeyValue">The primary key value.</param>
        public override void SelectPrimaryKey(PrimaryKeyValue primaryKeyValue)
        {
            if (LookupWindow != null)
            {
                LookupWindow.SelectPrimaryKey(primaryKeyValue);
            }

            if (LookupControl != null 
                && LookupControl.PageSize == 1
                && primaryKeyValue != null)
            {
                SelectedPrimaryKeyValue = primaryKeyValue;
                SetNewPrimaryKeyValue(primaryKeyValue);
                SetScrollPosition(GetOutputArgs().ScrollPosition);
            }
        }

        /// <summary>
        /// Sets the new primary key value.
        /// </summary>
        /// <param name="primaryKeyValue">The primary key value.</param>
        public override void SetNewPrimaryKeyValue(PrimaryKeyValue primaryKeyValue)
        {
            var entity = TableDefinition.GetEntityFromPrimaryKeyValue(primaryKeyValue);
            if (entity != null)
            {
                {
                    var filter = new TableFilterDefinition<TEntity>(TableDefinition);
                    IQueryable<TEntity> table = null;
                    //if (TableDefinition.PrimaryKeyFields.Count > 1 && DbMaintenanceMode)
                    //{
                    //    table = TableDefinition.Context.GetQueryableTable(TableDefinition, true);
                    //}
                    //else
                    //{
                        
                    //}
                    table = TableDefinition.Context.GetQueryable<TEntity>(LookupDefinition);
                    foreach (var field in primaryKeyValue.KeyValueFields)
                    {
                        var fieldFilter = filter.AddFixedFilter(field.FieldDefinition, Conditions.Equals
                            , GblMethods.GetPropertyValue(entity, field.FieldDefinition.PropertyName));
                        fieldFilter.SetPropertyName = field.FieldDefinition.PropertyName;
                    }

                    var param = GblMethods.GetParameterExpression<TEntity>();
                    var expr = filter.GetWhereExpresssion<TEntity>(param);
                    var query = FilterItemDefinition.FilterQuery(table, param, expr);
                    entity = query.Take(1).FirstOrDefault();

                    var splitPage = (int)Math.Floor((double)LookupControl.PageSize / 2);
                    var topCount = LookupControl.PageSize - splitPage;
                    var bottomCount = LookupControl.PageSize - topCount;
                    topCount--;

                    MakeList(entity, topCount, bottomCount, false, LookupOperations.SearchForChange);
                    SelectedPrimaryKeyValue = TableDefinition.GetPrimaryKeyValueFromEntity(entity);
                    SetLookupIndexFromEntity(param, entity);
                }
            }
        }

        /// <summary>
        /// Views the selected row.
        /// </summary>
        /// <param name="ownerWindow">The owner window.</param>
        /// <param name="inputParameter">The input parameter.</param>
        /// <param name="lookupReadOnlyMode">if set to <c>true</c> [lookup read only mode].</param>
        public override void ViewSelectedRow(object ownerWindow, object inputParameter, bool lookupReadOnlyMode = false)
        {
            var selectedIndex = LookupControl.SelectedIndex;
            if (selectedIndex >= 0 && selectedIndex < RowCount)
            {
                SelectedPrimaryKeyValue = GetSelectedPrimaryKeyValue();
                if (LookupWindow == null || LookupWindow.ReadOnlyMode)
                {
                    var args = new LookupAddViewArgs(this
                        , true
                        , LookupFormModes.View, string.Empty, ownerWindow)
                    {
                        ParentWindowPrimaryKeyValue = ParentWindowPrimaryKeyValue,
                        InputParameter = inputParameter,
                        LookupReadOnlyMode = lookupReadOnlyMode,
                        SelectedPrimaryKeyValue = SelectedPrimaryKeyValue,
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
                    LookupWindow.OnSelectButtonClick();
                }
            }
        }

        /// <summary>
        /// Adds the new row.
        /// </summary>
        /// <param name="ownerWindow">The owner window.</param>
        /// <param name="inputParameter">The input parameter.</param>
        public override void AddNewRow(object ownerWindow, object inputParameter = null)
        {
            SelectedPrimaryKeyValue = GetSelectedPrimaryKeyValue();
            var args = new LookupAddViewArgs(this
                , true
                , LookupFormModes.Add, string.Empty, ownerWindow)
            {
                ParentWindowPrimaryKeyValue = ParentWindowPrimaryKeyValue,
                InputParameter = inputParameter,
            };
            args.CallBackToken.RefreshData += LookupCallBack_RefreshData;
            LookupDefinition.TableDefinition.Context.OnAddViewLookup(args);
        }

        /// <summary>
        /// Refreshes the base query.
        /// </summary>
        private void RefreshBaseQuery()
        {
            BaseQuery = LookupDefinition.TableDefinition.Context.GetQueryable<TEntity>(LookupDefinition);
        }

        /// <summary>
        /// Refreshes the data.
        /// </summary>
        /// <param name="newText">The new text.</param>
        public override void RefreshData(string newText = "")
        {
            ControlsGlobals.UserInterface.SetWindowCursor(WindowCursorTypes.Wait);
            RefreshBaseQuery();

            MakeFilteredQuery();

            if (!newText.IsNullOrEmpty())
            {
                OnSearchForChange(newText, true);
                return;
            }

            ProcessedQuery = FilteredQuery.Take(LookupControl.PageSize);

            if (LookupControl.PageSize == 1 && FilteredQuery.Any())
            {
                SelectedPrimaryKeyValue = TableDefinition.GetPrimaryKeyValueFromEntity(ProcessedQuery.FirstOrDefault());
                var test = CurrentList;
            }

            CurrentList.Clear();

            try
            {
                CurrentList.AddRange(ProcessedQuery);
            }
            catch (Exception e)
            {
                ControlsGlobals.UserInterface.SetWindowCursor(WindowCursorTypes.Default);
                Console.WriteLine(e);
                ControlsGlobals.UserInterface.ShowMessageBox(e.Message, "Error!", RsMessageBoxIcons.Error);
                return;
            }

            ControlsGlobals.UserInterface.SetWindowCursor(WindowCursorTypes.Default);

            FireLookupDataChangedEvent(GetOutputArgs());
        }

        /// <summary>
        /// Makes the filtered query.
        /// </summary>
        /// <param name="applyOrders">if set to <c>true</c> [apply orders].</param>
        private void MakeFilteredQuery(bool applyOrders = true)
        {
            if (BaseQuery == null)
            {
                RefreshBaseQuery();
            }
            var param = GblMethods.GetParameterExpression<TEntity>();

            var whereExpression = LookupDefinition.FilterDefinition.GetWhereExpresssion<TEntity>(param);

            var condition = Conditions.Contains;

            if (LookupControl != null 
                && LookupControl.SearchType == LookupSearchTypes.Contains)
            {
                var contExpr = GetContainsExpr(param, condition);

                if (whereExpression == null)
                {
                    whereExpression = contExpr;
                }
                else
                {
                    whereExpression = FilterItemDefinition
                        .AppendExpression(whereExpression, contExpr, EndLogics.And);
                }
            }

            if (whereExpression == null)
            {
                FilteredQuery = ProcessedQuery = BaseQuery;
            }
            else
            {
                FilteredQuery = FilterItemDefinition.FilterQuery(BaseQuery, param, whereExpression);
            }

            if (applyOrders)
            {
                FilteredQuery = ApplyOrderBys(FilteredQuery, _orderByType == OrderByTypes.Ascending);
            }
        }

        /// <summary>
        /// Called when [column click].
        /// </summary>
        /// <param name="column">The column.</param>
        /// <param name="resetSortOrder">if set to <c>true</c> [reset sort order].</param>
        public override void OnColumnClick(LookupFieldColumnDefinition column, bool resetSortOrder)
        {
            _addInitialField = false;
            if (resetSortOrder)
            {
                var index = 0;
                if (LookupDefinition.InitialOrderByField != null)
                {
                    if (OrderByList[0].FieldDefinition == LookupDefinition.InitialOrderByField)
                    {
                        index = 1;
                    }
                }
                if (column == OrderByList[index])
                {
                    if (_orderByType == OrderByTypes.Ascending)
                        _orderByType = OrderByTypes.Descending;
                    else
                    {
                        _orderByType = OrderByTypes.Ascending;
                    }
                }
                else
                {
                    _orderByType = OrderByTypes.Ascending;
                }
                OrderByList.Clear();
                if (column != LookupDefinition.InitialSortColumnDefinition)
                    OrderByList.Add(column);

                if (LookupDefinition.InitialSortColumnDefinition is LookupFieldColumnDefinition fieldColumn)
                {
                    OrderByList.Add(fieldColumn);
                }
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

        /// <summary>
        /// Called when [mouse wheel forward].
        /// </summary>
        public override void OnMouseWheelForward()
        {
            if (!CurrentList.Any() || CurrentList.Count < 3 || AtBottom())
            {
                return;
            }

            var selectedEntity = CurrentList[2];
            if (selectedEntity != null)
            {
                CurrentList.Clear();
                CurrentList.AddRange(GetNextRecordSet(selectedEntity, LookupControl.PageSize));
                FireLookupDataChangedEvent(GetOutputArgs());
                LookupControl.SetLookupIndex(LookupControl.PageSize - 1);

            }

        }

        /// <summary>
        /// Called when [mouse wheel back].
        /// </summary>
        public override void OnMouseWheelBack()
        {
            if (!CurrentList.Any() || CurrentList.Count < 3 || AtTop())
            {
                return;
            }

            var selectedEntity = CurrentList[CurrentList.Count - 3];
            if (selectedEntity != null)
            {
                CurrentList.Clear();
                CurrentList.AddRange(GetPreviousRecordSet(selectedEntity, LookupControl.PageSize));
                FireLookupDataChangedEvent(GetOutputArgs());
                LookupControl.SetLookupIndex(0);
            }
        }

        /// <summary>
        /// Determines whether [is there data].
        /// </summary>
        /// <returns><c>true</c> if [is there data]; otherwise, <c>false</c>.</returns>
        public override bool IsThereData()
        {
            MakeFilteredQuery(true);
            try
            {
                return FilteredQuery.Any();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ControlsGlobals.UserInterface.ShowMessageBox(e.Message, "Error!", RsMessageBoxIcons.Error);
            }
            return false;
        }

        /// <summary>
        /// Sets the scroll position.
        /// </summary>
        /// <param name="scrollPosition">The scroll position.</param>
        private void SetScrollPosition(LookupScrollPositions scrollPosition)
        {
            ScrollPosition = scrollPosition;
        }

        /// <summary>
        /// Gets the scroll position.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>LookupScrollPositions.</returns>
        private LookupScrollPositions GetScrollPosition(TEntity entity)
        {
            var result = LookupScrollPositions.Middle;

            var previousRecordSet = GetPreviousRecordSet(entity, 1);
            var nextRecordSet = GetNextRecordSet(entity, 1);

            if (!previousRecordSet.Any())
            {
                result = LookupScrollPositions.Top;
            }

            if (!nextRecordSet.Any())
            {
                result = LookupScrollPositions.Bottom;
            }

            return result;
        }

        /// <summary>
        /// Gets the scroll position.
        /// </summary>
        /// <returns>LookupScrollPositions.</returns>
        private LookupScrollPositions GetScrollPosition()
        {
            var result = LookupScrollPositions.Disabled;
            if (!CurrentList.Any())
            {
                return result;
            }

            if (CurrentList.Count < LookupControl.PageSize)
            {
                return result;
            }

            var previousRecordSet = GetPreviousRecordSet(CurrentList[0], 1);
            var nextRecordSet = GetNextRecordSet(CurrentList[CurrentList.Count - 1], 1);
            var prevEntity = previousRecordSet.FirstOrDefault();
            var nextEntity = nextRecordSet.FirstOrDefault();

            if (LookupControl != null)
            {
                if (LookupControl.PageSize == 1)
                {
                    if (TableDefinition.IsEqualTo(CurrentList[0], nextEntity))
                    {
                        nextEntity = null;
                    }

                    if (TableDefinition.IsEqualTo(CurrentList[0], prevEntity))
                    {
                        prevEntity = null;
                    }

                }
            }
            if (prevEntity != null && nextEntity != null)
            {
                result = LookupScrollPositions.Middle;
            }
            else if (nextEntity != null)
            {
                result = LookupScrollPositions.Top;
            }
            else
            {
                result = LookupScrollPositions.Bottom;
            }
            return result;
        }

        /// <summary>
        /// Gets the previous record set.
        /// </summary>
        /// <param name="startEntity">The start entity.</param>
        /// <param name="recordCount">The record count.</param>
        /// <returns>List&lt;TEntity&gt;.</returns>
        private List<TEntity> GetPreviousRecordSet(TEntity startEntity, int recordCount)
        {
            var result = new List<TEntity>();
            if (_orderByType == OrderByTypes.Ascending)
            {
                result = GetPreviousRecordSet(startEntity, recordCount, true);
            }
            else
            {
                result = GetNextRecordSet(startEntity, recordCount, false);
            }

            return result;
        }

        /// <summary>
        /// Gets the previous record set.
        /// </summary>
        /// <param name="startEntity">The start entity.</param>
        /// <param name="recordCount">The record count.</param>
        /// <param name="rsAscending">if set to <c>true</c> [rs ascending].</param>
        /// <returns>List&lt;TEntity&gt;.</returns>
        private List<TEntity> GetPreviousRecordSet(TEntity startEntity, int recordCount, bool rsAscending)
        {
            var result = new List<TEntity>();

            var input = GetProcessInput(startEntity);

            if (HasMoreThan1Record(input))
            {
                AddPrimaryKeyFieldsToFilter(startEntity, input);
            }

            var getPreviousRecords = true;
            while (getPreviousRecords)
            {
                var queryCount = recordCount - result.Count;
                var lastFilter = input.FieldFilters.LastOrDefault();
                if (lastFilter == null)
                {
                    break;
                }

                var ascending = false;
                lastFilter.Condition = Conditions.LessThan;

                var doQuery = true;
                IQueryable<TEntity> query = null;
                if (recordCount == 1 && lastFilter.IsNullFilter())
                {
                    doQuery = false;
                }

                if (doQuery)
                {
                    if (lastFilter.IsNullFilter() && input.FieldFilters.Count > 1)
                    {
                        doQuery = false;
                    }
                }

                if (doQuery)
                {
                    query = GetQueryFromFilter(input.FilterDefinition, input, ascending);
                }

                if (query == null)
                {
                    RemoveFilter(input, lastFilter);
                    continue;
                }
                else
                {
                    query = query.Take(queryCount);
                    if (query.Count() > 0)
                    {
                        var newList = GetOutputResult(query, !rsAscending, queryCount, input);
                        if (rsAscending)
                        {
                            if (recordCount > 1)
                            {
                                var topEntity = newList.FirstOrDefault();
                                if (GetScrollPosition(topEntity) == LookupScrollPositions.Top)
                                {
                                    result.Clear();
                                    IntGotoTop();
                                    return result;
                                }
                            }

                            result.InsertRange(0, newList);
                        }
                        else
                        {
                            if (recordCount > 1)
                            {
                                var bottomEntity = newList.LastOrDefault();
                                var scrollPosition = GetScrollPosition(bottomEntity);
                                if (scrollPosition == LookupScrollPositions.Bottom)
                                {
                                    result.Clear();
                                    IntGotoBottom(LookupOperations.GetInitData);
                                    return result;
                                }
                            }

                            result.AddRange(newList);
                        }
                    }

                    if (lastFilter.IsNullableFilter() && query.Count() < queryCount)
                    {
                        if (!lastFilter.IsNullFilter())
                        {
                            lastFilter.Condition = Conditions.EqualsNull;
                        }

                        queryCount -= query.Count();
                        query = GetQueryFromFilter(input.FilterDefinition, input, ascending);
                        query = query.Take(queryCount);
                        if (query.Count() == 0)
                        {
                            RemoveFilter(input, lastFilter);
                            continue;
                        }
                        else
                        {
                            var newList = GetOutputResult(query, !rsAscending, queryCount, input);
                            if (rsAscending)
                            {
                                result.InsertRange(0, newList);
                            }
                            else
                            {
                                result.AddRange(newList);
                            }
                        }
                    }

                    RemoveFilter(input, lastFilter);
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the next record set.
        /// </summary>
        /// <param name="startEntity">The start entity.</param>
        /// <param name="recordCount">The record count.</param>
        /// <returns>List&lt;TEntity&gt;.</returns>
        private List<TEntity> GetNextRecordSet(TEntity startEntity, int recordCount)
        {
            var result = new List<TEntity>();
            if (_orderByType == OrderByTypes.Ascending)
            {
                result = GetNextRecordSet(startEntity, recordCount, true);
            }
            else
            {
                result = GetPreviousRecordSet(startEntity, recordCount, false);
            }
            return result;
        }

        /// <summary>
        /// Gets the next record set.
        /// </summary>
        /// <param name="startEntity">The start entity.</param>
        /// <param name="recordCount">The record count.</param>
        /// <param name="rsAscending">if set to <c>true</c> [rs ascending].</param>
        /// <returns>List&lt;TEntity&gt;.</returns>
        private List<TEntity> GetNextRecordSet(TEntity startEntity, int recordCount, bool rsAscending)
        {
            var result = new List<TEntity>();
            var firstFilterIsPrimaryKey = false;
            var input = GetProcessInput(startEntity);

            if (HasMoreThan1Record(input))
            {
                AddPrimaryKeyFieldsToFilter(startEntity, input);
                firstFilterIsPrimaryKey = true;
            }

            var getNextRecords = true;
            while (getNextRecords)
            {
                var queryCount = recordCount - result.Count;
                var lastFilter = input.FieldFilters.LastOrDefault();
                if (lastFilter == null || queryCount <= 0)
                {
                    break;
                }

                lastFilter.Condition = Conditions.GreaterThan;
                if (lastFilter.IsNullFilter())
                {
                    lastFilter.Condition = Conditions.NotEqualsNull;
                }

                var doQuery = true;
                IQueryable<TEntity> query = null;

                if (doQuery)
                {
                    query = GetQueryFromFilter(input.FilterDefinition, input, true);
                }

                if (query == null)
                {
                    RemoveFilter(input, lastFilter);
                    break;
                }
                else
                {
                    query = query.Take(queryCount);
                    var parseQuery = query.Count() > 0;

                    if (parseQuery)
                    {
                        var newList = GetOutputResult(query, rsAscending, queryCount, input);
                        if (rsAscending)
                        {
                            if (recordCount > 1)
                            {
                                var bottomEntity = newList.LastOrDefault();
                                var scrollPosition = GetScrollPosition(bottomEntity);
                                if (scrollPosition == LookupScrollPositions.Bottom)
                                {
                                    result.Clear();
                                    IntGotoBottom(LookupOperations.GetInitData);
                                    return result;
                                }
                            }
                            result.AddRange(newList);
                        }
                        else
                        {
                            if (recordCount > 1)
                            {
                                var topEntity = newList.FirstOrDefault();
                                if (GetScrollPosition(topEntity) == LookupScrollPositions.Top)
                                {
                                    result.Clear();
                                    IntGotoTop();
                                    return result;
                                }
                            }

                            result.InsertRange(0, newList);
                        }
                    }
                }

                RemoveFilter(input, lastFilter);
            }

            return result;
        }


        /// <summary>
        /// Gets the selected text.
        /// </summary>
        /// <returns>System.String.</returns>
        public override string GetSelectedText()
        {
            var autoFillValue = LookupDefinition.GetAutoFillValue(SelectedPrimaryKeyValue);
            var text = autoFillValue?.Text;
            //var entity = TableDefinition.GetEntityFromPrimaryKeyValue(SelectedPrimaryKeyValue);
            //var text = LookupDefinition.InitialSortColumnDefinition.GetDatabaseValue(entity);
            return text;
        }

        /// <summary>
        /// Gets the selected primary key value.
        /// </summary>
        /// <returns>PrimaryKeyValue.</returns>
        public override PrimaryKeyValue GetSelectedPrimaryKeyValue()
        {
            if (LookupControl.SelectedIndex < 0)
            {
                var newPrimaryKey = new PrimaryKeyValue(TableDefinition);
                return newPrimaryKey;
            }

            var entity = CurrentList[LookupControl.SelectedIndex];
            if (entity != null)
            {
                SelectedPrimaryKeyValue = TableDefinition.GetPrimaryKeyValueFromEntity(entity);
                return SelectedPrimaryKeyValue;
            }

            return null;
        }

        /// <summary>
        /// Gotoes the top.
        /// </summary>
        public override void GotoTop()
        {
            if (AtTop())
            {
                return;
            }
            IntGotoTop();
            LookupControl.SetLookupIndex(0);
        }

        /// <summary>
        /// Ints the goto top.
        /// </summary>
        private void IntGotoTop()
        {
            GetInitData();
        }

        /// <summary>
        /// Gotoes the bottom.
        /// </summary>
        public override void GotoBottom()
        {
            if (AtBottom())
            {
                return;
            }
            IntGotoBottom(LookupOperations.GetInitData);
            FireLookupDataChangedEvent(GetOutputArgs());
            LookupControl.SetLookupIndex(LookupControl.PageSize - 1);
        }

        /// <summary>
        /// Ints the goto bottom.
        /// </summary>
        /// <param name="operation">The operation.</param>
        private void IntGotoBottom(LookupOperations operation)
        {
            MakeFilteredQuery();
            var entity = FilteredQuery.LastOrDefault();
            CurrentList.Clear();
            CurrentList.AddRange(GetPreviousRecordSet(entity, LookupControl.PageSize - 1));
            SelectedPrimaryKeyValue = TableDefinition.GetPrimaryKeyValueFromEntity(entity);
            if (CurrentList.Count == LookupControl.PageSize - 1)
            {
                var lastEntity = CurrentList.LastOrDefault();
                if (!entity.IsEqualTo(lastEntity))
                {
                    CurrentList.Add(entity);
                }
            }
        }

        /// <summary>
        /// Ats the bottom.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool AtBottom()
        {
            var result = false;
            if (LookupControl.PageSize == 1)
            {
                return result;
            }

            switch (ScrollPosition)
            {
                case LookupScrollPositions.Bottom:
                case LookupScrollPositions.Disabled:
                    result = true; 
                    break;
            }
            return result;
        }

        /// <summary>
        /// Ats the top.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool AtTop()
        {
            var result = false;
            if (LookupControl.PageSize == 1)
            {
                return result;
            }
            switch (ScrollPosition)
            {
                case LookupScrollPositions.Top:
                case LookupScrollPositions.Disabled:
                    result = true;
                    break;
            }
            return result;
        }


        /// <summary>
        /// Gotoes the next record.
        /// </summary>
        public override void GotoNextRecord()
        {
            if (!CurrentList.Any() || AtBottom())
            {
                return;
            }

            var selectedEntity = CurrentList.FirstOrDefault();
            if (selectedEntity != null)
            {
                CurrentList.Clear();
                CurrentList.AddRange(GetNextRecordSet(selectedEntity, LookupControl.PageSize));
                if (LookupControl.PageSize == 1 && CurrentList.Any())
                {
                    SelectedPrimaryKeyValue = TableDefinition.GetPrimaryKeyValueFromEntity(CurrentList.LastOrDefault());
                }
                FireLookupDataChangedEvent(GetOutputArgs());
                LookupControl.SetLookupIndex(LookupControl.PageSize - 1);
            }
        }

        /// <summary>
        /// Gotoes the previous record.
        /// </summary>
        public override void GotoPreviousRecord()
        {
            if (!CurrentList.Any() || AtTop())
            {
                return;
            }

            var selectedEntity = CurrentList.LastOrDefault();
            if (selectedEntity != null)
            {
                CurrentList.Clear();
                CurrentList.AddRange(GetPreviousRecordSet(selectedEntity, LookupControl.PageSize));
                if (LookupControl.PageSize == 1 && CurrentList.Any())
                {
                    SelectedPrimaryKeyValue = TableDefinition.GetPrimaryKeyValueFromEntity(CurrentList.FirstOrDefault());
                }

                FireLookupDataChangedEvent(GetOutputArgs());
                LookupControl.SetLookupIndex(0);
            }
        }

        /// <summary>
        /// Gotoes the next page.
        /// </summary>
        public override void GotoNextPage()
        {
            if (!CurrentList.Any() || AtBottom())
            {
                return;
            }

            var selectedEntity = CurrentList.LastOrDefault();
            if (selectedEntity != null)
            {
                {
                    CurrentList.Clear();
                    CurrentList.AddRange(GetNextRecordSet(selectedEntity, LookupControl.PageSize));
                    FireLookupDataChangedEvent(GetOutputArgs());
                }
                LookupControl.SetLookupIndex(LookupControl.PageSize - 1);
            }
        }

        /// <summary>
        /// Gotoes the previous page.
        /// </summary>
        public override void GotoPreviousPage()
        {
            if (!CurrentList.Any() || AtTop())
            {
                return;
            }

            var selectedEntity = CurrentList.FirstOrDefault();
            if (selectedEntity != null)
            {
                CurrentList.Clear();
                CurrentList.AddRange(GetPreviousRecordSet(selectedEntity, LookupControl.PageSize));
                FireLookupDataChangedEvent(GetOutputArgs());
                LookupControl.SetLookupIndex(0);
            }
        }

        /// <summary>
        /// Called when [search for change].
        /// </summary>
        /// <param name="searchForText">The search for text.</param>
        /// <param name="initialValue">if set to <c>true</c> [initial value].</param>
        public override void OnSearchForChange(string searchForText, bool initialValue = false)
        {
            if (searchForText.IsNullOrEmpty())
            {
                GotoTop();
                return;
            }

            ControlsGlobals.UserInterface.SetWindowCursor(WindowCursorTypes.Wait);
            MakeFilteredQuery(false);

            var param = GblMethods.GetParameterExpression<TEntity>();

            var lookupExpr = LookupDefinition.FilterDefinition.GetWhereExpresssion<TEntity>(param);

            Expression filterExpr = null;
            if (LookupControl.SearchType == LookupSearchTypes.Contains)
            {
                var containsExpr = GetContainsExpr(param);
                filterExpr = containsExpr;
            }
            var searchColumn = OrderByList.FirstOrDefault() as LookupFieldColumnDefinition;

            if (searchColumn != null)
            {
                if (LookupDefinition.InitialOrderByField != null 
                    && LookupDefinition.InitialOrderByField == searchColumn.FieldDefinition)
                {
                    searchColumn = OrderByList[1];
                }
            }
            var type = searchColumn.FieldToDisplay.FieldType;
            var value = searchForText.GetPropertyFilterValue(searchColumn.FieldToDisplay.FieldDataType,
                searchColumn.FieldDefinition.FieldType);

            if (filterExpr == null)
            {
                var condition = Conditions.GreaterThanEquals;
                if (_orderByType == OrderByTypes.Descending)
                {
                    condition = Conditions.LessThanEquals;
                }
                if (type == typeof(string))
                {
                    filterExpr = FilterItemDefinition.GetBinaryExpression<TEntity>(param,
                        searchColumn.GetPropertyJoinName()
                        , Conditions.BeginsWith, type, searchForText);
                }
                else
                {
                    filterExpr = FilterItemDefinition.GetBinaryExpression<TEntity>(param, searchColumn.GetPropertyJoinName()
                        , condition, type, value);

                }
            }

            var fullExpr = FilterItemDefinition.AppendExpression(lookupExpr, filterExpr, EndLogics.And);

            var query = FilterItemDefinition.FilterQuery<TEntity>(FilteredQuery, param, fullExpr);
            var ascending = true;
            if (_orderByType == OrderByTypes.Descending)
            {
                ascending = false;
            }
            query = ApplyOrderBys(query, ascending);

            var entity = query.FirstOrDefault();

            if (LookupControl.SearchType == LookupSearchTypes.Contains)
            {
                var contQuery = query.Take(LookupControl.PageSize);
                CurrentList.Clear();
                CurrentList.AddRange(contQuery);
                FireLookupDataChangedEvent(GetOutputArgs());
                if (CurrentList.Any())
                {
                    LookupControl.SetLookupIndex(0);
                }
                ControlsGlobals.UserInterface.SetWindowCursor(WindowCursorTypes.Default);
                return;
            }

            if (entity == null || searchForText.IsNullOrEmpty())
            {
                if (initialValue)
                {
                    GetInitData();
                }
                ControlsGlobals.UserInterface.SetWindowCursor(WindowCursorTypes.Default);
                return;
            }

            var splitPage = (int)Math.Floor((double)LookupControl.PageSize / 2);
            var topCount = LookupControl.PageSize - splitPage;
            var bottomCount = LookupControl.PageSize - topCount;

            topCount--;
            MakeList(entity, topCount, bottomCount, false, LookupOperations.SearchForChange);
            SelectedPrimaryKeyValue = TableDefinition.GetPrimaryKeyValueFromEntity(entity);

            SetLookupIndexFromEntity(param, entity);

            ControlsGlobals.UserInterface.SetWindowCursor(WindowCursorTypes.Default);
        }

        /// <summary>
        /// Does the print output.
        /// </summary>
        /// <param name="pageSize">Size of the page.</param>
        public override void DoPrintOutput(int pageSize)
        {
            var printOutput = new LookupDataMauiPrintOutput();

            MakeFilteredQuery();

            var counter = 0;
            foreach (var entity in FilteredQuery)
            {
                counter++;
                var primaryKey = TableDefinition.GetPrimaryKeyValueFromEntity(entity);
                printOutput.Result.Add(primaryKey);

                if (counter % pageSize == 0)
                {
                    counter = 0;
                    FirePrintOutputEvent(printOutput);
                    printOutput.Result.Clear();
                }

                if (printOutput.Abort)
                {
                    return;
                }
            }

            if (printOutput.Result.Any())
            {
                FirePrintOutputEvent(printOutput);
                printOutput.Result.Clear();
            }
        }

        /// <summary>
        /// Gets the printer header row.
        /// </summary>
        /// <param name="primaryKeyValue">The primary key value.</param>
        /// <param name="printerSetupArgs">The printer setup arguments.</param>
        /// <returns>PrintingInputHeaderRow.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public override PrintingInputHeaderRow GetPrinterHeaderRow(PrimaryKeyValue primaryKeyValue, PrinterSetupArgs printerSetupArgs)
        {
            var headerRow = new PrintingInputHeaderRow();
            var entity = GetEntityForPrimaryKey(primaryKeyValue) as TEntity;
            headerRow.RowKey = LookupDefinition.InitialSortColumnDefinition
                .FormatColumnForHeaderRowKey(primaryKeyValue, entity);
            var columnMapId = 0;
            foreach (var columnMap in printerSetupArgs.ColumnMaps)
            {
                var value = columnMap.ColumnDefinition.GetDatabaseValue(entity);
                
                var originalValue = value;
                if (columnMap.ColumnDefinition.SearchForHostId.HasValue)
                {
                    value = DbDataProcessor.UserInterface.FormatValue(value,
                        columnMap.ColumnDefinition.SearchForHostId.Value);
                }

                if (value == originalValue)
                {
                    value = columnMap.ColumnDefinition.FormatValueForColumnMap(value);
                }

                switch (columnMap.ColumnType)
                {
                    case PrintColumnTypes.String:
                        PrintingInteropGlobals.HeaderProcessor.SetStringValue(headerRow
                            , columnMap.StringFieldIndex, value);
                        break;
                    case PrintColumnTypes.Number:
                        PrintingInteropGlobals.HeaderProcessor.SetNumberValue(headerRow
                            , columnMap.NumericFieldIndex, value);
                        break;
                    case PrintColumnTypes.Memo:
                        PrintingInteropGlobals.HeaderProcessor.SetMemoValue(headerRow
                            , columnMap.MemoFieldIndex, value);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                columnMapId++;
            }

            return headerRow;
        }

        /// <summary>
        /// Gets the entity for primary key.
        /// </summary>
        /// <param name="primaryKeyValue">The primary key value.</param>
        /// <returns>System.Object.</returns>
        public override object GetEntityForPrimaryKey(PrimaryKeyValue primaryKeyValue)
        {
            if (BaseQuery == null)
            {
                RefreshBaseQuery();
            }
            var entity = TableDefinition.GetEntityFromPrimaryKeyValue(primaryKeyValue);
            var filter = new TableFilterDefinition<TEntity>(TableDefinition);
            foreach (var primaryKeyField in TableDefinition.PrimaryKeyFields)
            {
                var fieldFilter = filter.AddFixedFilter(primaryKeyField, Conditions.Equals
                    , GblMethods.GetPropertyValue(entity, primaryKeyField.PropertyName));
                fieldFilter.SetPropertyName = primaryKeyField.PropertyName;
            }

            var param = GblMethods.GetParameterExpression<TEntity>();
            var expr = filter.GetWhereExpresssion<TEntity>(param);
            var query = FilterItemDefinition.FilterQuery(BaseQuery, param, expr);
            return query.FirstOrDefault();
        }

        /// <summary>
        /// Called when [size changed].
        /// </summary>
        public override void OnSizeChanged()
        {
            RefreshData(LookupControl.SearchText);
        }

        /// <summary>
        /// Sets the lookup index from entity.
        /// </summary>
        /// <param name="param">The parameter.</param>
        /// <param name="entity">The entity.</param>
        private void SetLookupIndexFromEntity(ParameterExpression param, TEntity entity)
        {
            //if (TableDefinition.PrimaryKeyFields.Count > 1)
            //{
            //    return;
            //}

            //var searchColumn = OrderByList.FirstOrDefault() as LookupFieldColumnDefinition;
            //var propertyName = searchColumn.GetPropertyJoinName();
            //var dbValue = searchColumn.GetDatabaseValue(entity).GetPropertyFilterValue(searchColumn
            //    .FieldToDisplay.FieldDataType, searchColumn.FieldToDisplay.FieldType);
            //Expression nullExpr = null;
            //if (dbValue != null && searchColumn.AllowNulls)
            //{
            //    nullExpr = FilterItemDefinition.GetBinaryExpression<TEntity>
            //    (param, searchColumn.GetPropertyJoinName(true)
            //        , Conditions.NotEqualsNull
            //        , searchColumn.FieldDefinition.FieldType);
            //}
            //IQueryable<TEntity> query;
            //var searchExpr = FilterItemDefinition.GetBinaryExpression<TEntity>(param
            //    , propertyName
            //    , Conditions.Equals, searchColumn.FieldToDisplay.FieldType
            //    , dbValue);
            //if (nullExpr != null)
            //{
            //    searchExpr = FilterItemDefinition.AppendExpression(nullExpr, searchExpr, EndLogics.And);
            //}

            var searchPrimaryKeyFilter = new TableFilterDefinition<TEntity>(TableDefinition);
            foreach (var primaryKeyField in TableDefinition.PrimaryKeyFields)
            {
                searchPrimaryKeyFilter.AddFixedFilter(primaryKeyField
                    , Conditions.Equals
                    , GblMethods.GetPropertyValue(entity, primaryKeyField.PropertyName));
            }

            var primaryKeyExpr = searchPrimaryKeyFilter.GetWhereExpresssion<TEntity>(param);
            //searchExpr = FilterItemDefinition.AppendExpression(searchExpr
            //    , primaryKeyExpr
            //    , EndLogics.And);

            var queryable = CurrentList.AsQueryable();
            var query = FilterItemDefinition.FilterQuery(queryable, param, primaryKeyExpr);
            entity = query.FirstOrDefault();
            if (entity != null)
            {
                LookupControl.SetLookupIndex(CurrentList.IndexOf(entity));
            }
        }

        /// <summary>
        /// Gets the output arguments.
        /// </summary>
        /// <returns>LookupDataMauiOutput.</returns>
        public LookupDataMauiOutput GetOutputArgs()
        {
            var scrollPosition = GetScrollPosition();
            var result = new LookupDataMauiOutput(scrollPosition);
            return result;
        }

        /// <summary>
        /// Gets the nearest entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="condition">The condition.</param>
        /// <returns>TEntity.</returns>
        private TEntity GetNearestEntity(TEntity entity, Conditions condition)
        {
            TEntity result = null;
            if (entity == null)
            {
                return result;
            }

            if (_orderByType == OrderByTypes.Descending)
            {
                if (condition == Conditions.GreaterThan)
                {
                    condition = Conditions.LessThan;
                }
                else
                {
                    condition = Conditions.GreaterThan;
                }
            }
            var input = GetProcessInput(entity);
            var lastFilter1 = input.FieldFilters.LastOrDefault();
            var checkNull = false;
            //if (lastFilter1 != null)
            //{
            //    if (lastFilter1.FieldDefinition.AllowNulls
            //        && lastFilter1.Value.IsNullOrEmpty())
            //    {
            //        lastFilter1.Condition = Conditions.EqualsNull;
            //        checkNull = true;
            //    }
            //}

            AddPrimaryKeyFieldsToFilter(entity, input);

            var ascending = condition == Conditions.GreaterThan;
            var removeFilter = true;
            while (input.FilterDefinition.FixedFilters.Any())
            {
                var filterItem = input.FilterDefinition.FixedFilters.OfType<FieldFilterDefinition>().LastOrDefault();
                if (filterItem != null && !filterItem.IsNullFilter())
                {
                    filterItem.Condition = condition;
                }
                
                var query = GetQueryFromFilter(input.FilterDefinition, input, ascending);

                query = query.Take(1);

                if (query.Count() == 1)
                {
                    return query.FirstOrDefault();
                }
                else
                {
                    if (input.FieldFilters.Any())
                    {
                        if (input.FieldFilters.FirstOrDefault().IsNullFilter())
                        {
                            if (!ascending)
                            {
                                return null;
                            }
                        }
                    }
                    var lastFilter = input.FieldFilters.LastOrDefault();
                    if (lastFilter != null)
                    {
                        if (lastFilter.IsNullableFilter())
                        {
                            var queryData = false;
                            if (lastFilter.Value.IsNullOrEmpty())
                            {
                                if (_orderByType == OrderByTypes.Descending)
                                {
                                    if (ascending)
                                    {
                                        lastFilter.Condition = Conditions.NotEqualsNull;
                                        queryData = true;
                                    }
                                }
                            }
                            else
                            {
                                //if (_orderByType == OrderByTypes.Ascending)
                                {
                                    if (!ascending)
                                    {
                                        lastFilter.Condition = Conditions.EqualsNull;
                                        queryData = true;
                                    }
                                }
                            }

                            if (queryData)
                            {
                                query = GetQueryFromFilter(input.FilterDefinition, input, ascending);
                                query = query.Take(1);

                                if (query.Count() == 1)
                                {
                                    return query.FirstOrDefault();
                                }
                            }
                        }
                        if (input.FilterDefinition.FixedFilters.Count > 1)
                        {
                            var filterIndex = input.FilterDefinition
                                .FixedBundle.IndexOf(lastFilter);
                            var nullFilter = input
                                .FieldFilters[filterIndex - 1];
                            if (nullFilter
                                .Value.IsNullOrEmpty())
                            {
                                var oldNullCondition = nullFilter.Condition;
                                var nullCondition = GetNullCondition(ascending);

                                if (oldNullCondition == nullCondition
                                    && lastFilter.IsPrimaryKey
                                    && lastFilter.Condition == condition)
                                {
                                    return result;
                                }


                                nullFilter.Condition = nullCondition;

                                removeFilter = false;
                                if (lastFilter.IsPrimaryKey)
                                {
                                    removeFilter = true;
                                }

                                if (lastFilter is FieldFilterDefinition fieldFilter)
                                {
                                    fieldFilter.Condition = condition;
                                }
                            }
                        }
                        else
                        {
                            removeFilter = true;
                        }
                        if (removeFilter)
                        {
                            RemoveFilter(input, lastFilter);
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the null condition.
        /// </summary>
        /// <param name="ascending">if set to <c>true</c> [ascending].</param>
        /// <returns>Conditions.</returns>
        private Conditions GetNullCondition(bool ascending)
        {
            var nullCondition = Conditions.EqualsNull;
            if (_orderByType == OrderByTypes.Ascending)
            {
                if (ascending)
                {
                    nullCondition = Conditions.NotEqualsNull;
                }
                else
                {
                    nullCondition = Conditions.EqualsNull;
                }
            }
            else
            {
                if (ascending)
                {
                    nullCondition = Conditions.EqualsNull;
                }
                else
                {
                    nullCondition = Conditions.NotEqualsNull;
                }
            }

            return nullCondition;
        }

        /// <summary>
        /// Adds the primary key fields to filter.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="input">The input.</param>
        private void AddPrimaryKeyFieldsToFilter(TEntity entity, LookupDataMauiProcessInput<TEntity> input)
        {
            foreach (var primaryKeyField in TableDefinition.PrimaryKeyFields)
            {
                var fieldFilter = input.FilterDefinition.AddFixedFilter(primaryKeyField, Conditions.Equals
                    , GblMethods.GetPropertyValue(entity, primaryKeyField.PropertyName));
                fieldFilter.SetPropertyName = primaryKeyField.PropertyName;
                fieldFilter.IsPrimaryKey = true;
            }

            input.FieldFilters = input.FilterDefinition.FixedFilters.OfType<FieldFilterDefinition>().ToList();
        }


        /// <summary>
        /// Gets the process input.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="getFilter">if set to <c>true</c> [get filter].</param>
        /// <param name="oldFilter">The old filter.</param>
        /// <returns>LookupDataMauiProcessInput&lt;TEntity&gt;.</returns>
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
                    if (orderBy.FieldDefinition.Description == "Description")
                    {
                        
                    }
                    var value = orderBy.GetDatabaseValue(entity);
                    var field = orderBy.FieldDefinition;
                    var filterItem = filterDefinition.AddFixedFilter(field, Conditions.Equals, value);
                    filterItem.SetPropertyName = orderBy.GetPropertyJoinName();
                    filterItem.LookupColumn = orderBy;
                    filterItem.FieldToSearch = orderBy.FieldToDisplay;
                    filterItem.NavigationProperties = orderBy.GetNavigationProperties();
                }
            }

            if (filterDefinition.FixedFilters.Count > 0)
            {
                result.FieldFilters = filterDefinition.FixedFilters.OfType<FieldFilterDefinition>().ToList();
            }

            return result;
        }


        /// <summary>
        /// Applies the order bys.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="ascending">if set to <c>true</c> [ascending].</param>
        /// <returns>IQueryable&lt;TEntity&gt;.</returns>
        private IQueryable<TEntity> ApplyOrderBys(IQueryable<TEntity> query, bool ascending)
        {
            var orderBys = OrderByList.OfType<LookupFieldColumnDefinition>();
            var first = true;

            if (LookupDefinition.InitialOrderByField != null && _addInitialField)
            {
                var insertColumn = true;
                if (orderBys.FirstOrDefault() is LookupFieldColumnDefinition fieldColumn)
                {
                    if (fieldColumn.FieldDefinition == LookupDefinition.InitialOrderByField)
                    {
                        insertColumn = false;
                    }
                }

                if (insertColumn)
                {
                    var column = LookupDefinition.AddHiddenColumn(
                        LookupDefinition.InitialOrderByField);
                    OrderByList.Insert(0, column);
                }
            }


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
                if (first)
                {
                    query = GblMethods.ApplyOrder(query, orderByType, primaryKeyField.PropertyName);
                    first = false;
                }
                else
                {
                    query = GblMethods.ApplyOrder(query, thenByType, primaryKeyField.PropertyName);
                }

                //if (ascending)
                //{
                //    query = GblMethods.ApplyOrder(query, OrderMethods.ThenBy, primaryKeyField.PropertyName);
                //}
                //else
                //{
                //    query = GblMethods.ApplyOrder(query, OrderMethods.ThenByDescending, primaryKeyField.PropertyName);
                //}
            }

            return query;
        }

        /// <summary>
        /// Handles the RefreshData event of the LookupCallBack control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void LookupCallBack_RefreshData(object sender, EventArgs e)
        {
            RefreshData(LookupControl.SearchText);
            OnDataSourceChanged();
        }

        /// <summary>
        /// Makes the list.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="topCount">The top count.</param>
        /// <param name="bottomCount">The bottom count.</param>
        /// <param name="setIndexToBottom">if set to <c>true</c> [set index to bottom].</param>
        /// <param name="operation">The operation.</param>
        private void MakeList(TEntity entity, int topCount, int bottomCount, bool setIndexToBottom
        , LookupOperations operation)
        {
            //var fireChangedEvent = true;
            CurrentList.Clear();

            if (LookupControl != null && LookupControl.PageSize == 1)
            {
                CurrentList.Add(entity);
                SelectedPrimaryKeyValue = TableDefinition.GetPrimaryKeyValueFromEntity(entity);
                
                
                FireLookupDataChangedEvent(GetOutputArgs());
                return;

            }

            var previousPageCount = 0;
            if (topCount > 0)
            {
                var previousPage = GetPreviousRecordSet(entity, topCount);
                //var previousPage = GetPage(entity, operation, topCount, true);
                previousPageCount = previousPage.Count;
                if (previousPage != null && previousPage.Any())
                {
                    CurrentList.InsertRange(0, previousPage);
                }
                else
                {
                    //fireChangedEvent = false;
                    IntGotoTop();
                }

                if (previousPage.Count < topCount  && !setIndexToBottom)
                {
                    //fireChangedEvent = false;
                    if (operation != LookupOperations.GetInitData)
                    {
                        IntGotoTop();
                    }

                    return;
                }
            }

            if (operation == LookupOperations.SearchForChange)
            {
                CurrentList.Add(entity);
            }

            if (bottomCount > 0)
            {
                //if (topCount + bottomCount > LookupControl.PageSize - 1)
                //{
                //    bottomCount--;
                //}
                //var newBottomCount = bottomCount;
                //if (topCount > 0)
                //{
                //    newBottomCount++;
                //}

                var nextPage = GetNextRecordSet(entity, bottomCount);
                //var nextPage = GetPage(entity, operation, newBottomCount, false);
                if (nextPage != null && nextPage.Any())
                {
                    //if (topCount > 0 && nextPage.Any())
                    //{
                    //    nextPage.Remove(nextPage.FirstOrDefault());
                    //}
                    CurrentList.AddRange(nextPage);
                }
                else
                {
                    //fireChangedEvent = false;
                    IntGotoBottom(operation);
                }
                var gotoBottom = false;
                if (nextPage.Count < bottomCount)
                {
                    gotoBottom = true;
                    if (operation == LookupOperations.GetInitData)
                    {
                        gotoBottom = false;
                    }
                }
                if (gotoBottom)
                {
                    //fireChangedEvent = false;
                    IntGotoBottom(operation);
                }
            }

            //if (fireChangedEvent)
            {
                FireLookupDataChangedEvent(GetOutputArgs());
            }

            if (setIndexToBottom)
            {
                LookupControl.SetLookupIndex(CurrentList.Count - 1);
            }
            else
            {
                LookupControl.SetLookupIndex(0);
            }
        }

        /// <summary>
        /// Gets the page.
        /// </summary>
        /// <param name="nextEntity">The next entity.</param>
        /// <param name="operation">The operation.</param>
        /// <param name="count">The count.</param>
        /// <param name="previous">if set to <c>true</c> [previous].</param>
        /// <param name="filter">The filter.</param>
        /// <returns>List&lt;TEntity&gt;.</returns>
        private List<TEntity> GetPage(TEntity nextEntity, LookupOperations operation, int count
            , bool previous, TableFilterDefinition<TEntity> filter = null)
        {
            var result = new List<TEntity>();
            var ascending = !previous;
            if (_orderByType == OrderByTypes.Descending)
            {
                ascending = !ascending;
            }
            var input = GetProcessInput(nextEntity, true);

            var nextCond = Conditions.GreaterThan;
            if (!ascending)
                nextCond = Conditions.LessThan;

            var query = GetFilterPageQuery(
                nextEntity
                , count, input
                , true
                , out var addedPrimaryKeyToFilter
                , !previous
                , out var hasMultiRecs
                , false);

            result.AddRange(query);

            count -= result.Count;

            if (result.Count == 0)
            {
                return result;
            }

            if (count > 0)
            {
                if (previous)
                {
                    nextEntity = result.FirstOrDefault();
                }
                else
                {
                    nextEntity = result.LastOrDefault();
                }

                var newList = AddAditionalList(input, result, count, addedPrimaryKeyToFilter, nextEntity, !previous, operation);

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

        /// <summary>
        /// Gets the filter page query.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="count">The count.</param>
        /// <param name="input">The input.</param>
        /// <param name="checkPrimaryKey">if set to <c>true</c> [check primary key].</param>
        /// <param name="addedPrimaryKeyToFilter">if set to <c>true</c> [added primary key to filter].</param>
        /// <param name="ascending">if set to <c>true</c> [ascending].</param>
        /// <param name="hasMultiRecs">if set to <c>true</c> [has multi recs].</param>
        /// <param name="checkNull">if set to <c>true</c> [check null].</param>
        /// <returns>IEnumerable&lt;TEntity&gt;.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        private IEnumerable<TEntity> GetFilterPageQuery(
            TEntity entity
            , int count
            , LookupDataMauiProcessInput<TEntity> input
            , bool checkPrimaryKey
            , out bool addedPrimaryKeyToFilter
            , bool ascending
            , out bool hasMultiRecs
            , bool checkNull)
        {
            addedPrimaryKeyToFilter = false;
            if (checkNull)
            {
                hasMultiRecs = false;
            }
            else
            {
                hasMultiRecs = DoesFilterHaveMoreThan1Record(entity, input, checkPrimaryKey);
            }

            var condition = Conditions.Equals;
            if (hasMultiRecs && checkPrimaryKey)
            {

                addedPrimaryKeyToFilter = true;
                AddPrimaryKeyFieldsToFilter(entity, input);
                
            }

            var qAsc = ascending;
            switch (_orderByType)
            {
                case OrderByTypes.Ascending:
                    break;
                case OrderByTypes.Descending:
                    qAsc = !ascending;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            var query = GetQueryFromFilter(input.FilterDefinition, input, qAsc);
            query = query.Take(count);
            var result = GetOutputResult(query, ascending, count, input);
            return result;
        }

        /// <summary>
        /// Doeses the filter have more than1 record.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="input">The input.</param>
        /// <param name="addFilters">if set to <c>true</c> [add filters].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
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

        /// <summary>
        /// Adds the aditional list.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="inputList">The input list.</param>
        /// <param name="count">The count.</param>
        /// <param name="addedPrimaryKey">if set to <c>true</c> [added primary key].</param>
        /// <param name="topEntity">The top entity.</param>
        /// <param name="ascending">if set to <c>true</c> [ascending].</param>
        /// <param name="operation">The operation.</param>
        /// <param name="filterIndex">Index of the filter.</param>
        /// <returns>List&lt;TEntity&gt;.</returns>
        private List<TEntity> AddAditionalList(
            LookupDataMauiProcessInput<TEntity> input
            , List<TEntity> inputList
            , int count
            , bool addedPrimaryKey
            , TEntity topEntity
            , bool ascending
            , LookupOperations operation
            , int filterIndex = 0)
        {
            FieldFilterDefinition lastFilter = null;
            var result = new List<TEntity>();

            //input = GetProcessInput(topEntity);
            lastFilter = input.FieldFilters.LastOrDefault();

            //ProcesAddFilters(input, filterIndex, addedPrimaryKey, topEntity);

            if (lastFilter.IsNullableFilter())
            {
                return AddAdditionalListNull(
                    input
                    , inputList
                    , count
                    , addedPrimaryKey
                    , topEntity
                    , ascending
                    , operation
                    , filterIndex);
            }

            lastFilter = input.FieldFilters.LastOrDefault();

            SetLastFilterCondition(ascending, lastFilter);
            var output = GetFilterPageQuery(
                topEntity
                , count
                , input
                , false
                , out addedPrimaryKey
                , ascending
                , out var hasMultiRecs
                , false);

            lastFilter = input.FieldFilters.LastOrDefault();
            RemoveFilter(input, lastFilter);
            if (output.Count() == 0)
            {
                if (input.FieldFilters.Count > 0)
                {
                    var entity = topEntity;
                    //RemoveFilter(input, lastFilter);
                    lastFilter = input.FieldFilters.LastOrDefault();
                    var isEnd = false;
                    switch (operation)
                    {
                        case LookupOperations.SearchForChange:
                            break;
                        default:
                            isEnd = IsEnd(topEntity, ascending, lastFilter);
                            break;
                    }

                    if (isEnd)
                    {
                        return result;
                    }
                    filterIndex++;
                    var newList = AddAditionalList(input, result, count, false, entity, ascending, operation, filterIndex);
                    result.InsertRange(0, newList);
                }
                return result;
            }

            result.InsertRange(0, output);
            count -= result.Count;

            if (count > 0)
            {
                TEntity entity = null;
                if (ascending)
                {
                    entity = result.LastOrDefault();
                }
                else
                {
                    entity = result.FirstOrDefault();
                }

                filterIndex++;
                var newList = AddAditionalList(input, result, count, false, entity, ascending
                    , operation, filterIndex);

                ProcessPageOutput(ascending, result, newList);
            }
            return result;
        }

        /// <summary>
        /// Sets the last filter condition.
        /// </summary>
        /// <param name="ascending">if set to <c>true</c> [ascending].</param>
        /// <param name="lastFilter">The last filter.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        private void SetLastFilterCondition(bool ascending, FieldFilterDefinition lastFilter)
        {
            switch (_orderByType)
            {
                case OrderByTypes.Ascending:
                    if (ascending)
                    {
                        lastFilter.Condition = Conditions.GreaterThan;
                    }
                    else
                    {
                        lastFilter.Condition = Conditions.LessThan;
                    }

                    break;
                case OrderByTypes.Descending:
                    if (ascending)
                    {
                        lastFilter.Condition = Conditions.LessThan;
                    }
                    else
                    {
                        lastFilter.Condition = Conditions.GreaterThan;
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Processes the page output.
        /// </summary>
        /// <param name="ascending">if set to <c>true</c> [ascending].</param>
        /// <param name="result">The result.</param>
        /// <param name="newList">The new list.</param>
        private static void ProcessPageOutput(bool ascending, List<TEntity> result, List<TEntity> newList)
        {
            if (ascending)
            {
                result.AddRange(newList);
            }
            else
            {
                result.InsertRange(0, newList);
            }
        }

        /// <summary>
        /// Proceses the add filters.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="filterIndex">Index of the filter.</param>
        /// <param name="addedPrimaryKey">if set to <c>true</c> [added primary key].</param>
        /// <param name="topEntity">The top entity.</param>
        private void ProcesAddFilters(LookupDataMauiProcessInput<TEntity> input
            , int filterIndex
            , bool addedPrimaryKey
            , TEntity topEntity)
        {
            if (addedPrimaryKey)
            {
                AddPrimaryKeyFieldsToFilter(topEntity, input);
            }


            FieldFilterDefinition lastFilter;
            if (filterIndex > 1 && input.FieldFilters.Count > 1)
            {
                lastFilter = input.FieldFilters.LastOrDefault();

                if (lastFilter != null && input.FieldFilters.Count > 1)
                {
                    RemoveFilter(input, lastFilter);
                }
            }
        }

        /// <summary>
        /// Determines whether the specified top entity is end.
        /// </summary>
        /// <param name="topEntity">The top entity.</param>
        /// <param name="ascending">if set to <c>true</c> [ascending].</param>
        /// <param name="lastFilter">The last filter.</param>
        /// <returns><c>true</c> if the specified top entity is end; otherwise, <c>false</c>.</returns>
        private bool IsEnd(TEntity topEntity, bool ascending, FieldFilterDefinition lastFilter)
        {
            var isEnd = false;
            //if (lastFilter.IsNullFilter())
            {
                var nextCond = Conditions.GreaterThan;
                if (!ascending)
                {
                    nextCond = Conditions.LessThan;
                }

                var nextEntity = GetNearestEntity(topEntity, nextCond);
                if (nextEntity == null)
                {
                    isEnd = true;
                }
            }

            return isEnd;
        }

        /// <summary>
        /// Adds the additional list null.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="inputList">The input list.</param>
        /// <param name="count">The count.</param>
        /// <param name="addedPrimaryKey">if set to <c>true</c> [added primary key].</param>
        /// <param name="topEntity">The top entity.</param>
        /// <param name="ascending">if set to <c>true</c> [ascending].</param>
        /// <param name="operation">The operation.</param>
        /// <param name="filterIndex">Index of the filter.</param>
        /// <returns>List&lt;TEntity&gt;.</returns>
        private List<TEntity> AddAdditionalListNull(
            LookupDataMauiProcessInput<TEntity> input
            , List<TEntity> inputList
            , int count
            , bool addedPrimaryKey
            , TEntity topEntity
            , bool ascending
            , LookupOperations operation
            , int filterIndex = 0)
        {
            var result = new List<TEntity>();

            var lastFilter = input.FieldFilters.LastOrDefault();

            if (lastFilter == null)
            {
                return result;
            }

            SetLastFilterCondition(ascending, lastFilter);

            if (lastFilter.IsNullFilter())
            {
                if (ascending)
                {
                    var nearestEntity = GetNearestEntity(topEntity, Conditions.GreaterThan);
                    if (nearestEntity == null)
                    {
                        return result;
                    }

                    result.Add(nearestEntity);
                    count--;

                    if (count == 0)
                    {
                        return result;
                    }

                    lastFilter.Value = lastFilter.LookupColumn.GetDatabaseValue(nearestEntity);
                    topEntity = nearestEntity;
                }
                else
                {
                    lastFilter.Condition = Conditions.NotEqualsNull;
                }
            }

            var output = GetFilterPageQuery(
                topEntity
                , count
                , input
                , false
                , out addedPrimaryKey
                , ascending
                , out var hasMultiRecs
                , false);

            var nextEntity = topEntity;
            count -= output.Count();
            if (output.Count() == 0)
            {
                var nearestCondition = Conditions.LessThan;
                if (_orderByType == OrderByTypes.Descending && ascending)
                {
                    nearestCondition = Conditions.GreaterThan;
                }
                var nearestEntity = GetNearestEntity(topEntity, nearestCondition);

                if (nearestEntity == null)
                {
                    return result;
                }

                var nextInput = GetProcessInput(nearestEntity);

                var nextLastFilter = nextInput.FieldFilters.LastOrDefault();
                if (lastFilter.IsNullFilter())
                {

                }
                else
                {
                    if (nextLastFilter.IsNullFilter())
                    {
                        nextLastFilter.Condition = Conditions.EqualsNull;
                    }
                }

                var qAsc = ascending;
                if (ascending && _orderByType == OrderByTypes.Descending)
                {
                    qAsc = !ascending;
                }
                var query = GetQueryFromFilter(nextInput.FilterDefinition, nextInput, qAsc);
                query = query.Take(count);
                var newList = GetOutputResult(query, ascending, count, nextInput);

                count -= newList.Count();
                ProcessPageOutput(ascending, result, newList.ToList());

                if (count > 0)
                {
                    if (ascending)
                    {
                        nextEntity = newList.LastOrDefault();
                    }
                    else
                    {
                        nextEntity = newList.FirstOrDefault();
                    }
                }
                lastFilter = input.FieldFilters.LastOrDefault();
                RemoveFilter(input, lastFilter);
            }
            else
            {
                var newList = output.ToList();
                ProcessPageOutput(ascending, result, output.ToList());

                if (count > 0)
                {
                    if (ascending)
                    {
                        nextEntity = newList.LastOrDefault();
                    }
                    else
                    {
                        nextEntity = newList.FirstOrDefault();
                    }
                }
            }
            if (count > 0)
            {
                lastFilter.Value = lastFilter.LookupColumn.GetDatabaseValue(nextEntity);
                var newList = AddAditionalList(input, result, count, false, nextEntity, ascending
                    , operation, filterIndex);
                ProcessPageOutput(ascending, result, newList);
            }

            return result;
        }

        /// <summary>
        /// Gets the output result.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="ascending">if set to <c>true</c> [ascending].</param>
        /// <param name="count">The count.</param>
        /// <param name="input">The input.</param>
        /// <returns>IEnumerable&lt;TEntity&gt;.</returns>
        private IEnumerable<TEntity> GetOutputResult(IQueryable<TEntity> query, bool ascending, int count
        , LookupDataMauiProcessInput<TEntity> input)
        {
            if (query.Count() == 1)
            {
                return query;
            }
            var result = new List<TEntity>();
            var tempList = new List<TEntity>(query);

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

        /// <summary>
        /// Adds the filter.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="column">The column.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
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

        /// <summary>
        /// Removes the filter.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="filterItem">The filter item.</param>
        private void RemoveFilter(LookupDataMauiProcessInput<TEntity> input, FilterItemDefinition filterItem)
        {
            if (filterItem is FieldFilterDefinition fieldFilter)
            {
                input.FieldFilters.Remove(fieldFilter);
            }
            input.FilterDefinition.RemoveFixedFilter(filterItem);
            input.FieldFilters = input.FilterDefinition.FixedFilters.OfType<FieldFilterDefinition>().ToList();
        }

        /// <summary>
        /// Determines whether [has more than1 record] [the specified input].
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns><c>true</c> if [has more than1 record] [the specified input]; otherwise, <c>false</c>.</returns>
        private bool HasMoreThan1Record(LookupDataMauiProcessInput<TEntity> input)
        {
            var result = false;
            var query = GetQueryFromFilter(input.FilterDefinition, input, true);
            query = query.Take(2);
            var count = query.Count();
            result = count > 1;
            return result;
        }

        /// <summary>
        /// Gets the query from filter.
        /// </summary>
        /// <param name="newFilter">The new filter.</param>
        /// <param name="input">The input.</param>
        /// <param name="ascending">if set to <c>true</c> [ascending].</param>
        /// <returns>IQueryable&lt;TEntity&gt;.</returns>
        private IQueryable<TEntity> GetQueryFromFilter(TableFilterDefinition<TEntity> newFilter
            , LookupDataMauiProcessInput<TEntity> input, bool ascending)
        {
            var filterExpr = newFilter.GetWhereExpresssion<TEntity>(input.Param);

            var containsExpr = GetContainsExpr(input.Param);

            var fullExpr =
                FilterItemDefinition.AppendExpression(input.LookupExpression
                    , filterExpr, EndLogics.And);

            if (containsExpr != null)
            {
                fullExpr = FilterItemDefinition.AppendExpression(fullExpr, containsExpr, EndLogics.And);
            }

            var query = FilterItemDefinition.FilterQuery(input.Query, input.Param, fullExpr);
            query = ApplyOrderBys(query, ascending);
            return query;
        }

        /// <summary>
        /// Gets the contains expr.
        /// </summary>
        /// <param name="param">The parameter.</param>
        /// <param name="condition">The condition.</param>
        /// <returns>Expression.</returns>
        private Expression GetContainsExpr(ParameterExpression param, Conditions? condition = null)
        {
            Expression containsExpr = null;
            var search = true;
            if (LookupControl.SearchType == LookupSearchTypes.Contains)
            {
                var containsColumn = OrderByList.FirstOrDefault();
                if (containsColumn != null)
                {
                    if (LookupDefinition.InitialOrderByField != null)
                    {
                        if (containsColumn.FieldDefinition == LookupDefinition.InitialOrderByField)
                        {
                            containsColumn = OrderByList[1];
                        }
                    }
                }

                var searchCond = Conditions.Contains;
                if (containsColumn.FieldDefinition.SearchForCondition != null)
                {
                    searchCond = containsColumn.FieldDefinition.SearchForCondition.GetValueOrDefault();
                }

                if (!LookupControl.SearchText.IsNullOrEmpty())
                {
                    containsExpr = FilterItemDefinition.GetBinaryExpression<TEntity>(param
                        , containsColumn.GetPropertyJoinName()
                        , searchCond
                        , containsColumn.FieldDefinition.FieldType
                        , LookupControl.SearchText.GetPropertyFilterValue(
                            containsColumn.FieldDefinition.FieldDataType, containsColumn.FieldDefinition.FieldType));
                }
            }

            return containsExpr;
        }

        /// <summary>
        /// Gets the next page.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="count">The count.</param>
        /// <returns>List&lt;TEntity&gt;.</returns>
        private List<TEntity> GetNextPage(TEntity entity, int count)
        {
            var result = new List<TEntity>();
            return result;
        }

    }
}
