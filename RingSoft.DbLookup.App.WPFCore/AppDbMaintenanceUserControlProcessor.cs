using System.Windows.Controls;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.App.WPFCore
{
    public class AppDbMaintenanceUserControlProcessor : DbMaintenanceUserControlProcessor
    {
        public override Button FindButton => _buttonsControl.FindButton;

        private DbMaintenanceButtonsControl _buttonsControl;

        public AppDbMaintenanceUserControlProcessor(
            DbMaintenanceViewModelBase viewModel
            , Control buttonsControl
            , DbMaintenanceUserControl userControl
            , DbMaintenanceStatusBar statusBar) 
            : base(viewModel, buttonsControl, userControl, statusBar)
        {
            if (buttonsControl is DbMaintenanceButtonsControl dbMaintenanceButtonsControl)
            {
                _buttonsControl = dbMaintenanceButtonsControl;
            }
        }
    }
}
