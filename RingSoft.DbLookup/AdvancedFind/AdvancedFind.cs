// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-19-2022
// ***********************************************************************
// <copyright file="AdvancedFind.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RingSoft.DbLookup.AdvancedFind
{
    /// <summary>
    /// Advanced Find Entity.
    /// </summary>
    public class AdvancedFind
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdvancedFind"/> class.
        /// </summary>
        public AdvancedFind()
        {
            Columns = new HashSet<AdvancedFindColumn>();
            Filters = new HashSet<AdvancedFindFilter>();
            SearchForAdvancedFindFilters = new HashSet<AdvancedFindFilter>();
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [Required]
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the table.
        /// </summary>
        /// <value>The table.</value>
        [Required]
        [MaxLength(50)]
        public string Table { get; set; }

        /// <summary>
        /// Gets or sets from formula.
        /// </summary>
        /// <value>From formula.</value>
        public string FromFormula { get; set; }

        /// <summary>
        /// Gets or sets the refresh rate.
        /// </summary>
        /// <value>The refresh rate.</value>
        public byte? RefreshRate { get; set; }

        /// <summary>
        /// Gets or sets the refresh value.
        /// </summary>
        /// <value>The refresh value.</value>
        public int? RefreshValue { get; set; }

        /// <summary>
        /// Gets or sets the refresh condition.
        /// </summary>
        /// <value>The refresh condition.</value>
        public byte? RefreshCondition { get; set; }

        /// <summary>
        /// Gets or sets the yellow alert.
        /// </summary>
        /// <value>The yellow alert.</value>
        public int? YellowAlert { get; set; }

        /// <summary>
        /// Gets or sets the red alert.
        /// </summary>
        /// <value>The red alert.</value>
        public int? RedAlert { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="AdvancedFind"/> is disabled.
        /// </summary>
        /// <value><c>null</c> if [disabled] contains no value, <c>true</c> if [disabled]; otherwise, <c>false</c>.</value>
        public bool? Disabled { get; set; }

        /// <summary>
        /// Gets or sets the columns.
        /// </summary>
        /// <value>The columns.</value>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<AdvancedFindColumn> Columns { get; set; }


        /// <summary>
        /// Gets or sets the filters.
        /// </summary>
        /// <value>The filters.</value>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<AdvancedFindFilter> Filters { get; set; }

        /// <summary>
        /// Gets or sets the search for advanced find filters.
        /// </summary>
        /// <value>The search for advanced find filters.</value>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<AdvancedFindFilter> SearchForAdvancedFindFilters { get; set; }

    }
}
