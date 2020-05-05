using Microsoft.EntityFrameworkCore;
using RingSoft.SimpleDemo.WPF.Northwind.Configurations;
using RingSoft.SimpleDemo.WPF.Northwind.Model;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace RingSoft.SimpleDemo.WPF.Northwind
{
    public class NorthwindDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Order_Detail> OrderDetails { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Shipper> Shippers { get; set; }

        private static NorthwindLookupContext _lookupContext;

        public NorthwindDbContext(NorthwindLookupContext lookupContext)
        {
            _lookupContext = lookupContext;
        }

        public NorthwindDbContext()
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_lookupContext.DataProcessor.ConnectionString);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new OrderDetailConfiguration());

            modelBuilder.Entity<Employee>().HasOne(p => p.Employee1)
                .WithMany(p => p.Employees1)
                .HasForeignKey(p => p.ReportsTo);

            base.OnModelCreating(modelBuilder);
        }
    }
}
