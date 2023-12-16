// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 09-11-2023
// ***********************************************************************
// <copyright file="ForeignKeyDefinition.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections.Generic;
using System.Linq;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;

namespace RingSoft.DbLookup.ModelDefinition
{
    /// <summary>
    /// Joins two tables together.
    /// </summary>
    public class ForeignKeyDefinition
    {
        /// <summary>
        /// Gets the primary ("One") table.
        /// </summary>
        /// <value>The primary table.</value>
        public TableDefinitionBase PrimaryTable { get; internal set; }

        /// <summary>
        /// Gets the foreign ("Many") table.
        /// </summary>
        /// <value>The foreign table.</value>
        public TableDefinitionBase ForeignTable { get; internal set; }

        /// <summary>
        /// Gets the fields that join the primary and foreign tables together..
        /// </summary>
        /// <value>The field joins.</value>
        public IReadOnlyList<ForeignKeyFieldJoin> FieldJoins => ForeignKeyFieldJoins;

        /// <summary>
        /// Gets the alias used in the SQL statement.
        /// </summary>
        /// <value>
        /// The alias.
        /// </value>
        private string _alias;

        /// <summary>
        /// Gets the alias.
        /// </summary>
        /// <value>The alias.</value>
        public string Alias
        {
            get
            {
                if (_alias.IsNullOrEmpty())
                {
                    return GetDefaultAlias();
                }
                return _alias;
            }
            //internal set => _alias = value;
        }

        /// <summary>
        /// Gets the default alias.
        /// </summary>
        /// <returns>System.String.</returns>
        private string GetDefaultAlias()
        {
            return $"{ForeignTable.TableName}_{PrimaryTable.TableName}_{FieldJoins[0].ForeignField.FieldName}";
        }

        /// <summary>
        /// Gets the name of the foreign object property.
        /// </summary>
        /// <value>The name of the foreign object property.</value>
        public string ForeignObjectPropertyName { get; internal set; }

        /// <summary>
        /// Gets or sets the name of the collection.
        /// </summary>
        /// <value>The name of the collection.</value>
        public string CollectionName { get; set; }

        /// <summary>
        /// Gets the foreign key field joins.
        /// </summary>
        /// <value>The foreign key field joins.</value>
        public List<ForeignKeyFieldJoin> ForeignKeyFieldJoins { get; internal set; } = new List<ForeignKeyFieldJoin>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ForeignKeyDefinition"/> class.
        /// </summary>
        internal ForeignKeyDefinition()
        {
            
        }

        /// <summary>
        /// Determines whether [is equal to] [the specified foreign key definition].
        /// </summary>
        /// <param name="foreignKeyDefinition">The foreign key definition.</param>
        /// <returns><c>true</c> if [is equal to] [the specified foreign key definition]; otherwise, <c>false</c>.</returns>
        public bool IsEqualTo(ForeignKeyDefinition foreignKeyDefinition)
        {
            var found = false;
            foreach (var foreignKeyFieldJoin in FieldJoins)
            {
                found = foreignKeyDefinition.FieldJoins.Any(p => 
                    p.ForeignField == foreignKeyFieldJoin.ForeignField);
                if (found)
                {
                    found = foreignKeyDefinition.FieldJoins.Any(p =>
                        p.PrimaryField == foreignKeyFieldJoin.PrimaryField);
                }
            }
            return found;
        }

        /// <summary>
        /// Adds a field join to the FieldJoins list.
        /// </summary>
        /// <param name="primaryField">The primary field.</param>
        /// <param name="foreignField">The foreign field.</param>
        /// <param name="addChildField">if set to <c>true</c> [add child field].</param>
        /// <returns>This object.</returns>
        public ForeignKeyDefinition AddFieldJoin(FieldDefinition primaryField, FieldDefinition foreignField, bool addChildField)
        {
            var foreignKeyFieldJoin = new ForeignKeyFieldJoin
            {
                PrimaryField = primaryField,
                ForeignField = foreignField
            };
            ForeignKeyFieldJoins.Add(foreignKeyFieldJoin);

            if (addChildField)
            {
                primaryField.TableDefinition.ChildFields.Add(foreignField);
            }

            return this;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
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
        /// <value>The primary field.</value>
        public FieldDefinition PrimaryField { get; internal set; }

        /// <summary>
        /// Gets the foreign field.
        /// </summary>
        /// <value>The foreign field.</value>
        public FieldDefinition ForeignField { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ForeignKeyFieldJoin"/> class.
        /// </summary>
        internal ForeignKeyFieldJoin()
        {
            
        }
    }
}
