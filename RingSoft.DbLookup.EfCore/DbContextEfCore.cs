// ***********************************************************************
// Assembly         : RingSoft.DbLookup.EfCore
// Author           : petem
// Created          : 05-31-2023
//
// Last Modified By : petem
// Last Modified On : 12-16-2023
// ***********************************************************************
// <copyright file="DbContextEfCore.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
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
    /// <summary>
    /// Class CustomExtensions.
    /// </summary>
    public static partial class CustomExtensions
    {
        /// <summary>
        /// Queries the specified entity name.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="entityName">Name of the entity.</param>
        /// <returns>IQueryable.</returns>
        public static IQueryable Query(this DbContext context, string entityName) =>
            context.Query(context.Model.FindEntityType(entityName).ClrType);

        /// <summary>
        /// The set method
        /// </summary>
        static readonly MethodInfo SetMethod = typeof(DbContext).GetMethod(nameof(DbContext.Set), Type.EmptyTypes);

        /// <summary>
        /// Queries the specified entity type.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="entityType">Type of the entity.</param>
        /// <returns>IQueryable.</returns>
        public static IQueryable Query(this DbContext context, Type entityType) =>
            (IQueryable)SetMethod.MakeGenericMethod(entityType).Invoke(context, null);
    }
}

namespace RingSoft.DbLookup.EfCore
{
    /// <summary>
    /// The base class for all Entity Framework DbContext classes.
    /// Implements the <see cref="DbContext" />
    /// Implements the <see cref="RingSoft.DbLookup.IDbContext" />
    /// </summary>
    /// <seealso cref="DbContext" />
    /// <seealso cref="RingSoft.DbLookup.IDbContext" />
    public abstract class DbContextEfCore : DbContext, IDbContext
    {
        ~DbContextEfCore()
        {
            Dispose();
            GC.Collect();
        }
        /// <summary>
        /// Configures the advanced find.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        public static void ConfigureAdvancedFind(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new RecordLockConfiguration());
            modelBuilder.ApplyConfiguration(new AdvancedFindConfiguration());
            modelBuilder.ApplyConfiguration(new AdvancedFindColumnConfiguration());
            modelBuilder.ApplyConfiguration(new AdvancedFindFilterConfiguration());

        }

        /// <summary>
        /// Gets or sets the advanced finds.
        /// </summary>
        /// <value>The advanced finds.</value>
        public DbSet<AdvancedFind.AdvancedFind> AdvancedFinds { get; set; }
        /// <summary>
        /// Gets or sets the advanced find columns.
        /// </summary>
        /// <value>The advanced find columns.</value>
        public DbSet<AdvancedFindColumn> AdvancedFindColumns { get; set; }
        /// <summary>
        /// Gets or sets the advanced find filters.
        /// </summary>
        /// <value>The advanced find filters.</value>
        public DbSet<AdvancedFindFilter> AdvancedFindFilters { get; set; }

        /// <summary>
        /// Gets or sets the record locks.
        /// </summary>
        /// <value>The record locks.</value>
        public DbSet<RecordLock> RecordLocks { get; set; }

        /// <summary>
        /// Gets the database context ef.
        /// </summary>
        /// <returns>DbContext.</returns>
        public DbContext GetDbContextEf()
        {
            return this;
        }

        /// <summary>
        /// Override this method to further configure the model that was discovered by convention from the entity types
        /// exposed in <see cref="T:Microsoft.EntityFrameworkCore.DbSet`1" /> properties on your derived context. The resulting model may be cached
        /// and re-used for subsequent instances of your derived context.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context. Databases (and other extensions) typically
        /// define extension methods on this object that allow you to configure aspects of the model that are specific
        /// to a given database.</param>
        /// <remarks><para>
        /// If a model is explicitly set on the options for this context (via <see cref="M:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.UseModel(Microsoft.EntityFrameworkCore.Metadata.IModel)" />)
        /// then this method will not be run. However, it will still run when creating a compiled model.
        /// </para>
        /// <para>
        /// See <see href="https://aka.ms/efcore-docs-modeling">Modeling entity types and relationships</see> for more information and
        /// examples.
        /// </para></remarks>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureAdvancedFind(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// Gets the new database context ef core.
        /// </summary>
        /// <returns>DbContextEfCore.</returns>
        public abstract DbContextEfCore GetNewDbContextEfCore();

        /// <summary>
        /// Saves the entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="message">The message.</param>
        /// <param name="silent">if set to <c>true</c> [silent].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool SaveEntity<TEntity>(TEntity entity, string message, bool silent = false) where TEntity : class, new()
        {
            var result = GetDbContextEf().SaveEntity(Set<TEntity>(), entity, message, silent);
            return result;
        }

