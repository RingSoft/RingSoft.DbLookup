using System;
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

        private OrderByTypes _orderByType = OrderByTypes.Ascending;

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

            _orderByType = lookupDefinition.InitialOrderByType;
        }

        public override void GetInitData()
        {
            RefreshData();
            FireLookupDataChangedEvent(GetOutputArgs());
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
            MakeFilteredQuery();
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
            FireLookupDataChangedEvent(GetOutputArgs());
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

            if (LookupControl != null && LookupControl.PageSize == 1)
            {
                SelectedPrimaryKeyValue = primaryKeyValue;
                SetNewPrimaryKeyValue(primaryKeyValue);
                SetScrollPosition(GetOutputArgs().ScrollPosition);
            }
        }

        public override void SetNewPrimaryKeyValue(PrimaryKeyValue primaryKeyValue)
        {
            var entity = TableDefinition.GetEntityFromPrimaryKeyValue(primaryKeyValue);
            if (entity != null)
            {
                var autoFillValue = entity.GetAutoFillValue();
                if (autoFillValue.Text.IsNullOrEmpty())
                {
                    var filter = new TableFilterDefinition<TEntity>(TableDefinition);
                    var context = SystemGlobals.DataRepository.GetDataContext();
                    var table = context.GetTable<TEntity>();
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
                    var splitPage = (int)Math.Ceiling((double)LookupControl.PageSize / 2);
                    var topCount = LookupControl.PageSize - splitPage;
                    var bottomCount = LookupControl.PageSize - topCount;

                    MakeList(entity, topCount, bottomCount, false);
                    SetLookupIndexFromEntity(param, entity);
                }
                else
                {
                    OnSearchForChange(autoFillValue.Text);
                }
            }
        }

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
                }
            }
        }

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

        private void RefreshBaseQuery()
        {
            BaseQuery = LookupDefinition.TableDefinition.Context.GetQueryable<TEntity>(LookupDefinition);
        }

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
            }

            CurrentList.Clear();

            CurrentList.AddRange(ProcessedQuery);
              
            ControlsGlobals.UserInterface.SetWindowCursor(WindowCursorTypes.Default);

            FireLookupDataChangedEvent(GetOutputArgs());
        }

        private void MakeFilteredQuery(bool applyOrders = true)
        {
            if (BaseQuery == null)
            {
                RefreshBaseQuery();
            }
            var param = GblMethods.GetParameterExpression<TEntity>();

            var whereExpression = LookupDefinition.FilterDefinition.GetWhereExpresssion<TEntity>(param);

            if (LookupControl != null && LookupControl.SearchType == LookupSearchTypes.Contains)
            {
                var contExpr = GetContainsExpr(param);
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

        public override void OnColumnClick(LookupFieldColumnDefinition column, bool resetSortOrder)
        {
            if (resetSortOrder)
            {
                if (column == OrderByList[0])
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

        public override void OnMouseWheelForward()
        {
            if (!CurrentList.Any() || CurrentList.Count < 3)
            {
                return;
            }

            var selectedEntity = CurrentList[CurrentList.Count - 3];
            if (selectedEntity != null)
            {
                var entity = GetNearestEntity(selectedEntity, Conditions.GreaterThan);
                if (entity == null)
                {
                    GotoBottom();
                }
                else
                {
                    MakeList(entity, LookupControl.PageSize - 4, 4, true);
                }
            }

        }

        public override void OnMouseWheelBack()
        {
            if (!CurrentList.Any() || CurrentList.Count < 3)
            {
                return;
            }

            var selectedEntity = CurrentList.FirstOrDefault();
            if (selectedEntity != null)
            {
                var entity = GetNearestEntity(selectedEntity, Conditions.LessThan);
                if (entity == null)
                {
                    GotoTop();
                }
                else
                {
                    MakeList(entity, 3, LookupControl.PageSize - 3, false);
                }
            }
        }

        public override bool IsThereData()
        {
            MakeFilteredQuery(true);
            return FilteredQuery.Any();
        }

        private void SetScrollPosition(LookupScrollPositions scrollPosition)
        {
            ScrollPosition = scrollPosition;
        }

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
            var prevEntity = GetNearestEntity(CurrentList[0], Conditions.LessThan);
            var nextEntity = GetNearestEntity(CurrentList[CurrentList.Count - 1], Conditions.GreaterThan);

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

        public override string GetSelectedText()
        {
            var autoFillValue = LookupDefinition.GetAutoFillValue(SelectedPrimaryKeyValue);
            var text = autoFillValue?.Text;
            //var entity = TableDefinition.GetEntityFromPrimaryKeyValue(SelectedPrimaryKeyValue);
            //var text = LookupDefinition.InitialSortColumnDefinition.GetDatabaseValue(entity);
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
            LookupControl.SetLookupIndex(0);
        }

        public override void GotoBottom()
        {
            MakeFilteredQuery();
            var entity = FilteredQuery.LastOrDefault();
            MakeList(entity, LookupControl.PageSize, 0, true);
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
                    MakeList(entity, LookupControl.PageSize, 0, true);
                }
            }
        }

        public override void GotoPreviousRecord()
        {
            if (!CurrentList.Any())
            {
                return;
            }

            var selectedEntity = CurrentList.FirstOrDefault();
            if (selectedEntity != null)
            {
                var entity = GetNearestEntity(selectedEntity, Conditions.LessThan);
                if (entity == null)
                {
                    GotoTop();
                }
                else
                {
                    MakeList(entity, 0, LookupControl.PageSize, false);
                }
            }
        }

        public override void GotoNextPage()
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
                    MakeList(entity, 0, LookupControl.PageSize, true);
                }
            }
        }

        public override void GotoPreviousPage()
        {
            if (!CurrentList.Any())
            {
                return;
            }

            var selectedEntity = CurrentList.FirstOrDefault();
            if (selectedEntity != null)
            {
                var entity = GetNearestEntity(selectedEntity, Conditions.LessThan);
                if (entity == null)
                {
                    GotoTop();
                }
                else
                {
                    MakeList(entity, LookupControl.PageSize, 0, false);
                }
            }
        }

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
            var type = searchColumn.FieldDefinition.FieldType;
            var value = searchForText.GetPropertyFilterValue(searchColumn.FieldDefinition.FieldDataType,
                searchColumn.FieldDefinition.FieldType);

            if (filterExpr == null)
            {

                filterExpr = FilterItemDefinition.GetBinaryExpression<TEntity>(param, searchColumn.GetPropertyJoinName()
                    , Conditions.GreaterThanEquals, type, value);
                if (type == typeof(string))
                {
                    filterExpr = FilterItemDefinition.GetBinaryExpression<TEntity>(param,
                        searchColumn.GetPropertyJoinName()
                        , Conditions.BeginsWith, type, searchForText);

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

            var splitPage = (int)Math.Ceiling((double)LookupControl.PageSize / 2);
            var topCount = LookupControl.PageSize - splitPage;
            var bottomCount = LookupControl.PageSize - topCount;

            MakeList(entity, topCount, bottomCount, false);

            SetLookupIndexFromEntity(param, entity);

            ControlsGlobals.UserInterface.SetWindowCursor(WindowCursorTypes.Default);
        }

        private void SetLookupIndexFromEntity(ParameterExpression param, TEntity entity)
        {
            var searchColumn = OrderByList.FirstOrDefault() as LookupFieldColumnDefinition;
            IQueryable<TEntity> query;
            var searchExpr = FilterItemDefinition.GetBinaryExpression<TEntity>(param
                , searchColumn.GetPropertyJoinName()
                , Conditions.Equals, searchColumn.FieldDefinition.FieldType
                , searchColumn.GetDatabaseValue(entity).GetPropertyFilterValue(searchColumn
                    .FieldDefinition.FieldDataType, searchColumn.FieldDefinition.FieldType));
            var queryable = CurrentList.AsQueryable();
            query = FilterItemDefinition.FilterQuery(queryable, param, searchExpr);
            entity = query.FirstOrDefault();
            if (entity != null)
            {
                LookupControl.SetLookupIndex(CurrentList.IndexOf(entity));
            }
        }

        public LookupDataMauiOutput GetOutputArgs()
        {
            var scrollPosition = GetScrollPosition();
            var result = new LookupDataMauiOutput(scrollPosition);
            return result;
        }

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

            AddPrimaryKeyFieldsToFilter(entity, input);

            var ascending = condition == Conditions.GreaterThan;

            while (input.FilterDefinition.FixedFilters.Any())
            {
                var filterItem = input.FilterDefinition.FixedFilters.OfType<FieldFilterDefinition>().LastOrDefault();
                if (filterItem != null)
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

        private void LookupCallBack_RefreshData(object sender, EventArgs e)
        {
            RefreshData(LookupControl.SearchText);
            OnDataSourceChanged();
        }

        private void MakeList(TEntity entity, int topCount, int bottomCount, bool setIndexToBottom)
        {
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
                var previousPage = GetPage(entity, topCount, true);
                previousPageCount = previousPage.Count;
                if (previousPage != null && previousPage.Any())
                {
                    CurrentList.InsertRange(0, previousPage);
                }
                else
                {
                    GotoTop();
                }

                if (previousPage.Count < topCount)
                {
                    GotoTop();
                    return;
                }
            }

            if (bottomCount > 0)
            {
                var newBottomCount = bottomCount;
                if (topCount > 0)
                {
                    newBottomCount++;
                }
                var nextPage = GetPage(entity, newBottomCount, false);
                if (nextPage != null && nextPage.Any())
                {
                    if (topCount > 0 && nextPage.Any())
                    {
                        nextPage.Remove(nextPage.FirstOrDefault());
                    }
                    CurrentList.AddRange(nextPage);
                }
                else
                {
                    GotoBottom();
                }

                if (nextPage.Count < bottomCount)
                {
                    GotoBottom();
                }
            }
            FireLookupDataChangedEvent(GetOutputArgs());
            if (setIndexToBottom)
            {
                LookupControl.SetLookupIndex(CurrentList.Count - 1);
            }
            else
            {
                LookupControl.SetLookupIndex(0);
            }
        }

        private List<TEntity> GetPage(TEntity nextEntity, int count
            , bool previous, TableFilterDefinition<TEntity> filter = null)
        {
            var result = new List<TEntity>();
            var ascending = !previous;
            if (_orderByType == OrderByTypes.Descending)
            {
                ascending = !ascending;
            }
            var input = GetProcessInput(nextEntity, true);

            var query = GetFilterPageQuery(nextEntity, count, input
                , true, out var addedPrimaryKeyToFilter, !previous, out var hasMultiRecs);

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

        private IEnumerable<TEntity> GetFilterPageQuery(
            TEntity entity
            , int count
            , LookupDataMauiProcessInput<TEntity> input
            , bool checkPrimaryKey
            , out bool addedPrimaryKeyToFilter
            , bool ascending
            , out bool hasMultiRecs)
        {
            addedPrimaryKeyToFilter = false;
            hasMultiRecs = DoesFilterHaveMoreThan1Record(entity, input, checkPrimaryKey);

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
            var output = GetFilterPageQuery(
                topEntity
                , count
                , input
                , false
                , out addedPrimaryKey
                , ascending
                , out var hasMultiRecs);

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
                var newList = AddAditionalList(input, result, count, false, entity, ascending, filterIndex);

                if (ascending)
                {
                    result.AddRange(newList);
                }
                else
                {
                    result.InsertRange(0, newList);
                }
            }
            return result;
        }

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
            query = query.Take(2);
            var count = query.Count();
            result = count > 1;
            return result;
        }

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

        private Expression GetContainsExpr(ParameterExpression param)
        {
            Expression containsExpr = null;
            if (LookupControl.SearchType == LookupSearchTypes.Contains)
            {
                var containsColumn = OrderByList.FirstOrDefault();
                containsExpr = FilterItemDefinition.GetBinaryExpression<TEntity>(param
                    , containsColumn.GetPropertyJoinName()
                    , Conditions.Contains
                    , containsColumn.FieldDefinition.FieldType
                    , LookupControl.SearchText);
            }

            return containsExpr;
        }

        private List<TEntity> GetNextPage(TEntity entity, int count)
        {
            var result = new List<TEntity>();
            return result;
        }

    }
}
