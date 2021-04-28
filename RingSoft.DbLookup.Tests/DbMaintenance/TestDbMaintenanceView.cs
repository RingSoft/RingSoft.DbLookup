using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbMaintenance;
using System;

namespace RingSoft.DbLookup.Tests.DbMaintenance
{
    public class TestDbMaintenanceView : IDbMaintenanceView
    {
        public event EventHandler<LookupSelectArgs> LookupFormReturn;
        public void OnValidationFail(FieldDefinition fieldDefinition, string text, string caption)
        {
            Console.WriteLine(text);
        }

        public void ResetViewForNewRecord()
        {
            Console.WriteLine($"{nameof(ResetViewForNewRecord)} method running");
        }

        public void OnRecordSelected()
        {
            Console.WriteLine($"{nameof(OnRecordSelected)} method running");
        }

        public void ShowFindLookupWindow(LookupDefinitionBase lookupDefinition, bool allowAdd, bool allowView, string initialSearchFor,
            PrimaryKeyValue initialSearchForPrimaryKey)
        {
            Console.WriteLine($"{nameof(ShowFindLookupWindow)} method running");
        }

        public void CloseWindow()
        {
            Console.WriteLine($"{nameof(CloseWindow)} method running");
        }

        public MessageButtons ShowYesNoCancelMessage(string text, string caption, bool playSound = false)
        {
            throw new NotImplementedException();
        }

        public bool ShowYesNoMessage(string text, string caption, bool playSound = false)
        {
            Console.WriteLine(text);
            return true;
        }

        public void ShowRecordSavedMessage()
        {
            Console.WriteLine($"{nameof(ShowRecordSavedMessage)} method running");
        }

        public void SetReadOnlyMode(bool readOnlyValue)
        {
            Console.WriteLine($"{nameof(SetReadOnlyMode)} method running");
        }
    }
}
