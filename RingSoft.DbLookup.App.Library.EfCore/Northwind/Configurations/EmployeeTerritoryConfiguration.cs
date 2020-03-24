using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.DbLookup.App.Library.Northwind.Model;

namespace RSDbLookupApp.Library.EfCore.Northwind.Configurations
{
    public class EmployeeTerritoryConfiguration : IEntityTypeConfiguration<EmployeeTerritory>
    {
        public void Configure(EntityTypeBuilder<EmployeeTerritory> builder)
        {
            builder.HasKey(p => new {p.EmployeeID, p.TerritoryID});

            builder.HasOne(p => p.Employee)
                .WithMany(p => p.Territories)
                .HasForeignKey(p => p.EmployeeID);

            builder.HasOne(p => p.Territory)
                .WithMany(p => p.Employees)
                .HasForeignKey(p => p.TerritoryID);
        }
    }
}
