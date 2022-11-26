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

namespace RingSoft.DbMaintenance
{
    public class AdvancedFindFilterRow : DbMaintenanceDataEntryGridRow<AdvancedFindFilter>
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
        public FilterItemDefinition FilterItemDefinition { get; internal set; }
        public FieldDefinition FieldDefinition { get; private set; }
        public string Formula { get; private set; }
        public FieldDataTypes FormulaDataType { get; private set; }
        public bool IsFixed { get; private set; }
        public string PrimaryTable { get; set; }
        public string PrimaryField { get; set; }
        public FieldDefinition ParentFieldDefinition { get; private set; }
        public string FormulaDisplayValue { get; private set; }

        public TextComboBoxControlSetup EndLogicsSetup { get; private set; }

        protected bool ResetLookup { get; set; } = true;

        public FieldDefinition AutoFillField { get; private set; }

        public bool Clearing { get; set; }

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
                    return new AdvancedFindFilterCellProps(this, columnId, SearchValueText, MakeFilterReturn());

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
                    Condition = filterProps.FilterReturn.Condition;
                    SearchValue = filterProps.FilterReturn.SearchValue;

                    //if (FilterItemDefinition is FieldFilterDefinition fieldFilter)
                    //{
                    //    Condition = fieldFilter.Condition = filterProps.FilterReturn.Condition;
                    //    SearchValue = fieldFilter.Value = filterProps.FilterReturn.SearchValue;

                    //    if (filterProps.FilterReturn.FieldDefinition.ParentJoinForeignKeyDefinition != null)
                    //    {
                    //        switch (Condition)
                    //        {
                    //            case Conditions.Equals:
                    //            case Conditions.NotEquals:
                    //            case Conditions.EqualsNull:
                    //            case Conditions.NotEqualsNull:
                    //                fieldFilter.FieldDefinition = AutoFillField;
                    //                break;
                    //            default:
                    //                var field = fieldFilter.FieldDefinition;
                    //                if (field.ParentJoinForeignKeyDefinition != null)
                    //                {
                    //                    field = field.ParentJoinForeignKeyDefinition.FieldJoins[0].PrimaryField;
                    //                }
                    //                if (field.TableDefinition.LookupDefinition.InitialSortColumnDefinition is LookupFieldColumnDefinition fieldColumn)
                    //                {
                    //                    fieldFilter.FieldDefinition = fieldColumn.FieldDefinition;
                    //                }
                    //                else if (fieldFilter.FieldDefinition.TableDefinition.LookupDefinition.InitialSortColumnDefinition is LookupFormulaColumnDefinition formulaColumn)
                    //                {
                    //                    var message = "This operation is not supported due to poor database design.";
                    //                    var caption = "Operation Not Supported";
                    //                    ControlsGlobals.UserInterface.ShowMessageBox(message, caption,
                    //                        RsMessageBoxIcons.Exclamation);
                    //                }
                    //                break;
                    //        }
                    //    }
                    //}
                    if (FieldDefinition != null)
                    {
                        SetCellValueProcessField(filterProps, FilterItemDefinition as FieldFilterDefinition);
                    }
                    else if (FilterItemDefinition is FormulaFilterDefinition formulaFilter)
                    {
                        formulaFilter.Condition = filterProps.FilterReturn.Condition;
                        Condition = filterProps.FilterReturn.Condition;
                        FormulaDataType = formulaFilter.DataType = filterProps.FilterReturn.FormulaValueType;
                        SearchValue = formulaFilter.FilterValue = filterProps.FilterReturn.SearchValue;
                        if (!filterProps.FilterReturn.Formula.IsNullOrEmpty())
                        {
                            var formula = filterProps.FilterReturn.Formula;

                            Formula = formulaFilter.Formula = filterProps.FilterReturn.Formula;
                            PrimaryTable = filterProps.FilterReturn.PrimaryTableName;
                            if (ParentFieldDefinition != null)
                            {
                                Table = ParentFieldDefinition.Description;
                                PrimaryTable = ParentFieldDefinition.TableDefinition.TableName;
                                PrimaryField = ParentFieldDefinition.FieldName;
                            }

                        }

                        FormulaDisplayValue = filterProps.FilterReturn.FormulaDisplayValue;
                        Field = $"{filterProps.FilterReturn.FormulaDisplayValue} Formula";
                    }
                    Manager.ViewModel.RecordDirty = true;
                    MakeSearchValueText();
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

