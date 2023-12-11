// ***********************************************************************
// Assembly         : RingSoft.DbMaintenance
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 03-19-2023
// ***********************************************************************
// <copyright file="AdvancedFindAfFilterRow.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Linq;
using System.Runtime.InteropServices;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DbLookup.TableProcessing;

namespace RingSoft.DbMaintenance
{
    /// <summary>
    /// Class AdvancedFindAfFilterRow.
    /// Implements the <see cref="RingSoft.DbMaintenance.AdvancedFindFilterRow" />
    /// </summary>
    /// <seealso cref="RingSoft.DbMaintenance.AdvancedFindFilterRow" />
    public class AdvancedFindAfFilterRow : AdvancedFindFilterRow
    {
        /// <summary>
        /// Gets or sets the automatic fill setup.
        /// </summary>
        /// <value>The automatic fill setup.</value>
        public AutoFillSetup AutoFillSetup { get; set; }

        /// <summary>
        /// Gets or sets the automatic fill value.
        /// </summary>
        /// <value>The automatic fill value.</value>
        public AutoFillValue AutoFillValue { get; set; }

        //public AdvancedFindFilterDefinition Filter { get; set; }

        /// <summary>
        /// Gets or sets the advanced find identifier.
        /// </summary>
        /// <value>The advanced find identifier.</value>
        public int AdvancedFindId { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdvancedFindAfFilterRow"/> class.
        /// </summary>
        /// <param name="manager">The manager.</param>
        /// <param name="path">The path.</param>
        /// <param name="primaryFieldDefinition">The primary field definition.</param>
        public AdvancedFindAfFilterRow(AdvancedFindFiltersManager manager
            , string path
            , FieldDefinition primaryFieldDefinition = null) : base(manager)
        {
            Path = path;
            SetupTableField(primaryFieldDefinition);
        }

        /// <summary>
        /// Setups the table field.
        /// </summary>
        /// <param name="primaryFieldDefinition">The primary field definition.</param>
        private void SetupTableField(FieldDefinition primaryFieldDefinition)
        {
            var lookup = SystemGlobals.AdvancedFindLookupContext.AdvancedFindLookup.Clone();
            var primaryTable = Manager.ViewModel.LookupDefinition.TableDefinition;
            if (primaryFieldDefinition != null)
            {
                PrimaryTable = primaryFieldDefinition.TableDefinition.TableName;
                PrimaryField = primaryFieldDefinition.FieldName;
                MakeParentField();
            }

            if (ParentFieldDefinition != null)
            {
                primaryTable = ParentFieldDefinition.TableDefinition;
                if (ParentFieldDefinition.ParentJoinForeignKeyDefinition != null)
                {
                    primaryTable = ParentFieldDefinition.ParentJoinForeignKeyDefinition.PrimaryTable;
                }

                Table = ParentFieldDefinition.Description;
            }
            else
            {
                Table = primaryTable.Description;
            }

            var path = Path;
            if (!path.IsNullOrEmpty())
            {
                var foundItem = Manager.ViewModel.AdvancedFindTree.ProcessFoundTreeViewItem(path);
                if (foundItem != null)
                {
                    if (foundItem.FieldDefinition.ParentJoinForeignKeyDefinition != null)
                    {
                        primaryTable = foundItem.FieldDefinition.ParentJoinForeignKeyDefinition.PrimaryTable;
                    }
                }
            }
            lookup.FilterDefinition.AddFixedFilter(p => p.Table, Conditions.Equals, primaryTable.EntityName);
            if (Manager.ViewModel.AdvancedFindId != 0)
            {
                lookup.FilterDefinition.AddFixedFilter(p => p.Id, Conditions.NotEquals,
                    Manager.ViewModel.AdvancedFindId);
            }

            Field = "<Advanced Find>";
            AutoFillSetup = new AutoFillSetup(lookup);
            object inputParameter = null;
            if (Manager.ViewModel.AdvancedFindInput != null)
            {
                inputParameter = Manager.ViewModel.AdvancedFindInput.InputParameter;
            }

            AutoFillSetup.AddViewParameter = new AdvancedFindInput
            {
                InputParameter = inputParameter,
                LockTable = primaryTable
            };
        }

        /// <summary>
        /// Gets the cell props.
        /// </summary>
        /// <param name="columnId">The column identifier.</param>
        /// <returns>DataEntryGridCellProps.</returns>
        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (AdvancedFindFiltersManager.FilterColumns) columnId;
            switch (column)
            {
                case AdvancedFindFiltersManager.FilterColumns.Search:
                    return new DataEntryGridAutoFillCellProps(this, columnId, AutoFillSetup, AutoFillValue)
                    {
                        AlwaysUpdateOnSelect = true,
                        TabOnSelect = false
                    };
                
            }
            return base.GetCellProps(columnId);
        }

