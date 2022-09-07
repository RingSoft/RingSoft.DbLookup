using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RingSoft.DbLookup.EfCore
{
    public class AdvancedFindConfiguration : IEntityTypeConfiguration<AdvancedFind.AdvancedFind>
    {
        public void Configure(EntityTypeBuilder<AdvancedFind.AdvancedFind> builder)
        {
            builder.Property(p => p.Id).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.Name).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.Table).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.FromFormula).HasColumnType(DbConstants.MemoColumnType);
            builder.Property(p => p.RefreshRate).HasColumnType(DbConstants.ByteColumnType);
            builder.Property(p => p.RefreshValue).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.RefreshCondition).HasColumnType(DbConstants.ByteColumnType);
            builder.Property(p => p.YellowAlert).HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.RedAlert).HasColumnType(DbConstants.DecimalColumnType);

        }
    }
}
