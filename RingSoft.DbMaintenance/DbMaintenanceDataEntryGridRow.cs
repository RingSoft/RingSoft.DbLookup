// ***********************************************************************
// Assembly         : RingSoft.DbMaintenance
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-16-2023
// ***********************************************************************
// <copyright file="DbMaintenanceDataEntryGridRow.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Linq;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup;
using RingSoft.DbLookup.ModelDefinition;

namespace RingSoft.DbMaintenance
{
    /// <summary>
    /// Class DbMaintenanceDataEntryGridRow.  Manages a DataEntryGridRow in a DbMaintenanceDataEntryGridManager
    /// Implements the <see cref="DataEntryGridRow" />
    /// </summary>
    /// <typeparam name="TEntity">The type of the t entity.</typeparam>
    /// <seealso cref="DataEntryGridRow" />
    public abstract class DbMaintenanceDataEntryGridRow<TEntity> : DataEntryGridRow
        where TEntity : class, new()
    {
        /// <summary>
        /// Gets the table definition.
        /// </summary>
        /// <value>The table definition.</value>
        public TableDefinition<TEntity> TableDefinition { get; }

        /// <summary>
        /// Gets the manager.
        /// </summary>
        /// <value>The manager.</value>
        public new DbMaintenanceDataEntryGridManager<TEntity> Manager { get; }

        /// <summary>
        /// Gets or sets a value indicating whether [allow save].
        /// </summary>
        /// <value><c>true</c> if [allow save]; otherwise, <c>false</c>.</value>
        public bool AllowSave { get; protected set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is fixed.
        /// </summary>
        /// <value><c>true</c> if this instance is fixed; otherwise, <c>false</c>.</value>
        public bool IsFixed { get; protected set; }

        //private DbMaintenanceDataEntryGridManager<TEntity> _dbMaintenanceDataEntryGridManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbMaintenanceDataEntryGridRow{TEntity}" /> class.
        /// </summary>
        /// <param name="manager">The manager.</param>
        protected DbMaintenanceDataEntryGridRow(DbMaintenanceDataEntryGridManager<TEntity> manager) : base(manager)
        {
            Manager = manager;
            TableDefinition = GblMethods.GetTableDefinition<TEntity>();
        }

        /// <summary>
        /// Loads from entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public abstract void LoadFromEntity(TEntity entity);

        /// <summary>
        /// Validates the row.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public virtual bool ValidateRow()
        {
            if (Manager.Columns != null)
            {
                foreach (var columnMap in Manager.Columns)
                {
                    var cellProps = GetCellProps(columnMap.ColumnId);
                    if (cellProps != null)
                    {
                        if (cellProps is DataEntryGridAutoFillCellProps autoFillCellProps)
                        {
                            var cellStyle = GetCellStyle(columnMap.ColumnId);
                            var description = columnMap.ColumnName;
                            if (cellStyle != null)
                            {
                                if (!cellStyle.ColumnHeader.IsNullOrEmpty())
                                {
                                    description = cellStyle.ColumnHeader;
                                }
                            }

                            if (!autoFillCellProps.AutoFillValue.ValidateAutoFill(autoFillCellProps.AutoFillSetup))
                            {
                                var message = $"{description} has an invalid value";
                                var caption = "Validation Failure";
                                Manager?.Grid.GotoCell(this, columnMap.ColumnId);
                                ControlsGlobals.UserInterface.ShowMessageBox(message, caption,
                                    RsMessageBoxIcons.Exclamation);
                                return false;
                            }
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Saves to entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="rowIndex">Index of the row.</param>
        public abstract void SaveToEntity(TEntity entity, int rowIndex);
    }
}
