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
            var result = LookupScrollPositions.Top;

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
                    MakeList(entity, LookupControl.PageSize - 1, 0);
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

            while (input.FilterDefinition.FixedFilters.Any())
            {
                var filterItem = input.FilterDefinition.FixedFilters.OfType<FieldFilterDefinition>().LastOrDefault();
                if (filterItem != null)
                {
                    filterItem.Condition = condition;
                }

                var filterExpr = input.FilterDefinition.GetWhereExpresssion<TEntity>(input.Param);

                var fullExpr = FilterItemDefinition
                    .AppendExpression(input.LookupExpression, filterExpr, EndLogics.And);

                var query = FilterItemDefinition.FilterQuery(input.Query, input.Param, fullExpr);

                query = ApplyOrderBys(query, true);

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
                        input.FilterDefinition.RemoveFixedFilter(lastFilter);
                    }
                }
            }
            
            return result;
        }


        private LookupDataMauiProcessInput<TEntity> GetProcessInput(TEntity entity)
        {
            var filterDefinition = new TableFilterDefinition<TEntity>(TableDefinition);
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

            foreach (var orderBy in orderBys)
            {
                var value = orderBy.GetDatabaseValue(entity);
                var field = orderBy.FieldDefinition;
                var filterItem = filterDefinition.AddFixedFilter(field, Conditions.Equals, value);
                filterItem.PropertyName = orderBy.GetPropertyJoinName();
            }

            foreach (var primaryKeyField in TableDefinition.PrimaryKeyFields)
            {
                var value = GblMethods.GetPropertyValue(entity, primaryKeyField.PropertyName);
                filterDefinition.AddFixedFilter(primaryKeyField, Conditions.Equals, value);
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

        private bool CreateFilterItem(Conditions condition, ParameterExpression param, TableFilterDefinition<TEntity> filterDefinition,
            FieldDefinition fieldDefinition, string value, Expression lookupExpr, string propName)
        {
            bool hasMoreThan1Record;
            var filter = filterDefinition.AddFixedFilter(fieldDefinition, Conditions.Equals, value);

            if (filter != null)
                filter.PropertyName = propName;
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
                    return false;
                }
            }

            return true;
        }

        private void LookupCallBack_RefreshData(object sender, EventArgs e)
        {
            RefreshData();
            OnDataSourceChanged();
        }

        private void MakeList(TEntity entity, int topCount, int bottomCount)
        {
            CurrentList.Clear();
            if (topCount > 0)
            {
                var previousPage = GetPreviousPage(entity, topCount);
                if (previousPage != null && previousPage.Any())
                {
                    CurrentList.AddRange(previousPage);
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
        }

        private List<TEntity> GetPreviousPage(TEntity entity, int count)
        {
            var result = new List<TEntity>();
            return result;
        }

        private List<TEntity> GetNextPage(TEntity entity, int count)
        {
            var result = new List<TEntity>();
            return result;
        }
    }
}
