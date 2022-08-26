using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Org.BouncyCastle.Asn1.Pkcs;
using RingSoft.DbLookup.AdvancedFind;

namespace RingSoft.DbLookup.EfCore
{
    public class AdvancedFindColumnConfiguration : IEntityTypeConfiguration<AdvancedFindColumn>
    {
        public void Configure(EntityTypeBuilder<AdvancedFindColumn> builder)
        {
            builder.Property(p => p.AdvancedFindId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.ColumnId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.Caption).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.TableName).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.FieldName).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.PrimaryTableName).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.DecimalFormatType).HasColumnType(DbConstants.ByteColumnType);
            builder.Property(p => p.FieldDataType).HasColumnType(DbConstants.ByteColumnType);
            builder.Property(p => p.Formula).HasColumnType(DbConstants.MemoColumnType);
            builder.Property(p => p.PercentWidth).HasColumnType(DbConstants.DecimalColumnType);

            builder.HasKey(p => new { p.AdvancedFindId, p.ColumnId });

            builder.HasOne(p => p.AdvancedFind)
                .WithMany(p => p.Columns).HasForeignKey(p => p.AdvancedFindId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
