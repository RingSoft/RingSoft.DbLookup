using Microsoft.EntityFrameworkCore;
using RingSoft.DbLookup.App.Library.LookupContext;
using RingSoft.DbLookup.App.Library.MegaDb.Model;
using RSDbLookupApp.Library.EfCore.MegaDb.Configurations;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RSDbLookupApp.Library.EfCore.MegaDb
{
    public class MegaDbDbContextEfCore : DbContext
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
                    optionsBuilder.UseSqlite(LookupContext.MegaDbContextConfiguration.SqliteDataProcessor.ConnectionString);
                    break;
                case DataProcessorTypes.SqlServer:
                    optionsBuilder.UseSqlServer(LookupContext.MegaDbContextConfiguration.SqlServerDataProcessor.ConnectionString);
                    break;
                case DataProcessorTypes.MySql:
                    optionsBuilder.UseMySql(LookupContext.MegaDbContextConfiguration.MySqlDataProcessor.ConnectionString);
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

            base.OnModelCreating(modelBuilder);
        }

        public async new Task<int> SaveChangesAsync(CancellationToken token)
        {
            var result = 0;
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
    }
}
