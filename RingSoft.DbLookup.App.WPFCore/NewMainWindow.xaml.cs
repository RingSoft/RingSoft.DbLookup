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
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup.App.WPFCore.Northwind;
using RingSoft.DbLookup.Controls.WPF;

namespace RingSoft.DbLookup.App.WPFCore
{
    /// <summary>
    /// Interaction logic for NewMainWindow.xaml
    /// </summary>
    public partial class NewMainWindow : Window
    {
        private VmUiControl _lookupUiControl;
        public NewMainWindow()
        {
            InitializeComponent();

            Loaded += (sender, args) =>
            {
                LocalViewModel.Initialize();
                var tabItem = new DbMaintenanceTabItem(
                    new OrderDetailsUserControl(), TabControl);
                TabControl.Items.Add(tabItem);

                var uControl = new OrdersGridUserControl();
                tabItem = new DbMaintenanceTabItem(
                    uControl, TabControl);
                TabControl.Items.Add(tabItem);
                tabItem.IsSelected = true;

                _lookupUiControl = new VmUiControl(OrderLookup, LocalViewModel.LookupUiCommand);

                uControl.Loaded += (sender, args) =>
                {
                    _lookupUiControl.Command.SetFocus();
                };
            };
        }
    }
}
