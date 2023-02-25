using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DbLookup.TableProcessing;
using System;
using System.Linq;

namespace RingSoft.DbMaintenance
{
    public abstract class AdvancedFindFilterRow : DbMaintenanceDataEntryGridRow<AdvancedFindFilter>
    {
        public AdvancedFindFiltersManager Manager { get; set; }

        public int LeftParenthesesCount { get; set; }
        public string Table { get; set; }
        public string Field { get; set; }
        public string SearchValueText { get; private set; }
        public int RightParenthesesCount { get; set; }
        public EndLogics? EndLogics { get; private set; }

        public Conditions Condition { get; set; }
        public string SearchValue { get; private set; }
        public string DisplaySearchValue { get; private set; }
        public FilterItemDefinition FilterItemDefinition { get; internal set; }
        public FieldDefinition FieldDefinition { get; private set; }
        public string PrimaryTable { get; set; }
        public string PrimaryField { get; set; }
        public FieldDefinition ParentFieldDefinition { get; private set; }
        public string FormulaDisplayValue { get; private set; }
        public DateFilterTypes DateFilterType { get; private set; }
        public int DateFilterValue { get; set; }
        public string DateSearchValue { get; private set; }

        public TextComboBoxControlSetup EndLogicsSetup { get; private set; }

        protected bool ResetLookup { get; set; } = true;

        public FieldDefinition AutoFillField { get; private set; }

        public bool Clearing { get; set; }

        public string Path { get; internal set; }

        public AdvancedFilterReturn FilterReturn { get; set; }

        private bool _fixedParenthesesSet = false;

        private FieldDefinition _searchAutoFillField;

