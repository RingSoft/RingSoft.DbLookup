// ***********************************************************************
// Assembly         : RingSoft.DbLookup.EfCore
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 07-01-2023
// ***********************************************************************
// <copyright file="ExtensionMethods.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using Microsoft.EntityFrameworkCore;
using RingSoft.DbLookup.DataProcessor;

namespace RingSoft.DbLookup.EfCore
{
    /// <summary>
    /// Class ExtensionMethods.
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Adds or updates the entity and commits the changes.  Adding only works if the entity's primary key field is an auto increment type.  Outputs all errors to the DbDataProcessor SqlErrorViewer.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="context">The context.</param>
        /// <param name="dbSet">The database set.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="debugMessage">The debug message.</param>
        /// <param name="silent">if set to <c>true</c> [silent].</param>
        /// <returns>True if no errors occurred while saving.</returns>
        public static bool SaveEntity<TEntity>(this DbContext context, DbSet<TEntity> dbSet, TEntity entity,
            string debugMessage, bool silent = false) where TEntity : class
        {
            if (!SaveNoCommitEntity(context, dbSet, entity, debugMessage, silent))
                return false;

            return context.SaveEfChanges(debugMessage, silent);
        }

        /// <summary>
        /// Adds or updates the entity but does not commit the changes.  Only works if the entity's primary key field is an auto increment type.  Outputs all errors to the DbDataProcessor SqlErrorViewer.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="context">The context.</param>
        /// <param name="dbSet">The database set.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="debugMessage">The debug message.</param>
        /// <param name="silent">if set to <c>true</c> [silent].</param>
        /// <returns>True if no errors occurred while saving.</returns>
        public static bool SaveNoCommitEntity<TEntity>(this DbContext context, DbSet<TEntity> dbSet, TEntity entity,
            string debugMessage, bool silent = false) where TEntity : class
        {
            try
            {
                dbSet.Update(entity);
            }
            catch (Exception e)
            {
                ProcessException(e, debugMessage);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Adds a new entity and commits the changes.  Use for entities whose primary key field is not an auto-increment type.  Outputs all errors to the DbDataProcessor SqlErrorViewer.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="context">The context.</param>
        /// <param name="dbSet">The database set.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="debugMessage">The debug message.</param>
        /// <returns>True if no errors occured while saving.</returns>
        public static bool AddNewEntity<TEntity>(this DbContext context, DbSet<TEntity> dbSet, TEntity entity,
            string debugMessage) where TEntity : class
        {
            if (!AddNewNoCommitEntity(context, dbSet, entity, debugMessage))
                return false;

            GblMethods.LastError = string.Empty;
            return context.SaveEfChanges(debugMessage);
        }

        /// <summary>
        /// Adds a new entity but does not commit the changes.  Use for entities whose primary key field is not an auto-increment type.  Outputs all errors to the DbDataProcessor SqlErrorViewer.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="context">The context.</param>
        /// <param name="dbSet">The database set.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="debugMessage">The debug message.</param>
        /// <param name="silent">if set to <c>true</c> [silent].</param>
        /// <returns>True if no errors occured while saving.</returns>
        public static bool AddNewNoCommitEntity<TEntity>(this DbContext context, DbSet<TEntity> dbSet, TEntity entity,
            string debugMessage, bool silent = false) where TEntity : class
        {
            try
            {
                dbSet.Add(entity);
            }
            catch (Exception e)
            {
                ProcessException(e, debugMessage, silent);
                return false;
            }
            GblMethods.LastError = string.Empty;
            return true;
        }

        /// <summary>
        /// Deletes the entity and commits the changes.  Outputs all errors to the DbDataProcessor SqlErrorViewer.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="dbContext">The database context.</param>
        /// <param name="dbSet">The database set.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="debugMessage">The debug message.</param>
        /// <param name="silent">if set to <c>true</c> [silent].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool DeleteEntity<TEntity>(this DbContext dbContext, DbSet<TEntity> dbSet, TEntity entity,
            string debugMessage, bool silent = false) where TEntity : class
        {
            if (!DeleteNoCommitEntity(dbContext, dbSet, entity, debugMessage))
                return false;

            var result = dbContext.SaveEfChanges(debugMessage);
            if (result)
            {
                GblMethods.LastError = string.Empty;
            }
            return result;
        }

        /// <summary>
        /// Deletes the entity but does not commit the changes.  Outputs all errors to the DbDataProcessor SqlErrorViewer.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="dbContext">The database context.</param>
        /// <param name="dbSet">The database set.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="debugMessage">The debug message.</param>
        /// <param name="silent">if set to <c>true</c> [silent].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool DeleteNoCommitEntity<TEntity>(this DbContext dbContext, DbSet<TEntity> dbSet, TEntity entity,
            string debugMessage, bool silent = false) where TEntity : class
        {
            try
            {
                dbSet.Remove(entity);
            }
            catch (Exception e)
            {
                ProcessException(e, debugMessage);
                return false;
            }
            GblMethods.LastError = string.Empty;
            return true;
        }

        /// <summary>
        /// A wrapper around DbContext SaveChanges.  Saves the context changes and displays the exception to the user.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="debugMessage">The debug message.</param>
        /// <param name="silent">if set to <c>true</c> [silent].</param>
        /// <returns>True, if data was saved without exceptions.</returns>
        public static bool SaveEfChanges(this DbContext dbContext, string debugMessage, bool silent = false)
        {
            try
            {
                dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                ProcessException(e, debugMessage, silent);
                return false;
            }
            GblMethods.LastError = string.Empty;
            return true;
        }

        /// <summary>
        /// Processes the exception.
        /// </summary>
        /// <param name="e">The e.</param>
        /// <param name="debugMessage">The debug message.</param>
        /// <param name="silent">if set to <c>true</c> [silent].</param>
        public static void ProcessException(this Exception e, string debugMessage, bool silent = false)
        {
            var exception = e;
            if (exception.InnerException != null)
                exception = exception.InnerException;

            if (!silent)
            {
                DbDataProcessor.DisplayDataException(exception, debugMessage);
            }

            GblMethods.LastError = exception.Message;
        }
    }
}
