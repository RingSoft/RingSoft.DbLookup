// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 07-13-2023
//
// Last Modified By : petem
// Last Modified On : 12-26-2023
// ***********************************************************************
// <copyright file="TestDataRegistry.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.RecordLocking;

namespace RingSoft.DbLookup.Testing
{
    /// <summary>
    /// Class DataRepositoryRegistryItemBase.
    /// </summary>
    public abstract class DataRepositoryRegistryItemBase
    {
        /// <summary>
        /// Gets the entity.
        /// </summary>
        /// <value>The entity.</value>
        public Type Entity { get; internal set; }

        /// <summary>
        /// Clears the data.
        /// </summary>
        public abstract void ClearData();
    }

    /// <summary>
    /// Class DataRepositoryRegistryItem.
    /// Implements the <see cref="RingSoft.DbLookup.Testing.DataRepositoryRegistryItemBase" />
    /// </summary>
    /// <typeparam name="TEntity">The type of the t entity.</typeparam>
    /// <seealso cref="RingSoft.DbLookup.Testing.DataRepositoryRegistryItemBase" />
    public class DataRepositoryRegistryItem<TEntity> : DataRepositoryRegistryItemBase where TEntity : new()
    {
        /// <summary>
        /// Gets the table.
        /// </summary>
        /// <value>The table.</value>
        public List<TEntity> Table { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataRepositoryRegistryItem{TEntity}" /> class.
        /// </summary>
        public DataRepositoryRegistryItem()
        {
            Table = new List<TEntity>();
            Entity = typeof(TEntity);
        }

        /// <summary>
        /// Clears the data.
        /// </summary>
        public override void ClearData()
        {
            Table.Clear();
        }
    }

    /// <summary>
    /// Class DataRepositoryRegistry.
    /// Implements the <see cref="RingSoft.DbLookup.IDbContext" />
    /// </summary>
    /// <seealso cref="RingSoft.DbLookup.IDbContext" />
    public class DataRepositoryRegistry : IDbContext
    {
        /// <summary>
        /// The last error
        /// </summary>
        private string _lastError;

        /// <summary>
        /// Gets the entities.
        /// </summary>
        /// <value>The entities.</value>
        public List<DataRepositoryRegistryItemBase> Entities { get; } =
            new List<DataRepositoryRegistryItemBase>();

        /// <summary>
        /// Gets the database context.
        /// </summary>
        /// <value>The database context.</value>
        public DataRepositoryRegistry DbContext { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataRepositoryRegistry" /> class.
        /// </summary>
        public DataRepositoryRegistry()
        {
            AddEntity(new DataRepositoryRegistryItem<RecordLock>());
        }

        /// <summary>
        /// Adds the entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void AddEntity(DataRepositoryRegistryItemBase entity)
        {
            Entities.Add(entity);
        }

        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <returns>List&lt;TEntity&gt;.</returns>
        public List<TEntity> GetList<TEntity>() where TEntity : class, new()
        {
            var result = new List<TEntity>();
            var entity = Entities.FirstOrDefault(p => p.Entity == typeof(TEntity));
            if (entity != null)
            {
                var existingEntity = entity as DataRepositoryRegistryItem<TEntity>;
                return existingEntity.Table;
            }

            return result;
        }

        /// <summary>
        /// Gets the entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <returns>IQueryable&lt;TEntity&gt;.</returns>
        public IQueryable<TEntity> GetEntity<TEntity>() where TEntity : class, new()
        {
            var entity = Entities.FirstOrDefault(p => p.Entity == typeof(TEntity));
            if (entity != null)
            {
                var existingEntity = entity as DataRepositoryRegistryItem<TEntity>;
                return existingEntity.Table.AsQueryable();
            }

            return null;
        }

        /// <summary>
        /// Gets the lookup data base.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="lookupDefinition">The lookup definition.</param>
        /// <param name="tableDefinition">The table definition.</param>
        /// <returns>ILookupDataBase.</returns>
        public ILookupDataBase GetLookupDataBase<TEntity>(LookupDefinitionBase lookupDefinition
            , TableDefinition<TEntity> tableDefinition) where TEntity : class, new()
        {
            var table = GetTable<TEntity>();
            return new TestLookupDataBase<TEntity>(table, tableDefinition);
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
            var tableDef = GblMethods.GetTableDefinition<TEntity>();
            var table = GetList<TEntity>();
            var indexEntity = -1;
            var foundEntity = false;
            foreach (var entity1 in table)
            {
                indexEntity++;
                if (tableDef.IsEqualTo(entity1, entity))
                {
                    foundEntity = true;
                    break;
                }
            }

            if (foundEntity)
            {
                table[indexEntity] = entity;
            }
            else
            {
                table.Add(entity);
                SetNewIdentity(tableDef, entity);
            }
            return true;
        }

        /// <summary>
        /// Sets the new identity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="tableDef">The table definition.</param>
        /// <param name="entity">The entity.</param>
        private void SetNewIdentity<TEntity>(TableDefinition<TEntity> tableDef, TEntity entity) where TEntity : class, new()
        {
            if (tableDef.IsIdentity())
            {
                var identField = tableDef.GetIdentityField();
                var value = GblMethods.GetPropertyValue(entity, identField.PropertyName).ToInt();
                if (value == 0)
                {
                    var table = GetList<TEntity>();
                    var maxId = 0;
                    foreach (var entity1 in table)
                    {
                        var value1 = GblMethods.GetPropertyValue(entity1, identField.PropertyName).ToInt();
                        if (value1 > maxId)
                        {
                            maxId = value1;
                        }
                    }

                    maxId++;
                    GblMethods.SetPropertyValue(entity, identField.PropertyName, maxId.ToString());
                }
            }
        }

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
            return SaveNoCommitEntity(entity, message);
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
            var table = GetList<TEntity>();
            if (table.Contains(entity))
            {
                table.Remove(entity);
            }
            return true;
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
            return DeleteEntity(entity, message);
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
            var table = GetList<TEntity>();
            table.Add(entity);
            SetNewIdentity(GblMethods.GetTableDefinition<TEntity>(), entity);
            return true;
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
            return AddNewNoCommitEntity(entity, message, silent);
        }

