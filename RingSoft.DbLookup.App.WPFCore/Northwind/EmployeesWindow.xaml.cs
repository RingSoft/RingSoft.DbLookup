using System.Windows;
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
        }
    }
}
