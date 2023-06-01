using Microsoft.EntityFrameworkCore;
using RingSoft.DbLookup.App.Library.EfCore.MegaDb.Configurations;
using RingSoft.DbLookup.App.Library.MegaDb.Model;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.App.Library.LibLookupContext;
using RingSoft.DbLookup.EfCore;
using RingSoft.DbLookup.RecordLocking;
using System.Linq;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace RingSoft.DbLookup.App.Library.EfCore.MegaDb
{
    public class MegaDbDbContextEfCore : DbContextEfCore
    {
        public DbSet<Item> Items { get; set; }

        public DbSet<Location> Locations { get; set; }

        public DbSet<Manufacturer> Manufacturers { get; set; }

        public static MegaDbLookupContextEfCore LookupContext { get; private set; }

        public DbSet<StockMaster> Stocks { get; set; }

        public DbSet<StockCostQuantity> StockCostQuantities { get; set; }

        internal MegaDbDbContextEfCore(MegaDbLookupContextEfCore lookupContext)
        {
            LookupContext = lookupContext;
        }

        public MegaDbDbContextEfCore()
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            switch (LookupContext.DataProcessorType)
            {
                case DataProcessorTypes.Sqlite:
                    DbConstants.ConstantGenerator = new SqliteDbConstants();
                    optionsBuilder.UseSqlite(LookupContext.MegaDbContextConfiguration.SqliteDataProcessor.ConnectionString);
                    break;
                case DataProcessorTypes.SqlServer:
                    DbConstants.ConstantGenerator = new SqlServerDbConstants();
                    optionsBuilder.UseSqlServer(LookupContext.MegaDbContextConfiguration.SqlServerDataProcessor.ConnectionString);
                    break;
                case DataProcessorTypes.MySql:
                    DbConstants.ConstantGenerator = new MySqlDbConstants();
                    optionsBuilder.UseMySQL(LookupContext.MegaDbContextConfiguration.MySqlDataProcessor.ConnectionString);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new StockConfiguration());
            modelBuilder.ApplyConfiguration(new StockCostQuantityConfiguration());

            modelBuilder.Entity<Item>().Property(p => p.Name).IsRequired();

            modelBuilder.Entity<Item>().HasOne(p => p.Location)
                .WithMany()
                .HasForeignKey(p => p.LocationId);

            modelBuilder.Entity<Item>().HasOne(p => p.Manufacturer)
                .WithMany()
                .HasForeignKey(p => p.ManufacturerId);

            modelBuilder.Entity<Location>().Property(p => p.Name).IsRequired();
            modelBuilder.Entity<Manufacturer>().Property(p => p.Name).IsRequired();

            AdvancedFindDataProcessorEfCore.ConfigureAdvancedFind(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        public async new Task<int> SaveChangesAsync(CancellationToken token)
        {
            int result;
            try
            {
                result = await base.SaveChangesAsync(token);
            }
            catch (Exception)
            {
                result = 0;
            }
            return result;
        }

        public DbContext GetDbContextEf()
        {
            return this;
        }

        public override DbContextEfCore GetNewDbContextEfCore()
        {
            return new MegaDbDbContextEfCore();
        }
    }
}
