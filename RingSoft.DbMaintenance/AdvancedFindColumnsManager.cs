using System.Collections.Specialized;
using System.Linq;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.Lookup;

namespace RingSoft.DbMaintenance
{
    public class AdvancedFindColumnsManager : DbMaintenanceDataEntryGridManager<AdvancedFindColumn>
    {
        public new AdvancedFindViewModel ViewModel { get; set; }

        private bool _addingNewRow;

        public AdvancedFindColumnsManager(AdvancedFindViewModel viewModel) : base(viewModel)
        {
            ViewModel = viewModel;
        }

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

        public void AddNewRow()
        {
            var newRow = GetNewRow();
            AddRow(newRow, -1);
            Grid?.RefreshGridView();
        }

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

        public void UpdateColumnWidth(LookupColumnDefinitionBase column)
        {
            var columnRow = Rows.OfType<AdvancedFindColumnRow>()
                .FirstOrDefault(p => p.LookupColumnDefinition == column);
            columnRow?.UpdatePercentWidth();
        }

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
