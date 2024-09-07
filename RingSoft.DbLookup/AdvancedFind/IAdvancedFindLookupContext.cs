// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-16-2023
// ***********************************************************************
// <copyright file="IAdvancedFindLookupContext.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.RecordLocking;

namespace RingSoft.DbLookup.AdvancedFind
{
    /// <summary>
    /// Interface IAdvancedFindLookupContext
    /// </summary>
    public interface IAdvancedFindLookupContext
    {
        /// <summary>
        /// Gets the context.
        /// </summary>
        /// <value>The context.</value>
        LookupContextBase Context { get; }

        /// <summary>
        /// Gets or sets the record locks.
        /// </summary>
        /// <value>The record locks.</value>
        TableDefinition<RecordLock> RecordLocks { get; set; }

        /// <summary>
        /// Gets or sets the advanced finds.
        /// </summary>
        /// <value>The advanced finds.</value>
        TableDefinition<AdvancedFind> AdvancedFinds { get; set; }

        /// <summary>
        /// Gets or sets the advanced find columns.
        /// </summary>
        /// <value>The advanced find columns.</value>
        TableDefinition<AdvancedFindColumn> AdvancedFindColumns { get; set; }

        /// <summary>
        /// Gets or sets the advanced find filters.
        /// </summary>
        /// <value>The advanced find filters.</value>
        TableDefinition<AdvancedFindFilter> AdvancedFindFilters { get; set; }

        /// <summary>
        /// Gets or sets the advanced find lookup.
        /// </summary>
        /// <value>The advanced find lookup.</value>
        LookupDefinition<AdvancedFindLookup, AdvancedFind> AdvancedFindLookup { get; set; }

        /// <summary>
        /// Gets or sets the advanced find column lookup.
        /// </summary>
        /// <value>The advanced find column lookup.</value>
        LookupDefinition<AdvancedFindLookup, AdvancedFindColumn> AdvancedFindColumnLookup { get; set; }

        /// <summary>
        /// Gets or sets the advanced find filter lookup.
        /// </summary>
        /// <value>The advanced find filter lookup.</value>
        LookupDefinition<AdvFindFilterLookup, AdvancedFindFilter> AdvancedFindFilterLookup { get; set; }

        /// <summary>
        /// Gets or sets the record locking lookup.
        /// </summary>
        /// <value>The record locking lookup.</value>
        LookupDefinition<RecordLockingLookup, RecordLock> RecordLockingLookup { get; set; }
    }
}
