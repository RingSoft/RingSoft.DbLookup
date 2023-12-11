// ***********************************************************************
// Assembly         : RingSoft.DbMaintenance
// Author           : petem
// Created          : 02-03-2023
//
// Last Modified By : petem
// Last Modified On : 02-24-2023
// ***********************************************************************
// <copyright file="AdvancedFindFormulaFilterRow.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.TableProcessing;

namespace RingSoft.DbMaintenance
{
    /// <summary>
    /// Class AdvancedFindFormulaFilterRow.
    /// Implements the <see cref="RingSoft.DbMaintenance.AdvancedFindFilterRow" />
    /// </summary>
    /// <seealso cref="RingSoft.DbMaintenance.AdvancedFindFilterRow" />
    public class AdvancedFindFormulaFilterRow : AdvancedFindFilterRow
    {
        /// <summary>
        /// Gets the formula.
        /// </summary>
        /// <value>The formula.</value>
        public string Formula { get; private set; }
        /// <summary>
        /// Gets the type of the formula data.
        /// </summary>
        /// <value>The type of the formula data.</value>
        public FieldDataTypes FormulaDataType { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdvancedFindFormulaFilterRow"/> class.
        /// </summary>
        /// <param name="manager">The manager.</param>
        public AdvancedFindFormulaFilterRow(AdvancedFindFiltersManager manager) : base(manager)
        {
        }

        /// <summary>
        /// Loads from filter definition.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="isFixed">if set to <c>true</c> [is fixed].</param>
        /// <param name="rowIndex">Index of the row.</param>
        public override void LoadFromFilterDefinition(FilterItemDefinition filter, bool isFixed, int rowIndex)
        {
            if (filter is FormulaFilterDefinition formulaFilter)
            {
                Field = $"{formulaFilter.Description} Formula";
            }
            base.LoadFromFilterDefinition(filter, isFixed, rowIndex);
        }

        /// <summary>
        /// Loads from filter return.
        /// </summary>
        /// <param name="advancedFilterReturn">The advanced filter return.</param>
        public override void LoadFromFilterReturn(AdvancedFilterReturn advancedFilterReturn)
        {
            TreeViewItem treeViewItem = null;

            if (!advancedFilterReturn.Path.IsNullOrEmpty())
            {
                treeViewItem =
                    Manager.ViewModel.LookupDefinition.AdvancedFindTree.ProcessFoundTreeViewItem(advancedFilterReturn.Path,
                        TreeViewType.Formula);
            }

            SetupTable(treeViewItem);
            Field = $"{advancedFilterReturn.FormulaDisplayValue} Formula";

            if (FilterItemDefinition == null)
            {
                FilterItemDefinition = Manager.ViewModel.LookupDefinition.FilterDefinition
                    .AddUserFilter(advancedFilterReturn.Formula
                    , advancedFilterReturn.Condition
                    , advancedFilterReturn.SearchValue
                    , ""
                    , advancedFilterReturn.FormulaValueType
                    , GetNewFilterIndex()
);
            }

            FilterItemDefinition.LoadFromFilterReturn(advancedFilterReturn, treeViewItem);

            base.LoadFromFilterReturn(advancedFilterReturn);
        }

        //public override void LoadFromEntity(AdvancedFindFilter entity)
        //{
        //    FilterItemDefinition = Manager.ViewModel.LookupDefinition.LoadFromAdvFindFilter(entity);
        //    LoadFromFilterDefinition(FilterItemDefinition, false, entity.FilterId);
        //    base.LoadFromEntity(entity);
        //}
    }
}
