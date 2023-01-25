﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RingSoft.DbLookup.AdvancedFind
{
    public enum DateFilterTypes
    {
        [Description("Specific Date")]
        SpecificDate = 0,
        [Description("Day(s) Ago")]
        Days = 1,
        [Description("Week(s) Ago")]
        Weeks = 2,
        [Description("Month(s) Ago")]
        Months = 3,
        [Description("Year(s) Ago")]
        Years = 4,
    }
    public class AdvancedFindFilter
    {
        [Required]
        [Key]
        public int AdvancedFindId { get; set; }

        public virtual AdvancedFind AdvancedFind { get; set; }

        [Required]
        [Key]
        public int FilterId { get; set; }

        public byte LeftParentheses { get; set; }

        [MaxLength(50)]
        public string TableName { get; set; }

        [MaxLength(50)]
        public string FieldName { get; set; }

        [MaxLength(50)]
        public string PrimaryTableName { get; set; }

        [MaxLength(50)]
        public string PrimaryFieldName { get; set; }

        [MaxLength(1000)]
        public string Path { get; set; }

        public byte Operand { get; set; }

        [MaxLength(50)]
        public string SearchForValue { get; set; }

        public string Formula { get; set; }

        public byte FormulaDataType { get; set; }

        [MaxLength(50)]
        public string FormulaDisplayValue { get; set; }

        public int? SearchForAdvancedFindId { get; set; }

        public virtual AdvancedFind SearchForAdvancedFind { get; set; }

        public bool CustomDate { get; set; }

        public byte RightParentheses { get; set; }

        public byte EndLogic { get; set; }

        [DefaultValue(0)]
        public byte DateFilterType { get; set; }
    }
}
