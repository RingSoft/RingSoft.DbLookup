using System.Windows.Controls;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.App.WPFCore.MegaDb
{
    /// <summary>
    /// Interaction logic for LocationWindow.xaml
    /// </summary>
    public partial class LocationWindow
    {
        public override DbMaintenanceViewModelBase ViewModel => LocationViewModel;
        public override Control MaintenanceButtonsControl => ButtonsControl;
        public override DbMaintenanceStatusBar DbStatusBar => StatusBar;

        public LocationWindow()
        {
            InitializeComponent();
            RegisterFormKeyControl(NameControl);
        }
    }
}
