using System.Data.Entity.ModelConfiguration;
using RingSoft.DbLookup.App.Library.MegaDb.Model;

namespace RingSoft.DbLookup.App.Library.Ef6.MegaDb.Configurations
{
    public class StockCostQuantityConfiguration : EntityTypeConfiguration<StockCostQuantity>
    {
        public StockCostQuantityConfiguration()
        {
            HasKey(p => new {p.StockNumber, p.Location, p.PurchasedDateTime});
            Property(p => p.Quantity).IsRequired();
            Property(p => p.Cost).IsRequired();

            HasRequired(p => p.StockMaster)
                .WithMany(p => p.CostQuantities)
                .HasForeignKey(p => new {p.StockNumber, p.Location});
        }
    }
}
