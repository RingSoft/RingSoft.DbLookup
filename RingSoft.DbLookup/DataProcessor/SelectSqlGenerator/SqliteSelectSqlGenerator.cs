// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-16-2023
// ***********************************************************************
// <copyright file="SqliteSelectSqlGenerator.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DataEntryControls.Engine;

namespace RingSoft.DbLookup.DataProcessor.SelectSqlGenerator
{
    /// <summary>
    /// Generates a SELECT SQL statement for the Sqlite database platform.
    /// </summary>
    /// <seealso cref="DbSelectSqlGenerator" />
    public class SqliteSelectSqlGenerator : DbSelectSqlGenerator
    {
        /// <summary>
        /// Generates the top record count SQL text.  Override if this method is not compatible with your database platform.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>System.String.</returns>
        protected override string GenerateTopRecordCountSqlText(SelectQuery query)
        {
            return string.Empty;
        }

        /// <summary>
        /// Generates the SELECT SQL statement.
        /// </summary>
        /// <param name="query">The QueryBuilder.Query object containing all the data for the SQL statement.</param>
        /// <param name="skipOrderBy">if set to <c>true</c> then don't generate the ORDER BY clause.</param>
        /// <param name="count">The count.</param>
        /// <returns>System.String.</returns>
        protected override string GenerateSelectQueryStatement(SelectQuery query, bool skipOrderBy = false, int? count = null)
        {
            var sql = base.GenerateSelectQueryStatement(query, skipOrderBy, count);
            if (query.MaxRecords > 0 && count == null)
                sql += $" LIMIT {query.MaxRecords}";

            return sql;
        }

        /// <summary>
        /// Formats a value for a SQL WHERE clause.  Override if this method is not compatible with your database platform.
        /// </summary>
        /// <param name="whereItem">The WhereItem object containing all the data to format the value.</param>
        /// <returns>A formatted value for use in a WHERE clause.</returns>
        protected override string FormatValueForSqlWhereItem(WhereItem whereItem)
        {
            var sql = base.FormatValueForSqlWhereItem(whereItem);
            switch (whereItem.ValueType)
            {
                case ValueTypes.String:
                case ValueTypes.Memo:
                    if (!whereItem.CaseSensitive)
                    {
                        if (!sql.IsNullOrEmpty())
                            sql += " COLLATE NOCASE";
                    }
                    break;
            }
            return sql;
        }

        /// <summary>
        /// Generates the where item enumerator SQL text.
        /// </summary>
        /// <param name="whereItem">The where item.</param>
        /// <returns>System.String.</returns>
        protected override string GenerateWhereEnumItemSqlText(WhereEnumItem whereItem)
        {
            var sql = base.GenerateWhereEnumItemSqlText(whereItem);

            if (!whereItem.CaseSensitive)
                sql += " COLLATE NOCASE";

            return sql;
        }

        /// <summary>
        /// Generates the where item SQL field name text for the inputted WhereItem object.
        /// Override if this method is not compatible with your database platform.
        /// </summary>
        /// <param name="whereItem">The WhereItem object containing all the data for the SQL statement.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        protected override string GenerateWhereItemSqlFieldNameText(WhereItem whereItem)
        {
            var fieldName = base.GenerateWhereItemSqlFieldNameText(whereItem);
            if (whereItem is WhereFormulaItem whereFormulaItem)
            {
                fieldName = whereFormulaItem.Formula;
            }
            switch (whereItem.ValueType)
            {
                case ValueTypes.DateTime:
                    switch (whereItem.DateType)
                    {
                        case DbDateTypes.DateOnly:
                            return $"date({fieldName})";
                        case DbDateTypes.DateTime:
                        case DbDateTypes.Millisecond:
                            return $"datetime({fieldName})";
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
            }
            return fieldName;
        }

        /// <summary>
        /// Formats the order by fragment.  Used by derived classes to add case sensitivity and other SQL text to the order by Table.Field parameter.
        /// </summary>
        /// <param name="orderByTableFieldSql">The order by Table.Field SQL string.</param>
        /// <param name="orderBySegment">The order by segment object.</param>
        /// <returns>The formatted order by Table.Field SQL text.  If this method is not overriden, this returns the order by Table.Field SQL text parameter.</returns>
        public override string FormatOrderByTableFieldSql(string orderByTableFieldSql, OrderBySegment orderBySegment)
        {
            if (!orderBySegment.CaseSensitive)
            {
                return $"{orderByTableFieldSql} COLLATE NOCASE";
            }

            return base.FormatOrderByTableFieldSql(orderByTableFieldSql, orderBySegment);
        }
    }
}
