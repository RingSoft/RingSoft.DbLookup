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
            builder.Property(p => p.Price).HasColumnType(DbConstants.DecimalColumnType);
            builder.HasKey(p => new {p.StockNumber, p.Location});
        }
    }
}
