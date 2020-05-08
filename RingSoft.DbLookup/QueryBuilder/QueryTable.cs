namespace RingSoft.DbLookup.QueryBuilder
{
    /// <summary>This class represents a table name that appears after the FROM or JOIN keyword.</summary>
    public class QueryTable
    {
        /// <summary>Gets the base query object that this table is assigned to.</summary>
        /// <value>  The base query object that this table is assigned to..</value>
        public SelectQuery Query { get; internal set; }

        /// <summary>Gets the name of the table.</summary>
        /// <value>The name of the table.</value>
        public string Name { get; internal set; }

        /// <summary>Gets the alias of the table.</summary>
        /// <value>  This value appears after the "AS" keyword in a JOIN clause.</value>
        public string Alias { get; internal protected set; }

        internal QueryTable()
        {
            
        }
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
    }
}
