// ***********************************************************************
// Assembly         : RingSoft.DbMaintenance
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 11-20-2023
// ***********************************************************************
// <copyright file="AdvancedFindFilterRow.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
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
    /// <summary>
    /// Class AdvancedFindFilterRow.
    /// Implements the <see cref="RingSoft.DbMaintenance.DbMaintenanceDataEntryGridRow{RingSoft.DbLookup.AdvancedFind.AdvancedFindFilter}" />
    /// </summary>
    /// <seealso cref="RingSoft.DbMaintenance.DbMaintenanceDataEntryGridRow{RingSoft.DbLookup.AdvancedFind.AdvancedFindFilter}" />
    public abstract class AdvancedFindFilterRow : DbMaintenanceDataEntryGridRow<AdvancedFindFilter>
    {
        /// <summary>
        /// Gets or sets the manager.
        /// </summary>
        /// <value>The manager.</value>
        public AdvancedFindFiltersManager Manager { get; set; }

        /// <summary>
        /// Gets or sets the left parentheses count.
        /// </summary>
        /// <value>The left parentheses count.</value>
        public int LeftParenthesesCount { get; set; }
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
        /// Gets the search value text.
        /// </summary>
        /// <value>The search value text.</value>
        public string SearchValueText { get; private set; }
        /// <summary>
        /// Gets or sets the right parentheses count.
        /// </summary>
        /// <value>The right parentheses count.</value>
        public int RightParenthesesCount { get; set; }
        /// <summary>
        /// Gets the end logics.
        /// </summary>
        /// <value>The end logics.</value>
        public EndLogics? EndLogics { get; private set; }

        /// <summary>
        /// Gets or sets the condition.
        /// </summary>
        /// <value>The condition.</value>
        public Conditions Condition { get; set; }
        /// <summary>
        /// Gets the search value.
        /// </summary>
        /// <value>The search value.</value>
        public string SearchValue { get; private set; }
        /// <summary>
        /// Gets the display search value.
        /// </summary>
        /// <value>The display search value.</value>
        public string DisplaySearchValue { get; private set; }
        /// <summary>
        /// Gets the filter item definition.
        /// </summary>
        /// <value>The filter item definition.</value>
        public FilterItemDefinition FilterItemDefinition { get; internal set; }
        /// <summary>
        /// Gets the field definition.
        /// </summary>
        /// <value>The field definition.</value>
        public FieldDefinition FieldDefinition { get; private set; }
        /// <summary>
        /// Gets or sets the primary table.
        /// </summary>
        /// <value>The primary table.</value>
        public string PrimaryTable { get; set; }
        /// <summary>
        /// Gets or sets the primary field.
        /// </summary>
        /// <value>The primary field.</value>
        public string PrimaryField { get; set; }
        /// <summary>
        /// Gets the parent field definition.
        /// </summary>
        /// <value>The parent field definition.</value>
        public FieldDefinition ParentFieldDefinition { get; private set; }
        /// <summary>
        /// Gets the formula display value.
        /// </summary>
        /// <value>The formula display value.</value>
        public string FormulaDisplayValue { get; private set; }
        /// <summary>
        /// Gets the type of the date filter.
        /// </summary>
        /// <value>The type of the date filter.</value>
        public DateFilterTypes DateFilterType { get; private set; }
        /// <summary>
        /// Gets or sets the date filter value.
        /// </summary>
        /// <value>The date filter value.</value>
        public int DateFilterValue { get; set; }
        /// <summary>
        /// Gets the date search value.
        /// </summary>
        /// <value>The date search value.</value>
        public string DateSearchValue { get; private set; }

        /// <summary>
        /// Gets the end logics setup.
        /// </summary>
        /// <value>The end logics setup.</value>
        public TextComboBoxControlSetup EndLogicsSetup { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether [reset lookup].
        /// </summary>
        /// <value><c>true</c> if [reset lookup]; otherwise, <c>false</c>.</value>
        protected bool ResetLookup { get; set; } = true;

        /// <summary>
        /// Gets the automatic fill field.
        /// </summary>
        /// <value>The automatic fill field.</value>
        public FieldDefinition AutoFillField { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="AdvancedFindFilterRow"/> is clearing.
        /// </summary>
        /// <value><c>true</c> if clearing; otherwise, <c>false</c>.</value>
        public bool Clearing { get; set; }

        /// <summary>
        /// Gets the path.
        /// </summary>
        /// <value>The path.</value>
        public string Path { get; internal set; }

        /// <summary>
        /// Gets or sets the filter return.
        /// </summary>
        /// <value>The filter return.</value>
        public AdvancedFilterReturn FilterReturn { get; set; }

        /// <summary>
        /// The fixed parentheses set
        /// </summary>
        private bool _fixedParenthesesSet = false;

        /// <summary>
        /// The search automatic fill field
        /// </summary>
        private FieldDefinition _searchAutoFillField;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdvancedFindFilterRow"/> class.
        /// </summary>
        /// <param name="manager">The manager.</param>
        public AdvancedFindFilterRow(AdvancedFindFiltersManager manager) : base(manager)
        {
            Manager = manager;
            EndLogicsSetup = new TextComboBoxControlSetup();
            EndLogicsSetup.LoadFromEnum<EndLogics>();
        }

        /// <summary>
        /// Gets the cell props.
        /// </summary>
        /// <param name="columnId">The column identifier.</param>
        /// <returns>DataEntryGridCellProps.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
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

        /// <summary>
        /// Gets the cell style.
        /// </summary>
        /// <param name="columnId">The column identifier.</param>
        /// <returns>DataEntryGridCellStyle.</returns>
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

        /// <summary>
        /// Sets the cell value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
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

        /// <summary>
        /// Sets the cell value from lookup return.
        /// </summary>
        /// <param name="filterReturn">The filter return.</param>
        public void SetCellValueFromLookupReturn(AdvancedFilterReturn filterReturn)
        {
            LoadFromFilterReturn(filterReturn);
            Manager.ViewModel.RefreshLookup();
        }


        /// <summary>
        /// Loads from entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
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

        /// <summary>
        /// Makes the parent field.
        /// </summary>
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
        public override void SaveToEntity(AdvancedFindFilter entity, int rowIndex)
        {
            entity.FilterId = rowIndex + 1;
            FilterItemDefinition?.SaveToEntity(entity);
            {

            }
        }

        /// <summary>
        /// Loads from filter definition.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="isFixed">if set to <c>true</c> [is fixed].</param>
        /// <param name="rowIndex">Index of the row.</param>
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

        /// <summary>
        /// Finishes the off filter.
        /// </summary>
        /// <param name="isFixed">if set to <c>true</c> [is fixed].</param>
        /// <param name="theEnd">if set to <c>true</c> [the end].</param>
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

        /// <summary>
        /// Makes the search value text.
        /// </summary>
        /// <param name="searchValue">The search value.</param>
        public void MakeSearchValueText(string searchValue = "")
        {
            SearchValueText = FilterItemDefinition.GetReportText(Manager.ViewModel.LookupDefinition, false);
 
        }

        /// <summary>
        /// Gets the new index of the filter.
        /// </summary>
        /// <returns>System.Int32.</returns>
        protected virtual int GetNewFilterIndex()
        {
            var result = Manager.GetNewRowIndex();
            var fixedItems = Manager.Rows.OfType<AdvancedFindFilterRow>()
                .Where(p => p.IsFixed)
                .ToList();
            return result - fixedItems.Count;
        }

        /// <summary>
        /// Loads from filter return.
        /// </summary>
        /// <param name="advancedFilterReturn">The advanced filter return.</param>
        public virtual void LoadFromFilterReturn(AdvancedFilterReturn advancedFilterReturn)
        {
            IsNew = false;
            Condition = advancedFilterReturn.Condition;
            SearchValue = advancedFilterReturn.SearchValue;
            FilterReturn = advancedFilterReturn;

            MakeSearchValueText();
        }

        /// <summary>
        /// Gets a value indicating whether [allow user delete].
        /// </summary>
        /// <value><c>true</c> if [allow user delete]; otherwise, <c>false</c>.</value>
        public override bool AllowUserDelete => !IsFixed;

        /// <summary>
        /// Setups the table.
        /// </summary>
        /// <param name="selectedTreeViewItem">The selected TreeView item.</param>
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

        /// <summary>
        /// Disposes this instance.
        /// </summary>
        public override void Dispose()
        {
            if (FilterItemDefinition != null)
                FilterItemDefinition.TableFilterDefinition.RemoveUserFilter(FilterItemDefinition);
            if (!Manager.ViewModel.Clearing)
            {
                if (ValidateParentheses())
                {
                    Manager.ViewModel.ResetLookup(false);
                }
            }

            base.Dispose();
        }

        /// <summary>
        /// Validates the parentheses.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
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
