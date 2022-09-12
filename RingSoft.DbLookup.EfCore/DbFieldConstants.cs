using RingSoft.DbLookup.DataProcessor;

namespace RingSoft.DbLookup.EfCore
{
    public abstract class DbFieldConstants
    {
        public abstract string GetColumnTypeForFieldType(DbFieldTypes fieldType);
    }
}
