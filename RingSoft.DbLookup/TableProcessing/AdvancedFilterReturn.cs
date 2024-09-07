// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 02-01-2023
//
// Last Modified By : petem
// Last Modified On : 12-16-2023
// ***********************************************************************
// <copyright file="AdvancedFilterReturn.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.QueryBuilder;

namespace RingSoft.DbLookup.TableProcessing
{
    /// <summary>
    /// Class AdvancedFilterReturn.
    /// </summary>
    public class AdvancedFilterReturn
    {
        /// <summary>
        /// Gets or sets the field definition.
        /// </summary>
        /// <value>The field definition.</value>
        public FieldDefinition FieldDefinition { get; set; }
        /// <summary>
        /// Gets or sets the name of the primary table.
        /// </summary>
        /// <value>The name of the primary table.</value>
        public string PrimaryTableName { get; set; }
        /// <summary>
        /// Gets or sets the primary field definition.
        /// </summary>
        /// <value>The primary field definition.</value>
        public FieldDefinition PrimaryFieldDefinition { get; set; }
        /// <summary>
        /// Gets or sets the condition.
        /// </summary>
        /// <value>The condition.</value>
        public Conditions Condition { get; set; }
        /// <summary>
        /// Gets or sets the search value.
        /// </summary>
        /// <value>The search value.</value>
        public string SearchValue { get; set; }
        /// <summary>
        /// Gets or sets the formula.
        /// </summary>
        /// <value>The formula.</value>
        public string Formula { get; set; }
        /// <summary>
        /// Gets or sets the formula display value.
        /// </summary>
        /// <value>The formula display value.</value>
        public string FormulaDisplayValue { get; set; }
        /// <summary>
        /// Gets or sets the type of the formula value.
        /// </summary>
        /// <value>The type of the formula value.</value>
        public FieldDataTypes FormulaValueType { get; set; }
        /// <summary>
        /// Gets or sets the lookup definition.
        /// </summary>
        /// <value>The lookup definition.</value>
        public LookupDefinitionBase LookupDefinition { get; set; }
        /// <summary>
        /// Gets or sets the table description.
        /// </summary>
        /// <value>The table description.</value>
        public string TableDescription { get; set; }
        /// <summary>
        /// Gets or sets the type of the date filter.
        /// </summary>
        /// <value>The type of the date filter.</value>
        public DateFilterTypes DateFilterType { get; set; }
        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        /// <value>The path.</value>
        public string Path { get; set; }
        /// <summary>
        /// Creates new index.
        /// </summary>
        /// <value>The new index.</value>
        public int NewIndex { get; set; } = -1;
    }
}
