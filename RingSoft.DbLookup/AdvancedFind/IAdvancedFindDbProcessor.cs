// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-19-2022
// ***********************************************************************
// <copyright file="IAdvancedFindDbProcessor.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using RingSoft.DbLookup.RecordLocking;

namespace RingSoft.DbLookup.AdvancedFind
{
    /// <summary>
    /// Interface IAdvancedFindDbProcessor
    /// </summary>
    public interface IAdvancedFindDbProcessor
    {
        /// <summary>
        /// Gets the advanced find.
        /// </summary>
        /// <param name="advancedFindId">The advanced find identifier.</param>
        /// <returns>DbLookup.AdvancedFind.AdvancedFind.</returns>
        DbLookup.AdvancedFind.AdvancedFind GetAdvancedFind(int advancedFindId);

        /// <summary>
        /// Saves the advanced find.
        /// </summary>
        /// <param name="advancedFind">The advanced find.</param>
        /// <param name="columns">The columns.</param>
        /// <param name="filters">The filters.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool SaveAdvancedFind(DbLookup.AdvancedFind.AdvancedFind advancedFind, List<AdvancedFindColumn> columns,
            List<AdvancedFindFilter> filters);

        /// <summary>
        /// Deletes the advanced find.
        /// </summary>
        /// <param name="advancedFindId">The advanced find identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool DeleteAdvancedFind(int advancedFindId);

        /// <summary>
        /// Gets the record lock.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="primaryKey">The primary key.</param>
        /// <returns>RecordLock.</returns>
        RecordLock GetRecordLock(string table, string primaryKey);
    }
}
