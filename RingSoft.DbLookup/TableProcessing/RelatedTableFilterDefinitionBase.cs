using RingSoft.DbLookupCore.ModelDefinition;
using RingSoft.DbLookupCore.ModelDefinition.FieldDefinitions;

namespace RingSoft.DbLookupCore.TableProcessing
{
    /// <summary>
    /// A related table filter not based on an entity.
    /// </summary>
    public class RelatedTableFilterDefinitionBase
    {
        /// <summary>
        /// Gets the table field join definition.
        /// </summary>
        /// <value>
        /// The table field join definition.
        /// </value>
        public TableFieldJoinDefinition TableFieldJoinDefinition { get; private set; }

        /// <summary>
        /// Gets the table filter definition.
        /// </summary>
        /// <value>
        /// The table filter definition.
        /// </value>
        public TableFilterDefinitionBase TableFilterDefinition { get; }

        /// <summary>
        /// Gets the table definition.
        /// </summary>
        /// <value>
        /// The table definition.
        /// </value>
        public TableDefinitionBase TableDefinition { get; private set; }

        protected internal RelatedTableFilterDefinitionBase(TableFilterDefinitionBase tableFilterDefinition)
        {
            TableFilterDefinition = tableFilterDefinition;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelatedTableFilterDefinitionBase"/> class.
        /// </summary>
        /// <param name="tableFilterDefinition">The table filter definition.</param>
        /// <param name="foreignFieldDefinition">The foreign field definition.</param>
        /// <param name="parentAlias">The parent alias.</param>
        public RelatedTableFilterDefinitionBase(TableFilterDefinitionBase tableFilterDefinition, FieldDefinition foreignFieldDefinition, string parentAlias = "")
        {
            TableFilterDefinition = tableFilterDefinition;
            SetJoin(foreignFieldDefinition, parentAlias);
        }

        protected internal void SetJoin(FieldDefinition foreignFieldDefinition, string parentAlias)
        {
            TableFieldJoinDefinition = new TableFieldJoinDefinition
            {
                ForeignKeyDefinition = foreignFieldDefinition.ParentJoinForeignKeyDefinition,
                ParentAlias = parentAlias
            };
            TableFilterDefinition.AddJoin(TableFieldJoinDefinition);
            TableDefinition = foreignFieldDefinition.ParentJoinForeignKeyDefinition.PrimaryTable;
        }

        /// <summary>
        /// Includes the specified foreign field definition.
        /// </summary>
        /// <param name="foreignFieldDefinition">The foreign field definition.</param>
        /// <returns></returns>
        public RelatedTableFilterDefinitionBase Include(FieldDefinition foreignFieldDefinition)
        {
            var returnRelatedTableFilter = new RelatedTableFilterDefinitionBase(TableFilterDefinition,
                foreignFieldDefinition, TableFieldJoinDefinition.Alias);

            return returnRelatedTableFilter;
        }

    }
}
