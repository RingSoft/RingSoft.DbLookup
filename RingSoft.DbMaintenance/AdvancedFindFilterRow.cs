using System;
using System.Linq;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DbLookup.TableProcessing;

namespace RingSoft.DbMaintenance
{
    public class AdvancedFindFilterRow : DbMaintenanceDataEntryGridRow<AdvancedFindFilter>
    {
        public AdvancedFindFiltersManager Manager { get; set; }

        public byte LeftParenthesesCount { get; set; }
        public string Table { get; private set; }
        public string Field { get; private set; }
        public string SearchValueText { get; private set; }
        public byte RightParenthesesCount { get; set; }
        public EndLogics? EndLogics { get; private set; }

        public Conditions Condition { get; private set; }
        public string SearchValue { get; private set; }
        public FilterItemDefinition FilterItemDefinition { get; private set; }
        public string Formula { get; private set; }
        public FieldDataTypes FormulaDataType { get; private set; }
        public bool IsFixed { get; private set; }
        public string PrimaryTable { get; private set; }
        public string PrimaryField { get; private set; }

        public TextComboBoxControlSetup EndLogicsSetup { get; private set; }

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
                    return new DataEntryGridTextCellProps(this, columnId, "(".StringDuplicate(LeftParenthesesCount));

                case AdvancedFindFiltersManager.FilterColumns.Table:
                    return new DataEntryGridTextCellProps(this, columnId, Table);

                case AdvancedFindFiltersManager.FilterColumns.Field:
                    return new DataEntryGridTextCellProps(this, columnId, Field);

                case AdvancedFindFiltersManager.FilterColumns.Search:
                    return new DataEntryGridTextCellProps(this, columnId, SearchValueText);

                case AdvancedFindFiltersManager.FilterColumns.RightParentheses:
                    return new DataEntryGridTextCellProps(this, columnId, ")".StringDuplicate(RightParenthesesCount));

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
                    break;
                case AdvancedFindFiltersManager.FilterColumns.Search:
                    break;
                case AdvancedFindFiltersManager.FilterColumns.RightParentheses:
                    var rightParenthesesValue = value as DataEntryGridTextCellProps;
                    RightParenthesesCount = (byte) rightParenthesesValue.Text.Length;
                    break;
                case AdvancedFindFiltersManager.FilterColumns.EndLogic:
                    var endLogicsValue = value as DataEntryGridTextComboBoxCellProps;
                    EndLogics = (EndLogics) endLogicsValue.SelectedItem.NumericValue;
                    FilterItemDefinition.EndLogic = (EndLogics)EndLogics;
                    Manager.ViewModel.ResetLookup();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            base.SetCellValue(value);
        }

        public override void LoadFromEntity(AdvancedFindFilter entity)
        {
            var filterReturn = new AdvancedFilterReturn();
            filterReturn.Condition = Condition;
            filterReturn.SearchValue = SearchValue;

            LeftParenthesesCount = entity.LeftParentheses;
            var table = entity.TableName;
            var field = entity.FieldName;
            if (entity.Formula.IsNullOrEmpty())
            {
                var tableDefinition =
                    Manager.ViewModel.LookupDefinition.TableDefinition.Context.TableDefinitions.FirstOrDefault(p =>
                        p.EntityName == table);

                var fieldDefinition = tableDefinition.FieldDefinitions.FirstOrDefault(p => p.PropertyName == field);

                Table = tableDefinition.Description;
                Field = fieldDefinition.Description;

                filterReturn.FieldDefinition = fieldDefinition;

            }

            filterReturn.Condition = (Conditions)entity.Operand;
            filterReturn.SearchValue = entity.SearchForValue;

            RightParenthesesCount = entity.RightParentheses;
            EndLogics = (EndLogics) entity.EndLogic;
            filterReturn.Formula = entity.Formula;
            filterReturn.FormulaValueType = (FieldDataTypes)entity.FormulaDataType;
            PrimaryTable = entity.PrimaryTableName;
            PrimaryField = entity.PrimaryFieldName;
            if (!PrimaryTable.IsNullOrEmpty() && !PrimaryField.IsNullOrEmpty() && filterReturn.FieldDefinition == null)
            {
                var primaryTableDefinition =
                    Manager.ViewModel.LookupDefinition.TableDefinition.Context.TableDefinitions.FirstOrDefault(p =>
                        p.TableName == PrimaryTable);

                var primaryFieldDefinition =
                    primaryTableDefinition.FieldDefinitions.FirstOrDefault(p => p.FieldName == PrimaryField);

                filterReturn.FormulaParentFieldDefinition = primaryFieldDefinition;
            }

            LoadFromFilterReturn(filterReturn);
            //MakeSearchValueText();
        }

