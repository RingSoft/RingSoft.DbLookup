// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-16-2023
// ***********************************************************************
// <copyright file="RecordLockingLookup.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

namespace RingSoft.DbLookup.RecordLocking
{
    /// <summary>
    /// Record Locking Lookup Entity
    /// </summary>
    public class RecordLockingLookup
    {
        /// <summary>
        /// Gets or sets the table.
        /// </summary>
        /// <value>The table.</value>
        public string Table { get; set; }

        /// <summary>
        /// Gets or sets the lock date.
        /// </summary>
        /// <value>The lock date.</value>
        public DateTime LockDate { get; set; }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>The user.</value>
        public string User { get; set; }
    }
}
