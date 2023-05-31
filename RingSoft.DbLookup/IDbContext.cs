using System.Collections.Generic;
using System.Linq;
using RingSoft.DbLookup.Lookup;

namespace RingSoft.DbLookup
{
    public interface IDbContext
    {
        ILookupDataBase GetLookupDataBase<TEntity>(LookupDefinitionBase lookupDefinition, LookupUserInterface lookupUi)
            where TEntity : class, new();

        bool SaveNoCommitEntity<TEntity>(TEntity entity, string message) where TEntity : class;

        bool SaveEntity<TEntity>(TEntity entity, string message) where TEntity : class;

        bool DeleteEntity<TEntity>(TEntity entity, string message) where TEntity : class;

        bool DeleteNoCommitEntity<TEntity>(TEntity entity, string message) where TEntity : class;

        bool AddNewNoCommitEntity<TEntity>(TEntity entity, string message) where TEntity : class;

        bool Commit(string message);

        void RemoveRange<TEntity>(IEnumerable<TEntity> listToRemove) where TEntity : class;

        void AddRange<TEntity>(List<TEntity> listToAdd) where TEntity : class;

        IQueryable<TEntity> GetTable<TEntity>() where TEntity : class;
    }
}
