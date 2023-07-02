using System;
using Microsoft.EntityFrameworkCore;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.RecordLocking;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.DataProcessor;

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

        public bool SaveEntity<TEntity>(TEntity entity, string message, bool silent = false) where TEntity : class, new()
        {
            var result = GetDbContextEf().SaveEntity(Set<TEntity>(), entity, message, silent);
            return result;
        }

        public ILookupDataBase GetLookupDataBase<TEntity>(LookupDefinitionBase lookupDefinition, TableDefinition<TEntity> tableDefinition) where TEntity : class, new()
        {
            return tableDefinition.LookupDefinition.GetLookupDataMaui(lookupDefinition, false);
        }

        public bool SaveNoCommitEntity<TEntity>(TEntity entity, string message, bool silent = false) where TEntity : class, new()
        {
            var context = GetDbContextEf();
            if (!context.SaveNoCommitEntity(Set<TEntity>(), entity, message, silent))
                return false;

            return true;
        }

        public bool DeleteEntity<TEntity>(TEntity entity, string message, bool silent = false) where TEntity : class, new()
        {
            return GetDbContextEf().DeleteEntity(Set<TEntity>(), entity, message, silent);
        }

        public bool DeleteNoCommitEntity<TEntity>(TEntity entity, string message, bool silent = false) where TEntity : class, new()
        {
            return GetDbContextEf().DeleteNoCommitEntity(Set<TEntity>(), entity, message, silent);
        }

        public bool AddNewNoCommitEntity<TEntity>(TEntity entity, string message, bool silent = false) where TEntity : class, new()
        {
            return GetDbContextEf().AddNewNoCommitEntity(Set<TEntity>(), entity, message, silent);
        }

        public bool Commit(string message, bool silent = false)
        {
            var result = GetDbContextEf().SaveEfChanges(message, silent);

            return result;
        }

        public void RemoveRange<TEntity>(IEnumerable<TEntity> listToRemove) where TEntity : class, new()
        {
            var dbSet = Set<TEntity>();

            dbSet.RemoveRange(listToRemove);
        }

        public void AddRange<TEntity>(List<TEntity> listToAdd) where TEntity : class, new()
        {
            var dbSet = Set<TEntity>();

            dbSet.AddRange(listToAdd);
        }

        public IQueryable<TEntity> GetTable<TEntity>() where TEntity : class, new()
        {
            var dbSet = Set<TEntity>();
            return dbSet;
        }

        public void SetIdentityInsert(DbDataProcessor processor, TableDefinitionBase tableDefinition
            , bool silent = false, bool value = true)
        {

            var sql = processor.GetIdentityInsertSql(tableDefinition.TableName, value);
            if (sql.IsNullOrEmpty())
            {
                return;
            }

            if (!OpenConnection(silent))
            {
                return;
            }

            if (!ExecuteSql(sql))
            {
                CloseConnection();
                return;
            }

            if (!value)
            {
                CloseConnection();
            }
        }

        public bool OpenConnection(bool silent = false)
        {
            try
            {
                Database.OpenConnection();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                if (silent)
                {
                    GblMethods.LastError = e.Message;
                }
                else
                {
                    ControlsGlobals.UserInterface.ShowMessageBox(e.Message, "Error", RsMessageBoxIcons.Error);
                }

                return false;
            }

            if (silent)
            {
                GblMethods.LastError = string.Empty;
            }
            return true;
        }

        public bool CloseConnection(bool silent = false)
        {
            try
            {
                Database.CloseConnection();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                if (silent)
                {
                    GblMethods.LastError = e.Message;
                }
                else
                {
                    ControlsGlobals.UserInterface.ShowMessageBox(e.Message, "Error", RsMessageBoxIcons.Error);
                }

                return false;
            }

            if (silent)
            {
                GblMethods.LastError = string.Empty;
            }
            return true;
        }

        public bool ExecuteSql(string sql, bool silent = false)
        {
            try
            {
                Database.ExecuteSqlRaw(sql);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                if (silent)
                {
                    GblMethods.LastError += e.Message;
                }
                else
                {
                    ControlsGlobals.UserInterface.ShowMessageBox(e.Message, "Error", RsMessageBoxIcons.Error);
                }

                return false;
            }
            GblMethods.LastError = string.Empty;
            return true;
        }

        public List<string> GetListOfDatabases(DbDataProcessor dataProcessor)
        {
            var result = new List<string>();

            var listSql = dataProcessor.GetDatabaseListSql();

            if (listSql.IsNullOrEmpty())
                return result;

            SetConnectionString(dataProcessor.ConnectionString);

            if (!OpenConnection())
            {
                SetConnectionString(null);
                return result;
            }

            try
            {
                result = Database.SqlQueryRaw<string>(listSql).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return result;
            }

            CloseConnection();

            SetConnectionString(null);
            GblMethods.LastError = string.Empty;
            return result;
        }

        public abstract void SetProcessor(DbDataProcessor processor);
        public abstract void SetConnectionString(string? connectionString);

        public IQueryable GetTable(string tableName)
        {
            return this.Query(tableName);
        }
    }
}