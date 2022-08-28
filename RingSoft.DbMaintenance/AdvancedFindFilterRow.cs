﻿using System;
using System.Linq;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DbLookup.TableProcessing;

namespace RingSoft.DbMaintenance
{
    public class AdvancedFindFilterRow : DbMaintenanceDataEntryGridRow<AdvancedFindFilter>
    {
        public AdvancedFindFiltersManager Manager { get; set; }

        public byte LeftParenthesesCount { get; private set; }
        public string Table { get; private set; }
        public string Field { get; private set; }
        public string SearchValueText { get; private set; }
        public byte RightParenthesesCount { get; private set; }
        public EndLogics EndLogics { get; private set; }

        public Conditions Condition { get; private set; }
        public string SearchValue { get; private set; }
        public FilterItemDefinition FilterItemDefinition { get; private set; }
        public string Formula { get; private set; }
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
                    return new DataEntryGridTextComboBoxCellProps(this, columnId, EndLogicsSetup,
                        EndLogicsSetup.GetItem((int) EndLogics)) {ControlMode = true};

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
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            base.SetCellValue(value);
        }

        public override void LoadFromEntity(AdvancedFindFilter entity)
        {
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
            }

            Condition = (Conditions) entity.Operand;
            SearchValue = entity.SearchForValue;
            RightParenthesesCount = entity.RightParentheses;
            EndLogics = (EndLogics) entity.EndLogic;
            Formula = entity.Formula;
            PrimaryTable = entity.PrimaryTableName;
            PrimaryField = entity.PrimaryFieldName;
        }

        public override bool ValidateRow()
        {
            return true;
        }

        public override void SaveToEntity(AdvancedFindFilter entity, int rowIndex)
        {
            entity.AdvancedFindId = Manager.ViewModel.AdvancedFindId;
            entity.FilterId = rowIndex;
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
        }

        public void LoadFromFilterDefinition(FilterItemDefinition filter, bool isFixed)
        {
            LeftParenthesesCount = (byte) filter.LeftParenthesesCount;
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
                    case FieldDataTypes.DateTime:
                    case FieldDataTypes.Bool:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

            }

            RightParenthesesCount = (byte) filter.RightParenthesesCount;
            EndLogics = filter.EndLogic;
            IsFixed = isFixed;
            if (isFixed)
            {
                AllowSave = false;
            }
        }
    }
}
