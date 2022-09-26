using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RingSoft.DbLookup.AdvancedFind
{
    public class AdvancedFind
    {
        public AdvancedFind()
        {
            Columns = new HashSet<AdvancedFindColumn>();
            Filters = new HashSet<AdvancedFindFilter>();
            SearchForAdvancedFindFilters = new HashSet<AdvancedFindFilter>();
        }

        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Table { get; set; }

        public string FromFormula { get; set; }

        public byte? RefreshRate { get; set; }

        public int? RefreshValue { get; set; }

        public byte? RefreshCondition { get; set; }

        public int? YellowAlert { get; set; }

        public int? RedAlert { get; set; }

        public bool? Disabled { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<AdvancedFindColumn> Columns { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<AdvancedFindFilter> Filters { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<AdvancedFindFilter> SearchForAdvancedFindFilters { get; set; }

    }
}
