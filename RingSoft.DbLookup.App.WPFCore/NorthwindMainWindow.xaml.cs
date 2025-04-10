using System.ComponentModel;
using System.Windows.Controls;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup.App.Library;
using RingSoft.DbLookup.App.WPFCore.Northwind;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbLookup.Controls.WPF.AdvancedFind;

namespace RingSoft.DbLookup.App.WPFCore
{
    /// <summary>
    /// Interaction logic for NewMainWindow.xaml
    /// </summary>
    public partial class NorthwindMainWindow
    {
        public RelayCommand OrdersCommand { get; }

        public RelayCommand CustomersCommand { get; }

        public RelayCommand EmployeesCommand { get; }

        public RelayCommand ProductsCommand { get; }

        public RelayCommand CloseAllTabsCommand { get; }

        public RelayCommand AdvFindCommand { get; }

        public RelayCommand DummyCommand { get; }

        public RelayCommand ExitCommand { get; }

        private VmUiControl _lookupUiControl;
        private bool _loaded;
        public NorthwindMainWindow()
        {
            OrdersCommand = new RelayCommand((() =>
            {
                TabControl.ShowTableControl(RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.Orders);
            }));

            CustomersCommand = new RelayCommand((() =>
            {
                TabControl.ShowTableControl(RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.Customers);
            }));

            EmployeesCommand = new RelayCommand((() =>
            {
                TabControl.ShowTableControl(RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.Employees);
            }));

            ProductsCommand = new RelayCommand((() =>
            {
                TabControl.ShowTableControl(RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.Products);
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
                //var advancedFindWindow = new AdvancedFindWindow();
                ////Peter Ringering - 11/23/2024 11:15:19 AM - E-71
                //LookupControlsGlobals.WindowRegistry.ShowDialog(advancedFindWindow);
                TabControl.ShowTableControl(SystemGlobals.LookupContext.AdvancedFinds);
            }));

            DummyCommand = new RelayCommand((() =>
            {
                var dummyUserControl = new DummyUserControl();
                TabControl.ShowUserControl(dummyUserControl, "Dummy", true, false);
            }));

            ExitCommand = new RelayCommand((() =>
            {
                Close();
            }));

            InitializeComponent();
            MainMenu.Items.Add(new MenuItem()
            {
                Header = "_Orders Grid...",
                Command = OrdersCommand,
            });
            MainMenu.Items.Add(new MenuItem()
            {
                Header = "_Customers...",
                Command = CustomersCommand,
            });
            MainMenu.Items.Add(new MenuItem()
            {
                Header = "_Employees...",
                Command = EmployeesCommand,
            });
            MainMenu.Items.Add(new MenuItem()
            {
                Header = "_Products...",
                Command = ProductsCommand,
            });
            MainMenu.Items.Add(new MenuItem()
            {
                Header = "_Advanced Find...",
                Command = AdvFindCommand,
            });
            MainMenu.Items.Add(new MenuItem()
            {
                Header = "_Dummy",
                Command = DummyCommand,
            });

            MainMenu.Items.Add(new WindowMenu());

            MainMenu.Items.Add(new MenuItem()
            {
                Header = "E_xit",
                Command = ExitCommand,
            });

            LookupControlsGlobals.SetTabSwitcherWindow(this, TabControl);
            Loaded += (sender, args) =>
            {
                TabControl.SetDestionationAsFirstTab = false;
                LocalViewModel.Initialize();
                var uControl = TabControl.ShowTableControl(RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.Orders);

                LocalViewModel.OrderLookupDefinition.Destination = TabControl;
                _lookupUiControl = new VmUiControl(OrderLookup, LocalViewModel.LookupUiCommand);

                _lookupUiControl.Command.SetFocus();
                uControl.Loaded += (sender, args) =>
                {
                    if (_loaded)
                    {
                        return;
                    }
                    _lookupUiControl.Command.SetFocus();
                    _loaded = true;
                };
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
