// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 06-18-2023
// ***********************************************************************
// <copyright file="DeleteTables.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using System.Collections.Generic;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.QueryBuilder;

namespace RingSoft.DbLookup
{
    /// <summary>
    /// Class DeleteTables.
    /// </summary>
    public class DeleteTables
    {
        /// <summary>
        /// Gets the tables.
        /// </summary>
        /// <value>The tables.</value>
        public List<DeleteTable> Tables { get; private set; } = new List<DeleteTable>();

        /// <summary>
        /// Gets or sets the primary key value.
        /// </summary>
        /// <value>The primary key value.</value>
        public PrimaryKeyValue PrimaryKeyValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [delete all data].
        /// </summary>
        /// <value><c>true</c> if [delete all data]; otherwise, <c>false</c>.</value>
        public bool DeleteAllData { get; set; }

        /// <summary>
        /// Gets or sets the context.
        /// </summary>
        /// <value>The context.</value>
        public IDbContext Context { get; set; }
    }

    /// <summary>
    /// Class DeleteTable.
    /// </summary>
    public class DeleteTable
    {
        /// <summary>
        /// The child field
        /// </summary>
        private FieldDefinition _childField;

        /// <summary>
        /// Gets or sets the child field.
        /// </summary>
        /// <value>The child field.</value>
        public FieldDefinition ChildField
        {
            get
            {
                if (_childField == null)
                {
                    
                }
                return _childField;
            }
            set
            {
                if (value == null)
                {
                    
                }
                _childField = value;
        }
    }

        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        /// <value>The parent.</value>
        public DeleteTables Parent { get; set; }

        /// <summary>
        /// Gets or sets the parent field.
        /// </summary>
        /// <value>The parent field.</value>
        public FieldDefinition ParentField { get; set; }

        /// <summary>
        /// Gets or sets the parent delete table.
        /// </summary>
        /// <value>The parent delete table.</value>
        public DeleteTable ParentDeleteTable { get; set; }

        /// <summary>
        /// Gets or sets the root field.
        /// </summary>
        /// <value>The root field.</value>
        public FieldDefinition RootField { get; set; }

        /// <summary>
        /// Gets or sets the query.
        /// </summary>
        /// <value>The query.</value>
        public SelectQueryMauiBase Query { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [delete all data].
        /// </summary>
        /// <value><c>true</c> if [delete all data]; otherwise, <c>false</c>.</value>
        public bool DeleteAllData { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [null all data].
        /// </summary>
        /// <value><c>true</c> if [null all data]; otherwise, <c>false</c>.</value>
        public bool NullAllData { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="DeleteTable"/> is processed.
        /// </summary>
        /// <value><c>true</c> if processed; otherwise, <c>false</c>.</value>
        public bool Processed { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the column.
        /// </summary>
        /// <value>The column.</value>
        public LookupFieldColumnDefinition Column { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            if (ChildField != null)
            {
                return ChildField.ToString();
            }
            return base.ToString();
        }
    }
}
