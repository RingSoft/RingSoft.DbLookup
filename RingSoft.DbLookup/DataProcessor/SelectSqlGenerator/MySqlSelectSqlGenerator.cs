// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-16-2023
// ***********************************************************************
// <copyright file="MySqlSelectSqlGenerator.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.QueryBuilder;

namespace RingSoft.DbLookup.DataProcessor.SelectSqlGenerator
{
    /// <summary>
    /// Generates a SELECT SQL statement for the MySQL database platform.
    /// </summary>
    /// <seealso cref="DbSelectSqlGenerator" />
    public class MySqlSelectSqlGenerator : DbSelectSqlGenerator
    {
        /// <summary>
        /// Gets the SQL object prefix character.
        /// </summary>
        /// <value>The SQL object prefix character.</value>
        public override char SqlObjectPrefixChar => '`';

        /// <summary>
        /// Gets the SQL object suffix character.
        /// </summary>
        /// <value>The SQL object suffix character.</value>
        public override char SqlObjectSuffixChar => '`';

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
        /// Generates the where item SQL field name text for the inputted WhereItem object.
        /// Override if this method is not compatible with your database platform.
        /// </summary>
        /// <param name="whereItem">The WhereItem object containing all the data for the SQL statement.</param>
        /// <returns>System.String.</returns>
        protected override string GenerateWhereItemSqlFieldNameText(WhereItem whereItem)
        {
            var sql = base.GenerateWhereItemSqlFieldNameText(whereItem);
            switch (whereItem.ValueType)
            {
                case ValueTypes.String:
                case ValueTypes.Memo:
                    if (whereItem.CaseSensitive && !whereItem.Value.IsNullOrEmpty())
                        sql = $"BINARY({sql})";
                    break;
            }
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
                    if (whereItem.CaseSensitive && !whereItem.Value.IsNullOrEmpty())
                        sql = $"BINARY({sql})";
                    break;
            }
            return sql;
        }

        /// <summary>
        /// Formats the order by fragment.  Used by derived classes to add case sensitivity and other SQL text to the order by Table.Field parameter.
        /// </summary>
        /// <param name="orderByTableFieldSql">The order by Table.Field SQL string.</param>
        /// <param name="orderBySegment">The order by segment object.</param>
        /// <returns>The formatted order by Table.Field SQL text.  If this method is not overriden, this returns the order by Table.Field SQL text parameter.</returns>
        public override string FormatOrderByTableFieldSql(string orderByTableFieldSql, OrderBySegment orderBySegment)
        {
            if (orderBySegment.CaseSensitive)
                return $"BINARY({orderByTableFieldSql})";

            return base.FormatOrderByTableFieldSql(orderByTableFieldSql, orderBySegment);
        }
    }
}
