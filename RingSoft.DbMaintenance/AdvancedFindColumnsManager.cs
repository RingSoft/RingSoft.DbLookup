// ***********************************************************************
// Assembly         : RingSoft.DbMaintenance
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 11-20-2023
// ***********************************************************************
// <copyright file="AdvancedFindColumnsManager.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.Lookup;

namespace RingSoft.DbMaintenance
{
    /// <summary>
    /// Class AdvancedFindColumnsManager.
    /// Implements the <see cref="RingSoft.DbMaintenance.DbMaintenanceDataEntryGridManager{RingSoft.DbLookup.AdvancedFind.AdvancedFindColumn}" />
    /// </summary>
    /// <seealso cref="RingSoft.DbMaintenance.DbMaintenanceDataEntryGridManager{RingSoft.DbLookup.AdvancedFind.AdvancedFindColumn}" />
    public class AdvancedFindColumnsManager : DbMaintenanceDataEntryGridManager<AdvancedFindColumn>
    {
        /// <summary>
        /// Gets or sets the view model.
        /// </summary>
        /// <value>The view model.</value>
        public new AdvancedFindViewModel ViewModel { get; set; }

        /// <summary>
        /// The adding new row
        /// </summary>
        private bool _addingNewRow;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdvancedFindColumnsManager"/> class.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        public AdvancedFindColumnsManager(AdvancedFindViewModel viewModel) : base(viewModel)
        {
            ViewModel = viewModel;
        }

        /// <summary>
        /// Gets the new row.
        /// </summary>
        /// <returns>DataEntryGridRow.</returns>
        protected override DataEntryGridRow GetNewRow()
        {
            var result = new AdvancedFindNewColumnRow(this);
            var oldNewRow = Rows.FirstOrDefault(p => p.IsNew);
            if (oldNewRow != null)
            {
                var oldRowIndex = Rows.IndexOf(oldNewRow);
                _addingNewRow = true;
                RemoveRow(oldNewRow);
                _addingNewRow = false;
            }

            return result;
        }

        /// <summary>
        /// Adds the new row.
        /// </summary>
        public void AddNewRow()
        {
            var newRow = GetNewRow();
            AddRow(newRow, -1);
            Grid?.RefreshGridView();
        }

        /// <summary>
        /// Constructs the new row from entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>DbMaintenanceDataEntryGridRow&lt;AdvancedFindColumn&gt;.</returns>
        protected override DbMaintenanceDataEntryGridRow<AdvancedFindColumn> ConstructNewRowFromEntity(AdvancedFindColumn entity)
        {
            AdvancedFindColumnRow result = null;
            if (entity.Formula.IsNullOrEmpty())
            {
                result = new AdvancedFindFieldColumnRow(this);
            }
            else
            {
                result = new AdvancedFindFormulaColumnRow(this);
            }
            return result;
        }

        //if (entity.Formula.IsNullOrEmpty())
        //{
        //    var tableDefinition =
        //        ViewModel.TableDefinition.Context.TableDefinitions.FirstOrDefault(p =>
        //            p.EntityName == entity.TableName);

        //    if (tableDefinition != null && tableDefinition.CanViewTable)
        //    {
        //        var fieldDefinition =
        //            tableDefinition.FieldDefinitions.FirstOrDefault(p => p.FieldName == entity.FieldName);

        //        var foundTreeViewItem = ViewModel.FindFieldInTree(ViewModel.TreeRoot, fieldDefinition);
        //        if (foundTreeViewItem == null)
        //        {
        //            ViewModel.ReadOnlyMode = true;
        //            return null;
        //        }
        //    }
        //    else
        //    {
        //        ViewModel.ReadOnlyMode = true;
        //        return null;
        //    }
        //}
        //return new AdvancedFindColumnRow(this);
        //}

        /// <summary>
        /// Loads from lookup definition.
        /// </summary>
        /// <param name="lookupDefinition">The lookup definition.</param>
        public void LoadFromLookupDefinition(LookupDefinitionBase lookupDefinition)
        {
            foreach (var column in lookupDefinition.VisibleColumns)
            {
                AdvancedFindColumnRow newRow = null;
                if (column is LookupFieldColumnDefinition)
                {
                    newRow = new AdvancedFindFieldColumnRow(this);
                }
                else if (column is LookupFormulaColumnDefinition)
                {
                    newRow = new AdvancedFindFormulaColumnRow(this);
                }
                newRow.LoadFromColumnDefinition(column);
                AddRow(newRow);
            }

            AddNewRow();
            //foreach (var column in lookupDefinition.VisibleColumns)
            //{
            //    LoadFromColumnDefinition(column);
            //}
        }

