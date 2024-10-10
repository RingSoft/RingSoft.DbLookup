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
using System.Windows.Shapes;
using RingSoft.DbLookup.App.WPFCore.Northwind;
using RingSoft.DbLookup.Controls.WPF;

namespace RingSoft.DbLookup.App.WPFCore
{
    /// <summary>
    /// Interaction logic for NewMainWindow.xaml
    /// </summary>
    public partial class NewMainWindow : Window
    {
        public NewMainWindow()
        {
            InitializeComponent();

            Loaded += (sender, args) =>
            {
                var tabItem = new DbMaintenanceTabItem(
                    new OrderDetailsUserControl(), TabControl);
                TabControl.Items.Add(tabItem);

                tabItem = new DbMaintenanceTabItem(
                    new OrdersGridUserControl(),  TabControl);
                TabControl.Items.Add(tabItem);
                tabItem.IsSelected = true;
            };
        }
    }
}
