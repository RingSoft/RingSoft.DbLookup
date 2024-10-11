using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup.App.Library;
using RingSoft.DbLookup.Controls.WPF;

namespace RingSoft.DbLookup.App.WPFCore
{
    /// <summary>
    /// Interaction logic for NewMainWindow.xaml
    /// </summary>
    public partial class NewMainWindow
    {
        private VmUiControl _lookupUiControl;
        private bool _loaded;
        public NewMainWindow()
        {
            InitializeComponent();
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
