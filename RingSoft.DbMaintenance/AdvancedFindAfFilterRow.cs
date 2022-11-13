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
    public class AdvancedFindAfFilterRow : AdvancedFindFilterRow
    {
        public AutoFillSetup AutoFillSetup { get; set; }

        public AutoFillValue AutoFillValue { get; set; }

        public AdvancedFindFilterDefinition Filter { get; set; }

        public int AdvancedFindId { get; set; }

        public AdvancedFindAfFilterRow(AdvancedFindFiltersManager manager, FieldDefinition primaryFieldDefinition = null) : base(manager)
        {
            SetupTableField(primaryFieldDefinition);
        }

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

        private void CreateFilterDefinition()
        {
            if (AutoFillValue != null && AutoFillValue.IsValid())
            {
                var advancedFindId = AutoFillValue
                    .PrimaryKeyValue.KeyValueFields[0].Value.ToInt();
                if (Filter == null)
                {
                    FilterItemDefinition = Filter = Manager.ViewModel.LookupDefinition.FilterDefinition.AddUserFilter(advancedFindId,
                        Manager.ViewModel.LookupDefinition);
                }
                else
                {
                    Filter.AdvancedFindId = advancedFindId;
                }
                AdvancedFindId = advancedFindId;
            }
            else if (Filter != null)
            {
                Manager.ViewModel.LookupDefinition.FilterDefinition.RemoveUserFilter(Filter);
            }
        }

        public override void SaveToEntity(AdvancedFindFilter entity, int rowIndex)
        {
            if (AutoFillValue != null && AutoFillValue.IsValid())
            {
                entity.SearchForAdvancedFindId = AutoFillValue.PrimaryKeyValue.KeyValueFields[0].Value.ToInt();
            }

            base.SaveToEntity(entity, rowIndex);
        }

        public override void LoadFromEntity(AdvancedFindFilter entity)
        {
            AutoFillValue = Manager.ViewModel.TableDefinition.Context.OnAutoFillTextRequest(
                SystemGlobals.AdvancedFindLookupContext.AdvancedFinds, entity.SearchForAdvancedFindId.ToString());

            //CreateFilterDefinition();

            base.LoadFromEntity(entity);
            if (FilterItemDefinition is AdvancedFindFilterDefinition advancedFindFilter)
            {
                Filter = advancedFindFilter;
                Filter.AdvancedFindId = advancedFindFilter.AdvancedFindId;
            }
            
            //SetupTableField(ParentFieldDefinition);
            //Manager.ViewModel.ResetLookup();
        }
    }
}
