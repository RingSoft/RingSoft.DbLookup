// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-16-2023
// ***********************************************************************
// <copyright file="OrderByFormulaSegment.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace RingSoft.DbLookup.QueryBuilder
{
    /// <summary>
    /// An Order By segment for a formula.
    /// </summary>
    /// <seealso cref="WhereItem" />
    public class OrderByFormulaSegment : OrderBySegment
    {
        /// <summary>
        /// Gets the type of the order by segment.
        /// </summary>
        /// <value>The type of the order by segment.</value>
        public override OrderBySegmentTypes OrderBySegmentType => OrderBySegmentTypes.Formula;

        /// <summary>
        /// Gets the formula.
        /// </summary>
        /// <value>The formula.</value>
        public string Formula { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderByFormulaSegment" /> class.
        /// </summary>
        internal OrderByFormulaSegment()
        {
            
        }
    }
}
