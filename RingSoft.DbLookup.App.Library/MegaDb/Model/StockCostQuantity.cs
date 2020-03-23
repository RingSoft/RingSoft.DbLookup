using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace RSDbLookupApp.Library.MegaDb.Model
{
    [Table("StockCostQuantity")]
    public class StockCostQuantity
    {
        public string StockNumber { get; set; }
        public string Location { get; set; }
        public DateTime PurchasedDateTime { get; set; }
        public decimal Quantity { get; set; }
        public decimal Cost { get; set; }

        public virtual StockMaster StockMaster { get; set; }
    }
}
