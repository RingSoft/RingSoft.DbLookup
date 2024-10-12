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
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.Controls.WPF.AdvancedFind;

namespace RingSoft.DbLookup.App.WPFCore
{
    /// <summary>
    /// Interaction logic for MegaDbMainWindow.xaml
    /// </summary>
    public partial class MegaDbMainWindow : Window
    {
        public RelayCommand ItemsCommand { get; }

        public RelayCommand StocksCommand { get; }

        public RelayCommand CloseAllTabsCommand { get; }

        public RelayCommand AdvFindCommand { get; }

        public RelayCommand ExitCommand { get; }

        private VmUiControl _lookupUiControl;
        private bool _loaded;

        public MegaDbMainWindow()
        {
            ItemsCommand = new RelayCommand((() =>
            {
                TabControl.ShowTableControl(
                    RsDbLookupAppGlobals
                        .EfProcessor
                        .MegaDbLookupContext
                        .Items);
            }));
            StocksCommand = new RelayCommand((() =>
            {
                TabControl.ShowTableControl(
                    RsDbLookupAppGlobals
                        .EfProcessor
                        .MegaDbLookupContext
                        .StockMasters);
            }));

            CloseAllTabsCommand = new RelayCommand((() =>
            {
                if (TabControl.CloseAllTabs())
                {
                    _lookupUiControl.Command.SetFocus();
                }
            }));

            AdvFindCommand = new RelayCommand((() =>
            {
                var advancedFindWindow = new AdvancedFindWindow();
                LookupControlsGlobals.WindowRegistry.ShowWindow(advancedFindWindow);
            }));

            ExitCommand = new RelayCommand((() =>
            {
                Close();
            }));

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

            MainMenu.Items.Add(new MenuItem()
            {
                Header = "I_tems",
                Command = ItemsCommand,
            });
            MainMenu.Items.Add(new MenuItem()
            {
                Header = "_Stock Master",
                Command = StocksCommand,
            });
            MainMenu.Items.Add(new MenuItem()
            {
                Header = "_Close All Tabs",
                Command = CloseAllTabsCommand,
            });
            MainMenu.Items.Add(new MenuItem()
            {
                Header = "_Advanced Find...",
                Command = AdvFindCommand,
            });

            MainMenu.Items.Add(new MenuItem()
            {
                Header = "E_xit",
                Command = ExitCommand,
            });
            LookupControlsGlobals.SetTabSwitcherWindow(this, TabControl);
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
