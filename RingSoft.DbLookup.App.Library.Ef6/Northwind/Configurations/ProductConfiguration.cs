using System.Data.Entity.ModelConfiguration;
using RingSoft.DbLookup.App.Library.Northwind.Model;

namespace RingSoft.DbLookup.App.Library.Ef6.Northwind.Configurations
{
    public class ProductConfiguration : EntityTypeConfiguration<Product>
    {
        public ProductConfiguration()
        {
            HasRequired(p => p.Category)
                .WithMany(p => p.Products)
                .HasForeignKey(p => p.CategoryID);

            HasRequired(p => p.Supplier)
                .WithMany(p => p.Products)
                .HasForeignKey(p => p.SupplierID);
        }
    }
}
