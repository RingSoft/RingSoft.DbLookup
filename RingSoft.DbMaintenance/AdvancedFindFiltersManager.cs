// ***********************************************************************
// Assembly         : RingSoft.DbMaintenance
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 11-20-2023
// ***********************************************************************
// <copyright file="AdvancedFindFiltersManager.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
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
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.TableProcessing;

namespace RingSoft.DbMaintenance
{
    /// <summary>
    /// Class AdvancedFindFiltersManager.
    /// Implements the <see cref="RingSoft.DbMaintenance.DbMaintenanceDataEntryGridManager{RingSoft.DbLookup.AdvancedFind.AdvancedFindFilter}" />
    /// </summary>
    /// <seealso cref="RingSoft.DbMaintenance.DbMaintenanceDataEntryGridManager{RingSoft.DbLookup.AdvancedFind.AdvancedFindFilter}" />
    public class AdvancedFindFiltersManager : DbMaintenanceDataEntryGridManager<AdvancedFindFilter>
    {
        /// <summary>
        /// The left parentheses column identifier
        /// </summary>
        public const int LeftParenthesesColumnId = 1;
        /// <summary>
        /// The table column identifier
        /// </summary>
        public const int TableColumnId = 2;
        /// <summary>
        /// The field column identifier
        /// </summary>
        public const int FieldColumnId = 3;
        /// <summary>
        /// The search column identifier
        /// </summary>
        public const int SearchColumnId = 4;
        /// <summary>
        /// The right parentheses column identifier
        /// </summary>
        public const int RightParenthesesColumnId = 5;
        /// <summary>
        /// The end logic column identifier
        /// </summary>
        public const int EndLogicColumnId = 6;

        /// <summary>
        /// Enum FilterColumns
        /// </summary>
        public enum FilterColumns
        {
            /// <summary>
            /// The left parentheses
            /// </summary>
            LeftParentheses = LeftParenthesesColumnId,
            /// <summary>
            /// The table
            /// </summary>
            Table = TableColumnId,
            /// <summary>
            /// The field
            /// </summary>
            Field = FieldColumnId,
            /// <summary>
            /// The search
            /// </summary>
            Search = SearchColumnId,
            /// <summary>
            /// The right parentheses
            /// </summary>
            RightParentheses = RightParenthesesColumnId,
            /// <summary>
            /// The end logic
            /// </summary>
            EndLogic = EndLogicColumnId
        }

        /// <summary>
        /// Gets or sets the view model.
        /// </summary>
        /// <value>The view model.</value>
        public AdvancedFindViewModel ViewModel { get; set; }

        /// <summary>
        /// The reset lookup
        /// </summary>
        private bool _resetLookup = true;
        /// <summary>
        /// The adding new row
        /// </summary>
        private bool _addingNewRow;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdvancedFindFiltersManager"/> class.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        public AdvancedFindFiltersManager(AdvancedFindViewModel viewModel) : base(viewModel)
        {
            ViewModel = viewModel;
        }

