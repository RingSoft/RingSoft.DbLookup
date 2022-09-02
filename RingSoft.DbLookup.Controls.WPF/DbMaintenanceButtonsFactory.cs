using System.Windows.Controls;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.Controls.WPF
{
    public class DbMaintenanceButtonsFactory
    {
        public virtual Control GetButtonsControl()
        {
            return new Control();
        }

        public virtual Control GetAdvancedFindButtonsControl(AdvancedFindViewModel viewModel)
        {
            return new Control();
        }
    }
}