        /// <summary>
        /// Sets the cell value.
        /// </summary>
        /// <param name="value">The value.</param>
        public override void SetCellValue(DataEntryGridEditingCellProps value)
        {
            var column = (AdvancedFindFiltersManager.FilterColumns)value.ColumnId;
            switch (column)
            {
                case AdvancedFindFiltersManager.FilterColumns.Search:
                    if (value is DataEntryGridAutoFillCellProps autoFillProps) 
                        AutoFillValue = autoFillProps.AutoFillValue;
                    ResetLookup = false;
                    CreateFilterDefinition();
                    Manager.ViewModel.ResetLookup();
                    break;
            }

            base.SetCellValue(value);
        }

        /// <summary>
        /// Creates the filter definition.
        /// </summary>
        private void CreateFilterDefinition()
        {
            var filter = FilterItemDefinition as AdvancedFindFilterDefinition;
            if (AutoFillValue != null && AutoFillValue.IsValid())
            {
                var advancedFindId = AutoFillValue
                    .PrimaryKeyValue.KeyValueFields[0].Value.ToInt();

                if (FilterItemDefinition == null)
                {
                    FilterItemDefinition = Manager.ViewModel.LookupDefinition.FilterDefinition.AddUserFilter(advancedFindId,
                        Manager.ViewModel.LookupDefinition, Path, true, GetNewFilterIndex());
                }
                else
                {
                    filter.AdvancedFindId = advancedFindId;
                }
                AdvancedFindId = advancedFindId;
            }
            else if (filter != null)
            {
                Manager.ViewModel.LookupDefinition.FilterDefinition.RemoveUserFilter(filter);
            }
        }

        /// <summary>
        /// Gets the new index of the filter.
        /// </summary>
        /// <returns>System.Int32.</returns>
        protected override int GetNewFilterIndex()
        {
            var result = Manager.Rows.IndexOf(this);
            var fixedItems = Manager.Rows.OfType<AdvancedFindFilterRow>()
                .Where(p => p.IsFixed)
                .ToList();
            return result - fixedItems.Count;
        }

        /// <summary>
        /// Saves to entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="rowIndex">Index of the row.</param>
        public override void SaveToEntity(AdvancedFindFilter entity, int rowIndex)
        {
            if (AutoFillValue != null && AutoFillValue.IsValid())
            {
                entity.SearchForAdvancedFindId = AutoFillValue.PrimaryKeyValue.KeyValueFields[0].Value.ToInt();
            }

            var newPath = Path;
            entity.Path = newPath;
            base.SaveToEntity(entity, rowIndex);
        }

        /// <summary>
        /// Loads from entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public override void LoadFromEntity(AdvancedFindFilter entity)
        {
            AutoFillValue = Manager.ViewModel.TableDefinition.Context.OnAutoFillTextRequest(
                SystemGlobals.AdvancedFindLookupContext.AdvancedFinds, entity.SearchForAdvancedFindId.ToString());
            Path = entity.Path;

            base.LoadFromEntity(entity);
        }

        /// <summary>
        /// Validates the row.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public override bool ValidateRow()
        {
            if (!AutoFillValue.IsValid())
            {
                var message = "Search For Advanced Find is invalid.";
                var caption = "Validation Failure";
                ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
                Manager.Grid?.GotoCell(this, AdvancedFindFiltersManager.SearchColumnId);
                return false;
            }
            return base.ValidateRow();
        }
    }
}