        private void SetCellValueProcessField(AdvancedFindFilterCellProps filterProps, FieldFilterDefinition filter)
        {
            filter.Value = SearchValue;
            if (FieldDefinition.ParentJoinForeignKeyDefinition != null)
            {
                FilterItemDefinition filterDefinition = null;
                switch (Condition)
                {
                    case Conditions.Equals:
                    case Conditions.NotEquals:
                    case Conditions.EqualsNull:
                    case Conditions.NotEqualsNull:
                        filterDefinition =
                            FilterItemDefinition.TableFilterDefinition.CreateFieldFilter(
                                filterProps.FilterReturn.FieldDefinition,
                                Condition, SearchValue);
                        break;
                    default:
                        var lookupColumn = FieldDefinition.ParentJoinForeignKeyDefinition
                            .PrimaryTable.LookupDefinition.InitialSortColumnDefinition;

                        if (lookupColumn is LookupFieldColumnDefinition lookupFieldColumn)
                        {
                            filterDefinition =
                                FilterItemDefinition.TableFilterDefinition.CreateFieldFilter(lookupFieldColumn.FieldDefinition,
                                    Condition, SearchValue);
                        }
                        else if (lookupColumn is LookupFormulaColumnDefinition lookupFormulaColumn)
                        {
                            filterDefinition = FilterItemDefinition.TableFilterDefinition.CreateFormulaFilter(
                                lookupFormulaColumn.OriginalFormula, lookupFormulaColumn.DataType, Condition, SearchValue, FilterItemDefinition.JoinDefinition.Alias);
                        }
                        break;
                }

                filterDefinition.JoinDefinition = FilterItemDefinition.JoinDefinition;
                FilterItemDefinition.TableFilterDefinition.ReplaceUserFilter(FilterItemDefinition,
                    filterDefinition);
                FilterItemDefinition = filterDefinition;
                FilterItemDefinition.LeftParenthesesCount = LeftParenthesesCount;
                FilterItemDefinition.RightParenthesesCount = RightParenthesesCount;
                if (EndLogics.HasValue)
                {
                    FilterItemDefinition.EndLogic = EndLogics.Value;
                }
                MakeSearchValueText();
            }
        }

