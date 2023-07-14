using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.DbLookup.EfCore;
using RingSoft.DbLookup.Tests.Model;

namespace RingSoft.DbLookup.Tests.Configurations
{
    public class TimeClockConfiguration : IEntityTypeConfiguration<TimeClock>
    {
        public void Configure(EntityTypeBuilder<TimeClock> builder)
        {
            builder.Property(p => p.Id).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.TimeClockId).HasColumnType(DbConstants.StringColumnType);
            builder.Property(p => p.PunchInDate).HasColumnType(DbConstants.DateColumnType);
            builder.Property(p => p.PunchOutDate).HasColumnType(DbConstants.DateColumnType);
            builder.Property(p => p.CustomerId).HasColumnType(DbConstants.IntegerColumnType);
            builder.Property(p => p.ErrorId).HasColumnType(DbConstants.IntegerColumnType);

            builder.HasOne(p => p.Customer)
                .WithMany(p => p.TimeClocks)
                .HasForeignKey(p => p.CustomerId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            builder.HasOne(p => p.Error)
                .WithMany(p => p.TimeClocks)
                .HasForeignKey(p => p.ErrorId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);
        }
    }
}
