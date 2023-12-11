// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-19-2022
// ***********************************************************************
// <copyright file="PrimaryJoinTable.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections.Generic;

namespace RingSoft.DbLookup.QueryBuilder
{
    /// <summary>
    /// Enum JoinTypes
    /// </summary>
    public enum JoinTypes
    {
        /// <summary>
        /// The inner join
        /// </summary>
        InnerJoin = 0,
        /// <summary>
        /// The left outer join
        /// </summary>
        LeftOuterJoin = 1
    }
    /// <summary>
    /// A join clause.  This table object is the Primary table in a JOIN statement.
    /// </summary>
    /// <seealso cref="QueryTable" />
    public class PrimaryJoinTable : QueryTable
    {
        /// <summary>
        /// Gets the foreign table.
        /// </summary>
        /// <value>The foreign table is the "Many" table in a one to many join.</value>
        public QueryTable ForeignTable { get; internal set; }

        /// <summary>
        /// Gets the type of the join.
        /// </summary>
        /// <value>The type of the join.</value>
        public JoinTypes JoinType { get; internal set; }

        /// <summary>
        /// Gets the join fields.
        /// </summary>
        /// <value>The join fields.</value>
        public IReadOnlyList<JoinField> JoinFields => _joinFields;

        /// <summary>
        /// The join fields
        /// </summary>
        private List<JoinField> _joinFields = new List<JoinField>();

        /// <summary>
        /// Initializes a new instance of the <see cref="PrimaryJoinTable"/> class.
        /// </summary>
        internal PrimaryJoinTable()
        {
            
        }

        /// <summary>
        /// Adds a join field to this join table.
        /// </summary>
        /// <param name="primaryFieldName">Name of the primary field.</param>
        /// <param name="foreignFieldName">Name of the foreign field.</param>
        /// <returns>PrimaryJoinTable.</returns>
        public PrimaryJoinTable AddJoinField(string primaryFieldName, string foreignFieldName)
        {
            var joinField = new JoinField
            {
                ForeignField = foreignFieldName,
                PrimaryTable = this,
                PrimaryField = primaryFieldName
            };

            _joinFields.Add(joinField);

            return this;
        }
    }
}
