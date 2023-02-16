using System.Collections.Generic;

namespace RingSoft.DbLookup.QueryBuilder
{
    public class SqlData
    {
        public string FieldName { get; private set; }

        public string FieldValue { get; private set;}

        public ValueTypes ValueType { get; private set; }
    }

    public class SqlDataStatement
    {
        public string TableName { get; private set; }

        public List<SqlData> SqlDatas { get; private set; } = new List<SqlData>();
    }
}
