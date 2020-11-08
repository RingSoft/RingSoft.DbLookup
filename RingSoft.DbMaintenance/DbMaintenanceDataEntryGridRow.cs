using RingSoft.DataEntryControls.Engine.DataEntryGrid;

namespace RingSoft.DbMaintenance
{
    public abstract class DbMaintenanceDataEntryGridRow<TEntity> : DataEntryGridRow
        where TEntity : new()
    {
        //private DbMaintenanceDataEntryGridManager<TEntity> _dbMaintenanceDataEntryGridManager;

        protected DbMaintenanceDataEntryGridRow(DbMaintenanceDataEntryGridManager<TEntity> manager) : base(manager)
        {
            //_dbMaintenanceDataEntryGridManager = manager;
        }

        public abstract void LoadFromEntity(TEntity entity);

        public abstract bool ValidateRow();

        public abstract void SaveToEntity(TEntity entity, int rowIndex);
    }
}
