using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace RingSoft.SimpleDemo.WPF.Northwind.Model
{
    [Table("Order Details")]
    public sealed class Order_Detail
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int OrderID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProductID { get; set; }

        [Column(TypeName = "money")]
        public decimal UnitPrice { get; set; }

        public short Quantity { get; set; }

        public float Discount { get; set; }

        public Order Order { get; set; }

        public Product Product { get; set; }
    }
}
