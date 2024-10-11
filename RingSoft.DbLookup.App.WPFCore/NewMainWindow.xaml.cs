using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup.App.Library;
using System.Windows.Controls;

namespace RingSoft.DbLookup.App.WPFCore
{
    /// <summary>
    /// Interaction logic for NewMainWindow.xaml
    /// </summary>
    public partial class NewMainWindow
    {
        private VmUiControl _lookupUiControl;
        public NewMainWindow()
        {
            InitializeComponent();

            Loaded += (sender, args) =>
            {
                LocalViewModel.Initialize();
                var uControl = TabControl.ShowTableControl(RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext.Orders);

                LocalViewModel.OrderLookupDefinition.Destination = TabControl;
                _lookupUiControl = new VmUiControl(OrderLookup, LocalViewModel.LookupUiCommand);

                _lookupUiControl.Command.SetFocus();
                uControl.Loaded += (sender, args) =>
                {
                    _lookupUiControl.Command.SetFocus();
                };
            };
        }
    }
}
