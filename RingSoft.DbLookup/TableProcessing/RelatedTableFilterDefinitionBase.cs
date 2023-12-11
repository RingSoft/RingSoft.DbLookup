// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-19-2022
// ***********************************************************************
// <copyright file="RelatedTableFilterDefinitionBase.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;

namespace RingSoft.DbLookup.TableProcessing
{
    /// <summary>
    /// A related table filter not based on an entity.
    /// </summary>
    public class RelatedTableFilterDefinitionBase
    {
        /// <summary>
        /// Gets the table field join definition.
        /// </summary>
        /// <value>The table field join definition.</value>
        public TableFieldJoinDefinition TableFieldJoinDefinition { get; private set; }

        /// <summary>
        /// Gets the table filter definition.
        /// </summary>
        /// <value>The table filter definition.</value>
        public TableFilterDefinitionBase TableFilterDefinition { get; }

        /// <summary>
        /// Gets the table definition.
        /// </summary>
        /// <value>The table definition.</value>
        public TableDefinitionBase TableDefinition { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelatedTableFilterDefinitionBase"/> class.
        /// </summary>
        /// <param name="tableFilterDefinition">The table filter definition.</param>
        protected internal RelatedTableFilterDefinitionBase(TableFilterDefinitionBase tableFilterDefinition)
        {
            TableFilterDefinition = tableFilterDefinition;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelatedTableFilterDefinitionBase" /> class.
        /// </summary>
        /// <param name="tableFilterDefinition">The table filter definition.</param>
        /// <param name="foreignFieldDefinition">The foreign field definition.</param>
        /// <param name="parentAlias">The parent alias.</param>
        public RelatedTableFilterDefinitionBase(TableFilterDefinitionBase tableFilterDefinition, FieldDefinition foreignFieldDefinition, string parentAlias = "")
        {
            TableFilterDefinition = tableFilterDefinition;
            SetJoin(foreignFieldDefinition, parentAlias);
        }

        /// <summary>
        /// Sets the join.
        /// </summary>
        /// <param name="foreignFieldDefinition">The foreign field definition.</param>
        /// <param name="parentAlias">The parent alias.</param>
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
        /// <returns>RelatedTableFilterDefinitionBase.</returns>
        public RelatedTableFilterDefinitionBase Include(FieldDefinition foreignFieldDefinition)
        {
            var returnRelatedTableFilter = new RelatedTableFilterDefinitionBase(TableFilterDefinition,
                foreignFieldDefinition, TableFieldJoinDefinition.Alias);

            return returnRelatedTableFilter;
        }

    }
}
