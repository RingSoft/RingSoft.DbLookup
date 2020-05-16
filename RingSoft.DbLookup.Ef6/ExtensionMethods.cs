﻿using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using RingSoft.DbLookup.DataProcessor;

namespace RingSoft.DbLookup.Ef6
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Saves the entity.  Outputs all errors to the DbDataProcessor SqlErrorViewer.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="dbContext">The database context.</param>
        /// <param name="dbSet">The database set.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="debugMessage">The debug message.</param>
        /// <returns>
        /// True if no errors occurred during saving.
        /// </returns>
        public static bool SaveEntity<TEntity>(this DbContext dbContext, DbSet<TEntity> dbSet, TEntity entity,
            string debugMessage) where TEntity : class
        {
            try
            {
                dbSet.AddOrUpdate(entity);
                dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                var exception = e;
                if (exception.InnerException != null)
                    exception = exception.InnerException;
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
                if (exception.InnerException != null)
                    exception = exception.InnerException;

                DbDataProcessor.DisplayDataException(exception, debugMessage);
                return false;
            }

            return true;
        }
    }
}
