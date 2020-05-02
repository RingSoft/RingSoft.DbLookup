using System.Data.Entity;
using RingSoft.DbLookup.App.Library.Ef6.Northwind.Configurations;
using RingSoft.DbLookup.App.Library.Northwind.Model;

namespace RingSoft.DbLookup.App.Library.Ef6.Northwind
{
    public class NorthwindDbContextEf6 : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeTerritory> EmployeeTerritories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Order_Detail> OrderDetails { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Shipper> Shippers { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Territory> Territories { get; set; }

        private static NorthwindLookupContextEf6 _lookupContext;

        public NorthwindDbContextEf6(NorthwindLookupContextEf6 lookupContext)
        {
            _lookupContext = lookupContext;
        }

        public NorthwindDbContextEf6() 
            : base(_lookupContext.DataProcessor.ConnectionString)
        {
            
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<NorthwindDbContextEf6>(null);

            modelBuilder.Configurations.Add(new OrderConfiguration());
            modelBuilder.Configurations.Add(new ProductConfiguration());
            modelBuilder.Configurations.Add(new OrderDetailConfiguration());
            modelBuilder.Configurations.Add(new EmployeeTerritoryConfiguration());
            modelBuilder.Configurations.Add(new CustomerCustomerDemoConfiguration());

            modelBuilder.Entity<Territory>().HasRequired(p => p.Region)
                .WithMany(p => p.Territories)
                .HasForeignKey(p => p.RegionID);

            modelBuilder.Entity<Employee>().HasOptional(p => p.Employee1)
                .WithMany(p => p.Employees1)
                .HasForeignKey(p => p.ReportsTo);


            base.OnModelCreating(modelBuilder);
        }
    }
}
