// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-16-2023
// ***********************************************************************
// <copyright file="TableFieldJoinDefinition.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.QueryBuilder;

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
        /// <value>The foreign key definition.</value>
        public ForeignKeyDefinition ForeignKeyDefinition { get; internal set; }

        /// <summary>
        /// Gets the alias used in the SQL statement.
        /// </summary>
        /// <value>
        /// The alias.
        /// </value>
        private string _alias;

        /// <summary>
        /// Gets or sets the alias.
        /// </summary>
        /// <value>The alias.</value>
        public string Alias
        {
            get
            {
                if (_alias.IsNullOrEmpty())
                {
                    return ForeignKeyDefinition.Alias;
                }
                return _alias;
            }
            set
            {
                _alias = value;
            }
        }

        /// <summary>
        /// Gets the parent alias used in the SQL string.
        /// </summary>
        /// <value>The parent alias.</value>
        public string ParentAlias { get; internal set; }

        /// <summary>
        /// Gets the parent object.
        /// </summary>
        /// <value>The parent object.</value>
        public IJoinParent ParentObject { get; internal set; }

        /// <summary>
        /// Gets the type of the join.
        /// </summary>
        /// <value>The type of the join.</value>
        public JoinTypes JoinType { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TableFieldJoinDefinition" /> class.
        /// </summary>
        internal TableFieldJoinDefinition()
        {
            
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return Alias;
        }

        /// <summary>
        /// Copies from.
        /// </summary>
        /// <param name="source">The source.</param>
        internal void CopyFrom(TableFieldJoinDefinition source)
        {
            ForeignKeyDefinition = source.ForeignKeyDefinition;
            ParentAlias = source.ParentAlias;
            ParentObject = source.ParentObject;
            Alias = source.Alias;
        }
    }
}
