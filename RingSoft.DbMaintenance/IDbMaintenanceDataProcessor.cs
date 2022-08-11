using System;
using System.Collections.Generic;
using System.Text;
using RingSoft.DbLookup;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;

namespace RingSoft.DbMaintenance
{
    public interface IDbMaintenanceDataProcessor
    {
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
    }
}
