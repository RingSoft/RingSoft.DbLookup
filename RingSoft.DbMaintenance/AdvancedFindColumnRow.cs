// ***********************************************************************
// Assembly         : RingSoft.DbMaintenance
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 11-20-2023
// ***********************************************************************
// <copyright file="AdvancedFindColumnRow.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
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
    /// <summary>
    /// Class AdvancedFindColumnRow.
    /// Implements the <see cref="RingSoft.DbMaintenance.DbMaintenanceDataEntryGridRow{RingSoft.DbLookup.AdvancedFind.AdvancedFindColumn}" />
    /// Implements the <see cref="IDisposable" />
    /// </summary>
    /// <seealso cref="RingSoft.DbMaintenance.DbMaintenanceDataEntryGridRow{RingSoft.DbLookup.AdvancedFind.AdvancedFindColumn}" />
    /// <seealso cref="IDisposable" />
    public abstract class AdvancedFindColumnRow : DbMaintenanceDataEntryGridRow<AdvancedFindColumn>, IDisposable
    {
        /// <summary>
        /// The table column identifier
        /// </summary>
        public const int TableColumnId = 1;
        /// <summary>
        /// The field column identifier
        /// </summary>
        public const int FieldColumnId = 2;
        /// <summary>
        /// The name column identifier
        /// </summary>
        public const int NameColumnId = 3;
        /// <summary>
        /// The percent column identifier
        /// </summary>
        public const int PercentColumnId = 4;

        /// <summary>
        /// Enum AdvancedFindColumnColumns
        /// </summary>
        public enum AdvancedFindColumnColumns
        {
            /// <summary>
            /// The table
            /// </summary>
            Table = TableColumnId,
            /// <summary>
            /// The field
            /// </summary>
            Field = FieldColumnId,
            /// <summary>
            /// The name
            /// </summary>
            Name = NameColumnId,
            /// <summary>
            /// The percent width
            /// </summary>
            PercentWidth = PercentColumnId
        }

        /// <summary>
        /// Gets or sets the table.
        /// </summary>
        /// <value>The table.</value>
        public string Table { get; set; }
        /// <summary>
        /// Gets or sets the field.
        /// </summary>
        /// <value>The field.</value>
        public string Field { get; set; }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the width of the percent.
        /// </summary>
        /// <value>The width of the percent.</value>
        public double PercentWidth { get; set; }

        /// <summary>
        /// Gets or sets the lookup field column definition.
        /// </summary>
        /// <value>The lookup field column definition.</value>
        public LookupFieldColumnDefinition LookupFieldColumnDefinition { get; set; }
        /// <summary>
        /// Gets or sets the lookup formula column definition.
        /// </summary>
        /// <value>The lookup formula column definition.</value>
        public LookupFormulaColumnDefinition LookupFormulaColumnDefinition { get; set; }
        /// <summary>
        /// Gets or sets the lookup column definition.
        /// </summary>
        /// <value>The lookup column definition.</value>
        public LookupColumnDefinitionBase LookupColumnDefinition { get; set; }
        /// <summary>
        /// Gets the table definition.
        /// </summary>
        /// <value>The table definition.</value>
        public TableDefinitionBase TableDefinition { get; private set; }
        /// <summary>
        /// Gets or sets the manager.
        /// </summary>
        /// <value>The manager.</value>
        public new AdvancedFindColumnsManager Manager { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdvancedFindColumnRow"/> class.
        /// </summary>
        /// <param name="manager">The manager.</param>
        public AdvancedFindColumnRow(AdvancedFindColumnsManager manager) : base(manager)
        {
            Manager = manager;
        }

        /// <summary>
        /// Gets the cell props.
        /// </summary>
        /// <param name="columnId">The column identifier.</param>
        /// <returns>DataEntryGridCellProps.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
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

        /// <summary>
        /// Gets the cell style.
        /// </summary>
        /// <param name="columnId">The column identifier.</param>
        /// <returns>DataEntryGridCellStyle.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
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

        /// <summary>
        /// Sets the cell value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
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
                        var percentWidth = (double) decimalProps.Value;
                        PercentWidth = percentWidth;
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

        /// <summary>
        /// Loads from entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public override void LoadFromEntity(AdvancedFindColumn entity)
        {
            LookupColumnDefinition = Manager.ViewModel.LookupDefinition.LoadFromAdvFindColumnEntity(entity);
            LoadFromColumnDefinition(LookupColumnDefinition);
        }

        /// <summary>
        /// Validates the row.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public override bool ValidateRow()
        {
            return true;
        }

        /// <summary>
        /// Saves to entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="rowIndex">Index of the row.</param>
        public override void SaveToEntity(AdvancedFindColumn entity, int rowIndex)
        {
            entity.AdvancedFindId = Manager.ViewModel.AdvancedFindId;
            entity.ColumnId = rowIndex + 1;
            LookupColumnDefinition.SaveToEntity(entity);
        }

        /// <summary>
        /// Loads from column definition.
        /// </summary>
        /// <param name="column">The column.</param>
        public virtual void LoadFromColumnDefinition(LookupColumnDefinitionBase column)
        {
            LookupColumnDefinition = column;

            Table = column.TableDescription;
            if (Table.IsNullOrEmpty())
            {
                Table = Manager.ViewModel.LookupDefinition.TableDefinition.Description;
            }
            Field = column.FieldDescription;

            Name = column.Caption;
            PercentWidth = column.PercentWidth / 100;
        }

        /// <summary>
        /// Sets the formula table field.
        /// </summary>
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

        /// <summary>
        /// Updates the width of the percent.
        /// </summary>
        public void UpdatePercentWidth()
        {
            PercentWidth = LookupColumnDefinition.PercentWidth / 100;
            if (Manager.Grid != null)
                Manager.Grid.UpdateRow(this);
        }

        /// <summary>
        /// Disposes this instance.
        /// </summary>
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