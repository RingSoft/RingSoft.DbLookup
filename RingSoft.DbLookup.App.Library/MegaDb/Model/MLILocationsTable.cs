using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RingSoft.DbLookup.App.Library.MegaDb.Model
{
    public class MLILocationsTable
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public virtual ICollection<StockMaster> StockMasters { get; set; }

        public MLILocationsTable()
        {
            StockMasters = new HashSet<StockMaster>();
        }
    }
}
