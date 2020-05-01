using System;

namespace RingSoft.DbLookup.App.Library.Northwind.LookupModel
{
    public class OrderLookup
    {
        public int OrderId { get; set; }

        public DateTime OrderDate { get; set; }

        public string Customer { get; set; }

        public string Employee { get; set; }
    }
}
