// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 07-13-2023
// ***********************************************************************
// <copyright file="RecordLock.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.ComponentModel.DataAnnotations;

namespace RingSoft.DbLookup.RecordLocking
{
    /// <summary>
    /// Record Locking Entity.
    /// </summary>
    public class RecordLock
    {
        /// <summary>
        /// Gets or sets the table.
        /// </summary>
        /// <value>The table.</value>
        [Required]
        [MaxLength(50)]
        public string Table { get; set; }

        /// <summary>
        /// Gets or sets the primary key.
        /// </summary>
        /// <value>The primary key.</value>
        [Required]
        [MaxLength(50)]
        public string PrimaryKey { get; set; }

        /// <summary>
        /// Gets or sets the lock date time.
        /// </summary>
        /// <value>The lock date time.</value>
        [Required]
        public DateTime LockDateTime { get; set; }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>The user.</value>
        [MaxLength(50)]
        public string? User { get; set; }
    }
}
