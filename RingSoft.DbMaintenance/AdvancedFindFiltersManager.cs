using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AdvancedFind;
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

        public AdvancedFindFiltersManager(AdvancedFindViewModel viewModel) : base(viewModel)
        {
            ViewModel = viewModel;
        }

        protected override DataEntryGridRow GetNewRow()
        {
            return new AdvancedFindFilterRow(this);
        }

        protected override DbMaintenanceDataEntryGridRow<AdvancedFindFilter> ConstructNewRowFromEntity(
            AdvancedFindFilter entity)
        {
            if (entity.Formula.IsNullOrEmpty())
            {
                var tableDefinition =
                    ViewModel.TableDefinition.Context.TableDefinitions.FirstOrDefault(p =>
                        p.EntityName == entity.TableName);

                if (tableDefinition != null)
                {
                    var fieldDefinition =
                        tableDefinition.FieldDefinitions.FirstOrDefault(p => p.FieldName == entity.FieldName);

                    var foundTreeViewItem = ViewModel.FindFieldInTree(ViewModel.TreeRoot, fieldDefinition);
                    if (foundTreeViewItem == null || !tableDefinition.CanViewTable)
                    {
                        ViewModel.ReadOnlyMode = true;
                        return null;
                    }
                }
                //else
                //{
                //    ViewModel.ReadOnlyMode = true;
                //    return null;
                //}
            }
            if (entity.SearchForAdvancedFindId == null)
            {
                return new AdvancedFindFilterRow(this);
            }

            if (entity.SearchForAdvancedFindId == null)
            {
                return new AdvancedFindFilterRow(this);
            }

            return new AdvancedFindAfFilterRow(this);
        }

        public void LoadFromLookupDefinition(LookupDefinitionBase lookupDefinition, bool creatingNew = false)
        {
            if (creatingNew)
            {
                _resetLookup = false;
                foreach (var dataEntryGridRow in Rows.ToList())
                {
                    RemoveRow(dataEntryGridRow);
                }
                _resetLookup = true;
            }

            if (lookupDefinition != null)
            {
                var rowIndex = 0;
                foreach (var fixedFilter in lookupDefinition.FilterDefinition.FixedFilters)
                {
                    var row = GetNewRow() as AdvancedFindFilterRow;
                    row.LoadFromFilterDefinition(fixedFilter, true, rowIndex);
                    AddRow(row);
                    if (rowIndex == lookupDefinition.FilterDefinition.FixedFilters.Count - 1)
                    {
                        var theEnd = !lookupDefinition.FilterDefinition.UserFilters.Any();
                        row.FinishOffFilter(true, theEnd);
                    }

                    rowIndex++;
                }
            }
        }

        public void LoadNewUserFilter(AdvancedFilterReturn filterReturn)
        {
            var row = GetNewRow() as AdvancedFindFilterRow;
            if (filterReturn != null)
                row.LoadFromFilterReturn(filterReturn);
            //if (ViewModel.LookupDefinition.FilterDefinition.FixedFilters.Any() &&
            //    ViewModel.LookupDefinition.FilterDefinition.FixedFilters.Count ==
            //    Rows.OfType<AdvancedFindFilterRow>().Where(p => p.IsFixed == true).Count())
            //{
            //    var lastFilterRow =
            //        Rows[ViewModel.LookupDefinition.FilterDefinition.FixedFilters.Count - 1] as
            //            AdvancedFindFilterRow;
            //    lastFilterRow.FinishOffFilter(true, false);
            //}
            ProcessLastFilterRow(false, Rows.LastOrDefault() as AdvancedFindFilterRow);

            AddRow(row);
            ProcessLastFilterRow(true, Rows.LastOrDefault() as AdvancedFindFilterRow);
            Grid?.RefreshGridView();
            ViewModel.ResetLookup();
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
                            advancedFindFilterRow.FinishOffFilter(false, false);
                        }

                        rowIndex++;
                    }
                }

                var lastRow = Rows.LastOrDefault() as AdvancedFindFilterRow;
                lastRow?.FinishOffFilter(false, true);
            }
            Grid?.RefreshGridView();
            //ViewModel.ResetLookup();
        }

        private void ProcessLastFilterRow(bool theEnd, AdvancedFindFilterRow row)
        {
            if (Rows.Any())
            {
                var lastFilterRow = Rows.Last() as AdvancedFindFilterRow;
                lastFilterRow.FinishOffFilter(false, theEnd);
            }
        }

        protected override void OnRowsChanged(NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                if (Rows.Any())
                {
                    ProcessLastFilterRow(true, Rows.Last() as AdvancedFindFilterRow);
                    if (_resetLookup)
                    {
                        ViewModel.ResetLookup();
                    }
                }
                
            }
            base.OnRowsChanged(e);
        }

        protected override void ClearRows(bool addRowToBottom = true)
        {
            _resetLookup = false;
            var filterRows = Rows.OfType<AdvancedFindFilterRow>();
            var userRows = filterRows.Where(p => p.IsFixed == false).ToList();
            
            foreach (var filterRow in userRows)
            {
                filterRow.Clearing = true;
                RemoveRow(filterRow);
            }

            if (Rows.Any())
            {
                var lastFilterRow = Rows.Last() as AdvancedFindFilterRow;
                lastFilterRow.FinishOffFilter(false, true);
                Grid?.RefreshGridView();

            }
            _resetLookup = true;
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
            var row = new AdvancedFindAfFilterRow(this, field);
            AddRow(row);
            if (Rows.Count > 1)
            {
                var advancedRows = Rows.OfType<AdvancedFindFilterRow>().ToList();
                advancedRows[Rows.Count - 2].FinishOffFilter(false, false);
            }
            Grid?.RefreshGridView();
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
    }
}
