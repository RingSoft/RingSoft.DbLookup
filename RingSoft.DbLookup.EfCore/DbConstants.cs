using System;
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

        public static DbFieldConstants ConstantGenerator { get; set; }

        public static string IntegerColumnType
        {
            get
            {
                if (ConstantGenerator == null)
                {
                    throw new ApplicationException(
                        $"{nameof(DbConstants)}.{nameof(DbConstants.ConstantGenerator)} not set.");
                }
                return ConstantGenerator.GetColumnTypeForFieldType(DbFieldTypes
                    .Integer);
            } 
        }

        public static string StringColumnType
        {
            get
            {
                if (ConstantGenerator == null)
                {
                    throw new ApplicationException(
                        $"{nameof(DbConstants)}.{nameof(DbConstants.ConstantGenerator)} not set.");
                }
                return ConstantGenerator.GetColumnTypeForFieldType(DbFieldTypes
                    .String);
            }
        }

        public static string DecimalColumnType
        {
            get
            {
                if (ConstantGenerator == null)
                {
                    throw new ApplicationException(
                        $"{nameof(DbConstants)}.{nameof(DbConstants.ConstantGenerator)} not set.");
                }
                return ConstantGenerator.GetColumnTypeForFieldType(DbFieldTypes
                    .Decimal);
            }
        }

        public static string DateColumnType
        {
            get
            {
                if (ConstantGenerator == null)
                {
                    throw new ApplicationException(
                        $"{nameof(DbConstants)}.{nameof(DbConstants.ConstantGenerator)} not set.");
                }
                return ConstantGenerator.GetColumnTypeForFieldType(DbFieldTypes
                    .DateTime);
            }
        }

        public static string ByteColumnType
        {
            get
            {
                if (ConstantGenerator == null)
                {
                    throw new ApplicationException(
                        $"{nameof(DbConstants)}.{nameof(DbConstants.ConstantGenerator)} not set.");
                }
                return ConstantGenerator.GetColumnTypeForFieldType(DbFieldTypes
                    .Byte);
            }
        }

        public static string BoolColumnType
        {
            get
            {
                if (ConstantGenerator == null)
                {
                    throw new ApplicationException(
                        $"{nameof(DbConstants)}.{nameof(DbConstants.ConstantGenerator)} not set.");
                }
                return ConstantGenerator.GetColumnTypeForFieldType(DbFieldTypes
                    .Bool);
            }
        }

        public static string MemoColumnType
        {
            get
            {
                if (ConstantGenerator == null)
                {
                    throw new ApplicationException(
                        $"{nameof(DbConstants)}.{nameof(DbConstants.ConstantGenerator)} not set.");
                }
                return ConstantGenerator.GetColumnTypeForFieldType(DbFieldTypes
                    .Memo);
            }
        }

    }
}
