using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RingSoft.DbLookup.App.Library.Northwind.Model
{
    public class CustomerDemographic
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CustomerDemographic()
        {
            Customers = new HashSet<CustomerCustomerDemo>();
        }

        [Key]
        [StringLength(10)]
        public string CustomerTypeID { get; set; }

        [Column(TypeName = "ntext")]
        public string CustomerDesc { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CustomerCustomerDemo> Customers { get; set; }
    }
}
