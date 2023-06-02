using System;
using Microsoft.EntityFrameworkCore;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.RecordLocking;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Microsoft.EntityFrameworkCore
{
    public static partial class CustomExtensions
    {
        public static IQueryable Query(this DbContext context, string entityName) =>
            context.Query(context.Model.FindEntityType(entityName).ClrType);

        static readonly MethodInfo SetMethod = typeof(DbContext).GetMethod(nameof(DbContext.Set), Type.EmptyTypes);

        public static IQueryable Query(this DbContext context, Type entityType) =>
            (IQueryable)SetMethod.MakeGenericMethod(entityType).Invoke(context, null);
    }
}

namespace RingSoft.DbLookup.EfCore
{
    public abstract class DbContextEfCore : DbContext, IAdvancedFindDbContextEfCore
    {
        public DbSet<AdvancedFind.AdvancedFind> AdvancedFinds { get; set; }
        public DbSet<AdvancedFindColumn> AdvancedFindColumns { get; set; }
        public DbSet<AdvancedFindFilter> AdvancedFindFilters { get; set; }

        public DbSet<RecordLock> RecordLocks { get; set; }

        public DbContextEfCore()
        {
            EfCoreGlobals.DbAdvancedFindContextCore = this;
            SystemGlobals.AdvancedFindDbProcessor = new AdvancedFindDataProcessorEfCore();
        }

        public virtual IAdvancedFindDbContextEfCore GetNewDbContext()
        {
            return GetNewDbContextEfCore();
        }


        public DbContext GetDbContextEf()
        {
            return this;
        }

        public abstract DbContextEfCore GetNewDbContextEfCore();

        public ILookupDataBase GetLookupDataBase<TEntity>(LookupDefinitionBase lookupDefinition, LookupUserInterface lookupUi) where TEntity : class, new()
        {
            return new LookupDataBase(lookupDefinition, lookupUi);
        }

        public bool SaveEntity<TEntity>(TEntity entity, string message) where TEntity : class
        {
            var result = GetDbContextEf().SaveEntity(Set<TEntity>(), entity, message);
            return result;
        }

        public bool SaveNoCommitEntity<TEntity>(TEntity entity, string message) where TEntity : class
        {
            var context = GetDbContextEf();
            if (!context.SaveNoCommitEntity(Set<TEntity>(), entity, message))
                return false;

            return true;
        }

        public bool DeleteEntity<TEntity>(TEntity entity, string message) where TEntity : class
        {
            return GetDbContextEf().DeleteEntity(Set<TEntity>(), entity, message);
        }

        public bool DeleteNoCommitEntity<TEntity>(TEntity entity, string message) where TEntity : class
        {
            return GetDbContextEf().DeleteNoCommitEntity(Set<TEntity>(), entity, message);
        }

        public bool AddNewNoCommitEntity<TEntity>(TEntity entity, string message) where TEntity : class
        {
            return GetDbContextEf().AddNewNoCommitEntity(Set<TEntity>(), entity, message);
        }

        public bool Commit(string message)
        {
            var result = GetDbContextEf().SaveEfChanges(message);

            return result;
        }

        public void RemoveRange<TEntity>(IEnumerable<TEntity> listToRemove) where TEntity : class
        {
            var dbSet = Set<TEntity>();

            dbSet.RemoveRange(listToRemove);
        }

        public void AddRange<TEntity>(List<TEntity> listToAdd) where TEntity : class
        {
            var dbSet = Set<TEntity>();

            dbSet.AddRange(listToAdd);
        }

        public IQueryable<TEntity> GetTable<TEntity>() where TEntity : class
        {
            var dbSet = Set<TEntity>();
            return dbSet;
        }

        public IQueryable GetTable(string tableName)
        {
            return this.Query(tableName);
        }
    }
}