using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RingSoft.DbLookup.App.Library.MegaDb.Model
{
    [Table("StockMaster")]
    public class StockMaster
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public int StockId { get; set; }

        public virtual StocksTable Stocks { get; set; }

        [Required]
        public int MliLocationId { get; set; }

        public virtual MLILocationsTable MliLocations { get; set; }

        [Required]
        public double Price { get; set; }

        public virtual ICollection<StockCostQuantity> CostQuantities { get; set; }

        public StockMaster()
        {
            CostQuantities = new HashSet<StockCostQuantity>();
        }
    }
}
