// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-16-2023
// ***********************************************************************
// <copyright file="QueryTable.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DataEntryControls.Engine;

namespace RingSoft.DbLookup.QueryBuilder
{
    /// <summary>
    /// This class represents a table name that appears after the FROM or JOIN keyword.
    /// </summary>
    public class QueryTable
    {
        /// <summary>
        /// Gets the base query object that this table is assigned to.
        /// </summary>
        /// <value>The base query object that this table is assigned to..</value>
        public SelectQuery Query { get; internal set; }

        /// <summary>
        /// Gets the name of the table.
        /// </summary>
        /// <value>The name of the table.</value>
        public string Name { get; internal set; }

        /// <summary>
        /// Gets the alias of the table.
        /// </summary>
        /// <value>This value appears after the "AS" keyword in a JOIN clause.</value>
        public string Alias { get; internal protected set; }

        /// <summary>
        /// Gets the formula.
        /// </summary>
        /// <value>The formula.</value>
        public string Formula { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryTable" /> class.
        /// </summary>
        internal QueryTable()
        {
            
        }
        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            if (Alias.IsNullOrEmpty())
                return Name;

            return Alias;
        }

        /// <summary>
        /// Gets the name of the table to use on the left side of the "." in "[].[]" SQL fragment.
        /// </summary>
        /// <returns>If Alias is not empty, then it is returned.  Else Name is returned.</returns>
        public string GetTableName()
        {
            if (!Alias.IsNullOrEmpty())
                return Alias;

            return Name;
        }

        /// <summary>
        /// Determines whether the specified formula has formula.
        /// </summary>
        /// <param name="formula">The formula.</param>
        /// <returns>QueryTable.</returns>
        public QueryTable HasFormula(string formula)
        {
            Formula = formula;
            return this;
        }
    }
}
