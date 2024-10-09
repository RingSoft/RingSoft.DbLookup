using System.Windows.Controls;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.Controls.WPF
{
    public class DbMaintenanceUserControlProcessor
    {
        public DbMaintenanceViewModelBase ViewModel { get; }

        public Control ButtonsControl { get; }
        public DbMaintenanceUserControlProcessor(
            DbMaintenanceViewModelBase viewModel
            , Control buttonsControl)
        {
            ViewModel = viewModel;
            ButtonsControl = buttonsControl;
        }
    }
}
