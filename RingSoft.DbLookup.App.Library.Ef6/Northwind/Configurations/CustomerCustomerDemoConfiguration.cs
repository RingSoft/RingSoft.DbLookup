using System.Data.Entity.ModelConfiguration;
using RingSoft.DbLookup.App.Library.Northwind.Model;

namespace RingSoft.DbLookup.App.Library.Ef6.Northwind.Configurations
{
    public class CustomerCustomerDemoConfiguration :  EntityTypeConfiguration<CustomerCustomerDemo>
    {
        public CustomerCustomerDemoConfiguration()
        {
            HasKey(p => new {p.CustomerID, p.CustomerTypeID});

            HasRequired(p => p.Customer)
                .WithMany(p => p.CustomerDemographics)
                .HasForeignKey(p => p.CustomerID);

            HasRequired(p => p.CustomerDemographic)
                .WithMany(p => p.Customers)
                .HasForeignKey(p => p.CustomerTypeID);
        }
    }
}
