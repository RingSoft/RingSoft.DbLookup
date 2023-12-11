// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 11-29-2023
// ***********************************************************************
// <copyright file="AutoFillValue.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace RingSoft.DbLookup.AutoFill
{
    /// <summary>
    /// An AutoFill results value.
    /// </summary>
    public class AutoFillValue
    {
        /// <summary>
        /// Gets the primary key value.
        /// </summary>
        /// <value>The primary key value.</value>
        public PrimaryKeyValue PrimaryKeyValue { get; }

        /// <summary>
        /// Gets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text { get; }

        /// <summary>
        /// Gets a value indicating whether [from lookup].
        /// </summary>
        /// <value><c>true</c> if [from lookup]; otherwise, <c>false</c>.</value>
        public bool FromLookup { get; internal set; } = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoFillValue"/> class.
        /// </summary>
        /// <param name="primaryKeyValue">The primary key value.</param>
        /// <param name="text">The text.</param>
        public AutoFillValue(PrimaryKeyValue primaryKeyValue, string text)
        {
            PrimaryKeyValue = primaryKeyValue;
            Text = text;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return Text;
        }
    }
}
