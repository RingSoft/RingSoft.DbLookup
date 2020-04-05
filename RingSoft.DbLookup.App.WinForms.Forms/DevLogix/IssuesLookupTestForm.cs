using RingSoft.DbLookup.Controls.WinForms;

namespace RingSoft.DbLookup.App.WinForms.Forms.DevLogix
{
    public partial class IssuesLookupTestForm : BaseForm
    {
        public IssuesLookupTestForm()
        {
            InitializeComponent();

            CloseButton.Click += (sender, args) => Close();

            var issuesLookup = WinFormsAppStart.DevLogixLookupContext.DevLogixConfiguration.IssuesLookup;
            //issuesLookup.FilterDefinition.AddFixedFilter(p => p.IsResolved, Conditions.Equals, true);
            IssuesLookupControl.LookupDefinition = issuesLookup;
            IssuesLookupControl.RefreshData(true);
            //DbDataProcessor.ShowSQLWindow();
        }
    }
}
