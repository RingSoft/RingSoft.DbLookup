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

        public AdvancedFindColumnsManager(AdvancedFindViewModel viewModel) : base(viewModel)
        {
            ViewModel = viewModel;
        }

        protected override DataEntryGridRow GetNewRow()
        {
            return new AdvancedFindFieldColumnRow(this);
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
            columnRow?.LoadFromColumnDefinition(column);
            AddRow(columnRow);
            Grid?.RefreshGridView();
        }

        public override void RemoveRow(DataEntryGridRow rowToDelete)
        {
            base.RemoveRow(rowToDelete);
        }
    }
}
