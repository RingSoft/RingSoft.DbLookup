// ***********************************************************************
// Assembly         : RingSoft.DbLookup.EfCore
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 06-28-2023
// ***********************************************************************
// <copyright file="MySqlDbConstants.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using RingSoft.DbLookup.DataProcessor;

namespace RingSoft.DbLookup.EfCore
{
    /// <summary>
    /// Class MySqlDbConstants.
    /// Implements the <see cref="RingSoft.DbLookup.EfCore.DbFieldConstants" />
    /// </summary>
    /// <seealso cref="RingSoft.DbLookup.EfCore.DbFieldConstants" />
    public class MySqlDbConstants : DbFieldConstants
    {
        /// <summary>
        /// Gets the type of the column type for field.
        /// </summary>
        /// <param name="fieldType">Type of the field.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">fieldType - null</exception>
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
