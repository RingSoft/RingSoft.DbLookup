using System.Data.Entity;
using RingSoft.DbLookup.App.Library.Ef6.MegaDb.Configurations;
using RingSoft.DbLookup.App.Library.MegaDb.Model;

namespace RingSoft.DbLookup.App.Library.Ef6.MegaDb
{
    public class MegaDbDbContextEf6 : DbContext
    {
        public DbSet<Item> Items { get; set; }

        public DbSet<Location> Locations { get; set; }

        public DbSet<Manufacturer> Manufacturers { get; set; }

        public DbSet<StockMaster> Stocks { get; set; }

        public DbSet<StockCostQuantity> StockCostQuantities { get; set; }

        public static MegaDbLookupContextEf6 LookupContext { get; private set; }

        public MegaDbDbContextEf6(MegaDbLookupContextEf6 lookupContext)
        {
            LookupContext = lookupContext;
        }

        public MegaDbDbContextEf6()
            : base(LookupContext.DataProcessor.ConnectionString)
        {
            
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<MegaDbDbContextEf6>(null);

            modelBuilder.Configurations.Add(new StockConfiguration());
            modelBuilder.Configurations.Add(new StockCostQuantityConfiguration());

            modelBuilder.Entity<Item>().Property(p => p.Name).IsRequired();

            modelBuilder.Entity<Item>().HasRequired(p => p.Location)
                .WithMany()
                .HasForeignKey(p => p.LocationId);

            modelBuilder.Entity<Item>().HasRequired(p => p.Manufacturer)
                .WithMany()
                .HasForeignKey(p => p.ManufacturerId);

            modelBuilder.Entity<Location>().Property(p => p.Name).IsRequired();
            modelBuilder.Entity<Manufacturer>().Property(p => p.Name).IsRequired();

            base.OnModelCreating(modelBuilder);
        }
    }
}
