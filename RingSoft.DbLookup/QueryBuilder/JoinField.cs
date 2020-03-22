namespace RingSoft.DbLookup.QueryBuilder
{
    public class JoinField
    {
        /// <summary>
        /// Gets the primary table.
        /// </summary>
        /// <value>
        /// The primary table is the "One" table in a one to many join.
        /// </value>
        public PrimaryJoinTable PrimaryTable { get; internal set; }

        /// <summary>
        /// Gets the primary field.
        /// </summary>
        /// <value>
        /// The primary field.
        /// </value>
        public string PrimaryField { get; internal set; }

        /// <summary>
        /// Gets the foreign field.
        /// </summary>
        /// <value>
        /// The foreign field.
        /// </value>
        public string ForeignField { get; internal set; }

        internal JoinField()
        {
            
        }
    }
}
