using System.Windows.Controls;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.Controls.WPF
{
    public interface IDbMaintenanceProcessor
    {
        void Initialize(BaseWindow window, Control buttonsControl,
            DbMaintenanceViewModelBase viewModel, IDbMaintenanceView view);

        void RegisterFormKeyControl(AutoFillControl keyAutoFillControl);

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

        bool SetControlReadOnlyMode(Control control, bool readOnlyValue);
    }
}
