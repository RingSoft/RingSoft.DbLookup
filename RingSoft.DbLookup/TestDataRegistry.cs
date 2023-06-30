﻿using System;
using System.Collections.Generic;
using System.Linq;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;

namespace RingSoft.DbLookup
{
    public abstract class DataRepositoryRegistryItemBase
    {
        public Type Entity { get; internal set; }

        public abstract void ClearData();
    }

    public class DataRepositoryRegistryItem<TEntity> : DataRepositoryRegistryItemBase where TEntity : new()
    {
        public List<TEntity> Table { get; private set; }

        public DataRepositoryRegistryItem(TEntity entity)
        {
            Table = new List<TEntity>();
            Entity = typeof(TEntity);
        }

        public override void ClearData()
        {
            Table.Clear();
        }
    }

    public class DataRepositoryRegistry : IDbContext
    {
        public List<DataRepositoryRegistryItemBase> Entities { get; private set; } =
            new List<DataRepositoryRegistryItemBase>();

        public DataRepositoryRegistry DbContext { get; private set; }

        public DataRepositoryRegistry()
        {
        }

        public void AddEntity(DataRepositoryRegistryItemBase entity)
        {
            Entities.Add(entity);
        }

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

        public ILookupDataBase GetLookupDataBase<TEntity>(LookupDefinitionBase lookupDefinition, LookupUserInterface lookupUi) where TEntity : class, new()
        {
            var table = GetTable<TEntity>();
            return new TestLookupDataBase<TEntity>(table);
        }

        public bool SaveNoCommitEntity<TEntity>(TEntity entity, string message) where TEntity : class, new()
        {
            var table = GetList<TEntity>();
            if (!table.Contains(entity))
            {
                table.Add(entity);
            }
            return true;
        }

        public bool SaveEntity<TEntity>(TEntity entity, string message) where TEntity : class, new()
        {
            return SaveNoCommitEntity(entity, message);
        }

        public bool DeleteEntity<TEntity>(TEntity entity, string message) where TEntity : class, new()
        {
            var table = GetList<TEntity>();
            if (table.Contains(entity))
            {
                table.Remove(entity);
            }
            return true;
        }

        public bool DeleteNoCommitEntity<TEntity>(TEntity entity, string message) where TEntity : class, new()
        {
            return DeleteEntity(entity, message);
        }

        public bool AddNewNoCommitEntity<TEntity>(TEntity entity, string message) where TEntity : class, new()
        {
            return SaveNoCommitEntity(entity, message);
        }

        public bool Commit(string message)
        {
            return true;
        }

        public void RemoveRange<TEntity>(IEnumerable<TEntity> listToRemove) where TEntity : class, new()
        {
            var table = GetList<TEntity>();
            foreach (var entity in listToRemove)
            {
                if (table.Contains(entity))
                {
                    table.Remove(entity);
                }
            }
        }

        public void AddRange<TEntity>(List<TEntity> listToAdd) where TEntity : class, new()
        {
            var table = GetList<TEntity>();
            table.AddRange(listToAdd);
        }

        public IQueryable<TEntity> GetTable<TEntity>() where TEntity : class, new()
        {
            return GetList<TEntity>().AsQueryable();
        }

        public void SetIdentityInsert(DbDataProcessor processor, TableDefinitionBase tableDefinition, bool value = true)
        {
            
        }

        public bool OpenConnection()
        {
            return true;
        }

        public bool CloseConnection()
        {
            return true;
        }

        public bool ExecuteSql(string sql)
        {
            return true;
        }

        public List<string> GetListOfDatabases(DbDataProcessor dataProcessor)
        {
            return new List<string>();
        }

        public void SetProcessor(DbDataProcessor processor)
        {
            
        }

        public void SetConnectionString(string connectionString)
        {
        }

        public IQueryable GetTable(string tableName)
        {
            return null;
        }
    }

    public class TestDataRepository : IDataRepository
    {
        public DataRepositoryRegistry DataContext { get; }
        public TestDataRepository(DataRepositoryRegistry context)
        {
            DataContext = context;
        }

        IDbContext DbLookup.IDataRepository.GetDataContext()
        {
            return DataContext;
        }

        public IDbContext GetDataContext(DbDataProcessor dataProcessor)
        {
            return DataContext;
        }

        public void ClearData()
        {
            foreach (var entity in DataContext.Entities)
            {
                entity.ClearData();
            }
        }

    }
}
