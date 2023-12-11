// ***********************************************************************
// Assembly         : RingSoft.DbLookup.EfCore
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-19-2022
// ***********************************************************************
// <copyright file="AdvancedFindConfiguration.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RingSoft.DbLookup.EfCore
{
    /// <summary>
    /// Class AdvancedFindConfiguration.
    /// Implements the <see cref="Microsoft.EntityFrameworkCore.IEntityTypeConfiguration{RingSoft.DbLookup.AdvancedFind.AdvancedFind}" />
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.IEntityTypeConfiguration{RingSoft.DbLookup.AdvancedFind.AdvancedFind}" />
    public class AdvancedFindConfiguration : IEntityTypeConfiguration<AdvancedFind.AdvancedFind>
    {
        /// <summary>
        /// Configures the specified builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public void Configure(EntityTypeBuilder<AdvancedFind.AdvancedFind> builder)
        {
            builder.Property(p => p.Disabled).HasColumnType(DbConstants.BoolColumnType);
            builder.Property(p => p.Id).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.Name).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.Table).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.FromFormula).HasColumnType(DbConstants.MemoColumnType);
            builder.Property(p => p.RefreshRate).HasColumnType(DbConstants.ByteColumnType);
            builder.Property(p => p.RefreshValue).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.RefreshCondition).HasColumnType(DbConstants.ByteColumnType);
            builder.Property(p => p.YellowAlert).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.RedAlert).HasColumnType(DbConstants.IntegerColumnType);

        }
    }
}
