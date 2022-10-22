using System;
using System.Collections.Generic;
using System.Text;
using RingSoft.DbLookup;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;

namespace RingSoft.DbMaintenance
{
    public enum MaintenanceKey
    {
        Alt = 0,
        Ctrl = 1,
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
    }
}
