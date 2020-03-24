using System.Data.Entity.ModelConfiguration;
using RingSoft.DbLookup.App.Library.DevLogix.Model;

namespace RingSoft.DbLookup.App.Library.Ef6.DevLogix.Configurations
{
    public class TaskConfiguration : EntityTypeConfiguration<Task>
    {
        public TaskConfiguration()
        {
            Property(p => p.Name).IsRequired();
            HasOptional(p => p.Project).WithMany().HasForeignKey(p => p.ProjectId);
            HasOptional(p => p.AssignedTo).WithMany().HasForeignKey(p => p.AssignedToId);
        }
    }
}
