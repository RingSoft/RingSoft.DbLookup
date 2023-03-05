using System;
using System.Windows.Controls;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;

// ReSharper disable once CheckNamespace
namespace RingSoft.DbLookup.Controls.WPF
{
    public class LookupSearchForHostFactory
    {
        public const int SearchForStringHostId = 0;
        public const int SearchForIntegerHostId = 1;
        public const int SearchForDecimalHostId = 2;
        public const int SearchForDateHostId = 3;

        internal LookupSearchForHost CreateSearchForHost(LookupColumnDefinitionBase columnDefinition)
        {
            var hostId = columnDefinition.SearchForHostId;
            if (hostId == null)
            {
                hostId = ConvertFieldDataTypeToSearchForHostId(columnDefinition.DataType);
                if (columnDefinition is LookupFieldColumnDefinition lookupFieldColumn
                    && lookupFieldColumn.FieldDefinition is IntegerFieldDefinition integerFieldDefinition
                    && integerFieldDefinition.EnumTranslation != null)
                    hostId = SearchForStringHostId;
            }

            var searchForHost = CreateSearchForHost(hostId);
            searchForHost.InternalInitialize(columnDefinition);

            return searchForHost;
        }

        internal LookupSearchForHost CreateSearchForHost(FieldDataTypes fieldDataType)
        {
            var hostId = ConvertFieldDataTypeToSearchForHostId(fieldDataType);
            var searchForHost = CreateSearchForHost(hostId);
            searchForHost.InternalInitialize();

            return searchForHost;

        }

        public static int ConvertFieldDataTypeToSearchForHostId(FieldDataTypes dataType)
        {
            switch (dataType)
            {
                case FieldDataTypes.String:
                case FieldDataTypes.Bool:
                    return SearchForStringHostId;
                case FieldDataTypes.Integer:
                    return SearchForIntegerHostId;
                case FieldDataTypes.Decimal:
                    return SearchForDecimalHostId;
                case FieldDataTypes.DateTime:
                    return SearchForDateHostId;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dataType), dataType, null);
            }
        }

        protected virtual LookupSearchForHost CreateSearchForHost(int? hostId)
        {
            if (hostId == SearchForDecimalHostId)
                return new LookupSearchForDecimalHost();
            if (hostId == SearchForDateHostId)
                return new LookupSearchForDateHost();
            if (hostId == SearchForIntegerHostId)
                return new LookupSearchForIntegerHost();

            return new LookupSearchForStringHost();
        }

        public virtual string FormatValue(int hostId, string value)
        {
            return value;
        }
    }
}