        public AdvancedFindFilterRow(AdvancedFindFiltersManager manager) : base(manager)
        {
            Manager = manager;
            EndLogicsSetup = new TextComboBoxControlSetup();
            EndLogicsSetup.LoadFromEnum<EndLogics>();
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (AdvancedFindFiltersManager.FilterColumns) columnId;

            switch (column)
            {
                case AdvancedFindFiltersManager.FilterColumns.LeftParentheses:
                    return new AdvancedFilterParenthesesCellProps(this, columnId,
                        "(".StringDuplicate(LeftParenthesesCount), '(');

                case AdvancedFindFiltersManager.FilterColumns.Table:
                    return new DataEntryGridTextCellProps(this, columnId, Table);

                case AdvancedFindFiltersManager.FilterColumns.Field:
                    return new DataEntryGridTextCellProps(this, columnId, Field);

                case AdvancedFindFiltersManager.FilterColumns.Search:
                    return new AdvancedFindFilterCellProps(this, columnId, SearchValueText, FilterReturn);

                case AdvancedFindFiltersManager.FilterColumns.RightParentheses:
                    return new AdvancedFilterParenthesesCellProps(this, columnId,
                        ")".StringDuplicate(RightParenthesesCount), ')');

                case AdvancedFindFiltersManager.FilterColumns.EndLogic:
                    if (EndLogics == null)
                    {
                        return new DataEntryGridTextCellProps(this, columnId);
                    }
                    var result = new DataEntryGridTextComboBoxCellProps(this, columnId, EndLogicsSetup,
                        EndLogicsSetup.GetItem((int) EndLogics), ComboBoxValueChangedTypes.SelectedItemChanged);
                    return result;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override DataEntryGridCellStyle GetCellStyle(int columnId)
        {
            if (IsFixed)
            {
                return new DataEntryGridCellStyle() { State = DataEntryGridCellStates.Disabled };
            }
            var column = (AdvancedFindFiltersManager.FilterColumns) columnId;

            switch (column)
            {
                case AdvancedFindFiltersManager.FilterColumns.Table:
                case AdvancedFindFiltersManager.FilterColumns.Field:
                    return new DataEntryGridCellStyle() {State = DataEntryGridCellStates.Disabled};
                case AdvancedFindFiltersManager.FilterColumns.EndLogic:
                    if (EndLogics == null)
                    {
                        return new DataEntryGridCellStyle() {State = DataEntryGridCellStates.Disabled};
                    }
                    break;
                case AdvancedFindFiltersManager.FilterColumns.LeftParentheses:
                case AdvancedFindFiltersManager.FilterColumns.RightParentheses:
                    break;
            }

            return base.GetCellStyle(columnId);
        }

        public override void SetCellValue(DataEntryGridEditingCellProps value)
        {
            var column = (AdvancedFindFiltersManager.FilterColumns) value.ColumnId;

            switch (column)
            {
                case AdvancedFindFiltersManager.FilterColumns.LeftParentheses:
                    var leftParenthesesValue = value as DataEntryGridTextCellProps;
                    LeftParenthesesCount = (byte) leftParenthesesValue.Text.Length;
                    FilterItemDefinition.LeftParenthesesCount = LeftParenthesesCount;
                    break;
                case AdvancedFindFiltersManager.FilterColumns.Search:
                    var filterProps = value as AdvancedFindFilterCellProps;
                    if (filterProps == null)
                    {
                        base.SetCellValue(value);
                        return;
                    }
                    LoadFromFilterReturn(filterProps.FilterReturn);
                    break;
                case AdvancedFindFiltersManager.FilterColumns.RightParentheses:
                    var rightParenthesesValue = value as DataEntryGridTextCellProps;
                    RightParenthesesCount = (byte) rightParenthesesValue.Text.Length;
                    FilterItemDefinition.RightParenthesesCount = RightParenthesesCount;
                    break;
                case AdvancedFindFiltersManager.FilterColumns.EndLogic:
                    var endLogicsValue = value as DataEntryGridTextComboBoxCellProps;
                    EndLogics = (EndLogics) endLogicsValue.SelectedItem.NumericValue;
                    FilterItemDefinition.EndLogic = (EndLogics)EndLogics;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            Manager.Grid?.UpdateRow(this);
            if (ResetLookup)
            {
                Manager.ViewModel.ResetLookup();
            }
            else
            {
                ResetLookup = true;
            }

            base.SetCellValue(value);
        }

        public void SetCellValueFromLookupReturn(AdvancedFilterReturn filterReturn)
        {
            LoadFromFilterReturn(filterReturn);
            Manager.ViewModel.RefreshLookup();
        }


        public override void LoadFromEntity(AdvancedFindFilter entity)
        {
            var filter = Manager.ViewModel.LookupDefinition.LoadFromAdvFindFilter(entity, true, null);
            LoadFromFilterDefinition(filter, false, entity.FilterId);
            if (filter != null)
            {
                FilterItemDefinition = filter;
                FilterReturn = new AdvancedFilterReturn();
                FilterItemDefinition.SaveToFilterReturn(FilterReturn);
                FilterReturn.TableDescription = Table;
                FilterReturn.LookupDefinition = Manager.ViewModel.LookupDefinition;
                MakeSearchValueText();
            }
        }

        protected void MakeParentField()
        {
            if (!PrimaryTable.IsNullOrEmpty() && !PrimaryField.IsNullOrEmpty())
            {
                var primaryTableDefinition =
                    Manager.ViewModel.LookupDefinition.TableDefinition.Context.TableDefinitions.FirstOrDefault(p =>
                        p.TableName == PrimaryTable);

                ParentFieldDefinition = primaryTableDefinition.FieldDefinitions
                    .FirstOrDefault(p => p.FieldName == PrimaryField);
            }
        }


        public override bool ValidateRow()
        {
            return true;
        }

        public override void SaveToEntity(AdvancedFindFilter entity, int rowIndex)
        {
            entity.FilterId = rowIndex + 1;
            FilterItemDefinition.SaveToEntity(entity);
            {

            }
        }

        public virtual void LoadFromFilterDefinition(FilterItemDefinition filter, bool isFixed, int rowIndex)
        {
            IsNew = false;
            IsFixed = isFixed;
            if (isFixed)
            {
                AllowSave = false;
                if (rowIndex == 0)
                {
                    LeftParenthesesCount++;
                }
            }
            Path = filter.Path;
            FilterItemDefinition = filter;
            if (filter.Path.IsNullOrEmpty())
            {
                Table = Manager.ViewModel.LookupDefinition.TableDefinition.Description;
            }
            else
            {
                var foundItem =
                    Manager.ViewModel.LookupDefinition.AdvancedFindTree.ProcessFoundTreeViewItem(filter.Path,
                        filter.TreeViewType);
                if (foundItem != null && Table.IsNullOrEmpty())
                {
                    if (foundItem.Parent != null)
                    {
                        Table = foundItem.Parent.Name;
                    }
                    else
                    {
                        Table = Manager.ViewModel.LookupDefinition.TableDefinition.Description;
                    }

                    if (Field.IsNullOrEmpty())
                    {
                        Field = foundItem.Name;
                    }
                }
            }

            LeftParenthesesCount += filter.LeftParenthesesCount;
            RightParenthesesCount += filter.RightParenthesesCount;
            EndLogics = filter.EndLogic;

            IsFixed = filter.IsFixed;
            //if (filter.IsFixed && filter.TableFilterDefinition.FixedFilters.ToList().IndexOf(filter) == 0)
            //{
            //    LeftParenthesesCount++;
            //}
            
            MakeSearchValueText();
        }

        public void FinishOffFilter(bool isFixed, bool theEnd)
        {
            if (isFixed && !_fixedParenthesesSet && theEnd)
            {
                RightParenthesesCount++;
                _fixedParenthesesSet = true;
            }

            if (theEnd)
            {
                EndLogics = null;
                if (FilterItemDefinition != null)
                {
                    FilterItemDefinition.EndLogic = DbLookup.QueryBuilder.EndLogics.And;
                }
            }
            else
            {
                if (EndLogics == null)
                {
                    EndLogics = DbLookup.QueryBuilder.EndLogics.And;
                }

                if (FilterItemDefinition != null)
                {
                    FilterItemDefinition.EndLogic = (DbLookup.QueryBuilder.EndLogics) EndLogics;
                }
            }
        }

        public void MakeSearchValueText(string searchValue = "")
        {
            SearchValueText = FilterItemDefinition.GetReportText(Manager.ViewModel.LookupDefinition, false);
 
        }

        protected virtual int GetNewFilterIndex()
        {
            var result = Manager.GetNewRowIndex();
            var fixedItems = Manager.Rows.OfType<AdvancedFindFilterRow>()
                .Where(p => p.IsFixed)
                .ToList();
            return result - fixedItems.Count;
        }

        public virtual void LoadFromFilterReturn(AdvancedFilterReturn advancedFilterReturn)
        {
            IsNew = false;
            Condition = advancedFilterReturn.Condition;
            SearchValue = advancedFilterReturn.SearchValue;
            FilterReturn = advancedFilterReturn;

            MakeSearchValueText();
        }
 
        public override bool AllowUserDelete => !IsFixed;

        protected void SetupTable(TreeViewItem selectedTreeViewItem)
        {
            if (selectedTreeViewItem == null)
            {
                Table = Manager.ViewModel.LookupDefinition.TableDefinition.Description;
            }
            else
            {
                if (selectedTreeViewItem.Parent == null)
                {
                    Table = Manager.ViewModel.LookupDefinition.TableDefinition.Description;
                }
                else
                {
                    Table = selectedTreeViewItem.Parent.Name;
                }
            }
        }

        public override void Dispose()
        {
            if (FilterItemDefinition != null)
                FilterItemDefinition.TableFilterDefinition.RemoveUserFilter(FilterItemDefinition);
            if (!Manager.ViewModel.Clearing)
            {
                if (ValidateParentheses())
                {
                    Manager.ViewModel.ResetLookup();
                }
            }

            base.Dispose();
        }

        private bool ValidateParentheses()
        {
            if (FilterItemDefinition != null)
            {
                var leftParenCount = 0;
                var rightParenCount = 0;
                foreach (var userFilter in FilterItemDefinition.TableFilterDefinition.UserFilters)
                {
                    leftParenCount += userFilter.LeftParenthesesCount;
                    rightParenCount += userFilter.RightParenthesesCount;
                }

                if (leftParenCount != rightParenCount)
                {
                    Manager.ViewModel.ClearLookup(false);
                    return false;
                }
            }

            return true;
        }
    }
}
