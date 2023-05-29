using System;
using System.Collections.Generic;
using System.Text;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;

namespace RingSoft.DbMaintenance
{
    public enum MaintenanceKey
    {
        Alt = 0,
        Ctrl = 1,
    }

    public class DbAutoFillMap
    {
        public AutoFillSetup AutoFillSetup { get; }

        public AutoFillValue AutoFillValue { get; }

        public DbAutoFillMap(AutoFillSetup autoFillSetup, AutoFillValue autoFillValue)
        {
            AutoFillSetup = autoFillSetup;
            AutoFillValue = autoFillValue;
        }

        public override string ToString()
        {
            if (AutoFillSetup.ForeignField != null)
            {
                return AutoFillSetup.ForeignField.Description;
            }

            return base.ToString();
        }
    }


    public interface IDbMaintenanceDataProcessor
    {
        bool KeyControlRegistered { get; set; }

        event EventHandler<LookupAddViewArgs> LookupAddView;

        void InitializeFromLookupData(LookupAddViewArgs e);

        void OnValidationFail(FieldDefinition fieldDefinition, string text, string caption);

        void OnRecordSelected();

        void ShowFindLookupWindow(LookupDefinitionBase lookupDefinition, bool allowAdd, bool allowView,
            string initialSearchFor, PrimaryKeyValue initialSearchForPrimaryKey);

        void CloseWindow();

        MessageButtons ShowYesNoCancelMessage(string text, string caption, bool playSound = false);

        bool ShowYesNoMessage(string text, string caption, bool playSound = false);

        void ShowRecordSavedMessage();

        void OnReadOnlyModeSet(bool readOnlyValue);

        bool IsMaintenanceKeyDown(MaintenanceKey key);
        void Activate();

        void SetWindowReadOnlyMode();

        bool ShowRecordLockWindow(PrimaryKeyValue lockKey, string message, object inputParameter);

        bool CheckDeleteTables(DeleteTables deleteTables);

        void PrintOutput(PrinterSetupArgs printerSetupArgs);

        void SetSaveStatus(string message, AlertLevels alertLevel);

        List<DbAutoFillMap> GetAutoFills();

        void HandleAutoFillValFail(DbAutoFillMap autoFillMap);
    }
}
