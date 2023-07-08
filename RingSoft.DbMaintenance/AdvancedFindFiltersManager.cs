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
    public class AdvancedFindFiltersManager : DbMaintenanceDataEntryGridManager<AdvancedFindFilter>
    {
        public const int LeftParenthesesColumnId = 1;
        public const int TableColumnId = 2;
        public const int FieldColumnId = 3;
        public const int SearchColumnId = 4;
        public const int RightParenthesesColumnId = 5;
        public const int EndLogicColumnId = 6;

        public enum FilterColumns
        {
            LeftParentheses = LeftParenthesesColumnId,
            Table = TableColumnId,
            Field = FieldColumnId,
            Search = SearchColumnId,
            RightParentheses = RightParenthesesColumnId,
            EndLogic = EndLogicColumnId
        }

        public AdvancedFindViewModel ViewModel { get; set; }

        private bool _resetLookup = true;
        private bool _addingNewRow;

        public AdvancedFindFiltersManager(AdvancedFindViewModel viewModel) : base(viewModel)
        {
            ViewModel = viewModel;
        }

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

        public void AddNewFilterRow(AdvancedFindFilterRow row)
        {
            var newIndex = GetNewRowIndex();

            if (newIndex < Rows.Count - 1)
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

        private int GetLastRowIndex()
        {
            var lastRow = Rows.FirstOrDefault(p => p.IsNew);
            var lastRowIndex = Rows.IndexOf(lastRow);
            return lastRowIndex;
        }

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

        private void ProcessLastFilterRow(bool theEnd, AdvancedFindFilterRow row)
        {
            if (Rows.Any())
            {
                var lastFilterRow = Rows.Last() as AdvancedFindFilterRow;
                FinishOffFilter();
            }
        }

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

        public void AddNewRow()
        {
            var newRow = GetNewRow();
            AddRow(newRow, -1);
            Grid?.RefreshGridView();
        }

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

        public void SetFocusToGrid()
        {
            Grid?.GotoCell(Rows[0], (SearchColumnId));
        }
    }
}
