using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.DbLookup.App.Library.DevLogix.Model;

namespace RSDbLookupApp.Library.EfCore.DevLogix.Configurations
{
    public class IssueConfiguration : IEntityTypeConfiguration<Issue>
    {
        public void Configure(EntityTypeBuilder<Issue> builder)
        {
            builder.Property(p => p.Description).IsRequired();
            builder.Property(p => p.TaskId).IsRequired();
            builder.HasOne(p => p.Task)
                .WithMany().HasForeignKey(f => f.TaskId);
        }
    }
}
