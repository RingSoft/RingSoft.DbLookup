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
    public enum LookupOperations
    {
        GetInitData = 0,
        PageUp = 1,
        PageDown = 2,
        RecordUp = 3,
        RecordDown = 4,
        GotoBottom = 5,
        GotoTop = 6,
        WheelForward = 7,
        WheelBackward = 8,
        SearchForChange = 9,
    }
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
        private bool _addInitialField = true;

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

            if (LookupControl != null 
                && LookupControl.PageSize == 1
                && primaryKeyValue != null)
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
                //var autoFillValue = entity.GetAutoFillValue();
                //if (autoFillValue == null || autoFillValue.Text.IsNullOrEmpty())
                {
                    var filter = new TableFilterDefinition<TEntity>(TableDefinition);
                    //var context = SystemGlobals.DataRepository.GetDataContext();
                    //var table = context.GetTable<TEntity>();
                    var table = TableDefinition.Context.GetQueryable<TEntity>(LookupDefinition);
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

                    MakeList(entity, topCount, bottomCount, false, LookupOperations.SearchForChange);
                    SetLookupIndexFromEntity(param, entity);
                }
                //else
                //{
                //    OnSearchForChange(autoFillValue.Text);
                //}
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
                    LookupWindow.OnSelectButtonClick();
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
                    MakeList(entity, LookupControl.PageSize - 4, 4, true
                    , LookupOperations.WheelForward);
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
                    MakeList(entity, 3, LookupControl.PageSize - 3, false
                    , LookupOperations.WheelBackward);
                }
            }
        }

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

        public override void GotoTop()
        {
            GetInitData();
            LookupControl.SetLookupIndex(0);
        }

        public override void GotoBottom()
        {
            MakeFilteredQuery();
            var entity = FilteredQuery.LastOrDefault();
            MakeList(entity, LookupControl.PageSize, 0, true, LookupOperations.GotoBottom);
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
                    MakeList(entity, LookupControl.PageSize, 0, true, LookupOperations.GotoBottom);
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
                    MakeList(entity, 0, LookupControl.PageSize, false, LookupOperations.RecordUp);
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
                    MakeList(entity, 0, LookupControl.PageSize, true, LookupOperations.PageDown);
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
                    MakeList(entity, LookupControl.PageSize, 0, false, LookupOperations.PageUp);
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

            MakeList(entity, topCount, bottomCount, false, LookupOperations.SearchForChange);

            SetLookupIndexFromEntity(param, entity);

            ControlsGlobals.UserInterface.SetWindowCursor(WindowCursorTypes.Default);
        }

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

        private void SetLookupIndexFromEntity(ParameterExpression param, TEntity entity)
        {
            if (TableDefinition.PrimaryKeyFields.Count > 1)
            {
                return;
            }
            var searchColumn = OrderByList.FirstOrDefault() as LookupFieldColumnDefinition;
            var propertyName = searchColumn.GetPropertyJoinName();
            var dbValue = searchColumn.GetDatabaseValue(entity).GetPropertyFilterValue(searchColumn
                .FieldToDisplay.FieldDataType, searchColumn.FieldToDisplay.FieldType);
            Expression nullExpr = null;
            if (dbValue != null && searchColumn.AllowNulls)
            {
                nullExpr = FilterItemDefinition.GetBinaryExpression<TEntity>
                (param, searchColumn.GetPropertyJoinName(true)
                    , Conditions.NotEqualsNull
                    , searchColumn.FieldDefinition.FieldType);
            }
            IQueryable<TEntity> query;
            var searchExpr = FilterItemDefinition.GetBinaryExpression<TEntity>(param
                , propertyName
                , Conditions.Equals, searchColumn.FieldToDisplay.FieldType
                , dbValue);
            if (nullExpr != null)
            {
                searchExpr = FilterItemDefinition.AppendExpression(nullExpr, searchExpr, EndLogics.And);
            }

            var searchPrimaryKeyFilter = new TableFilterDefinition<TEntity>(TableDefinition);
            foreach (var primaryKeyField in TableDefinition.PrimaryKeyFields)
            {
                searchPrimaryKeyFilter.AddFixedFilter(primaryKeyField
                    , Conditions.Equals
                    , GblMethods.GetPropertyValue(entity, primaryKeyField.PropertyName));
            }

            searchExpr = FilterItemDefinition.AppendExpression(searchExpr
                , searchPrimaryKeyFilter.GetWhereExpresssion<TEntity>(param)
                , EndLogics.And);

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
                    var lastFilter = input.FieldFilters.LastOrDefault();
                    if (lastFilter != null)
                    {
                        if (lastFilter.FieldDefinition.AllowNulls)
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
                                if (_orderByType == OrderByTypes.Ascending)
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

        private void LookupCallBack_RefreshData(object sender, EventArgs e)
        {
            RefreshData(LookupControl.SearchText);
            OnDataSourceChanged();
        }

        private void MakeList(TEntity entity, int topCount, int bottomCount, bool setIndexToBottom
        , LookupOperations operation)
        {
            var fireChangedEvent = true;
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
                var previousPage = GetPage(entity, operation, topCount, true);
                previousPageCount = previousPage.Count;
                if (previousPage != null && previousPage.Any())
                {
                    CurrentList.InsertRange(0, previousPage);
                }
                else
                {
                    fireChangedEvent = false;
                    GotoTop();
                }

                if (previousPage.Count < topCount  && !setIndexToBottom)
                {
                    fireChangedEvent = false;
                    if (operation != LookupOperations.GetInitData)
                    {
                        GotoTop();
                    }

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
                var nextPage = GetPage(entity, operation, newBottomCount, false);
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
                    fireChangedEvent = false;
                    GotoBottom();
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
                    fireChangedEvent = false;
                    GotoBottom();
                }
            }

            if (fireChangedEvent)
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

            if (lastFilter.FieldDefinition.AllowNulls)
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

            if (addedPrimaryKey)
            {
                RemoveFilter(input, lastFilter);
                lastFilter = input.FieldFilters.LastOrDefault();
            }

            var nearestCondition = Conditions.LessThan;
            if (ascending)
            {
                nearestCondition = Conditions.GreaterThan;
            }

            var nextEntity = GetNearestEntity(topEntity, nearestCondition);

            if (nextEntity == null)
            {
                return result;
            }

            var nextInput = GetProcessInput(nextEntity);

            FieldFilterDefinition nextLastFilter = nextInput.FieldFilters.LastOrDefault();
            var hasMoreThan1Record = HasMoreThan1Record(nextInput);
            if (hasMoreThan1Record)
            {
                AddPrimaryKeyFieldsToFilter(nextEntity, nextInput);
            }

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
            var query = GetQueryFromFilter(nextInput.FilterDefinition, nextInput, ascending);
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

        private List<TEntity> GetNextPage(TEntity entity, int count)
        {
            var result = new List<TEntity>();
            return result;
        }

    }
}
