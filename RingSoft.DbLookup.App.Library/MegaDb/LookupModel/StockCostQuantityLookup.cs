using System;

namespace RSDbLookupApp.Library.MegaDb.LookupModel
{
    public class StockCostQuantityLookup
    {
        public string StockNumber { get; set; }

        public string Location { get; set; }

        public DateTime PurchasedDate { get; set; }

        public double Quantity { get; set; }

        public double Cost { get; set; }
    }
}
