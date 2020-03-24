using System.Data.Entity.ModelConfiguration;
using RingSoft.DbLookup.App.Library.DevLogix.Model;

namespace RingSoft.DbLookup.App.Library.Ef6.DevLogix.Configurations
{
    public class ErrorConfiguration : EntityTypeConfiguration<Error>
    {
        public ErrorConfiguration()
        {
            Property(p => p.Number).IsRequired();
            Property(p => p.Date).IsRequired();
            HasOptional(p => p.AssignedToUser).WithMany().HasForeignKey(p => p.AssignedToId);
            HasOptional(p => p.TestUser).WithMany().HasForeignKey(p => p.TesterId);
        }
    }
}
