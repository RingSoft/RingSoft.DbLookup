using System;
using RingSoft.DbLookup.DataProcessor;

namespace RingSoft.DbLookup.EfCore
{
    public class MySqlDbConstants : DbFieldConstants
    {
        public override string GetColumnTypeForFieldType(DbFieldTypes fieldType)
        {
            switch (fieldType)
            {
                case DbFieldTypes.Integer:
                    return "int";
                case DbFieldTypes.String:
                    return "varchar";
                case DbFieldTypes.Decimal:
                    return "double";
                case DbFieldTypes.DateTime:
                    return "datetime";
                case DbFieldTypes.Byte:
                    return "tinyint";
                case DbFieldTypes.Bool:
                    return "tinyint";
                case DbFieldTypes.Memo:
                    return "longtext";
                default:
                    throw new ArgumentOutOfRangeException(nameof(fieldType), fieldType, null);
            }
        }
    }
}
