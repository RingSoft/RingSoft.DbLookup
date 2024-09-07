// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-16-2023
// ***********************************************************************
// <copyright file="JoinField.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace RingSoft.DbLookup.QueryBuilder
{
    /// <summary>
    /// Maps a Primary field to a foreign field in a Join table.
    /// </summary>
    public class JoinField
    {
        /// <summary>
        /// Gets the primary table.
        /// </summary>
        /// <value>The primary table is the "One" table in a one to many join.</value>
        public PrimaryJoinTable PrimaryTable { get; internal set; }

        /// <summary>
        /// Gets the primary field.
        /// </summary>
        /// <value>The primary field.</value>
        public string PrimaryField { get; internal set; }

        /// <summary>
        /// Gets the foreign field.
        /// </summary>
        /// <value>The foreign field.</value>
        public string ForeignField { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="JoinField" /> class.
        /// </summary>
        internal JoinField()
        {
            
        }
    }
}
