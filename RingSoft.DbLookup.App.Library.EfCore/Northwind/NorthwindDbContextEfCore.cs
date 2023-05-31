using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.App.Library.EfCore.Northwind.Configurations;
using RingSoft.DbLookup.App.Library.LibLookupContext;
using RingSoft.DbLookup.App.Library.Northwind.Model;
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

        public DbSet<RecordLock> RecordLocks { get; set; }

        public DbSet<AdvancedFind.AdvancedFind> AdvancedFinds { get; set; }
        public DbSet<AdvancedFindColumn> AdvancedFindColumns { get; set; }
        public DbSet<AdvancedFindFilter> AdvancedFindFilters { get; set; }

        //public DbContext GetDbContextEf()
        //{
        //    return this;
        //}

        public override IAdvancedFindDbContextEfCore GetNewDbContext()
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
                    DbConstants.ConstantGenerator = new SqliteDbConstants();
                    optionsBuilder.UseSqlite(_lookupContext.NorthwindContextConfiguration.SqliteDataProcessor.ConnectionString);
                    break;
                case DataProcessorTypes.SqlServer:
                    DbConstants.ConstantGenerator = new SqlServerDbConstants();
                    optionsBuilder.UseSqlServer(_lookupContext.NorthwindContextConfiguration.SqlServerDataProcessor.ConnectionString);
                    break;
                case DataProcessorTypes.MySql:
                    DbConstants.ConstantGenerator = new MySqlDbConstants();
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

        //public bool SaveNoCommitEntity<TEntity>(TEntity entity, string message) where TEntity : class
        //{
        //    var context = GetDbContextEf();
        //    if (!context.SaveNoCommitEntity(Set<TEntity>(), entity, message))
        //        return false;

        //    return true;
        //}

        //public bool SaveEntity<TEntity>(TEntity entity, string message) where TEntity : class
        //{
        //    return GetDbContextEf().SaveEntity(Set<TEntity>(), entity, message);
        //}

        //public bool DeleteEntity<TEntity>(TEntity entity, string message) where TEntity : class
        //{
        //    return GetDbContextEf().DeleteEntity(Set<TEntity>(), entity, message);
        //}

        //public bool DeleteNoCommitEntity<TEntity>(TEntity entity, string message) where TEntity : class
        //{
        //    return GetDbContextEf().DeleteNoCommitEntity(Set<TEntity>(), entity, message);
        //}

        //public bool AddNewNoCommitEntity<TEntity>(TEntity entity, string message) where TEntity : class
        //{
        //    return GetDbContextEf().AddNewNoCommitEntity(Set<TEntity>(), entity, message);
        //}

        //public bool Commit(string message)
        //{
        //    var result = GetDbContextEf().SaveEfChanges(message);

        //    return result;
        //}

        //public void RemoveRange<TEntity>(IEnumerable<TEntity> listToRemove) where TEntity : class
        //{
        //    var dbSet = Set<TEntity>();

        //    dbSet.RemoveRange(listToRemove);
        //}

        //public void AddRange<TEntity>(List<TEntity> listToAdd) where TEntity : class
        //{
        //    var dbSet = Set<TEntity>();

        //    dbSet.AddRange(listToAdd);
        //}

        //public IQueryable<TEntity> GetTable<TEntity>() where TEntity : class
        //{
        //    var dbSet = Set<TEntity>();
        //    return dbSet;
        //}
    }
}
