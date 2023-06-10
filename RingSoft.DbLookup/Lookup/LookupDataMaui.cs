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
    }
    public class LookupDataMaui<TEntity> : LookupDataMauiBase where TEntity : class, new()
    {
        public TableDefinition<TEntity> TableDefinition { get; }

        public IQueryable<TEntity> BaseQuery { get; private set; }

        public IQueryable<TEntity> FilteredQuery { get; private set; }

        public IQueryable<TEntity> ProcessedQuery { get; private set; }

        public List<TEntity> CurrentList { get; } = new List<TEntity>();

        public override int RowCount => CurrentList.Count;

        public List<LookupColumnDefinitionBase> OrderByList{ get; }

        public LookupDataMaui(LookupDefinitionBase lookupDefinition)
            : base(lookupDefinition)
        {
            if (lookupDefinition.TableDefinition is TableDefinition<TEntity> table)
            {
                TableDefinition = table;
            }
            OrderByList = new List<LookupColumnDefinitionBase>();   
            OrderByList.Add(lookupDefinition.InitialOrderByColumn);
        }

        public override void GetInitData()
        {
            RefreshData();
            FireLookupDataChangedEvent();
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
            FireLookupDataChangedEvent();
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

            var orderColumnName = LookupDefinition.InitialOrderByColumn.GetPropertyJoinName();
            if (!orderColumnName.IsNullOrEmpty())
            {
                FilteredQuery = GblMethods.ApplyOrder(FilteredQuery, OrderMethods.OrderBy,
                    orderColumnName);
            }

            ProcessedQuery = FilteredQuery.Take(LookupControl.PageSize);

            CurrentList.Clear();

            CurrentList.AddRange(ProcessedQuery);

            FireLookupDataChangedEvent();
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

            var selectedEntity = CurrentList[0];
            if (selectedEntity != null)
            {
                GetNextPage(selectedEntity, LookupControl.PageSize);
            }
        }

        private void GetNextPage(TEntity entity, int size)
        {
            var filterDefinition = new TableFilterDefinition<TEntity>(TableDefinition);

            var input = GetProcessInput(filterDefinition);

            var query = GetPage(input,entity, Conditions.GreaterThan, size);

            CurrentList.Clear();
            CurrentList.AddRange( MakeList(query, size, input, Conditions.GreaterThan));

            if (size == LookupControl.PageSize)
            {
                FireLookupDataChangedEvent();
                LookupControl.SetLookupIndex(LookupControl.PageSize - 1);
            }
        }

        private IQueryable<TEntity> GetPage(LookupDataMauiProcessInput<TEntity> input, TEntity entity, Conditions condition, int size)
        {
            var filterDefinition = new TableFilterDefinition<TEntity>(TableDefinition);
            
            var query = GetFilteredPage(input, entity, condition, size);

            return query;
        }

        private LookupDataMauiProcessInput<TEntity> GetProcessInput(TableFilterDefinition<TEntity> filterDefinition)
        {
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
            return result;
        }

        private IQueryable<TEntity> GetFilteredPage(LookupDataMauiProcessInput<TEntity> input, TEntity entity, Conditions condition,  int size)
        {
            var hasMoreThan1Record = false;

            var filterDefinition = input.FilterDefinition;
            var query = input.Query;
            var orderBys = input.OrderByList;
            var lookupExpr = input.LookupExpression;
            var param = input.Param;

            foreach (var orderBy in orderBys)
            {
                var fieldDefinition = orderBy.FieldDefinition;
                var value = orderBy.GetDatabaseValue(entity);

                hasMoreThan1Record =
                    CreateFilterItem(condition, param, filterDefinition, fieldDefinition, value, lookupExpr);

                if (!hasMoreThan1Record)
                {
                    break;
                }
            }

            if (hasMoreThan1Record)
            {
                foreach (var primaryKeyField in TableDefinition.PrimaryKeyFields)
                {
                    var value = GblMethods.GetPropertyValue(entity, primaryKeyField.PropertyName);

                    hasMoreThan1Record = CreateFilterItem(condition, param, filterDefinition, primaryKeyField, value,
                        lookupExpr);

                    if (!hasMoreThan1Record)
                    {
                        break;
                    }
                }
            }

            query = ApplyOrderBys(query);


            var pageExpr = filterDefinition.GetWhereExpresssion<TEntity>(param);

            var fullExpr = FilterItemDefinition.AppendExpression(lookupExpr, pageExpr, EndLogics.And);

            var pagedQuery = FilterItemDefinition.FilterQuery(query, param, fullExpr);
            pagedQuery = pagedQuery.Take(size);

            return pagedQuery;
        }

        private IQueryable<TEntity> ApplyOrderBys(IQueryable<TEntity> query)
        {
            var orderBys = OrderByList.OfType<LookupFieldColumnDefinition>();
            var first = true;
            foreach (var orderBy in orderBys)
            {
                if (first)
                {
                    query = GblMethods.ApplyOrder(query, OrderMethods.OrderBy, orderBy.GetPropertyJoinName());
                    first = false;
                }
                else
                {
                    query = GblMethods.ApplyOrder(query, OrderMethods.ThenBy, orderBy.GetPropertyJoinName());
                }
            }

            foreach (var primaryKeyField in TableDefinition.PrimaryKeyFields)
            {
                query = GblMethods.ApplyOrder(query, OrderMethods.ThenBy, primaryKeyField.PropertyName);
            }

            return query;
        }

        private List<TEntity> MakeList(IQueryable<TEntity> initPage, int size, LookupDataMauiProcessInput<TEntity> input, Conditions condition)
        {
            var filterDefinition = input.FilterDefinition;
            var result = initPage.ToList();
            var page = initPage;

            if (initPage.Count() < size)
            {
                filterDefinition.RemoveFixedFilter(filterDefinition.FixedBundle.Filters.LastOrDefault());
                while (filterDefinition.FixedBundle.Filters.Any())
                {

                    var filter = filterDefinition.FixedBundle.Filters.LastOrDefault();
                    if (filter is FieldFilterDefinition fieldFilter)
                    {
                        fieldFilter.Condition = condition;
                        var filterExpr = filterDefinition.GetWhereExpresssion<TEntity>(input.Param);

                        var fullExpr = FilterItemDefinition.AppendExpression(input.LookupExpression, filterExpr, EndLogics.And);

                        var query = FilterItemDefinition.FilterQuery(input.Query, input.Param, fullExpr);
                        ApplyOrderBys(query);

                        size -= page.Count();

                        query = query.Take(size);

                        if (condition == Conditions.GreaterThan)
                        {
                            result.AddRange(query);
                            page = query;
                        }

                        filterDefinition.RemoveFixedFilter(filterDefinition.FixedBundle.Filters.LastOrDefault());
                        size -= page.Count();
                        if (page.Count() >= size)
                        {
                            break;
                        }
                    }
                }
            }
            return result;
        }

        private bool CreateFilterItem(Conditions condition, ParameterExpression param, TableFilterDefinition<TEntity> filterDefinition,
            FieldDefinition fieldDefinition, string value, Expression lookupExpr)
        {
            bool hasMoreThan1Record;
            var filter = filterDefinition.AddFixedFilter(fieldDefinition, Conditions.Equals, value);
            if (filter != null)
            {
                var entityExpr = filterDefinition.GetWhereExpresssion<TEntity>(param);

                var fullExpr = FilterItemDefinition.AppendExpression(lookupExpr, entityExpr, EndLogics.And);

                var query = TableDefinition.Context.GetQueryable<TEntity>(LookupDefinition);

                query = FilterItemDefinition.FilterQuery(query, param, fullExpr);
                query = query.Take(2);

                hasMoreThan1Record = query.Count() > 1;

                if (!hasMoreThan1Record)
                {
                    filter.Condition = condition;
                    return true;
                }
            }

            return false;
        }

        private void LookupCallBack_RefreshData(object sender, EventArgs e)
        {
            RefreshData();
            OnDataSourceChanged();
        }

        private TableFilterDefinition<TEntity> GetPageFilters(TEntity entity)
        {
            var result = new TableFilterDefinition<TEntity>(TableDefinition);

            var orderBys = OrderByList.OfType<LookupFieldColumnDefinition>();

            foreach (var orderColumn in orderBys)
            {
                var value = orderColumn.GetDatabaseValue(entity);

                result.AddFixedFilter(orderColumn.FieldDefinition, Conditions.Equals, value);
            }

            foreach (var primaryKeyField in TableDefinition.PrimaryKeyFields)
            {
                var value = GblMethods.GetPropertyValue(entity, primaryKeyField.PropertyName);
                result.AddFixedFilter(primaryKeyField, Conditions.Equals, value);
            }
            return result;
        }
    }
}
