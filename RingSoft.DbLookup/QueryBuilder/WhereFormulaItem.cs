// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 01-25-2023
// ***********************************************************************
// <copyright file="WhereFormulaItem.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace RingSoft.DbLookup.QueryBuilder
{
    /// <summary>
    /// A Where Item that searches the results of a formula.
    /// </summary>
    /// <seealso cref="WhereItem" />
    public class WhereFormulaItem : WhereItem
    {
        /// <summary>
        /// Gets the type of the where item.
        /// </summary>
        /// <value>The type of the where item.</value>
        public override WhereItemTypes WhereItemType => WhereItemTypes.Formula;

        /// <summary>
        /// Gets the formula.
        /// </summary>
        /// <value>The formula.</value>
        public string Formula { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether that this formula has no condition or value.
        /// </summary>
        /// <value><c>true</c> no condition or value; otherwise, <c>false</c>.</value>
        public bool NoValue { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WhereFormulaItem"/> class.
        /// </summary>
        internal WhereFormulaItem()
        {
            
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return Formula;
        }
    }
}
