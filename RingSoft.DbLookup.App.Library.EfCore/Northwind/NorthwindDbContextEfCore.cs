using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.App.Library.EfCore.Northwind.Configurations;
using RingSoft.DbLookup.App.Library.LibLookupContext;
using RingSoft.DbLookup.App.Library.Northwind.Model;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.EfCore;
using RingSoft.DbLookup.RecordLocking;

namespace RingSoft.DbLookup.App.Library.EfCore.Northwind
{
    public class NorthwindDbContextEfCore : DbContextEfCore
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

        private static NorthwindLookupContextEfCore _lookupContext;

        private string? _connectionString;

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
                    DbConstants.ConstantGenerator = new SqliteDbConstants();
                    if (_connectionString == null)
                    {
                        optionsBuilder.UseSqlite(_lookupContext.NorthwindContextConfiguration.SqliteDataProcessor.ConnectionString);
                    }
                    else
                    {
                        optionsBuilder.UseSqlite(_connectionString);
                    }
                    break;
                case DataProcessorTypes.SqlServer:
                    DbConstants.ConstantGenerator = new SqlServerDbConstants();
                    if (_connectionString == null)
                    {
                        optionsBuilder.UseSqlServer(_lookupContext.NorthwindContextConfiguration.SqlServerDataProcessor.ConnectionString);
                    }
                    else
                    {
                        optionsBuilder.UseSqlServer(_connectionString);
                    }
                    break;
                case DataProcessorTypes.MySql:
                    DbConstants.ConstantGenerator = new MySqlDbConstants();
                    if (_connectionString == null)
                    {
                        optionsBuilder.UseMySQL(_lookupContext.NorthwindContextConfiguration.MySqlDataProcessor.ConnectionString);
                    }
                    else
                    {
                        optionsBuilder.UseMySQL(_connectionString);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EmployeeConiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new OrderDetailConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeTerritoryConfiguration());
            modelBuilder.ApplyConfiguration(new CustomerCustomerDemoConfiguration());

            modelBuilder.Entity<Territory>().HasOne(p => p.Region)
                .WithMany(p => p.Territories)
                .HasForeignKey(p => p.RegionID);

            ConfigureAdvancedFind(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        public override DbContextEfCore GetNewDbContextEfCore()
        {
            return new NorthwindDbContextEfCore();
        }

        public override void SetProcessor(DbDataProcessor processor)
        {
            if (processor is SqliteDataProcessor sqliteDataProcessor)
            {
                _lookupContext.DataProcessorType = DataProcessorTypes.Sqlite;
            }
            else if (processor is SqlServerDataProcessor sqlServerDataProcessor)
            {
                _lookupContext.DataProcessorType = DataProcessorTypes.SqlServer;
            }
            else
            {
                _lookupContext.DataProcessorType = DataProcessorTypes.MySql;
            }

        }

        public override void SetConnectionString(string? connectionString)
        {
            _connectionString = connectionString;
        }
    }
}
