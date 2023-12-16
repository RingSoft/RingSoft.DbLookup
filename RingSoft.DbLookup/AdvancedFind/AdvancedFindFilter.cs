// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 07-13-2023
// ***********************************************************************
// <copyright file="AdvancedFindFilter.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RingSoft.DbLookup.AdvancedFind
{
    /// <summary>
    /// AdvancedFindFilter Entity
    /// </summary>
    public class AdvancedFindFilter
    {
        /// <summary>
        /// Gets or sets the advanced find identifier.
        /// </summary>
        /// <value>The advanced find identifier.</value>
        [Required]
        public int AdvancedFindId { get; set; }

        /// <summary>
        /// Gets or sets the advanced find.
        /// </summary>
        /// <value>The advanced find.</value>
        public virtual AdvancedFind AdvancedFind { get; set; }

        /// <summary>
        /// Gets or sets the filter identifier.
        /// </summary>
        /// <value>The filter identifier.</value>
        [Required]
        public int FilterId { get; set; }

        /// <summary>
        /// Gets or sets the left parentheses.
        /// </summary>
        /// <value>The left parentheses.</value>
        public byte LeftParentheses { get; set; }

        /// <summary>
        /// Gets or sets the name of the table.
        /// </summary>
        /// <value>The name of the table.</value>
        [MaxLength(50)]
        public string TableName { get; set; }

        /// <summary>
        /// Gets or sets the name of the field.
        /// </summary>
        /// <value>The name of the field.</value>
        [MaxLength(50)]
        public string FieldName { get; set; }

        /// <summary>
        /// Gets or sets the name of the primary table.
        /// </summary>
        /// <value>The name of the primary table.</value>
        [MaxLength(50)]
        public string PrimaryTableName { get; set; }

        /// <summary>
        /// Gets or sets the name of the primary field.
        /// </summary>
        /// <value>The name of the primary field.</value>
        [MaxLength(50)]
        public string PrimaryFieldName { get; set; }

        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        /// <value>The path.</value>
        [MaxLength(1000)]
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the operand.
        /// </summary>
        /// <value>The operand.</value>
        public byte Operand { get; set; }

        /// <summary>
        /// Gets or sets the search for value.
        /// </summary>
        /// <value>The search for value.</value>
        [MaxLength(50)]
        public string SearchForValue { get; set; }

        /// <summary>
        /// Gets or sets the formula.
        /// </summary>
        /// <value>The formula.</value>
        public string Formula { get; set; }

        /// <summary>
        /// Gets or sets the type of the formula data.
        /// </summary>
        /// <value>The type of the formula data.</value>
        public byte FormulaDataType { get; set; }

        /// <summary>
        /// Gets or sets the formula display value.
        /// </summary>
        /// <value>The formula display value.</value>
        [MaxLength(50)]
        public string FormulaDisplayValue { get; set; }

        /// <summary>
        /// Gets or sets the search for advanced find identifier.
        /// </summary>
        /// <value>The search for advanced find identifier.</value>
        public int? SearchForAdvancedFindId { get; set; }

        /// <summary>
        /// Gets or sets the search for advanced find.
        /// </summary>
        /// <value>The search for advanced find.</value>
        public virtual AdvancedFind SearchForAdvancedFind { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [custom date].
        /// </summary>
        /// <value><c>true</c> if [custom date]; otherwise, <c>false</c>.</value>
        public bool CustomDate { get; set; }

        /// <summary>
        /// Gets or sets the right parentheses.
        /// </summary>
        /// <value>The right parentheses.</value>
        public byte RightParentheses { get; set; }

        /// <summary>
        /// Gets or sets the end logic.
        /// </summary>
        /// <value>The end logic.</value>
        public byte EndLogic { get; set; }

        /// <summary>
        /// Gets or sets the type of the date filter.
        /// </summary>
        /// <value>The type of the date filter.</value>
        [DefaultValue(0)]
        public byte DateFilterType { get; set; }
    }
}
