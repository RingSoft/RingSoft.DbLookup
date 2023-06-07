using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using MySqlX.XDevAPI.Common;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DbLookup.TableProcessing;

namespace RingSoft.DbLookup.Lookup
{
    public abstract class LookupDataMauiBase
    {
        public int PageSize { get; protected set; }

        public abstract int RowCount { get; }
        
        public LookupDefinitionBase LookupDefinition { get; }

        public event EventHandler LookupDataChanged;

        public LookupDataMauiBase(LookupDefinitionBase lookupDefinition)
        {
            LookupDefinition = lookupDefinition;
        }

        protected void FireLookupDataChangedEvent()
        {
            LookupDataChanged?.Invoke(this, EventArgs.Empty);
        }

        public abstract void GetInitData(int pageSize);

        public abstract string GetFormattedRowValue(int rowIndex, LookupColumnDefinitionBase column);

        public abstract string GetDatabaseRowValue(int rowIndex, LookupColumnDefinitionBase column);

        public abstract int GetRecordCount();

        public abstract void ClearData();
    }
    public class LookupDataMaui<TEntity> : LookupDataMauiBase where TEntity : new()
    {
        public IQueryable<TEntity> BaseQuery { get; }

        public IQueryable<TEntity> FilteredQuery { get; private set; }

        public IQueryable<TEntity> ProcessedQuery { get; private set; }

        public List<TEntity> CurrentList { get; } = new List<TEntity>();

        public override int RowCount => CurrentList.Count;

        public LookupDataMaui(IQueryable<TEntity> query, LookupDefinitionBase lookupDefinition)
        : base(lookupDefinition)
        {
            BaseQuery = query;
        }


        public override void GetInitData(int pageSize)
        {
            PageSize = pageSize;

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


            ProcessedQuery = FilteredQuery.Take(PageSize);

            CurrentList.Clear();

            CurrentList.AddRange(ProcessedQuery);

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
    }
}
