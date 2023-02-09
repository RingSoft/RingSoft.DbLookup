using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DbLookup.TableProcessing;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.DataProcessor;

namespace RingSoft.DbLookup.Lookup
{
    public enum LookupScrollPositions
    {
        Disabled = 0,
        Top = 1,
        Middle = 2,
        Bottom = 3
    }

    /// <summary>
    /// Arguments sent when the lookup's selected row index changes.
    /// </summary>
    public class SelectedIndexChangedEventArgs
    {
        /// <summary>
        /// Creates new index.
        /// </summary>
        /// <value>
        /// The new index.
        /// </value>
        public int NewIndex { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectedIndexChangedEventArgs"/> class.
        /// </summary>
        /// <param name="newIndex">The new index.</param>
        public SelectedIndexChangedEventArgs(int newIndex)
        {
            NewIndex = newIndex;
        }
    }

    /// <summary>
    /// The base class containing a lookup's data.
    /// </summary>
    public class LookupDataBase
    {
        /// <summary>
        /// Gets the lookup results data table.
        /// </summary>
        /// <value>
        /// The lookup results data table.
        /// </value>
        public DataTable LookupResultsDataTable { get; private set; }

        /// <summary>
        /// Gets the lookup definition.
        /// </summary>
        /// <value>
        /// The lookup definition.
        /// </value>
        public LookupDefinitionBase LookupDefinition { get; set; }

        /// <summary>
        /// Gets the lookup control.
        /// </summary>
        /// <value>
        /// The lookup control.
        /// </value>
        public ILookupControl LookupControl { get; }

        /// <summary>
        /// Gets the sort column definition.
        /// </summary>
        /// <value>
        /// The sort column definition.
        /// </value>
        public LookupColumnDefinitionBase SortColumnDefinition { get; set; }

        /// <summary>
        /// Gets the type of the order by.
        /// </summary>
        /// <value>
        /// The type of the order by.
        /// </value>
        public OrderByTypes OrderByType { get; private set; }

        /// <summary>
        /// Gets the scroll position.
        /// </summary>
        /// <value>
        /// The scroll position.
        /// </value>
        public LookupScrollPositions ScrollPosition { get; internal set; }

        /// <summary>
        /// Gets the record count.
        /// </summary>
        /// <value>
        /// The record count.
        /// </value>
        public int RecordCount { get; private set; }

        /// <summary>
        /// Gets or sets the index of the selected row.
        /// </summary>
        /// <value>
        /// The index of the selected row.
        /// </value>
        public int SelectedRowIndex
        {
            get { return _selectedIndex; }
            set
            {
                _selectedIndex = value;
                SelectedIndexChanged?.Invoke(this, new SelectedIndexChangedEventArgs(value));
            }
        }

        /// <summary>
        /// Gets the order by list.
        /// </summary>
        /// <value>
        /// The order by list.
        /// </value>
        public IReadOnlyList<LookupColumnDefinitionBase> OrderByList => _orderByList;

        /// <summary>
        /// Gets the selected primary key value.
        /// </summary>
        /// <value>
        /// The selected primary key value.
        /// </value>
        public PrimaryKeyValue SelectedPrimaryKeyValue
        {
            get
            {
                var primaryKeyValue = new PrimaryKeyValue(LookupDefinition.TableDefinition);
                var dataRow = GetSelectedRow();
                if (dataRow != null)
                    primaryKeyValue.PopulateFromDataRow(dataRow);

                return primaryKeyValue;
            }
        }

        /// <summary>
        /// Gets or sets the parent window's primary key value.
        /// </summary>
        /// <value>
        /// The parent window's primary key value.
        /// </value>
        public PrimaryKeyValue ParentWindowPrimaryKeyValue { get; set; }

        /// <summary>
        /// Occurs when this object's data has changed.
        /// </summary>
        public event EventHandler<LookupDataChangedArgs> LookupDataChanged;

        public event EventHandler<LookupDataChangedArgs> PrintDataChanged;

        /// <summary>
        /// Occurs when a user wishes to view a selected lookup row.  Used to show the appropriate editor for the selected lookup row.
        /// </summary>
        public event EventHandler<LookupAddViewArgs> LookupView;

        /// <summary>
        /// Occurs when a window changes the database data.
        /// </summary>
        public event EventHandler DataSourceChanged; 

        /// <summary>
        /// Occurs when the selected index has changed.
        /// </summary>
        public event EventHandler<SelectedIndexChangedEventArgs> SelectedIndexChanged;

        private readonly List<LookupColumnDefinitionBase> _orderByList = new List<LookupColumnDefinitionBase>();
        private int _selectedIndex;
        private bool _selectingRecord;
        private bool _countingRecords;
        private int _countProcess;
        private bool _searchForChanging;
        private bool _printMode;

        private void OutputData(int selectedRowIndex, LookupScrollPositions currentPosition)
        {
            SetScrollPosition(currentPosition);
            if (LookupControl.PageSize == 1)
                SelectedRowIndex = LookupResultsDataTable.Rows.Count - 1;
            else
            {
                SelectedRowIndex = selectedRowIndex;
            }

            if (ScrollPosition == LookupScrollPositions.Disabled)
                RecordCount = LookupResultsDataTable.Rows.Count;

            ProcessLookupData();

            var args = new LookupDataChangedArgs(LookupResultsDataTable, SelectedRowIndex, ScrollPosition)
            {
                CountingRecords = _countingRecords,
                SearchForChanging = _searchForChanging
            };
            LookupDataChanged?.Invoke(this, args);
        }

        protected virtual void ProcessLookupData()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupDataBase"/> class.
        /// </summary>
        /// <param name="lookupDefinition">The lookup definition.</param>
        /// <param name="userInterface">The user interface.</param>
        public LookupDataBase(LookupDefinitionBase lookupDefinition, ILookupControl userInterface)
        {
            //if (lookupDefinition.InitialSortColumnDefinition == null)
            //    throw new ArgumentException(
            //        "Lookup definition does not have any visible columns defined or its initial sort column is null.");

            LookupDefinition = lookupDefinition;
            LookupControl = userInterface;
            SortColumnDefinition = lookupDefinition.InitialOrderByColumn;
            OrderByType = lookupDefinition.InitialOrderByType;
        }

        /// <summary>
        /// Gets the first top page of data.
        /// </summary>
        /// <returns>The data processor result.</returns>
        public DataProcessResult GetInitData()
        {
            return GetInitData(true, true);
        }

