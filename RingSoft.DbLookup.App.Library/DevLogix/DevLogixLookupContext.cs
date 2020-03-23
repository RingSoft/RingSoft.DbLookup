using RingSoft.DbLookup.App.Library.DevLogix.Model;
using RingSoft.DbLookup.App.Library.LookupContext;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.QueryBuilder;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace RingSoft.DbLookup.App.Library.DevLogix
{
    public class DevLogixLookupContext : AppLookupContext, IDevLogixLookupContext
    {
        public override AppLookupContextConfiguration LookupContextConfiguration => DevLogixConfiguration;

        public DevLogixLookupContextConfiguration DevLogixConfiguration { get; }

        public TableDefinition<Error> Errors { get; set; }

        public TableDefinition<User> Users { get; set; }

        public TableDefinition<Project> Projects { get; set; }

        public TableDefinition<Task> Tasks { get; set; }

        public TableDefinition<Issue> Issues { get; set; }
        public DevLogixLookupContext()
        {
            DevLogixConfiguration = new DevLogixLookupContextConfiguration(this);
        }

        protected override void InitializeTableDefinitions()
        {
        }

        protected override void InitializeFieldDefinitions()
        {
            InitializeErrorsFields();

            InitializeUsersFields();

            InitializeProjectsFields(Projects);

            InitializeTasksFields();

            InitializeIssuesFields();
        }

        private void InitializeUsersFields()
        {
            Users.GetFieldDefinition(p => p.Notes).IsMemo();
            Users.GetFieldDefinition(p => p.Rights).IsMemo();
        }

        private void InitializeErrorsFields()
        {
            Errors.GetFieldDefinition(p => p.AssignedToId)
                .SetParentField(Users.GetFieldDefinition(p => p.Id));
            Errors.GetFieldDefinition(p => p.TesterId)
                .SetParentField(Users.GetFieldDefinition(p => p.Id));

            InitializeErrorsFields(Errors);
        }

        public static void InitializeErrorsFields(TableDefinition<Error> errorsTableDefinition)
        {
            errorsTableDefinition.GetFieldDefinition(p => p.Date).HasDateType(DbDateTypes.DateTime);
            errorsTableDefinition.GetFieldDefinition(p => p.FixedDate).HasDateType(DbDateTypes.DateTime);
            errorsTableDefinition.GetFieldDefinition(p => p.CompletedDate).HasDateType(DbDateTypes.DateTime);
            errorsTableDefinition.GetFieldDefinition(p => p.Description).IsMemo();
            errorsTableDefinition.GetFieldDefinition(p => p.Resolution).IsMemo();
        }

        public static void InitializeProjectsFields(TableDefinition<Project> projectsTableDefinition)
        {
            projectsTableDefinition.GetFieldDefinition(p => p.Notes).IsMemo();
            projectsTableDefinition.GetFieldDefinition(p => p.Deadline).HasDateType(DbDateTypes.DateTime);
            projectsTableDefinition.GetFieldDefinition(p => p.OriginalDeadline).HasDateType(DbDateTypes.DateTime);
        }

        private void InitializeTasksFields()
        {
            Tasks.GetFieldDefinition(p => p.ProjectId)
                .SetParentField(Projects.GetFieldDefinition(p => p.Id));
            Tasks.GetFieldDefinition(p => p.AssignedToId)
                .SetParentField(Users.GetFieldDefinition(p => p.Id));

            InitializeTasksFields(Tasks);
        }

        public static void InitializeTasksFields(TableDefinition<Task> tasksTableDefinition)
        {
            tasksTableDefinition.GetFieldDefinition(p => p.DueDate).HasDateType(DbDateTypes.DateTime);
            tasksTableDefinition.GetFieldDefinition(p => p.CompletedDate).HasDateType(DbDateTypes.DateTime);
            tasksTableDefinition.GetFieldDefinition(p => p.Notes).IsMemo();
            tasksTableDefinition.GetFieldDefinition(p => p.PercentComplete)
                .HasDecimalFieldType(DecimalFieldTypes.Percent).HasDecimalCount(0);
        }

        private void InitializeIssuesFields()
        {
            Issues.GetFieldDefinition(p => p.TaskId).IsRequired()
                .SetParentField(Tasks.GetFieldDefinition(p => p.Id));

            InitializeIssuesFields(Issues);
        }

        public static void InitializeIssuesFields(TableDefinition<Issue> issuesTableDefinition)
        {
            issuesTableDefinition.GetFieldDefinition(p => p.ResolvedDate).HasDateType(DbDateTypes.DateTime);
            issuesTableDefinition.GetFieldDefinition(p => p.Notes).IsMemo();
            issuesTableDefinition.GetFieldDefinition(p => p.IsResolved).HasTrueFalseText("Yes", "No");
        }

        protected override void InitializePrimaryKeys()
        {
            Errors.AddPropertyToPrimaryKey(p => p.Id);
            Users.AddPropertyToPrimaryKey(p => p.Id);
            Projects.AddPropertyToPrimaryKey(p => p.Id);
            Tasks.AddPropertyToPrimaryKey(p => p.Id);
            Issues.AddPropertyToPrimaryKey(p => p.Id);
        }

        protected override void InitializeLookupDefinitions()
        {
            DevLogixConfiguration.ConfigureLookups();
        }
    }
}
