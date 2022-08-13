﻿using System;
using Microsoft.EntityFrameworkCore;
using RingSoft.DbLookup.AdvancedFind;

namespace RingSoft.DbLookup.EfCore
{
    public interface IAdvancedFindDbContextEfCore : IDisposable
    {
        DbSet<AdvancedFind.AdvancedFind> AdvancedFinds { get; set; }

        DbSet<AdvancedFindColumn> AdvancedFindColumns { get; set; }

        DbSet<AdvancedFindFilter> AdvancedFindFilters { get; set; }

        DbContext GetDbContextEf();

        IAdvancedFindDbContextEfCore GetNewDbContext();
    }
}