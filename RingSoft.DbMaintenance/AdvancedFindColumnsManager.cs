using RingSoft.DataEntryControls.Engine.DataEntryGrid;
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
            return new AdvancedFindColumnRow(this);
        }

        protected override DbMaintenanceDataEntryGridRow<AdvancedFindColumn> ConstructNewRowFromEntity(AdvancedFindColumn entity)
        {
            return new AdvancedFindColumnRow(this);
        }

        public void LoadFromLookupDefinition(LookupDefinitionBase lookupDefinition)
        {
            foreach (var column in lookupDefinition.VisibleColumns)
            {
                var newRow = new AdvancedFindColumnRow(this);
                newRow.LoadFromColumnDefinition(column);
                AddRow(newRow);
            }
        }
    }
}
