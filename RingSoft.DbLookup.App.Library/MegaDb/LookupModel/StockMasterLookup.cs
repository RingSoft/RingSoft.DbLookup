﻿namespace RingSoft.DbLookup.App.Library.MegaDb.LookupModel
{
    public class StocksTableLookup
    {
        public string Name { get; set; }
    }

    public class MliLocationsTableLookup
    {
        public string Name { get; set; }
    }

    public class StockMasterLookup
    {
        public string StockNumber { get; set; }

        public string Location { get; set; }

        public double Price { get; set; }
    }
}
