using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.SimpleDemo.WPF.Northwind.Model;

namespace RingSoft.SimpleDemo.WPF.Northwind.Configurations
{
    public class OrderDetailConfiguration : IEntityTypeConfiguration<Order_Detail>
    {
        public void Configure(EntityTypeBuilder<Order_Detail> builder)
        {
            builder.HasKey(p => new {p.OrderID, p.ProductID});

            builder.HasOne(p => p.Order)
                .WithMany(p => p.Order_Details)
                .HasForeignKey(p => p.OrderID);

            builder.HasOne(p => p.Product)
                .WithMany(p => p.Order_Details)
                .HasForeignKey(p => p.ProductID);
        }
    }
}
