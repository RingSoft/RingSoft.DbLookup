using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.DbLookup.AdvancedFind;

namespace RingSoft.DbLookup.EfCore
{
    public class AdvancedFindFilterConfiguration : IEntityTypeConfiguration<AdvancedFindFilter>
    {
        public void Configure(EntityTypeBuilder<AdvancedFindFilter> builder)
        {
            builder.Property(p => p.AdvancedFindId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.FilterId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.TableName).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.FieldName).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.PrimaryTableName).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.PrimaryFieldName).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.Formula).HasColumnType(DbConstants.MemoColumnType);
            builder.Property(p => p.CustomDate).HasColumnType(DbConstants.BoolColumnType);
            builder.Property(p => p.EndLogic).HasColumnType(DbConstants.ByteColumnType);
            builder.Property(p => p.LeftParentheses).HasColumnType(DbConstants.ByteColumnType);
            builder.Property(p => p.Operand).HasColumnType(DbConstants.ByteColumnType);
            builder.Property(p => p.RightParentheses).HasColumnType(DbConstants.ByteColumnType);
            builder.Property(p => p.SearchForValue).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.SearchForAdvancedFindId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.FormulaDataType).HasColumnType(DbConstants.ByteColumnType);
            builder.Property(p => p.FormulaDisplayValue).HasColumnType(DbConstants.StringColumnType);

            builder.HasOne(p => p.AdvancedFind)
                .WithMany(p => p.Filters).HasForeignKey(p => p.AdvancedFindId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.SearchForAdvancedFind)
                .WithMany(p => p.SearchForAdvancedFindFilters).HasForeignKey(p => p.SearchForAdvancedFindId)
                .OnDelete(DeleteBehavior.Cascade).IsRequired(false);

            builder.HasKey(p => new { p.AdvancedFindId, p.FilterId });

        }
    }
}
