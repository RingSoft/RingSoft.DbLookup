using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.DbLookup.App.Library.MegaDb.Model;
using RingSoft.DbLookup.EfCore;

namespace RingSoft.DbLookup.App.Library.EfCore.MegaDb.Configurations
{
    public class StockCostQuantityConfiguration : IEntityTypeConfiguration<StockCostQuantity>
    {
        public void Configure(EntityTypeBuilder<StockCostQuantity> builder)
        {
            builder.HasKey(p => new {p.StockMasterId, p.PurchasedDateTime});
            builder.Property(p => p.StockMasterId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.PurchasedDateTime).HasColumnType(DbConstants.DateColumnType);
            builder.Property(p => p.Quantity).IsRequired().HasColumnType(DbConstants.DecimalColumnType);
            builder.Property(p => p.Cost).IsRequired().HasColumnType(DbConstants.DecimalColumnType);

            builder.HasOne(p => p.StockMaster)
                .WithMany(p => p.CostQuantities)
                .HasForeignKey(p => new {p.StockMasterId})
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
