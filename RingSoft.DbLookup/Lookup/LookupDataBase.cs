using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using RingSoft.DbLookupCore.GetDataProcessor;
using RingSoft.DbLookupCore.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookupCore.QueryBuilder;
using RingSoft.DbLookupCore.TableProcessing;

namespace RingSoft.DbLookupCore.Lookup
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
        public LookupDefinitionBase LookupDefinition { get; }

        public ILookupUserInterface UserInterface { get; }

        /// <summary>
        /// Gets the sort column definition.
        /// </summary>
        /// <value>
        /// The sort column definition.
        /// </value>
        public LookupColumnBase SortColumnDefinition { get; private set; }

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
                if (SelectedIndexChanged != null)
                    SelectedIndexChanged(this, new SelectedIndexChangedEventArgs(value));
            }
        }

        /// <summary>
        /// Gets the order by list.
        /// </summary>
        /// <value>
        /// The order by list.
        /// </value>
        public IReadOnlyList<LookupColumnBase> OrderByList => _orderByList;

        /// <summary>
        /// Gets the primary key value.
        /// </summary>
        /// <value>
        /// The primary key value.
        /// </value>
        public PrimaryKeyValue PrimaryKeyValue
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

        private readonly List<LookupColumnBase> _orderByList = new List<LookupColumnBase>();
        private int _selectedIndex;
        private bool _selectingRecord;

        private void OutputData(int selectedRowIndex, LookupScrollPositions currentPosition)
        {
            SetScrollPosition(currentPosition);
            if (UserInterface.PageSize == 1)
                SelectedRowIndex = LookupResultsDataTable.Rows.Count - 1;
            else
            {
                SelectedRowIndex = selectedRowIndex;
            }

            if (ScrollPosition == LookupScrollPositions.Disabled)
                RecordCount = LookupResultsDataTable.Rows.Count;

            ProcessLookupData();

            LookupDataChanged?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void ProcessLookupData()
        {

        }

        /// <summary>
        /// Occurs when this object's data has changed.
        /// </summary>
        public event EventHandler LookupDataChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupDataBase"/> class.
        /// </summary>
        /// <param name="lookupDefinition">The lookup definition.</param>
        /// <param name="userInterface">The user interface.</param>
        public LookupDataBase(LookupDefinitionBase lookupDefinition, ILookupUserInterface userInterface)
        {
            LookupDefinition = lookupDefinition;
            UserInterface = userInterface;
            SortColumnDefinition = lookupDefinition.InitialSortColumnDefinition;
            OrderByType = lookupDefinition.InitialOrderByType;
        }

        /// <summary>
        /// Gets the first top page of data.
        /// </summary>
        /// <param name="resetSelectedRowIndex">if set to <c>true</c> then reset the selected row index after fetching data.</param>
        /// <param name="resetRecordCount">if set to <c>true</c> then reset record count after fetching data.</param>
        /// <returns></returns>
        public GetDataResult GetInitData(bool resetSelectedRowIndex, bool resetRecordCount = true)
        {
            if (resetRecordCount)
                RecordCount = 0;

            var query = new SelectQuery(LookupDefinition.TableDefinition.TableName).SetMaxRecords(UserInterface.PageSize);

            SetupBaseQuery(query, false);

            query.DebugMessage = "LookupData.GetInitData";
            var getDataResult = LookupDefinition.TableDefinition.Context.DataProcessor.GetData(query);

            if (getDataResult.ResultCode == GetDataResultCodes.Success)
            {
                LookupResultsDataTable = getDataResult.DataSet.Tables[0];
                var selectedIndex = -1;
                if (!resetSelectedRowIndex && LookupResultsDataTable.Rows.Count > 0)
                    selectedIndex = SelectedRowIndex;
                OutputData(selectedIndex, LookupScrollPositions.Top);
            }

            return getDataResult;
        }

        /// <summary>
        /// Selects the primary key.
        /// </summary>
        /// <param name="primaryKey">The primary key.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Primary key value Table Definition '{PrimaryKeyValue.TableDefinition.TableName}' does not match this Lookup Definition's Table Definition '{LookupDefinition.TableDefinition.TableName}'</exception>
        public GetDataResult SelectPrimaryKey(PrimaryKeyValue primaryKey)
        {
            if (primaryKey.TableDefinition != LookupDefinition.TableDefinition)
                throw new ArgumentException(
                    $"Primary key value Table Definition '{PrimaryKeyValue.TableDefinition.TableName}' does not match this Lookup Definition's Table Definition '{LookupDefinition.TableDefinition.TableName}'");

            var query = new SelectQuery(LookupDefinition.TableDefinition.TableName).SetMaxRecords(UserInterface.PageSize);

            SetupBaseQuery(query, false);

            foreach (var keyValueField in primaryKey.KeyValueFields)
            {
                query.AddWhereItem(keyValueField.FieldDefinition.FieldName, Conditions.Equals, keyValueField.Value);
            }

            query.DebugMessage = "LookupData.SelectPrimaryKey";
            var getDataResult = LookupDefinition.TableDefinition.Context.DataProcessor.GetData(query);

            if (getDataResult.ResultCode == GetDataResultCodes.Success)
            {
                var dataTable = getDataResult.DataSet.Tables[0];
                LookupResultsDataTable = dataTable;
                var selectedIndex = -1;
                if (LookupResultsDataTable.Rows.Count > 0)
                    selectedIndex = 0;
                OutputData(selectedIndex, LookupScrollPositions.Middle);
            }

            return getDataResult;
        }

        private void SetupBaseQuery(SelectQuery query, bool reverseOrderBy, bool addSortColumnToQueryWhere = true)
        {
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
            if (UserInterface.SearchType == LookupSearchTypes.Contains &&
                SortColumnDefinition.DataType == FieldDataTypes.String)
            {
                AddSortColumnToQueryWhere(query, Conditions.Contains, UserInterface.SearchText);
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
                        query.AddWhereItemFormula(sortFormulaColumnDefinition.Formula, condition, searchText);
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void AddColumnToQuery(SelectQuery query, LookupColumnBase lookupDefinitionColumn, bool hiddenColumn)
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
                            case FieldDataTypes.Enum:
                                if (hiddenColumn)
                                {
                                    query.AddSelectColumn(lookupFieldColumn.FieldDefinition.FieldName, queryTable,
                                        sqlFieldName, lookupFieldColumn.Distinct);
                                }
                                else
                                {
                                    if (lookupFieldColumn.FieldDefinition is EnumFieldDefinition enumField)
                                        query.AddSelectEnumColumn(queryTable, enumField.FieldName, sqlFieldName,
                                            enumField.EnumTranslation);
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

            AddColumnToQueryOrderBy(SortColumnDefinition, query, orderBy);

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

        private void AddColumnToQueryOrderBy(LookupColumnBase columnDefinition, SelectQuery query, OrderByTypes orderBy)
        {
            switch (columnDefinition.ColumnType)
            {
                case LookupColumnTypes.Field:
                    if (columnDefinition is LookupFieldColumnDefinition fieldColumnDefinition)
                    {
                        switch (fieldColumnDefinition.FieldDefinition.FieldDataType)
                        {
                            case FieldDataTypes.Enum:
                                if (fieldColumnDefinition.FieldDefinition is EnumFieldDefinition enumFieldDefinition)
                                {
                                    query.AddOrderByEnumSegment(GetQueryTableForColumn(query, fieldColumnDefinition),
                                        fieldColumnDefinition.FieldDefinition.FieldName, orderBy,
                                        enumFieldDefinition.EnumTranslation, false);
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

        private QueryTable GetQueryTableForColumn(SelectQuery query, LookupColumnBase lookupColumnDefinition)
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

            GetInitData(true, false);
        }

        /// <summary>
        /// Goto the next record.
        /// </summary>
        public void GotoNextRecord()
        {
            if (LookupResultsDataTable == null || LookupResultsDataTable.Rows.Count <= 0)
                return;

            var newDataTable = LookupResultsDataTable.Clone();
            if (GetNextRecords(LookupResultsDataTable.Rows[0], UserInterface.PageSize, newDataTable, "GotoNextRecord"))
            {
                if (newDataTable.Rows.Count >= UserInterface.PageSize)
                {
                    LookupResultsDataTable = newDataTable;
                    OutputData(LookupResultsDataTable.Rows.Count - 1, LookupScrollPositions.Middle);
                }
            }
        }

        /// <summary>
        /// Goto the next page.
        /// </summary>
        public void GotoNextPage()
        {
            if (LookupResultsDataTable == null || LookupResultsDataTable.Rows.Count <= 0)
                return;

            var newDataTable = LookupResultsDataTable.Clone();
            if (GetNextRecords(LookupResultsDataTable.Rows[LookupResultsDataTable.Rows.Count - 1],
                UserInterface.PageSize, newDataTable, "GotoNextPage"))
            {
                if (newDataTable.Rows.Count > 0)
                {
                    if (newDataTable.Rows.Count < UserInterface.PageSize)
                    {
                        var index = newDataTable.Rows.Count - 1;
                        newDataTable = LookupResultsDataTable.Clone();
                        GetNextRecords(LookupResultsDataTable.Rows[index],
                            UserInterface.PageSize, newDataTable, "OnPageDown");
                    }

                    LookupResultsDataTable = newDataTable;
                    OutputData(LookupResultsDataTable.Rows.Count - 1, LookupScrollPositions.Middle);
                }
            }
        }

        /// <summary>
        /// Goto the previous record.
        /// </summary>
        public void GotoPreviousRecord()
        {
            if (LookupResultsDataTable == null || LookupResultsDataTable.Rows.Count <= 0)
                return;

            var newDataTable = LookupResultsDataTable.Clone();
            if (GetPreviousRecords(LookupResultsDataTable.Rows[LookupResultsDataTable.Rows.Count - 1],
                UserInterface.PageSize, newDataTable, "GotoPreviousRecord"))
            {
                if (newDataTable.Rows.Count >= UserInterface.PageSize)
                {
                    LookupResultsDataTable = newDataTable;
                    OutputData(0, LookupScrollPositions.Middle);
                }
            }
        }

        /// <summary>
        /// Goto the previous page.
        /// </summary>
        public void GotoPreviousPage()
        {
            if (LookupResultsDataTable == null || LookupResultsDataTable.Rows.Count <= 0)
                return;

            var newDataTable = LookupResultsDataTable.Clone();
            if (GetPreviousRecords(LookupResultsDataTable.Rows[0], UserInterface.PageSize, newDataTable, "GotoPreviousPage"))
            {
                if (newDataTable.Rows.Count > 0)
                {
                    if (newDataTable.Rows.Count < UserInterface.PageSize)
                    {
                        var index = newDataTable.Rows.Count;
                        newDataTable = LookupResultsDataTable.Clone();
                        GetPreviousRecords(LookupResultsDataTable.Rows[LookupResultsDataTable.Rows.Count - index],
                            UserInterface.PageSize, newDataTable, "OnPageUp");
                    }
                    LookupResultsDataTable = newDataTable;
                    OutputData(0, LookupScrollPositions.Middle);
                }
            }
        }

        private bool GetNextRecords(DataRow startRow, int recordCount, DataTable resultsTable, string debugMessage)
        {
            var query = new SelectQuery(LookupDefinition.TableDefinition.TableName);
            SetupBaseQuery(query, false);

            var lastWhereIndex = GetLastWhereIndex(startRow, query, $"{debugMessage}.GetNextRecords");
            var checkNull = false;
            var recordsFound = 0;
            var originalRecordsCount = recordCount;
            while (lastWhereIndex >= 0 && recordCount > 0)
            {
                var lastWhereItem = query.WhereItems[query.WhereItems.Count - 1];
                if (lastWhereItem.Value.IsNullOrEmpty() && lastWhereIndex == 0 && recordsFound < originalRecordsCount
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
                var result = LookupDefinition.TableDefinition.Context.DataProcessor.GetData(query);
                if (result.ResultCode == GetDataResultCodes.Success)
                {
                    foreach (DataRow dataRow in result.DataSet.Tables[0].Rows)
                    {
                        resultsTable.ImportRow(dataRow);
                    }
                    recordsFound += result.DataSet.Tables[0].Rows.Count;
                    recordCount -= result.DataSet.Tables[0].Rows.Count;
                }
                if (lastWhereIndex == 0 && recordCount > 0 && !checkNull && OrderByType == OrderByTypes.Descending)
                {
                    checkNull = true;
                }
                else
                {
                    query.RemoveWhereItem(lastWhereItem);
                    lastWhereIndex--;
                }
            }

            return true;
        }

        private int GetLastWhereIndex(DataRow startRow, SelectQuery query, string debugMessage)
        {
            var lastWhereIndex = 0;

            var searchValue = startRow.GetRowValue(SortColumnDefinition.SelectSqlAlias);
            var hasMoreThan1Record = HasMoreThan1Record(searchValue, SortColumnDefinition, query,
                GetQueryTableForColumn(query, SortColumnDefinition), $"{debugMessage}.GetLastWhereIndex");

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
                var result = LookupDefinition.TableDefinition.Context.DataProcessor.GetData(baseQuery);
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

        private bool HasMoreThan1Record(string searchValue, LookupColumnBase lookupColumnType, SelectQuery query,
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
                        return HasMoreThan1Record(searchValue, lookupFormulaColumn.Formula, query, debugMessage);
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
                case FieldDataTypes.Enum:
                    if (fieldDefinition is EnumFieldDefinition enumFieldDefinition)
                    {
                        query.AddWhereItemEnum(queryTable, fieldDefinition.FieldName, Conditions.Equals, searchValue,
                            enumFieldDefinition.EnumTranslation);
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
            var result = LookupDefinition.TableDefinition.Context.DataProcessor.GetData(query);
            if (result.ResultCode == GetDataResultCodes.Success && result.DataSet.Tables[0].Rows.Count > 1)
                return true;

            return false;
        }

        private bool HasMoreThan1Record(string searchValue, string formula, SelectQuery query, string debugMessage)
        {
            query.SetMaxRecords(2);
            query.AddWhereItemFormula(formula, Conditions.Equals, searchValue);

            query.DebugMessage = $"LookupData.{debugMessage}.HasMoreThan1Record?";
            var result = LookupDefinition.TableDefinition.Context.DataProcessor.GetData(query);
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
            if (LookupResultsDataTable.Rows.Count >= UserInterface.PageSize && UserInterface.PageSize > 0)
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
                        GetPreviousRecords(LookupResultsDataTable.Rows[0],
                            1, prevCheckTable, "SetScrollPosition");
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
                if (GetNextRecords(previousTable.Rows[0], UserInterface.PageSize, newDataTable, "OnChangePageSize"))
                {
                    if (newDataTable.Rows.Count < UserInterface.PageSize)
                    {
                        var previousCount = UserInterface.PageSize - newDataTable.Rows.Count;
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
            GetInitData(true, false);
        }

        /// <summary>
        /// Goto the last row in the database.
        /// </summary>
        public void GotoBottom()
        {
            var query = new SelectQuery(LookupDefinition.TableDefinition.TableName).SetMaxRecords(UserInterface.PageSize);

            SetupBaseQuery(query, true);

            query.DebugMessage = "LookupData.GotoBottom";
            var getDataResult = LookupDefinition.TableDefinition.Context.DataProcessor.GetData(query);

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
            if (searchText.IsNullOrEmpty())
            {
                if (LookupResultsDataTable != null && LookupResultsDataTable.Rows.Count > 0)
                    GotoTop();
                return;
            }
            switch (UserInterface.SearchType)
            {
                case LookupSearchTypes.Equals:
                    GetSearchEqualsData(searchText);
                    break;
                case LookupSearchTypes.Contains:
                    GetInitData(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
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
            var result = LookupDefinition.TableDefinition.Context.DataProcessor.GetData(query);
            if (result.ResultCode != GetDataResultCodes.Success)
                return;

            LookupResultsDataTable = result.DataSet.Tables[0];
            if (LookupResultsDataTable.Rows.Count == 0)
            {
                GotoBottom();
                return;
            }

            var topRecordCount = Convert.ToInt32(Math.Floor((double)UserInterface.PageSize / 2));
            var bottomRecordCount = (UserInterface.PageSize - topRecordCount) - 1;
            var middleRow = LookupResultsDataTable.Rows[0];
            var scrollPosition = LookupScrollPositions.Middle;
            
            if (!GetPreviousRecords(middleRow, topRecordCount, LookupResultsDataTable, "GetSearchEquals"))
                return;

            if (LookupResultsDataTable.Rows.Count < topRecordCount + 1)
                scrollPosition = LookupScrollPositions.Top;

            bottomRecordCount += ((topRecordCount - LookupResultsDataTable.Rows.Count) + 1);

            if (!GetNextRecords(middleRow, bottomRecordCount, LookupResultsDataTable, "GetSearchEquals"))
                return;

            if (LookupResultsDataTable.Rows.Count < UserInterface.PageSize)
            {
                scrollPosition = LookupScrollPositions.Bottom;
                if (!GetPreviousRecords(LookupResultsDataTable.Rows[0],
                    UserInterface.PageSize - LookupResultsDataTable.Rows.Count, LookupResultsDataTable, "GetSearchEquals"))
                    return;

                if (LookupResultsDataTable.Rows.Count < UserInterface.PageSize)
                    scrollPosition = LookupScrollPositions.Disabled;
            }

            OutputData(LookupResultsDataTable.Rows.IndexOf(middleRow), scrollPosition);
        }

        private bool AddSearchWhereToQuery(string searchText, SelectQuery query, Conditions condition)
        {
            switch (SortColumnDefinition.DataType)
            {
                case FieldDataTypes.String:
                case FieldDataTypes.Enum:
                    break;
                case FieldDataTypes.Integer:
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
                        switch (lookupFieldColumn.FieldDefinition.FieldDataType)
                        {
                            case FieldDataTypes.Enum:
                                if (lookupFieldColumn.FieldDefinition is EnumFieldDefinition enumFieldDefinition)
                                {
                                    query.AddWhereItemEnum(queryTable, lookupFieldColumn.FieldDefinition.FieldName, condition,
                                        searchText, enumFieldDefinition.EnumTranslation);
                                }

                                break;
                            case FieldDataTypes.Bool:
                                if (lookupFieldColumn.FieldDefinition is BoolFieldDefinition boolFieldDefinition)
                                {
                                    query.AddWhereItemBoolText(queryTable, lookupFieldColumn.FieldDefinition.FieldName,
                                        condition, searchText, boolFieldDefinition.TrueText,
                                        boolFieldDefinition.FalseText);
                                }

                                break;
                            default:
                                var dateType = DbDateTypes.DateOnly;
                                if (lookupFieldColumn.FieldDefinition.ValueType == ValueTypes.DateTime)
                                {
                                    if (lookupFieldColumn.FieldDefinition is DateFieldDefinition dateField)
                                        dateType = dateField.DateType;
                                }

                                query.AddWhereItem(queryTable, lookupFieldColumn.FieldDefinition.FieldName, condition,
                                    searchText, lookupFieldColumn.FieldDefinition.ValueType, dateType);
                                break;
                        }
                    }

                    break;
                case LookupColumnTypes.Formula:
                    if (SortColumnDefinition is LookupFormulaColumnDefinition lookupFormulaColumn)
                    {
                        query.AddWhereItemFormula(lookupFormulaColumn.Formula, condition, searchText);
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
        public void ViewSelectedRow(int selectedIndex)
        {
            if (selectedIndex >= 0 && selectedIndex < LookupResultsDataTable.Rows.Count)
            {
                _selectingRecord = true;
                var args = new LookupAddViewArgs(this, true, LookupFormModes.View, string.Empty)
                {
                    ParentWindowPrimaryKeyValue = ParentWindowPrimaryKeyValue
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
        public void AddNewRow()
        {
            var args = new LookupAddViewArgs(this, true, LookupFormModes.Add, string.Empty)
            {
                ParentWindowPrimaryKeyValue = ParentWindowPrimaryKeyValue
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
                if (UserInterface.SearchText.IsNullOrEmpty())
                    GetInitData(true);
                else
                    OnSearchForChange(UserInterface.SearchText);
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
        /// <returns></returns>
        public DataRow GetSelectedRow()
        {
            if (LookupResultsDataTable != null && SelectedRowIndex >= 0 &&
                SelectedRowIndex < LookupResultsDataTable.Rows.Count)
            {
                return LookupResultsDataTable.Rows[SelectedRowIndex];
            }

            return null;
        }

        /// <summary>
        /// Gets the record count.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> GetRecordCount()
        {
            var selectQuery = new SelectQuery(LookupDefinition.TableDefinition.TableName);

            if (UserInterface.SearchType == LookupSearchTypes.Contains &&
                SortColumnDefinition.DataType == FieldDataTypes.String &&
                SortColumnDefinition.ColumnType == LookupColumnTypes.Field)
            {
                if (SortColumnDefinition is LookupFieldColumnDefinition lookupFieldColumn)
                {
                    if (!lookupFieldColumn.JoinQueryTableAlias.IsNullOrEmpty())
                    {
                        List<TableFieldJoinDefinition> joins = new List<TableFieldJoinDefinition>();
                        var join = LookupDefinition.Joins.FirstOrDefault(j =>
                            j.Alias == lookupFieldColumn.JoinQueryTableAlias);
                        if (join != null)
                        {
                            AddCountParentJoin(joins, join);
                            TableFilterDefinitionBase.ProcessFieldJoins(selectQuery, joins);
                        }
                    }
                }
            }

            foreach (var tableDefinitionPrimaryKeyField in LookupDefinition.TableDefinition.PrimaryKeyFields)
            {
                selectQuery.AddSelectColumn(tableDefinitionPrimaryKeyField.FieldName);
            }

            LookupDefinition.FilterDefinition.ProcessQuery(selectQuery);

            AddSortColumnToQueryWhere(selectQuery);

            var countQuery = new CountQuery(selectQuery, "Count");
            GetDataResult result = null;
            await Task.Run(() =>
            {
                result = LookupDefinition.TableDefinition.Context.DataProcessor.GetData(countQuery);
            });
            if (result.ResultCode == GetDataResultCodes.Success)
            {
                RecordCount = result.DataSet.Tables[0].Rows[0].GetRowValue("Count").ToInt();
                return true;
            }

            return false;
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
            var getDataResult = LookupDefinition.TableDefinition.Context.DataProcessor.GetData(query);

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
    }
}
