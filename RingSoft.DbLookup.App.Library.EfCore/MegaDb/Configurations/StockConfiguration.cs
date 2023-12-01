using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.DbLookup.App.Library.MegaDb.Model;
using RingSoft.DbLookup.EfCore;

namespace RingSoft.DbLookup.App.Library.EfCore.MegaDb.Configurations
{
    public class StockConfiguration : IEntityTypeConfiguration<StockMaster>
    {
        public void Configure(EntityTypeBuilder<StockMaster> builder)
        {
            builder.Property(p => p.Id).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.StockId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.MliLocationId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.Price).HasColumnType(DbConstants.DecimalColumnType);

            builder.HasOne(p => p.Stock)
                .WithMany(p => p.StockMasters)
                .HasForeignKey(p => p.StockId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.MliLocation)
                .WithMany(p => p.StockMasters)
                .HasForeignKey(p => p.MliLocationId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
