using System.Collections.Generic;
using RingSoft.DbLookupCore.ModelDefinition.FieldDefinitions;

namespace RingSoft.DbLookupCore.ModelDefinition
{
    /// <summary>
    /// Joins two tables together.
    /// </summary>
    public class ForeignKeyDefinition
    {
        /// <summary>
        /// Gets the primary ("One") table.
        /// </summary>
        /// <value>
        /// The primary table.
        /// </value>
        public TableDefinitionBase PrimaryTable { get; internal set; }

        /// <summary>
        /// Gets the foreign ("Many") table.
        /// </summary>
        /// <value>
        /// The foreign table.
        /// </value>
        public TableDefinitionBase ForeignTable { get; internal set; }

        /// <summary>
        /// Gets the fields that join the primary and foreign tables together..
        /// </summary>
        /// <value>
        /// The field joins.
        /// </value>
        public IReadOnlyList<ForeignKeyFieldJoin> FieldJoins => _foreignKeyFieldJoins;

        /// <summary>
        /// Gets the alias used in the SQL statement.
        /// </summary>
        /// <value>
        /// The alias.
        /// </value>
        public string Alias =>
            $"{ForeignTable.TableName}_{PrimaryTable.TableName}_{FieldJoins[0].ForeignField.FieldName}";

        /// <summary>
        /// Gets the name of the foreign object property.
        /// </summary>
        /// <value>
        /// The name of the foreign object property.
        /// </value>
        public string ForeignObjectPropertyName { get; internal set; }

        private readonly List<ForeignKeyFieldJoin> _foreignKeyFieldJoins = new List<ForeignKeyFieldJoin>();

        internal ForeignKeyDefinition()
        {
        }

        /// <summary>
        /// Adds a field join to the FieldJoins list.
        /// </summary>
        /// <param name="primaryField">The primary field.</param>
        /// <param name="foreignField">The foreign field.</param>
        /// <returns>This object.</returns>
        public ForeignKeyDefinition AddFieldJoin(FieldDefinition primaryField, FieldDefinition foreignField)
        {
            var foreignKeyFieldJoin = new ForeignKeyFieldJoin
            {
                PrimaryField = primaryField,
                ForeignField = foreignField
            };
            _foreignKeyFieldJoins.Add(foreignKeyFieldJoin);

            return this;
        }

        public override string ToString()
        {
            var primaryTitle = PrimaryTable.EntityName.IsNullOrEmpty()
                ? PrimaryTable.TableName
                : PrimaryTable.EntityName;

            var foreignTitle = ForeignTable.EntityName.IsNullOrEmpty()
                ? ForeignTable.TableName
                : ForeignTable.EntityName;

            return $"{primaryTitle} - {foreignTitle}";
        }
    }

    /// <summary>
    /// The joining of two fields in a foreign key definition.
    /// </summary>
    public class ForeignKeyFieldJoin
    {
        /// <summary>
        /// Gets the primary field.
        /// </summary>
        /// <value>
        /// The primary field.
        /// </value>
        public FieldDefinition PrimaryField { get; internal set; }

        /// <summary>
        /// Gets the foreign field.
        /// </summary>
        /// <value>
        /// The foreign field.
        /// </value>
        public FieldDefinition ForeignField { get; internal set; }

        internal ForeignKeyFieldJoin()
        {
            
        }
    }
}
