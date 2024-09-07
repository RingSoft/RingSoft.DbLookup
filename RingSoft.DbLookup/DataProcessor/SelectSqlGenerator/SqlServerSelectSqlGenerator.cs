// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-16-2023
// ***********************************************************************
// <copyright file="SqlServerSelectSqlGenerator.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.QueryBuilder;

namespace RingSoft.DbLookup.DataProcessor.SelectSqlGenerator
{
    /// <summary>
    /// Generates a SELECT SQL statement for the Microsoft SQL Server database platform.
    /// </summary>
    /// <seealso cref="DbSelectSqlGenerator" />
    public class SqlServerSelectSqlGenerator : DbSelectSqlGenerator
    {
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
                        sql = $"CAST({sql} AS varbinary)";
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
                        sql = $"CAST({sql} AS varbinary)";
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
                return $"{orderByTableFieldSql} COLLATE Latin1_General_bin";

            return base.FormatOrderByTableFieldSql(orderByTableFieldSql, orderBySegment);
        }
    }
}
