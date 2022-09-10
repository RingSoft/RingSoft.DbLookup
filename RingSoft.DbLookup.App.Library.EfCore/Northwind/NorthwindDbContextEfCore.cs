using System;
using Microsoft.EntityFrameworkCore;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.App.Library.EfCore.Northwind.Configurations;
using RingSoft.DbLookup.App.Library.LibLookupContext;
using RingSoft.DbLookup.App.Library.Northwind.Model;
using RingSoft.DbLookup.EfCore;

namespace RingSoft.DbLookup.App.Library.EfCore.Northwind
{
    public class NorthwindDbContextEfCore : DbContext, IAdvancedFindDbContextEfCore
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

        public DbSet<AdvancedFind.AdvancedFind> AdvancedFinds { get; set; }
        public DbSet<AdvancedFindColumn> AdvancedFindColumns { get; set; }
        public DbSet<AdvancedFindFilter> AdvancedFindFilters { get; set; }

        public DbContext GetDbContextEf()
        {
            return this;
        }

        public IAdvancedFindDbContextEfCore GetNewDbContext()
        {
            return new NorthwindDbContextEfCore();
        }

        private static NorthwindLookupContextEfCore _lookupContext;

        public NorthwindDbContextEfCore(NorthwindLookupContextEfCore lookupContext)
        {
            _lookupContext = lookupContext;
        }

        public NorthwindDbContextEfCore()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            switch (_lookupContext.DataProcessorType)
            {
                case DataProcessorTypes.Sqlite:
                    optionsBuilder.UseSqlite(_lookupContext.NorthwindContextConfiguration.SqliteDataProcessor.ConnectionString);
                    break;
                case DataProcessorTypes.SqlServer:
                    optionsBuilder.UseSqlServer(_lookupContext.NorthwindContextConfiguration.SqlServerDataProcessor.ConnectionString);
                    break;
                case DataProcessorTypes.MySql:
                    optionsBuilder.UseMySQL(_lookupContext.NorthwindContextConfiguration.MySqlDataProcessor.ConnectionString);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new OrderDetailConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeTerritoryConfiguration());
            modelBuilder.ApplyConfiguration(new CustomerCustomerDemoConfiguration());

            modelBuilder.Entity<Territory>().HasOne(p => p.Region)
                .WithMany(p => p.Territories)
                .HasForeignKey(p => p.RegionID);

            modelBuilder.Entity<Employee>().HasOne(p => p.Employee1)
                .WithMany(p => p.Employees1)
                .HasForeignKey(p => p.ReportsTo);

            AdvancedFindDataProcessorEfCore.ConfigureAdvancedFind(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }
    }
}
