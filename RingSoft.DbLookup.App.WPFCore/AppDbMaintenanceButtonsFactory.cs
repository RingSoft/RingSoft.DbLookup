using System.Windows.Controls;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.App.WPFCore
{
    public class AppDbMaintenanceButtonsFactory : DbMaintenanceButtonsFactory
    {
        public override Control GetButtonsControl()
        {
            return new DbMaintenanceButtonsControl();
        }

        public override Control GetAdvancedFindButtonsControl(AdvancedFindViewModel viewModel)
        {
            var result = new DbMaintenanceButtonsControl();
            var additionalButtons = new AdvancedFindAdditionalButtonsControl();
            result.AdditionalButtonsPanel.Children.Add(additionalButtons);
            additionalButtons.ImportDefaultLookupButton.Command = viewModel.ImportDefaultLookupCommand;
            additionalButtons.RefreshSettingsButton.Command = viewModel.RefreshSettingsCommand;
            additionalButtons.PrintLookupOutputButton.Command = viewModel.PrintLookupOutputCommand;
            result.UpdateLayout();
            return result;
        }

        public override Control GetRecordLockingButtonsControl(RecordLockingViewModel viewModel)
        {
            var result = new DbMaintenanceButtonsControl();
            result.UpdateLayout();
            return result;
        }
    }
}
