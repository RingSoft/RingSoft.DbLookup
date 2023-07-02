using System.Collections.Generic;
using System.Linq;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;

namespace RingSoft.DbLookup
{
    public interface IDbContext
    {
        ILookupDataBase GetLookupDataBase<TEntity>(LookupDefinitionBase lookupDefinition
            , TableDefinition<TEntity> tableDefinition)
            where TEntity : class, new();

        bool SaveNoCommitEntity<TEntity>(TEntity entity, string message, bool silent = false) where TEntity : class, new();

        bool SaveEntity<TEntity>(TEntity entity, string message, bool silent = false) where TEntity : class, new();

        bool DeleteEntity<TEntity>(TEntity entity, string message, bool silent = false) where TEntity : class, new();

        bool DeleteNoCommitEntity<TEntity>(TEntity entity, string message, bool silent = false) where TEntity : class, new();

        bool AddNewNoCommitEntity<TEntity>(TEntity entity, string message, bool silent = false) where TEntity : class, new();

        bool Commit(string message, bool silent = false);

        void RemoveRange<TEntity>(IEnumerable<TEntity> listToRemove) where TEntity : class, new();

        void AddRange<TEntity>(List<TEntity> listToAdd) where TEntity : class, new();

        IQueryable<TEntity> GetTable<TEntity>() where TEntity : class, new();

        void SetIdentityInsert(DbDataProcessor processor, TableDefinitionBase tableDefinition, bool silent = false
            , bool value = true);

        bool OpenConnection(bool silent = false);

        bool CloseConnection(bool silent = false);

        bool ExecuteSql(string sql, bool silent = false);

        List<string> GetListOfDatabases(DbDataProcessor dataProcessor);

        void SetProcessor(DbDataProcessor processor);

        void SetConnectionString(string? connectionString);
    }
}
