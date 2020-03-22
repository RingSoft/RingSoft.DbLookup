using RingSoft.DbLookupCore.QueryBuilder;

namespace RingSoft.DbLookupCore.GetDataProcessor.SelectSqlGenerator
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
        /// <value>
        /// The SQL object prefix character.
        /// </value>
        protected override char SqlObjectPrefixChar => '`';

        /// <summary>
        /// Gets the SQL object suffix character.
        /// </summary>
        /// <value>
        /// The SQL object suffix character.
        /// </value>
        protected override char SqlObjectSuffixChar => '`';

        public MySqlSelectSqlGenerator()
        {
            
        }

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

        public override string FormatOrderByTableFieldSql(string orderByTableFieldSql, OrderBySegment orderBySegment)
        {
            if (orderBySegment.CaseSensitive)
                return $"BINARY({orderByTableFieldSql})";

            return base.FormatOrderByTableFieldSql(orderByTableFieldSql, orderBySegment);
        }
    }
}
