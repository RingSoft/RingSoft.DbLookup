// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-16-2023
// ***********************************************************************
// <copyright file="IDbContext.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;

namespace RingSoft.DbLookup
{
    /// <summary>
    /// Interface IDbContext
    /// </summary>
    public interface IDbContext : IDisposable
    {
        /// <summary>
        /// Gets the lookup data base.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="lookupDefinition">The lookup definition.</param>
        /// <param name="tableDefinition">The table definition.</param>
        /// <returns>ILookupDataBase.</returns>
        ILookupDataBase GetLookupDataBase<TEntity>(LookupDefinitionBase lookupDefinition
            , TableDefinition<TEntity> tableDefinition)
            where TEntity : class, new();

        /// <summary>
        /// Saves without committing the entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="message">The message.</param>
        /// <param name="silent">if set to <c>true</c> [silent].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool SaveNoCommitEntity<TEntity>(TEntity entity, string message, bool silent = false) where TEntity : class, new();

        /// <summary>
        /// Saves the entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="message">The message.</param>
        /// <param name="silent">if set to <c>true</c> [silent].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool SaveEntity<TEntity>(TEntity entity, string message, bool silent = false) where TEntity : class, new();

        /// <summary>
        /// Deletes the entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="message">The message.</param>
        /// <param name="silent">if set to <c>true</c> [silent].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool DeleteEntity<TEntity>(TEntity entity, string message, bool silent = false) where TEntity : class, new();

        /// <summary>
        /// Deletes the no commit entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="message">The message.</param>
        /// <param name="silent">if set to <c>true</c> [silent].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool DeleteNoCommitEntity<TEntity>(TEntity entity, string message, bool silent = false) where TEntity : class, new();

        /// <summary>
        /// Adds the new no commit entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="message">The message.</param>
        /// <param name="silent">if set to <c>true</c> [silent].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool AddNewNoCommitEntity<TEntity>(TEntity entity, string message, bool silent = false) where TEntity : class, new();

        /// <summary>
        /// Adds the save entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="message">The message.</param>
        /// <param name="silent">if set to <c>true</c> [silent].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool AddSaveEntity<TEntity>(TEntity entity, string message, bool silent = false) where TEntity : class, new();

        /// <summary>
        /// Commits the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="silent">if set to <c>true</c> [silent].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool Commit(string message, bool silent = false);

        /// <summary>
        /// Removes the range.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="listToRemove">The list to remove.</param>
        void RemoveRange<TEntity>(IEnumerable<TEntity> listToRemove) where TEntity : class, new();

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="listToAdd">The list to add.</param>
        void AddRange<TEntity>(List<TEntity> listToAdd) where TEntity : class, new();

        /// <summary>
        /// Gets the table.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <returns>IQueryable&lt;TEntity&gt;.</returns>
        IQueryable<TEntity> GetTable<TEntity>() where TEntity : class, new();

        /// <summary>
        /// Sets the identity insert.
        /// </summary>
        /// <param name="processor">The processor.</param>
        /// <param name="tableDefinition">The table definition.</param>
        /// <param name="silent">if set to <c>true</c> [silent].</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        void SetIdentityInsert(DbDataProcessor processor, TableDefinitionBase tableDefinition, bool silent = false
            , bool value = true);

        /// <summary>
        /// Opens the connection.
        /// </summary>
        /// <param name="silent">if set to <c>true</c> [silent].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool OpenConnection(bool silent = false);

        /// <summary>
        /// Closes the connection.
        /// </summary>
        /// <param name="silent">if set to <c>true</c> [silent].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool CloseConnection(bool silent = false);

        /// <summary>
        /// Executes the SQL.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <param name="silent">if set to <c>true</c> [silent].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool ExecuteSql(string sql, bool silent = false);

        /// <summary>
        /// Gets the list of databases.
        /// </summary>
        /// <param name="dataProcessor">The data processor.</param>
        /// <returns>List&lt;System.String&gt;.</returns>
        List<string> GetListOfDatabases(DbDataProcessor dataProcessor);

        /// <summary>
        /// Sets the processor.
        /// </summary>
        /// <param name="processor">The processor.</param>
        void SetProcessor(DbDataProcessor processor);

        /// <summary>
        /// Sets the connection string.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        void SetConnectionString(string? connectionString);
    }
}
