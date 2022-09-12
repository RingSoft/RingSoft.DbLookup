using System;
using RingSoft.DbLookup.DataProcessor;

namespace RingSoft.DbLookup.EfCore
{
    public class SqliteDbConstants : DbFieldConstants
    {
        public override string GetColumnTypeForFieldType(DbFieldTypes fieldType)
        {
            switch (fieldType)
            {
                case DbFieldTypes.Integer:
                    return "integer";
                case DbFieldTypes.String:
                    return "nvarchar";
                case DbFieldTypes.Decimal:
                    return "numeric";
                case DbFieldTypes.DateTime:
                    return "datetime";
                case DbFieldTypes.Byte:
                    return "smallint";
                case DbFieldTypes.Bool:
                    return "bit";
                case DbFieldTypes.Memo:
                    return "ntext";
                default:
                    throw new ArgumentOutOfRangeException(nameof(fieldType), fieldType, null);
            }
        }
    }
}
