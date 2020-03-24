using System.Data.Entity.ModelConfiguration;
using RingSoft.DbLookup.App.Library.Northwind.Model;

namespace RingSoft.DbLookup.App.Library.Ef6.Northwind.Configurations
{
    public class Order_DetailConfiguration : EntityTypeConfiguration<Order_Detail>
    {
        public Order_DetailConfiguration()
        {
            HasKey(p => new {p.OrderID, p.ProductID});

            HasRequired(p => p.Order)
                .WithMany(p => p.Order_Details)
                .HasForeignKey(p => p.OrderID);

            HasRequired(p => p.Product)
                .WithMany(p => p.Order_Details)
                .HasForeignKey(p => p.ProductID);
        }
    }
}
