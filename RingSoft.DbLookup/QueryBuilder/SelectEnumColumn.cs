// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-19-2022
// ***********************************************************************
// <copyright file="SelectEnumColumn.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DataEntryControls.Engine;

namespace RingSoft.DbLookup.QueryBuilder
{
    /// <summary>
    /// A column that has a numeric database value translated into a text value.
    /// </summary>
    /// <seealso cref="SelectColumn" />
    public class SelectEnumColumn : SelectColumn
    {
        /// <summary>
        /// Gets the type of the column.
        /// </summary>
        /// <value>The type of the column.</value>
        public override ColumnTypes ColumnType => ColumnTypes.Enum;

        /// <summary>
        /// Gets the enum translation.
        /// </summary>
        /// <value>The enum translation.</value>
        public EnumFieldTranslation EnumTranslation { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectEnumColumn"/> class.
        /// </summary>
        internal SelectEnumColumn()
        {
            
        }
    }
}
