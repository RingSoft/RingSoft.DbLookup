using RingSoft.DbLookup.App.Library.DevLogix.Model;
using RingSoft.DbLookup.App.Library.LookupContext;
using RingSoft.DbLookup.ModelDefinition;

namespace RingSoft.DbLookup.App.Library.DevLogix
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