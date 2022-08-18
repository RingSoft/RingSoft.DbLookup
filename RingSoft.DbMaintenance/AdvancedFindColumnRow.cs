using System;
using System.Linq;
using System.Security.Permissions;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;

namespace RingSoft.DbMaintenance
{
    public class AdvancedFindColumnRow : DbMaintenanceDataEntryGridRow<AdvancedFindColumn>
    {
        public const int TableColumnId = 1;
        public const int FieldColumnId = 2;
        public const int NameColumnId = 3;
        public const int PercentColumnId = 4;

        public enum AdvancedFindColumnColumns
        {
            Table = TableColumnId,
            Field = FieldColumnId,
            Name = NameColumnId,
            PercentWidth = PercentColumnId
        }

        public string Table { get; set; }
        public string Field { get; set; }
        public string Name { get; set; }
        public decimal PercentWidth { get; set; }
        public LookupFieldColumnDefinition LookupFieldColumnDefinition { get; set; }
        public LookupFormulaColumnDefinition LookupFormulaColumnDefinition { get; set; }

        public new AdvancedFindColumnsManager Manager { get; set; }

        public AdvancedFindColumnRow(AdvancedFindColumnsManager manager) : base(manager)
        {
            Manager = manager;
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (AdvancedFindColumnColumns) columnId;
            switch (column)
            {
                case AdvancedFindColumnColumns.Table:
                    return new DataEntryGridTextCellProps(this, columnId, Table);
                case AdvancedFindColumnColumns.Field:
                    return new DataEntryGridTextCellProps(this, columnId, Field);
                case AdvancedFindColumnColumns.Name:
                    return new DataEntryGridTextCellProps(this, columnId, Name);
                case AdvancedFindColumnColumns.PercentWidth:
                    return new DataEntryGridDecimalCellProps(this, columnId,
                        new DecimalEditControlSetup
                        {
                            AllowNullValue = false,
                            FormatType = DecimalEditFormatTypes.Percent,
                            MaximumValue = 100,
                            MinimumValue = 0

                        }, PercentWidth);

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override DataEntryGridCellStyle GetCellStyle(int columnId)
        {
            var column = (AdvancedFindColumnColumns) columnId;

            switch (column)
            {
                case AdvancedFindColumnColumns.Table:
                case AdvancedFindColumnColumns.Field:
                    return new DataEntryGridCellStyle
                    {
                        State = DataEntryGridCellStates.Disabled
                    };
                case AdvancedFindColumnColumns.Name:
                case AdvancedFindColumnColumns.PercentWidth:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return base.GetCellStyle(columnId);
        }

        public override void SetCellValue(DataEntryGridEditingCellProps value)
        {
            var column = (AdvancedFindColumnColumns) value.ColumnId;
            switch (column)
            {
                case AdvancedFindColumnColumns.Name:
                    var props = (DataEntryGridTextCellProps) value;
                    Name = props.Text;
                    break;
                case AdvancedFindColumnColumns.PercentWidth:
                    var decimalProps = (DataEntryGridDecimalCellProps) value;
                    PercentWidth = (decimal) decimalProps.Value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            base.SetCellValue(value);
        }

        public override void LoadFromEntity(AdvancedFindColumn entity)
        {
            var tableDefinition =
                Manager.ViewModel.TableDefinition.Context.TableDefinitions.FirstOrDefault(p =>
                    p.EntityName == entity.TableName);
            Table = tableDefinition.Description;
            Field = tableDefinition.FieldDefinitions.FirstOrDefault(p => p.FieldName == entity.FieldName).Description;
            Name = entity.Caption;
            PercentWidth = entity.PercentWidth;
        }

        public override bool ValidateRow()
        {
            return true;
        }

        public override void SaveToEntity(AdvancedFindColumn entity, int rowIndex)
        {
            entity.AdvancedFindId = Manager.ViewModel.AdvancedFindId;
            entity.ColumnId = rowIndex;
            var tableDefinition =
                Manager.ViewModel.TableDefinition.Context.TableDefinitions.FirstOrDefault(p => p.Description == Table);
            entity.TableName = tableDefinition.EntityName;
            entity.FieldName = tableDefinition.FieldDefinitions.FirstOrDefault(p => p.Description == Field).FieldName;
            entity.Caption = Name;
            entity.PercentWidth = PercentWidth;
            if (LookupFormulaColumnDefinition != null)
            {
                entity.Formula = LookupFormulaColumnDefinition.Formula;
            }
        }

        public void LoadFromColumnDefinition(LookupColumnDefinitionBase column)
        {
            LookupFieldColumnDefinition = column as LookupFieldColumnDefinition;
            LookupFormulaColumnDefinition = column as LookupFormulaColumnDefinition;

            if (column.ParentObject == null)
            {
                Table = Manager.ViewModel.LookupDefinition.TableDefinition.Description;
            }
            else
            {
                var lookupJoin = column.ParentObject as LookupJoin;
                Table = lookupJoin.JoinDefinition.ForeignKeyDefinition.PrimaryTable.Description;
            }

            Name = column.Caption;
            PercentWidth = (decimal) column.PercentWidth/100;
            if (LookupFormulaColumnDefinition != null)
            {
                Field = "<Formula>";
            }

            if (LookupFieldColumnDefinition != null)
            {
                Field = LookupFieldColumnDefinition.FieldDefinition.Description;
            }
        }
    }
}
