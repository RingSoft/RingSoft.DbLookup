using System.Linq;
using Microsoft.EntityFrameworkCore;
using RingSoft.DbLookup.AdvancedFind;

namespace RingSoft.DbLookup.EfCore
{
    public class AdvancedFindDataProcessorEfCore : IAdvancedFindDbProcessor
    {
        public static void ConfigureAdvancedFind(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdvancedFindColumn>().HasKey(p => new { p.AdvancedFindId, p.ColumnId });

            modelBuilder.Entity<AdvancedFindColumn>().HasOne(p => p.AdvancedFind)
                .WithMany(p => p.Columns).HasForeignKey(p => p.AdvancedFindId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AdvancedFindFilter>().HasKey(p => new { p.AdvancedFindId, p.FilterId });

            modelBuilder.Entity<AdvancedFindFilter>().HasOne(p => p.AdvancedFind)
                .WithMany(p => p.Filters).HasForeignKey(p => p.AdvancedFindId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AdvancedFindFilter>().HasOne(p => p.SearchForAdvancedFind)
                .WithMany(p => p.SearchForAdvancedFindFilters).HasForeignKey(p => p.SearchForAdvancedFindId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public AdvancedFind.AdvancedFind GetAdvancedFind(int advancedFindId)
        {
            var context = EfCoreGlobals.DbAdvancedFindContextCore.GetNewDbContext();
            return context.AdvancedFinds.Include(p => p.Columns)
                .Include(p => p.Filters)
                .FirstOrDefault(p => p.Id == advancedFindId);
        }
    }
}
