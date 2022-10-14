using RingSoft.DataEntryControls.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RingSoft.DbLookup.QueryBuilder;

namespace RingSoft.DbLookup.DataProcessor.SelectSqlGenerator
{
    /// <summary>Implement this interface to generate a SELECT SQL statement for a database platform based on what's in the Query object.</summary>
    public abstract class DbSelectSqlGenerator
    {
        public virtual char SqlObjectPrefixChar => '[';
        public virtual char SqlObjectSuffixChar => ']';

        /// <summary>
        /// Gets the SQL prefix that needs to be at the beginning of each line in the generated SQL statement.  Useful in
        /// formatting nested SQL statements.  This usually contains TAB characters.
        /// </summary>
        /// <value>
        /// The SQL line prefix.
        /// </value>
        public string SqlLinePrefix { get; protected set; }

        public string FormatSqlObject(string sqlObject) => $"{SqlObjectPrefixChar}{sqlObject}{SqlObjectSuffixChar}";

        /// <summary>
        /// Generates the SELECT SQL statement.
        /// </summary>
        /// <param name="query">The QueryBuilder.QueryBase object containing all the data for the SQL statement.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public string GenerateSelectStatement(QueryBase query)
        {
            switch (query.QueryType)
            {
                case QueryTypes.SelectQuery:
                    if (query is SelectQuery selectQuery)
                    {
                        return GenerateSelectQueryStatement(selectQuery);
                    }
                    break;
                case QueryTypes.CountQuery:
                    if (query is CountQuery countQuery)
                    {
                        return GenerateCountQueryStatement(countQuery);
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return string.Empty;
        }

        /// <summary>
        /// Generates a SQL statement to count the number of records from the database.
        /// </summary>
        /// <param name="countQuery">The count query.</param>
        /// <returns></returns>
        protected virtual string GenerateCountQueryStatement(CountQuery countQuery)
        {
            var sqlStringBuilder = new StringBuilder();

            sqlStringBuilder.AppendLine(
                $"{SqlLinePrefix}SELECT COUNT(*) AS {FormatSqlObject(countQuery.CountColumnName)}");
            sqlStringBuilder.AppendLine($"{SqlLinePrefix}FROM");
            sqlStringBuilder.AppendLine($"{SqlLinePrefix}(");

            SqlLinePrefix += '\t';
            sqlStringBuilder.AppendLine(GenerateSelectQueryStatement(countQuery.SelectQuery, true));
            SqlLinePrefix = SqlLinePrefix.TrimRight("\t");

            sqlStringBuilder.AppendLine($"{SqlLinePrefix}) AS {FormatSqlObject(countQuery.CountColumnName)}");

            var sql = sqlStringBuilder.ToString();
            return sql;
        }

        /// <summary>
        /// Generates the SELECT SQL statement.
        /// </summary>
        /// <param name="selectQuery">The QueryBuilder.SelectQuery object containing all the data for the SQL statement.</param>
        /// <param name="skipOrderBy">if set to <c>true</c> then don't generate the ORDER BY clause.</param>
        /// <returns></returns>
        protected virtual string GenerateSelectQueryStatement(SelectQuery selectQuery, bool skipOrderBy = false)
        {
            var sqlStringBuilder = new StringBuilder();

            sqlStringBuilder.AppendLine(GenerateSelectClause(selectQuery));

            if (selectQuery.NestedQuery != null)
            {
                sqlStringBuilder.AppendLine(GenerateNestedQuery(selectQuery, selectQuery.NestedQuery));
            }
            else
            {
                sqlStringBuilder.AppendLine(GenerateFromClause(selectQuery));
            }

            if (selectQuery.JoinTables.Any())
                sqlStringBuilder.AppendLine(GenerateJoins(selectQuery));

            if (selectQuery.WhereItems.Any())
                sqlStringBuilder.AppendLine(GenerateWhereClause(selectQuery));

            var selectColumns = selectQuery.Columns.Where(w => w.IsDistinct);
            var distinctColumns = selectColumns as SelectColumn[] ?? selectColumns.ToArray();
            if (distinctColumns.Any())
                sqlStringBuilder.AppendLine(GenerateGroupBy(distinctColumns));

            if (selectQuery.OrderBySegments.Any() && !skipOrderBy)
                sqlStringBuilder.AppendLine(GenerateOrderByClause(selectQuery));

            var sqlString = sqlStringBuilder.ToString().TrimEnd('\r', '\n');
            return sqlString;
        }

        /// <summary>
        /// Generates the SELECT clause.  Override if this method is not compatible with your database platform.
        /// </summary>
        /// <param name="query">The QueryBuilder.Query object containing all the data for the SQL statement.</param>
        /// <returns></returns>
        protected virtual string GenerateSelectClause(SelectQuery query)
        {
            var sqlStringBuilder = new StringBuilder();
            sqlStringBuilder.Append($"{SqlLinePrefix}SELECT ");
            sqlStringBuilder.Append($"{GenerateTopRecordCountSqlText(query)} ");

            if (query.Columns.Any())
            {
                var firstColumn = true;
                var selectColumnsSql = string.Empty;
                foreach (var selectColumn in query.Columns)
                {
                    var selectFragment = string.Empty;
                    var selectLinePrefix = $"{SqlLinePrefix}\t";
                    var tableField = $"{FormatSqlObject(selectColumn.Table.GetTableName())}.{FormatSqlObject(selectColumn.ColumnName)}";

                    switch (selectColumn.ColumnType)
                    {
                        case ColumnTypes.General:
                            if (!firstColumn)
                                selectFragment = selectLinePrefix;

                            selectFragment +=tableField;
                            break;
                        case ColumnTypes.Formula:
                            if (selectColumn is SelectFormulaColumn selectFormulaColumn)
                            {
                                if (firstColumn)
                                    selectFragment = "\r\n";
                                selectFragment +=
                                    FormatSelectFragmentFormula(selectFormulaColumn.Formula, selectLinePrefix);
                            }
                            break;
                        case ColumnTypes.Enum:
                            if (selectColumn is SelectEnumColumn selectEnumColumn)
                            {
                                if (firstColumn)
                                    selectFragment = "\r\n";

                                var enumText = GenerateEnumeratorSqlFieldText(tableField, selectEnumColumn.EnumTranslation);
                                selectFragment +=
                                    FormatSelectFragmentFormula(enumText, selectLinePrefix);
                            }

                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    if (!selectColumn.Alias.IsNullOrEmpty())
                        selectFragment += $" AS {FormatSqlObject(selectColumn.Alias)}";

                    firstColumn = false;
                    selectColumnsSql += $"{selectFragment},\r\n";
                }

                selectColumnsSql = selectColumnsSql.TrimEnd(',', '\r', '\n');
                sqlStringBuilder.AppendLine(selectColumnsSql);
            }
            else
            {
                sqlStringBuilder.AppendLine("*");
            }

            var sql = sqlStringBuilder.ToString().TrimEnd('\r', '\n');
            return sql;
        }

        private string FormatSelectFragmentFormula(string formula, string selectLinePrefix)
        {
            var sql = $"{selectLinePrefix}(\r\n";
            selectLinePrefix += "\t";
            sql +=
                $"{selectLinePrefix}{FormatFormulaSqlText(formula, selectLinePrefix)}\r\n";
            selectLinePrefix = selectLinePrefix.TrimRight("\t");
            sql += $"{selectLinePrefix}) ";

            return sql;
        }
        /// <summary>
        /// Generates SQL field text for an enumerator field.  Override if this method is not compatible with your database platform.
        /// </summary>
        /// <param name="tableAndFieldName">Name of the table and field combined e.g([TableName].[FieldName]).</param>
        /// <param name="translationInfo">The translation information.</param>
        /// <returns></returns>
        protected virtual string GenerateEnumeratorSqlFieldText(string tableAndFieldName, EnumFieldTranslation translationInfo)
        {
            var sql = $"CASE {tableAndFieldName}\r\n";
            foreach (var typeTranslation in translationInfo.TypeTranslations)
            {
                sql += $"\tWHEN {typeTranslation.NumericValue} THEN {FormatStringValueForSql(typeTranslation.TextValue)}\r\n";
            }

            sql += "END";
            return sql;
        }

        private string FormatFormulaSqlText(string sqlText, string linePrefix)
        {
            return sqlText.Replace("\r\n", $"\r\n{linePrefix}");
        }

        /// <summary>
        /// Generates the top record count SQL text.  Override if this method is not compatible with your database platform.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        protected virtual string GenerateTopRecordCountSqlText(SelectQuery query)
        {
            if (query.MaxRecords > 0)
                return $"TOP {query.MaxRecords}";

            return string.Empty;
        }

        /// <summary>
        /// Generates the FROM clause.  Override if this method is not compatible with your database platform.
        /// </summary>
        /// <param name="query">The QueryBuilder.Query object containing all the data for the SQL statement.</param>
        /// <returns></returns>
        protected virtual string GenerateFromClause(SelectQuery query)
        {
            if (!query.BaseTable.Formula.IsNullOrEmpty())
            {
                return $"{SqlLinePrefix} FROM (\r\n{query.BaseTable.Formula})\r\n AS {FormatSqlObject(query.BaseTable.GetTableName())}";
            }
            return $"{SqlLinePrefix}FROM {FormatSqlObject(query.BaseTable.Name)}";

        }

        /// <summary>
        /// Generates the nested query.  Override if this method is not compatible with your database platform.
        /// </summary>
        /// <param name="query">The QueryBuilder.Query object containing all the data for the SQL statement.</param>
        /// <param name="nestedQuery">The QueryBuilder.Query object containing all the data for the nested SQL statement.</param>
        /// <returns></returns>
        protected virtual string GenerateNestedQuery(SelectQuery query, SelectQuery nestedQuery)
        {
            var sqlStringBuilder = new StringBuilder();

            sqlStringBuilder.AppendLine($"{SqlLinePrefix}FROM");
            sqlStringBuilder.AppendLine($"{SqlLinePrefix}(");

            SqlLinePrefix += '\t';
            sqlStringBuilder.AppendLine(GenerateSelectQueryStatement(nestedQuery));
            SqlLinePrefix = SqlLinePrefix.TrimRight("\t");

            sqlStringBuilder.AppendLine($"{SqlLinePrefix}) AS {FormatSqlObject(query.BaseTable.Name)}");

            return sqlStringBuilder.ToString();
        }

        /// <summary>
        /// Generates the JOIN clauses.  Override if this method is not compatible with your database platform.
        /// </summary>
        /// <param name="query">The QueryBuilder.Query object containing all the data for the SQL statement.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        protected virtual string GenerateJoins(SelectQuery query)
        {
            var sqlStringBuilder = new StringBuilder();
            foreach (var joinTable in query.JoinTables)
            {
                string joinSql = SqlLinePrefix;
                switch (joinTable.JoinType)
                {
                    case JoinTypes.InnerJoin:
                        joinSql += "INNER JOIN ";
                        break;
                    case JoinTypes.LeftOuterJoin:
                        joinSql += "LEFT OUTER JOIN ";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                joinSql += $"{FormatSqlObject(joinTable.Name)} ";
                if (!joinTable.Alias.IsNullOrEmpty())
                    joinSql += $"AS {FormatSqlObject(joinTable.Alias)} ";
                joinSql += "ON\r\n";

                foreach (var joinField in joinTable.JoinFields)
                {
                    joinSql += $"{SqlLinePrefix}\t{FormatSqlObject(joinField.PrimaryTable.GetTableName())}.{FormatSqlObject(joinField.PrimaryField)} = ";
                    joinSql += $"{FormatSqlObject(joinField.PrimaryTable.ForeignTable.GetTableName())}.{FormatSqlObject(joinField.ForeignField)} AND\r\n";
                }

                joinSql = joinSql.TrimRight(" AND\r\n");
                sqlStringBuilder.AppendLine($"{joinSql}");
            }

            var sql = sqlStringBuilder.ToString().TrimRight("\r\n");
            return sql;
        }

        /// <summary>
        /// Generates the where clause.  Override if this method is not compatible with your database platform.
        /// </summary>
        /// <param name="query">The QueryBuilder.Query object containing all the data for the SQL statement.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        protected virtual string GenerateWhereClause(SelectQuery query)
        {
            if (!query.WhereItems.Any())
                return string.Empty;

            var sqlStringBuilder = new StringBuilder();
            sqlStringBuilder.AppendLine($"{SqlLinePrefix}WHERE");
            var whereLinePrefix = SqlLinePrefix + '\t';
            var lastEndLogic = string.Empty;

            foreach (var whereItem in query.WhereItems)
            {
                var whereItemSql = string.Empty;
                for (int i = 0; i < whereItem.LeftParenthesesCount; i++)
                {
                    whereItemSql += $"{whereLinePrefix}(\r\n";
                    whereLinePrefix += '\t';
                }

                switch (whereItem.WhereItemType)
                {
                    case WhereItemTypes.General:
                        whereItemSql += $"{whereLinePrefix}{GenerateWhereItemSqlText(whereItem)}";
                        break;
                    case WhereItemTypes.Formula:
                        var processNull = true;
                        if (whereItem is WhereFormulaItem whereFormulaItem)
                        {
                            switch (whereItem.Condition)
                            {
                                case Conditions.EqualsNull:
                                case Conditions.NotEqualsNull:
                                    if (whereFormulaItem.Formula.IsNullOrEmpty())
                                    {
                                        whereItemSql += $"{whereLinePrefix}{GenerateWhereItemSqlText(whereItem)}";
                                        processNull = false;
                                    }
                                    break;
                            }

                            if (processNull)
                            {
                                string formulaText;

                                if (whereFormulaItem.NoValue)
                                    formulaText = GenerateWhereItemNoValueFormulaText(whereFormulaItem.Formula);
                                else
                                    formulaText =
                                        GenerateWhereItemFormulaText(whereFormulaItem, whereFormulaItem.Formula);

                                whereItemSql +=
                                    $"{whereLinePrefix}{FormatFormulaSqlText(formulaText, whereLinePrefix)}";
                            }
                        }

                        break;

                    case WhereItemTypes.Enum:
                        if (whereItem is WhereEnumItem whereEnumItem)
                        {
                            var enumText = GenerateWhereEnumItemSqlText(whereEnumItem);
                            whereItemSql += $"{whereLinePrefix}{FormatFormulaSqlText(enumText, whereLinePrefix)}";
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if (whereItem.RightParenthesesCount > 0)
                    whereItemSql += "\r\n";

                for (int i = 0; i < whereItem.RightParenthesesCount; i++)
                {
                    whereLinePrefix = whereLinePrefix.TrimRight("\t");
                    whereItemSql += $"{whereLinePrefix})";
                    if (i < whereItem.RightParenthesesCount - 1)
                        whereItemSql += "\r\n";
                }

                switch (whereItem.EndLogic)
                {
                    case EndLogics.And:
                        lastEndLogic = " AND\r\n";
                        break;
                    case EndLogics.Or:
                        lastEndLogic = " OR\r\n";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                whereItemSql += lastEndLogic;
                sqlStringBuilder.Append(whereItemSql);
            }

            var sql = $"{sqlStringBuilder.ToString().TrimRight(lastEndLogic)}";
            return sql;
        }

        /// <summary>
        /// Generates WHERE SQL fragment text for the inputted WhereItem object.
        /// Override if this method is not compatible with your database platform.
        /// </summary>
        /// <param name="whereItem">The WhereItem object containing all the data for the SQL statement.</param>
        /// <returns></returns>
        protected virtual string GenerateWhereItemSqlText(WhereItem whereItem)
        {
            var sqlFieldName = GenerateWhereItemSqlFieldNameText(whereItem);
            var condition = GenerateConditionSqlText(whereItem, sqlFieldName);
            var sqlValue = FormatValueForSqlWhereItem(whereItem);
            var sql = $"({sqlFieldName} {condition} {sqlValue})";

            switch (whereItem.Condition)
            {
                case Conditions.EqualsNull:
                case Conditions.NotEqualsNull:
                    sql = $"({sqlFieldName} {condition})";
                    break;
            }
            return sql;
        }

        /// <summary>
        /// Generates the where item SQL field name text for the inputted WhereItem object.
        /// Override if this method is not compatible with your database platform.
        /// </summary>
        /// <param name="whereItem">The WhereItem object containing all the data for the SQL statement.</param>
        /// <returns></returns>
        protected virtual string GenerateWhereItemSqlFieldNameText(WhereItem whereItem)
        {
            var sqlFieldName =
                $"{FormatSqlObject(whereItem.Table.GetTableName())}.{FormatSqlObject(whereItem.FieldName)}";
            return sqlFieldName;
        }

        /// <summary>
        /// Generates the where item enumerator SQL text.
        /// </summary>
        /// <param name="whereItem">The where item.</param>
        /// <returns></returns>
        protected virtual string GenerateWhereEnumItemSqlText(WhereEnumItem whereItem)
        {
            var tableField = $"{FormatSqlObject(whereItem.Table.GetTableName())}.{FormatSqlObject(whereItem.FieldName)}";
            var formula = GenerateEnumeratorSqlFieldText(tableField, whereItem.EnumTranslation);
            var sql = GenerateWhereItemFormulaText(whereItem, formula);
            return sql;
        }

        private string GenerateWhereItemFormulaText(WhereItem whereItem, string formula)
        {
            var tableField = string.Empty;
            if (whereItem.Table != null)
                tableField =
                    $"{FormatSqlObject(whereItem.Table.GetTableName())}.{FormatSqlObject(whereItem.FieldName)}";
            var sqlValue = FormatValueForSqlWhereItem(whereItem);
            var condition = GenerateConditionSqlText(whereItem, tableField);
            var sql = "(\r\n";
            sql += $"\t{FormatFormulaSqlText(formula, "\t")}\r\n";
            sql += $") {condition} {sqlValue}";
            return sql;
        }

        private string GenerateWhereItemNoValueFormulaText(string formula)
        {
            var sql = "(\r\n";
            sql += $"\t{FormatFormulaSqlText(formula, "\t")}\r\n";
            sql += $")";
            return sql;
        }

        /// <summary>
        /// Generates WHERE clause condition SQL text for the inputted WhereItem object.
        /// Override if this method is not compatible with your database platform.
        /// </summary>
        /// <param name="whereItem">The WhereItem object containing all the data for the SQL statement.</param>
        /// <param name="sqlFieldName">The field name in '[TableName].[FieldName]' format.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">Condition - null</exception>
        protected virtual string GenerateConditionSqlText(WhereItem whereItem, string sqlFieldName)
        {
            var formula = false;
            switch (whereItem.WhereItemType)
            {
                case WhereItemTypes.General:
                    break;
                case WhereItemTypes.Formula:
                case WhereItemTypes.Enum:
                    formula = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var conditionResult = string.Empty;
            var condition = whereItem.Condition;
            if (whereItem.Value.IsNullOrEmpty())
            {
                switch (condition)
                {
                    case Conditions.EqualsNull:
                        condition = Conditions.EqualsNull;
                        break;
                    default:
                        condition = Conditions.NotEqualsNull;
                        break;
                }
            }
            switch (condition)
            {
                case Conditions.Equals:
                    return "=";
                case Conditions.NotEquals:
                    return "<>";
                case Conditions.GreaterThan:
                    return ">";
                case Conditions.GreaterThanEquals:
                    return ">=";
                case Conditions.LessThan:
                    return "<";
                case Conditions.LessThanEquals:
                    return "<=";
                case Conditions.Contains:
                    return $"LIKE '%{whereItem.Value.Replace("'", "''")}%'";
                case Conditions.NotContains:
                    return $"NOT LIKE '%{whereItem.Value}%'";
                case Conditions.EqualsNull:
                    if (whereItem.ValueType == ValueTypes.String && !formula)
                        return $"IS NULL OR {sqlFieldName} = ''";
                    else
                    {
                        return $"IS NULL";
                    }
                case Conditions.NotEqualsNull:
                    if (whereItem.ValueType == ValueTypes.String && !formula)
                        return $"IS NOT NULL AND {sqlFieldName} <> ''";
                    else
                    {
                        return $"IS NOT NULL";
                    }
                case Conditions.BeginsWith:
                    conditionResult = $"LIKE '{whereItem.Value.Replace("'", "''")}%'";
                    return conditionResult;
                case Conditions.EndsWith:
                    conditionResult = $"LIKE '%{whereItem.Value.Replace("'", "''")}'";
                    return conditionResult;
                default:
                    throw new ArgumentOutOfRangeException(nameof(whereItem.Condition), whereItem.Condition, null);
            }
        }

        /// <summary>
        /// Formats a value for a SQL WHERE clause.  Override if this method is not compatible with your database platform.
        /// </summary>
        /// <param name="whereItem">The WhereItem object containing all the data to format the value.</param>
        /// <returns>A formatted value for use in a WHERE clause.</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        protected virtual string FormatValueForSqlWhereItem(WhereItem whereItem)
        {
            var valueReturn = whereItem.Value;

            switch (whereItem.Condition)
            {
                case Conditions.EqualsNull:
                case Conditions.NotEqualsNull:
                case Conditions.BeginsWith:
                case Conditions.EndsWith:
                case Conditions.Contains:
                case Conditions.NotContains:
                    return string.Empty;

            }
            switch (whereItem.ValueType)
            {
                case ValueTypes.String:
                case ValueTypes.Memo:
                    if (whereItem.Value.IsNullOrEmpty())
                        valueReturn = string.Empty;
                    else
                    {
                        valueReturn = FormatStringValueForSql(whereItem.Value);
                    }
                    break;
                case ValueTypes.Numeric:
                    break;
                case ValueTypes.DateTime:
                    valueReturn = $"'{valueReturn}'";
                    break;
                case ValueTypes.Bool:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return valueReturn;
        }

        /// <summary>
        /// Formats the string value for SQL.  Override if this method is not compatible with your database platform.
        /// </summary>
        /// <param name="stringValue">The string value.</param>
        /// <returns>A string value formatted for use in a SQL statement.</returns>
        protected virtual string FormatStringValueForSql(string stringValue)
        {
            return $"'{stringValue.Replace("'", "''")}'";
        }

        protected virtual string GenerateOrderByClause(SelectQuery query)
        {
            if (!query.OrderBySegments.Any())
                return string.Empty;

            var sql = $"{SqlLinePrefix}ORDER BY ";
            var firstOrder = true;

            foreach (var orderBySegment in query.OrderBySegments)
            {
                var tableField = $"{FormatSqlObject(orderBySegment.Table.GetTableName())}.";
                tableField += $"{FormatSqlObject(orderBySegment.FieldName)}";
                var orderByFragment = string.Empty;
                var orderByLinePrefix = $"{SqlLinePrefix}\t";

                switch (orderBySegment.OrderBySegmentType)
                {
                    case OrderBySegmentTypes.General:
                        if (!firstOrder)
                            orderByFragment = orderByLinePrefix;

                        orderByFragment += tableField;
                        break;
                    case OrderBySegmentTypes.Formula:
                        if (orderBySegment is OrderByFormulaSegment orderByFormulaSegment)
                        {
                            if (firstOrder)
                                orderByFragment = "\r\n";

                            orderByFragment +=
                                FormatOrderByFormulaFragment(orderByFormulaSegment.Formula, orderByLinePrefix);
                        }
                        break;
                    case OrderBySegmentTypes.Enum:
                        if (orderBySegment is OrderByEnumSegment orderByEnumSegment)
                        {
                            if (firstOrder)
                                orderByFragment = "\r\n";

                            var enumText = GenerateEnumeratorSqlFieldText(tableField, orderByEnumSegment.EnumTranslation);
                            orderByFragment +=
                                FormatOrderByFormulaFragment(enumText, orderByLinePrefix);
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                firstOrder = false;
                orderByFragment = FormatOrderByTableFieldSql(orderByFragment, orderBySegment);
                switch (orderBySegment.OrderByType)
                {
                    case OrderByTypes.Ascending:
                        orderByFragment += " ASC,";
                        break;
                    case OrderByTypes.Descending:
                        orderByFragment += " DESC,";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                sql += $"{orderByFragment}\r\n";
            }

            sql = $"{sql.TrimRight(",\r\n")}\r\n";
            return sql;
        }

        /// <summary>
        /// Formats the order by fragment.  Used by derived classes to add case sensitivity and other SQL text to the order by Table.Field parameter.
        /// </summary>
        /// <param name="orderByTableFieldSql">The order by Table.Field SQL string.</param>
        /// <param name="orderBySegment">The order by segment object.</param>
        /// <returns>The formatted order by Table.Field SQL text.  If this method is not overriden, this returns the order by Table.Field SQL text parameter.</returns>
        public virtual string FormatOrderByTableFieldSql(string orderByTableFieldSql, OrderBySegment orderBySegment)
        {
            return orderByTableFieldSql;
        }

        private string FormatOrderByFormulaFragment(string formula, string selectLinePrefix)
        {
            var sql = $"{selectLinePrefix}(\r\n";
            selectLinePrefix += "\t";
            sql +=
                $"{selectLinePrefix}{FormatFormulaSqlText(formula, selectLinePrefix)}\r\n";
            selectLinePrefix = selectLinePrefix.TrimRight("\t");
            sql += $"{selectLinePrefix}) ";

            return sql;
        }

        protected virtual string GenerateGroupBy(IEnumerable<SelectColumn> distinctColumns)
        {
            var sql = $"{SqlLinePrefix}GROUP BY ";
            var firstGroup = true;
            foreach (var distinctColumn in distinctColumns)
            {
                var tableField = $"{FormatSqlObject(distinctColumn.Table.GetTableName())}.";
                tableField += $"{FormatSqlObject(distinctColumn.ColumnName)}";
                var groupByFragment = string.Empty;
                var groupByLinePrefix = $"{SqlLinePrefix}\t";

                switch (distinctColumn.ColumnType)
                {
                    case ColumnTypes.General:
                        if (!firstGroup)
                            groupByFragment = groupByLinePrefix;

                        groupByFragment += tableField;

                        break;
                    case ColumnTypes.Formula:
                        break;
                    case ColumnTypes.Enum:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                firstGroup = false;
                sql += $"{groupByFragment},\r\n";
            }
            sql = $"{sql.TrimRight(",\r\n")}\r\n";
            return sql;
        }

        public virtual string ConvertValueToSqlText(string value, ValueTypes valueType,DbDateTypes dateType)
        {
            if (value.IsNullOrEmpty())
                return "NULL";

            switch (valueType)
            {
                case ValueTypes.String:
                case ValueTypes.Memo:
                    return FormatStringValueForSql(value);
                case ValueTypes.Numeric:
                    return value;
                case ValueTypes.DateTime:
                    DateTime date = DateTime.MinValue;
                    if (DateTime.TryParse(value, out date))
                    {
                        switch (dateType)
                        {
                            case DbDateTypes.DateOnly:
                                value = date.ToString("yyyy-MM-dd");
                                break;
                            case DbDateTypes.DateTime:
                                value = date.ToString("yyyy-MM-dd hh:mm:ss");
                                break;
                            default:
                                throw new ArgumentOutOfRangeException(nameof(dateType), dateType, null);
                        }
                    }

                    return $"'{value}'";
                case ValueTypes.Bool:
                    var result = value.ToBool();
                    if (result)
                    {
                        return "1";
                    }
                    else
                    {
                        return "0";
                    }
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }
    }
}
