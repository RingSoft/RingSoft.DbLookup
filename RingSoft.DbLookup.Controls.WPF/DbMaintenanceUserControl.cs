using System.Windows.Controls;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.Controls.WPF
{
    public abstract class DbMaintenanceUserControl : BaseUserControl, IDbMaintenanceView
    {
        public DbMaintenanceViewModelBase ViewModel { get; private set; }

        public Control MaintenanceButtons { get; private set; }

        public DbMaintenanceStatusBar StatusBar { get; private set; }

        public DbMaintenanceUserControlProcessor Processor { get; private set; }

        public LookupAddViewArgs LookupAddViewArgs { get; set; }

        public object AddViewParameter { get; set; }

        public IUserControlHost Host { get; internal set; }

        public AutoFillControl _keyControl;

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
                if (_keyControl != null)
                {
                    RegisterFormKeyControl(_keyControl);
                    _keyControl = null;
                }
                ViewModel.Processor = Processor;
                if (LookupAddViewArgs != null) Processor.InitializeFromLookupData(LookupAddViewArgs);
                ViewModel.InputParameter = AddViewParameter;
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

        protected void RegisterFormKeyControl(AutoFillControl keyAutoFillControl)
        {
            if (Processor == null)
            {
                _keyControl = keyAutoFillControl;
                return;
            }
            Processor.RegisterFormKeyControl(keyAutoFillControl);
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
