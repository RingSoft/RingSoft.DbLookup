﻿namespace RingSoft.DbLookup.QueryBuilder
{
    public enum ColumnTypes
    {
        General = 0,
        Formula = 1,
        Enum = 2
    }

    /// <summary>This represents a column as part of a "SELECT" clause.</summary>
    public class SelectColumn
    {
        /// <summary>
        /// Gets the type of the column.
        /// </summary>
        /// <value>
        /// The type of the column.
        /// </value>
        public virtual ColumnTypes ColumnType => ColumnTypes.General;

        /// <summary>Gets the table object this select column is attached to.</summary>
        /// <value> The table object this select column is attached to.</value>
        public QueryTable Table { get; internal set; }

        /// <summary>Gets the name of the column.</summary>
        /// <value>The name of the column.</value>
        public string ColumnName { get; internal set; }

        /// <summary>Gets the alias of the column.</summary>
        /// <value>  This value appears after the "AS" keyword in a SELECT clause.</value>
        public string Alias { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether this column is distinct.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this column is distinct; otherwise, <c>false</c>.
        /// </value>
        public bool IsDistinct { get; internal set; }

        internal SelectColumn()
        {
            
        }

        public override string ToString()
        {
            if (Alias.IsNullOrEmpty())
                return ColumnName;

            return Alias;
        }
    }
}
