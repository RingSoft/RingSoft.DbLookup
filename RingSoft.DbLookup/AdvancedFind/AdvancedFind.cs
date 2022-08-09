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

        public string Name { get; set; }

        public string Table { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<AdvancedFindColumn> Columns { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<AdvancedFindFilter> Filters { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<AdvancedFindFilter> SearchForAdvancedFindFilters { get; set; }

    }
}
