using System.ComponentModel.DataAnnotations;

namespace RingSoft.DbLookup.AdvancedFind
{
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

        public string TableName { get; set; }

        public string FieldName { get; set; }

        public string PrimaryTableName { get; set; }

        public string PrimaryFieldName { get; set; }

        public byte Operand { get; set; }

        public string SearchForValue { get; set; }

        public string Formula { get; set; }

        public byte FormulaDataType { get; set; }

        public int? SearchForAdvancedFindId { get; set; }

        public virtual AdvancedFind SearchForAdvancedFind { get; set; }

        public bool CustomDate { get; set; }

        public byte RightParentheses { get; set; }

        public byte EndLogic { get; set; }
    }
}
