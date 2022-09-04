using System;
using System.Linq;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.QueryBuilder;

namespace RingSoft.DbMaintenance
{
    public class AdvancedFindAfFilterRow : AdvancedFindFilterRow
    {
        public AutoFillSetup AutoFillSetup { get; set; }

        public AutoFillValue AutoFillValue { get; set; }

        public AdvancedFindAfFilterRow(AdvancedFindFiltersManager manager, FieldDefinition primaryFieldDefinition = null) : base(manager)
        {
            var lookup = SystemGlobals.AdvancedFindLookupContext.AdvancedFindLookup.Clone();
            var primaryTable = manager.ViewModel.LookupDefinition.TableDefinition;
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

            lookup.FilterDefinition.AddFixedFilter(p => p.Table, Conditions.Equals, primaryTable.EntityName);
            if (manager.ViewModel.AdvancedFindId != 0)
            {
                lookup.FilterDefinition.AddFixedFilter(p => p.Id, Conditions.NotEquals,
                    manager.ViewModel.AdvancedFindId);
            }

            Field = "<Advanced Find>";
            AutoFillSetup = new AutoFillSetup(lookup);
            object inputParameter = null;
            if (manager.ViewModel.AdvancedFindInput != null)
            {
                inputParameter = manager.ViewModel.AdvancedFindInput.InputParameter;
            }

            AutoFillSetup.AddViewParameter = new AdvancedFindInput
            {
                InputParameter = inputParameter,
                LockTable = primaryTable
            };
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (AdvancedFindFiltersManager.FilterColumns) columnId;
            switch (column)
            {
                case AdvancedFindFiltersManager.FilterColumns.Search:
                    return new DataEntryGridAutoFillCellProps(this, columnId, AutoFillSetup, AutoFillValue);
                
            }
            return base.GetCellProps(columnId);
        }

        public override void SetCellValue(DataEntryGridEditingCellProps value)
        {
            var column = (AdvancedFindFiltersManager.FilterColumns)value.ColumnId;
            switch (column)
            {
                case AdvancedFindFiltersManager.FilterColumns.Search:
                    var autoFillProps = value as DataEntryGridAutoFillCellProps;
                    AutoFillValue = autoFillProps.AutoFillValue;
                    break;
            }

            base.SetCellValue(value);
        }
    }
}
