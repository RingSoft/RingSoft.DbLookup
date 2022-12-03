using System;
using Microsoft.EntityFrameworkCore;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.RecordLocking;

namespace RingSoft.DbLookup.EfCore
{
    public interface IAdvancedFindDbContextEfCore : IDisposable, IDbContext
    {
        DbSet<RecordLock> RecordLocks { get; set; }

        DbSet<AdvancedFind.AdvancedFind> AdvancedFinds { get; set; }

        DbSet<AdvancedFindColumn> AdvancedFindColumns { get; set; }

        DbSet<AdvancedFindFilter> AdvancedFindFilters { get; set; }

        DbContext GetDbContextEf();

        IAdvancedFindDbContextEfCore GetNewDbContext();
    }
}
