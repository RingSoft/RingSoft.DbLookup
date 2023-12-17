// ***********************************************************************
// Assembly         : RingSoft.DbLookup.EfCore
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 11-23-2023
// ***********************************************************************
// <copyright file="DbConstants.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using RingSoft.DbLookup.DataProcessor;

namespace RingSoft.DbLookup.EfCore
{
    /// <summary>
    /// Generates Entity Framework Property column string names HasColumnType.
    /// </summary>
    public class DbConstants
    {
        /// <summary>
        /// Gets or sets the constant generator.
        /// </summary>
        /// <value>The constant generator.</value>
        public static DbFieldConstants ConstantGenerator { get; set; }

        /// <summary>
        /// Gets the type of the integer column.
        /// </summary>
        /// <value>The type of the integer column.</value>
        /// <exception cref="System.ApplicationException"></exception>
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

        /// <summary>
        /// Gets the type of the string column.
        /// </summary>
        /// <value>The type of the string column.</value>
        /// <exception cref="System.ApplicationException"></exception>
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

        /// <summary>
        /// Gets the type of the decimal column.
        /// </summary>
        /// <value>The type of the decimal column.</value>
        /// <exception cref="System.ApplicationException"></exception>
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

        /// <summary>
        /// Gets the type of the date column.
        /// </summary>
        /// <value>The type of the date column.</value>
        /// <exception cref="System.ApplicationException"></exception>
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

        /// <summary>
        /// Gets the type of the byte column.
        /// </summary>
        /// <value>The type of the byte column.</value>
        /// <exception cref="System.ApplicationException"></exception>
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

        /// <summary>
        /// Gets the type of the bool column.
        /// </summary>
        /// <value>The type of the bool column.</value>
        /// <exception cref="System.ApplicationException"></exception>
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

        /// <summary>
        /// Gets the type of the memo column.
        /// </summary>
        /// <value>The type of the memo column.</value>
        /// <exception cref="System.ApplicationException"></exception>
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
