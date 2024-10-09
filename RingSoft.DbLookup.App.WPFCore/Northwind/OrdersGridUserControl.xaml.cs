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
    /// Interaction logic for OrdersGridUserControl.xaml
    /// </summary>
    public partial class OrdersGridUserControl
    {
        public OrdersGridUserControl()
        {
            InitializeComponent();
            OrdersViewModel.GridMode = true;
        }

        protected override DbMaintenanceViewModelBase OnGetViewModel()
        {
            return OrdersViewModel;
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
