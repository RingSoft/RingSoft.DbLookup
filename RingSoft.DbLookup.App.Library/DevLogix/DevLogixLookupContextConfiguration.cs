using RingSoft.DbLookup.App.Library.DevLogix.LookupModel;
using RingSoft.DbLookup.App.Library.DevLogix.Model;
using RingSoft.DbLookup.App.Library.LibLookupContext;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.QueryBuilder;

namespace RingSoft.DbLookup.App.Library.DevLogix
{
    public class DevLogixLookupContextConfiguration : AppLookupContextConfiguration
    {
        private IDevLogixLookupContext _lookupContext;

        public LookupDefinition<ErrorLookup, Error> ErrorsLookup { get; set; }

        public LookupDefinition<IssueLookup, Issue> IssuesLookup { get; set; }

        public DevLogixLookupContextConfiguration(IDevLogixLookupContext lookupContext)
        {
            _lookupContext = lookupContext;
            Reinitialize();
        }

        public override void Reinitialize(RegistrySettings registrySettings)
        {
            SqliteDataProcessor.FilePath = $"{RsDbLookupAppGlobals.AssemblyDirectory}\\DevLogix";
            SqliteDataProcessor.FileName = "DevLogix.sqlite";

            SqlServerDataProcessor.Database = "DevLogix";

            MySqlDataProcessor.Database = "devlogix";

            base.Reinitialize(registrySettings);
        }

        public void ConfigureLookups()
        {
            ErrorsLookup = new LookupDefinition<ErrorLookup, Error>(_lookupContext.Errors);
            ErrorsLookup.AddVisibleColumnDefinition(p => p.ErrorNumber, "Error Number", p => p.Number, 25);
            ErrorsLookup.AddVisibleColumnDefinition(p => p.Id, "Error ID", p => p.Id, 10);
            ErrorsLookup.Include(p => p.AssignedToUser)
                .AddHiddenColumn(p => p.DevTypeEnum, p => p.DeveloperType);
            ErrorsLookup.AddVisibleColumnDefinition(p => p.Date, "Date", p => p.Date, 25);
            ErrorsLookup.AddVisibleColumnDefinition(p => p.HoursSpent, "Hours Spent", p => p.HoursSpent, 15);
            ErrorsLookup.Include(p => p.AssignedToUser)
                .AddVisibleColumnDefinition( p => p.DeveloperType, "Developer Type", p => p.DeveloperType, 25);

            _lookupContext.Errors.HasLookupDefinition(ErrorsLookup);

            IssuesLookup = new LookupDefinition<IssueLookup, Issue>(_lookupContext.Issues);
            IssuesLookup.AddVisibleColumnDefinition(p => p.Description, "Description", p => p.Description, 30);
            IssuesLookup.AddVisibleColumnDefinition(p => p.Id, "Issue ID", p => p.Id, 10);
            IssuesLookup.Include(p => p.Task).Include(p => p.Project)
                .AddVisibleColumnDefinition(p => p.Project, "Project", p => p.Name, 15);

            IssuesLookup.Include(p => p.TaskId)
                .AddVisibleColumnDefinition(p => p.Task, "Task", _lookupContext.Tasks.GetFieldDefinition(p => p.Name), 15);

            IssuesLookup.AddVisibleColumnDefinition(p => p.Resolved, "Resolved?", p => p.IsResolved, 12);
            IssuesLookup.Include(p => p.Task)
                .AddVisibleColumnDefinition(p => p.TaskPercentComplete, "Task % Complete", p => p.PercentComplete, 18);

            _lookupContext.Issues.HasLookupDefinition(IssuesLookup);
        }

        public void InitializeModel()
        {
            _lookupContext.Users.GetFieldDefinition(p => p.Notes).IsMemo();
            _lookupContext.Users.GetFieldDefinition(p => p.Rights).IsMemo();

            _lookupContext.Errors.GetFieldDefinition(p => p.Date).HasDateType(DbDateTypes.DateTime);
            _lookupContext.Errors.GetFieldDefinition(p => p.FixedDate).HasDateType(DbDateTypes.DateTime);
            _lookupContext.Errors.GetFieldDefinition(p => p.CompletedDate).HasDateType(DbDateTypes.DateTime);
            _lookupContext.Errors.GetFieldDefinition(p => p.Description).IsMemo();
            _lookupContext.Errors.GetFieldDefinition(p => p.Resolution).IsMemo();

            _lookupContext.Projects.GetFieldDefinition(p => p.Notes).IsMemo();
            _lookupContext.Projects.GetFieldDefinition(p => p.Deadline).HasDateType(DbDateTypes.DateTime);
            _lookupContext.Projects.GetFieldDefinition(p => p.OriginalDeadline).HasDateType(DbDateTypes.DateTime);

            _lookupContext.Tasks.GetFieldDefinition(p => p.DueDate).HasDateType(DbDateTypes.DateTime);
            _lookupContext.Tasks.GetFieldDefinition(p => p.CompletedDate).HasDateType(DbDateTypes.DateTime);
            _lookupContext.Tasks.GetFieldDefinition(p => p.Notes).IsMemo();
            _lookupContext.Tasks.GetFieldDefinition(p => p.PercentComplete)
                .HasDecimalFieldType(DecimalFieldTypes.Percent).HasDecimalCount(0);

            _lookupContext.Issues.GetFieldDefinition(p => p.ResolvedDate).HasDateType(DbDateTypes.DateTime);
            _lookupContext.Issues.GetFieldDefinition(p => p.Notes).IsMemo();
            _lookupContext.Issues.GetFieldDefinition(p => p.IsResolved).HasTrueFalseText("Yes", "No");
        }
    }
}
