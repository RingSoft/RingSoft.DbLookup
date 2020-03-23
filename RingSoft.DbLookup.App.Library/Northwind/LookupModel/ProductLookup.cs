namespace RSDbLookupApp.Library.Northwind.LookupModel
{
    public class ProductLookup
    {
        public string ProductName { get; set; }

        public decimal UnitPrice { get; set; }

        public short UnitsInStock { get; set; }

        public virtual string Category { get; set; }
    }
}