        public override void LoadFromEntity(AdvancedFindFilter entity)
        {
            var lookupFilterResult = Manager.ViewModel.LookupDefinition.LoadFromAdvFindFilter(entity);
            if (lookupFilterResult != null && lookupFilterResult.FilterItemDefinition != null)
            {
                Table = lookupFilterResult.FilterItemDefinition.TableDescription;
            }
            if (lookupFilterResult.FieldDefinition != null &&
                lookupFilterResult.FilterItemDefinition is FieldFilterDefinition)
            {
                FieldDefinition = lookupFilterResult.FieldDefinition;
                AutoFillField = FieldDefinition;
            }
            LoadFromFilterDefinition(lookupFilterResult.FilterItemDefinition, false, entity.AdvancedFindId);
            if (lookupFilterResult.FieldDefinition != null)
            {
                //Table = lookupFilterResult.FieldDefinition.TableDefinition.Description;
                if (lookupFilterResult.FieldDefinition != null &&
                    lookupFilterResult.FieldDefinition.ParentJoinForeignKeyDefinition != null &&
                    lookupFilterResult.FilterItemDefinition is FormulaFilterDefinition)
                {
                    FieldDefinition = lookupFilterResult.FieldDefinition;
                    AutoFillField = FieldDefinition.ParentJoinForeignKeyDefinition.FieldJoins[0].PrimaryField;
                }
            }

            //if (FieldDefinition != null)
            {
                //FieldDefinition = fieldFilter.FieldDefinition;
                PrimaryTable = entity.PrimaryTableName;
                PrimaryField = entity.PrimaryFieldName;
                MakeParentField();

                //if (ParentFieldDefinition != null)
                //{
                //    Table = ParentFieldDefinition.Description;
                //}
            }
            Formula = entity.Formula;
            FormulaDisplayValue = entity.FormulaDisplayValue;
            if (FieldDefinition == null && entity.SearchForAdvancedFindId == null)
            {
                Field = $"{FormulaDisplayValue} Formula";
            }
            else if (entity.SearchForAdvancedFindId == null)
            {
                Field = FieldDefinition.Description;
            }


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

        private AdvancedFilterReturn MakeFilterReturn()
        {
            var filterReturn = new AdvancedFilterReturn();
            filterReturn.Condition = Condition;
            filterReturn.SearchValue = SearchValue;
            filterReturn.PrimaryFieldDefinition = ParentFieldDefinition;

            if (FieldDefinition == null)
            {
                filterReturn.PrimaryTableName = Table;
            }
            else
            {
                
            }

            filterReturn.FieldDefinition = FieldDefinition;


            filterReturn.Condition = Condition;
            filterReturn.SearchValue = SearchValue;

            filterReturn.Formula = Formula;
            filterReturn.FormulaValueType = FormulaDataType;
            filterReturn.FormulaDisplayValue = FormulaDisplayValue;
            filterReturn.LookupDefinition = Manager.ViewModel.LookupDefinition;

            return filterReturn;
        }

        public override bool ValidateRow()
        {
            return true;
        }

        public override void SaveToEntity(AdvancedFindFilter entity, int rowIndex)
        {
            
            entity.AdvancedFindId = Manager.ViewModel.AdvancedFindId;
            entity.FilterId = rowIndex + 1;
            entity.LeftParentheses = (byte)LeftParenthesesCount;
            //if (FilterItemDefinition is FieldFilterDefinition fieldFilterDefinition)
            //{
            //    entity.TableName = fieldFilterDefinition.FieldDefinition.TableDefinition.EntityName;
            //    entity.FieldName = fieldFilterDefinition.FieldDefinition.PropertyName;
            //}

            if (FieldDefinition != null)
            {
                entity.TableName = FieldDefinition.TableDefinition.EntityName;
                entity.FieldName = FieldDefinition.PropertyName;
            }

            entity.Operand = (byte) Condition;
            entity.SearchForValue = SearchValue;
            entity.RightParentheses = (byte)RightParenthesesCount;

            //if (ParentFieldDefinition != null)
            {
                entity.PrimaryTableName = PrimaryTable;
                entity.PrimaryFieldName = PrimaryField;

            }
            if (EndLogics != null)
                entity.EndLogic = (byte) EndLogics;
            entity.Formula = Formula;
            entity.FormulaDisplayValue = FormulaDisplayValue;
            entity.FormulaDataType = (byte)FormulaDataType;
        }

        public void LoadFromFilterDefinition(FilterItemDefinition filter, bool isFixed, int rowIndex)
        {
            FilterItemDefinition = filter;
            FilterItemDefinition newFilter = null;
            LeftParenthesesCount = filter.LeftParenthesesCount;
            RightParenthesesCount = filter.RightParenthesesCount;
            EndLogics = filter.EndLogic;
            if (isFixed && rowIndex == 0)
            {
                LeftParenthesesCount++;
            }
            if (filter is FieldFilterDefinition fieldFilterDefinition)
            {
                if (fieldFilterDefinition.FieldDefinition.ParentJoinForeignKeyDefinition != null)
                {
                    FieldDefinition = AutoFillField = fieldFilterDefinition.FieldDefinition
                        .ParentJoinForeignKeyDefinition.FieldJoins[0]
                        .PrimaryField;
                }
                
                //Table = fieldFilterDefinition.FieldDefinition.TableDefinition.Description;
                Field = fieldFilterDefinition.FieldDefinition.Description;
                Condition = fieldFilterDefinition.Condition;
                SearchValue = fieldFilterDefinition.Value;

                if (isFixed)
                {
                    switch (fieldFilterDefinition.FieldDefinition.FieldDataType)
                    {
                        case FieldDataTypes.String:
                            if (fieldFilterDefinition.FieldDefinition is StringFieldDefinition stringField)
                                newFilter = Manager.ViewModel.LookupDefinition.FilterDefinition.AddFixedFilter(stringField,
                                    fieldFilterDefinition.Condition,
                                    fieldFilterDefinition.Value);
                            break;
                        case FieldDataTypes.Integer:
                            if (fieldFilterDefinition.FieldDefinition is IntegerFieldDefinition integerField)
                                newFilter = Manager.ViewModel.LookupDefinition.FilterDefinition.AddFixedFilter(integerField,
                                    fieldFilterDefinition.Condition,
                                    fieldFilterDefinition.Value.ToInt());
                            break;
                        case FieldDataTypes.Decimal:
                            if (fieldFilterDefinition.FieldDefinition is DecimalFieldDefinition decimalField)
                                newFilter = Manager.ViewModel.LookupDefinition.FilterDefinition.AddFixedFilter(decimalField,
                                    fieldFilterDefinition.Condition,
                                    fieldFilterDefinition.Value.ToDecimal());
                            break;
                        case FieldDataTypes.DateTime:
                            if (fieldFilterDefinition.FieldDefinition is DateFieldDefinition dateField)
                                newFilter = Manager.ViewModel.LookupDefinition.FilterDefinition.AddFixedFilter(dateField,
                                    fieldFilterDefinition.Condition,
                                    DateTime.Parse(fieldFilterDefinition.Value));
                            break;
                        case FieldDataTypes.Bool:
                            if (fieldFilterDefinition.FieldDefinition is BoolFieldDefinition boolField)
                                newFilter = Manager.ViewModel.LookupDefinition.FilterDefinition.AddFixedFilter(boolField,
                                    fieldFilterDefinition.Condition,
                                    fieldFilterDefinition.Value.ToBool());

                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

            }
            else if (filter is FormulaFilterDefinition formulaFilter)
            {
                //Table = filter.TableFilterDefinition.TableDefinition.Description;
                Field = $"{formulaFilter.Description} Formula";
                Formula = formulaFilter.Formula;
                SearchValue = formulaFilter.FilterValue;
                Condition = formulaFilter.Condition.GetValueOrDefault();
                FormulaDataType = formulaFilter.DataType;
                if (isFixed)
                {

                    newFilter = Manager.ViewModel.LookupDefinition.FilterDefinition.AddFixedFilter(formulaFilter.Description,
                        Condition,
                        SearchValue, Formula, FormulaDataType);
                }
            }
            IsFixed = isFixed;
            if (isFixed)
            {
                AllowSave = false;
                if (newFilter != null)
                {
                    newFilter.RightParenthesesCount = filter.RightParenthesesCount;

                    newFilter.LeftParenthesesCount = (byte)filter.LeftParenthesesCount;

                    newFilter.EndLogic = (EndLogics)filter.EndLogic;
                }


            }
            MakeSearchValueText();
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
            if (searchValue.IsNullOrEmpty())
            {
                searchValue = SearchValue;
            }
            var searchValueText = MakeBeginSearchValueText();
            if (FieldDefinition != null)
            {
                TableDefinitionBase tableToSearch = null;
                var makeSearchValueText = true;
                if (AutoFillField != null)
                {
                    tableToSearch = AutoFillField.TableDefinition;
                    if (AutoFillField.ParentJoinForeignKeyDefinition != null)
                    {
                        tableToSearch = AutoFillField.ParentJoinForeignKeyDefinition.PrimaryTable;
                    }
                }
                else
                {
                    tableToSearch = FieldDefinition.TableDefinition;
                }
                switch (Condition)
                {
                    case Conditions.Equals:
                    case Conditions.NotEquals:
                        if (AutoFillField != null)
                        {
                            var searchAutoFillValue =
                                Manager.ViewModel.LookupDefinition.TableDefinition.Context.OnAutoFillTextRequest(tableToSearch, searchValue);
                            if (searchAutoFillValue != null)
                            {
                                searchValueText += searchAutoFillValue.Text;
                                makeSearchValueText = false;
                            }
                        }

                        break;
                    default:
                        searchValueText += MakeEndSearchValueText(searchValue);

                        makeSearchValueText = false;
                        break;
                }
                if (makeSearchValueText)
                {
                    searchValueText += MakeEndSearchValueText(searchValue);
                }
            }
            else
            {
                searchValueText = $"{searchValueText} {searchValue}";
            }
            SearchValueText = searchValueText;
        }

        private string MakeEndSearchValueText(string searchValue)
        {
            var result = string.Empty;
            if (FieldDefinition is IntegerFieldDefinition integerField)
            {
                if (integerField.EnumTranslation != null)
                {
                    var item = integerField.EnumTranslation.TypeTranslations.FirstOrDefault(p =>
                        p.NumericValue == searchValue.ToInt());
                    result = item.TextValue;
                    return result;
                }
            }
            else if (FieldDefinition is BoolFieldDefinition boolField)
            {
                var boolItem = searchValue.ToBool();
                if (boolItem)
                {
                    result = "True";
                }
                else
                {
                    result = "False";
                }

                return result;
            }

            switch (Condition)
            {
                case Conditions.EqualsNull:
                case Conditions.NotEqualsNull:
                    break;
                default:
                    result = FieldDefinition.FormatValue(searchValue);
                    break;
            }
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
            //if (fieldDefinition == null)
            //{
            //    fieldDefinition = advancedFilterReturn.PrimaryFieldDefinition;

            //    if (fieldDefinition == null)
            //    {
            //        Table = Manager.ViewModel.LookupDefinition.TableDefinition.Description;
            //    }
            //}
            //else
            //{
            //    Table = fieldDefinition.TableDefinition.Description;
            //}
            Table = advancedFilterReturn.TableDescription;
            ParentFieldDefinition = advancedFilterReturn.PrimaryFieldDefinition;
            //if (fieldDefinition != null)
            //{
            //    PrimaryTable = fieldDefinition.TableDefinition.TableName;
            //    PrimaryField = fieldDefinition.FieldName;
            //}
            if (ParentFieldDefinition != null)
            {
                //Table = ParentFieldDefinition.Description;
                PrimaryTable = ParentFieldDefinition.TableDefinition.TableName;
                PrimaryField = ParentFieldDefinition.FieldName;
            }

            FieldDefinition = fieldDefinition;
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
                //PrimaryTable = Table = advancedFilterReturn.PrimaryTableName;
                Field = $"{advancedFilterReturn.FormulaDisplayValue} Formula";
                var alias = Manager.ViewModel.LookupDefinition.TableDefinition.TableName;
                if (includeResult?.LookupJoin != null)
                {
                    alias = includeResult.LookupJoin.JoinDefinition.Alias;
                }

                Formula = advancedFilterReturn.Formula;
                FormulaDisplayValue = advancedFilterReturn.FormulaDisplayValue;
                FilterItemDefinition = Manager.ViewModel.LookupDefinition.FilterDefinition.AddUserFilter(Formula,
                    Condition, SearchValue, alias, advancedFilterReturn.FormulaValueType);
            }

            MakeSearchValueText();
            Manager?.Grid?.RefreshGridView();
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
                        DbLookup.ModelDefinition.FieldDefinitions.FieldDefinition fieldToSearch = fieldDefinition;
                        if (fieldDefinition.ParentJoinForeignKeyDefinition != null)
                        {
                            fieldToSearch = fieldDefinition.ParentJoinForeignKeyDefinition.FieldJoins[0].PrimaryField;
                        }

                        FilterItemDefinition =
                            Manager.ViewModel.LookupDefinition.FilterDefinition.AddUserFilter(
                                fieldToSearch, Condition, SearchValue);

                        if (includeResult.LookupJoin != null)
                        {
                            FilterItemDefinition.JoinDefinition = includeResult.LookupJoin.JoinDefinition;
                        }
                        if (fieldDefinition.ParentJoinForeignKeyDefinition != null)
                        {
                            AutoFillField = fieldDefinition.ParentJoinForeignKeyDefinition.FieldJoins[0].PrimaryField;
                            _searchAutoFillField = fieldDefinition;
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

                                    switch (Condition)
                                    {
                                        case Conditions.EqualsNull:
                                        case Conditions.NotEqualsNull:
                                            if (fieldDefinition.ParentJoinForeignKeyDefinition != null && fieldDefinition.TableDefinition != Manager.ViewModel.LookupDefinition.TableDefinition)
                                            {
                                                fieldDefinition = fieldDefinition.ParentJoinForeignKeyDefinition
                                                    .ForeignKeyFieldJoins[0].PrimaryField;
                                            }
                                            FilterItemDefinition =
                                                Manager.ViewModel.LookupDefinition.FilterDefinition.AddUserFilter(
                                                    fieldDefinition, Condition, SearchValue);
                                            if (includeResult.LookupJoin != null &&
                                                fieldDefinition.TableDefinition != Manager.ViewModel.LookupDefinition.TableDefinition)
                                            {
                                                FilterItemDefinition.JoinDefinition = includeResult.LookupJoin.JoinDefinition;
                                            }

                                            break;
                                        default:
                                            FilterItemDefinition = 
                                                Manager.ViewModel.LookupDefinition.FilterDefinition.AddUserFilter(
                                                initialSortColumnFormula.OriginalFormula, Condition, SearchValue,
                                                includeResult.LookupJoin.JoinDefinition.Alias);
                                            break;
                                    }
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
