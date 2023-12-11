// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 07-01-2023
// ***********************************************************************
// <copyright file="FormulaFilterDefinition.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.QueryBuilder;
using System;

namespace RingSoft.DbLookup.TableProcessing
{
    /// <summary>
    /// Class FormulaFilterDefinition.
    /// Implements the <see cref="RingSoft.DbLookup.TableProcessing.FilterItemType{RingSoft.DbLookup.TableProcessing.FormulaFilterDefinition}" />
    /// </summary>
    /// <seealso cref="RingSoft.DbLookup.TableProcessing.FilterItemType{RingSoft.DbLookup.TableProcessing.FormulaFilterDefinition}" />
    public class FormulaFilterDefinition : FilterItemType<FormulaFilterDefinition>
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        public override FilterItemTypes Type => FilterItemTypes.Formula;

        /// <summary>
        /// Gets the formula.
        /// </summary>
        /// <value>The formula.</value>
        public string Formula { get; set; }

        /// <summary>
        /// Gets or sets the condition.
        /// </summary>
        /// <value>The condition.</value>
        public Conditions? Condition { get; set; }

        /// <summary>
        /// Gets or sets the filter value.
        /// </summary>
        /// <value>The filter value.</value>
        public string FilterValue { get; set; }

        /// <summary>
        /// Gets or sets the type of the data.
        /// </summary>
        /// <value>The type of the data.</value>
        public FieldDataTypes DataType { get; set; }

        /// <summary>
        /// Gets or sets the alias.
        /// </summary>
        /// <value>The alias.</value>
        public string Alias { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormulaFilterDefinition"/> class.
        /// </summary>
        /// <param name="tableFilterDefinition">The table filter definition.</param>
        internal FormulaFilterDefinition(TableFilterDefinitionBase tableFilterDefinition) : base(tableFilterDefinition)
        {
            
        }

        /// <summary>
        /// Gets the type of the TreeView.
        /// </summary>
        /// <value>The type of the TreeView.</value>
        public override TreeViewType TreeViewType => TreeViewType.Formula;
        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        /// <value>The name of the property.</value>
        public override string PropertyName { get; internal set; }

        /// <summary>
        /// Copies from.
        /// </summary>
        /// <param name="source">The source.</param>
        internal override void CopyFrom(FilterItemDefinition source)
        {
            var sourceFormulaItem = (FormulaFilterDefinition) source;
            Formula = sourceFormulaItem.Formula;
            Condition = sourceFormulaItem.Condition;
            FilterValue = sourceFormulaItem.FilterValue;

            base.CopyFrom(source);
        }

        /// <summary>
        /// Gets the report begin text print mode.
        /// </summary>
        /// <param name="lookupDefinition">The lookup definition.</param>
        /// <returns>System.String.</returns>
        protected internal override string GetReportBeginTextPrintMode(LookupDefinitionBase lookupDefinition)
        {
            if (!Path.IsNullOrEmpty())
            {
                var foundItem = lookupDefinition.AdvancedFindTree.ProcessFoundTreeViewItem(Path);
                if (foundItem != null)
                {
                    return $"{foundItem.Name}.{Description} Formula";
                }

            }
            return $"{lookupDefinition.TableDefinition.Description}.{Description} Formula ";
        }

        /// <summary>
        /// Gets the report text.
        /// </summary>
        /// <param name="lookupDefinition">The lookup definition.</param>
        /// <param name="printMode">if set to <c>true</c> [print mode].</param>
        /// <returns>System.String.</returns>
        public override string GetReportText(LookupDefinitionBase lookupDefinition, bool printMode)
        {
            var result = string.Empty;
            if (printMode)
            {
                result += GetReportBeginTextPrintMode(lookupDefinition) + " ";
            }

            if (Condition == null)
            {
                result = "<Complex Formula>";
            }
            else
            {
                result += GetConditionText(Condition.Value) + " ";
                switch (Condition)
                {
                    case Conditions.EqualsNull:
                    case Conditions.NotEqualsNull:
                        result = result.Trim();
                        break;
                    default:
                        if (ValueType == ValueTypes.DateTime)
                        {
                            var dateSearchValue = GetDateReportText();
                            result += dateSearchValue;
                        }
                        else
                        {
                            result += Value;
                        }
                        break;
                }
            }

            return result;
        }

        /// <summary>
        /// Loads from entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="lookupDefinition">The lookup definition.</param>
        /// <param name="path">The path.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public override bool LoadFromEntity(AdvancedFindFilter entity, LookupDefinitionBase lookupDefinition,
            string path = "")
        {
            Formula = entity.Formula;
            Description = entity.FormulaDisplayValue;
            DataType = (FieldDataTypes)entity.FormulaDataType;
            Condition = (Conditions)entity.Operand;
            FilterValue = entity.SearchForValue;
            Path = entity.Path;
            ValueType = DataType.ConvertFieldTypeIntoValueType();

            var result = base.LoadFromEntity(entity, lookupDefinition);

            if (Path.IsNullOrEmpty())
            {
                Alias = TableFilterDefinition.TableDefinition.TableName;
            }
            else
            {
                var foundItem = lookupDefinition.AdvancedFindTree.ProcessFoundTreeViewItem(Path, TreeViewType.Formula);
                Alias = lookupDefinition.AdvancedFindTree.MakeIncludes(foundItem).LookupJoin.JoinDefinition.Alias;
            }

            return result;
        }

