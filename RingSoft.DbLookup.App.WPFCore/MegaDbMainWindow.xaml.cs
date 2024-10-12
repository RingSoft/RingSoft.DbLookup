using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup.App.Library.Northwind.LookupModel;
using RingSoft.DbLookup.App.Library;
using RingSoft.DbLookup.Controls.WPF;
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
using System.ComponentModel;

namespace RingSoft.DbLookup.App.WPFCore
{
    /// <summary>
    /// Interaction logic for MegaDbMainWindow.xaml
    /// </summary>
    public partial class MegaDbMainWindow : Window
    {
        private VmUiControl _lookupUiControl;
        private bool _loaded;

        public MegaDbMainWindow()
        {
            InitializeComponent();
            LookupControlsGlobals.SetTabSwitcherWindow(this, TabControl);
            Loaded += (sender, args) =>
            {
                LocalViewModel.Initialize();
                //var uControl = TabControl.ShowTableControl(RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.Orders);

                LocalViewModel.ItemLookupDefinition.Destination = TabControl;
                LocalViewModel.StockLookupDefinition.Destination = TabControl;
                _lookupUiControl = new VmUiControl(ItemLookup, LocalViewModel.LookupUiCommand);

                _lookupUiControl.Command.SetFocus();
                //uControl.Loaded += (sender, args) =>
                //{
                //    if (_loaded)
                //    {
                //        return;
                //    }
                //    _lookupUiControl.Command.SetFocus();
                //    _loaded = true;
                //};
            };
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (!TabControl.CloseAllTabs())
            {
                e.Cancel = true;
            }

            base.OnClosing(e);
        }
    }
}
