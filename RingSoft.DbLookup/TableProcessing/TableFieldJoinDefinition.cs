using RingSoft.DbLookup.ModelDefinition;

namespace RingSoft.DbLookup.TableProcessing
{
    /// <summary>
    /// Defines how 2 fields are linked in a lookup definition.
    /// </summary>
    public class TableFieldJoinDefinition
    {
        /// <summary>
        /// Gets the foreign key definition.
        /// </summary>
        /// <value>
        /// The foreign key definition.
        /// </value>
        public ForeignKeyDefinition ForeignKeyDefinition { get; internal set; }

        /// <summary>
        /// Gets the alias used in the SQL statement.
        /// </summary>
        /// <value>
        /// The alias.
        /// </value>
        public string Alias => ForeignKeyDefinition.Alias;

        /// <summary>
        /// Gets the parent alias used in the SQL string.
        /// </summary>
        /// <value>
        /// The parent alias.
        /// </value>
        public string ParentAlias { get; internal set; }

        public override string ToString()
        {
            return Alias;
        }

        internal void CopyFrom(TableFieldJoinDefinition source)
        {
            ForeignKeyDefinition = source.ForeignKeyDefinition;
            ParentAlias = source.ParentAlias;
        }
    }
}
