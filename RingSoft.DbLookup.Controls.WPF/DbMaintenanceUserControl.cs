using System.Windows.Controls;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.Controls.WPF
{
    public abstract class DbMaintenanceUserControl : BaseUserControl
    {
        public DbMaintenanceViewModelBase ViewModel { get; }

        public Control MaintenanceButtons { get; }

        public DbMaintenanceStatusBar StatusBar { get; }

        public DbMaintenanceUserControlProcessor Processor { get; }

        public DbMaintenanceUserControl()
        {
            ViewModel = OnGetViewModel();
            MaintenanceButtons = OnGetMaintenanceButtons();
            StatusBar = OnGetStatusBar();
            Processor = LookupControlsGlobals.DbMaintenanceProcessorFactory.GetUserControlProcessor(
                ViewModel
                , MaintenanceButtons);
        }

        protected abstract DbMaintenanceViewModelBase OnGetViewModel();

        protected abstract Control OnGetMaintenanceButtons();

        protected abstract DbMaintenanceStatusBar OnGetStatusBar();
    }
}
