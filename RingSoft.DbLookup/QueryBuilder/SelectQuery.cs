using System;
using System.Collections.Generic;
using System.Globalization;
using RingSoft.DataEntryControls.Engine;

namespace RingSoft.DbLookup.QueryBuilder
{
    /// <summary>This class is used to build a SELECT SQL statement.  It is not comprehensive.</summary>
    public class SelectQuery : QueryBase
    {
        /// <summary>
        /// Gets the type of the query.
        /// </summary>
        /// <value>
        /// The type of the query.
        /// </value>
        public override QueryTypes QueryType => QueryTypes.SelectQuery;

        /// <summary>
        /// Gets the nested query.
        /// </summary>
        /// <value>
        /// The nested query.
        /// </value>
        public SelectQuery NestedQuery { get; internal set; }

        /// <summary>
        /// Gets the maximum number of records to retrieve.
        /// </summary>
        /// <value>
        /// The maximum records.
        /// </value>
        public int MaxRecords { get; set; }

        /// <summary>Gets the base table object attached to this query.</summary>
        /// <value>The base table. that is appears after the FROM keyword in a SQL statement.</value>
        public QueryTable BaseTable { get; private set; }

        /// <summary>Gets the SELECT columns.</summary>
        /// <value>The columns in the SELECT statement.</value>
        public IReadOnlyList<SelectColumn> Columns => _columns;

        /// <summary>
        /// Gets the join tables.
        /// </summary>
        /// <value>
        /// The join tables.
        /// </value>
        public IReadOnlyList<PrimaryJoinTable> JoinTables => _joinTables;

        /// <summary>
        /// Gets the where items.
        /// </summary>
        /// <value>
        /// The where items.
        /// </value>
        public IReadOnlyList<WhereItem> WhereItems => _whereItems;

        /// <summary>
        /// Gets the order by segments.
        /// </summary>
        /// <value>
        /// The order by segments.
        /// </value>
        public IReadOnlyList<OrderBySegment> OrderBySegments => _orderBySegments;

        private readonly List<SelectColumn> _columns = new List<SelectColumn>();
        private readonly List<PrimaryJoinTable> _joinTables = new List<PrimaryJoinTable>();
        private readonly List<WhereItem> _whereItems = new List<WhereItem>();
        private readonly List<OrderBySegment> _orderBySegments = new List<OrderBySegment>();

