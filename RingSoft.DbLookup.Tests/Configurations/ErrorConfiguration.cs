using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.DbLookup.EfCore;
using RingSoft.DbLookup.Tests.Model;

namespace RingSoft.DbLookup.Tests.Configurations
{
    public  class ErrorConfiguration : IEntityTypeConfiguration<Error>
    {
        public void Configure(EntityTypeBuilder<Error> builder)
        {
            builder.Property(p => p.Id).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.ErrorId).HasColumnType(DbConstants.StringColumnType);
        }
    }
}