        /// <summary>
        /// Updates the width of the column.
        /// </summary>
        /// <param name="column">The column.</param>
        public void UpdateColumnWidth(LookupColumnDefinitionBase column)
        {
            var columnRow = Rows.OfType<AdvancedFindColumnRow>()
                .FirstOrDefault(p => p.LookupColumnDefinition == column);
            columnRow?.UpdatePercentWidth();
        }

        /// <summary>
        /// Loads from column definition.
        /// </summary>
        /// <param name="column">The column.</param>
        public void LoadFromColumnDefinition(LookupColumnDefinitionBase column)
        {
            AdvancedFindColumnRow columnRow = null;
            if (column is LookupFieldColumnDefinition)
            {
                columnRow = new AdvancedFindFieldColumnRow(this);
            }
            else if (column is LookupFormulaColumnDefinition)
            {
                columnRow = new AdvancedFindFormulaColumnRow(this);
            }

            var newColumnRows = Rows.Where(p => p.IsNew == true);
            var newColumnRow = newColumnRows.FirstOrDefault();
            var startIndex = GetNewColumnIndex();
            if (newColumnRow != null)
            {
                startIndex = Rows.IndexOf(newColumnRow);
                if (startIndex < Rows.Count - 1)
                {
                    RemoveRow(newColumnRow);
                }
            }
            
            columnRow?.LoadFromColumnDefinition(column);
            AddRow(columnRow, startIndex);
            Grid?.RefreshGridView();
        }

        /// <summary>
        /// Gets the new index of the column.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public int GetNewColumnIndex()
        {
            var newColumnRows = Rows.Where(p => p.IsNew == true);
            var newColumnRow = newColumnRows.FirstOrDefault();
            var startIndex = Rows.Count - 1;
            if (newColumnRow != null)
            {
                startIndex = Rows.IndexOf(newColumnRow);
                if (Rows.Count == 1)
                {
                    startIndex = -1;
                }

            }
            return startIndex;
        }

        /// <summary>
        /// Determines whether [is delete ok] [the specified row index].
        /// </summary>
        /// <param name="rowIndex">Index of the row.</param>
        /// <returns><c>true</c> if [is delete ok] [the specified row index]; otherwise, <c>false</c>.</returns>
        public override bool IsDeleteOk(int rowIndex)
        {
            var rows = Rows.OfType<AdvancedFindColumnRow>().ToList();
            var row = rows[rowIndex];
            if (rowIndex == Rows.Count - 1 && row.IsNew)
            {
                return false;
            }
            else
            {
                return true;
            }
            //return base.IsDeleteOk(rowIndex);
        }


        /// <summary>
        /// Removes the row.
        /// </summary>
        /// <param name="rowToDelete">The row to delete.</param>
        public override void RemoveRow(DataEntryGridRow rowToDelete)
        {
            if (rowToDelete.IsNew)
            {
                if (!_addingNewRow)
                {
                    AddNewRow();
                }
            }

            base.RemoveRow(rowToDelete);
        }

        /// <summary>
        /// Determines whether [is sort column initial sort column].
        /// </summary>
        /// <returns><c>true</c> if [is sort column initial sort column]; otherwise, <c>false</c>.</returns>
        public bool IsSortColumnInitialSortColumn()
        {
            var result = false;
            var row = Rows.FirstOrDefault();
            if (row != null && row is AdvancedFindColumnRow columnRow)
            {
                if (columnRow.LookupColumnDefinition is LookupFieldColumnDefinition fieldColumn)
                {
                    var oldLookupColumn = ViewModel
                        .LookupDefinition
                        .TableDefinition
                        .LookupDefinition
                        .InitialSortColumnDefinition;

                    if (oldLookupColumn is LookupFieldColumnDefinition oldFieldColumn)
                    {
                        if (oldFieldColumn.FieldDefinition == fieldColumn.FieldDefinition)
                        {
                            result = true;
                        }
                    }
                }
            }
            return result;
        }
    }
}
