// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-16-2023
// ***********************************************************************
// <copyright file="LookupDefaults.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Globalization;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;

namespace RingSoft.DbLookup
{
    /// <summary>
    /// Sets up the lookup default properties.  These properties should be set before any lookup definitions are constructed.
    /// </summary>
    public static class LookupDefaults
    {
        /// <summary>
        /// The text column identifier
        /// </summary>
        public const int TextColumnId = 0;
        /// <summary>
        /// The custom content column identifier
        /// </summary>
        public const int CustomContentColumnId = 1;

        /// <summary>
        /// Gets the default number culture.
        /// </summary>
        /// <value>The default number culture.</value>
        public static CultureInfo DefaultNumberCulture { get; private set; }

        /// <summary>
        /// Gets the default date culture.
        /// </summary>
        /// <value>The default date culture.</value>
        public static CultureInfo DefaultDateCulture { get; private set; }

        /// <summary>
        /// Gets the default double count.
        /// </summary>
        /// <value>The default double count.</value>
        public static int DefaultDecimalCount { get; private set; } = 2;

        /// <summary>
        /// Initializes static members of the <see cref="LookupDefaults" /> class.
        /// </summary>
        static LookupDefaults()
        {
            DefaultNumberCulture = DefaultDateCulture = new CultureInfo(CultureInfo.CurrentCulture.Name);
            DecimalFieldDefinition.FormatCulture(DefaultNumberCulture);
        }

        /// <summary>
        /// Sets the default number culture identifier.
        /// </summary>
        /// <param name="cultureId">The culture identifier.</param>
        public static void SetDefaultNumberCultureId(string cultureId)
        {
            DefaultNumberCulture = new CultureInfo(cultureId);
        }

        /// <summary>
        /// Sets the default date format identifier.
        /// </summary>
        /// <param name="cultureId">The culture identifier.</param>
        public static void SetDefaultDateFormatId(string cultureId)
        {
            DefaultDateCulture = new CultureInfo(cultureId);
        }

        /// <summary>
        /// Sets the default double count.
        /// </summary>
        /// <param name="decimalCount">The double count.</param>
        public static void SetDefaultDecimalCount(int decimalCount)
        {
            DefaultDecimalCount = decimalCount;
        }
    }
}
