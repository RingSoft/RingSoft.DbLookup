﻿using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.Lookup;
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
            return new AdvancedFindFilterRow(this);
        }

        public void LoadFromLookupDefinition(LookupDefinitionBase lookupDefinition, bool creatingNew = false)
        {
            if (creatingNew)
            {
                foreach (var dataEntryGridRow in Rows.ToList())
                {
                    RemoveRow(dataEntryGridRow);
                }
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
            ViewModel.ResetLookup();
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
                    ViewModel.ResetLookup();
                }
                
            }
            base.OnRowsChanged(e);
        }

        protected override void ClearRows(bool addRowToBottom = true)
        {
            var filterRows = Rows.OfType<AdvancedFindFilterRow>();
            var userRows = filterRows.Where(p => p.IsFixed == false).ToList();
            
            foreach (var filterRow in userRows)
            {
                RemoveRow(filterRow);
            }

            if (Rows.Any())
            {
                var lastFilterRow = Rows.Last() as AdvancedFindFilterRow;
                lastFilterRow.FinishOffFilter(false, true);
                Grid?.RefreshGridView();

            }
        }
    }
}
