using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.DbLookup.App.Library.MegaDb.Model;

namespace RingSoft.DbLookup.App.Library.EfCore.MegaDb.Configurations
{
    public class StockConfiguration : IEntityTypeConfiguration<StockMaster>
    {
        public void Configure(EntityTypeBuilder<StockMaster> builder)
        {
            builder.HasKey(p => new {p.StockNumber, p.Location});
        }
    }
}
