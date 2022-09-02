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
            result.UpdateLayout();
            return result;
        }
    }
}
