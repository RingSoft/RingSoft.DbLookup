// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-16-2023
// ***********************************************************************
// <copyright file="OrderByEnumSegment.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DataEntryControls.Engine;

namespace RingSoft.DbLookup.QueryBuilder
{
    /// <summary>
    /// An Order By segment for the text value of an enumerator.
    /// </summary>
    /// <seealso cref="WhereItem" />
    public class OrderByEnumSegment : OrderBySegment
    {
        /// <summary>
        /// Gets the type of the order by segment.
        /// </summary>
        /// <value>The type of the order by segment.</value>
        public override OrderBySegmentTypes OrderBySegmentType => OrderBySegmentTypes.Enum;

        /// <summary>
        /// Gets the enum translation.
        /// </summary>
        /// <value>The enum translation.</value>
        public EnumFieldTranslation EnumTranslation { get; internal set; }
    }
}
