using System.Windows.Controls;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.App.WPFCore
{
    public class AppDbMaintenanceProcessorFactory : RingSoft.DbLookup.Controls.WPF.DbMaintenanceProcessorFactory
    {
        public override DbMaintenanceWindowProcessor GetProcessor()
        {
            return new AppDbMaintenanceWindowProcessor();
        }

        public override DbMaintenanceUserControlProcessor GetUserControlProcessor(DbMaintenanceViewModelBase viewModel, Control buttonsControl,
            DbMaintenanceUserControl userControl, DbMaintenanceStatusBar statusBar, IUserControlHost host)
        {
            return new AppDbMaintenanceUserControlProcessor(viewModel, buttonsControl, userControl, statusBar, host);
        }
    }
}
