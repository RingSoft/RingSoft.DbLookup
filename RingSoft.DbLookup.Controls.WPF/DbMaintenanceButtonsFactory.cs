using System.Windows.Controls;

namespace RingSoft.DbLookup.Controls.WPF
{
    public class DbMaintenanceButtonsFactory
    {
        public virtual Control GetButtonsControl()
        {
            return new Control();
        }
    }
}