        /// <summary>
        /// Saves to entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public override void SaveToEntity(AdvancedFindFilter entity)
        {
            entity.Formula = Formula;
            entity.FormulaDisplayValue = Description;
            entity.FormulaDataType = (byte)DataType;
            entity.Operand = (byte)Condition.Value;

            base.SaveToEntity(entity);
            //entity.SearchForValue = FilterValue;
        }

        /// <summary>
        /// Loads from filter return.
        /// </summary>
        /// <param name="filterReturn">The filter return.</param>
        /// <param name="treeViewItem">The tree view item.</param>
        /// <returns>System.String.</returns>
        public override string LoadFromFilterReturn(AdvancedFilterReturn filterReturn, TreeViewItem treeViewItem)
        {
            Formula = filterReturn.Formula;
            Condition = filterReturn.Condition;
            Description = filterReturn.FormulaDisplayValue;
            DataType = filterReturn.FormulaValueType;
            Path = filterReturn.Path;
            FilterValue = filterReturn.SearchValue;
            ValueType = filterReturn.FormulaValueType.ConvertFieldTypeIntoValueType();

            if (Path.IsNullOrEmpty())
            {
                Alias = TableFilterDefinition.TableDefinition.TableName;
            }
            else
            {
                Alias = treeViewItem.BaseTree.MakeIncludes(treeViewItem).LookupJoin.JoinDefinition.Alias;
            }
            var value = base.LoadFromFilterReturn(filterReturn, treeViewItem);
            DisplayValue = Value = value;
            return value;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return Formula;
        }

        /// <summary>
        /// Saves to filter return.
        /// </summary>
        /// <param name="filterReturn">The filter return.</param>
        public override void SaveToFilterReturn(AdvancedFilterReturn filterReturn)
        {
            filterReturn.Formula = Formula;
            if (Condition != null) filterReturn.Condition = Condition.Value;
            filterReturn.FormulaDisplayValue = Description;
            filterReturn.FormulaValueType = DataType;
            filterReturn.SearchValue = FilterValue;

            base.SaveToFilterReturn(filterReturn);
        }

        /// <summary>
        /// Gets the new filter item definition.
        /// </summary>
        /// <returns>FilterItemDefinition.</returns>
        public override FilterItemDefinition GetNewFilterItemDefinition()
        {
            return new FormulaFilterDefinition(TableFilterDefinition);
        }

        /// <summary>
        /// Gets the new path.
        /// </summary>
        /// <returns>System.String.</returns>
        internal override string GetNewPath()
        {
            return string.Empty;
        }

        /// <summary>
        /// Converts to universal time.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>DateTime.</returns>
        protected internal override DateTime ConvertToUniversalTime(DateTime date)
        {
            if (ValueType == ValueTypes.DateTime && SystemGlobals.ConvertAllDatesToUniversalTime)
            {
                date = date.ToUniversalTime();
            }
            return base.ConvertToUniversalTime(date);
        }

    }
}
