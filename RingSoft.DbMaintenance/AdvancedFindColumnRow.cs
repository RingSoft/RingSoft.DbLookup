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
        public double PercentWidth { get; set; }
        public LookupFieldColumnDefinition LookupFieldColumnDefinition { get; set; }
        public LookupFormulaColumnDefinition LookupFormulaColumnDefinition { get; set; }
        public LookupColumnDefinitionBase LookupColumnDefinition { get; set; }
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
                    if (decimalProps.Value == null)
                    {
                        PercentWidth = 0;
                    }
                    else
                    {
                        var percentWidth = (decimal) decimalProps.Value;
                        PercentWidth = Decimal.ToDouble(percentWidth);

                    }
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
            FieldDefinition fieldDefinition = null;
            if (!entity.FieldName.IsNullOrEmpty())
            {
                fieldDefinition =
                    tableDefinition.FieldDefinitions.FirstOrDefault(p => p.FieldName == entity.FieldName);
                Field = fieldDefinition.Description;
            }

            TableDefinitionBase primaryTable = null;
            FieldDefinition primaryField = null;
            if (!entity.PrimaryTableName.IsNullOrEmpty() && !entity.PrimaryFieldName.IsNullOrEmpty())
            {
                primaryTable =
                    Manager.ViewModel.LookupDefinition.TableDefinition.Context.TableDefinitions.FirstOrDefault(p =>
                        p.EntityName == entity.PrimaryTableName);

                primaryField =
                    primaryTable.FieldDefinitions.FirstOrDefault(p => p.FieldName == entity.PrimaryFieldName);

            }

            Name = entity.Caption;
            var percentWidth = (double) entity.PercentWidth * 100;

            PercentWidth = percentWidth / 100;

            if (fieldDefinition == null)
            {
                tableDefinition = primaryTable;
                fieldDefinition = primaryField;
            }

            var items = Manager.ViewModel.TreeRoot;
            var foundTreeViewItem = FindFieldInTree(items, fieldDefinition);

            LookupColumnDefinition = Manager.ViewModel.MakeIncludes(foundTreeViewItem, Name);

            if (!entity.Formula.IsNullOrEmpty())
            {
               LookupFormulaColumnDefinition = LookupColumnDefinition as LookupFormulaColumnDefinition;
                Field = "<Formula>";
            }
            else
            {
                LookupFieldColumnDefinition = LookupColumnDefinition as LookupFieldColumnDefinition;
            }
        }

        public TreeViewItem FindFieldInTree(ObservableCollection<TreeViewItem> items, FieldDefinition fieldDefinition)
        {
            TreeViewItem foundTreeViewItem = null;
            foreach (var treeViewItem in items)
            {
                if (treeViewItem.FieldDefinition == fieldDefinition)
                {
                    return treeViewItem;
                }
                else
                {
                    foundTreeViewItem = FindFieldInTree(treeViewItem.Items, fieldDefinition);
                    if (foundTreeViewItem != null)
                    {
                        return foundTreeViewItem;
                    }

                }
            }
            return foundTreeViewItem;
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

            if (LookupColumnDefinition.ParentObject != null)
            {
                var lookupJoin = LookupColumnDefinition.ParentObject as LookupJoin;
                var primaryTableDefinition = lookupJoin.JoinDefinition.ForeignKeyDefinition.ForeignTable;
                entity.PrimaryTableName = primaryTableDefinition.EntityName;
                var primaryField = lookupJoin.JoinDefinition.ForeignKeyDefinition.FieldJoins[0].ForeignField;
                if (primaryField != null)
                {
                    entity.PrimaryFieldName = primaryField.FieldName;
                }
            }

            entity.Caption = Name;
            entity.PercentWidth = PercentWidth;
            if (LookupFormulaColumnDefinition != null)
            {
                entity.Formula = LookupFormulaColumnDefinition.Formula;
                entity.FieldDataType = (byte) LookupFormulaColumnDefinition.DataType;
            }
            else
            {
                entity.FieldName = tableDefinition.FieldDefinitions.FirstOrDefault(p => p.Description == Field)
                    .FieldName;
            }
        }

        public void LoadFromColumnDefinition(LookupColumnDefinitionBase column)
        {
            LookupColumnDefinition = column;
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
            PercentWidth = column.PercentWidth / 100;
            if (LookupFormulaColumnDefinition != null)
            {
                Field = "<Formula>";
            }

            if (LookupFieldColumnDefinition != null)
            {
                Field = LookupFieldColumnDefinition.FieldDefinition.Description;
            }
        }

        public void UpdatePercentWidth()
        {
            PercentWidth = LookupColumnDefinition.PercentWidth / 100;
            if (Manager.Grid != null)
                Manager.Grid.UpdateRow(this);
        }
    }
}