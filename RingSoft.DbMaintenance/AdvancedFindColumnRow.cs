using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Permissions;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;

namespace RingSoft.DbMaintenance
{
    public abstract class AdvancedFindColumnRow : DbMaintenanceDataEntryGridRow<AdvancedFindColumn>, IDisposable
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
        public double PercentWidth { get; set; }

        public LookupFieldColumnDefinition LookupFieldColumnDefinition { get; set; }
        public LookupFormulaColumnDefinition LookupFormulaColumnDefinition { get; set; }
        public LookupColumnDefinitionBase LookupColumnDefinition { get; set; }
        public TableDefinitionBase TableDefinition { get; private set; }
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
                    return new AdvancedFindMemoCellProps(this, columnId, Name){FormMode = AdvancedFindMemoCellProps.MemoFormMode.Caption, ReadOnlyMode = false};
                case AdvancedFindColumnColumns.PercentWidth:
                    return new DataEntryGridDecimalCellProps(this, columnId,
                        new DecimalEditControlSetup
                        {
                            AllowNullValue = false,
                            FormatType = DecimalEditFormatTypes.Percent,
                            MaximumValue = 100,
                            MinimumValue = 0},
                        PercentWidth.ToString().ToDecimal());

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
                    return new DataEntryGridCellStyle
                    {
                        State = DataEntryGridCellStates.Disabled
                    };
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
                    var props = (AdvancedFindMemoCellProps) value;
                    Name = props.Text;
                    LookupColumnDefinition.UpdateCaption(Name);
                    Manager.ViewModel.ResetLookup();
                    break;
                case AdvancedFindColumnColumns.PercentWidth:
                    var decimalProps = (DataEntryGridDecimalCellProps) value;
                    if (decimalProps.Value == null)
                    {
                        PercentWidth = 0;
                    }
                    else
                    {
                        var percentWidth = (decimal) decimalProps.Value;
                        PercentWidth = Decimal.ToDouble(percentWidth);
                    }
                    LookupColumnDefinition.UpdatePercentWidth(PercentWidth * 100);
                    Manager.ViewModel.ResetLookup();
                    break;
                case AdvancedFindColumnColumns.Field:
                    var fieldProps = (AdvancedFindColumnFormulaCellProps)value;
                    LookupFormulaColumnDefinition = fieldProps.LookupFormulaColumn;
                    Manager.ViewModel.ResetLookup();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            base.SetCellValue(value);
        }

        public override void LoadFromEntity(AdvancedFindColumn entity)
        {
            LookupColumnDefinition = Manager.ViewModel.LookupDefinition.LoadFromAdvFindColumnEntity(entity);
            LoadFromColumnDefinition(LookupColumnDefinition);
            //Name = entity.Caption;
            //var percentWidth = (double)entity.PercentWidth * 100;
            //PercentWidth = percentWidth / 100;

            //Table = LookupColumnDefinition.TableDescription;
            //Field = LookupColumnDefinition.FieldDescription;

            //if (LookupColumnDefinition != null)
            //{
            //    if (LookupColumnDefinition is LookupFormulaColumnDefinition)
            //    {
            //        LookupFormulaColumnDefinition = LookupColumnDefinition as LookupFormulaColumnDefinition;
            //        SetFormulaTableField();
            //    }
            //    else
            //    {
            //        LookupFieldColumnDefinition = LookupColumnDefinition as LookupFieldColumnDefinition;
            //        TableDefinition = LookupFieldColumnDefinition.FieldDefinition.TableDefinition;

            //    }
            //}
        }

        public override bool ValidateRow()
        {
            return true;
        }

        public override void SaveToEntity(AdvancedFindColumn entity, int rowIndex)
        {
            entity.AdvancedFindId = Manager.ViewModel.AdvancedFindId;
            entity.ColumnId = rowIndex + 1;
            LookupColumnDefinition.SaveToEntity(entity);
            //var tableDefinition = TableDefinition;
            //entity.TableName = tableDefinition.EntityName;
            //entity.Path = LookupColumnDefinition.Path;

            //if (LookupColumnDefinition.ParentObject != null)
            //{
            //    var lookupJoin = LookupColumnDefinition.ParentObject as LookupJoin;
            //    var primaryTableDefinition = lookupJoin.JoinDefinition.ForeignKeyDefinition.ForeignTable;
            //    entity.PrimaryTableName = primaryTableDefinition.EntityName;
            //    var primaryField = lookupJoin.JoinDefinition.ForeignKeyDefinition.FieldJoins[0].ForeignField;
            //    if (primaryField != null)
            //    {
            //        entity.PrimaryFieldName = primaryField.FieldName;
            //    }
            //}
            //entity.Caption = Name;
            //entity.PercentWidth = PercentWidth;
            //if (LookupFormulaColumnDefinition != null)
            //{
            //    entity.Formula = LookupFormulaColumnDefinition.OriginalFormula;
            //    entity.FieldDataType = (byte) LookupFormulaColumnDefinition.DataType;
            //    entity.DecimalFormatType = (byte) LookupFormulaColumnDefinition.DecimalFieldType;
            //}
            //else
            //{
            //    if (LookupFieldColumnDefinition != null)
            //    {
            //        entity.FieldName = LookupFieldColumnDefinition.FieldDefinition.FieldName;
            //    }
            //}
        }

        public void LoadFromColumnDefinition(LookupColumnDefinitionBase column)
        {
            LookupColumnDefinition = column;

            //if (column.ParentObject == null)
            //{
            //    Table = Manager.ViewModel.LookupDefinition.TableDefinition.Description;
            //}
            //else
            //{
            //    var lookupJoin = column.ParentObject as LookupJoin;
            //    Table = lookupJoin.JoinDefinition.ForeignKeyDefinition.PrimaryTable.Description;
            //}
            Table = column.TableDescription;
            if (Table.IsNullOrEmpty())
            {
                Table = Manager.ViewModel.LookupDefinition.TableDefinition.Description;
            }
            Field = column.FieldDescription;

            Name = column.Caption;
            PercentWidth = column.PercentWidth / 100;
            //if (LookupFormulaColumnDefinition != null)
            //{
            //    SetFormulaTableField();
            //}

            //if (LookupFieldColumnDefinition != null)
            //{
            //    //Field = LookupFieldColumnDefinition.FieldDefinition.Description;
            //    TableDefinition = LookupFieldColumnDefinition.FieldDefinition.TableDefinition;
            //}
        }

        private void SetFormulaTableField()
        {
            TableDefinition = LookupFormulaColumnDefinition.PrimaryTable;
            if (TableDefinition == null)
            {
                TableDefinition = Manager.ViewModel.LookupDefinition.TableDefinition;
            }

            if (!Table.IsNullOrEmpty())
            {
                return;
            }

            var foundItem = Manager.ViewModel.ProcessFoundTreeViewItem(LookupFormulaColumnDefinition.Formula,
                LookupFormulaColumnDefinition.ParentField);
            if (foundItem == null)
            {
                Table = TableDefinition.Description;
            }
            else
            {
                if (foundItem.Name == "<Formula>")
                {
                    if (foundItem.Parent == null)
                    {
                        Table = TableDefinition.Description;
                    }
                    else
                    {
                        Table = foundItem.Parent.Name;
                    }
                }
                else
                {
                    if (foundItem.Parent != null && LookupFormulaColumnDefinition != null)
                    {
                        Table = foundItem.Parent.Name;
                    }
                    else
                    {
                        Table = foundItem.Name;
                    }
                }
            }

            Field = "<Formula>";
        }

        public void UpdatePercentWidth()
        {
            PercentWidth = LookupColumnDefinition.PercentWidth / 100;
            if (Manager.Grid != null)
                Manager.Grid.UpdateRow(this);
        }

        public override void Dispose()
        {
            Manager.ViewModel.LookupDefinition.DeleteVisibleColumn(LookupColumnDefinition);
            if (!Manager.ViewModel.LookupDefinition.VisibleColumns.Any())
            {
                Manager.ViewModel.CreateLookupDefinition();
            }
            Manager.ViewModel.ResetLookup();
            base.Dispose();
        }
    }
}