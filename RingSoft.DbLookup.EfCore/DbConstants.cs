using RingSoft.DbLookup.DataProcessor;

namespace RingSoft.DbLookup.EfCore
{
    public class DbConstants
    {
        //public const string IntegerColumnType = "integer";
        //public const string StringColumnType = "nvarchar";
        //public const string DecimalColumnType = "numeric";
        //public const string DateColumnType = "datetime";
        //public const string ByteColumnType = "tinyint";
        //public const string BoolColumnType = "bit";
        //public const string MemoColumnType = "ntext";

        public static string IntegerColumnType
        {
            get => SystemGlobals.AdvancedFindLookupContext.Context.DataProcessor.GetColumnTypeForFieldType(DbFieldTypes
                .Integer);
        }

        public static string StringColumnType
        {
            get => SystemGlobals.AdvancedFindLookupContext.Context.DataProcessor.GetColumnTypeForFieldType(DbFieldTypes
                .String);
        }

        public static string DecimalColumnType
        {
            get => SystemGlobals.AdvancedFindLookupContext.Context.DataProcessor.GetColumnTypeForFieldType(DbFieldTypes
                .Decimal);
        }

        public static string DateColumnType
        {
            get => SystemGlobals.AdvancedFindLookupContext.Context.DataProcessor.GetColumnTypeForFieldType(DbFieldTypes
                .DateTime);
        }

        public static string ByteColumnType
        {
            get => SystemGlobals.AdvancedFindLookupContext.Context.DataProcessor.GetColumnTypeForFieldType(DbFieldTypes
                .Byte);
        }

        public static string BoolColumnType
        {
            get => SystemGlobals.AdvancedFindLookupContext.Context.DataProcessor.GetColumnTypeForFieldType(DbFieldTypes
                .Bool);
        }

        public static string MemoColumnType
        {
            get => SystemGlobals.AdvancedFindLookupContext.Context.DataProcessor.GetColumnTypeForFieldType(DbFieldTypes
                .Memo);
        }

    }
}
