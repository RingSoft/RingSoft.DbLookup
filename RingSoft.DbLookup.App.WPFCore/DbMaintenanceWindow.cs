using System;
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

        public AutoFillControl KeyAutoFillControl { get; private set; }

        public IDbMaintenanceProcessor Processor { get; set; }

        public event EventHandler<LookupSelectArgs> LookupFormReturn;

        public DbMaintenanceWindow()
        {
            EnterToTab = true;
        }
        public void Initialize()
        {
            Processor = LookupControlsGlobals.DbMaintenanceProcessorFactory.GetProcessor();
            Processor.Initialize(this, MaintenanceButtonsControl, ViewModel, this);

       }

        protected void RegisterFormKeyControl(AutoFillControl keyAutoFillControl)
        {
            Processor.RegisterFormKeyControl(keyAutoFillControl);
        }


        public void InitializeFromLookupData(LookupAddViewArgs e)
        {
            Processor.InitializeFromLookupData(e);
        }

        public virtual void OnValidationFail(FieldDefinition fieldDefinition, string text, string caption)
        {
            Processor.OnValidationFail(fieldDefinition, text, caption);
        }

        public virtual void ResetViewForNewRecord()
        {
        }

        public void OnRecordSelected()
        {
            Processor.OnRecordSelected();
        }

        public void ShowFindLookupWindow(LookupDefinitionBase lookupDefinition, bool allowAdd, bool allowView,
            string initialSearchFor, PrimaryKeyValue initialSearchForPrimaryKey)
        {
            Processor.ShowFindLookupWindow(lookupDefinition, allowAdd, allowView, initialSearchFor,
                initialSearchForPrimaryKey);
        }

        public void CloseWindow()
        {
            Processor.CloseWindow();
        }

        public MessageButtons ShowYesNoCancelMessage(string text, string caption, bool playSound = false)
        {
            return Processor.ShowYesNoCancelMessage(text, caption, playSound);
        }

        public bool ShowYesNoMessage(string text, string caption, bool playSound = false)
        {
            return Processor.ShowYesNoMessage(text, caption, playSound);
        }

        public void ShowRecordSavedMessage()
        {
            Processor.ShowRecordSavedMessage();
        }

        protected override void OnReadOnlyModeSet(bool readOnlyValue)
        {
            Processor.OnReadOnlyModeSet(readOnlyValue);
        }

        public override void SetControlReadOnlyMode(Control control, bool readOnlyValue)
        {
            if (Processor.SetControlReadOnlyMode(control, readOnlyValue))
                base.SetControlReadOnlyMode(control, readOnlyValue);
        }
    }
}
