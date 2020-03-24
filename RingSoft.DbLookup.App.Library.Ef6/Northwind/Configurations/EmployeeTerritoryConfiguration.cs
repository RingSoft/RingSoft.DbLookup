using System.Data.Entity.ModelConfiguration;
using RingSoft.DbLookup.App.Library.Northwind.Model;

namespace RingSoft.DbLookup.App.Library.Ef6.Northwind.Configurations
{
    public class EmployeeTerritoryConfiguration : EntityTypeConfiguration<EmployeeTerritory>
    {
        public EmployeeTerritoryConfiguration()
        {
            HasKey(p => new {p.EmployeeID, p.TerritoryID});

            HasRequired(p => p.Employee)
                .WithMany(p => p.Territories)
                .HasForeignKey(p => p.EmployeeID);

            HasRequired(p => p.Territory)
                .WithMany(p => p.Employees)
                .HasForeignKey(p => p.TerritoryID);
        }
    }
}
