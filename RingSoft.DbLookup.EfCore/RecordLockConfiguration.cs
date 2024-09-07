// ***********************************************************************
// Assembly         : RingSoft.DbLookup.EfCore
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-16-2023
// ***********************************************************************
// <copyright file="RecordLockConfiguration.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.DbLookup.RecordLocking;

namespace RingSoft.DbLookup.EfCore
{
    /// <summary>
    /// Class RecordLockConfiguration.
    /// Implements the <see cref="Microsoft.EntityFrameworkCore.IEntityTypeConfiguration{RingSoft.DbLookup.RecordLocking.RecordLock}" />
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.IEntityTypeConfiguration{RingSoft.DbLookup.RecordLocking.RecordLock}" />
    public class RecordLockConfiguration : IEntityTypeConfiguration<RecordLock>
    {
        /// <summary>
        /// Configures the specified builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public void Configure(EntityTypeBuilder<RecordLock> builder)
        {
            builder.Property(p => p.Table).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.PrimaryKey).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.LockDateTime).HasColumnType(DbConstants.DateColumnType);
            builder.Property(p => p.User).HasColumnType(DbConstants.StringColumnType);

            builder.HasKey(p => new {p.Table, p.PrimaryKey});
        }
    }
}
