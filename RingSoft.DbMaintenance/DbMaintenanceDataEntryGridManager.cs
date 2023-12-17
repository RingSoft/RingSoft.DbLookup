// ***********************************************************************
// Assembly         : RingSoft.DbMaintenance
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-08-2023
// ***********************************************************************
// <copyright file="DbMaintenanceDataEntryGridManager.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
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
    /// <summary>
    /// Class DbMaintenanceDataEntryGridManagerBase.
    /// Implements the <see cref="DataEntryGridManager" />
    /// </summary>
    /// <seealso cref="DataEntryGridManager" />
    public abstract class DbMaintenanceDataEntryGridManagerBase : DataEntryGridManager
    {
        /// <summary>
        /// Gets the view model.
        /// </summary>
        /// <value>The view model.</value>
        public DbMaintenanceViewModelBase ViewModel { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbMaintenanceDataEntryGridManagerBase"/> class.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        public DbMaintenanceDataEntryGridManagerBase(DbMaintenanceViewModelBase viewModel)
        {
            ViewModel = viewModel;
        }
        /// <summary>
        /// Loads the grid from header entity.
        /// </summary>
        /// <typeparam name="THeaderEntity">The type of the t header entity.</typeparam>
        /// <param name="headerEntity">The header entity.</param>
        public abstract void LoadGridFromHeaderEntity<THeaderEntity>(THeaderEntity headerEntity) 
            where THeaderEntity : class, new();

        /// <summary>
        /// Validates the grid.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public abstract bool ValidateGrid();

        /// <summary>
        /// Saves the no commit data.
        /// </summary>
        /// <typeparam name="THeaderEntity">The type of the t header entity.</typeparam>
        /// <param name="headerEntity">The header entity.</param>
        /// <param name="context">The context.</param>
        public abstract void SaveNoCommitData<THeaderEntity>(THeaderEntity headerEntity, IDbContext context)
            where THeaderEntity : class, new();

        /// <summary>
        /// Deletes the no commit data.
        /// </summary>
        /// <typeparam name="THeaderEntity">The type of the t header entity.</typeparam>
        /// <param name="headerEntity">The header entity.</param>
        /// <param name="context">The context.</param>
        public abstract void DeleteNoCommitData<THeaderEntity>(THeaderEntity headerEntity, IDbContext context)
            where THeaderEntity : class, new();

        /// <summary>
        /// Gets the table definition.
        /// </summary>
        /// <value>The table definition.</value>
        public abstract TableDefinitionBase TableDefinition { get; }

    }
    /// <summary>
    /// Class DbMaintenanceDataEntryGridManager.  Used to manage DbMaintenanceViewModel grids.
    /// Implements the <see cref="RingSoft.DbMaintenance.DbMaintenanceDataEntryGridManagerBase" />
    /// </summary>
    /// <typeparam name="TEntity">The type of the t entity.</typeparam>
    /// <seealso cref="RingSoft.DbMaintenance.DbMaintenanceDataEntryGridManagerBase" />
    public abstract class DbMaintenanceDataEntryGridManager<TEntity> : DbMaintenanceDataEntryGridManagerBase
        where TEntity : class, new()
    {
        /// <summary>
        /// Gets the table definition.
        /// </summary>
        /// <value>The table definition.</value>
        public override TableDefinitionBase TableDefinition => GblMethods.GetTableDefinition<TEntity>();

        /// <summary>
        /// Initializes a new instance of the <see cref="DbMaintenanceDataEntryGridManager{TEntity}"/> class.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        public DbMaintenanceDataEntryGridManager(DbMaintenanceViewModelBase viewModel) : base(viewModel)
        {
            
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// Loads the grid.
        /// </summary>
        /// <param name="entityList">The entity list.</param>
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

        /// <summary>
        /// Adds the row from entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
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

        /// <summary>
        /// Constructs the new row from entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>DbMaintenanceDataEntryGridRow&lt;TEntity&gt;.</returns>
        protected abstract DbMaintenanceDataEntryGridRow<TEntity> ConstructNewRowFromEntity(TEntity entity);

        /// <summary>
        /// Gets the parent row identifier from entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>System.String.</returns>
        protected virtual string GetParentRowIdFromEntity(TEntity entity)
        {
            return string.Empty;
        }

        /// <summary>
        /// Raises the dirty flag.
        /// </summary>
        public override void RaiseDirtyFlag()
        {
            ViewModel.RecordDirty = true;
            base.RaiseDirtyFlag();
        }

        /// <summary>
        /// Validates the grid.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
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

        /// <summary>
        /// Saves the no commit data.
        /// </summary>
        /// <typeparam name="THeaderEntity">The type of the t header entity.</typeparam>
        /// <param name="headerEntity">The header entity.</param>
        /// <param name="context">The context.</param>
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

        /// <summary>
        /// Gets the existing database data.
        /// </summary>
        /// <typeparam name="THeaderEntity">The type of the t header entity.</typeparam>
        /// <param name="headerEntity">The header entity.</param>
        /// <param name="context">The context.</param>
        /// <returns>IEnumerable&lt;TEntity&gt;.</returns>
        /// <exception cref="System.Exception">Invalid Header Object or Details Object</exception>
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

        /// <summary>
        /// Deletes the no commit data.
        /// </summary>
        /// <typeparam name="THeaderEntity">The type of the t header entity.</typeparam>
        /// <param name="headerEntity">The header entity.</param>
        /// <param name="context">The context.</param>
        public override void DeleteNoCommitData<THeaderEntity>(THeaderEntity headerEntity, IDbContext context)
        {
            var table = context.GetTable<TEntity>();
            var existingList = GetExistingDbData(headerEntity, context);
            context.RemoveRange(existingList);
        }

        /// <summary>
        /// Gets the entity list.
        /// </summary>
        /// <returns>List&lt;TEntity&gt;.</returns>
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

        /// <summary>
        /// Determines whether this instance has data.
        /// </summary>
        /// <returns><c>true</c> if this instance has data; otherwise, <c>false</c>.</returns>
        public bool HasData()
        {
            var result = false;
            result = Rows.Any(row => !row.IsNew);
            return result;
        }

        /// <summary>
        /// Loads the grid from header entity.
        /// </summary>
        /// <typeparam name="THeaderEntity">The type of the t header entity.</typeparam>
        /// <param name="headerEntity">The header entity.</param>
        /// <exception cref="System.Exception">Invalid Header Object or Details Object</exception>
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
