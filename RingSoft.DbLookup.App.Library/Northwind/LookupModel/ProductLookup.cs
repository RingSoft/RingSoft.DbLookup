﻿namespace RingSoft.DbLookup.App.Library.Northwind.LookupModel
{
    public class ProductLookup
    {
        public string ProductName { get; set; }

        public double UnitPrice { get; set; }

        public short UnitsInStock { get; set; }

        public virtual string Category { get; set; }
    }
}
