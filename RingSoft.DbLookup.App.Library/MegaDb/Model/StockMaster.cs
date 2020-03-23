using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RSDbLookupApp.Library.MegaDb.Model
{
    [Table("StockMaster")]
    public class StockMaster
    {
        public string StockNumber { get; set; }
        public string Location { get; set; }
        public decimal Price { get; set; }

        public virtual ICollection<StockCostQuantity> CostQuantities { get; set; }
    }
}
