// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 06-30-2023
// ***********************************************************************
// <copyright file="AdvancedFindColumn.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.ComponentModel.DataAnnotations;

namespace RingSoft.DbLookup.AdvancedFind
{
    /// <summary>
    /// Class AdvancedFindColumn.
    /// </summary>
    public class AdvancedFindColumn
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
        /// Gets or sets the column identifier.
        /// </summary>
        /// <value>The column identifier.</value>
        [Required]
        public int ColumnId { get; set; }

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
        /// Gets or sets the caption.
        /// </summary>
        /// <value>The caption.</value>
        [MaxLength(250)]
        public string Caption { get; set; }

        /// <summary>
        /// Gets or sets the width of the percent.
        /// </summary>
        /// <value>The width of the percent.</value>
        public double PercentWidth { get; set; }

        /// <summary>
        /// Gets or sets the formula.
        /// </summary>
        /// <value>The formula.</value>
        public string Formula { get; set; }

        /// <summary>
        /// Gets or sets the type of the field data.
        /// </summary>
        /// <value>The type of the field data.</value>
        public byte FieldDataType { get; set; }

        /// <summary>
        /// Gets or sets the type of the decimal format.
        /// </summary>
        /// <value>The type of the decimal format.</value>
        public byte DecimalFormatType { get; set; }
    }
}
