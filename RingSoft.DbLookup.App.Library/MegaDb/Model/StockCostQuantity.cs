using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace RingSoft.DbLookup.App.Library.MegaDb.Model
{
    [Table("StockCostQuantity")]
    public class StockCostQuantity
    {
        public string StockNumber { get; set; }
        public string Location { get; set; }
        public DateTime PurchasedDateTime { get; set; }
        public double Quantity { get; set; }
        public double Cost { get; set; }

        public virtual StockMaster StockMaster { get; set; }
    }
}
