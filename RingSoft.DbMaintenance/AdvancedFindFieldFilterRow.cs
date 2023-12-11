// ***********************************************************************
// Assembly         : RingSoft.DbMaintenance
// Author           : petem
// Created          : 02-01-2023
//
// Last Modified By : petem
// Last Modified On : 03-19-2023
// ***********************************************************************
// <copyright file="AdvancedFindFieldFilterRow.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.TableProcessing;

namespace RingSoft.DbMaintenance
{
    /// <summary>
    /// Class AdvancedFindFieldFilterRow.
    /// Implements the <see cref="RingSoft.DbMaintenance.AdvancedFindFilterRow" />
    /// </summary>
    /// <seealso cref="RingSoft.DbMaintenance.AdvancedFindFilterRow" />
    public class AdvancedFindFieldFilterRow : AdvancedFindFilterRow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdvancedFindFieldFilterRow"/> class.
        /// </summary>
        /// <param name="manager">The manager.</param>
        public AdvancedFindFieldFilterRow(AdvancedFindFiltersManager manager) : base(manager)
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
            if (filter is FieldFilterDefinition fieldFilter)
            {
                //var path = fieldFilter.Path;
                //if (path.IsNullOrEmpty() && fieldFilter.FieldDefinition != null)
                //{
                if (fieldFilter.FieldDefinition != null)
                {
                    Field = fieldFilter.FieldDefinition.Description;
                }

                Table = fieldFilter.TableDescription;
            }
            base.LoadFromFilterDefinition(filter, isFixed, rowIndex);
        }

        /// <summary>
        /// Loads from filter return.
        /// </summary>
        /// <param name="advancedFilterReturn">The advanced filter return.</param>
        public override void LoadFromFilterReturn(AdvancedFilterReturn advancedFilterReturn)
        {
            var treeViewItem =
                Manager.ViewModel.LookupDefinition.AdvancedFindTree.ProcessFoundTreeViewItem(advancedFilterReturn.Path,
                    TreeViewType.Field);

            SetupTable(treeViewItem);
            Field = treeViewItem.Name;

            if (FilterItemDefinition == null)
            {
                FilterItemDefinition = Manager.ViewModel.LookupDefinition.FilterDefinition
                    .AddUserFilter(advancedFilterReturn.FieldDefinition, advancedFilterReturn.Condition,
                        advancedFilterReturn.SearchValue, GetNewFilterIndex());
            }

            FilterItemDefinition.LoadFromFilterReturn(advancedFilterReturn, treeViewItem);
            
            base.LoadFromFilterReturn(advancedFilterReturn);
        }

        /// <summary>
        /// Loads from entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public override void LoadFromEntity(AdvancedFindFilter entity)
        {
            //Base does this
            //FilterItemDefinition = Manager.ViewModel.LookupDefinition.LoadFromAdvFindFilter(entity);
            //LoadFromFilterDefinition(FilterItemDefinition, false, entity.FilterId);

            if (!entity.Path.IsNullOrEmpty())
            {
                var treeViewItem =
                    Manager.ViewModel.LookupDefinition.AdvancedFindTree.ProcessFoundTreeViewItem(entity.Path);

                if (treeViewItem != null)
                {
                    SetupTable(treeViewItem);
                    Field = treeViewItem.Name;
                }
            }

            base.LoadFromEntity(entity);
        }
    }
}
