using RingSoft.DbLookup.App.Library;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbMaintenance;
using System.Windows.Controls.Primitives;

namespace RingSoft.DbLookup.App.WPFCore.MegaDb
{
    /// <summary>
    /// Interaction logic for LocationWindow.xaml
    /// </summary>
    public partial class LocationWindow
    {
        public override DbMaintenanceViewModelBase ViewModel => LocationViewModel;
        public override DbMaintenanceButtonsControl MaintenanceButtonsControl => ButtonsControl;
        public override DbMaintenanceStatusBar DbStatusBar => StatusBar;

        public LocationWindow()
        {
            InitializeComponent();
            RegisterFormKeyControl(NameControl);
        }
    }
}
