using System.Windows.Controls;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.Controls.WPF
{
    public abstract class DbMaintenanceButtonsFactory
    {
        public abstract Control GetButtonsControl();

        public abstract Control GetAdvancedFindButtonsControl(AdvancedFindViewModel viewModel);

        public abstract Control GetRecordLockingButtonsControl(RecordLockingViewModel viewModel);
    }
}
