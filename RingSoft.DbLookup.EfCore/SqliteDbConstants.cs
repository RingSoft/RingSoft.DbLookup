// ***********************************************************************
// Assembly         : RingSoft.DbLookup.EfCore
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-19-2022
// ***********************************************************************
// <copyright file="SqliteDbConstants.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using RingSoft.DbLookup.DataProcessor;

namespace RingSoft.DbLookup.EfCore
{
    /// <summary>
    /// Class SqliteDbConstants.
    /// Implements the <see cref="RingSoft.DbLookup.EfCore.DbFieldConstants" />
    /// </summary>
    /// <seealso cref="RingSoft.DbLookup.EfCore.DbFieldConstants" />
    public class SqliteDbConstants : DbFieldConstants
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
