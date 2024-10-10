using System.Windows.Controls;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.Controls.WPF
{
    public abstract class DbMaintenanceUserControl : BaseUserControl, IDbMaintenanceView
    {
        public DbMaintenanceViewModelBase ViewModel { get; private set; }

        public Control MaintenanceButtons { get; private set; }

        public DbMaintenanceStatusBar StatusBar { get; private set; }

        public DbMaintenanceUserControlProcessor Processor { get; private set; }

        public IUserControlHost Host { get; internal set; }

        private bool _loaded;

        public DbMaintenanceUserControl()
        {
            Loaded += (sender, args) =>
            {
                if (_loaded)
                {
                    return;
                }
                ViewModel = OnGetViewModel();
                MaintenanceButtons = OnGetMaintenanceButtons();
                StatusBar = OnGetStatusBar();
                Processor = LookupControlsGlobals.DbMaintenanceProcessorFactory.GetUserControlProcessor(
                    ViewModel
                    , MaintenanceButtons
                    , this
                    , StatusBar
                    , Host);
                ViewModel.Processor = Processor;
                Processor.Initialize();
                ViewModel.OnViewLoaded(this);
                _loaded = true;
            };
        }

        protected abstract DbMaintenanceViewModelBase OnGetViewModel();

        protected abstract Control OnGetMaintenanceButtons();

        protected abstract DbMaintenanceStatusBar OnGetStatusBar();
        public virtual void ResetViewForNewRecord()
        {
            
        }

        protected override void OnReadOnlyModeSet(bool readOnlyValue)
        {
            Processor.OnReadOnlyModeSet(readOnlyValue);
            base.OnReadOnlyModeSet(readOnlyValue);
        }

        public override void SetControlReadOnlyMode(Control control, bool readOnlyValue)
        {
            if (Processor.SetControlReadOnlyMode(control, readOnlyValue))
                base.SetControlReadOnlyMode(control, readOnlyValue);
        }
    }
}
