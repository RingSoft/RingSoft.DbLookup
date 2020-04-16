using RingSoft.DbLookup.App.Library;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.App.WPFCore.Northwind
{
    /// <summary>
    /// Interaction logic for EmployeesWindow.xaml
    /// </summary>
    public partial class EmployeesWindow
    {
        public override DbMaintenanceViewModelBase ViewModel => EmployeeViewModel;
        public override DbMaintenanceButtonsControl MaintenanceButtonsControl => ButtonsControl;

        public EmployeesWindow()
        {
            InitializeComponent();

            Initialize();

            AddModifyButton.Click += (sender, args) => EmployeeViewModel.OnAddModify();
        }
        public override void ResetViewForNewRecord()
        {
            TabControl.SelectedIndex = 0;
            FirstNameTextBox.Focus();
            base.ResetViewForNewRecord();
        }

        public override void OnValidationFail(FieldDefinition fieldDefinition, string text, string caption)
        {
            var table = RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.Employees;

            if (fieldDefinition == table.GetFieldDefinition(p => p.FirstName))
                FirstNameTextBox.Focus();
            else if (fieldDefinition == table.GetFieldDefinition(p => p.LastName))
                LastNameTextBox.Focus();
            else if (fieldDefinition == table.GetFieldDefinition(p => p.ReportsTo))
                SupervisorControl.Focus();

            base.OnValidationFail(fieldDefinition, text, caption);
        }
    }
}
