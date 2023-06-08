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

        /// <summary>
        /// Gets or sets the parent window's primary key value.
        /// </summary>
        /// <value>
        /// The parent window's primary key value.
        /// </value>
        public PrimaryKeyValue ParentWindowPrimaryKeyValue { get; set; }

        public ILookupControl LookupControl { get; private set; }

        public ILookupWindow LookupWindow { get; private set; }

        /// <summary>
        /// Occurs when a user wishes to view a selected lookup row.  Used to show the appropriate editor for the selected lookup row.
        /// </summary>
        public event EventHandler<LookupAddViewArgs> LookupView;

        public event EventHandler LookupDataChanged;
        public event EventHandler DataSourceChanged;

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

        public bool InputMode { get; protected set; }

        public PrimaryKeyValue SelectedPrimaryKeyValue { get; protected set; }

        public abstract PrimaryKeyValue GetPrimaryKeyValueForSearchText(string searchText);

        public abstract void SelectPrimaryKey(PrimaryKeyValue primaryKeyValue);

        public abstract void ViewSelectedRow(object ownerWindow, object AddViewParameter, bool readOnlyMode = false);

        public abstract void AddNewRow(object ownerWindow, object inputParameter = null);

        public abstract void RefreshData();

        public void SetParentControls(ILookupControl control, ILookupWindow lookupWindow = null)
        {
            LookupControl = control;
            LookupWindow = lookupWindow;
        }

        public abstract string GetSelectedText();

        /// <summary>
        /// Occurs when a user wishes to view a selected lookup row.  Fires the LookupView event.
        /// </summary>
        /// <param name="e">The lookup primary key row arguments.</param>
        protected virtual void OnLookupView(LookupAddViewArgs e)
        {
            LookupView?.Invoke(this, e);
        }

        protected virtual void OnDataSourceChanged()
        {
            DataSourceChanged?.Invoke(this, EventArgs.Empty);
        }

        public abstract PrimaryKeyValue GetSelectedPrimaryKeyValue();
    }
    public class LookupDataMaui<TEntity> : LookupDataMauiBase where TEntity : class, new()
    {
        public TableDefinition<TEntity> TableDefinition { get; }

        public IQueryable<TEntity> BaseQuery { get; private set; }

        public IQueryable<TEntity> FilteredQuery { get; private set; }

        public IQueryable<TEntity> ProcessedQuery { get; private set; }

        public List<TEntity> CurrentList { get; } = new List<TEntity>();

        public override int RowCount => CurrentList.Count;

        private bool _selectingRecord;

        public LookupDataMaui(IQueryable<TEntity> query, LookupDefinitionBase lookupDefinition)
        : base(lookupDefinition)
        {
            if (lookupDefinition.TableDefinition is TableDefinition<TEntity> table)
            {
                TableDefinition = table;
            }
            BaseQuery = query;
        }

        public LookupDataMaui(LookupDefinitionBase lookupDefinition) : base(lookupDefinition)
        {
            if (lookupDefinition.TableDefinition is TableDefinition<TEntity> table)
            {
                TableDefinition = table;
            }
        }
        public override void GetInitData(int pageSize)
        {
            PageSize = pageSize;

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
            throw new NotImplementedException();
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
            //if (LookupDefinition.InitialSortColumnDefinition is LookupFieldColumnDefinition fieldColumn)
            //{
            //    var expression = FilterItemDefinition.GetBinaryExpression<TEntity>(param
            //    , fieldColumn.GetPropertyJoinName(), Conditions.Contains, "C");

            //    var newQuery = FilterItemDefinition.FilterQuery(BaseQuery, param, expression);
            //}

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

        private void LookupCallBack_RefreshData(object sender, EventArgs e)
        {
            RefreshData();
            OnDataSourceChanged();
        }
    }
}
