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
    public abstract class DataRepositoryRegistryItemBase
    {
        public Type Entity { get; internal set; }

        public abstract void ClearData();
    }

    public class DataRepositoryRegistryItem<TEntity> : DataRepositoryRegistryItemBase where TEntity : new()
    {
        public List<TEntity> Table { get; private set; }

        public DataRepositoryRegistryItem()
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
        private string _lastError;

        public List<DataRepositoryRegistryItemBase> Entities { get; private set; } =
            new List<DataRepositoryRegistryItemBase>();

        public DataRepositoryRegistry DbContext { get; private set; }

        public DataRepositoryRegistry()
        {
            AddEntity(new DataRepositoryRegistryItem<RecordLock>());
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

        public ILookupDataBase GetLookupDataBase<TEntity>(LookupDefinitionBase lookupDefinition
            , TableDefinition<TEntity> tableDefinition) where TEntity : class, new()
        {
            var table = GetTable<TEntity>();
            return new TestLookupDataBase<TEntity>(table, tableDefinition);
        }

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

        private  void SetNewIdentity<TEntity>(TableDefinition<TEntity> tableDef, TEntity entity) where TEntity : class, new()
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

        public bool SaveEntity<TEntity>(TEntity entity, string message, bool silent = false) where TEntity : class, new()
        {
            return SaveNoCommitEntity(entity, message);
        }

        public bool DeleteEntity<TEntity>(TEntity entity, string message, bool silent = false) where TEntity : class, new()
        {
            var table = GetList<TEntity>();
            if (table.Contains(entity))
            {
                table.Remove(entity);
            }
            return true;
        }

        public bool DeleteNoCommitEntity<TEntity>(TEntity entity, string message, bool silent = false) where TEntity : class, new()
        {
            return DeleteEntity(entity, message);
        }

        public bool AddNewNoCommitEntity<TEntity>(TEntity entity, string message, bool silent = false) where TEntity : class, new()
        {
            var table = GetList<TEntity>();
            table.Add(entity);
            SetNewIdentity(GblMethods.GetTableDefinition<TEntity>(), entity);
            return true;
        }

        public bool AddSaveEntity<TEntity>(TEntity entity, string message, bool silent = false) where TEntity : class, new()
        {
            return AddNewNoCommitEntity(entity, message, silent);
        }

        public bool Commit(string message, bool silent = false)
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


        public void SetIdentityInsert(DbDataProcessor processor, TableDefinitionBase tableDefinition, bool silent = false, bool value = true)
        {

        }

        public bool OpenConnection(bool silent = false)
        {
            return true;
        }

        public bool CloseConnection(bool silent = false)
        {
            return true;
        }

        public bool ExecuteSql(string sql, bool silent = false)
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

        IDbContext IDataRepository.GetDataContext()
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
