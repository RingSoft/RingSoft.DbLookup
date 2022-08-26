using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.DbLookup.AdvancedFind;

namespace RingSoft.DbLookup.EfCore
{
    public class AdvancedFindColumnConfiguration : IEntityTypeConfiguration<AdvancedFindColumn>
    {
        public void Configure(EntityTypeBuilder<AdvancedFindColumn> builder)
        {
            builder.HasKey(p => new { p.AdvancedFindId, p.ColumnId });

            builder.HasOne(p => p.AdvancedFind)
                .WithMany(p => p.Columns).HasForeignKey(p => p.AdvancedFindId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