        /// <summary>
        /// Commits the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="silent">if set to <c>true</c> [silent].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool Commit(string message, bool silent = false)
        {
            return true;
        }

        /// <summary>
        /// Removes the range.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="listToRemove">The list to remove.</param>
        public void RemoveRange<TEntity>(IEnumerable<TEntity> listToRemove) where TEntity : class, new()
        {
            var table = GetList<TEntity>();
            var listToParse = listToRemove.ToList();
            foreach (var entity in listToParse)
            {
                var existObj = table.FirstOrDefault(
                    p => p.IsEqualTo(entity));
                if (existObj != null)
                {
                    table.Remove(existObj);
                }
            }
        }

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="listToAdd">The list to add.</param>
        public void AddRange<TEntity>(List<TEntity> listToAdd) where TEntity : class, new()
        {
            var table = GetList<TEntity>();
            table.AddRange(listToAdd);
        }

        /// <summary>
        /// Gets the table.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <returns>IQueryable&lt;TEntity&gt;.</returns>
        public IQueryable<TEntity> GetTable<TEntity>() where TEntity : class, new()
        {
            return GetList<TEntity>().AsQueryable();
        }


        /// <summary>
        /// Sets the identity insert.
        /// </summary>
        /// <param name="processor">The processor.</param>
        /// <param name="tableDefinition">The table definition.</param>
        /// <param name="silent">if set to <c>true</c> [silent].</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public void SetIdentityInsert(DbDataProcessor processor, TableDefinitionBase tableDefinition, bool silent = false, bool value = true)
        {

        }

        /// <summary>
        /// Opens the connection.
        /// </summary>
        /// <param name="silent">if set to <c>true</c> [silent].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool OpenConnection(bool silent = false)
        {
            return true;
        }

        /// <summary>
        /// Closes the connection.
        /// </summary>
        /// <param name="silent">if set to <c>true</c> [silent].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool CloseConnection(bool silent = false)
        {
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
            return true;
        }

        /// <summary>
        /// Gets the list of databases.
        /// </summary>
        /// <param name="dataProcessor">The data processor.</param>
        /// <returns>List&lt;System.String&gt;.</returns>
        public List<string> GetListOfDatabases(DbDataProcessor dataProcessor)
        {
            return new List<string>();
        }

        /// <summary>
        /// Sets the processor.
        /// </summary>
        /// <param name="processor">The processor.</param>
        public void SetProcessor(DbDataProcessor processor)
        {

        }

        /// <summary>
        /// Sets the connection string.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public void SetConnectionString(string connectionString)
        {
        }

        /// <summary>
        /// Gets the table.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns>IQueryable.</returns>
        public IQueryable GetTable(string tableName)
        {
            return null;
        }
    }

    /// <summary>
    /// Class TestDataRepository.
    /// Implements the <see cref="RingSoft.DbLookup.SystemDataRepository" />
    /// </summary>
    /// <seealso cref="RingSoft.DbLookup.SystemDataRepository" />
    public class TestDataRepository : SystemDataRepository
    {
        /// <summary>
        /// Gets the data context.
        /// </summary>
        /// <value>The data context.</value>
        public DataRepositoryRegistry DataContext { get; }
        /// <summary>
        /// Initializes a new instance of the <see cref="TestDataRepository" /> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public TestDataRepository(DataRepositoryRegistry context)
        {
            DataContext = context;
        }

        /// <summary>
        /// Gets the data context.
        /// </summary>
        /// <returns>IDbContext.</returns>
        public override IDbContext GetDataContext()
        {
            return DataContext;
        }

        /// <summary>
        /// Gets the data context.
        /// </summary>
        /// <param name="dataProcessor">The data processor.</param>
        /// <returns>IDbContext.</returns>
        public override IDbContext GetDataContext(DbDataProcessor dataProcessor)
        {
            return DataContext;
        }

        /// <summary>
        /// Clears the data.
        /// </summary>
        public void ClearData()
        {
            foreach (var entity in DataContext.Entities)
            {
                entity.ClearData();
            }
        }

    }
}
