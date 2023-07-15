using Microsoft.EntityFrameworkCore;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.DbLookup.EfCore;
using RingSoft.DbLookup.Tests.Configurations;
using RingSoft.DbLookup.Tests.Model;

namespace RingSoft.DbLookup.Tests
{
    public class TestDbContext : DbContextEfCore
    {
        public DbSet<Customer> Customers { get; set; }

        public DbSet<Error> Errors { get; set; }

        public DbSet<TimeClock> TimeClocks { get; set; }

        public TestDbContext()
        {
            DbConstants.ConstantGenerator = new SqlServerDbConstants();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CustomerConfiguration());
            modelBuilder.ApplyConfiguration(new ErrorConfiguration());
            modelBuilder.ApplyConfiguration(new TimeClockConfiguration());

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("");
            base.OnConfiguring(optionsBuilder);
        }

        public override DbContextEfCore GetNewDbContextEfCore()
        {
            return this;
        }

        public override void SetProcessor(DbDataProcessor processor)
        {
            
        }

        public override void SetConnectionString(string connectionString)
        {
            
        }
    }
}
