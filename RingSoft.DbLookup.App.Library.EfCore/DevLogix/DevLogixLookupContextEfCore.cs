using Microsoft.EntityFrameworkCore;
using RingSoft.DbLookup.App.Library.DevLogix;
using RingSoft.DbLookup.App.Library.DevLogix.Model;
using RingSoft.DbLookup.App.Library.LookupContext;
using RingSoft.DbLookup.ModelDefinition;

namespace RingSoft.DbLookup.App.Library.EfCore.DevLogix
{
    public class DevLogixLookupContextEfCore : AppLookupContextEfCore, IDevLogixLookupContext
    {
        public override AppLookupContextConfiguration LookupContextConfiguration => DevLogixConfiguration;

        public DevLogixLookupContextConfiguration DevLogixConfiguration { get; }

        protected override DbContext DbContext => DevLogixDbContext;

        internal DevLogixDbContextEfCore DevLogixDbContext { get; }

        public TableDefinition<Error> Errors { get; set; }

        public TableDefinition<User> Users { get; set; }

        public TableDefinition<Project> Projects { get; set; }

        public TableDefinition<Task> Tasks { get; set; }

        public TableDefinition<Issue> Issues { get; set; }
        public DevLogixLookupContextEfCore()
        {
            DevLogixConfiguration = new DevLogixLookupContextConfiguration(this);
            DevLogixDbContext = new DevLogixDbContextEfCore(this);
            Initialize();
        }

        protected override void InitializeLookupDefinitions()
        {
            DevLogixConfiguration.ConfigureLookups();
        }

        protected override void InitializeTableDefinitions()
        {
            
        }

        protected override void InitializeFieldDefinitions()
        {
            DevLogixConfiguration.InitializeModel();
        }

        public bool ValidateRegistryDbConnectionSettings(RegistrySettings registrySettings)
        {
            return true;
        }
    }
}
