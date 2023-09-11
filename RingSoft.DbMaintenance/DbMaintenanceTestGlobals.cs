using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.Testing;

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
            SystemGlobals.UnitTestMode = true;

            var viewModel = (TViewModel)Activator.CreateInstance(typeof(TViewModel));
            ViewModel = viewModel;
            ViewModel.Processor = this;

            var view = (TView)Activator.CreateInstance(typeof(TView));
            View = view;

            ViewModel.CheckDirtyFlag = false;

            ViewModel.OnViewLoaded(View);
        }

        public virtual void ClearData()
        {

            ViewModel.NewCommand.Execute(null);
            DataRepository.ClearData();
        }

        public virtual void SetWindowCursor(WindowCursorTypes cursor)
        {

        }

        public virtual async Task ShowMessageBox(string text, string caption, RsMessageBoxIcons icon)
        {

        }

        public virtual async Task<MessageBoxButtonsResult> ShowYesNoMessageBox(string text, string caption, bool playSound = false)
        {
            return MessageBoxButtonsResult.Yes;
        }

        public virtual async Task<MessageBoxButtonsResult> ShowYesNoCancelMessageBox(string text, string caption, bool playSound = false)
        {
            return MessageBoxButtonsResult.Yes;
        }

        public bool KeyControlRegistered { get; set; }
        public event EventHandler<LookupAddViewArgs> LookupAddView;
        public virtual void InitializeFromLookupData(LookupAddViewArgs e)
        {
            
        }

        public virtual void OnValidationFail(FieldDefinition fieldDefinition, string text, string caption)
        {
            
        }

        public virtual void OnRecordSelected()
        {
            
        }

        public virtual void ShowFindLookupWindow(LookupDefinitionBase lookupDefinition, bool allowAdd, bool allowView, string initialSearchFor,
            PrimaryKeyValue initialSearchForPrimaryKey)
        {
            
        }

        public virtual void CloseWindow()
        {
            
        }

        public virtual MessageButtons ShowYesNoCancelMessage(string text, string caption, bool playSound = false)
        {
            return MessageButtons.Yes;
        }

        public virtual bool ShowYesNoMessage(string text, string caption, bool playSound = false)
        {
            return true;
        }

        public virtual void ShowRecordSavedMessage()
        {
            
        }

        public virtual void OnReadOnlyModeSet(bool readOnlyValue)
        {
            
        }

        public virtual bool IsMaintenanceKeyDown(MaintenanceKey key)
        {
            return false;
        }

        public virtual void Activate()
        {
            
        }

        public virtual void SetWindowReadOnlyMode()
        {
            
        }

        public virtual bool ShowRecordLockWindow(PrimaryKeyValue lockKey, string message, object inputParameter)
        {
            return true;
        }

        public virtual bool CheckDeleteTables(DeleteTables deleteTables)
        {
            return true;
        }

        public virtual void PrintOutput(PrinterSetupArgs printerSetupArgs)
        {
            
        }

        public virtual void SetSaveStatus(string message, AlertLevels alertLevel)
        {
            
        }

        public virtual List<DbAutoFillMap> GetAutoFills()
        {
            return null;
        }

        public virtual void HandleAutoFillValFail(DbAutoFillMap autoFillMap)
        {
            
        }
    }
}
