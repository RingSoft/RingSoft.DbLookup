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
    }
}
