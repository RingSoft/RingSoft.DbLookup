using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using System;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;

namespace RingSoft.DbMaintenance
{
    public class DbMaintenanceTestGlobals<TViewModel, TView> 
        : IControlsUserInterface
    , IDbMaintenanceDataProcessor

        where TViewModel : DbMaintenanceViewModelBase
        where TView : IDbMaintenanceView, new()
    {
        public TViewModel ViewModel { get; private set; }

        public TView View { get; private set; }


        public TestDataRepository DataRepository { get; }
        public DbMaintenanceTestGlobals(TestDataRepository dataDepository)
        {
            DataRepository = dataDepository;
        }

        public virtual void Initialize()
        {
            ControlsGlobals.UserInterface = this;

            var viewModel = (TViewModel)Activator.CreateInstance(typeof(TViewModel));
            ViewModel = viewModel;
            ViewModel.Processor = this;

            var view = (TView)Activator.CreateInstance(typeof(TView));
            View = view;

            ViewModel.OnViewLoaded(View);
        }

        public virtual void ClearData()
        {

            ViewModel.NewCommand.Execute(null);
            DataRepository.ClearData();
        }

        public void SetWindowCursor(WindowCursorTypes cursor)
        {

        }

        public void ShowMessageBox(string text, string caption, RsMessageBoxIcons icon)
        {

        }

        public MessageBoxButtonsResult ShowYesNoMessageBox(string text, string caption, bool playSound = false)
        {
            return MessageBoxButtonsResult.Yes;
        }

        public MessageBoxButtonsResult ShowYesNoCancelMessageBox(string text, string caption, bool playSound = false)
        {
            return MessageBoxButtonsResult.Yes;
        }

        public bool KeyControlRegistered { get; set; }
        public event EventHandler<LookupAddViewArgs> LookupAddView;
        public void InitializeFromLookupData(LookupAddViewArgs e)
        {
            
        }

        public void OnValidationFail(FieldDefinition fieldDefinition, string text, string caption)
        {
            
        }

        public void OnRecordSelected()
        {
            
        }

        public void ShowFindLookupWindow(LookupDefinitionBase lookupDefinition, bool allowAdd, bool allowView, string initialSearchFor,
            PrimaryKeyValue initialSearchForPrimaryKey)
        {
            
        }

        public void CloseWindow()
        {
            
        }

        public MessageButtons ShowYesNoCancelMessage(string text, string caption, bool playSound = false)
        {
            return MessageButtons.Yes;
        }

        public bool ShowYesNoMessage(string text, string caption, bool playSound = false)
        {
            return true;
        }

        public void ShowRecordSavedMessage()
        {
            
        }

        public void OnReadOnlyModeSet(bool readOnlyValue)
        {
            
        }

        public bool IsMaintenanceKeyDown(MaintenanceKey key)
        {
            return false;
        }

        public void Activate()
        {
            
        }

        public void SetWindowReadOnlyMode()
        {
            
        }

        public bool ShowRecordLockWindow(PrimaryKeyValue lockKey, string message, object inputParameter)
        {
            return true;
        }

        public bool CheckDeleteTables(DeleteTables deleteTables)
        {
            return true;
        }

        public void PrintOutput(PrinterSetupArgs printerSetupArgs)
        {
            
        }
    }
}
