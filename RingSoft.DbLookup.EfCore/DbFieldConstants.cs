// ***********************************************************************
// Assembly         : RingSoft.DbLookup.EfCore
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-19-2022
// ***********************************************************************
// <copyright file="DbFieldConstants.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DbLookup.DataProcessor;

namespace RingSoft.DbLookup.EfCore
{
    /// <summary>
    /// Class DbFieldConstants.
    /// </summary>
    public abstract class DbFieldConstants
    {
        /// <summary>
        /// Gets the type of the column type for field.
        /// </summary>
        /// <param name="fieldType">Type of the field.</param>
        /// <returns>System.String.</returns>
        public abstract string GetColumnTypeForFieldType(DbFieldTypes fieldType);
    }
}
