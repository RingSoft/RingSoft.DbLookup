using System;
using RingSoft.DbLookup;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;

namespace RingSoft.DbMaintenance
{
    public class TestDbMaintenanceProcessor : IDbMaintenanceDataProcessor
    {
        public bool MessageBoxResult { get; set; }
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
            return MessageBoxResult;
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
    }
}
