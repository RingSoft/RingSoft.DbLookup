// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-19-2022
// ***********************************************************************
// <copyright file="WhereEnumItem.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DataEntryControls.Engine;

namespace RingSoft.DbLookup.QueryBuilder
{
    /// <summary>
    /// A Where Item that searches the text value of an enumerator.
    /// </summary>
    /// <seealso cref="WhereItem" />
    public class WhereEnumItem : WhereItem
    {
        /// <summary>
        /// Gets the type of the where item.
        /// </summary>
        /// <value>The type of the where item.</value>
        public override WhereItemTypes WhereItemType => WhereItemTypes.Enum;

        /// <summary>
        /// Gets the enum translation.
        /// </summary>
        /// <value>The enum translation.</value>
        public EnumFieldTranslation EnumTranslation { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WhereEnumItem"/> class.
        /// </summary>
        internal WhereEnumItem()
        {
            
        }
    }
}
