using System;
using System.Collections.Generic;
using System.Windows.Controls;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.Controls.WPF
{
    public class DbMaintenanceUserControlProcessor : IDbMaintenanceDataProcessor
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

        public bool DeleteChildrenResult { get; set; }
        public bool PreDeleteResult { get; set; }
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
            throw new NotImplementedException();
        }

        public bool ShowYesNoMessage(string text, string caption, bool playSound = false)
        {
            throw new NotImplementedException();
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

        public void SetSaveStatus(string message, AlertLevels alertLevel)
        {
            
        }

        public List<DbAutoFillMap> GetAutoFills()
        {
            throw new NotImplementedException();
        }

        public void HandleAutoFillValFail(DbAutoFillMap autoFillMap)
        {
            
        }

        public ITwoTierProcessingProcedure GetDeleteProcedure(DeleteTables deleteTables)
        {
            throw new NotImplementedException();
        }

        public void GetPreDeleteProcedure(List<FieldDefinition> fields, DeleteTables deleteTables)
        {
            
        }

        public void SetWindowReadOnlyMode(bool readOnlyMode)
        {
            
        }
    }
}
