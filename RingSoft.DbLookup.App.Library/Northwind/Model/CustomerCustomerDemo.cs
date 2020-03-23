using System.ComponentModel.DataAnnotations;

namespace RingSoft.DbLookup.App.Library.Northwind.Model
{
    public class CustomerCustomerDemo
    {
        [StringLength(5)]
        public string CustomerID { get; set; }

        public virtual Customer Customer { get; set; }

        [StringLength(10)]
        public string CustomerTypeID { get; set; }

        public CustomerDemographic CustomerDemographic { get; set; }
    }
}
