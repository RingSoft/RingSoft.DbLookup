using System;
using System.Collections.Generic;
using System.Linq;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup;

namespace RingSoft.DbMaintenance
{
    //public class AutoFillMap
    public abstract class DbMaintenanceDataEntryGridManager<TEntity> : DataEntryGridManager
        where TEntity : class, new()
    {
        public DbMaintenanceViewModelBase ViewModel { get; }

        public DbMaintenanceDataEntryGridManager(DbMaintenanceViewModelBase viewModel)
        {
            ViewModel = viewModel;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        public virtual void LoadGrid(IEnumerable<TEntity> entityList)
        {
            if (entityList == null)
                return;

            Grid?.SetBulkInsertMode();
            PreLoadGridFromEntity();
            foreach (var entity in entityList)
            {
                var parentRowId = GetParentRowIdFromEntity(entity);
                if (string.IsNullOrEmpty(parentRowId))
                    AddRowFromEntity(entity);
            }

            PostLoadGridFromEntity();
            Grid?.SetBulkInsertMode(false);
        }

        public virtual void AddRowFromEntity(TEntity entity)
        {
            var newRow = ConstructNewRowFromEntity(entity);
            if (newRow != null)
            {
                AddRow(newRow);
                newRow.LoadFromEntity(entity);
                Grid?.UpdateRow(newRow);
            }
        }

        protected abstract DbMaintenanceDataEntryGridRow<TEntity> ConstructNewRowFromEntity(TEntity entity);

        protected virtual string GetParentRowIdFromEntity(TEntity entity)
        {
            return string.Empty;
        }

        public override void RaiseDirtyFlag()
        {
            ViewModel.RecordDirty = true;
            base.RaiseDirtyFlag();
        }

        public virtual bool ValidateGrid()
        {
            if (Grid != null && !Grid.CommitCellEdit())
                return false;

            foreach (var dataEntryGridRow in Rows)
            {
                if (dataEntryGridRow is DbMaintenanceDataEntryGridRow<TEntity> row && !row.IsNew)
                    if (!row.ValidateRow())
                        return false;
            }
            return true;
        }

        public void SaveNoCommitData(IDbContext context, object headerObject)
        {
            var table = context.GetTable<TEntity>();
            var existingList = GetExistingDbData(table, headerObject);
            var newList = GetEntityList(headerObject);
            context.RemoveRange(existingList);
            context.AddRange(newList);
        }

        public void DeleteNoCommitData(object headerObject, IDbContext context)
        {
            var table = context.GetTable<TEntity>();
            var existingList = GetExistingDbData(table, headerObject);
            context.RemoveRange(existingList);
        }

        protected virtual IEnumerable<TEntity> GetExistingDbData(IQueryable<TEntity> table, object headerObject)
        {
            throw new Exception("You must override Manager's GetExistingDbData and not call the base.");
        }

        public virtual List<TEntity> GetEntityList(object headerObject = null)
        {
            if (Grid != null)
            {
                Grid.CommitCellEdit();
            }

            var result = new List<TEntity>();
            var rowIndex = 0;
            foreach (var dataEntryGridRow in Rows)
            {
                if (dataEntryGridRow is DbMaintenanceDataEntryGridRow<TEntity> row && !row.IsNew)
                {
                    if (row.AllowSave)
                    {
                        var entity = (TEntity)Activator.CreateInstance(typeof(TEntity));
                        row.SaveToEntity(entity, rowIndex);
                        if (headerObject != null)
                        {
                            row.ProcessHeaderObject(entity, headerObject);
                        }

                        result.Add(entity);
                        rowIndex++;
                    }
                }
            }

            return result;
        }

        public bool HasData()
        {
            var result = false;
            result = Rows.Any(row => !row.IsNew);
            return result;
        }
    }
}
