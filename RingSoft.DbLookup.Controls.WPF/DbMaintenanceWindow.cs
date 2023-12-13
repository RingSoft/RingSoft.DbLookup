using System.Windows.Controls;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.Controls.WPF
{
    public abstract class DbMaintenanceWindow : BaseWindow, IDbMaintenanceView
    {
        public abstract DbMaintenanceViewModelBase ViewModel { get; }

        public abstract Control MaintenanceButtonsControl { get; }

        public abstract DbMaintenanceStatusBar DbStatusBar { get; }

        public AutoFillControl KeyAutoFillControl { get; private set; }

        public IDbMaintenanceProcessor Processor { get; }

        public DbMaintenanceWindow()
        {
            Processor = LookupControlsGlobals.DbMaintenanceProcessorFactory.GetProcessor();
            Loaded += (sender, args) =>
            {
                Processor.Initialize(this, MaintenanceButtonsControl, ViewModel, this, DbStatusBar);
                Closing += (sender, args) => ViewModel.OnWindowClosing(args);
                ViewModel.OnViewLoaded(this);
            };
        }

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

        protected void RegisterFormKeyControl(AutoFillControl keyAutoFillControl)
        {
            Processor.RegisterFormKeyControl(keyAutoFillControl);
        }
    }
}