        /// <summary>Initializes a new instance of the <see cref="SelectQuery"/> class.</summary>
        /// <param name="baseTableName">Name of the table for the FROM clause.</param>
        public SelectQuery(string baseTableName)
        {
            BaseTable = new QueryTable
            {
                Query = this,
                Name = baseTableName,
                Alias = baseTableName
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectQuery"/> class.
        /// </summary>
        /// <param name="queryName">The query name.</param>
        /// <param name="nestedQuery">The nested query.</param>
        public SelectQuery(string queryName, SelectQuery nestedQuery)
        {
            BaseTable = new QueryTable
            {
                Query = this,
                Name = queryName,
                Alias = queryName
            };
            NestedQuery = nestedQuery;
        }

        /// <summary>
        /// Sets the maximum number of records to retrieve.
        /// </summary>
        /// <param name="maxRecords">The record count.</param>
        /// <returns></returns>
        public SelectQuery SetMaxRecords(int maxRecords)
        {
            MaxRecords = maxRecords;
            return this;
        }

        /// <summary>
        /// Adds a column to the columns list.
        /// </summary>
        /// <param name="name">The name of the column.</param>
        /// <param name="isDistinct">Is the column distinct?</param>
        /// <returns>
        /// This object.
        /// </returns>
        public SelectQuery AddSelectColumn(string name, bool isDistinct = false)
        {
            return AddSelectColumn(name, string.Empty, isDistinct);
        }

        /// <summary>
        /// Adds a column to the columns list.
        /// </summary>
        /// <param name="name">The name of the column.</param>
        /// <param name="alias">The value appears after the "AS" keyword.</param>
        /// <param name="isDistinct">Is the column distinct?</param>
        /// <returns></returns>
        //// <returns>This object.</returns>
        public SelectQuery AddSelectColumn(string name, string alias, bool isDistinct = false)
        {
            return AddSelectColumn(name, BaseTable, alias, isDistinct);
        }

        /// <summary>
        /// Adds a column to the columns list.
        /// </summary>
        /// <param name="name">The name of the column.</param>
        /// <param name="queryTable">The query table object attached to this column.</param>
        /// <param name="isDistinct">Is the column distinct?</param>
        /// <returns>
        /// This object.
        /// </returns>
        public SelectQuery AddSelectColumn(string name, QueryTable queryTable, bool isDistinct = false)
        {
            return AddSelectColumn(name, queryTable, string.Empty, isDistinct);
        }

        /// <summary>
        /// Adds a column to the columns list.
        /// </summary>
        /// <param name="name">The name of the column.</param>
        /// <param name="queryTable">The query table object attached to this column.</param>
        /// <param name="alias">The value appears after the "AS" keyword.</param>
        /// <param name="isDistinct">Is the column distinct?</param>
        /// <returns>
        /// This object.
        /// </returns>
        /// <exception cref="ArgumentException"></exception>
        public SelectQuery AddSelectColumn(string name, QueryTable queryTable, string alias, bool isDistinct = false)
        {
            if (queryTable.Query != this)
            {
                throw new ArgumentException(GetQueryTableMismatchMessage(queryTable));
            }
            var selectColumn = new SelectColumn
            {
                Alias = alias,
                ColumnName = name,
                Table = queryTable,
                IsDistinct = isDistinct
            };

            _columns.Add(selectColumn);

            return this;
        }

        /// <summary>
        /// Creates a formula column and adds it to the columns list.
        /// </summary>
        /// <param name="name">The name.</param>
        /// /// <param name="formula">The formula.</param>
        /// <returns>This object.</returns>
        public SelectQuery AddSelectFormulaColumn(string name, string formula)
        {
            var selectFormulaColumn = new SelectFormulaColumn()
            {
                Alias = name,
                Formula = formula,
                Table = BaseTable
            };

            _columns.Add(selectFormulaColumn);

            return this;
        }

        /// <summary>
        /// Adds the select boolean column.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="alias">The alias.</param>
        /// <param name="trueText">The true text.</param>
        /// <param name="falseText">The false text.</param>
        /// <returns>This object.</returns>
        public SelectQuery AddSelectBooleanColumn(string columnName, string alias, string trueText, string falseText)
        {
            return AddSelectBooleanColumn(BaseTable, columnName, alias, trueText, falseText);
        }

        /// <summary>
        /// Adds the select boolean column.
        /// </summary>
        /// <param name="queryTable">The query table.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="alias">The alias.</param>
        /// <param name="trueText">The true text.</param>
        /// <param name="falseText">The false text.</param>
        /// <returns>This object.</returns>
        public SelectQuery AddSelectBooleanColumn(QueryTable queryTable, string columnName, string alias, string trueText,
            string falseText)
        {
            var enumTransation = new EnumFieldTranslation();
            enumTransation.LoadFromBoolean(trueText, falseText);
            return AddSelectEnumColumn(queryTable, columnName, alias, enumTransation);
        }

        /// <summary>
        /// Creates an enumerator column and adds it to the columns list.
        /// </summary>
        /// <typeparam name="T">An enumerator class</typeparam>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="alias">The alias.</param>
        /// <returns>This object.</returns>
        public SelectQuery AddSelectEnumColumn<T>(string columnName, string alias) where T : Enum
        {
            return AddSelectEnumColumn<T>(BaseTable, columnName, alias);
        }

        /// <summary>
        /// Creates an enumerator column and adds it to the columns list.
        /// </summary>
        /// <typeparam name="T">An enumerator class</typeparam>
        /// <param name="queryTable">The query table object attached to this column.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="alias">The alias.</param>
        /// <returns>This object.</returns>
        public SelectQuery AddSelectEnumColumn<T>(QueryTable queryTable, string columnName, string alias) where T : Enum
        {
            var enumFieldTranslation = new EnumFieldTranslation();
            enumFieldTranslation.LoadFromEnum<T>();
            return AddSelectEnumColumn(queryTable, columnName, alias, enumFieldTranslation);
        }

        internal SelectQuery AddSelectEnumColumn(QueryTable queryTable, string columnName, string alias, EnumFieldTranslation translation)
        {
            var selectEnumColumn = new SelectEnumColumn()
            {
                ColumnName = columnName,
                Alias = alias,
                Table = queryTable,
                EnumTranslation = translation
            };

            _columns.Add(selectEnumColumn);
            return this;
        }

        private string GetQueryTableMismatchMessage(QueryTable queryTable)
        {
            return
                $"Invalid QueryTable.  {queryTable.Query.BaseTable.Name} does not match this Query's BaseTable ({BaseTable.Name})";
        }
        /// <summary>
        /// Adds a primary join table.
        /// </summary>
        /// <param name="joinType">Type of the join.</param>
        /// <param name="primaryTableName">The name of the primary table being joined.</param>
        /// <returns>The join table object that was created for further configuration.</returns>
        public PrimaryJoinTable AddPrimaryJoinTable(JoinTypes joinType, string primaryTableName)
        {
            return AddPrimaryJoinTable(joinType, BaseTable, primaryTableName);
        }

        /// <summary>
        /// Adds a primary join table.
        /// </summary>
        /// <param name="joinType">Type of the join.</param>
        /// <param name="foreignTable">The foreign ("Many") table.</param>
        /// <param name="primaryTableName">The name of the primary table being joined.</param>
        /// <returns>The join table object that was created for further configuration.</returns>
        public PrimaryJoinTable AddPrimaryJoinTable(JoinTypes joinType, QueryTable foreignTable, string primaryTableName)
        {
            return AddPrimaryJoinTable(joinType, foreignTable, primaryTableName, string.Empty);
        }

        /// <summary>
        /// Adds a primary join table.
        /// </summary>
        /// <param name="joinType">Type of the join.</param>
        /// <param name="primaryTableName">The name of the table being joined.</param>
        /// <param name="alias">The value that appears after the "AS" keyword.</param>
        /// <returns>The join table object that was created for further configuration.</returns>
        public PrimaryJoinTable AddPrimaryJoinTable(JoinTypes joinType, string primaryTableName, string alias)
        {
            return AddPrimaryJoinTable(joinType, BaseTable, primaryTableName, alias);
        }

        /// <summary>
        /// Adds a primary join table.
        /// </summary>
        /// <param name="joinType">Type of the join.</param>
        /// <param name="foreignTable">The foreign ("Many") table.</param>
        /// <param name="primaryTableName">The name of the primary table being joined.</param>
        /// <param name="alias">The value that appears after the "AS" keyword.</param>
        /// <returns>The join table object that was created for further configuration.</returns>
        public PrimaryJoinTable AddPrimaryJoinTable(JoinTypes joinType, QueryTable foreignTable, string primaryTableName, string alias)
        {
            var primaryJoinTable = new PrimaryJoinTable
            {
                Alias = alias,
                JoinType = joinType,
                Name = primaryTableName,
                ForeignTable = foreignTable,
                Query = this
            };

            _joinTables.Add(primaryJoinTable);

            return primaryJoinTable;
        }

        /// <summary>
        /// Creates a where item object and adds it to the WhereItems list.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The string value.</param>
        /// <param name="isMemo">if set to <c>true</c> [is memo/ntext].</param>
        /// <returns>The where item object that was created for further configuration.</returns>
        public WhereItem AddWhereItem(string fieldName, Conditions condition, string value, bool isMemo = false)
        {
            return AddWhereItem(BaseTable, fieldName, condition, value, isMemo);
        }

        /// <summary>
        /// Creates a where item object and adds it to the WhereItems list.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The string value.</param>
        /// <param name="isMemo">if set to <c>true</c> [is memo].</param>
        /// <returns>The where item object that was created for further configuration.</returns>
        public WhereItem AddWhereItem(QueryTable table, string fieldName, Conditions condition, string value, bool isMemo = false)
        {
            return AddNewWhereItem(table, fieldName, condition, value,  isMemo?ValueTypes.Memo:ValueTypes.String);
        }

        /// <summary>
        /// Creates a where item object and adds it to the WhereItems list.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The date value.</param>
        /// <param name="dateType">Type of the date.</param>
        /// <returns>The where item object that was created for further configuration.</returns>
        public WhereItem AddWhereItem(string fieldName, Conditions condition, DateTime value, DbDateTypes dateType)
        {
            return AddWhereItem(BaseTable, fieldName, condition, value, dateType);
        }

        /// <summary>
        /// Creates a where item object and adds it to the WhereItems list.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <param name="dateType">Type of the date.</param>
        /// <returns>The where item object that was created for further configuration.</returns>
        /// <exception cref="ArgumentOutOfRangeException">dateType - null</exception>
        public WhereItem AddWhereItem(QueryTable table, string fieldName, Conditions condition, DateTime value,
            DbDateTypes dateType)
        {
            var result = AddNewWhereItem(table, fieldName, condition, FormatDateString(value, dateType), ValueTypes.DateTime);
            result.DateType = dateType;
            return result;
        }

        internal static string FormatDateString(DateTime value, DbDateTypes dateType)
        {
            string stringValue;
            switch (dateType)
            {
                case DbDateTypes.DateOnly:
                    stringValue = $"{value:yyyy-MM-dd}";
                    break;
                case DbDateTypes.DateTime:
                    stringValue = $"{value:yyyy-MM-dd HH:mm:ss}";
                    break;
                case DbDateTypes.Millisecond:
                    stringValue = $"{value:yyyy-MM-dd HH:mm:ss.fff}";
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(dateType), dateType, null);
            }

            return stringValue;
        }

        /// <summary>
        /// Creates a where item object and adds it to the WhereItems list.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The integer value.</param>
        /// <returns>The where item object that was created for further configuration.</returns>
        public WhereItem AddWhereItem(string fieldName, Conditions condition, int value)
        {
            return AddWhereItem(BaseTable, fieldName, condition, value);
        }

        /// <summary>
        /// Creates a where item object and adds it to the WhereItems list.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The integer value.</param>
        /// <returns>The where item object that was created for further configuration.</returns>
        public WhereItem AddWhereItem(QueryTable table, string fieldName, Conditions condition, int value)
        {
            return AddNewWhereItem(table, fieldName, condition, value.ToString(), ValueTypes.Numeric);
        }

        /// <summary>
        /// Creates a where item object and adds it to the WhereItems list.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The double value.</param>
        /// <returns>The where item object that was created for further configuration.</returns>
        public WhereItem AddWhereItem(string fieldName, Conditions condition, double value)
        {
            return AddWhereItem(BaseTable, fieldName, condition, value);
        }

        /// <summary>
        /// Creates a where item object and adds it to the WhereItems list.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The double value.</param>
        /// <returns>The where item object that was created for further configuration.</returns>
        public WhereItem AddWhereItem(QueryTable table, string fieldName, Conditions condition, double value)
        {
            return AddNewWhereItem(table, fieldName, condition, value.ToString(CultureInfo.InvariantCulture),
                ValueTypes.Numeric);
        }

        /// <summary>
        /// Creates a where item object and adds it to the WhereItems list.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The bool value.</param>
        /// <returns>The where item object that was created for further configuration.</returns>
        public WhereItem AddWhereItem(string fieldName, Conditions condition, bool value)
        {
            return AddWhereItem(BaseTable, fieldName, condition, value);
        }

        /// <summary>
        /// Creates a where item object and adds it to the WhereItems list.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The bool value.</param>
        /// <returns>The where item object that was created for further configuration.</returns>
        public WhereItem AddWhereItem(QueryTable table, string fieldName, Conditions condition, bool value)
        {
            return AddNewWhereItem(table, fieldName, condition, BoolToString(value), ValueTypes.Bool);
        }

        public static string BoolToString(bool value)
        {
            var stringValue = "1";
            if (!value)
                stringValue = "0";

            return stringValue;
        }

        /// <summary>
        /// Creates a where item object and adds it to the WhereItems list.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <param name="type">The value type.</param>
        /// <param name="dateType">The date value type if ValueType is a DateTime.</param>
        /// <returns>The where item object that was created for further configuration.</returns>
        /// <exception cref="ArgumentException"></exception>
        public WhereItem AddWhereItem(QueryTable table, string fieldName, Conditions condition, string value,
            ValueTypes type, DbDateTypes dateType)
        {
            if (table.Query != this)
            {
                throw new ArgumentException(GetQueryTableMismatchMessage(table));
            }

            if (type == ValueTypes.DateTime)
            {
                DateTime dateValue;
                if (!DateTime.TryParse(value, out dateValue))
                    throw new ArgumentException("Value is not a DateTimeValue");

                value = FormatDateString(dateValue, dateType);
            }

            var result = AddNewWhereItem(table, fieldName, condition, value, type);
            result.DateType = dateType;
            return result;
        }

        public WhereItem AddWhereItemCheckNull(QueryTable table, string idField, Conditions condition, bool checkString)
        {
            var idWhere = AddNewWhereItem(table, idField, condition, string.Empty, ValueTypes.Numeric);
            idWhere.CheckDescriptionForNull = checkString;

            return idWhere;
        }

        /// <summary>
        /// Adds the where item bool text.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <param name="trueText">The true text.</param>
        /// <param name="falseText">The false text.</param>
        /// <returns>The where item object that was created for further configuration.</returns>
        public WhereEnumItem AddWhereItemBoolText(string fieldName, Conditions condition, string value, string trueText,
            string falseText)
        {
            return AddWhereItemBoolText(BaseTable, fieldName, condition, value, trueText, falseText);
        }

        /// <summary>
        /// Adds the where item bool text.
        /// </summary>
        /// <param name="queryTable">The query table.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <param name="trueText">The true text.</param>
        /// <param name="falseText">The false text.</param>
        /// <returns>The where item object that was created for further configuration.</returns>
        public WhereEnumItem AddWhereItemBoolText(QueryTable queryTable, string fieldName, Conditions condition,
            string value, string trueText, string falseText)
        {
            var enumTransation = new EnumFieldTranslation();
            enumTransation.LoadFromBoolean(trueText, falseText);
            return AddWhereItemEnum(queryTable, fieldName, condition, value, enumTransation);
        }

        /// <summary>
        /// Creates an enumerator where item and adds it to the where items list.
        /// </summary>
        /// <typeparam name="T">An enumerator class</typeparam>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <returns>The where item object that was created for further configuration.</returns>
        public WhereEnumItem AddWhereItemEnum<T>(string fieldName, Conditions condition, string value) where T : Enum
        {
            return AddWhereItemEnum<T>(BaseTable, fieldName, condition, value);
        }

        /// <summary>
        /// Creates an enumerator where item and adds it to the where items list.
        /// </summary>
        /// <typeparam name="T">An enumerator class</typeparam>
        /// <param name="queryTable">The query table.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <returns>The where item object that was created for further configuration.</returns>
        public WhereEnumItem AddWhereItemEnum<T>(QueryTable queryTable, string fieldName, Conditions condition, string value) where T : Enum
        {
            var enumFieldTranslation = new EnumFieldTranslation();
            enumFieldTranslation.LoadFromEnum<T>();
            return AddWhereItemEnum(queryTable, fieldName, condition, value, enumFieldTranslation);
        }

        internal WhereEnumItem AddWhereItemEnum(QueryTable queryTable, string fieldName, Conditions condition, string value, EnumFieldTranslation translation)
        {
            var whereItemEnum = new WhereEnumItem
            {
                Table = queryTable,
                FieldName = fieldName,
                Condition = condition,
                Value = value,
                ValueType = ValueTypes.String,
                EnumTranslation = translation
            };

            _whereItems.Add(whereItemEnum);
            return whereItemEnum;
        }

        /// <summary>
        /// Creates a where item formula object and adds it to the WhereItems list.
        /// </summary>
        /// <param name="formula">The formula.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public WhereFormulaItem AddWhereItemFormula(string formula, Conditions condition, string value)
        {
            return AddWhereItemFormula(formula, condition, value, ValueTypes.String);
        }

        /// <summary>
        /// Creates a where item formula object and adds it to the WhereItems list.
        /// </summary>
        /// <param name="formula">The formula.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <param name="dateType">Type of the date.</param>
        /// <returns></returns>
        public WhereFormulaItem AddWhereItemFormula(string formula, Conditions condition, DateTime value, DbDateTypes dateType)
        {
            return AddWhereItemFormula(formula, condition, FormatDateString(value, dateType), ValueTypes.DateTime);
        }

        /// <summary>
        /// Creates a where item formula object and adds it to the WhereItems list.
        /// </summary>
        /// <param name="formula">The formula.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public WhereFormulaItem AddWhereItemFormula(string formula, Conditions condition, int value)
        {
            return AddWhereItemFormula(formula, condition, value.ToString(), ValueTypes.Numeric);
        }

        /// <summary>
        /// Creates a where item formula object and adds it to the WhereItems list.
        /// </summary>
        /// <param name="formula">The formula.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public WhereFormulaItem AddWhereItemFormula(string formula, Conditions condition, double value)
        {
            return AddWhereItemFormula(formula, condition, value.ToString(CultureInfo.InvariantCulture),
                ValueTypes.Numeric);
        }

        /// <summary>
        /// Creates a where item formula object and adds it to the WhereItems list.
        /// </summary>
        /// <param name="formula">The formula.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns></returns>
        public WhereFormulaItem AddWhereItemFormula(string formula, Conditions condition, bool value)
        {
            return AddWhereItemFormula(formula, condition, BoolToString(value), ValueTypes.Bool);
        }

        /// <summary>
        /// Creates a where item formula object and adds it to the WhereItems list.  Used for formulas that have no condition or value.
        /// </summary>
        /// <param name="formula">The formula.</param>
        /// <returns></returns>
        public WhereFormulaItem AddWhereItemFormula(string formula)
        {
            var whereItem = new WhereFormulaItem
            {
                Formula = formula,
                NoValue = true
            };
            _whereItems.Add(whereItem);

            return whereItem;
        }

        internal WhereFormulaItem AddWhereItemFormula(string formula, Conditions condition, string value,
            ValueTypes valueType)
        {
            var whereItem = new WhereFormulaItem
            {
                Table = BaseTable,
                Condition = condition,
                Value = value,
                ValueType = valueType,
                Formula = formula
            };

            _whereItems.Add(whereItem);

            return whereItem;
        }

        private WhereItem AddNewWhereItem(QueryTable table, string fieldName, Conditions condition, string value,
            ValueTypes type)
        {
            var whereItem = new WhereItem
            {
                Condition = condition,
                FieldName = fieldName,
                Table = table,
                Value = value,
                ValueType = type
            };

            _whereItems.Add(whereItem);

            return whereItem;
        }

        /// <summary>
        /// Removes the where item.
        /// </summary>
        /// <param name="whereItem">The where item.</param>
        /// <returns>This object.</returns>
        public SelectQuery RemoveWhereItem(WhereItem whereItem)
        {
            _whereItems.Remove(whereItem);
            return this;
        }

        /// <summary>
        /// Adds an order by segment.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="orderByType">Type of the order by.</param>
        /// <param name="isCaseSensitive">Is order by case sensitive.  By default it is set to false.</param>
        /// <returns>This object.</returns>
        public SelectQuery AddOrderBySegment(string fieldName, OrderByTypes orderByType, bool isCaseSensitive = false)
        {
            return AddOrderBySegment(BaseTable, fieldName, orderByType, isCaseSensitive);
        }

        /// <summary>
        /// Adds an order by segment.
        /// </summary>
        /// <param name="queryTable">The query table.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="orderByType">Type of the order by.</param>
        /// <param name="isCaseSensitive">Is order by case sensitive.  By default it is set to false.</param>
        /// <returns>This object.</returns>
        public SelectQuery AddOrderBySegment(QueryTable queryTable, string fieldName, OrderByTypes orderByType, bool isCaseSensitive = false)
        {
            var orderBySegment = new  OrderBySegment
            {
                Table = queryTable,
                FieldName = fieldName,
                OrderByType = orderByType,
                CaseSensitive = isCaseSensitive
            };

            _orderBySegments.Add(orderBySegment);

            return this;
        }

        /// <summary>
        /// Adds the order by bool text segment.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="orderByType">Type of the order by.</param>
        /// <param name="trueText">The true text.</param>
        /// <param name="falseText">The false text.</param>
        /// <param name="isCaseSensitive">if set to <c>true</c> [is case sensitive].</param>
        /// <returns>This object.</returns>
        public SelectQuery AddOrderByBoolTextSegment(string fieldName, OrderByTypes orderByType, string trueText,
            string falseText, bool isCaseSensitive = false)
        {
            return AddOrderByBoolTextSegment(BaseTable, fieldName, orderByType, trueText, falseText, isCaseSensitive);
        }

        /// <summary>
        /// Adds the order by bool text segment.
        /// </summary>
        /// <param name="queryTable">The query table.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="orderByType">Type of the order by.</param>
        /// <param name="trueText">The true text.</param>
        /// <param name="falseText">The false text.</param>
        /// <param name="isCaseSensitive">if set to <c>true</c> [is case sensitive].</param>
        /// <returns>This object.</returns>
        public SelectQuery AddOrderByBoolTextSegment(QueryTable queryTable, string fieldName, OrderByTypes orderByType, string trueText,
            string falseText, bool isCaseSensitive = false)
        {
            var enumTransation = new EnumFieldTranslation();
            enumTransation.LoadFromBoolean(trueText, falseText);
            return AddOrderByEnumSegment(queryTable, fieldName, orderByType, enumTransation, isCaseSensitive);
        }


        /// <summary>
        /// Adds an order by enumerator segment.
        /// </summary>
        /// <typeparam name="T">An enumerator class.</typeparam>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="orderByType">Type of the order by.</param>
        /// <param name="isCaseSensitive">Is order by case sensitive.  By default it is set to false.</param>
        /// <returns>This object.</returns>
        public SelectQuery AddOrderByEnumSegment<T>(string fieldName, OrderByTypes orderByType, bool isCaseSensitive = false) where T : Enum
        {
            return AddOrderByEnumSegment<T>(BaseTable, fieldName, orderByType, isCaseSensitive);
        }

        /// <summary>
        /// Adds an order by enumerator segment.
        /// </summary>
        /// <typeparam name="T">An enumerator class.</typeparam>
        /// <param name="queryTable">The query table.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="orderByType">Type of the order by.</param>
        /// <param name="isCaseSensitive">Is order by case sensitive.  By default it is set to false.</param>
        /// <returns>This object.</returns>
        public SelectQuery AddOrderByEnumSegment<T>(QueryTable queryTable, string fieldName, OrderByTypes orderByType, bool isCaseSensitive = false) where  T : Enum
        {
            var enumFieldTranslation = new EnumFieldTranslation();
            enumFieldTranslation.LoadFromEnum<T>();
            return AddOrderByEnumSegment(queryTable, fieldName, orderByType, enumFieldTranslation, isCaseSensitive);
        }

        internal SelectQuery AddOrderByEnumSegment(QueryTable queryTable, string fieldName, OrderByTypes orderByType, EnumFieldTranslation translation, bool isCaseSensitive)
        {
            var orderByEnumSegment = new OrderByEnumSegment
            {
                Table = queryTable,
                FieldName = fieldName,
                OrderByType = orderByType,
                EnumTranslation = translation,
                CaseSensitive = isCaseSensitive
            };

            _orderBySegments.Add(orderByEnumSegment);

            return this;
        }

        /// <summary>
        /// Adds a formula order by segment.
        /// </summary>
        /// <param name="formula">The formula.</param>
        /// <param name="orderByType">Type of the order by.</param>
        /// <param name="isCaseSensitive">Is order by case sensitive.  By default it is set to false.</param>
        /// <returns>This object.</returns>
        public SelectQuery AddOrderByFormulaSegment(string formula, OrderByTypes orderByType, bool isCaseSensitive = false)
        {
            var orderByFormulaSegment = new OrderByFormulaSegment
            {
                Table = BaseTable,
                Formula = formula,
                OrderByType = orderByType,
                CaseSensitive = isCaseSensitive
            };

            _orderBySegments.Add(orderByFormulaSegment);

            return this;
        }

        /// <summary>
        /// Copies the select columns to top level query.
        /// </summary>
        /// <param name="topLevelQuery">The top level query.</param>
        public void CopySelectColumnsToTopLevelQuery(SelectQuery topLevelQuery)
        {
            foreach (var baseQueryColumn in Columns)
            {
                topLevelQuery.AddSelectColumn(baseQueryColumn.Alias.IsNullOrEmpty()
                    ? baseQueryColumn.ColumnName
                    : baseQueryColumn.Alias);
            }
        }
    }
}
