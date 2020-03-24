using System.Data.Entity;
using RingSoft.DbLookup.App.Library.DevLogix.Model;
using RingSoft.DbLookup.App.Library.Ef6.DevLogix.Configurations;

namespace RingSoft.DbLookup.App.Library.Ef6.DevLogix
{
    public class DevLogixDbContextEf6 : DbContext
    {
        public DbSet<Error> Errors { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<Task> Tasks { get; set; }

        public DbSet<Issue> Issues { get; set; }

        private DevLogixLookupContextEf6 _lookupContext;

        public DevLogixDbContextEf6(DevLogixLookupContextEf6 lookupContext)
        {
            _lookupContext = lookupContext;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<DevLogixDbContextEf6>(null);

            modelBuilder.Configurations.Add(new ErrorConfiguration());
            modelBuilder.Configurations.Add(new IssueConfiguration());
            modelBuilder.Configurations.Add(new TaskConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
