using System;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.TableProcessing;

namespace RingSoft.DbMaintenance
{
    public class AdvancedFindFieldFilterRow : AdvancedFindFilterRow
    {
        public AdvancedFindFieldFilterRow(AdvancedFindFiltersManager manager) : base(manager)
        {
        }

        public override void LoadFromFilterDefinition(FilterItemDefinition filter, bool isFixed, int rowIndex)
        {
            if (filter is FieldFilterDefinition fieldFilter)
            {
                //var path = fieldFilter.Path;
                //if (path.IsNullOrEmpty() && fieldFilter.FieldDefinition != null)
                //{
                    Field = fieldFilter.FieldDefinition.Description;
                    //if (fieldFilter.JoinDefinition != null)
                    {
                        //Table = fieldFilter.JoinDefinition.ForeignKeyDefinition.FieldJoins[0].ForeignField.Description;
                        Table = fieldFilter.TableDescription;
                    }
                    //}
            }
            base.LoadFromFilterDefinition(filter, isFixed, rowIndex);
        }

        public override void LoadFromFilterReturn(AdvancedFilterReturn advancedFilterReturn)
        {
            var treeViewItem =
                Manager.ViewModel.LookupDefinition.AdvancedFindTree.ProcessFoundTreeViewItem(advancedFilterReturn.Path,
                    TreeViewType.Field);

            SetupTable(treeViewItem);
            Field = treeViewItem.Name;

            if (FilterItemDefinition == null)
            {
                FilterItemDefinition = Manager.ViewModel.LookupDefinition.FilterDefinition
                    .AddUserFilter(advancedFilterReturn.FieldDefinition, advancedFilterReturn.Condition,
                        advancedFilterReturn.SearchValue, GetNewFilterIndex());
            }

            FilterItemDefinition.LoadFromFilterReturn(advancedFilterReturn, treeViewItem);
            
            base.LoadFromFilterReturn(advancedFilterReturn);
        }

        public override void LoadFromEntity(AdvancedFindFilter entity)
        {
            //Base does this
            //FilterItemDefinition = Manager.ViewModel.LookupDefinition.LoadFromAdvFindFilter(entity);
            //LoadFromFilterDefinition(FilterItemDefinition, false, entity.FilterId);

            if (!entity.Path.IsNullOrEmpty())
            {
                var treeViewItem =
                    Manager.ViewModel.LookupDefinition.AdvancedFindTree.ProcessFoundTreeViewItem(entity.Path);

                if (treeViewItem != null)
                {
                    SetupTable(treeViewItem);
                    Field = treeViewItem.Name;
                }
            }

            base.LoadFromEntity(entity);
        }
    }
}
