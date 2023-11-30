using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace RingSoft.DbLookup.App.Library.MegaDb.Model
{
    [Table("StockCostQuantity")]
    public class StockCostQuantity
    {
        public int StockMasterId { get; set; }
        public virtual StockMaster StockMaster { get; set; }
        public DateTime PurchasedDateTime { get; set; }
        public double Quantity { get; set; }
        public double Cost { get; set; }
    }
}
