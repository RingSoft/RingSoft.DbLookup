namespace RingSoft.DbLookup.App.Library.Northwind.LookupModel
{
    public class OrderDetailLookup
    {
        public string Order { get; set; }

        public string Customer { get; set; }

        public string Product { get; set; }

        public double UnitPrice { get; set; }

        public short Quantity { get; set; }

        public double ExtendedPrice { get; set; }

        public double Discount { get; set; }

        public string CategoryName { get; set; }
    }
}
