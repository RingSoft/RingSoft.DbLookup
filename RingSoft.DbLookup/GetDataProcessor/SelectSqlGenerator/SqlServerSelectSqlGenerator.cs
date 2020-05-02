using RingSoft.DbLookup.QueryBuilder;

namespace RingSoft.DbLookup.GetDataProcessor.SelectSqlGenerator
{
    /// <summary>
    /// Generates a SELECT SQL statement for the Microsoft SQL Server database platform.
    /// </summary>
    /// <seealso cref="DbSelectSqlGenerator" />
    public class SqlServerSelectSqlGenerator : DbSelectSqlGenerator
    {
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

        public override string FormatOrderByTableFieldSql(string orderByTableFieldSql, OrderBySegment orderBySegment)
        {
            if (orderBySegment.CaseSensitive)
                return $"{orderByTableFieldSql} COLLATE Latin1_General_bin";

            return base.FormatOrderByTableFieldSql(orderByTableFieldSql, orderBySegment);
        }
    }
}
