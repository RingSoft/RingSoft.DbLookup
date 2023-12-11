// ***********************************************************************
// Assembly         : RingSoft.DbMaintenance
// Author           : petem
// Created          : 02-03-2023
//
// Last Modified By : petem
// Last Modified On : 02-03-2023
// ***********************************************************************
// <copyright file="AdvancedFindFieldColumnRow.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace RingSoft.DbMaintenance
{
    /// <summary>
    /// Class AdvancedFindFieldColumnRow.
    /// Implements the <see cref="RingSoft.DbMaintenance.AdvancedFindColumnRow" />
    /// </summary>
    /// <seealso cref="RingSoft.DbMaintenance.AdvancedFindColumnRow" />
    public class AdvancedFindFieldColumnRow : AdvancedFindColumnRow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdvancedFindFieldColumnRow"/> class.
        /// </summary>
        /// <param name="manager">The manager.</param>
        public AdvancedFindFieldColumnRow(AdvancedFindColumnsManager manager) : base(manager)
        {
        }
    }
}
