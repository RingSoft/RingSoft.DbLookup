using Microsoft.EntityFrameworkCore;
using RingSoft.DbLookup.App.Library.DevLogix.Model;
using RingSoft.DbLookup.App.Library.EfCore.DevLogix.Configurations;
using RingSoft.DbLookup.EfCore;

namespace RingSoft.DbLookup.App.Library.EfCore.DevLogix
{
    public class DevLogixDbContextEfCore : DbContext
    {
        public DbSet<Error> Errors { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<Task> Tasks { get; set; }

        public DbSet<Issue> Issues { get; set; }

        private DevLogixLookupContextEfCore _lookupContext;

        public DevLogixDbContextEfCore(DevLogixLookupContextEfCore lookupContext)
        {
            _lookupContext = lookupContext;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            DbConstants.ConstantGenerator = new SqliteDbConstants();
            optionsBuilder.UseSqlite(_lookupContext.DevLogixConfiguration.SqliteDataProcessor.ConnectionString);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ErrorConfiguration());
            modelBuilder.ApplyConfiguration(new IssueConfiguration());
            modelBuilder.ApplyConfiguration(new TaskConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
