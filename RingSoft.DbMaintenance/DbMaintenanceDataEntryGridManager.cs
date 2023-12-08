using System;
using System.Collections.Generic;
using System.Linq;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DbLookup.TableProcessing;

namespace RingSoft.DbMaintenance
{
    public abstract class DbMaintenanceDataEntryGridManagerBase : DataEntryGridManager
    {
        public DbMaintenanceViewModelBase ViewModel { get; }

        public DbMaintenanceDataEntryGridManagerBase(DbMaintenanceViewModelBase viewModel)
        {
            ViewModel = viewModel;
        }
        public abstract void LoadGridFromHeaderEntity<THeaderEntity>(THeaderEntity headerEntity) 
            where THeaderEntity : class, new();

        public abstract bool ValidateGrid();

        public abstract void SaveNoCommitData<THeaderEntity>(THeaderEntity headerEntity, IDbContext context)
            where THeaderEntity : class, new();

        public abstract void DeleteNoCommitData<THeaderEntity>(THeaderEntity headerEntity, IDbContext context)
            where THeaderEntity : class, new();

        public abstract TableDefinitionBase TableDefinition { get; }

    }
    public abstract class DbMaintenanceDataEntryGridManager<TEntity> : DbMaintenanceDataEntryGridManagerBase
        where TEntity : class, new()
    {
        public override TableDefinitionBase TableDefinition => GblMethods.GetTableDefinition<TEntity>();

        public DbMaintenanceDataEntryGridManager(DbMaintenanceViewModelBase viewModel) : base(viewModel)
        {
            
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

        public override bool ValidateGrid()
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

        public override void SaveNoCommitData<THeaderEntity>(THeaderEntity headerEntity, IDbContext context)
        {
            var headerTableDef = GblMethods.GetTableDefinition<THeaderEntity>();
            var detailTableDef = GblMethods.GetTableDefinition<TEntity>();
            if (headerTableDef == null || detailTableDef == null)
            {
                return;
            }

            var detailsList = GetEntityList();
            var detailsFields = detailTableDef
                .PrimaryKeyFields
                .Where(p => p.ParentJoinForeignKeyDefinition != null
                            && p.ParentJoinForeignKeyDefinition.PrimaryTable == headerTableDef);
            foreach (var fieldDefinition in detailsFields)
            {
                foreach (var foreignKeyFieldJoin in fieldDefinition.ParentJoinForeignKeyDefinition.FieldJoins)
                {
                    foreach (var entity in detailsList)
                    {
                        var headerValue = GblMethods.GetPropertyValue(headerEntity,
                            foreignKeyFieldJoin.PrimaryField.PropertyName);
                        GblMethods.SetPropertyValue(entity, foreignKeyFieldJoin.ForeignField.PropertyName, headerValue);
                    }
                }
            }

            var existingData = GetExistingDbData(headerEntity, context);
            context.RemoveRange(existingData);
            context.AddRange(detailsList);
        }

        private IEnumerable<TEntity> GetExistingDbData<THeaderEntity>(THeaderEntity headerEntity,
            IDbContext context) where THeaderEntity : class, new()
        {
            var headerTableDef = GblMethods.GetTableDefinition<THeaderEntity>();
            var detailTableDef = GblMethods.GetTableDefinition<TEntity>();
            if (headerTableDef == null || detailTableDef == null)
            {
                throw new Exception("Invalid Header Object or Details Object");
            }

            var detailsFields = detailTableDef
                .PrimaryKeyFields
                .Where(p => p.ParentJoinForeignKeyDefinition != null
                            && p.ParentJoinForeignKeyDefinition.PrimaryTable == headerTableDef);

            var filter = new TableFilterDefinition<TEntity>(detailTableDef);
            foreach (var fieldDefinition in detailsFields)
            {
                foreach (var foreignKeyFieldJoin in fieldDefinition.ParentJoinForeignKeyDefinition.FieldJoins)
                {
                    var pkValue = GblMethods.GetPropertyValue(headerEntity,
                        foreignKeyFieldJoin.PrimaryField.PropertyName);
                    filter.AddFixedFieldFilter(foreignKeyFieldJoin.ForeignField, Conditions.Equals, pkValue);
                }
            }

            var table = context.GetTable<TEntity>();
            var param = GblMethods.GetParameterExpression<TEntity>();
            var expr = filter.GetWhereExpresssion<TEntity>(param);
            var result = FilterItemDefinition.FilterQuery(table, param, expr);
            return result;
        }

        public override void DeleteNoCommitData<THeaderEntity>(THeaderEntity headerEntity, IDbContext context)
        {
            var table = context.GetTable<TEntity>();
            var existingList = GetExistingDbData(headerEntity, context);
            context.RemoveRange(existingList);
        }
        
        public virtual List<TEntity> GetEntityList()
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

        public override void LoadGridFromHeaderEntity<THeaderEntity>(THeaderEntity headerEntity)
        {
            var headerTableDef = GblMethods.GetTableDefinition<THeaderEntity>();
            var detailTableDef = GblMethods.GetTableDefinition<TEntity>();
            if (headerTableDef == null || detailTableDef == null)
            {
                throw new Exception("Invalid Header Object or Details Object");
            }

            var childKey = headerTableDef
                .ChildKeys
                .FirstOrDefault(p => p.ForeignTable == detailTableDef);

            if (childKey != null)
            {
                var detailsPropertyObject = GblMethods.GetPropertyObject(headerEntity, childKey.CollectionName);
                if (detailsPropertyObject is IEnumerable<TEntity> details)
                {
                    LoadGrid(details);
                }
            }
        }
    }
}
