// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 11-23-2023
//
// Last Modified By : petem
// Last Modified On : 11-26-2023
// ***********************************************************************
// <copyright file="SystemDataRepository.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.RecordLocking;
using System.Collections.Generic;
using System.Linq;

namespace RingSoft.DbLookup
{
    /// <summary>
    /// Class SystemDataRepository.
    /// Implements the <see cref="IAdvancedFindDbProcessor" />
    /// </summary>
    /// <seealso cref="IAdvancedFindDbProcessor" />
    public abstract class SystemDataRepository : IAdvancedFindDbProcessor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SystemDataRepository"/> class.
        /// </summary>
        public SystemDataRepository()
        {
            Initialize();
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Initialize()
        {
            SystemGlobals.AdvancedFindDbProcessor = this;
            SystemGlobals.DataRepository = this;
        }

        /// <summary>
        /// Gets the advanced find.
        /// </summary>
        /// <param name="advancedFindId">The advanced find identifier.</param>
        /// <returns>DbLookup.AdvancedFind.AdvancedFind.</returns>
        public AdvancedFind.AdvancedFind GetAdvancedFind(int advancedFindId)
        {
            var advFind = new AdvancedFind.AdvancedFind
            {
                Id = advancedFindId,
            };
            return advFind.FillOutProperties(true);
        }

        /// <summary>
        /// Saves the advanced find.
        /// </summary>
        /// <param name="advancedFind">The advanced find.</param>
        /// <param name="columns">The columns.</param>
        /// <param name="filters">The filters.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool SaveAdvancedFind(AdvancedFind.AdvancedFind advancedFind, List<AdvancedFindColumn> columns,
            List<AdvancedFindFilter> filters)
        {
            var result = true;
            var context = GetDataContext();
            if (context.SaveEntity(advancedFind, $"Saving Advanced Find '{advancedFind.Name}.'"))
            {
                var columnsQuery = context.GetTable<AdvancedFindColumn>();
                var oldColumns = columnsQuery.Where(p => p.AdvancedFindId == advancedFind.Id);

                foreach (var advancedFindColumn in columns)
                {
                    advancedFindColumn.AdvancedFindId = advancedFind.Id;
                }
                context.RemoveRange(oldColumns);
                context.AddRange(columns);

                var filtersQuery = context.GetTable<AdvancedFindFilter>();
                var oldFilters = filtersQuery.Where(
                    p => p.AdvancedFindId == advancedFind.Id);

                foreach (var advancedFindFilter in filters)
                {
                    advancedFindFilter.AdvancedFindId = advancedFind.Id;
                }
                context.RemoveRange(oldFilters);
                context.AddRange(filters);

                result = context.Commit($"Saving Advanced Find '{advancedFind.Name}' Details");
            }
            return result;
        }

        /// <summary>
        /// Deletes the advanced find.
        /// </summary>
        /// <param name="advancedFindId">The advanced find identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool DeleteAdvancedFind(int advancedFindId)
        {
            var context = SystemGlobals.DataRepository.GetDataContext();
            var query = context.GetTable<AdvancedFind.AdvancedFind>();
            var advancedFind = query.FirstOrDefault(p => p.Id == advancedFindId);
            if (advancedFind != null)
            {
                var columnsQuery = context.GetTable<AdvancedFindColumn>();
                var oldColumns = columnsQuery.Where(
                    p => p.AdvancedFindId == advancedFindId);


                var filtersQuery = context.GetTable<AdvancedFindFilter>();
                var oldFilters = filtersQuery.Where(
                    p => p.AdvancedFindId == advancedFindId);

                context.RemoveRange(oldColumns);
                context.RemoveRange(oldFilters);

                if (context.DeleteNoCommitEntity(advancedFind, $"Deleting Advanced Find '{advancedFind.Name}'."))
                {
                    return context.Commit($"Deleting Advanced Find '{advancedFind.Name}'.");
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Gets the record lock.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="primaryKey">The primary key.</param>
        /// <returns>RecordLock.</returns>
        public RecordLock GetRecordLock(string table, string primaryKey)
        {
            var context = SystemGlobals.DataRepository.GetDataContext();
            var query = context.GetTable<RecordLock>();
            return query.FirstOrDefault(p => p.Table == table && p.PrimaryKey == primaryKey);
        }

        /// <summary>
        /// Gets the data context.
        /// </summary>
        /// <returns>IDbContext.</returns>
        public abstract IDbContext GetDataContext();

        /// <summary>
        /// Gets the data context.
        /// </summary>
        /// <param name="dataProcessor">The data processor.</param>
        /// <returns>IDbContext.</returns>
        public abstract IDbContext GetDataContext(DbDataProcessor dataProcessor);
    }
}
