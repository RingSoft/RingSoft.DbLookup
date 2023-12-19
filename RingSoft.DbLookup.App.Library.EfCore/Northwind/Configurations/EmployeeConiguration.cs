using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.DbLookup.App.Library.Northwind.Model;

namespace RingSoft.DbLookup.App.Library.EfCore.Northwind.Configurations
{
    public class EmployeeConiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasOne(p => p.Supervisor)
                .WithMany(p => p.Underlings)
                .HasForeignKey(p => p.SupervisorId);
        }
    }
}
