using Microsoft.EntityFrameworkCore;
using RingSoft.DbLookup.AdvancedFind;

namespace RingSoft.DbLookup.EfCore
{
    public interface IAdvancedFindDbContextEfCore
    {
        DbSet<AdvancedFind.AdvancedFind> AdvancedFinds { get; set; }

        DbSet<AdvancedFindColumn> AdvancedFindColumns { get; set; }

        DbSet<AdvancedFindFilter> AdvancedFindFilters { get; set; }

        IAdvancedFindDbContextEfCore GetNewDbContext();
    }
}
