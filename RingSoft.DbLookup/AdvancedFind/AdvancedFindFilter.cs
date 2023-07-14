using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RingSoft.DbLookup.AdvancedFind
{
    public class AdvancedFindFilter
    {
        [Required]
        public int AdvancedFindId { get; set; }

        public virtual AdvancedFind AdvancedFind { get; set; }

        [Required]
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
