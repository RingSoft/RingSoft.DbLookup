using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.DbLookup.App.Library.MegaDb.Model;

namespace RSDbLookupApp.Library.EfCore.MegaDb.Configurations
{
    public class StockCostQuantityConfiguration : IEntityTypeConfiguration<StockCostQuantity>
    {
        public void Configure(EntityTypeBuilder<StockCostQuantity> builder)
        {
            builder.HasKey(p => new {p.StockNumber, p.Location, p.PurchasedDateTime});
            builder.Property(p => p.Quantity).IsRequired();
            builder.Property(p => p.Cost).IsRequired();

            builder.HasOne(p => p.StockMaster)
                .WithMany(p => p.CostQuantities)
                .HasForeignKey(p => new {p.StockNumber, p.Location});
        }
    }
}
