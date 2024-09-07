// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-16-2023
// ***********************************************************************
// <copyright file="SelectFormulaColumn.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace RingSoft.DbLookup.QueryBuilder
{
    /// <summary>
    /// Class SelectFormulaColumn.
    /// Implements the <see cref="RingSoft.DbLookup.QueryBuilder.SelectColumn" />
    /// </summary>
    /// <seealso cref="RingSoft.DbLookup.QueryBuilder.SelectColumn" />
    public class SelectFormulaColumn : SelectColumn
    {
        /// <summary>
        /// Gets the type of the column.
        /// </summary>
        /// <value>The type of the column.</value>
        public override ColumnTypes ColumnType => ColumnTypes.Formula;

        /// <summary>
        /// Gets the formula.
        /// </summary>
        /// <value>The formula.</value>
        public string Formula { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectFormulaColumn" /> class.
        /// </summary>
        internal SelectFormulaColumn()
        {
            
        }
    }
}
