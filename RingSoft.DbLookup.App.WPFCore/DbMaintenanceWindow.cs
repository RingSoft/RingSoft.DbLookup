using System;
using System.Collections.Generic;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.App.WPFCore
{
    public abstract class DbMaintenanceWindow : BaseWindow, IDbMaintenanceView
    {
        public abstract DbMaintenanceViewModelBase ViewModel { get; }

        public abstract DbMaintenanceButtonsControl MaintenanceButtonsControl { get; }

        public abstract DbMaintenanceStatusBar DbStatusBar { get; }

        public AutoFillControl KeyAutoFillControl { get; private set; }

        public IDbMaintenanceProcessor Processor { get; set; }

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
        public void Initialize()
        {
        }

        protected void RegisterFormKeyControl(AutoFillControl keyAutoFillControl)
        {
            Processor.RegisterFormKeyControl(keyAutoFillControl);
        }


        public virtual void OnValidationFail(FieldDefinition fieldDefinition, string text, string caption)
        {
            Processor.OnValidationFail(fieldDefinition, text, caption);
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
    }
}
