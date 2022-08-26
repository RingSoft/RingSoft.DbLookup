using System.Windows.Controls;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.Controls.WPF
{
    public interface IDbMaintenanceProcessor : IDbMaintenanceDataProcessor
    {
        void Initialize(BaseWindow window, Control buttonsControl,
            DbMaintenanceViewModelBase viewModel, IDbMaintenanceView view);

        void RegisterFormKeyControl(AutoFillControl keyAutoFillControl);

        bool SetControlReadOnlyMode(Control control, bool readOnlyValue);

        void CheckAddOnFlyMode();
    }
}
