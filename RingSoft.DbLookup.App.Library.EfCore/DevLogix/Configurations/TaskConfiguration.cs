using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.DbLookup.App.Library.DevLogix.Model;

namespace RSDbLookupApp.Library.EfCore.DevLogix.Configurations
{
    public class TaskConfiguration : IEntityTypeConfiguration<Task>
    {
        public void Configure(EntityTypeBuilder<Task> builder)
        {
            builder.Property(p => p.Name).IsRequired();
            builder.HasOne(p => p.Project).WithMany().HasForeignKey(p => p.ProjectId);
            builder.HasOne(p => p.AssignedTo).WithMany().HasForeignKey(p => p.AssignedToId);
        }
    }
}
