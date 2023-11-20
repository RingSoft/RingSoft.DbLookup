﻿using System;
using System.Linq;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup;
using RingSoft.DbLookup.ModelDefinition;

namespace RingSoft.DbMaintenance
{
    public abstract class DbMaintenanceDataEntryGridRow<TEntity> : DataEntryGridRow
        where TEntity : class, new()
    {
        public TableDefinition<TEntity> TableDefinition { get; }

        public new DbMaintenanceDataEntryGridManager<TEntity> Manager { get; }

        public bool AllowSave { get; protected set; } = true;

        public bool IsFixed { get; protected set; }

        //private DbMaintenanceDataEntryGridManager<TEntity> _dbMaintenanceDataEntryGridManager;

        protected DbMaintenanceDataEntryGridRow(DbMaintenanceDataEntryGridManager<TEntity> manager) : base(manager)
        {
            Manager = manager;
            TableDefinition = GblMethods.GetTableDefinition<TEntity>();
        }

        public abstract void LoadFromEntity(TEntity entity);

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

        public virtual void ProcessHeaderObject(TEntity entity, object headerObject)
        {
            throw new Exception(
                "Did you override Row's ProcessHeaderObject?  If so, don't call the base, otherwise, you need to override Row's ProcessHeaderObject and not call the base.");
        }

        public abstract void SaveToEntity(TEntity entity, int rowIndex);
    }
}
