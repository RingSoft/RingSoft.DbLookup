using System;
using Microsoft.EntityFrameworkCore;
using RingSoft.DbLookup.DataProcessor;

namespace RingSoft.DbLookup.EfCore
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Adds or updates the entity.  Adding only works if the entity's primary key field is an auto increment type.  Outputs all errors to the DbDataProcessor SqlErrorViewer.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="context">The context.</param>
        /// <param name="dbSet">The database set.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="debugMessage">The debug message.</param>
        /// <returns>
        /// True if no errors occured while saving.
        /// </returns>
        public static bool SaveEntity<TEntity>(this DbContext context, DbSet<TEntity> dbSet, TEntity entity,
            string debugMessage) where TEntity : class
        {
            try
            {
                dbSet.Update(entity);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                var exception = e;
                if (exception.InnerException != null)
                    exception = exception.InnerException;

                DbDataProcessor.DisplayDataException(exception, debugMessage);

                return false;
            }

            return true;
        }

        /// <summary>
        /// Adds a new entity.  Use for entities whose primary key field is not an auto-increment type.  Outputs all errors to the DbDataProcessor SqlErrorViewer.
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
            try
            {
                dbSet.Add(entity);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                var exception = e;
                if (exception.InnerException != null)
                    exception = exception.InnerException;

                DbDataProcessor.DisplayDataException(exception, debugMessage);

                return false;
            }

            return true;
        }

        /// <summary>
        /// Deletes the entity.  Outputs all errors to the DbDataProcessor SqlErrorViewer.
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
            try
            {
                dbSet.Remove(entity);
                dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                var exception = e;
                if (exception.InnerException != null)
                    exception = exception.InnerException;

                DbDataProcessor.DisplayDataException(exception, debugMessage);
                return false;
            }

            return true;
        }
    }
}
