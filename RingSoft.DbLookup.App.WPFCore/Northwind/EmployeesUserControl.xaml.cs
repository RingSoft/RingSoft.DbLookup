using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.App.WPFCore.Northwind
{
    /// <summary>
    /// Interaction logic for EmployeesUserControl.xaml
    /// </summary>
    public partial class EmployeesUserControl
    {
        public EmployeesUserControl()
        {
            InitializeComponent();
        }

        protected override DbMaintenanceViewModelBase OnGetViewModel()
        {
            return EmployeeViewModel;
        }

        protected override Control OnGetMaintenanceButtons()
        {
            return ButtonsControl;
        }

        protected override DbMaintenanceStatusBar OnGetStatusBar()
        {
            return StatusBar;
        }

        protected override string GetTitle()
        {
            return "Employees";
        }

        public override void SetInitialFocus()
        {
            EmployeeViewModel.FirstNameUiCommand.SetFocus();
        }

        protected override void ShowRecordTitle()
        {
            Host.ChangeTitle($"{Title} - {EmployeeViewModel.FirstName} {EmployeeViewModel.LastName}");
        }
    }
}