        /// <summary>
        /// Gets the new row.
        /// </summary>
        /// <returns>DataEntryGridRow.</returns>
        protected override DataEntryGridRow GetNewRow()
        {
            var result = new AdvancedFindNewFilterRow(this);
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
        /// Constructs the new row from entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>DbMaintenanceDataEntryGridRow&lt;AdvancedFindFilter&gt;.</returns>
        protected override DbMaintenanceDataEntryGridRow<AdvancedFindFilter> ConstructNewRowFromEntity(
            AdvancedFindFilter entity)
        {
            if (entity.Formula.IsNullOrEmpty() && entity.SearchForAdvancedFindId == null)
            {
                return new AdvancedFindFieldFilterRow(this);
                //var tableDefinition =
                //    ViewModel.TableDefinition.Context.TableDefinitions.FirstOrDefault(p =>
                //        p.EntityName == entity.TableName);

                //if (tableDefinition != null)
                //{
                //    var fieldDefinition =
                //        tableDefinition.FieldDefinitions.FirstOrDefault(p => p.FieldName == entity.FieldName);

                //    var foundTreeViewItem = ViewModel.FindFieldInTree(ViewModel.TreeRoot, fieldDefinition);
                //    if (foundTreeViewItem == null || !tableDefinition.CanViewTable)
                //    {
                //        ViewModel.ReadOnlyMode = true;
                //        return null;
                //    }
                //}
            } 
            if (entity.SearchForAdvancedFindId == null)
            {
                if (!entity.Formula.IsNullOrEmpty())
                {
                    return new AdvancedFindFormulaFilterRow(this);
                }
                //return new AdvancedFindFilterRow(this);
            }

            return new AdvancedFindAfFilterRow(this, entity.Path)
            {
            };
        }

        /// <summary>
        /// Loads from lookup definition.
        /// </summary>
        /// <param name="lookupDefinition">The lookup definition.</param>
        /// <param name="creatingNew">if set to <c>true</c> [creating new].</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public void LoadFromLookupDefinition(LookupDefinitionBase lookupDefinition, bool creatingNew = false)
        {
            if (creatingNew)
            {
                _resetLookup = false;
                var nonFixedRows = Rows.OfType<AdvancedFindFilterRow>()
                    .Where(p => p.IsFixed == false);

                foreach (var dataEntryGridRow in nonFixedRows.ToList())
                {
                    RemoveRow(dataEntryGridRow);
                }
                _resetLookup = true;
            }

            var fixedRows = Rows.OfType<AdvancedFindFilterRow>()
                .Where(p => p.IsFixed);

            if (lookupDefinition != null && !fixedRows.Any())
            {
                var rowIndex = 0;
                foreach (var fixedFilter in lookupDefinition.FilterDefinition.FixedFilters)
                {
                    AdvancedFindFilterRow row = null;

                    switch (fixedFilter.Type)
                    {
                        case FilterItemTypes.Field:
                            row = new AdvancedFindFieldFilterRow(this);
                            break;
                        case FilterItemTypes.Formula:
                            row = new AdvancedFindFormulaFilterRow(this);
                            break;
                        case FilterItemTypes.AdvancedFind:
                            row = new AdvancedFindAfFilterRow(this, fixedFilter.Path);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    row.LoadFromFilterDefinition(fixedFilter, true, rowIndex);
                    AddRow(row);
                    if (rowIndex == lookupDefinition.FilterDefinition.FixedFilters.Count - 1)
                    {
                        var theEnd = !lookupDefinition.FilterDefinition.UserFilters.Any();
                        FinishOffFilter();
                    }

                    rowIndex++;
                }
            }
        }

        /// <summary>
        /// Loads the new user filter.
        /// </summary>
        /// <param name="filterReturn">The filter return.</param>
        public void LoadNewUserFilter(AdvancedFilterReturn filterReturn)
        {
            //var row = GetNewRow() as AdvancedFindFilterRow;
            //if (filterReturn != null)
            //{
            //    row.Path = ViewModel.SelectedTreeViewItem.MakePath();
            //    row.LoadFromFilterReturn(filterReturn);
            //}

            //ProcessLastFilterRow(false, Rows.LastOrDefault() as AdvancedFindFilterRow);

            filterReturn.NewIndex = GetNewRowIndex();
            AdvancedFindFilterRow row = null;
            if (filterReturn.Formula.IsNullOrEmpty())
            {
                row = new AdvancedFindFieldFilterRow(this);
            }
            else
            {
                row = new AdvancedFindFormulaFilterRow(this);
            }
            row.LoadFromFilterReturn(filterReturn);
            AddNewFilterRow(row);
        }

        /// <summary>
        /// Adds the new filter row.
        /// </summary>
        /// <param name="row">The row.</param>
        public void AddNewFilterRow(AdvancedFindFilterRow row)
        {
            var newIndex = GetNewRowIndex();

            if (newIndex < Rows.Count - 1 && newIndex >= 0)
            {
                var firstNewRow = Rows[newIndex];
                if (firstNewRow.IsNew)
                {
                    RemoveRow(firstNewRow);
                    AddNewRow();
                }
            }

            AddRow(row, newIndex);

            //if (Rows.Count > 1)
            //{
            //    var advancedRows = Rows.OfType<AdvancedFindFilterRow>().ToList();
            //}

            FinishOffFilter();
            Grid?.RefreshGridView();
            ViewModel.ResetLookup();
        }

        /// <summary>
        /// Gets the new index of the row.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public int GetNewRowIndex()
        {
            var result = -1;
            if (Rows.Any())
            {
                var lastRowIndex = GetLastRowIndex();
                result = lastRowIndex;
                //if (lastRowIndex != Rows.Count - 1)
                //{
                //    result = lastRowIndex;
                //}
            }
            return result;
        }

        /// <summary>
        /// Determines whether this instance [can insert row] the specified start index.
        /// </summary>
        /// <param name="startIndex">The start index.</param>
        /// <returns><c>true</c> if this instance [can insert row] the specified start index; otherwise, <c>false</c>.</returns>
        protected override bool CanInsertRow(int startIndex)
        {
            if (startIndex >= 0)
            {
                var rows = Rows.OfType<AdvancedFindFilterRow>().ToList();
                var rowToInsert = rows[startIndex];
                if (rowToInsert.IsFixed || rowToInsert.IsNew)
                {
                    return false;
                }
            }

            return base.CanInsertRow(startIndex);
        }

        /// <summary>
        /// Gets the last index of the row.
        /// </summary>
        /// <returns>System.Int32.</returns>
        private int GetLastRowIndex()
        {
            var lastRow = Rows.FirstOrDefault(p => p.IsNew);
            var lastRowIndex = Rows.IndexOf(lastRow);
            return lastRowIndex;
        }

        /// <summary>
        /// Loads the grid.
        /// </summary>
        /// <param name="entityList">The entity list.</param>
        public override void LoadGrid(IEnumerable<AdvancedFindFilter> entityList)
        {
            ViewModel.LookupDefinition.FilterDefinition.ClearUserFilters();
            base.LoadGrid(entityList);
            if (Rows.Any())
            {
                if (Rows.Count >= 2)
                {
                    var rowIndex = 0;
                    foreach (var advancedFindFilterRow in Rows.OfType<AdvancedFindFilterRow>())
                    {
                        if (rowIndex < Rows.Count - 1)
                        {
                            FinishOffFilter();
                        }

                        rowIndex++;
                    }
                }

                var lastRow = Rows.LastOrDefault() as AdvancedFindFilterRow;
                FinishOffFilter();
            }
            Grid?.RefreshGridView();
            //ViewModel.ResetLookup();
        }

        /// <summary>
        /// Processes the last filter row.
        /// </summary>
        /// <param name="theEnd">if set to <c>true</c> [the end].</param>
        /// <param name="row">The row.</param>
        private void ProcessLastFilterRow(bool theEnd, AdvancedFindFilterRow row)
        {
            if (Rows.Any())
            {
                var lastFilterRow = Rows.Last() as AdvancedFindFilterRow;
                FinishOffFilter();
            }
        }

        /// <summary>
        /// Handles the <see cref="E:RowsChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        protected override void OnRowsChanged(NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (HasData())
                    {
                        ViewModel.View.ShowFiltersEllipse(true);
                    }

                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (Rows.Any())
                    {
                        FinishOffFilter();
                        //if (_resetLookup )
                        //{
                        //    ViewModel.ResetLookup();
                        //}

                    }
                    if (Rows.Count <= 1)
                    {
                        ViewModel.View.ShowFiltersEllipse(false);
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            base.OnRowsChanged(e);
        }

        /// <summary>
        /// Clears the rows.
        /// </summary>
        /// <param name="addRowToBottom">if set to <c>true</c> [add row to bottom].</param>
        protected override void ClearRows(bool addRowToBottom = true)
        {
            var filterRows = Rows.OfType<AdvancedFindFilterRow>().ToList();
            var fixedRows = filterRows.Where(p => p.IsFixed);
            base.ClearRows(addRowToBottom);
            _resetLookup = false;
            var userRows = filterRows.Where(p => p.IsFixed == false
                && p.IsNew == false).ToList();

            var insertIndex = 0;
            foreach (var fixedRow in fixedRows)
            {
                AddRow(fixedRow, insertIndex);
                var count = Rows.Count;
                insertIndex++;
            }
            //if (Rows.Any())
            //{
            //    var lastFilterRow = Rows.Last() as AdvancedFindFilterRow;
            //    lastFilterRow.FinishOffFilter(false, true);
            //    Grid?.RefreshGridView();

            //}
            FinishOffFilter();
            _resetLookup = true;
        }

        /// <summary>
        /// Finishes the off filter.
        /// </summary>
        private void FinishOffFilter()
        {
            if (Rows.Any())
            {
                var newRows = Rows.OfType<AdvancedFindFilterRow>()
                    .Where(p => p.IsNew == false);

                if (newRows.Count() > 0)
                {
                    var lastRow = newRows.Last();
                    lastRow.FinishOffFilter(lastRow.IsFixed, true);
                }

                var nonFixedindex = 0;
                var nonNewRows = Rows.OfType<AdvancedFindFilterRow>().Where(p => !p.IsNew);
                var lastUserRow = nonNewRows.LastOrDefault();
                foreach (var nonFixedRow in nonNewRows)
                {
                    if (nonFixedindex < nonNewRows.Count() && nonNewRows.Count() > 1
                        && nonFixedRow != lastUserRow)
                    {
                        nonFixedRow.FinishOffFilter(nonFixedRow.IsFixed, false);
                    }
                    else if (lastUserRow == nonFixedRow)
                    {
                        nonFixedRow.FinishOffFilter(nonFixedRow.IsFixed, true);
                    }
                    {
                        
                    }
                    nonFixedindex++;
                }
            }
        }

        /// <summary>
        /// Validates the parentheses.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool ValidateParentheses()
        {
            var result = true;
            var leftParentheses = 0;
            var rightParentheses = 0;

            foreach (var filterRow in Rows.OfType<AdvancedFindFilterRow>())
            {
                leftParentheses += filterRow.LeftParenthesesCount;
                rightParentheses += filterRow.RightParenthesesCount;
            }

            if (leftParentheses != rightParentheses)
            {
                var caption = "Left Parentheses/Right Parentheses Count Mismatch";
                var title = "Parentheses Mismatch";
                ControlsGlobals.UserInterface.ShowMessageBox(caption, title, RsMessageBoxIcons.Exclamation);
                result = false;
            }
            return result;
        }

        /// <summary>
        /// Adds the advanced find filter row.
        /// </summary>
        /// <param name="field">The field.</param>
        public void AddAdvancedFindFilterRow(FieldDefinition field = null)
        {
            var row = new AdvancedFindAfFilterRow(this
                , ViewModel.SelectedTreeViewItem.MakePath()
                , field);
            
            AddNewFilterRow(row);
            if (Rows.Count > 1)
            {
                var advancedRows = Rows.OfType<AdvancedFindFilterRow>().ToList();
                FinishOffFilter();
                ViewModel.View.ShowFiltersEllipse(true);
            }
            //Grid?.RefreshGridView();
            Grid?.GotoCell(row, (int)FilterColumns.Search);
        }

        /// <summary>
        /// Validates the advanced find.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool ValidateAdvancedFind()
        {
            var rows = Rows.OfType<AdvancedFindAfFilterRow>().ToList();
            if (rows.Any())
            {
                foreach (var advancedFindAfFilterRow in rows)
                {
                    var advancedFindList = new List<int>();
                    advancedFindList.Add(ViewModel.AdvancedFindId);
                    var advancedFindId = advancedFindAfFilterRow.AdvancedFindId;
                    if (!ValidateAdvancedFind(advancedFindList, advancedFindId))
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Validates the advanced find.
        /// </summary>
        /// <param name="advancedFindList">The advanced find list.</param>
        /// <param name="advancedFindId">The advanced find identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool ValidateAdvancedFind(List<int> advancedFindList, int advancedFindId)
        {
            if (advancedFindId <= 0)
                return true;

            advancedFindList.Add(advancedFindId);
            var advancedFindSearchFor = SystemGlobals.AdvancedFindDbProcessor.GetAdvancedFind(advancedFindId);
            foreach (var advancedFindFilter in advancedFindSearchFor.Filters)
            {
                if (advancedFindFilter.SearchForAdvancedFindId.HasValue)
                {
                    if (advancedFindList.IndexOf(advancedFindFilter.SearchForAdvancedFindId.Value) != -1)
                    {
                        var message = "Advanced find circular reference found. Aborting query.";
                        var caption = "Circular Reference Found";
                        ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
                        return false;
                    }

                    if (!ValidateAdvancedFind(advancedFindList, advancedFindFilter.SearchForAdvancedFindId.Value))
                        return false;
                }
            }

            return true;
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
        /// Determines whether [is delete ok] [the specified row index].
        /// </summary>
        /// <param name="rowIndex">Index of the row.</param>
        /// <returns><c>true</c> if [is delete ok] [the specified row index]; otherwise, <c>false</c>.</returns>
        public override bool IsDeleteOk(int rowIndex)
        {
            var rows = Rows.OfType<AdvancedFindFilterRow>().ToList();
            var row = rows[rowIndex];
            if (rowIndex == Rows.Count - 1 && row.IsNew)
            {
                return false;
            }
            else if (!row.IsFixed)
            {
                return true;
            }

            return base.IsDeleteOk(rowIndex);
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
        /// Sets the focus to grid.
        /// </summary>
        public void SetFocusToGrid()
        {
            Grid?.GotoCell(Rows[0], (SearchColumnId));
        }
    }
}
