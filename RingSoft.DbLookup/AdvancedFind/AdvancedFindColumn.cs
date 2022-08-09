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

        public string TableName { get; set; }

        public string FieldName { get; set; }

        public string PrimaryTableName { get; set; }

        public string PrimaryFieldName { get; set; }

        public string Caption { get; set; }

        public decimal PercentWidth { get; set; }

    }
}
