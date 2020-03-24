using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.DbLookup.App.Library.Northwind.Model;

namespace RingSoft.DbLookup.App.Library.EfCore.Northwind.Configurations
{
    public class Order_DetailConfiguration : IEntityTypeConfiguration<Order_Detail>
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
