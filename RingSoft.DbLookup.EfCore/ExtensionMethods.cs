using System;
using Microsoft.EntityFrameworkCore;
using RingSoft.DbLookup.DataProcessor;

namespace RingSoft.DbLookup.EfCore
{
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
        /// <returns>
        /// True if no errors occurred while saving.
        /// </returns>
        public static bool SaveEntity<TEntity>(this DbContext context, DbSet<TEntity> dbSet, TEntity entity,
            string debugMessage) where TEntity : class
        {
            if (!SaveNoCommitEntity(context, dbSet, entity, debugMessage))
                return false;

            return context.SaveEfChanges(debugMessage);
        }

        /// <summary>
        /// Adds or updates the entity but does not commit the changes.  Only works if the entity's primary key field is an auto increment type.  Outputs all errors to the DbDataProcessor SqlErrorViewer.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="context">The context.</param>
        /// <param name="dbSet">The database set.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="debugMessage">The debug message.</param>
        /// <returns>
        /// True if no errors occurred while saving.
        /// </returns>
        public static bool SaveNoCommitEntity<TEntity>(this DbContext context, DbSet<TEntity> dbSet, TEntity entity,
            string debugMessage) where TEntity : class
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
        /// <returns>
        /// True if no errors occured while saving.
        /// </returns>
        public static bool AddNewEntity<TEntity>(this DbContext context, DbSet<TEntity> dbSet, TEntity entity,
            string debugMessage) where TEntity : class
        {
            if (!AddNewNoCommitEntity(context, dbSet, entity, debugMessage))
                return false;

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
        /// <returns>
        /// True if no errors occured while saving.
        /// </returns>
        public static bool AddNewNoCommitEntity<TEntity>(this DbContext context, DbSet<TEntity> dbSet, TEntity entity,
            string debugMessage) where TEntity : class
        {
            try
            {
                dbSet.Add(entity);
            }
            catch (Exception e)
            {
                ProcessException(e, debugMessage);
                return false;
            }

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
        /// <returns></returns>
        public static bool DeleteEntity<TEntity>(this DbContext dbContext, DbSet<TEntity> dbSet, TEntity entity,
            string debugMessage) where TEntity : class
        {
            if (!DeleteNoCommitEntity(dbContext, dbSet, entity, debugMessage))
                return false;

            return dbContext.SaveEfChanges(debugMessage);
        }

        /// <summary>
        /// Deletes the entity but does not commit the changes.  Outputs all errors to the DbDataProcessor SqlErrorViewer.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="dbContext">The database context.</param>
        /// <param name="dbSet">The database set.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="debugMessage">The debug message.</param>
        /// <returns></returns>
        public static bool DeleteNoCommitEntity<TEntity>(this DbContext dbContext, DbSet<TEntity> dbSet, TEntity entity,
            string debugMessage) where TEntity : class
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

            return true;
        }

        /// <summary>
        /// A wrapper around DbContext SaveChanges.  Saves the context changes and displays the exception to the user.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="debugMessage">The debug message.</param>
        /// <returns>True, if data was saved without exceptions.</returns>
        public static bool SaveEfChanges(this DbContext dbContext, string debugMessage)
        {
            try
            {
                dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                ProcessException(e, debugMessage);
                return false;
            }

            return true;
        }

        public static void ProcessException(this Exception e, string debugMessage)
        {
            var exception = e;
            if (exception.InnerException != null)
                exception = exception.InnerException;

            DbDataProcessor.DisplayDataException(exception, debugMessage);
        }
    }
}
