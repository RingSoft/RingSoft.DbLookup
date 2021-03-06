﻿using System.Data.Entity;
using RingSoft.DbLookup.App.Library.DevLogix;
using RingSoft.DbLookup.App.Library.DevLogix.Model;
using RingSoft.DbLookup.App.Library.LibLookupContext;
using RingSoft.DbLookup.ModelDefinition;

namespace RingSoft.DbLookup.App.Library.Ef6.DevLogix
{
    public class DevLogixLookupContextEf6 : AppLookupContextEf6, IDevLogixLookupContext
    {
        protected override DbContext DbContext => DevLogixDbContext;

        public override AppLookupContextConfiguration LookupContextConfiguration => DevLogixConfiguration;

        public DevLogixLookupContextConfiguration DevLogixConfiguration { get; }

        internal DevLogixDbContextEf6 DevLogixDbContext { get; }
        public TableDefinition<Error> Errors { get; set; }
        public TableDefinition<User> Users { get; set; }
        public TableDefinition<Project> Projects { get; set; }
        public TableDefinition<Task> Tasks { get; set; }
        public TableDefinition<Issue> Issues { get; set; }

        public DevLogixLookupContextEf6()
        {
            DevLogixConfiguration = new DevLogixLookupContextConfiguration(this);
            DevLogixDbContext = new DevLogixDbContextEf6();
            Initialize();
        }

        protected override void SetupModel()
        {
            DevLogixConfiguration.InitializeModel();
        }

        public bool ValidateRegistryDbConnectionSettings(RegistrySettings registrySettings)
        {
            return true;
        }

        protected override void InitializeLookupDefinitions()
        {
            DevLogixConfiguration.ConfigureLookups();
        }
    }
}
