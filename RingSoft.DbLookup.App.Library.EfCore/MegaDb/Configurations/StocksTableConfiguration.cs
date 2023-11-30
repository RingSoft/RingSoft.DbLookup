using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.DbLookup.App.Library.MegaDb.Model;
using RingSoft.DbLookup.EfCore;

namespace RingSoft.DbLookup.App.Library.EfCore.MegaDb.Configurations
{
    public class StocksTableConfiguration : IEntityTypeConfiguration<StocksTable>
    {
        public void Configure(EntityTypeBuilder<StocksTable> builder)
        {
            builder.Property(p => p.Id).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.Name).HasColumnType(DbConstants.StringColumnType);
        }
    }
}
