using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbMaintenance;
using System;
using System.Windows.Controls;

namespace RingSoft.DbLookup.App.WPFCore.Northwind
{
    /// <summary>
    /// Interaction logic for CustomersUserControl.xaml
    /// </summary>
    public partial class CustomersUserControl
    {
        public CustomersUserControl()
        {
            InitializeComponent();
            RegisterFormKeyControl(CustomerControl);
        }

        protected override DbMaintenanceViewModelBase OnGetViewModel()
        {
            return CustomersViewModel;
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
            return "Customers";
        }

        public override void SetInitialFocus()
        {
            if (CustomersViewModel.KeyAutoFillUiCommand.IsEnabled)
            {
                CustomersViewModel.KeyAutoFillUiCommand.SetFocus();
                return;
            }
            CustomersViewModel.CompanyNameUiCommand.SetFocus();
        }
    }
}
