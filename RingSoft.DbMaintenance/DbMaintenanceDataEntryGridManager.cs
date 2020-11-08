using System;
using System.Collections.Generic;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;

namespace RingSoft.DbMaintenance
{
    public abstract class DbMaintenanceDataEntryGridManager<TEntity> : DataEntryGridManager
        where TEntity : new()
    {
        public DbMaintenanceViewModelBase ViewModel { get; }

        public DbMaintenanceDataEntryGridManager(DbMaintenanceViewModelBase viewModel)
        {
            ViewModel = viewModel;
        }

        public virtual void LoadGrid(IEnumerable<TEntity> entityList)
        {
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
            AddRow(newRow);
            newRow.LoadFromEntity(entity);
            Grid?.UpdateRow(newRow);
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
            if (Grid != null && !Grid.CommitEdit())
                return false;

            foreach (var dataEntryGridRow in Rows)
            {
                if (dataEntryGridRow is DbMaintenanceDataEntryGridRow<TEntity> row && !row.IsNew)
                    if (!row.ValidateRow())
                        return false;
            }
            return true;
        }

        public List<TEntity> GetEntityList()
        {
            if (Grid == null)
                return null;

            var result = new List<TEntity>();
            var rowIndex = 0;
            foreach (var dataEntryGridRow in Rows)
            {
                if (dataEntryGridRow is DbMaintenanceDataEntryGridRow<TEntity> row && !row.IsNew)
                {
                    var entity = (TEntity)Activator.CreateInstance(typeof(TEntity));
                    row.SaveToEntity(entity, rowIndex);
                    result.Add(entity);
                    rowIndex++;
                }
            }

            return result;
        }
    }
}
