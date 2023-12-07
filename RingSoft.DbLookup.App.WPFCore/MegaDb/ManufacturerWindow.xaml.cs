using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.App.WPFCore.MegaDb
{
    /// <summary>
    /// Interaction logic for ManufacturerWindow.xaml
    /// </summary>
    public partial class ManufacturerWindow
    {
        public override DbMaintenanceViewModelBase ViewModel => ManufacturerViewModel;
        public override DbMaintenanceButtonsControl MaintenanceButtonsControl => ButtonsControl;
        public override DbMaintenanceStatusBar DbStatusBar => StatusBar;

        public ManufacturerWindow()
        {
            InitializeComponent();

            RegisterFormKeyControl(NameControl);
        }
    }
}
