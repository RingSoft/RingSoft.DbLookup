// ***********************************************************************
// Assembly         : RingSoft.DbLookup.Controls.WPF
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-16-2023
// ***********************************************************************
// <copyright file="LookupSearchForHostFactory.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Windows.Controls;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;

// ReSharper disable once CheckNamespace
namespace RingSoft.DbLookup.Controls.WPF
{
    /// <summary>
    /// Class LookupSearchForHostFactory.
    /// </summary>
    public class LookupSearchForHostFactory
    {
        /// <summary>
        /// The search for string host identifier
        /// </summary>
        public const int SearchForStringHostId = 0;
        /// <summary>
        /// The search for integer host identifier
        /// </summary>
        public const int SearchForIntegerHostId = 1;
        /// <summary>
        /// The search for decimal host identifier
        /// </summary>
        public const int SearchForDecimalHostId = 2;
        /// <summary>
        /// The search for date host identifier
        /// </summary>
        public const int SearchForDateHostId = 3;

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupSearchForHostFactory"/> class.
        /// </summary>
        public LookupSearchForHostFactory()
        {
            LookupControlsGlobals.LookupControlSearchForFactory = this;
        }
        /// <summary>
        /// Creates the search for host.
        /// </summary>
        /// <param name="columnDefinition">The column definition.</param>
        /// <returns>LookupSearchForHost.</returns>
        internal LookupSearchForHost CreateSearchForHost(LookupColumnDefinitionBase columnDefinition)
        {
            var hostId = columnDefinition.SearchForHostId;
            if (hostId == null)
            {
                hostId = ConvertFieldDataTypeToSearchForHostId(columnDefinition.DataType);
                if (columnDefinition is LookupFieldColumnDefinition lookupFieldColumn
                    && lookupFieldColumn.FieldDefinition is IntegerFieldDefinition integerFieldDefinition
                    && integerFieldDefinition.EnumTranslation != null)
                    hostId = GblMethods.SearchForEnumHostId;
            }

            var searchForHost = CreateSearchForHost(hostId);
            searchForHost.InternalInitialize(columnDefinition);

            return searchForHost;
        }

        /// <summary>
        /// Creates the search for host.
        /// </summary>
        /// <param name="fieldDataType">Type of the field data.</param>
        /// <returns>LookupSearchForHost.</returns>
        internal LookupSearchForHost CreateSearchForHost(FieldDataTypes fieldDataType)
        {
            var hostId = ConvertFieldDataTypeToSearchForHostId(fieldDataType);
            var searchForHost = CreateSearchForHost(hostId);
            searchForHost.InternalInitialize();

            return searchForHost;

        }

        /// <summary>
        /// Converts the field data type to search for host identifier.
        /// </summary>
        /// <param name="dataType">Type of the data.</param>
        /// <returns>System.Int32.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">dataType - null</exception>
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

        /// <summary>
        /// Creates the search for host.
        /// </summary>
        /// <param name="hostId">The host identifier.</param>
        /// <returns>LookupSearchForHost.</returns>
        protected virtual LookupSearchForHost CreateSearchForHost(int? hostId)
        {
            if (hostId == SearchForDecimalHostId)
                return new LookupSearchForDecimalHost();
            if (hostId == SearchForDateHostId)
                return new LookupSearchForDateHost();
            if (hostId == SearchForIntegerHostId)
                return new LookupSearchForIntegerHost();
            if (hostId == GblMethods.SearchForEnumHostId)
            {
                return new LookupSearchForEnumHost();
            }
            return new LookupSearchForStringHost();
        }

        /// <summary>
        /// Formats the value.
        /// </summary>
        /// <param name="hostId">The host identifier.</param>
        /// <param name="value">The value.</param>
        /// <returns>System.String.</returns>
        public virtual string FormatValue(int hostId, string value)
        {
            return value;
        }
    }
}
