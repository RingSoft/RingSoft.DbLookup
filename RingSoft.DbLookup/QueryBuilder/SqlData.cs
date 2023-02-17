using System.Collections.Generic;
using RingSoft.DbLookup.ModelDefinition;

namespace RingSoft.DbLookup.QueryBuilder
{
    public class SqlData
    {
        public string FieldName { get; private set; }

        public string FieldValue { get; private set;}

        public ValueTypes ValueType { get; private set; }

        public DbDateTypes DateType { get; private set; }

        public SqlData(string fieldName, string fieldValue, ValueTypes valueType, DbDateTypes dateType = DbDateTypes.DateOnly)
        {
            FieldName = fieldName;
            FieldValue = fieldValue;
            ValueType = valueType;
            DateType = dateType;
        }
    }

    public class UpdateDataStatement
    {
        public IReadOnlyList<SqlData> SqlDatas  => _sqlDatas.AsReadOnly();

        public PrimaryKeyValue PrimaryKeyValue { get; private set; }

        private List<SqlData> _sqlDatas = new List<SqlData>();

        public UpdateDataStatement(PrimaryKeyValue primaryKeyValue)
        {
            PrimaryKeyValue = primaryKeyValue;
        }

        public void AddSqlData(SqlData sqlData)
        {
            _sqlDatas.Add(sqlData);
        }
    }

    public class InsertDataStatement
    {
        public TableDefinitionBase TableDefinition { get; private set; }

        public IReadOnlyList<SqlData> SqlDatas => _sqlDatas.AsReadOnly();

        private List<SqlData> _sqlDatas = new List<SqlData>();

        public InsertDataStatement(TableDefinitionBase tableDefinition)
        {
            TableDefinition = tableDefinition;
        }
        public void AddSqlData(SqlData sqlData)
        {
            _sqlDatas.Add(sqlData);
        }

    }
}
