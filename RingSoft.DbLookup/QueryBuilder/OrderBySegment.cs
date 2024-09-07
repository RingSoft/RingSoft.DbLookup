// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-16-2023
// ***********************************************************************
// <copyright file="OrderBySegment.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace RingSoft.DbLookup.QueryBuilder
{
    /// <summary>
    /// Enum OrderBySegmentTypes
    /// </summary>
    public enum OrderBySegmentTypes
    {
        /// <summary>
        /// The general
        /// </summary>
        General = 0,
        /// <summary>
        /// The formula
        /// </summary>
        Formula = 1,
        /// <summary>
        /// The enum
        /// </summary>
        Enum = 2
    }

    /// <summary>
    /// Enum OrderByTypes
    /// </summary>
    public enum OrderByTypes
    {
        /// <summary>
        /// The ascending
        /// </summary>
        Ascending = 0,
        /// <summary>
        /// The descending
        /// </summary>
        Descending = 1
    }
    /// <summary>
    /// An segment of an ORDER BY clause.
    /// </summary>
    public class OrderBySegment
    {
        /// <summary>
        /// Gets the type of the order by segment.
        /// </summary>
        /// <value>The type of the order by segment.</value>
        public virtual OrderBySegmentTypes OrderBySegmentType => OrderBySegmentTypes.General;

        /// <summary>
        /// Gets the table object this Where Item is attached to.
        /// </summary>
        /// <value>The table.</value>
        public QueryTable Table { get; internal set; }

        /// <summary>
        /// Gets the name of the field.
        /// </summary>
        /// <value>The name of the field.</value>
        public string FieldName { get; internal set; }

        /// <summary>
        /// Gets the type of the order by. e.g.(ASC or DESC)
        /// </summary>
        /// <value>The type of the order by.</value>
        public OrderByTypes OrderByType { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether the order by is case sensitive.
        /// </summary>
        /// <value><c>true</c> if case sensitive; otherwise, <c>false</c>.</value>
        public bool CaseSensitive { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderBySegment" /> class.
        /// </summary>
        internal OrderBySegment()
        {
            
        }
    }
}