        private DataProcessResult GetInitData(bool resetSelectedRowIndex, bool resetRecordCount, int newSelectedIndex = -1)
        {
            if (resetRecordCount)
                RecordCount = 0;

            var query = GetQuery();

            query.DebugMessage = "LookupData.GetInitData";
            var getDataResult = LookupDefinition.TableDefinition.Context.DataProcessor.GetData(query, !_printMode);

            if (getDataResult.ResultCode == GetDataResultCodes.Success)
            {
                LookupResultsDataTable = getDataResult.DataSet.Tables[0];
                var selectedIndex = newSelectedIndex;
                if (LookupResultsDataTable.Rows.Count <= 0)
                    selectedIndex = -1;

                if (!resetSelectedRowIndex && LookupResultsDataTable.Rows.Count > 0)
                    selectedIndex = SelectedRowIndex;
                OutputData(selectedIndex, LookupScrollPositions.Top);
            }

            return getDataResult;
        }

        public SelectQuery GetQuery()
        {
            var query = new SelectQuery(LookupDefinition.TableDefinition.TableName).SetMaxRecords(LookupControl.PageSize);
            query.BaseTable.Formula = LookupDefinition.FromFormula;

            SetupBaseQuery(query, false);
            return query;
        }

        /// <summary>
        /// Selects the primary key.
        /// </summary>
        /// <param name="primaryKey">The primary key.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Primary key value Table Definition '{PrimaryKeyValue.TableDefinition.TableName}' does not match this Lookup Definition's Table Definition '{LookupDefinition.TableDefinition.TableName}'</exception>
        public DataProcessResult SelectPrimaryKey(PrimaryKeyValue primaryKey)
        {
            if (primaryKey.TableDefinition != LookupDefinition.TableDefinition)
                throw new ArgumentException(
                    $"Primary key value Table Definition '{SelectedPrimaryKeyValue.TableDefinition.TableName}' does not match this Lookup Definition's Table Definition '{LookupDefinition.TableDefinition.TableName}'");

            var query = new SelectQuery(LookupDefinition.TableDefinition.TableName).SetMaxRecords(1);

            SetupBaseQuery(query, false);

            foreach (var keyValueField in primaryKey.KeyValueFields)
            {
                var dateType = DbDateTypes.DateOnly;
                if (keyValueField.FieldDefinition is DateFieldDefinition dateField)
                    dateType = dateField.DateType;

                query.AddWhereItem(query.BaseTable, keyValueField.FieldDefinition.FieldName, Conditions.Equals,
                    keyValueField.Value, keyValueField.FieldDefinition.ValueType, dateType);
            }

            query.DebugMessage = "LookupData.SelectPrimaryKey";
            var getDataResult = LookupDefinition.TableDefinition.Context.DataProcessor.GetData(query, !_printMode);

            if (getDataResult.ResultCode == GetDataResultCodes.Success)
            {
                var dataTable = getDataResult.DataSet.Tables[0];
                LookupResultsDataTable = dataTable;
                var selectedIndex = -1;
                if (LookupResultsDataTable.Rows.Count > 0)
                    selectedIndex = 0;

                if (LookupControl.PageSize > 1 && selectedIndex == 0)
                    FillAroundFoundEqualsRow();
                else
                {
                    if (LookupControl.PageSize > 1)
                    {
                        getDataResult = GetInitData();
                    }
                    else
                    {
                        OutputData(selectedIndex, LookupScrollPositions.Middle);
                    }
                }
            }

            return getDataResult;
        }

        public string GetSqlStatement()
        {
            var query = new SelectQuery(LookupDefinition.TableDefinition.TableName);
            SetupBaseQuery(query, false);

            var sql = LookupDefinition.TableDefinition.Context.DataProcessor.SqlGenerator
                .GenerateSelectStatement(query);

            return sql;
        }

        private void SetupBaseQuery(SelectQuery query, bool reverseOrderBy, bool addSortColumnToQueryWhere = true)
        {
            query.BaseTable.Formula = LookupDefinition.FromFormula;
            TableFilterDefinitionBase.ProcessFieldJoins(query, LookupDefinition.Joins);

            var distinctColumns = LookupDefinition.GetDistinctColumns();
            if (distinctColumns.Any())
            {
                foreach (var column in distinctColumns)
                {
                    query.AddSelectColumn(column.FieldDefinition.FieldName);
                }
            }
            else
            {
                foreach (var tableDefinitionPrimaryKeyField in LookupDefinition.TableDefinition.PrimaryKeyFields)
                {
                    query.AddSelectColumn(tableDefinitionPrimaryKeyField.FieldName);
                }
            }

            foreach (var lookupDefinitionColumn in LookupDefinition.VisibleColumns)
            {
                AddColumnToQuery(query, lookupDefinitionColumn, false);
            }

            foreach (var lookupDefinitionColumn in LookupDefinition.HiddenColumns)
            {
                AddColumnToQuery(query, lookupDefinitionColumn, true);
            }

            LookupDefinition.FilterDefinition.ProcessQuery(query);

            if (addSortColumnToQueryWhere)
                AddSortColumnToQueryWhere(query);

            SetupQueryOrderBy(query, reverseOrderBy);
        }

        private void AddSortColumnToQueryWhere(SelectQuery query)
        {
            if (LookupControl.SearchType == LookupSearchTypes.Contains &&
                SortColumnDefinition.DataType == FieldDataTypes.String)
            {
                AddSortColumnToQueryWhere(query, Conditions.Contains, LookupControl.SearchText);
            }
        }