        public override bool ValidateRow()
        {
            return true;
        }

        public override void SaveToEntity(AdvancedFindFilter entity, int rowIndex)
        {
            
            entity.AdvancedFindId = Manager.ViewModel.AdvancedFindId;
            entity.FilterId = rowIndex + 1;
            entity.LeftParentheses = LeftParenthesesCount;
            if (FilterItemDefinition is FieldFilterDefinition fieldFilterDefinition)
            {
                entity.TableName = fieldFilterDefinition.FieldDefinition.TableDefinition.EntityName;
                entity.FieldName = fieldFilterDefinition.FieldDefinition.PropertyName;
            }

            entity.Operand = (byte) Condition;
            entity.SearchForValue = SearchValue;
            entity.RightParentheses = RightParenthesesCount;
            entity.PrimaryTableName = PrimaryTable;
            entity.PrimaryFieldName = PrimaryField;
            if (EndLogics != null)
                entity.EndLogic = (byte) EndLogics;
            entity.Formula = Formula;
            entity.FormulaDataType = (byte)FormulaDataType;
        }

        public void LoadFromFilterDefinition(FilterItemDefinition filter, bool isFixed, int rowIndex)
        {
            FilterItemDefinition = filter;
            LeftParenthesesCount = (byte) filter.LeftParenthesesCount;
            if (isFixed && rowIndex == 0)
            {
                LeftParenthesesCount++;
            }
            if (filter is FieldFilterDefinition fieldFilterDefinition)
            {
                Table = fieldFilterDefinition.FieldDefinition.TableDefinition.Description;
                Field = fieldFilterDefinition.FieldDefinition.Description;
                Condition = fieldFilterDefinition.Condition;
                SearchValue = fieldFilterDefinition.Value;

                switch (fieldFilterDefinition.FieldDefinition.FieldDataType)
                {
                    case FieldDataTypes.String:
                        if (fieldFilterDefinition.FieldDefinition is StringFieldDefinition stringField)
                            Manager.ViewModel.LookupDefinition.FilterDefinition.AddFixedFilter(stringField,
                                fieldFilterDefinition.Condition,
                                fieldFilterDefinition.Value);
                        break;
                    case FieldDataTypes.Integer:
                        if (fieldFilterDefinition.FieldDefinition is IntegerFieldDefinition integerField)
                            Manager.ViewModel.LookupDefinition.FilterDefinition.AddFixedFilter(integerField,
                                fieldFilterDefinition.Condition,
                                fieldFilterDefinition.Value.ToInt());
                        break;
                    case FieldDataTypes.Decimal:
                        if (fieldFilterDefinition.FieldDefinition is DecimalFieldDefinition decimalField)
                            Manager.ViewModel.LookupDefinition.FilterDefinition.AddFixedFilter(decimalField,
                                fieldFilterDefinition.Condition,
                                fieldFilterDefinition.Value.ToDecimal());
                        break;
                    case FieldDataTypes.DateTime:
                        if (fieldFilterDefinition.FieldDefinition is DateFieldDefinition dateField)
                            Manager.ViewModel.LookupDefinition.FilterDefinition.AddFixedFilter(dateField,
                                fieldFilterDefinition.Condition,
                                DateTime.Parse(fieldFilterDefinition.Value));
                        break;
                    case FieldDataTypes.Bool:
                        if (fieldFilterDefinition.FieldDefinition is BoolFieldDefinition boolField)
                            Manager.ViewModel.LookupDefinition.FilterDefinition.AddFixedFilter(boolField,
                                fieldFilterDefinition.Condition,
                                fieldFilterDefinition.Value.ToBool());

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                MakeSearchValueText();

            }

            RightParenthesesCount = (byte) filter.RightParenthesesCount;
            EndLogics = filter.EndLogic;
            IsFixed = isFixed;
            if (isFixed)
            {
                AllowSave = false;
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

        public void MakeSearchValueText()
        {
            var searchValueText = MakeBeginSearchValueText();
            if (FilterItemDefinition is FieldFilterDefinition fieldFilter)
            {
                if (fieldFilter.FieldDefinition.ParentJoinForeignKeyDefinition != null)
                {
                    var tableToSearch = fieldFilter.FieldDefinition.ParentJoinForeignKeyDefinition.PrimaryTable;
                    switch (Condition)
                    {
                        case Conditions.Equals:
                        case Conditions.NotEquals:
                            var searchAutoFillValue =
                                Manager.ViewModel.LookupDefinition.TableDefinition.Context.OnAutoFillTextRequest(tableToSearch, SearchValue);
                            if (searchAutoFillValue != null)
                                searchValueText += searchAutoFillValue.Text;
                            break;
                        default:
                            searchValueText += MakeEndSearchValueText(fieldFilter);
                            break;
                    }
                }
                else
                {
                    searchValueText += MakeEndSearchValueText(fieldFilter);
                }
            }
            else
            {
                searchValueText = $"{searchValueText} {SearchValue}";
            }
            SearchValueText = searchValueText;
        }

        private string MakeEndSearchValueText(FieldFilterDefinition fieldFilter)
        {
            var result = string.Empty;
            result = fieldFilter.FieldDefinition.FormatValue(SearchValue);
            return result;
        }

        private string MakeBeginSearchValueText()
        {
            var searchValue = string.Empty;
            switch (Condition)
            {
                case Conditions.Equals:
                    searchValue = "= ";
                    break;
                case Conditions.NotEquals:
                    searchValue = "<> ";
                    break;
                case Conditions.GreaterThan:
                    searchValue = "> ";
                    break;
                case Conditions.GreaterThanEquals:
                    searchValue = ">= ";
                    break;
                case Conditions.LessThan:
                    searchValue = "< ";
                    break;
                case Conditions.LessThanEquals:
                    searchValue = "<= ";
                    break;
                case Conditions.Contains:
                    searchValue = "Contains ";
                    break;
                case Conditions.NotContains:
                    searchValue = "Does Not Contain ";
                    break;
                case Conditions.EqualsNull:
                    searchValue = "Equals NULL";
                    break;
                case Conditions.NotEqualsNull:
                    searchValue = "Does Not Equal NULL";
                    break;
                case Conditions.BeginsWith:
                    searchValue = "Begins With ";
                    break;
                case Conditions.EndsWith:
                    searchValue = "Ends With ";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return searchValue;
        }

        public void LoadFromFilterReturn(AdvancedFilterReturn advancedFilterReturn)
        {
            Condition = advancedFilterReturn.Condition;
            SearchValue = advancedFilterReturn.SearchValue;
            var fieldDefinition = advancedFilterReturn.FieldDefinition;
            if (fieldDefinition == null)
            {
                fieldDefinition = advancedFilterReturn.FormulaParentFieldDefinition;
                PrimaryTable = fieldDefinition.TableDefinition.TableName;
                PrimaryField = fieldDefinition.FieldName;
            }

            if (fieldDefinition != null)
            {
                Table = fieldDefinition.TableDefinition.Description;
            }
            else
            {
                Table = Manager.ViewModel.LookupDefinition.TableDefinition.Description;
            }

            var foundTreeItem =
                Manager.ViewModel.ProcessFoundTreeViewItem(string.Empty, fieldDefinition);

            ProcessIncludeResult includeResult = null;
            if (foundTreeItem != null)
                includeResult = Manager.ViewModel.MakeIncludes(foundTreeItem, "", false);

            if (advancedFilterReturn.Formula.IsNullOrEmpty())
            {
                LoadFromFilterResultField(fieldDefinition, foundTreeItem, includeResult);
            }
            else
            {
                Field = $"{advancedFilterReturn.FormulaDisplayValue} Formula";
                var alias = Manager.ViewModel.LookupDefinition.TableDefinition.TableName;
                if (includeResult?.LookupJoin != null)
                {
                    alias = includeResult.LookupJoin.JoinDefinition.Alias;
                }

                FilterItemDefinition = Manager.ViewModel.LookupDefinition.FilterDefinition.AddUserFilter(advancedFilterReturn.Formula,
                    Condition, SearchValue, alias, advancedFilterReturn.FormulaValueType);
            }

            MakeSearchValueText();
            //Manager.ViewModel.ResetLookup();
        }

        private void LoadFromFilterResultField(FieldDefinition fieldDefinition, TreeViewItem foundTreeItem,
            ProcessIncludeResult includeResult)
        {
            Field = fieldDefinition.Description;

            fieldDefinition = foundTreeItem.FieldDefinition;
            if (foundTreeItem.FieldDefinition.ParentJoinForeignKeyDefinition != null)
            {
                switch (Condition)
                {
                    case Conditions.Equals:
                    case Conditions.NotEquals:
                        FilterItemDefinition =
                            Manager.ViewModel.LookupDefinition.FilterDefinition.AddUserFilter(
                                fieldDefinition, Condition, SearchValue);

                        if (includeResult.LookupJoin != null)
                        {
                            FilterItemDefinition.JoinDefinition = includeResult.LookupJoin.JoinDefinition;
                        }

                        break;
                    default:
                        if (foundTreeItem.FieldDefinition.ParentJoinForeignKeyDefinition != null)
                        {
                            switch (foundTreeItem.FieldDefinition.ParentJoinForeignKeyDefinition.PrimaryTable.LookupDefinition
                                        .InitialSortColumnDefinition.ColumnType)
                            {
                                case LookupColumnTypes.Field:
                                    var initialSortColumnField =
                                        foundTreeItem.FieldDefinition.ParentJoinForeignKeyDefinition.PrimaryTable
                                            .LookupDefinition.InitialSortColumnDefinition as LookupFieldColumnDefinition;
                                    if (initialSortColumnField != null)
                                    {
                                        fieldDefinition = initialSortColumnField.FieldDefinition;
                                        foundTreeItem =
                                            Manager.ViewModel.ProcessFoundTreeViewItem("", fieldDefinition);
                                        includeResult = Manager.ViewModel.MakeIncludes(foundTreeItem, "", false);
                                    }

                                    FilterItemDefinition =
                                        Manager.ViewModel.LookupDefinition.FilterDefinition.AddUserFilter(
                                            fieldDefinition, Condition, SearchValue);

                                    if (includeResult.LookupJoin != null)
                                    {
                                        FilterItemDefinition.JoinDefinition = includeResult.LookupJoin.JoinDefinition;
                                    }

                                    break;

                                case LookupColumnTypes.Formula:
                                    var initialSortColumnFormula =
                                        foundTreeItem.FieldDefinition.ParentJoinForeignKeyDefinition.PrimaryTable
                                            .LookupDefinition
                                            .InitialSortColumnDefinition as LookupFormulaColumnDefinition;
                                    if (foundTreeItem.Parent != null)
                                    {
                                        fieldDefinition = foundTreeItem.Parent.FieldDefinition;
                                    }

                                    FilterItemDefinition = Manager.ViewModel.LookupDefinition.FilterDefinition.AddUserFilter(
                                        initialSortColumnFormula.OriginalFormula, Condition, SearchValue,
                                        includeResult.LookupJoin.JoinDefinition.Alias);
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                        }

                        break;
                }
            }
            else
            {
                FilterItemDefinition = Manager.ViewModel.LookupDefinition.FilterDefinition.AddUserFilter(fieldDefinition, Condition,
                    SearchValue);

                FilterItemDefinition.JoinDefinition = includeResult.LookupJoin?.JoinDefinition;
            }
        }

        public override bool AllowUserDelete => !IsFixed;

        public override void Dispose()
        {
            FilterItemDefinition.TableFilterDefinition.RemoveUserFilter(FilterItemDefinition);
            base.Dispose();
        }
    }
}
