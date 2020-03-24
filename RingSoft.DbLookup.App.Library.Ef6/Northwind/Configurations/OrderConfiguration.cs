using System.Data.Entity.ModelConfiguration;
using RingSoft.DbLookup.App.Library.Northwind.Model;

namespace RingSoft.DbLookup.App.Library.Ef6.Northwind.Configurations
{
    public class OrderConfiguration : EntityTypeConfiguration<Order>
    {
        public OrderConfiguration()
        {
            HasRequired(p => p.Customer)
                .WithMany(p => p.Orders)
                .HasForeignKey(p => p.CustomerID);

            HasRequired(p => p.Employee)
                .WithMany(p => p.Orders)
                .HasForeignKey(p => p.EmployeeID);

            HasRequired(p => p.Shipper)
                .WithMany(p => p.Orders)
                .HasForeignKey(p => p.ShipVia);
        }
    }
}
