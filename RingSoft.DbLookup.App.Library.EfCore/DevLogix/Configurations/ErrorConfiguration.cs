using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.DbLookup.App.Library.DevLogix.Model;

namespace RSDbLookupApp.Library.EfCore.DevLogix.Configurations
{
    public class ErrorConfiguration : IEntityTypeConfiguration<Error>
    {
        public void Configure(EntityTypeBuilder<Error> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Number).IsRequired();
            builder.Property(p => p.Date).IsRequired();
            builder.HasOne(p => p.AssignedToUser)
                .WithMany()
                .HasForeignKey(p => p.AssignedToId);

            builder.HasOne(p => p.TestUser)
                .WithMany()
                .HasForeignKey(p => p.TesterId);
        }
    }
}
