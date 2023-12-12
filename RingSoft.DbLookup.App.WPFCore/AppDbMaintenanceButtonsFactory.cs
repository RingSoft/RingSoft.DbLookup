using System.Windows.Controls;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.App.WPFCore
{
    public class AppDbMaintenanceButtonsFactory : DbMaintenanceButtonsFactory
    {
        //public override Control GetButtonsControl()
        //{
        //    return new DbMaintenanceButtonsControl();
        //}

        public override IAdvancedFindButtonsControl GetAdvancedFindButtonsControl(AdvancedFindViewModel viewModel)
        {
            var maintButtons = new DbMaintenanceButtonsControl();
            var additionalButtons = new AdvancedFindAdditionalButtonsControl(maintButtons);
            maintButtons.AdditionalButtonsPanel.Children.Add(additionalButtons);
            maintButtons.UpdateLayout();
            return additionalButtons;
        }

        public override Control GetRecordLockingButtonsControl(RecordLockingViewModel viewModel)
        {
            var result = new DbMaintenanceButtonsControl();
            result.UpdateLayout();
            return result;
        }
    }
}
