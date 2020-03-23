using RingSoft.DbLookup.ModelDefinition;
using RSDbLookupApp.Library.DevLogix.Model;
using RSDbLookupApp.Library.Northwind.Model;

namespace RSDbLookupApp.Library.DevLogix
{
    public interface IDevLogixLookupContext : IAppLookupContext
    {
        DevLogixLookupContextConfiguration DevLogixConfiguration { get; }

        TableDefinition<Error> Errors { get; set; }

        TableDefinition<User> Users { get; set; }

        TableDefinition<Project> Projects { get; set; }

        TableDefinition<Task> Tasks { get; set; }

        TableDefinition<Issue> Issues { get; set; }
    }
}