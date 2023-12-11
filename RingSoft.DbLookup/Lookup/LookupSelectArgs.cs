// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 06-07-2023
// ***********************************************************************
// <copyright file="LookupSelectArgs.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace RingSoft.DbLookup.Lookup
{
    /// <summary>
    /// Arguments sent by the LookupSelect event.
    /// </summary>
    public class LookupSelectArgs
    {
        /// <summary>
        /// Gets the lookup data.
        /// </summary>
        /// <value>The lookup data.</value>
        public LookupDataMauiBase LookupData { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupSelectArgs" /> class.
        /// </summary>
        /// <param name="lookupData">The lookup data.</param>
        public LookupSelectArgs(LookupDataMauiBase lookupData)
        {
            LookupData = lookupData;
        }
    }
}
