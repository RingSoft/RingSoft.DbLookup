// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-19-2022
// ***********************************************************************
// <copyright file="SelectColumn.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DataEntryControls.Engine;

namespace RingSoft.DbLookup.QueryBuilder
{
    /// <summary>
    /// Enum ColumnTypes
    /// </summary>
    public enum ColumnTypes
    {
        /// <summary>
        /// The general
        /// </summary>
        General = 0,
        /// <summary>
        /// The formula
        /// </summary>
        Formula = 1,
        /// <summary>
        /// The enum
        /// </summary>
        Enum = 2
    }

    /// <summary>
    /// This represents a column as part of a "SELECT" clause.
    /// </summary>
    public class SelectColumn
    {
        /// <summary>
        /// Gets the type of the column.
        /// </summary>
        /// <value>The type of the column.</value>
        public virtual ColumnTypes ColumnType => ColumnTypes.General;

        /// <summary>
        /// Gets the table object this select column is attached to.
        /// </summary>
        /// <value>The table object this select column is attached to.</value>
        public QueryTable Table { get; internal set; }

        /// <summary>
        /// Gets the name of the column.
        /// </summary>
        /// <value>The name of the column.</value>
        public string ColumnName { get; internal set; }

        /// <summary>
        /// Gets the alias of the column.
        /// </summary>
        /// <value>This value appears after the "AS" keyword in a SELECT clause.</value>
        public string Alias { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether this column is distinct.
        /// </summary>
        /// <value><c>true</c> if this column is distinct; otherwise, <c>false</c>.</value>
        public bool IsDistinct { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectColumn"/> class.
        /// </summary>
        internal SelectColumn()
        {
            
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            if (Alias.IsNullOrEmpty())
                return ColumnName;

            return Alias;
        }
    }
}
