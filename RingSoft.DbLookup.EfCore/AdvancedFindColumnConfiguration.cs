// ***********************************************************************
// Assembly         : RingSoft.DbLookup.EfCore
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 07-13-2023
// ***********************************************************************
// <copyright file="AdvancedFindColumnConfiguration.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.DbLookup.AdvancedFind;

namespace RingSoft.DbLookup.EfCore
{
    /// <summary>
    /// Class AdvancedFindColumnConfiguration.
    /// Implements the <see cref="Microsoft.EntityFrameworkCore.IEntityTypeConfiguration{RingSoft.DbLookup.AdvancedFind.AdvancedFindColumn}" />
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.IEntityTypeConfiguration{RingSoft.DbLookup.AdvancedFind.AdvancedFindColumn}" />
    public class AdvancedFindColumnConfiguration : IEntityTypeConfiguration<AdvancedFindColumn>
    {
        /// <summary>
        /// Configures the specified builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public void Configure(EntityTypeBuilder<AdvancedFindColumn> builder)
        {
            builder.Property(p => p.AdvancedFindId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.ColumnId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.Caption).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.TableName).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.FieldName).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.PrimaryTableName).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.PrimaryFieldName).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.Path).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.DecimalFormatType).HasColumnType(DbConstants.ByteColumnType);
            builder.Property(p => p.FieldDataType).HasColumnType(DbConstants.ByteColumnType);
            builder.Property(p => p.Formula).HasColumnType(DbConstants.MemoColumnType);
            builder.Property(p => p.PercentWidth).HasColumnType(DbConstants.DecimalColumnType);

            builder.HasOne(p => p.AdvancedFind)
                .WithMany(p => p.Columns).HasForeignKey(p => p.AdvancedFindId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasKey(p => new { p.AdvancedFindId, p.ColumnId });
        }
    }
}
