using System.Data.Entity.ModelConfiguration;
using RingSoft.DbLookup.App.Library.DevLogix.Model;

namespace RingSoft.DbLookup.App.Library.Ef6.DevLogix.Configurations
{
    public class IssueConfiguration : EntityTypeConfiguration<Issue>
    {
        public IssueConfiguration()
        {
            Property(p => p.Description).IsRequired();
            HasRequired(p => p.Task).WithMany().HasForeignKey(p => p.TaskId);
        }
    }
}
