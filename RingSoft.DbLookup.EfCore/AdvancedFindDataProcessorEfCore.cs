using System.Collections.Generic;
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

        public bool SaveAdvancedFind(AdvancedFind.AdvancedFind advancedFind, List<AdvancedFindColumn> columns,
            List<AdvancedFindFilter> filters)
        {
            var context = EfCoreGlobals.DbAdvancedFindContextCore.GetNewDbContext();
            var dbContext = context.GetDbContextEf();

            if (context.AdvancedFinds.FirstOrDefault(p => p.Id == advancedFind.Id) == null)
                dbContext.AddNewEntity(context.AdvancedFinds, advancedFind, "Adding New Advanced Find");
            else
            {
                context.Dispose();
                context = EfCoreGlobals.DbAdvancedFindContextCore.GetNewDbContext();
                dbContext = context.GetDbContextEf();
                if (!dbContext.SaveEntity(context.AdvancedFinds, advancedFind, "Saving Advanced Find"))
                {
                    context.Dispose();
                    return false;
                }
            }

            context.AdvancedFindColumns.RemoveRange(context.AdvancedFindColumns.Where(p => p.AdvancedFindId == advancedFind.Id));

            foreach (var advancedFindColumn in columns)
            {
                advancedFindColumn.AdvancedFindId = advancedFind.Id;
            }
            context.AdvancedFindColumns.AddRange(columns);

            context.AdvancedFindFilters.RemoveRange(context.AdvancedFindFilters.Where(p => p.AdvancedFindId == advancedFind.Id));
            context.AdvancedFindFilters.AddRange(filters);
            var result = dbContext.SaveEfChanges("Commiting Advanced Find");

            context.Dispose();
            return result;
        }

        public bool DeleteAdvancedFind(int advancedFindId)
        {
            var context = EfCoreGlobals.DbAdvancedFindContextCore.GetNewDbContext();
            var dbContext = context.GetDbContextEf();
            var advancedFind = context.AdvancedFinds.FirstOrDefault(p => p.Id == advancedFindId);

            context.AdvancedFindColumns.RemoveRange(
                context.AdvancedFindColumns.Where(p => p.AdvancedFindId == advancedFindId));
            context.AdvancedFindFilters.RemoveRange(
                context.AdvancedFindFilters.Where(p => p.AdvancedFindId == advancedFindId));

            return dbContext.DeleteEntity(context.AdvancedFinds, advancedFind, "Deleting Customer");

        }
    }
}
