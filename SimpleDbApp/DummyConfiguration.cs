using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.DbLookup.EfCore;

namespace SimpleDbApp
{
    public class DummyConfiguration : IEntityTypeConfiguration<DummyTable>
    {
        public void Configure(EntityTypeBuilder<DummyTable> builder)
        {
            builder.Property(p => p.Id).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.Name).HasColumnType(DbConstants.StringColumnType);
        }
    }
}