        /// <summary>
        /// Gets the lookup data base.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="lookupDefinition">The lookup definition.</param>
        /// <param name="tableDefinition">The table definition.</param>
        /// <returns>ILookupDataBase.</returns>
        public ILookupDataBase GetLookupDataBase<TEntity>(LookupDefinitionBase lookupDefinition, TableDefinition<TEntity> tableDefinition) where TEntity : class, new()
        {
            return tableDefinition.LookupDefinition.GetLookupDataMaui(lookupDefinition, false);
        }

        /// <summary>
        /// Saves the no commit entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="message">The message.</param>
        /// <param name="silent">if set to <c>true</c> [silent].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool SaveNoCommitEntity<TEntity>(TEntity entity, string message, bool silent = false) where TEntity : class, new()
        {
            var context = GetDbContextEf();
            if (!context.SaveNoCommitEntity(Set<TEntity>(), entity, message, silent))
                return false;

            return true;
        }

        /// <summary>
        /// Deletes the entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="message">The message.</param>
        /// <param name="silent">if set to <c>true</c> [silent].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool DeleteEntity<TEntity>(TEntity entity, string message, bool silent = false) where TEntity : class, new()
        {
            return GetDbContextEf().DeleteEntity(Set<TEntity>(), entity, message, silent);
        }

        /// <summary>
        /// Deletes the no commit entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="message">The message.</param>
        /// <param name="silent">if set to <c>true</c> [silent].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool DeleteNoCommitEntity<TEntity>(TEntity entity, string message, bool silent = false) where TEntity : class, new()
        {
            return GetDbContextEf().DeleteNoCommitEntity(Set<TEntity>(), entity, message, silent);
        }

        /// <summary>
        /// Adds the new no commit entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="message">The message.</param>
        /// <param name="silent">if set to <c>true</c> [silent].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool AddNewNoCommitEntity<TEntity>(TEntity entity, string message, bool silent = false) where TEntity : class, new()
        {
            return GetDbContextEf().AddNewNoCommitEntity(Set<TEntity>(), entity, message, silent);
        }

        /// <summary>
        /// Adds the save entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="message">The message.</param>
        /// <param name="silent">if set to <c>true</c> [silent].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool AddSaveEntity<TEntity>(TEntity entity, string message, bool silent = false) where TEntity : class, new()
        {
            var list = new List<TEntity>();
            list.Add(entity);
            AddRange(list);
            return Commit(message, silent);
        }

        /// <summary>
        /// Commits the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="silent">if set to <c>true</c> [silent].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool Commit(string message, bool silent = false)
        {
            var result = GetDbContextEf().SaveEfChanges(message, silent);

            return result;
        }

        /// <summary>
        /// Removes the range.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="listToRemove">The list to remove.</param>
        public void RemoveRange<TEntity>(IEnumerable<TEntity> listToRemove) where TEntity : class, new()
        {
            var dbSet = Set<TEntity>();

            dbSet.RemoveRange(listToRemove);
        }

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="listToAdd">The list to add.</param>
        public void AddRange<TEntity>(List<TEntity> listToAdd) where TEntity : class, new()
        {
            var dbSet = Set<TEntity>();

            dbSet.AddRange(listToAdd);
        }

        /// <summary>
        /// Gets the table.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <returns>IQueryable&lt;TEntity&gt;.</returns>
        public IQueryable<TEntity> GetTable<TEntity>() where TEntity : class, new()
        {
            var dbSet = Set<TEntity>();
            return dbSet;
        }

        /// <summary>
        /// Sets the identity insert.
        /// </summary>
        /// <param name="processor">The processor.</param>
        /// <param name="tableDefinition">The table definition.</param>
        /// <param name="silent">if set to <c>true</c> [silent].</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
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

        /// <summary>
        /// Opens the connection.
        /// </summary>
        /// <param name="silent">if set to <c>true</c> [silent].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
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
                    var msg = e.Message;
                    if (e.InnerException != null)
                    {
                        msg = e.InnerException.Message;
                    }
                    ControlsGlobals.UserInterface.ShowMessageBox(msg, "Error", RsMessageBoxIcons.Error);
                }

                return false;
            }

            if (silent)
            {
                GblMethods.LastError = string.Empty;
            }
            return true;
        }

        /// <summary>
        /// Closes the connection.
        /// </summary>
        /// <param name="silent">if set to <c>true</c> [silent].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
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

        /// <summary>
        /// Executes the SQL.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <param name="silent">if set to <c>true</c> [silent].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
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

        /// <summary>
        /// Gets the list of databases.
        /// </summary>
        /// <param name="dataProcessor">The data processor.</param>
        /// <returns>List&lt;System.String&gt;.</returns>
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

        /// <summary>
        /// Sets the processor.
        /// </summary>
        /// <param name="processor">The processor.</param>
        public abstract void SetProcessor(DbDataProcessor processor);
        /// <summary>
        /// Sets the connection string.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public abstract void SetConnectionString(string? connectionString);

        /// <summary>
        /// Gets the table.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns>IQueryable.</returns>
        public IQueryable GetTable(string tableName)
        {
            return this.Query(tableName);
        }
    }
}