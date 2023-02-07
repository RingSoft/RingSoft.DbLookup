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

        //public AdvancedFindFilterDefinition Filter { get; set; }

        public int AdvancedFindId { get; set; }

        public AdvancedFindAfFilterRow(AdvancedFindFiltersManager manager
            , string path
            , FieldDefinition primaryFieldDefinition = null) : base(manager)
        {
            Path = path;
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

            var path = Path;
            if (!path.IsNullOrEmpty())
            {
                var foundItem = Manager.ViewModel.AdvancedFindTree.ProcessFoundTreeViewItem(path);
                if (foundItem != null)
                {
                    if (foundItem.FieldDefinition.ParentJoinForeignKeyDefinition != null)
                    {
                        primaryTable = foundItem.FieldDefinition.ParentJoinForeignKeyDefinition.PrimaryTable;
                    }
                }
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
            var filter = FilterItemDefinition as AdvancedFindFilterDefinition;
            if (AutoFillValue != null && AutoFillValue.IsValid())
            {
                var advancedFindId = AutoFillValue
                    .PrimaryKeyValue.KeyValueFields[0].Value.ToInt();

                if (FilterItemDefinition == null)
                {
                    FilterItemDefinition = Manager.ViewModel.LookupDefinition.FilterDefinition.AddUserFilter(advancedFindId,
                        Manager.ViewModel.LookupDefinition, Path);
                }
                else
                {
                    filter.AdvancedFindId = advancedFindId;
                }
                AdvancedFindId = advancedFindId;
            }
            else if (filter != null)
            {
                Manager.ViewModel.LookupDefinition.FilterDefinition.RemoveUserFilter(filter);
            }
        }

        public override void SaveToEntity(AdvancedFindFilter entity, int rowIndex)
        {
            if (AutoFillValue != null && AutoFillValue.IsValid())
            {
                entity.SearchForAdvancedFindId = AutoFillValue.PrimaryKeyValue.KeyValueFields[0].Value.ToInt();
            }

            var newPath = Path;
            //var foundItem = Manager.ViewModel.AdvancedFindTree.ProcessFoundTreeViewItem(Path, TreeViewType.Field);
            //if (foundItem != null)
            //{
            //    newPath = foundItem.FieldDefinition.MakePath();
            //}
            entity.Path = newPath;
            base.SaveToEntity(entity, rowIndex);
        }

        public override void LoadFromEntity(AdvancedFindFilter entity)
        {
            //FilterItemDefinition = Manager.ViewModel.LookupDefinition.LoadFromAdvFindFilter(entity);
            //LoadFromFilterDefinition(FilterItemDefinition, false, entity.FilterId);

            AutoFillValue = Manager.ViewModel.TableDefinition.Context.OnAutoFillTextRequest(
                SystemGlobals.AdvancedFindLookupContext.AdvancedFinds, entity.SearchForAdvancedFindId.ToString());
            Path = entity.Path;

            base.LoadFromEntity(entity);

            //Path = entity.Path;
            //CreateFilterDefinition();

            //base.LoadFromEntity(entity);
            //var test = this;
            //if (ParentFieldDefinition?.ParentJoinForeignKeyDefinition != null)
            //{
            //    SetupTableField(ParentFieldDefinition);
            //}
            //if (FilterItemDefinition is AdvancedFindFilterDefinition advancedFindFilter)
            //{
            //    Filter = advancedFindFilter;
            //    //Filter.AdvancedFindId = advancedFindFilter.AdvancedFindId;
            //}

            //SetupTableField(ParentFieldDefinition);
            //Manager.ViewModel.ResetLookup();
        }
    }
}
