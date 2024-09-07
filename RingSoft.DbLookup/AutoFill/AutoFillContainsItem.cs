// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-16-2023
// ***********************************************************************
// <copyright file="AutoFillContainsItem.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace RingSoft.DbLookup.AutoFill
{
    /// <summary>
    /// This class represents an item in the Auto Fill control's drop down contains box.
    /// </summary>
    public class AutoFillContainsItem
    {
        /// <summary>
        /// Gets or sets the prefix text.
        /// </summary>
        /// <value>The prefix text.</value>
        public string PrefixText { get; set; }

        /// <summary>
        /// Gets or sets the contains text that is in bold.
        /// </summary>
        /// <value>The contains text.</value>
        public string ContainsText { get; set; }

        /// <summary>
        /// Gets or sets the suffix text.
        /// </summary>
        /// <value>The suffix text.</value>
        public string SuffixText { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return PrefixText + ContainsText + SuffixText;
        }
    }
}
