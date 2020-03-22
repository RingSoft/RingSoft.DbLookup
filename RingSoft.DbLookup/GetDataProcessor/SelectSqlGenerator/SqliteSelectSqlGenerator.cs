using System;
using RingSoft.DbLookup.QueryBuilder;

namespace RingSoft.DbLookup.GetDataProcessor.SelectSqlGenerator
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
        /// <returns></returns>
        protected override string GenerateTopRecordCountSqlText(SelectQuery query)
        {
            return string.Empty;
        }

        /// <summary>
        /// Generates the SELECT SQL statement.
        /// </summary>
        /// <param name="query">The QueryBuilder.Query object containing all the data for the SQL statement.</param>
        /// <param name="skipOrderBy">if set to <c>true</c> then don't generate the ORDER BY clause.</param>
        /// <returns></returns>
        protected override string GenerateSelectQueryStatement(SelectQuery query, bool skipOrderBy = false)
        {
            var sql = base.GenerateSelectQueryStatement(query, skipOrderBy);
            if (query.MaxRecords > 0)
                sql += $" LIMIT {query.MaxRecords}";

            return sql;
        }

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

        protected override string GenerateWhereEnumItemSqlText(WhereEnumItem whereItem)
        {
            var sql = base.GenerateWhereEnumItemSqlText(whereItem);

            if (!whereItem.CaseSensitive)
                sql += " COLLATE NOCASE";

            return sql;
        }

        protected override string GenerateWhereItemSqlFieldNameText(WhereItem whereItem)
        {
            var fieldName = base.GenerateWhereItemSqlFieldNameText(whereItem);
            switch (whereItem.ValueType)
            {
                case ValueTypes.DateTime:
                    switch (whereItem.DateType)
                    {
                        case DbDateTypes.DateOnly:
                            return $"date({fieldName})";
                        case DbDateTypes.DateTime:
                            return $"datetime({fieldName})";
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
            }
            return fieldName;
        }

        public override string FormatOrderByTableFieldSql(string orderByTableFieldSql, OrderBySegment orderBySegment)
        {
            if (!orderBySegment.CaseSensitive)
                return $"{orderByTableFieldSql} COLLATE NOCASE";

            return base.FormatOrderByTableFieldSql(orderByTableFieldSql, orderBySegment);
        }
    }
}
