using RingSoft.DbLookup.Controls.WPF;
using System.Windows.Controls;

namespace RingSoft.DbLookup.App.WPFCore
{
    /// <summary>
    /// Interaction logic for AdvancedFindAdditionalButtonsControl.xaml
    /// </summary>
    public partial class AdvancedFindAdditionalButtonsControl : UserControl, IAdvancedFindButtonsControl
    {
        public Control DbMaintenanceButtonsControl => _maintButtonsControl;
        public Button ImportDefaultLookupButton => ImportDefaultLookupButton1;
        public Button RefreshSettingsButton => RefreshSettingsButton1;
        public Button PrintLookupOutputButton => PrintLookupOutputButton1;

        private DbMaintenanceButtonsControl _maintButtonsControl;
        public AdvancedFindAdditionalButtonsControl(DbMaintenanceButtonsControl maintButtonsControl)
        {
            InitializeComponent();
            _maintButtonsControl = maintButtonsControl;
        }
    }
}
