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

        }
    }
}
