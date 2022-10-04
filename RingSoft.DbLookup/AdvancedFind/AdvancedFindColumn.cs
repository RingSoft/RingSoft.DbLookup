using System.ComponentModel.DataAnnotations;

namespace RingSoft.DbLookup.AdvancedFind
{
    public class AdvancedFindColumn
    {
        [Required]
        public int AdvancedFindId { get; set; }

        public virtual AdvancedFind AdvancedFind { get; set; }

        [Required]
        public int ColumnId { get; set; }

        [MaxLength(50)]
        public string TableName { get; set; }

        [MaxLength(50)]
        public string FieldName { get; set; }

        [MaxLength(50)]
        public string PrimaryTableName { get; set; }

        [MaxLength(50)]
        public string PrimaryFieldName { get; set; }

        [MaxLength(250)]
        public string Caption { get; set; }

        public double PercentWidth { get; set; }

        public string Formula { get; set; }

        public byte FieldDataType { get; set; }

        public byte DecimalFormatType { get; set; }
    }
}
