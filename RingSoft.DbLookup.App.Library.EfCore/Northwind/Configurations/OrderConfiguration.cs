using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.DbLookup.App.Library.Northwind.Model;

namespace RingSoft.DbLookup.App.Library.EfCore.Northwind.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasOne(p => p.Customer)
                .WithMany(p => p.Orders)
                .HasForeignKey(p => p.CustomerID).IsRequired();

            builder.HasOne(p => p.Employee)
                .WithMany(p => p.Orders)
                .HasForeignKey(p => p.EmployeeID).IsRequired();

            builder.HasOne(p => p.Shipper)
                .WithMany(p => p.Orders)
                .HasForeignKey(p => p.ShipVia).IsRequired();
        }
    }
}
