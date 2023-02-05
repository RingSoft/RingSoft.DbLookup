using System;
using System.Linq;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DbLookup.TableProcessing;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        public bool IsFixed { get; private set; }
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
        }

        private void ConvertDate(AdvancedFilterReturn filterReturn)
        {
            DateFilterType = filterReturn.DateFilterType;
            switch (DateFilterType)
            {
                case DateFilterTypes.SpecificDate:
                    break;
                default:
                    DateFilterValue = SearchValue.ToInt();
                    SearchValue = LookupDefinitionBase.ProcessSearchValue(SearchValue, DateFilterType);
                    break;
            }

            if (FieldDefinition is DateFieldDefinition dateField)
            {
                if (dateField.ConvertToLocalTime)
                {
                    var date = SearchValue.ToDate();
                    if (date != null)
                    {
                        SearchValue = date.Value.ToUniversalTime().FormatDateValue(dateField.DateType);

                        DisplaySearchValue = date.Value.FormatDateValue(dateField.DateType, false);
                        DateSearchValue = DisplaySearchValue;
                    }
                }

            }

            GetDateDisplayValue();
        }

        private void GetDateDisplayValue()
        {
            switch (DateFilterType)
            {
                case DateFilterTypes.SpecificDate:
                    DisplaySearchValue = string.Empty;
                    break;
                default:
                    var enumTranslation = new EnumFieldTranslation();
                    enumTranslation.LoadFromEnum<DateFilterTypes>();
                    var typeTrans = enumTranslation.TypeTranslations.FirstOrDefault(p =>
                        p.NumericValue == (int)DateFilterType);
                    if (typeTrans != null)
                    {
                        DisplaySearchValue = $"{DateFilterValue} {typeTrans.TextValue}";
                    }

                    break;
            }
        }

        //private void SetCellValueProcessField(AdvancedFindFilterCellProps filterProps, FieldFilterDefinition filter)
        //{
        //    SetCellValueFromLookupReturn(filterProps.FilterReturn);
        //    filter.Value = SearchValue;
        //    filter.Condition = Condition;
        //    if (FieldDefinition.ParentJoinForeignKeyDefinition != null)
        //    {
        //        FilterItemDefinition filterDefinition = null;
        //        var fieldDefinition = filterProps.FilterReturn.FieldDefinition;
        //        if (fieldDefinition.ParentJoinForeignKeyDefinition != null && fieldDefinition.TableDefinition != Manager.ViewModel.LookupDefinition.TableDefinition)
        //        {
        //            fieldDefinition = fieldDefinition.ParentJoinForeignKeyDefinition
        //                .ForeignKeyFieldJoins[0].PrimaryField;
        //        }

        //        switch (Condition)
        //        {
        //            case Conditions.EqualsNull:
        //            case Conditions.NotEqualsNull:
        //            case Conditions.Equals:
        //            case Conditions.NotEquals:
        //                var fieldToSearch = fieldDefinition;
        //                if (fieldDefinition.ParentJoinForeignKeyDefinition != null)
        //                {
        //                    fieldToSearch = fieldDefinition.ParentJoinForeignKeyDefinition.FieldJoins[0].PrimaryField;
        //                }
        //                FieldDefinition = fieldDefinition;
        //                AutoFillField = fieldDefinition;
        //                filterDefinition =
        //                    FilterItemDefinition.TableFilterDefinition.CreateFieldFilter(fieldToSearch,
        //                        Condition, SearchValue);
        //                break;
        //            default:
        //                var lookupColumn = FieldDefinition.ParentJoinForeignKeyDefinition
        //                    .PrimaryTable.LookupDefinition.InitialSortColumnDefinition;

        //                if (lookupColumn is LookupFieldColumnDefinition lookupFieldColumn)
        //                {
        //                    filterDefinition =
        //                        FilterItemDefinition.TableFilterDefinition.CreateFieldFilter(lookupFieldColumn.FieldDefinition,
        //                            Condition, SearchValue);
        //                }
        //                else if (lookupColumn is LookupFormulaColumnDefinition lookupFormulaColumn)
        //                {
        //                    filterDefinition = FilterItemDefinition.TableFilterDefinition.CreateFormulaFilter(
        //                        lookupFormulaColumn.OriginalFormula, lookupFormulaColumn.DataType, Condition, 
        //                        SearchValue, FilterItemDefinition.JoinDefinition.Alias);
        //                }
        //                break;
        //        }

        //        filterDefinition.JoinDefinition = FilterItemDefinition.JoinDefinition;
        //        FilterItemDefinition.TableFilterDefinition.ReplaceUserFilter(FilterItemDefinition,
        //            filterDefinition);
        //        FilterItemDefinition = filterDefinition;
        //        FilterItemDefinition.LeftParenthesesCount = LeftParenthesesCount;
        //        FilterItemDefinition.RightParenthesesCount = RightParenthesesCount;
        //        if (EndLogics.HasValue)
        //        {
        //            FilterItemDefinition.EndLogic = EndLogics.Value;
        //        }
        //        MakeSearchValueText();
        //    }
        //}

        public override void LoadFromEntity(AdvancedFindFilter entity)
        {
            var filter = Manager.ViewModel.LookupDefinition.LoadFromAdvFindFilter(entity);
            if (filter != null)
            {
                FilterItemDefinition = filter;
                FilterReturn = new AdvancedFilterReturn();
                FilterItemDefinition.SaveToFilterReturn(FilterReturn);
                FilterReturn.TableDescription = Table;
                FilterReturn.LookupDefinition = Manager.ViewModel.LookupDefinition;
                MakeSearchValueText();
            }
            //var lookupFilterResult = Manager.ViewModel.LookupDefinition.LoadFromAdvFindFilter(entity);
            //Path = entity.Path;
            //DateFilterType = (DateFilterTypes)entity.DateFilterType;
            //switch (DateFilterType)
            //{
            //    case DateFilterTypes.SpecificDate:
            //        break;
            //    default:
            //        DateFilterValue = entity.SearchForValue.ToInt();
            //        break;
            //}
            //if (lookupFilterResult != null && lookupFilterResult.FilterItemDefinition != null)
            //{
            //    Table = lookupFilterResult.FilterItemDefinition.TableDescription;
            //}
            //if (lookupFilterResult.FieldDefinition != null &&
            //    lookupFilterResult.FilterItemDefinition is FieldFilterDefinition)
            //{
            //    FieldDefinition = lookupFilterResult.FieldDefinition;
            //    if (FieldDefinition.ParentJoinForeignKeyDefinition != null)
            //    {
            //        AutoFillField = FieldDefinition;
            //    }
            //}
            //LoadFromFilterDefinition(lookupFilterResult.FilterItemDefinition, false, entity.AdvancedFindId);
            //if (lookupFilterResult.FieldDefinition != null)
            //{
            //    Field = lookupFilterResult.FieldDefinition.Description;
            //}
            //if (lookupFilterResult.FieldDefinition != null)
            //{
            //    //Table = lookupFilterResult.FieldDefinition.TableDefinition.Description;
            //    if (lookupFilterResult.FieldDefinition != null &&
            //        lookupFilterResult.FieldDefinition.ParentJoinForeignKeyDefinition != null &&
            //        lookupFilterResult.FilterItemDefinition is FormulaFilterDefinition)
            //    {
            //        FieldDefinition = lookupFilterResult.FieldDefinition;
            //        AutoFillField = FieldDefinition.ParentJoinForeignKeyDefinition.FieldJoins[0].PrimaryField;
            //    }
            //}

            ////if (FieldDefinition != null)
            //{
            //    //FieldDefinition = fieldFilter.FieldDefinition;
            //    PrimaryTable = entity.PrimaryTableName;
            //    PrimaryField = entity.PrimaryFieldName;
            //    MakeParentField();

            //    //if (ParentFieldDefinition != null)
            //    //{
            //    //    Table = ParentFieldDefinition.Description;
            //    //}
            //}
            //Formula = entity.Formula;
            //FormulaDisplayValue = entity.FormulaDisplayValue;
            //if (FieldDefinition == null && entity.SearchForAdvancedFindId == null)
            //{
            //    Field = $"{FormulaDisplayValue} Formula";
            //}
            //else if (entity.SearchForAdvancedFindId == null && Field.IsNullOrEmpty())
            //{
            //    Field = FieldDefinition.Description;
            //}


            //Table = FilterItemDefinition.TableDescription;
            //if (FilterItemDefinition is FieldFilterDefinition fieldFilter)
            //{
            //    Condition = fieldFilter.Condition;
            //    SearchValue = fieldFilter.Value;
            //    Field = fieldFilter.FieldDefinition.Description;
            //    MakeParentField();
            //    if (fieldFilter.FieldDefinition.ParentJoinForeignKeyDefinition != null)
            //    {
            //        AutoFillField = fieldFilter.FieldDefinition.ParentJoinForeignKeyDefinition.FieldJoins[0].PrimaryField;
            //    }

            //}
            //else if (FilterItemDefinition is FormulaFilterDefinition formulaFilter)
            //{
            //    if (formulaFilter.Condition != null) Condition = formulaFilter.Condition.Value;
            //    formulaFilter.FilterValue = entity.SearchForValue;
            //    Field = "<Formula>";
            //    Formula = formulaFilter.Formula;
            //    FormulaDataType = (FieldDataTypes) entity.FormulaDataType;
            //    FormulaDisplayValue = entity.FormulaDisplayValue;
            //}

            //LeftParenthesesCount = (byte)FilterItemDefinition.LeftParenthesesCount;
            //RightParenthesesCount = (byte)FilterItemDefinition.RightParenthesesCount;
            //MakeSearchValueText();

            //LeftParenthesesCount = entity.LeftParentheses;
            //var table = entity.TableName;
            //var field = entity.FieldName;
            //var tableDefinition =
            //    Manager.ViewModel.LookupDefinition.TableDefinition.Context.TableDefinitions.FirstOrDefault(p =>
            //        p.EntityName == table);

            //FieldDefinition fieldDefinition = null;
            //if (!field.IsNullOrEmpty())
            //{
            //    fieldDefinition = tableDefinition.FieldDefinitions.FirstOrDefault(p => p.PropertyName == field);
            //}

            //FieldDefinition = fieldDefinition;

            //PrimaryTable = entity.PrimaryTableName;
            //PrimaryField = entity.PrimaryFieldName;

            //MakeParentField();

            //Formula = entity.Formula;
            //if (!Formula.IsNullOrEmpty())
            //    Table = PrimaryTable;

            //RightParenthesesCount = entity.RightParentheses;
            //EndLogics = (EndLogics)entity.EndLogic;
            //Condition = (Conditions)entity.Operand;

            //SearchValue = entity.SearchForValue;
            //FormulaDataType = (FieldDataTypes) entity.FormulaDataType;
            //FormulaDisplayValue = entity.FormulaDisplayValue;

            //if (entity.SearchForAdvancedFindId == null)
            //{
            //    var filterReturn = MakeFilterReturn();

            //    LoadFromFilterReturn(filterReturn);
            //}
            //MakeSearchValueText();
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

        //private AdvancedFilterReturn MakeFilterReturn()
        //{
        //    var filterReturn = new AdvancedFilterReturn();
        //    filterReturn.Condition = Condition;
        //    filterReturn.PrimaryFieldDefinition = ParentFieldDefinition;

        //    if (FieldDefinition == null)
        //    {
        //        filterReturn.PrimaryTableName = Table;
        //    }

        //    filterReturn.FieldDefinition = FieldDefinition;


        //    filterReturn.Condition = Condition;
        //    filterReturn.SearchValue = SearchValue;
        //    if (FieldDefinition is DateFieldDefinition dateFieldDefinition 
        //        || (!Formula.IsNullOrEmpty() && FormulaDataType == FieldDataTypes.DateTime))
        //    {
        //        filterReturn.DateFilterType = DateFilterType;
        //        switch (DateFilterType)
        //        {
        //            case DateFilterTypes.SpecificDate:
        //                if (!DateSearchValue.IsNullOrEmpty())
        //                {
        //                    filterReturn.SearchValue = DateSearchValue;
        //                }
        //                break;
        //            default:
        //                filterReturn.SearchValue = DateFilterValue.ToString();
        //                break;
        //        }
        //    }

        //    filterReturn.Formula = Formula;
        //    filterReturn.FormulaValueType = FormulaDataType;
        //    filterReturn.FormulaDisplayValue = FormulaDisplayValue;
        //    filterReturn.LookupDefinition = Manager.ViewModel.LookupDefinition;

        //    return filterReturn;
        //}

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
            IsFixed = isFixed;
            if (isFixed)
            {
                AllowSave = false;
            }
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
                if (foundItem != null)
                {
                    if (foundItem.Parent != null)
                    {
                        Table = foundItem.Parent.Name;
                    }
                    else
                    {
                        Table = Manager.ViewModel.LookupDefinition.TableDefinition.Description;
                    }

                    Field = foundItem.Name;
                }
            }


            IsFixed = filter.IsFixed;
            if (filter.IsFixed && filter.TableFilterDefinition.FixedFilters.ToList().IndexOf(filter) == 0)
            {
                LeftParenthesesCount++;
            }
            
            MakeSearchValueText();
        }

        private void SetFixedTableName(TreeViewItem foundItem)
        {
            if (foundItem != null)
            {
                if (foundItem.Parent == null)
                {
                    Table = Manager.ViewModel.LookupDefinition.TableDefinition.Description;
                }
                else
                {
                    Table = foundItem.Parent.Name;
                }
            }
        }

        public void FinishOffFilter(bool isFixed, bool theEnd)
        {
            if (isFixed)
            {
                RightParenthesesCount++;
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
            SearchValueText = FilterItemDefinition.GetReportText(Manager.ViewModel.LookupDefinition);
 
        }

        private string MakeBeginSearchValueText()
        {
            return DbLookup.TableProcessing.FilterItemDefinition.GetConditionText(Condition);
        }

        public virtual void LoadFromFilterReturn(AdvancedFilterReturn advancedFilterReturn)
        {
            Condition = advancedFilterReturn.Condition;
            SearchValue = advancedFilterReturn.SearchValue;
            FilterReturn = advancedFilterReturn;

            MakeSearchValueText();
            }
 
        public override bool AllowUserDelete => !IsFixed;

        protected void SetupTable(TreeViewItem selectedTreeViewItem)
        {
            if (selectedTreeViewItem != null)
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
                Manager.ViewModel.ResetLookup();
            }

            base.Dispose();
        }
    }
}
