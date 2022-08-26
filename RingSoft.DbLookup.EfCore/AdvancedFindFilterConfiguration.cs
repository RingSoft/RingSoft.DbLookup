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
            builder.HasOne(p => p.AdvancedFind)
                .WithMany(p => p.Filters).HasForeignKey(p => p.AdvancedFindId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.SearchForAdvancedFind)
                .WithMany(p => p.SearchForAdvancedFindFilters).HasForeignKey(p => p.SearchForAdvancedFindId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasKey(p => new { p.AdvancedFindId, p.FilterId });

        }
    }
}
