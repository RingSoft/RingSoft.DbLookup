using System.Windows.Controls;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup.App.Library;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbLookup.Controls.WPF.AdvancedFind;

namespace RingSoft.DbLookup.App.WPFCore
{
    /// <summary>
    /// Interaction logic for NewMainWindow.xaml
    /// </summary>
    public partial class NewMainWindow
    {
        public RelayCommand OrdersCommand { get; }

        public RelayCommand CustomersCommand { get; }

        public RelayCommand AdvFindCommand { get; }

        private VmUiControl _lookupUiControl;
        private bool _loaded;
        public NewMainWindow()
        {
            OrdersCommand = new RelayCommand((() =>
            {
                TabControl.ShowTableControl(RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.Orders);
            }));

            CustomersCommand = new RelayCommand((() =>
            {
                TabControl.ShowTableControl(RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.Customers);
            }));

            AdvFindCommand = new RelayCommand((() =>
            {
                var advancedFindWindow = new AdvancedFindWindow();
                LookupControlsGlobals.WindowRegistry.ShowWindow(advancedFindWindow);
            }));

            InitializeComponent();
            MainMenu.Items.Add(new MenuItem()
            {
                Header = "_Order Grid...",
                Command = OrdersCommand,
            });
            MainMenu.Items.Add(new MenuItem()
            {
                Header = "_Customers...",
                Command = CustomersCommand,
            });
            MainMenu.Items.Add(new MenuItem()
            {
                Header = "_Advanced Find...",
                Command = AdvFindCommand,
            });

            LookupControlsGlobals.SetTabSwitcherWindow(this, TabControl);
            Loaded += (sender, args) =>
            {
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
    }
}
