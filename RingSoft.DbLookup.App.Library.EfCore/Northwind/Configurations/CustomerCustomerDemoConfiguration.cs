using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RingSoft.DbLookup.App.Library.Northwind.Model;

namespace RingSoft.DbLookup.App.Library.EfCore.Northwind.Configurations
{
    public class CustomerCustomerDemoConfiguration :  IEntityTypeConfiguration<CustomerCustomerDemo>
    {
        public void Configure(EntityTypeBuilder<CustomerCustomerDemo> builder)
        {
            builder.HasKey(p => new {p.CustomerID, p.CustomerTypeID});

            builder.HasOne(p => p.Customer)
                .WithMany(p => p.CustomerDemographics)
                .HasForeignKey(p => p.CustomerID);

            builder.HasOne(p => p.CustomerDemographic)
                .WithMany(p => p.Customers)
                .HasForeignKey(p => p.CustomerTypeID);
        }
    }
}