        private void AddSortColumnToQueryWhere(SelectQuery query, Conditions condition, string searchText)
        {
            switch (SortColumnDefinition.ColumnType)
            {
                case LookupColumnTypes.Field:
                    if (SortColumnDefinition is LookupFieldColumnDefinition sortFieldColumnDefinition)
                    {
                        {
                            var searchQueryTable = GetQueryTableForColumn(query, sortFieldColumnDefinition);

                            query.AddWhereItem(searchQueryTable,
                                sortFieldColumnDefinition.FieldDefinition.FieldName, condition, searchText,
                                sortFieldColumnDefinition.FieldDefinition.ValueType == ValueTypes.Memo);
                        }
                    }

                    break;
                case LookupColumnTypes.Formula:
                    if (SortColumnDefinition is LookupFormulaColumnDefinition sortFormulaColumnDefinition)
                    {
                        query.AddWhereItemFormula(sortFormulaColumnDefinition.Formula, condition, searchText,
                            sortFormulaColumnDefinition.ValueType, sortFormulaColumnDefinition.DateType);
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void AddColumnToQuery(SelectQuery query, LookupColumnDefinitionBase lookupDefinitionColumn, bool hiddenColumn)
        {
            var sqlFieldName = lookupDefinitionColumn.SelectSqlAlias;
            var queryTable = GetQueryTableForColumn(query, lookupDefinitionColumn);

            switch (lookupDefinitionColumn.ColumnType)
            {
                case LookupColumnTypes.Field:
                    if (lookupDefinitionColumn is LookupFieldColumnDefinition lookupFieldColumn)
                    {
                        switch (lookupFieldColumn.FieldDefinition.FieldDataType)
                        {
                            case FieldDataTypes.Integer:
                                if (lookupFieldColumn.FieldDefinition is IntegerFieldDefinition integerFieldDefinition)
                                {
                                    if (hiddenColumn || integerFieldDefinition.EnumTranslation == null)
                                    {
                                        query.AddSelectColumn(lookupFieldColumn.FieldDefinition.FieldName, queryTable,
                                            sqlFieldName, lookupFieldColumn.Distinct);
                                    }
                                    else
                                    {
                                        query.AddSelectEnumColumn(queryTable, integerFieldDefinition.FieldName, sqlFieldName,
                                            integerFieldDefinition.EnumTranslation);
                                    }
                                }
                                break;
                            case FieldDataTypes.Bool:
                                if (lookupFieldColumn.FieldDefinition is BoolFieldDefinition boolField)
                                {
                                    query.AddSelectBooleanColumn(queryTable, boolField.FieldName, sqlFieldName,
                                        boolField.TrueText, boolField.FalseText);
                                }
                                break;
                            default:
                                query.AddSelectColumn(lookupFieldColumn.FieldDefinition.FieldName, queryTable,
                                    sqlFieldName, lookupFieldColumn.Distinct);
                                break;
                        }
                    }
                    break;
                case LookupColumnTypes.Formula:
                    if (lookupDefinitionColumn is LookupFormulaColumnDefinition lookupFormulaColumn)
                    {
                        query.AddSelectFormulaColumn(lookupFormulaColumn.SelectSqlAlias, lookupFormulaColumn.Formula);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void SetupQueryOrderBy(SelectQuery query, bool reverseOrderBy)
        {
            var orderBy = OrderByType;
            if (reverseOrderBy)
            {
                if (OrderByType == OrderByTypes.Ascending)
                {
                    orderBy = OrderByTypes.Descending;
                }
                else
                {
                    orderBy = OrderByTypes.Ascending;
                }
            }

            if (SortColumnDefinition != null)
            {
                AddColumnToQueryOrderBy(SortColumnDefinition, query, orderBy);
            }
            
            foreach (var lookupColumnDefinition in _orderByList)
            {
                AddColumnToQueryOrderBy(lookupColumnDefinition, query, orderBy);
            }

            var distinctColumns = LookupDefinition.GetDistinctColumns();
            if (!distinctColumns.Any())
            {
                foreach (var tableDefinitionPrimaryKeyField in LookupDefinition.TableDefinition.PrimaryKeyFields)
                {
                    if (!query.OrderBySegments.Any(o =>
                        o.Table.Name == tableDefinitionPrimaryKeyField.TableDefinition.TableName &&
                        o.FieldName == tableDefinitionPrimaryKeyField.FieldName))
                        query.AddOrderBySegment(tableDefinitionPrimaryKeyField.FieldName, orderBy);
                }
            }
        }

        private void AddColumnToQueryOrderBy(LookupColumnDefinitionBase columnDefinition, SelectQuery query, OrderByTypes orderBy)
        {
            switch (columnDefinition.ColumnType)
            {
                case LookupColumnTypes.Field:
                    if (columnDefinition is LookupFieldColumnDefinition fieldColumnDefinition)
                    {
                        switch (fieldColumnDefinition.FieldDefinition.FieldDataType)
                        {
                            case FieldDataTypes.Integer:
                                if (fieldColumnDefinition.FieldDefinition is IntegerFieldDefinition
                                        integerFieldDefinition
                                    && integerFieldDefinition.EnumTranslation != null)
                                {
                                    query.AddOrderByEnumSegment(GetQueryTableForColumn(query, fieldColumnDefinition),
                                        fieldColumnDefinition.FieldDefinition.FieldName, orderBy,
                                        integerFieldDefinition.EnumTranslation, false);
                                }
                                else
                                {
                                    query.AddOrderBySegment(GetQueryTableForColumn(query, fieldColumnDefinition),
                                        fieldColumnDefinition.FieldDefinition.FieldName, orderBy);
                                }
                                break;
                            case FieldDataTypes.Bool:
                                if (fieldColumnDefinition.FieldDefinition is BoolFieldDefinition boolFieldDefinition)
                                {
                                    query.AddOrderByBoolTextSegment(
                                        GetQueryTableForColumn(query, fieldColumnDefinition),
                                        fieldColumnDefinition.FieldDefinition.FieldName, orderBy,
                                        boolFieldDefinition.TrueText, boolFieldDefinition.FalseText);
                                }
                                break;
                            default:
                                query.AddOrderBySegment(GetQueryTableForColumn(query, fieldColumnDefinition),
                                    fieldColumnDefinition.FieldDefinition.FieldName, orderBy);
                                break;
                        }
                    }
                    break;
                case LookupColumnTypes.Formula:
                    if (columnDefinition is LookupFormulaColumnDefinition lookupFormulaColumn)
                    {
                        query.AddOrderByFormulaSegment(lookupFormulaColumn.Formula, orderBy);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private QueryTable GetQueryTableForColumn(SelectQuery query, LookupColumnDefinitionBase lookupColumnDefinition)
        {
            switch (lookupColumnDefinition.ColumnType)
            {
                case LookupColumnTypes.Field:
                    if (lookupColumnDefinition is LookupFieldColumnDefinition lookupFieldColumnDefinition)
                    {
                        if (!lookupFieldColumnDefinition.JoinQueryTableAlias.IsNullOrEmpty())
                        {
                            QueryTable foreignTable =
                                query.JoinTables.FirstOrDefault(f => f.Alias == lookupFieldColumnDefinition.JoinQueryTableAlias);
                            if (foreignTable != null)
                                return foreignTable;
                        }
                    }
                    break;
                case LookupColumnTypes.Formula:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return query.BaseTable;
        }

        /// <summary>
        /// Called when a column header is clicked.
        /// </summary>
        /// <param name="visibleColumnIndex">Index of the visible column.</param>
        /// <param name="resetSortOrder">if set to <c>true</c> then reset the sort order.</param>
        /// <exception cref="ArgumentException">Visible column at index {visibleColumnIndex} does not exist.</exception>
        public void OnColumnClick(int visibleColumnIndex, bool resetSortOrder)
        {
            if (LookupDefinition.VisibleColumns.Count <= visibleColumnIndex)
                throw new ArgumentException($"Visible column at index {visibleColumnIndex} does not exist.");

            var column = LookupDefinition.VisibleColumns[visibleColumnIndex];
            if (SortColumnDefinition == column)
            {
                resetSortOrder = true;
                if (OrderByType == OrderByTypes.Ascending)
                    OrderByType = OrderByTypes.Descending;
                else
                {
                    OrderByType = OrderByTypes.Ascending;
                }
            }
            else
            {
                if (resetSortOrder)
                    OrderByType = OrderByTypes.Ascending;
            }

            if (resetSortOrder)
            {
                SortColumnDefinition = column;
                _orderByList.Clear();
                if (SortColumnDefinition != LookupDefinition.InitialSortColumnDefinition)
                    _orderByList.Add(LookupDefinition.InitialSortColumnDefinition);
            }
            else
            {
                if (_orderByList.Contains(column))
                {
                    _orderByList.Remove(column);
                }
                else
                {
                    _orderByList.Add(column);
                }
            }

            if (LookupResultsDataTable == null || LookupResultsDataTable.Rows.Count <= 0)
                return;

            if (resetSortOrder)
                GetInitData(true, false);
            else 
                OnSearchForChange(LookupControl.SearchText);
        }

        public void OnMouseWheelForward()
        {
            var newSelectedRowIndex = SelectedRowIndex;
            if (SelectedRowIndex >= 0)
            {
                newSelectedRowIndex = SelectedRowIndex - 3;
                if (newSelectedRowIndex < 0)
                    newSelectedRowIndex = -1;
            }

            GotoNextRecord(3, newSelectedRowIndex, false);
        }

        /// <summary>
        /// Goto the next record.
        /// </summary>
        public void GotoNextRecord(int recordCount = 1)
        {
            GotoNextRecord(recordCount, 0, true);
        }

        private void GotoNextRecord(int recordCount, int selectedRowIndex, bool setSelIndexToBottom)
        {
            if (LookupResultsDataTable == null || LookupResultsDataTable.Rows.Count <= 0)
                return;

            var startIndex = recordCount - 1;
            var newDataTable = LookupResultsDataTable.Clone();
            if (GetNextRecords(LookupResultsDataTable.Rows[startIndex], LookupControl.PageSize, newDataTable, "GotoNextRecord"))
            {
                var newStartIndex = startIndex - (LookupControl.PageSize - newDataTable.Rows.Count);
                if (recordCount > 1 && newDataTable.Rows.Count < LookupControl.PageSize && newStartIndex >= 0 && !_printMode)
                {
                    newDataTable = LookupResultsDataTable.Clone();
                    GetNextRecords(LookupResultsDataTable.Rows[newStartIndex],
                        LookupControl.PageSize, newDataTable, "GotoNextPage");
                }
                if (newDataTable.Rows.Count >= LookupControl.PageSize)
                {
                    if (setSelIndexToBottom)
                        selectedRowIndex = LookupResultsDataTable.Rows.Count - 1;
                    else if (selectedRowIndex > 0)
                    {
                        selectedRowIndex += (startIndex - newStartIndex);
                    }
                    LookupResultsDataTable = newDataTable;
                    OutputData(selectedRowIndex, LookupScrollPositions.Middle);
                }

                if (newDataTable.Rows.Count < LookupControl.PageSize && _printMode)
                {
                    LookupResultsDataTable = newDataTable;
                    OutputData(selectedRowIndex, LookupScrollPositions.Bottom);

                }
            }
        }

        /// <summary>
        /// Goto the next page.
        /// </summary>
        public void GotoNextPage()
        {
            GotoNextRecord(LookupControl.PageSize);
        }

        public void OnMouseWheelBack()
        {
            var newSelectedRowIndex = SelectedRowIndex;
            if (SelectedRowIndex >= 0)
            {
                newSelectedRowIndex = SelectedRowIndex + 3;
                if (newSelectedRowIndex >= LookupControl.PageSize)
                    newSelectedRowIndex = -1;
            }

            GotoPreviousRecord(3, newSelectedRowIndex, false);
        }

        /// <summary>
        /// Goto the previous record.
        /// </summary>
        public void GotoPreviousRecord(int recordCount = 1)
        {
            GotoPreviousRecord(recordCount, 0, true);
        }

        private void GotoPreviousRecord(int recordCount, int selectedRowIndex, bool setSelIndexToTop)
        {
            if (LookupResultsDataTable == null || LookupResultsDataTable.Rows.Count <= 0)
                return;

            var startIndex = LookupResultsDataTable.Rows.Count - recordCount;
            var newDataTable = LookupResultsDataTable.Clone();
            if (GetPreviousRecords(LookupResultsDataTable.Rows[startIndex],
                LookupControl.PageSize, newDataTable, "GotoPreviousRecord"))
            {
                if (newDataTable.Rows.Count > 0)
                {
                    var newStartIndex = startIndex + (LookupResultsDataTable.Rows.Count - newDataTable.Rows.Count);
                    if (recordCount > 1 && newDataTable.Rows.Count < LookupControl.PageSize &&
                        newStartIndex < LookupResultsDataTable.Rows.Count)
                    {
                        newDataTable = LookupResultsDataTable.Clone();
                        GetPreviousRecords(LookupResultsDataTable.Rows[newStartIndex],
                            LookupControl.PageSize, newDataTable, "OnPageUp");
                    }
                    if (newDataTable.Rows.Count >= LookupControl.PageSize)
                    {
                        if (setSelIndexToTop)
                            selectedRowIndex = 0;
                        else if (selectedRowIndex < LookupControl.PageSize - 1)
                        {
                            selectedRowIndex += (startIndex - newStartIndex);
                        }

                        LookupResultsDataTable = newDataTable;
                        OutputData(selectedRowIndex, LookupScrollPositions.Middle);
                    }
                }
            }
        }

        /// <summary>
        /// Goto the previous page.
        /// </summary>
        public void GotoPreviousPage()
        {
            GotoPreviousRecord(LookupControl.PageSize);
        }

        private bool GetNextRecords(DataRow startRow, int recordCount, DataTable resultsTable, string debugMessage)
        {
            var query = new SelectQuery(LookupDefinition.TableDefinition.TableName);
            SetupBaseQuery(query, false);

            var lastWhereIndex = GetLastWhereIndex(startRow, query, $"{debugMessage}.GetNextRecords");
            var checkNull = false;
            var recordsFound = 0;
            var originalRecordsCount = recordCount;
            if (query.WhereItems.Any())
            {
                while (lastWhereIndex >= 0 && recordCount > 0)
                {
                    {
                        var lastWhereItem = query.WhereItems[query.WhereItems.Count - 1];
                        if (lastWhereItem.Value.IsNullOrEmpty() && lastWhereIndex == 0 &&
                            recordsFound < originalRecordsCount
                            && OrderByType == OrderByTypes.Descending)
                            break;

                        if (checkNull)
                        {
                            lastWhereItem.UpdateCondition(Conditions.EqualsNull);
                        }
                        else
                        {
                            if (OrderByType == OrderByTypes.Ascending)
                            {
                                lastWhereItem.UpdateCondition(Conditions.GreaterThan);
                            }
                            else
                            {
                                lastWhereItem.UpdateCondition(Conditions.LessThan);
                            }
                        }

                        query.SetMaxRecords(recordCount);
                        query.DebugMessage = $"LookupData.{debugMessage}.GetNextRecords";
                        var result = LookupDefinition.TableDefinition.Context.DataProcessor.GetData(query, !_printMode);
                        if (result.ResultCode == GetDataResultCodes.Success)
                        {
                            foreach (DataRow dataRow in result.DataSet.Tables[0].Rows)
                            {
                                resultsTable.ImportRow(dataRow);
                            }

                            recordsFound += result.DataSet.Tables[0].Rows.Count;
                            recordCount -= result.DataSet.Tables[0].Rows.Count;
                        }

                        if (lastWhereIndex == 0 && recordCount > 0 && !checkNull &&
                            OrderByType == OrderByTypes.Descending)
                        {
                            checkNull = true;
                        }
                        else
                        {
                            query.RemoveWhereItem(lastWhereItem);
                            lastWhereIndex--;
                        }
                    }
                }
            }

            return true;
        }

        private int GetLastWhereIndex(DataRow startRow, SelectQuery query, string debugMessage)
        {
            var lastWhereIndex = 0;

            var hasMoreThan1Record = false;
            var searchValue = string.Empty;
            if (SortColumnDefinition != null)
            {
                searchValue = startRow.GetRowValue(SortColumnDefinition.SelectSqlAlias);
                hasMoreThan1Record = HasMoreThan1Record(searchValue, SortColumnDefinition, query,
                    GetQueryTableForColumn(query, SortColumnDefinition), $"{debugMessage}.GetLastWhereIndex");

            }


            if (hasMoreThan1Record)
            {
                foreach (var lookupColumnDefinition in _orderByList)
                {
                    lastWhereIndex++;
                    searchValue = startRow.GetRowValue(lookupColumnDefinition.SelectSqlAlias);
                    hasMoreThan1Record = HasMoreThan1Record(searchValue, lookupColumnDefinition, query,
                        GetQueryTableForColumn(query, lookupColumnDefinition), $"{debugMessage}.GetLastWhereIndex");
                    if (!hasMoreThan1Record)
                        break;
                }
            }

            if (hasMoreThan1Record)
            {
                foreach (var primaryKeyField in LookupDefinition.TableDefinition.PrimaryKeyFields)
                {
                    if (!query.WhereItems.Any(a =>
                        a.WhereItemType == WhereItemTypes.General &&
                        a.Table.Name == primaryKeyField.TableDefinition.TableName &&
                        a.FieldName == primaryKeyField.FieldName))
                    {
                        lastWhereIndex++;
                        searchValue = startRow.GetRowValue(primaryKeyField.FieldName);
                        hasMoreThan1Record = HasMoreThan1Record(searchValue, primaryKeyField, query, query.BaseTable,
                            $"{debugMessage}.GetLastWhereIndex");
                        if (!hasMoreThan1Record)
                            break;
                    }
                }
            }

            return lastWhereIndex;
        }

        private bool GetPreviousRecords(DataRow startRow, int recordCount, DataTable resultsTable, string debugMessage)
        {
            var baseQuery = new SelectQuery(LookupDefinition.TableDefinition.TableName);
            SetupBaseQuery(baseQuery, true);

            var lastWhereIndex = GetLastWhereIndex(startRow, baseQuery, $"{debugMessage}.GetPreviousRecords");
            var checkNull = false;
            var recordsFound = 0;
            var originalRecordsCount = recordCount;
            while (lastWhereIndex >= 0 && recordCount > 0)
            {
                var lastWhereItem = baseQuery.WhereItems[baseQuery.WhereItems.Count - 1];
                if (lastWhereItem.Value.IsNullOrEmpty() && lastWhereIndex == 0 && recordsFound < originalRecordsCount
                    && OrderByType == OrderByTypes.Ascending)
                    break;

                if (checkNull)
                {
                    lastWhereItem.UpdateCondition(Conditions.EqualsNull);
                }
                else
                {
                    if (OrderByType == OrderByTypes.Ascending)
                    {
                        lastWhereItem.UpdateCondition(Conditions.LessThan);
                    }
                    else
                    {
                        lastWhereItem.UpdateCondition(Conditions.GreaterThan);
                    }
                }
                baseQuery.SetMaxRecords(recordCount);
                baseQuery.DebugMessage = $"LookupData.{debugMessage}.GetPreviousRecords";
                var result = LookupDefinition.TableDefinition.Context.DataProcessor.GetData(baseQuery, !_printMode);
                if (result.ResultCode == GetDataResultCodes.Success)
                {
                    foreach (DataRow dataRow in result.DataSet.Tables[0].Rows)
                    {
                        var newRow = resultsTable.NewRow();
                        newRow.ItemArray = dataRow.ItemArray;
                        resultsTable.Rows.InsertAt(newRow, 0);
                    }

                    recordsFound += result.DataSet.Tables[0].Rows.Count;
                    recordCount -= result.DataSet.Tables[0].Rows.Count;
                }
                if (lastWhereIndex == 0 && recordCount > 0 && !checkNull && OrderByType == OrderByTypes.Ascending)
                {
                    checkNull = true;
                }
                else
                {
                    baseQuery.RemoveWhereItem(lastWhereItem);
                    lastWhereIndex--;
                }
            }

            return true;
        }

        private bool HasMoreThan1Record(string searchValue, LookupColumnDefinitionBase lookupColumnType, SelectQuery query,
            QueryTable queryTable, string debugMessage)
        {
            switch (lookupColumnType.ColumnType)
            {
                case LookupColumnTypes.Field:
                    if (lookupColumnType is LookupFieldColumnDefinition lookupFieldColumn)
                    {
                        return HasMoreThan1Record(searchValue, lookupFieldColumn.FieldDefinition, query, queryTable,
                            debugMessage);
                    }
                    break;
                case LookupColumnTypes.Formula:
                    if (lookupColumnType is LookupFormulaColumnDefinition lookupFormulaColumn)
                    {
                        return HasMoreThan1Record(searchValue, lookupFormulaColumn.Formula, query,
                            lookupFormulaColumn.ValueType, debugMessage, lookupFormulaColumn.DateType);
                    }
                    break;
            }
            throw new ArgumentOutOfRangeException();
        }

        private bool HasMoreThan1Record(string searchValue, FieldDefinition fieldDefinition, SelectQuery query,
            QueryTable queryTable, string debugMessage)
        {
            query.SetMaxRecords(2);
            var dateType = DbDateTypes.DateOnly;
            if (fieldDefinition is DateFieldDefinition dateField)
                dateType = dateField.DateType;

            switch (fieldDefinition.FieldDataType)
            {
                case FieldDataTypes.Integer:
                    if (fieldDefinition is IntegerFieldDefinition integerFieldDefinition && integerFieldDefinition.EnumTranslation != null)
                    {
                        query.AddWhereItemEnum(queryTable, fieldDefinition.FieldName, Conditions.Equals, searchValue,
                            integerFieldDefinition.EnumTranslation);
                    }
                    else
                    {
                        query.AddWhereItem(queryTable, fieldDefinition.FieldName, Conditions.Equals,
                            searchValue, fieldDefinition.ValueType, dateType);
                    }
                    break;
                case FieldDataTypes.Bool:
                    if (fieldDefinition is BoolFieldDefinition boolFieldDefinition)
                    {
                        query.AddWhereItemBoolText(queryTable, fieldDefinition.FieldName, Conditions.Equals,
                            searchValue, boolFieldDefinition.TrueText, boolFieldDefinition.FalseText);
                    }
                    break;
                default:
                    query.AddWhereItem(queryTable, fieldDefinition.FieldName, Conditions.Equals,
                        searchValue, fieldDefinition.ValueType, dateType);
                    break;
            }

            query.DebugMessage = $"LookupData.{debugMessage}.HasMoreThan1Record?";
            var result = LookupDefinition.TableDefinition.Context.DataProcessor.GetData(query, !_printMode);
            if (result.ResultCode == GetDataResultCodes.Success && result.DataSet.Tables[0].Rows.Count > 1)
                return true;

            return false;
        }

        private bool HasMoreThan1Record(string searchValue, string formula, SelectQuery query, ValueTypes valueType,
            string debugMessage, DbDateTypes dateType)
        {
            query.SetMaxRecords(2);
            query.AddWhereItemFormula(formula, Conditions.Equals, searchValue, valueType, dateType);

            query.DebugMessage = $"LookupData.{debugMessage}.HasMoreThan1Record?";
            var result = LookupDefinition.TableDefinition.Context.DataProcessor.GetData(query, !_printMode);
            if (result.ResultCode == GetDataResultCodes.Success && result.DataSet.Tables[0].Rows.Count > 1)
                return true;

            return false;
        }

        private void SetScrollPosition(LookupScrollPositions currentPosition)
        {
            ScrollPosition = LookupScrollPositions.Disabled;
            if (LookupResultsDataTable == null)
                return;

            var prevCheckTable = LookupResultsDataTable.Clone();
            var nextCheckTable = LookupResultsDataTable.Clone();
            if (LookupResultsDataTable.Rows.Count >= LookupControl.PageSize && LookupControl.PageSize > 0)
            {
                switch (currentPosition)
                {
                    case LookupScrollPositions.Disabled:
                        break;
                    case LookupScrollPositions.Top:
                        GetNextRecords(
                            LookupResultsDataTable.Rows[LookupResultsDataTable.Rows.Count - 1], 1,
                            nextCheckTable, "SetScrollPosition");
                        break;
                    case LookupScrollPositions.Middle:
                        GetPreviousRecords(LookupResultsDataTable.Rows[0],
                            1, prevCheckTable, "SetScrollPosition");
                        GetNextRecords(
                            LookupResultsDataTable.Rows[LookupResultsDataTable.Rows.Count - 1], 1,
                            nextCheckTable, "SetScrollPosition");
                        break;
                    case LookupScrollPositions.Bottom:
                        if (!_printMode)
                        {
                            GetPreviousRecords(LookupResultsDataTable.Rows[0],
                                1, prevCheckTable, "SetScrollPosition");
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(currentPosition), currentPosition, null);
                }

                if (prevCheckTable.Rows.Count == 1 && nextCheckTable.Rows.Count == 1)
                    ScrollPosition = LookupScrollPositions.Middle;
                else if (nextCheckTable.Rows.Count == 0)
                {
                    ScrollPosition = LookupScrollPositions.Bottom;
                }
                else
                {
                    ScrollPosition = LookupScrollPositions.Top;
                }
            }
            else
            {
                if (_printMode)
                {
                    ScrollPosition = LookupScrollPositions.Bottom;
                }
            }
        }

        /// <summary>
        /// Called when the page size changes.
        /// </summary>
        public void OnChangePageSize()
        {
            if (LookupResultsDataTable == null || LookupResultsDataTable.Rows.Count <= 0)
                return;

            var newDataTable = LookupResultsDataTable.Clone();
            var previousTable = LookupResultsDataTable.Clone();
            if (GetPreviousRecords(LookupResultsDataTable.Rows[0], 1, previousTable, "OnChangePageSize"))
            {
                if (previousTable.Rows.Count == 0)
                {
                    GetInitData(false, false);
                    return;
                }
                if (GetNextRecords(previousTable.Rows[0], LookupControl.PageSize, newDataTable, "OnChangePageSize"))
                {
                    if (newDataTable.Rows.Count < LookupControl.PageSize)
                    {
                        var previousCount = LookupControl.PageSize - newDataTable.Rows.Count;
                        previousTable.Clear();
                        if (GetPreviousRecords(newDataTable.Rows[0], previousCount, previousTable, "OnChangePageSize"))
                        {
                            CopyRowsOnTop(previousTable, newDataTable);
                        }
                    }
                    LookupResultsDataTable = newDataTable;
                    OutputData(SelectedRowIndex, LookupScrollPositions.Middle);
                }
            }
        }

        private static void CopyRowsOnTop(DataTable sourceTable, DataTable destinationTable)
        {
            for (var index = sourceTable.Rows.Count - 1; index >= 0; index--)
            {
                var dataRow = sourceTable.Rows[index];
                var newRow = destinationTable.NewRow();
                newRow.ItemArray = dataRow.ItemArray;
                destinationTable.Rows.InsertAt(newRow, 0);
            }
        }

        /// <summary>
        /// Goto the first row in the database.
        /// </summary>
        public void GotoTop()
        {
            GetInitData(true, false, 0);
        }

        /// <summary>
        /// Goto the last row in the database.
        /// </summary>
        public void GotoBottom()
        {
            var query = new SelectQuery(LookupDefinition.TableDefinition.TableName).SetMaxRecords(LookupControl.PageSize);

            SetupBaseQuery(query, true);

            query.DebugMessage = "LookupData.GotoBottom";
            var getDataResult = LookupDefinition.TableDefinition.Context.DataProcessor.GetData(query, !_printMode);

            if (getDataResult.ResultCode == GetDataResultCodes.Success)
            {
                var newDataTable = getDataResult.DataSet.Tables[0];
                if (LookupResultsDataTable != null)
                {
                    newDataTable = LookupResultsDataTable.Clone();
                    foreach (DataRow dataRow in getDataResult.DataSet.Tables[0].Rows)
                    {
                        var newRow = newDataTable.NewRow();
                        newRow.ItemArray = dataRow.ItemArray;
                        newDataTable.Rows.InsertAt(newRow, 0);
                    }
                }

                LookupResultsDataTable = newDataTable;
                OutputData(LookupResultsDataTable.Rows.Count - 1, LookupScrollPositions.Bottom);
            }
        }

        /// <summary>
        /// Called when the search for value changes.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void OnSearchForChange(string searchText)
        {
            _searchForChanging = true;
            if (searchText.IsNullOrEmpty())
            {
                if (LookupResultsDataTable != null && LookupResultsDataTable.Rows.Count > 0)
                {
                    if (LookupControl.SearchType == LookupSearchTypes.Contains)
                        ResetRecordCount();
                    GotoTop();
                }

                _searchForChanging = false;
                return;
            }
            switch (LookupControl.SearchType)
            {
                case LookupSearchTypes.Equals:
                    GetSearchEqualsData(searchText);
                    break;
                case LookupSearchTypes.Contains:
                    GetInitData(true, true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _searchForChanging = false;
        }

        private void GetSearchEqualsData(string searchText)
        {
            var query = new SelectQuery(LookupDefinition.TableDefinition.TableName).SetMaxRecords(1);
            SetupBaseQuery(query, false);

            var condition = Conditions.GreaterThanEquals;
            switch (OrderByType)
            {
                case OrderByTypes.Ascending:
                    break;
                case OrderByTypes.Descending:
                    condition = Conditions.LessThanEquals;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (!AddSearchWhereToQuery(searchText, query, condition))
                return;

            query.DebugMessage = "LookupData.GetSearchEqualsData";
            var result = LookupDefinition.TableDefinition.Context.DataProcessor.GetData(query, !_printMode);
            if (result.ResultCode != GetDataResultCodes.Success)
                return;

            LookupResultsDataTable = result.DataSet.Tables[0];
            if (LookupResultsDataTable.Rows.Count == 0)
            {
                GotoBottom();
                return;
            }

            FillAroundFoundEqualsRow();
        }

        private void FillAroundFoundEqualsRow()
        {
            var middleRow = LookupResultsDataTable.Rows[0];

            var topRecordCount = Convert.ToInt32(Math.Floor((double) LookupControl.PageSize / 2));
            var bottomRecordCount = (LookupControl.PageSize - topRecordCount) - 1;
            var scrollPosition = LookupScrollPositions.Middle;

            if (!GetPreviousRecords(middleRow, topRecordCount, LookupResultsDataTable, "GetSearchEquals"))
                return;

            if (LookupResultsDataTable.Rows.Count < topRecordCount + 1)
                scrollPosition = LookupScrollPositions.Top;

            bottomRecordCount += ((topRecordCount - LookupResultsDataTable.Rows.Count) + 1);

            if (!GetNextRecords(middleRow, bottomRecordCount, LookupResultsDataTable, "GetSearchEquals"))
                return;

            if (LookupResultsDataTable.Rows.Count < LookupControl.PageSize)
            {
                scrollPosition = LookupScrollPositions.Bottom;
                if (!GetPreviousRecords(LookupResultsDataTable.Rows[0],
                    LookupControl.PageSize - LookupResultsDataTable.Rows.Count, LookupResultsDataTable, "GetSearchEquals"))
                    return;

                if (LookupResultsDataTable.Rows.Count < LookupControl.PageSize)
                    scrollPosition = LookupScrollPositions.Disabled;
            }

            OutputData(LookupResultsDataTable.Rows.IndexOf(middleRow), scrollPosition);
        }

        private bool AddSearchWhereToQuery(string searchText, SelectQuery query, Conditions condition)
        {
            switch (SortColumnDefinition.DataType)
            {
                case FieldDataTypes.String:
                    break;
                case FieldDataTypes.Integer:
                    if (SortColumnDefinition is LookupFieldColumnDefinition lookupFieldColumn
                        && lookupFieldColumn.FieldDefinition is IntegerFieldDefinition integerFieldDefinition
                        && integerFieldDefinition.EnumTranslation == null
                        && !int.TryParse(searchText, out _))
                        return false;
                    break;
                case FieldDataTypes.Decimal:
                    if (!double.TryParse(searchText, out _))
                        return false;
                    break;
                case FieldDataTypes.DateTime:
                    if (!DateTime.TryParse(searchText, out var date))
                        return false;
                    if (date.Year < 1000)
                        return false;
                    break;
                case FieldDataTypes.Bool:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var queryTable = GetQueryTableForColumn(query, SortColumnDefinition);

            switch (SortColumnDefinition.ColumnType)
            {
                case LookupColumnTypes.Field:
                    if (SortColumnDefinition is LookupFieldColumnDefinition lookupFieldColumn)
                    {
                        var processDefault = true;
                        switch (lookupFieldColumn.FieldDefinition.FieldDataType)
                        {
                            case FieldDataTypes.Integer:
                                if (lookupFieldColumn.FieldDefinition is IntegerFieldDefinition integerFieldDefinition
                                    && integerFieldDefinition.EnumTranslation != null)
                                {
                                    query.AddWhereItemEnum(queryTable, lookupFieldColumn.FieldDefinition.FieldName, condition,
                                        searchText, integerFieldDefinition.EnumTranslation);
                                    processDefault = false;
                                }

                                break;
                            case FieldDataTypes.Bool:
                                if (lookupFieldColumn.FieldDefinition is BoolFieldDefinition boolFieldDefinition)
                                {
                                    query.AddWhereItemBoolText(queryTable, lookupFieldColumn.FieldDefinition.FieldName,
                                        condition, searchText, boolFieldDefinition.TrueText,
                                        boolFieldDefinition.FalseText);
                                    processDefault = false;
                                }

                                break;
                        }

                        if (processDefault)
                        {
                            var dateType = DbDateTypes.DateOnly;
                            if (lookupFieldColumn.FieldDefinition.ValueType == ValueTypes.DateTime)
                            {
                                if (lookupFieldColumn.FieldDefinition is DateFieldDefinition dateField)
                                    dateType = dateField.DateType;
                            }

                            query.AddWhereItem(queryTable, lookupFieldColumn.FieldDefinition.FieldName, condition,
                                searchText, lookupFieldColumn.FieldDefinition.ValueType, dateType);
                        }
                    }

                    break;
                case LookupColumnTypes.Formula:
                    if (SortColumnDefinition is LookupFormulaColumnDefinition lookupFormulaColumn)
                    {
                        switch (lookupFormulaColumn.DataType)
                        {
                            case FieldDataTypes.DateTime:
                                if (DateTime.TryParse(searchText, out var date))
                                {
                                    query.AddWhereItemFormula(lookupFormulaColumn.Formula, condition, date,
                                        lookupFormulaColumn.DateType);
                                }
                                break;
                            default:
                                query.AddWhereItemFormula(lookupFormulaColumn.Formula, condition, searchText,
                                    lookupFormulaColumn.ValueType);
                                break;
                        }
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return true;
        }

        /// <summary>
        /// Views the selected row.
        /// </summary>
        /// <param name="selectedIndex">Index of the selected row.</param>
        /// <param name="ownerWindow">The owner window.</param>
        /// <param name="inputParameter">The input parameter.</param>
        public void ViewSelectedRow(int selectedIndex, object ownerWindow, object inputParameter = null, bool lookupReadOnlyMode = false)
        {
            if (selectedIndex >= 0 && selectedIndex < LookupResultsDataTable.Rows.Count)
            {
                _selectingRecord = true;
                var args = new LookupAddViewArgs(this, true, LookupFormModes.View, string.Empty, ownerWindow)
                {
                    ParentWindowPrimaryKeyValue = ParentWindowPrimaryKeyValue,
                    InputParameter = inputParameter,
                    LookupReadOnlyMode = lookupReadOnlyMode 
                };
                OnLookupView(args);
                if (!args.Handled)
                {
                    _selectingRecord = false;
                    args.CallBackToken.RefreshData += LookupCallBack_RefreshData;
                    LookupDefinition.TableDefinition.Context.OnAddViewLookup(args);
                }
            }
        }

        /// <summary>
        /// Adds a new row.
        /// </summary>
        /// <param name="ownerWindow">The owner window.</param>
        /// <param name="inputParameter">The input parameter.</param>
        public void AddNewRow(object ownerWindow, object inputParameter = null)
        {
            var args = new LookupAddViewArgs(this, true, LookupFormModes.Add, string.Empty, ownerWindow)
            {
                ParentWindowPrimaryKeyValue = ParentWindowPrimaryKeyValue,
                InputParameter = inputParameter
            };
            args.CallBackToken.RefreshData += LookupCallBack_RefreshData;
            LookupDefinition.TableDefinition.Context.OnAddViewLookup(args);
        }

        private void LookupCallBack_RefreshData(object sender, EventArgs e)
        {
            RefreshData();
            DataSourceChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Refreshes the data.
        /// </summary>
        public void RefreshData()
        {
            if (!_selectingRecord)
            {
                if (LookupControl.SearchText.IsNullOrEmpty())
                    GetInitData(true, true);
                else
                    OnSearchForChange(LookupControl.SearchText);
            }
            _selectingRecord = false;
        }

        /// <summary>
        /// Occurs when a user wishes to view a selected lookup row.  Fires the LookupView event.
        /// </summary>
        /// <param name="e">The lookup primary key row arguments.</param>
        protected virtual void OnLookupView(LookupAddViewArgs e)
        {
            LookupView?.Invoke(this, e);
        }

        /// <summary>
        /// Gets the selected row.
        /// </summary>
        /// <returns>The selected row.  Null if no row is selected.</returns>
        public DataRow GetSelectedRow()
        {
            if (LookupResultsDataTable != null && SelectedRowIndex >= 0 &&
                SelectedRowIndex < LookupResultsDataTable.Rows.Count)
            {
                return LookupResultsDataTable.Rows[SelectedRowIndex];
            }

            return null;
        }

        public int GetRecordCountWait()
        {
            var countQuery = SetupCountQuery();
            DataProcessResult result = null;
            result = LookupDefinition.TableDefinition.Context.DataProcessor.GetData(countQuery, false);
            if (PostRecordCount(result))
            {
                return RecordCount;
            }

            return 0;
        }
        /// <summary>
        /// Gets the record count.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> GetRecordCount()
        {
            var countQuery = SetupCountQuery();
            DataProcessResult result = null;
            await Task.Run(() =>
            {
                result = LookupDefinition.TableDefinition.Context.DataProcessor.GetData(countQuery, false);
            });

            return PostRecordCount(result);
        }

        private bool PostRecordCount(DataProcessResult result)
        {
            _countingRecords = false;

            if (result.ResultCode == GetDataResultCodes.Success)
            {
                RecordCount = result.DataSet.Tables[0].Rows[0].GetRowValue("Count").ToInt();
                return true;
            }

            return false;
        }

        private CountQuery SetupCountQuery()
        {
            _countingRecords = true;
            var selectQuery = new SelectQuery(LookupDefinition.TableDefinition.TableName);
            SetupBaseQuery(selectQuery, false);

            AddSortColumnToQueryWhere(selectQuery);

            var countQuery = new CountQuery(selectQuery, "Count");
            return countQuery;
        }

        private List<TableFieldJoinDefinition> AddCountJoinDefinitions()
        {
            List<TableFieldJoinDefinition> joins = null;
            if (LookupDefinition.Joins.Any())
            {
                joins = new List<TableFieldJoinDefinition>();
                foreach (var join in LookupDefinition.Joins)
                {
                    AddCountParentJoin(joins, @join);
                }
            }

            if (LookupDefinition.FilterDefinition.Joins.Any())
            {
                if (joins == null)
                    joins = new List<TableFieldJoinDefinition>();
                foreach (var join in LookupDefinition.FilterDefinition.Joins)
                {
                    AddCountParentJoin(joins, join);
                }
            }

            return joins;
        }

        private void AddCountParentJoin(List<TableFieldJoinDefinition> joins, TableFieldJoinDefinition join)
        {
            if (!join.ParentAlias.IsNullOrEmpty())
            {
                var parentJoin = LookupDefinition.Joins.FirstOrDefault(j =>
                    j.Alias == join.ParentAlias);
                if (parentJoin != null)
                    AddCountParentJoin(joins, parentJoin);
            }

            joins.Add(join);
        }

        /// <summary>
        /// Resets the record count.
        /// </summary>
        public void ResetRecordCount()
        {
            RecordCount = 0;
        }

        /// <summary>
        /// Gets the primary key value of the record whose sort column equals the search text.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <returns>The resulting primary key value.</returns>
        public PrimaryKeyValue GetPrimaryKeyValueForSearchText(string searchText)
        {
            var result = new PrimaryKeyValue(LookupDefinition.TableDefinition);

            var query = new SelectQuery(LookupDefinition.TableDefinition.TableName).SetMaxRecords(1);

            SetupBaseQuery(query, false);
            if (!AddSearchWhereToQuery(searchText, query, Conditions.Equals))
                return result;

            query.DebugMessage = "LookupData.SelectPrimaryKey";
            var getDataResult = LookupDefinition.TableDefinition.Context.DataProcessor.GetData(query, !_printMode);

            if (getDataResult.ResultCode == GetDataResultCodes.Success)
            {
                var resultRows = getDataResult.DataSet.Tables[0].Rows;
                if (resultRows.Count > 0)
                {
                    result.PopulateFromDataRow(resultRows[0]);
                }
            }
            return result;
        }

        public void ClearLookupData()
        {
            LookupResultsDataTable = new DataTable();
            
            OutputData(0, LookupScrollPositions.Disabled);
        }

        public void GetPrintData()
        {
            _printMode = true;
            LookupDataChanged += (sender, args) =>
            {
                PrintDataChanged?.Invoke(this, args);
                if (args.OutputTable.Rows.Count <  LookupControl.PageSize 
                    || args.ScrollPosition == LookupScrollPositions.Bottom 
                    || args.Abort)
                {
                    _printMode = false;
                }
                else
                {
                    GotoNextPage();
                }
            };
            GetInitData();
        }
    }
}
